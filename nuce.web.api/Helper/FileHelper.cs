using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace nuce.web.api.Helper
{
    public static class FileHelper
    {
        private static string[] IMAGE_MIMETYPE = { "image/jpg", "image/jpeg", "image/pjpeg", "image/gif", "image/x-png", "image/png" };
        private static string[] IMAGE_EXTENSION = { ".jpg", ".png", ".gif", ".jpeg" };

        public static async Task SaveFileAsync(IFormFile file, string path)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }

        public static bool isValidImageUpload(this IFormFile file)
        {
            if (!string.IsNullOrEmpty(file.ContentType) && !IMAGE_MIMETYPE.Contains(file.ContentType.ToLower()))
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

        public static bool isValidSize(IFormFile file, long max)
        {
            var size = file.Length;
            return size <= max;
        }

        public static async Task<MemoryStream> DownloadFileAsync(string path)
        {
            var uri = new Uri(path, UriKind.Absolute);
            using (var webClient = new WebClient())
            {
                byte[] byteImg = await webClient.DownloadDataTaskAsync(uri);
                var memoryStream = new MemoryStream(byteImg);
                return memoryStream;
            }
        }

        public static Image ResizeImage(this Image image, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            if (onlyResizeIfWider && image.Width <= newWidth) newWidth = image.Width;

            var newHeight = image.Height * newWidth / image.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead  
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            var res = new Bitmap(newWidth, newHeight);

            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return res;
        }

        public static Image CropImage(this Image src, int width, int height)
        {
            Rectangle cropRect = new Rectangle(0, 0, width, height);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage((Bitmap)src, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
                return target;
            }
        }

        public static byte[] ImageToByte(this Image image)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(image, typeof(byte[]));
            return xByte;
        }

        public static Stream ImageToStream(this Image image)
        {
            var stream = new MemoryStream();

            image.Save(stream, ImageFormat.Bmp);
            stream.Position = 0;

            return stream;
        }
        /// <summary>
        /// Bỏ . trong extension
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string FileExtensionWithoutDot(this string filename)
        {
            string extension = Path.GetExtension(filename);
            return extension.Substring(1, extension.Length - 1);
        }
    }
}
