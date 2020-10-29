using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel
{
    public class ResponseBody
    {
        public static string SUCCESS_STATUS = "success";
        public static string ERROR_STATUS = "error";
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
