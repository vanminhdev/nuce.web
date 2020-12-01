using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.survey.student.Common
{
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
    }
}
