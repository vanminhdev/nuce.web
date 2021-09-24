using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using nuce.web.api.Services.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Implements
{
    public class PathProvider : IPathProvider
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public PathProvider(IWebHostEnvironment _webHostEnvironment, IConfiguration _configuration)
        {
            this._webHostEnvironment = _webHostEnvironment;
            this._configuration = _configuration;
        }
        public string MapPath(string path)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, path);
        }
        public string MapPathStudentImage(string path)
        {
            string host = _configuration["StudentImageServer"];
            return $"{host}/{path}";
        }
    }
}
