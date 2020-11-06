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

        public class RoleFunc
        {
            public string FuncType { get; set; }
            public string FuncName { get; set; }
        }

        public static Dictionary<string, List<RoleFunc>> RoleFunction = new Dictionary<string, List<RoleFunc>>()
        {
            {
                "Admin", new List<RoleFunc>() {
                    new RoleFunc { FuncType = "QuanTri", FuncName = "QuanLyTaiKhoan" },
                    new RoleFunc { FuncType = "QuanTri", FuncName = "CauHinhTrangWeb" }
                }
            },
            {
                "P_KhaoThi", new List<RoleFunc>() {
                    new RoleFunc { FuncType = "KhaoThi", FuncName = "QuanLyDotKhaoThi" },
                    new RoleFunc { FuncType = "KhaoThi", FuncName = "QuanLyCauHoi" }
                }
            },
            {
                "P_CTSV", new List<RoleFunc>() {
                    
                }
            },
        };
    }
}
