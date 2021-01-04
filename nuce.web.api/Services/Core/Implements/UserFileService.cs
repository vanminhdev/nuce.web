using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nuce.web.api.Helper;
using nuce.web.api.Models.Core;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace nuce.web.api.Services.Core.Implements
{
    public class UserFileService : IUserFileService
    {
        private readonly NuceCoreIdentityContext _context;
        private readonly IConfiguration _configuration;
        public UserFileService(NuceCoreIdentityContext _context, IConfiguration _configuration)
        {
            this._context = _context;
            this._configuration = _configuration;
        }
        /// <summary>
        /// Cập nhật ảnh vào resource folder
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="fileCode"></param>
        /// <param name="username"></param>
        /// <param name="loggedUserRoles"></param>
        /// <returns></returns>
        public async Task UploadUserImage(IFormFile formFile, string fileCode, string username, List<string> loggedUserRoles)
        {
            if (!FileHelper.isValidImageUpload(formFile))
            {
                throw new Exception("Ảnh không hợp lệ");
            }
            // 1mb
            long maxSize = 1024 * 1024;
            if (!FileHelper.isValidSize(formFile, maxSize))
            {
                throw new Exception("Dung lượng phải nhỏ hơn 1MB");
            }

            var oldFileUpload = _context.FileUpload.FirstOrDefault(f => f.Code == fileCode);
            if (oldFileUpload == null)
            {
                throw new Exception("Mã ảnh không hợp lệ");
            }

            if (!loggedUserRoles.Contains(oldFileUpload.Role))
            {
                throw new UnauthorizedAccessException("Bạn không có quyền");
            }

            string baseDir = @$"{_configuration.GetValue<string>("FolderResources")}";

            string dir = "Images";

            var newDir = Path.Combine(baseDir, dir);
            Directory.CreateDirectory(newDir);

            string fileName = fileCode.ToLower();

            string newFileName = $"{fileName}{Path.GetExtension(formFile.FileName).ToLower()}";
            string filePath = $"{newDir}/{newFileName}";

            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await FileHelper.SaveFileAsync(formFile, filePath);

                    oldFileUpload.FilePath = Path.Combine(dir, newFileName);
                    oldFileUpload.UpdateDatetime = DateTime.Now;
                    oldFileUpload.UpdateUsername = username;
                    oldFileUpload.Type = FileHelper.FileExtensionWithoutDot(newFileName);
                    oldFileUpload.FileType = FileHelper.FileExtensionWithoutDot(newFileName);

                    await _context.SaveChangesAsync();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lấy ảnh theo mã
        /// </summary>
        /// <param name="imgCode"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public async Task<ItemAvatarModel> GetUserImage(string imgCode, int? width, int? height) {
            var fileupload = await _context.FileUpload.FirstOrDefaultAsync(f => f.Code == imgCode);

            if (fileupload == null)
            {
                throw new KeyNotFoundException("Mã ảnh không tồn tại");
            }

            var imgPath = fileupload.FilePath;
            string baseDir = @$"{_configuration.GetValue<string>("FolderResources")}";

            string fullPath = Path.Combine(baseDir, imgPath);

            string extension = FileHelper.FileExtensionWithoutDot(imgPath);

            if (width == null || height == null)
            {
                var data = await File.ReadAllBytesAsync(fullPath);
                return new ItemAvatarModel { Data = data, Extension = extension };
            }
            Image img = Image.FromFile(imgPath);

            Image resizedNewImg = FileHelper.ResizeImage(img, width ?? 0, 40000, false);
            var newImg = resizedNewImg.CropImage(width ?? 0, height ?? 0);

            var result = newImg.ImageToByte();
            return new ItemAvatarModel { Data = result, Extension = extension };
        }
    }
}
