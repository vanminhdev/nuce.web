using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Models.Survey.JsonData
{
    public class AnswerJson
    {
        public string Code { get; set; }
        public string Content { get; set; }
        public QuestionJson AnswerChildQuestion { get; set; }
        public List<string> ShowQuestion { get; set; }
        public List<string> HideQuestion { get; set; }
    }

    /// <summary>
    /// Là mẫu lưu vào bài làm cho sinh viên
    /// </summary>
    public class SelectedAnswer
    {
        /// <summary>
        /// Mã câu hỏi, nếu là câu hỏi con của đáp án sẽ có dạng AnswerCode_QuestionCode
        /// </summary>
        public string QuestionCode { get; set; }
        /// <summary>
        /// Mã câu trả lời
        /// </summary>
        public string AnswerCode { get; set; }
        /// <summary>
        /// Mã câu trả lời nhiều lựa chọn
        /// </summary>
        public List<string> AnswerCodes { get; set; }
        /// <summary>
        /// Nội dung câu trả lời cho câu hỏi text
        /// </summary>
        public string AnswerContent { get; set; }
        /// <summary>
        /// Có là câu câu hỏi con của một câu trả lời
        /// </summary>
        public bool? IsAnswerChildQuestion { get; set; }
        /// <summary>
        /// Số sao vote
        /// </summary>
        public int? NumStart { get; set; }
        /// <summary>
        /// Tỉnh thành công tác
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Mã sinh viên
        /// </summary>
        public string StudentCode { get; set; }
        /// <summary>
        /// Ngày bắt đầu làm bài
        /// </summary>
        public DateTime? NgayBatDauLamBai { get; set; }
    }

    /// <summary>
    /// Có thêm trường bài khảo sát để sau tra trong đây để hiển thị đề đã được lưu vào bài khảo sát
    /// </summary>
    public class SelectedAnswerExtend : SelectedAnswer
    {
        public Guid TheSurveyId { get; set; }
    }
}
