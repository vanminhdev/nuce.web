using EduWebService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Core.NuceIdentity;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public UserService(UserManager<IdentityUser> _userManager, IConfiguration configuration)
        {
            this._userManager = _userManager;
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

        public JwtSecurityToken CreateJWTToken(List<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        public async Task<IdentityUser> FindByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<bool> UserIsvalidAsync(LoginModel model, IdentityUser user)
        {
            bool result = false;
            if (model.IsStudent)
            {
                ServiceSoapClient srvc = new ServiceSoapClient(EndpointConfiguration.ServiceSoap12);
                var isvalid = await srvc.authenAsync(model.Username, model.Password);

                result = isvalid == 1;
            }
            else
            {
                result = user != null && await _userManager.CheckPasswordAsync(user, model.Password);
            }
            return result;
        }
    }
}
