using Microsoft.AspNetCore.Http;
using nuce.web.api.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface IUserFileService
    {
        public Task UploadUserImage(IFormFile formFile, string fileCode, string username, List<string> loggedUserRoles);
        public Task<ItemAvatarModel> GetUserImage(string imgCode, int? width, int? height);
    }
}
