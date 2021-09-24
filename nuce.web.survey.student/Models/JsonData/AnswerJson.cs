using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.survey.student.Models.JsonData
{
    public class AnswerJson
    {
        public string Code { get; set; }
        public string Content { get; set; }
        public QuestionJson AnswerChildQuestion { get; set; }
        public List<string> ShowQuestion { get; set; }
        public List<string> HideQuestion { get; set; }
    }

    public class SelectedAnswer
    {
        public string QuestionCode { get; set; }
        public string AnswerCode { get; set; }
        public List<string> AnswerCodes { get; set; }
        public string AnswerContent { get; set; }
        public bool? IsAnswerChildQuestion { get; set; }
        public int? NumStart { get; set; }
        public string City { get; set; }
    }
}
