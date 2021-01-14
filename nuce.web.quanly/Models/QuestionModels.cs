using nuce.web.quanly.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Models
{
    public class Question
    {
        public string id { get; set; }
        public string code { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public int? order { get; set; }
        public string parentCode { get; set; }
    }

    public class QuestionCreate
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Mã không được để trống")]
        //public string code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung không được để trống")]
        public string content { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Loại câu hỏi không được để trống")]
        public string type { get; set; }

        [Required(ErrorMessage = "Số thứ tự không được để trống")]
        [Range(0, 300, ErrorMessage = "Số thứ tự không hợp lệ")]
        public int? order { get; set; }
        public List<QuestionDetail> questionChilds { get; set; }
        public List<string> questionChildCodes { get; set; }
    }

    public class QuestionDetail
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Id không được để trống")]
        [NotContainWhiteSpace(ErrorMessage = "Id không được chứa khoảng trắng")]
        public string id { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Mã không được để trống")]
        //[OnlyNumber(ErrorMessage = "Mã phải ở dạng số")]
        //[NotContainWhiteSpace(ErrorMessage = "Mã không được chứa khoảng trắng")]
        public string code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung không được để trống")]
        public string content { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Loại câu hỏi không được để trống")]
        public string type { get; set; }

        [Required(ErrorMessage = "Số thứ tự không được để trống")]
        [Range(0, 300, ErrorMessage = "Số thứ tự không hợp lệ")]
        public int? order { get; set; }

        public string parentCode { get; set; }

        public List<QuestionDetail> questionChilds { get; set; }

        public List<string> questionChildCodes { get; set; }
    }
}