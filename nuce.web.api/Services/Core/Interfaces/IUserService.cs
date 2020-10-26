using nuce.web.api.Models.Ctsv;
using Microsoft.AspNetCore.Identity;
using nuce.web.api.ViewModel.Core.NuceIdentity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface IUserService
    {
        public Task<IdentityUser> FindByNameAsync(string username);
        public Task<List<Claim>> AddClaimsAsync(LoginModel model, IdentityUser user);
        public JwtSecurityToken CreateJWTAccessToken(List<Claim> claims);
        public JwtSecurityToken CreateJWTRefreshToken(List<Claim> claims);
        public Task<bool> UserIsvalidAsync(LoginModel model, IdentityUser user);
        public string GetCurrentStudentCode();
        public string GetClaimByKey(string key);
        public long? GetCurrentStudentID();
        public AsAcademyStudent GetCurrentStudent();
    }
}
