using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Models.Base
{
    public class FileDownload
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}