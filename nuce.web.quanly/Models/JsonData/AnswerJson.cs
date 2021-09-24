using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.quanly.Models.JsonData
{
    public class AnswerJson
    {
        public string Code { get; set; }
        public string Content { get; set; }
        public QuestionJson AnswerChildQuestion { get; set; }
        public List<string> ShowQuestion { get; set; }
        public List<string> HideQuestion { get; set; }
    }
}
