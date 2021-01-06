using EduWebService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Core;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core.NuceIdentity;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static EduWebService.ServiceSoapClient;

namespace nuce.web.api.Services.Core.Implements
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private readonly IStudentRepository _studentRepository;
        private readonly NuceCoreIdentityContext _identityContext;

        public UserService(UserManager<ApplicationUser> _userManager,
                IConfiguration configuration, 
                IHttpContextAccessor httpContextAccessor,
                IStudentRepository _studentRepository,
                NuceCoreIdentityContext nuceCoreIdentityContext
        )
        {
            this._userManager = _userManager;
            this._studentRepository = _studentRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _configuration = configuration;
            _identityContext = nuceCoreIdentityContext;
        }
        public async Task<List<Claim>> AddClaimsAsync(LoginModel model, ApplicationUser user)
        {
            string username = model.IsStudent ? model.Username : user.UserName;

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            if (model.IsStudent)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, RoleNames.Student));
                authClaims.Add(new Claim(UserParameters.MSSV, username));
                authClaims.Add(new Claim(ClaimTypes.GivenName,  _studentRepository.FindByCode(username)?.FulName));
            }
            else
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
            }
            return authClaims;
        }
        public JwtSecurityToken CreateJWTAccessToken(List<Claim> claims)
        {
            return CreateJWTToken(claims, DateTime.Now.AddDays(1));
        }
        public JwtSecurityToken CreateJWTRefreshToken(List<Claim> claims)
        {
            return CreateJWTToken(claims, DateTime.Now.AddDays(999));
        }
        private JwtSecurityToken CreateJWTToken(List<Claim> claims, DateTime expires)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: expires,
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
        public async Task<ApplicationUser> FindByName(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<bool> UserLogin(LoginModel model)
        {
            if (model.IsStudent)
            {
                ServiceSoapClient srvc = new ServiceSoapClient(EndpointConfiguration.ServiceSoap12);
                try
                {
                    return await srvc.authenAsync(model.Username, model.Password) == 1;
                }
                catch (Exception)
                {
                    throw new CallEduWebServiceException("Hiện tại không thể kết nối đến Đào tạo");
                }
            }
            else
            {
                var user = await FindByName(model.Username);
                if (user == null)
                {
                    throw new RecordNotFoundException("Tài khoản không tồn tại");
                }
                else if (user.Status != (int)UserStatus.Active)
                {
                    throw new InvalidInputDataException("Tài khoản không được kích hoạt");
                }
                else
                {
                    return await _userManager.CheckPasswordAsync(user, model.Password);
                }
            }
        }
        public string GetCurrentStudentCode()
        {
            return GetClaimByKey(UserParameters.MSSV);
        }
        public string GetClaimByKey(string key)
        {
            var identity = _httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                return identity.FindFirst(key)?.Value;
            }
            return null;
        }
        public List<string> GetClaimListByKey(string key)
        {
            var identity = _httpContext.User.Identity as ClaimsIdentity;
            List<string> result = new List<string>();
            if (identity != null)
            {
                var claimList = identity.FindAll(key);
                foreach (var claim in claimList)
                {
                    result.Add(claim.Value);
                }
            }
            return result;
        }
        public long? GetCurrentStudentID()
        {
            string studentCode = GetCurrentStudentCode();
            var student = _studentRepository.FindByCode(studentCode);
            if (student != null)
            {
                return student.Id;
            }
            return null;
        }
        public AsAcademyStudent GetCurrentStudent()
        {
            string studentCode = GetCurrentStudentCode();
            return _studentRepository.FindByCode(studentCode);
        }
        public string GetUserName()
        {
            return GetClaimByKey(ClaimTypes.Name);
        }
        public AsAcademyStudent GetStudentByEmail(string email)
        {
            return _studentRepository.FindByEmailNhaTruong(email);
        }

        public async Task<PaginationModel<UserModel>> GetAllAsync(UserFilter filter, int skip = 0, int pageSize = 20)
        {
            _identityContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var currentUsernameWorking = GetUserName();
            var query = _identityContext.Users.Where(u => u.Status != (int)UserStatus.Deleted && u.UserName != currentUsernameWorking);
            var recordsTotal = query.Count();

            if (!string.IsNullOrWhiteSpace(filter.Username))
            {
                query = query.Where(u => u.UserName.Contains(filter.Username));
            }
            var recordsFiltered = query.Count();

            var querySkip = query
                .OrderBy(u => u.UserName)
                .Skip(skip).Take(pageSize);

            var data = await querySkip
                .Select(u => new UserModel {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Status = u.Status
                })
                .ToListAsync();

            return new PaginationModel<UserModel>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }
        public async Task<UserDetailModel> GetByIdAsync(string id)
        {
            _identityContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status != (int)UserStatus.Deleted);
            if(user == null)
            {
                throw new RecordNotFoundException();
            }
            var roles = await _identityContext.UserRoles.Where(ur => ur.UserId == id)
                .Join(_identityContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                .ToListAsync();
            return new UserDetailModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Status = user.Status,
                Roles = roles
            };
        }
        public async Task DeactiveUserAsync(string id)
        {
            var user =  await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status != (int)UserStatus.Deleted && u.UserName != GetUserName());
            if (user == null)
            {
                throw new RecordNotFoundException();
            }
            user.Status = (int)UserStatus.Deactive;
            await _identityContext.SaveChangesAsync();
        }
        public async Task ActiveUserAsync(string id)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status != (int)UserStatus.Deleted && u.UserName != GetUserName());
            if (user == null)
            {
                throw new RecordNotFoundException();
            }
            user.Status = (int)UserStatus.Active;
            await _identityContext.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(string id)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status != (int)UserStatus.Deleted && u.UserName != GetUserName());
            if (user == null)
            {
                throw new RecordNotFoundException();
            }
            user.Status = (int)UserStatus.Deleted;
            await _identityContext.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(string id, UserUpdateModel user)
        {
            var userUpdate = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == id && u.UserName != GetUserName());
            if (userUpdate == null)
            {
                throw new RecordNotFoundException();
            }
            //userUpdate.UserName = user.UserName.Trim();
            userUpdate.Email = user.Email.Trim();
            userUpdate.PhoneNumber = user.PhoneNumber.Trim();
            //userUpdate.Status = (int)user.Status;
            var oldRoles = (await _userManager.GetRolesAsync(userUpdate)).ToList();
            var newRoles = user.Roles;

            //cần làm trước
            //xoá những roles cũ có trong roles mới
            oldRoles.RemoveAll(r => newRoles.Contains(r)); //là những roles cần bỏ

            //xoá những roles mới có trong roles cũ
            newRoles.RemoveAll(r => oldRoles.Contains(r)); //là những roles cần thêm

            using (var transaction = _identityContext.Database.BeginTransaction())
            {
                try
                {
                    await _identityContext.SaveChangesAsync();
                    foreach(var role in newRoles)
                    {
                        await _userManager.AddToRoleAsync(userUpdate, role);
                    }

                    foreach (var role in oldRoles)
                    {
                        await _userManager.RemoveFromRoleAsync(userUpdate, role);
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
        public async Task ResetPasswordAsync(string id, string newPassword)
        {
            var userReset = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == id && u.UserName != GetUserName());
            if (userReset == null)
            {
                throw new RecordNotFoundException();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(userReset);
            await _userManager.ResetPasswordAsync(userReset, token, newPassword.Trim());
        }
    }
}
