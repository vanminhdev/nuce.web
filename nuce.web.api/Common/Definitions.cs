using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Common
{
    public class Definitions
    {
        public static Dictionary<string, string> Roles = new Dictionary<string, string>()
        {
            { "Admin", "Quản trị" },
            { "P_KhaoThi", "Phòng khảo thí" },
            { "P_CTSV", "Phòng công tác sinh viên" },
        };
    }

    //public class RoleNames
    //{
    //    public const string Admin = "Admin";
    //    public const string KhaoThi = "P_KhaoThi";
    //    public const string CTSV = "P_CTSV";
    //    /// <summary>
    //    /// Khảo thí tạo danh mục
    //    /// </summary>
    //    public const string KhaoThi_Add_Cat = "P_KhaoThi_Add_Cat";
    //    /// <summary>
    //    /// Khảo thí cập nhật danh mục
    //    /// </summary>
    //    public const string KhaoThi_Edit_Cat = "P_KhaoThi_Edit_Cat";
    //    /// <summary>
    //    /// Khảo thí đăng bài
    //    /// </summary>
    //    public const string KhaoThi_Add_NewsItem = "P_KhaoThi_Add_NewsItem";
    //    /// <summary>
    //    /// Khảo thí duyệt bài
    //    /// </summary>
    //    public const string KhaoThi_Approve_NewsItem = "P_KhaoThi_Approve_NewsItem";
    //    /// <summary>
    //    /// Khảo thí upload ảnh cho website
    //    /// </summary>
    //    public const string KhaoThi_Upload_WebImage = "P_KhaoThi_Upload_WebImage";
    //    /// <summary>
    //    /// Khảo thí cập nhật thông tin liên hệ
    //    /// </summary>
    //    public const string KhaoThi_Edit_Contact = "P_KhaoThi_Edit_Contact";
    //    /// <summary>
    //    /// Khảo thí tạo tài khoản
    //    /// </summary>
    //    public const string KhaoThi_Add_Account = "P_KhaoThi_Add_Account";
    //    /// <summary>
    //    /// Khảo thí phân chức năng cho user
    //    /// </summary>
    //    public const string KhaoThi_Pick_Role = "P_KhaoThi_Pick_Role";
    //    /// <summary>
    //    /// Khảo thí khôi phục mật khẩu cho user khác
    //    /// </summary>
    //    public const string KhaoThi_Reset_Password = "P_KhaoThi_Reset_Password";
    //    /// <summary>
    //    /// Khảo thí khoá tài khoản
    //    /// </summary>
    //    public const string KhaoThi_Lock_Account = "P_KhaoThi_Lock_Account";
    //    /// <summary>
    //    /// Khảo thí sửa tài khoản
    //    /// </summary>
    //    public const string KhaoThi_Delete_Account = "P_KhaoThi_Delete_Account";
    //    /// <summary>
    //    /// Khảo thí sinh viên thường
    //    /// </summary>
    //    public const string KhaoThi_Survey_Normal = "P_KhaoThi_Survey_Normal";
    //    /// <summary>
    //    /// Khảo thí sinh viên trước tốt nghiệp
    //    /// </summary>
    //    public const string KhaoThi_Survey_Undergraduate = "P_KhaoThi_Survey_Undergraduate";
    //    /// <summary>
    //    /// Khảo thí cựu sinh viên
    //    /// </summary>
    //    public const string KhaoThi_Survey_Graduate = "P_KhaoThi_Survey_Graduate";
    //    /// <summary>
    //    /// Khảo thí backup dữ liệu
    //    /// </summary>
    //    public const string KhaoThi_Backup_Database = "P_KhaoThi_Backup_Database";

    //    public const string KhaoThi_Survey_KhoaBan = "P_KhaoThi_Survey_KhoaBan";
    //    public const string KhaoThi_Survey_GiangVien = "P_KhaoThi_Survey_Lecturer";
    //    /// <summary>
    //    /// Khảo thí fake user vào chỉ để xem
    //    /// </summary>
    //    public const string FakeStudent = "FakeStudent";
    //    public const string UndergraduateStudent = "UndergraduateStudent";
    //    public const string GraduateStudent = "GraduateStudent";
    //    public const string Student = "Student";
    //}

    /// <summary>
    /// Loại đợt khảo sát
    /// </summary>
    public enum SurveyRoundType
    {
        /// <summary>
        /// Đánh giá chất lượng giảng dạy
        /// </summary>
        RatingTeachingQuality = 1
    }

    /// <summary>
    /// Loại bài khảo sát
    /// </summary>
    public enum TheSurveyType
    {
        /// <summary>
        /// Đề cho môn lý thuyết
        /// </summary>
        TheoreticalSubjects = 1,

        /// <summary>
        /// Đề cho môn thực hành, thí nghiệm và thực tập
        /// </summary>
        PracticalSubjects = 2,

        /// <summary>
        /// Đề cho môn lý thuyết + thực hành
        /// </summary>
        TheoreticalPracticalSubjects = 3,

        /// <summary>
        /// Đề cho môn đồ án
        /// </summary>
        AssignmentSubjects = 4,

        /// <summary>
        /// Đề cho môn không được phần loại
        /// </summary>
        DefaultSubjects = 5,
    }

    /// <summary>
    /// Loại bài khảo sát cho cựu sinh viên và sinh viên sắp tốt nghiệp
    /// </summary>
    public enum GraduateTheSurveyType
    {
        Type1 = 1,
    }

    /// <summary>
    /// Loại câu hỏi
    /// </summary>
    public class QuestionType
    {
        /// <summary>
        /// Một lựa chọn
        /// </summary>
        public static string SC = "SC";
        /// <summary>
        /// Nhiều lựa chọn
        /// </summary>
        public static string MC = "MC";
        /// <summary>
        /// Câu hỏi đúng
        /// </summary>
        public static string TQ = "TQ";
        /// <summary>
        /// Câu hỏi sai
        /// </summary>
        public static string FQ = "FQ";
        /// <summary>
        /// Câu hỏi kéo thả
        /// </summary>
        public static string SQ = "SQ";
        /// <summary>
        /// Ghép đôi phù hợp
        /// </summary>
        public static string MA = "MA";
        /// <summary>
        /// Điền từ vào chỗ trống
        /// </summary>
        public static string MW = "MW";
        /// <summary>
        /// Trả lời ngắn
        /// </summary>
        public static string SA = "SA";
        /// <summary>
        /// Câu hỏi số
        /// </summary>
        public static string NR = "NR";
        /// <summary>
        /// Khoanh vùng điểm ảnh
        /// </summary>
        public static string HS = "HS";
        /// <summary>
        /// Câu hỏi nhiều câu hỏi phụ
        /// </summary>
        public static string GQ = "GQ";
        /// <summary>
        /// Nghe nói
        /// </summary>
        public static string SP = "SP";
        /// <summary>
        /// Câu hỏi cảm xúc 
        /// </summary>
        public static string EQ = "EQ";
        /// <summary>
        /// Hai cấp
        /// </summary>
        public static string TL = "TL";
        /// <summary>
        /// Tiêu đề vd: I. abc  II.def
        /// </summary>
        public static string T = "T";
        /// <summary>
        /// Lựa chọn tỉnh thành
        /// </summary>
        public static string CityC = "CityC";
        /// <summary>
        /// Rate Start
        /// </summary>
        public static string StarRating = "StarRating";
    }

    /// <summary>
    /// Tên bảng làm việc mất nhiều thời gian
    /// </summary>
    public class TableNameTask
    {
        public static readonly string AsAcademyStudentClassRoom = "AS_Academy_Student_ClassRoom";
        public static readonly string AsAcademyStudent = "AS_Academy_Student";
        public static readonly string AsAcademyCStudentClassRoom = "AS_Academy_C_Student_ClassRoom";
        public static readonly string AsEduSurveyBaiKhaoSatSinhVien = "AS_Edu_Survey_BaiKhaoSat_SinhVien";
        public static readonly string AsEduSurveyReportTotal = "AS_Edu_Survey_ReportTotal";
        public static readonly string AsEduSurveyUndergraduateReportTotal = "AS_Edu_Survey_Undergraduate_ReportTotal";
    }

    /// <summary>
    /// Loại của tham số trả ra cho giao diện trang client
    /// </summary>
    public class ClientParameterTypes
    {
        public static readonly string KhaoKhaoSatSV = "KSSV";
        public static readonly string Contact = "CONTACT";
    }

    /// <summary>
    /// Loại file upload
    /// </summary>
    public class ContentTypes
    {
        public static readonly string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static readonly string Xls = "application/vnd.ms-excel";
    }
}
