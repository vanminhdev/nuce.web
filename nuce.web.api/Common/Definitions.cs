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

    public class RoleNames
    {
        public static string UndergraduateStudent = "UndergraduateStudent";
        public static string GraduateStudent = "GraduateStudent";
        public static string Student = "Student";
    }

    /// <summary>
    /// Loại thao tác
    /// </summary>
    public enum BackupType
    {
        /// <summary>
        /// Lưu trữ
        /// </summary>
        Backup = 1,
        /// <summary>
        /// Khôi phục
        /// </summary>
        Restore = 2
    }

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
        public static readonly string AsEduSurveyBaiKhaoSatSinhVien = "AS_Edu_Survey_BaiKhaoSat_SinhVien";
        public static readonly string AsEduSurveyReportTotal = "AS_Edu_Survey_ReportTotal";
        public static readonly string AsEduSurveyUndergraduateReportTotal = "AS_Edu_Survey_Undergraduate_ReportTotal";
    }

    /// <summary>
    /// Loại file upload
    /// </summary>
    public class ContentTypes
    {
        public static readonly string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}
