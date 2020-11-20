using Microsoft.AspNetCore.Http;
using nuce.web.api.Services.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Implements
{
    public class UploadFile : IUploadFile
    {
        private string[] IMAGE_MIMETYPE = { "image/jpg", "image/jpeg", "image/pjpeg", "image/gif", "image/x-png", "image/png" };
        private string[] IMAGE_EXTENSION = { ".jpg", ".png", ".gif", ".jpeg" };
        public async Task SaveFileAsync(IFormFile file, string path)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }
        public bool isValidImage(IFormFile file)
        {
            if (!IMAGE_MIMETYPE.Contains(file.ContentType.ToLower()))
            {
                return false;
            }
            string extenstion = Path.GetExtension(file.FileName).ToLower();
            if (!IMAGE_EXTENSION.Contains(extenstion))
            {
                return false;
            }
            return true;
        }
        public bool isValidSize(IFormFile file, long max)
        {
            var size = file.Length;
            return size <= max;
        }
    }
}
