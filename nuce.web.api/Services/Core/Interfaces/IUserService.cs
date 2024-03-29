﻿using nuce.web.api.Models.Ctsv;
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
using nuce.web.api.ViewModel.Base;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface IUserService
    {
        public Task<ApplicationUser> FindByName(string username);
        public Task<List<Claim>> AddClaimsAsync(LoginModel model, ApplicationUser user);
        public JwtSecurityToken CreateJWTAccessToken(List<Claim> claims);
        public JwtSecurityToken CreateJWTRefreshToken(List<Claim> claims);
        public Task<bool> UserLogin(LoginModel model);
        public string GetCurrentStudentCode();
        public string GetClaimByKey(string key);
        public List<string> GetClaimListByKey(string key);
        public long? GetCurrentStudentID();
        public AsAcademyStudent GetCurrentStudent();
        public string GetUserName();
        public AsAcademyStudent GetStudentByEmail(string email);
        public Task<PaginationModel<UserModel>> GetAllAsync(UserFilter filter, int skip = 0, int pageSize = 20);
        public Task<UserDetailModel> GetByIdAsync(string id);
        public Task ActiveUserAsync(string id);
        public Task DeactiveUserAsync(string id);
        public Task DeleteUserAsync(string id);
        public Task UpdateUserAsync(string id, UserUpdateModel user);
        public Task ResetPasswordAsync(string id, string newPassword);
        public Task<ApplicationUser> CreateUser(UserCreateModel model);
    }
}
