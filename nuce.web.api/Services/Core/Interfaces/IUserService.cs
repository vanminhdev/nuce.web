using nuce.web.api.Models.Ctsv;
using Microsoft.AspNetCore.Identity;
using nuce.web.api.ViewModel.Core.NuceIdentity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using nuce.web.api.ViewModel;
using Microsoft.AspNetCore.Mvc.Filters;
using nuce.web.api.Models.Core;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface IUserService
    {
        public Task<ApplicationUser> FindByNameAsync(string username);
        public Task<List<Claim>> AddClaimsAsync(LoginModel model, ApplicationUser user);
        public JwtSecurityToken CreateJWTAccessToken(List<Claim> claims);
        public JwtSecurityToken CreateJWTRefreshToken(List<Claim> claims);
        public Task<ResponseBody> UserIsvalidAsync(LoginModel model, ApplicationUser user);
        public string GetCurrentStudentCode();
        public string GetClaimByKey(string key);
        public long? GetCurrentStudentID();
        public AsAcademyStudent GetCurrentStudent();
        public Task<UserPaginationModel> GetAllAsync(UserFilter filter, int skip = 0, int pageSize = 20);
        public Task<UserDetailModel> GetByIdAsync(string id);
        public Task ActiveUserAsync(string id);
        public Task DeactiveUserAsync(string id);
        public Task DeleteUserAsync(string id);
        public Task UpdateUserAsync(string id, UserUpdateModel user);
        public Task ResetPasswordAsync(string id, string newPassword);
    }
}
