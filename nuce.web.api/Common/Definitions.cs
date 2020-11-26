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
            { "K_CNTT", "Khoa CNTT" }
        };
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
    }
}
