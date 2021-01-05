using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.survey.student.Common
{
    public class RoleNames
    {
        public static string UndergraduateStudent = "UndergraduateStudent";
        public static string GraduateStudent = "GraduateStudent";
        public static string Student = "Student";
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

    public class DayOfWeekDict
    {
        public static Dictionary<DayOfWeek, string> Value = new Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Monday, "Thứ 2" },
            { DayOfWeek.Tuesday, "Thứ 3" },
            { DayOfWeek.Wednesday, "Thứ 4" },
            { DayOfWeek.Thursday, "Thứ 5" },
            { DayOfWeek.Friday, "Thứ 6" },
            { DayOfWeek.Saturday, "Thứ 7" },
            { DayOfWeek.Sunday, "Chủ nhật" },
        };
    };
}
