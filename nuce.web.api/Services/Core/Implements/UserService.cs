using EduWebService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using nuce.web.api.Common;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Core.NuceIdentity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static EduWebService.ServiceSoapClient;

namespace nuce.web.api.Services.Core.Implements
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private readonly IStudentRepository _studentRepository;
        public UserService(UserManager<IdentityUser> _userManager,
                IConfiguration configuration, 
                IHttpContextAccessor httpContextAccessor,
                IStudentRepository _studentRepository
        )
        {
            this._userManager = _userManager;
            this._studentRepository = _studentRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _configuration = configuration;
        }
        public async Task<List<Claim>> AddClaimsAsync(LoginModel model, IdentityUser user)
        {
            string username = model.IsStudent ? model.Username : user.UserName;

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            if (model.IsStudent)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, "Student"));
                authClaims.Add(new Claim(UserParameters.MSSV, username));
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

        public async Task<IdentityUser> FindByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<ResponseBody> UserIsvalidAsync(LoginModel model, IdentityUser user)
        {
            bool result = false;
            if (model.IsStudent)
            {
                ServiceSoapClient srvc = new ServiceSoapClient(EndpointConfiguration.ServiceSoap12);
                try
                {
                    var isvalid = await srvc.authenAsync(model.Username, model.Password);

                    result = isvalid == 1;
                }
                catch (Exception ex)
                {
                    return new ResponseBody { Data = false, Message = "Lỗi khi gọi service đào tạo" };
                }
            }
            else
            {
                result = user != null && await _userManager.CheckPasswordAsync(user, model.Password);
            }
            return new ResponseBody { Data = result, Message = "Tên đăng nhập hoặc mật khẩu không chính xác" };
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
                return identity.FindFirst(key) != null ? identity.FindFirst(key).Value : null;
            }
            return null;
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
    }
}
