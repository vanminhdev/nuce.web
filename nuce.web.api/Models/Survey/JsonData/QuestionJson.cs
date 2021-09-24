using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Models.Survey.JsonData
{
    public class QuestionJson
    {
        /// <summary>
        /// nếu là câu hỏi con của đáp án sẽ có dạng AnswerCode_QuestionCode
        /// </summary>
        public string Code { get; set; }
        public int DifficultID { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public float Mark { get; set; }
        public string Explain { get; set; }
        public string Type { get; set; }
        public List<AnswerJson> Answers { get; set; }
        public List<QuestionJson> ChildQuestion { get; set; }
    }
}
