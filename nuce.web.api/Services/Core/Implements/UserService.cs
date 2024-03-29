﻿using EduWebService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Core;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Repositories.EduData.Implements;
using nuce.web.api.Repositories.EduData.Interfaces;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.EduData.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core.NuceIdentity;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
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
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IDepartmentRepository   _departmentRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IStudentEduDataService _studentEduDataService;
        private readonly ILogger<UserService> _logger;

        public UserService(
                ILogger<UserService> logger,
                UserManager<ApplicationUser> _userManager,
                IConfiguration configuration, 
                IHttpContextAccessor httpContextAccessor,
                IStudentRepository _studentRepository,
                NuceCoreIdentityContext nuceCoreIdentityContext,
                ILecturerRepository _lecturerRepository,
                IDepartmentRepository _departmentRepository,
                IFacultyRepository _facultyRepository,
                IStudentEduDataService _studentEduDataService
        )
        {
            _logger = logger;
            this._userManager = _userManager;
            this._studentRepository = _studentRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _configuration = configuration;
            _identityContext = nuceCoreIdentityContext;
            this._lecturerRepository = _lecturerRepository;
            this._departmentRepository = _departmentRepository;
            this._facultyRepository = _facultyRepository;
            this._studentEduDataService = _studentEduDataService;
        }

        public async Task<List<Claim>> AddClaimsAsync(LoginModel model, ApplicationUser user)
        {
            string username = UserParameters.LoginViaDaotao.Contains(model.LoginUserType) ? model.Username : user?.UserName ?? model.Username;

            var authClaims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(UserParameters.UserType, model.LoginUserType.ToString())
            };

            if (model.LoginUserType == LoginUserType.Student)
            {
                // Role: SV
                // MÃ: SV
                // Tên: SV
                authClaims.Add(new Claim(ClaimTypes.Role, RoleNames.Student));
                string name = _studentRepository.FindByCode(username)?.FulName;
                if (name == null)
                {
                    name = _studentEduDataService.FindByCode(username)?.FullName;
                }
                authClaims.Add(new Claim(ClaimTypes.GivenName, name ?? ""));
                authClaims.Add(new Claim(UserParameters.UserCode, username));
            }
            else if (model.LoginUserType == LoginUserType.Lecturer)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, RoleNames.KhaoThi_Survey_GiangVien));
                string givenName = (await _lecturerRepository.FindByCode(username))?.FullName ?? user.UserName;
                authClaims.Add(new Claim(ClaimTypes.GivenName, givenName));
                authClaims.Add(new Claim(UserParameters.UserCode, username));
            }
            else
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
            }
            #region gán tên cho tài khoản của khoa & bộ môn
            if (model.LoginUserType == LoginUserType.Faculty)
            {
                if (user.ExCode == null)
                {
                    throw new ArgumentException("ExCode đăng nhập khoa = null");
                }
                var faculty = await _facultyRepository.FindByCode(user.ExCode);
                string givenName = faculty?.Name ?? user.UserName;
                if (faculty == null)
                {
                    _logger.LogError($"Khong tim thay khoa {username} khi add Claim tai '_facultyRepository.FindByCode(user.ExCode)' voi ExCode = {user.ExCode}");
                }
                authClaims.Add(new Claim(ClaimTypes.GivenName, givenName));
                authClaims.Add(new Claim(UserParameters.UserCode, user.ExCode));
            }
            else if (model.LoginUserType == LoginUserType.Department)
            {
                if (user.ExCode == null)
                {
                    throw new ArgumentException("ExCode đăng nhập bộ môn = null");
                }
                var department = await _departmentRepository.FindByCode(user.ExCode);
                string givenName = department?.Name ?? user.UserName;
                if (department == null)
                {
                    _logger.LogError($"Khong tim thay bo mon {username} khi add Claim tai '_departmentRepository.FindByCode(user.ExCode)' voi ExCode = {user.ExCode}");
                }
                authClaims.Add(new Claim(ClaimTypes.GivenName, givenName));
                authClaims.Add(new Claim(UserParameters.UserCode, user.ExCode));
            }
            #endregion
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
            try
            {
                var listStuIgnore = _configuration.GetSection("ListStudentIgnore").AsEnumerable()
                    .Where(item => item.Value != null)
                    .Select(item => item.Value).ToList();
                if (listStuIgnore.Contains(model.Username))
                {
                    return true;
                }
            }
            catch
            {
            }
            if (UserParameters.LoginViaDaotao.Contains(model.LoginUserType))
            {
                bool isSuccess = false;
                bool checkCallServiceSoap = false;
                ServiceSoapClient srvc = new ServiceSoapClient(EndpointConfiguration.ServiceSoap12); //lấy trên đào tạo
                try
                {
                    isSuccess = await srvc.authenAsync(model.Username, model.Password) == 1;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "khong goi duoc service soap");
                    checkCallServiceSoap = true;
                }

                if (!isSuccess) //lấy trong local
                {
                    try
                    {
                        HttpClient clientAuth = new HttpClient()
                        {
                            BaseAddress = new Uri(_configuration["ApiAuth"]),
                            Timeout = TimeSpan.FromSeconds(60)
                        };
                        var json = JsonSerializer.Serialize(new
                        {
                            MaND = model.Username,
                            Pass = model.Password
                        });
                        var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                        var res = await clientAuth.PostAsync("/api/Auth", content);
                        isSuccess =  res.IsSuccessStatusCode;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "khong goi duoc api xac thuc local");
                        throw new CallEduWebServiceException("Hiện tại không thể kết nối đến Đào tạo");
                    }
                }

                if (checkCallServiceSoap && !isSuccess) //không gọi được đào tạo và local cũng k có
                {
                    throw new CallEduWebServiceException("Hiện tại không thể kết nối đến Đào tạo");
                }
                return isSuccess;
            }
            else
            {
                var user = await FindByName(model.Username);
                if (user == null)
                {
                    throw new RecordNotFoundException("Tài khoản không tồn tại");
                }
                else if (user.Status == (int)UserStatus.Deactive)
                {
                    throw new InvalidInputDataException("Tài khoản không được kích hoạt");
                }
                else if (user.Status == (int)UserStatus.Deleted)
                {
                    throw new InvalidInputDataException("Tài khoản không tồn tại");
                }
                else
                {
                    return await _userManager.CheckPasswordAsync(user, model.Password);
                }
            }
        }
        public string GetCurrentStudentCode()
        {
            return GetClaimByKey(UserParameters.UserCode);
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
            userUpdate.Email = user.Email?.Trim();
            userUpdate.PhoneNumber = user.PhoneNumber?.Trim();
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
        public async Task<ApplicationUser> CreateUser(UserCreateModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                throw new SystemException("Tên tài khoản đã tồn tại");
            }

            //tai khoan
            var user = new ApplicationUser();
            user.UserName = model.Username.Trim();
            if (!string.IsNullOrWhiteSpace(model.Email))
                user.Email = model.Email.Trim();
            if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
                user.PhoneNumber = model.PhoneNumber.Trim();
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.Status = (int)UserStatus.Active;

            #region thêm quyền bố
            var parentRoles = _identityContext.Roles.Where(r => model.Roles.Contains(r.Id) && r.Parent != null)
                                                .Select(r => r.Parent)
                                                .Distinct();
            model.Roles.AddRange(parentRoles);
            #endregion

            using (var transaction = _identityContext.Database.BeginTransaction())
            {
                try
                {
                    //tạo tài khoản
                    var result = await _userManager.CreateAsync(user, model.Password.Trim());
                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Không tạo được tài khoản");
                    }
                    //thêm vai trò
                    foreach (var role in model.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }

                    transaction.Commit();
                    
                    return user;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
    }
}
