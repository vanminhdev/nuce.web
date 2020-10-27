using Microsoft.AspNetCore.Hosting;
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
        public PathProvider(IWebHostEnvironment _webHostEnvironment)
        {
            this._webHostEnvironment = _webHostEnvironment;
        }
        public string MapPath(string path)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, path);
        }
    }
}
