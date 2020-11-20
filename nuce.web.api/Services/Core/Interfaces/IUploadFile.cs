using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface IUploadFile
    {
        public Task SaveFileAsync(IFormFile file, string path);
        public bool isValidSize(IFormFile file, long max);
        public bool isValidImage(IFormFile file);
    }
}
