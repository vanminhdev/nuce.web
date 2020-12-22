using nuce.web.quanly.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Models
{
    public class Answer
    {
        public string id { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Mã không được để trống")]
        //[OnlyNumber(ErrorMessage = "Mã phải ở dạng số")]
        public string code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung không được để trống")]
        public string content { get; set; }

        [Required(ErrorMessage = "Số thứ tự không được để trống")]
        public int? order { get; set; }

        public string cauHoiId { get; set; }
        
        public string cauHoiCode { get; set; }
    }

    public class AnswerOfQuestion
    {
        public List<Answer> Answers { get; set; }
        public string QuestionContent { get; set; }
        public string QuestionId { get; set; }
    }

    public class UpdateAnswer
    {
        public Answer AnswerBind { get; set; }
        public string QuestionContent { get; set; }
        public string QuestionId { get; set; }
    }

    public class AnswerCreate
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Mã không được để trống")]
        //public string code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung không được để trống")]
        public string content { get; set; }

        [Required(ErrorMessage = "Số thứ tự không được để trống")]
        public int? order { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Id câu hỏi không được để trống")]
        public string cauHoiId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã câu hỏi không được để trống")]
        public string cauHoiCode { get; set; }
    }

    public class AnswerCreateOfQuestion
    {
        public AnswerCreate AnswerBind { get; set; }
        public string QuestionContent { get; set; }
        public string QuestionId { get; set; }
    }
}