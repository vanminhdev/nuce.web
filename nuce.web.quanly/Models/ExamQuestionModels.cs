﻿using nuce.web.quanly.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Models
{
    public class ExamQuestions
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }

    public class ExamQuestionsCreate
    {
        public string Name { get; set; }
    }

    public class ExamStructure
    {
        public Guid id { get; set; }
        public string code { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public int order { get; set; }
        public Guid questionId { get; set; }
    }

    public class AddQuestionExam
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã không được để trống")]
        [NotContainWhiteSpace]
        public string examQuestionId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã không được để trống")]
        [OnlyNumber(ErrorMessage = "Mã phải ở dạng số")]
        public string questionCode { get; set; }

        [Required(ErrorMessage = "Số thứ tự không được để trống")]
        public int? order { get; set; }
    }

    public class SortQuestion
    {
        public string cauHoiId { get; set; }
        public int order { get; set; }
    }
}