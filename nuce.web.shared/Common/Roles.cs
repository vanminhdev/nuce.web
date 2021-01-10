using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nuce.web.shared
{
    /// <summary>
    /// Danh sách chi tiết các role
    /// </summary>
    public class RoleNames
    {
        public const string Admin = "Admin";
        public const string KhaoThi = "P_KhaoThi";
        public const string CTSV = "P_CTSV";
        /// <summary>
        /// Khảo thí tạo danh mục
        /// </summary>
        public const string KhaoThi_Add_Cat = "P_KhaoThi_Add_Cat";
        /// <summary>
        /// Khảo thí cập nhật danh mục
        /// </summary>
        public const string KhaoThi_Edit_Cat = "P_KhaoThi_Edit_Cat";
        /// <summary>
        /// Khảo thí đăng bài
        /// </summary>
        public const string KhaoThi_Add_NewsItem = "P_KhaoThi_Add_NewsItem";
        /// <summary>
        /// Khảo thí duyệt bài
        /// </summary>
        public const string KhaoThi_Approve_NewsItem = "P_KhaoThi_Approve_NewsItem";
        /// <summary>
        /// Khảo thí chỉnh sửa bài
        /// </summary>
        public const string KhaoThi_Edit_NewsItem = "P_KhaoThi_Edit_NewsItem";
        /// <summary>
        /// Khảo thí upload ảnh cho website
        /// </summary>
        public const string KhaoThi_Upload_WebImage = "P_KhaoThi_Upload_WebImage";
        /// <summary>
        /// Khảo thí cập nhật thông tin liên hệ
        /// </summary>
        public const string KhaoThi_Edit_Contact = "P_KhaoThi_Edit_Contact";
        /// <summary>
        /// Khảo thí tạo tài khoản
        /// </summary>
        public const string KhaoThi_Add_Account = "P_KhaoThi_Add_Account";
        /// <summary>
        /// Khảo thí cập nhật tài khoản
        /// </summary>
        public const string KhaoThi_Edit_Account = "P_KhaoThi_Edit_Account";
        /// <summary>
        /// Khảo thí phân chức năng cho user
        /// </summary>
        public const string KhaoThi_Pick_Role = "P_KhaoThi_Pick_Role";
        /// <summary>
        /// Khảo thí khôi phục mật khẩu cho user khác
        /// </summary>
        public const string KhaoThi_Reset_Password = "P_KhaoThi_Reset_Password";
        /// <summary>
        /// Khảo thí khoá tài khoản
        /// </summary>
        public const string KhaoThi_Lock_Account = "P_KhaoThi_Lock_Account";
        /// <summary>
        /// Khảo thí sửa tài khoản
        /// </summary>
        public const string KhaoThi_Delete_Account = "P_KhaoThi_Delete_Account";
        /// <summary>
        /// Khảo thí Chuyên viên phụ trách khảo sát sinh viên thường
        /// </summary>
        public const string KhaoThi_Survey_Normal = "P_KhaoThi_Survey_Normal";
        /// <summary>
        /// Khảo thí Chuyên viên phụ trách khảo sát sinh viên trước tốt nghiệp
        /// </summary>
        public const string KhaoThi_Survey_Undergraduate = "P_KhaoThi_Survey_Undergraduate";
        /// <summary>
        /// Khảo thí Chuyên viên phụ trách khảo sát cựu sinh viên
        /// </summary>
        public const string KhaoThi_Survey_Graduate = "P_KhaoThi_Survey_Graduate";
        /// <summary>
        /// Khảo thí backup dữ liệu
        /// </summary>
        public const string KhaoThi_Backup_Database = "P_KhaoThi_Backup_Database";
        /// <summary>
        /// Khảo thí KHOA BAN tham gia khảo sát/xem kết quả ks
        /// </summary>
        public const string KhaoThi_Survey_KhoaBan = "P_KhaoThi_Survey_KhoaBan";
        /// <summary>
        /// Khảo thí BỘ MÔN tham gia khảo sát/xem kết quả ks
        /// </summary>
        public const string KhaoThi_Survey_Department = "P_KhaoThi_Survey_Department";
        /// <summary>
        /// Khảo thí GIẢNG VIÊN tham gia khảo sát/xem kết quả ks
        /// </summary>
        public const string KhaoThi_Survey_GiangVien = "P_KhaoThi_Survey_Lecturer";
        /// <summary>
        /// Khảo thí fake user vào chỉ để xem
        /// </summary>
        public const string FakeStudent = "FakeStudent";
        /// <summary>
        /// Khảo thí Sinh viên trước tốt nghiệp đăng nhập để khảo sát
        /// </summary>
        public const string UndergraduateStudent = "UndergraduateStudent";
        /// <summary>
        /// Khảo thí Sinh viên đã tốt nghiệp đăng nhập để khảo sát
        /// </summary>
        public const string GraduateStudent = "GraduateStudent";
        /// <summary>
        /// Khảo thí Sinh viên thường đăng nhập để khảo sát
        /// </summary>
        public const string Student = "Student";
    }
    /// <summary>
    /// Hiển thị menu
    /// </summary>
    public class MenuRoleGroup
    {
        public static IEnumerable<string> Account = new List<string>
        {
            RoleNames.Admin,
            RoleNames.KhaoThi_Add_Account,
            RoleNames.KhaoThi_Edit_Account,
            RoleNames.KhaoThi_Delete_Account,
            RoleNames.KhaoThi_Lock_Account,
            RoleNames.KhaoThi_Pick_Role,
            RoleNames.KhaoThi_Reset_Password,
        };

        public static List<string> Category = new List<string>
        {
            RoleNames.KhaoThi_Add_Cat,
            RoleNames.KhaoThi_Edit_Cat,
            RoleNames.KhaoThi_Edit_Contact,
            RoleNames.KhaoThi_Upload_WebImage
        };

        public static List<string> News = new List<string>
        {
            RoleNames.KhaoThi_Add_NewsItem,
            RoleNames.KhaoThi_Edit_NewsItem,
            RoleNames.KhaoThi_Approve_NewsItem,
        };

        public static List<string> SurveyObject = new List<string>
        {
            RoleNames.KhaoThi_Survey_Normal,
            RoleNames.KhaoThi_Survey_Graduate,
            RoleNames.KhaoThi_Survey_Undergraduate
        };

    }
    /// <summary>
    /// Role Prefix
    /// </summary>
    public class ApiRole
    {
        public const string Account = RoleNames.Admin
                                    + "," + RoleNames.KhaoThi_Add_Account
                                    + "," + RoleNames.KhaoThi_Edit_Account
                                    + "," + RoleNames.KhaoThi_Delete_Account
                                    + "," + RoleNames.KhaoThi_Lock_Account
                                    + "," + RoleNames.KhaoThi_Pick_Role
                                    + "," + RoleNames.KhaoThi_Reset_Password;

        public const string Category = RoleNames.KhaoThi_Add_Cat
                                    + "," + RoleNames.KhaoThi_Edit_Cat;

        public const string News = RoleNames.KhaoThi_Add_NewsItem
                                    + "," + RoleNames.KhaoThi_Edit_NewsItem
                                    + "," + RoleNames.KhaoThi_Approve_NewsItem;

        public const string SurveyObject = RoleNames.KhaoThi_Survey_Normal
                                    + "," + RoleNames.GraduateStudent
                                    + "," + RoleNames.UndergraduateStudent;
    }
    
}
