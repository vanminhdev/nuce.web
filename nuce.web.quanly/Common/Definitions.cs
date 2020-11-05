using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.quanly.Common
{
    public class Definitions
    {
        public static Dictionary<string, string> Roles = new Dictionary<string, string>()
        {
            { "Admin", "Quản trị" },
            { "PhongBan", "Phòng ban" },
            { "P_KhaoThi", "Phòng khảo thí" },
            { "P_CTSV", "Phòng công tác sinh viên" },
            { "Khoa", "Khoa" },
            { "K_CNTT", "Khoa CNTT" }
        };

        public class Function
        {
            public string Link { get; set; }
            public string Title { get; set; }
        }

        //public static Dictionary<string, List<Function>> Functions = new Dictionary<string, List<Function>>()
        //{
        //    {
        //        "Admin", new List<Function>() {
        //            new Function{ Link = "/usermanager/index"  }
        //        }
        //    }
        //};
    }
}
