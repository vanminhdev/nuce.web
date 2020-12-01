﻿using System;
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
            { "P_KhaoThi", "Phòng khảo thí" },
            { "P_CTSV", "Phòng công tác sinh viên" },
            { "K_CNTT", "Khoa CNTT" }
        };

        public static Dictionary<string, string> QuestionType = new Dictionary<string, string>()
        {
            {"SC", "Một lựa chọn" },
            {"MC", "Nhiều lựa chọn" },
            //{"TQ", "Câu hỏi đúng" },
            //{"FQ", "Câu hỏi sai" },
            //{"SQ", "Câu hỏi kéo thả" },
            //{"MA", "Ghép đôi phù hợp" },
            //{"MW", "Điền từ vào chỗ trống" },
            {"SA", "Trả lời ngắn" },
            {"NR", "Câu hỏi số" },
            //{"HS", "Khoanh vùng điểm ảnh" },
            //{"GQ", "Câu hỏi nhiều câu hỏi phụ" },
            //{"SP", "Nghe nói" },
            //{"EQ", "Câu hỏi cảm xúc" },
            //{"TL", "Hai cấp" },
            {"T",  "Tiêu đề"}
        };
    }

    public enum BackupTypeDefination
    {
        BACKUP = 1,
        RESTORE = 2
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
    }
}
