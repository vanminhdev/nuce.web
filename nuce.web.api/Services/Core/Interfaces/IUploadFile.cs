using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface IUploadFile
    {
        public Task SaveFileAsync(IFormFile file, string path);
        public bool isValidSize(IFormFile file, long max);
        public bool isValidImageUpload(IFormFile file);
        public Task<MemoryStream> DownloadFileAsync(string path);
        public Image ResizeImage(Image image, int newWidth, int maxHeight, bool onlyResizeIfWider);
        public Image CropImage(Image src, int width, int height);
        public byte[] ImageToByte(Image image);
        public Stream ImageToStream(Image image);
    }
}
