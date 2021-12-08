using System;
using System.Collections.Generic;
using System.Text;

namespace nuce.web.shared.Models.Survey
{
    public class AnswerSaveVM
    {
        public string studentCode { get; set; }
        public string theSurveyId { get; set; }
        public string questionCode { get; set; }
        public string answerCode { get; set; }
        public string answerCodeInMulSelect { get; set; }
        public string answerContent { get; set; }
        public int? numStar { get; set; }
        public string city { get; set; }
        public bool isAnswerCodesAdd { get; set; } = true;
    }

    public class SubmitBaiLam
    {
        public string theSurveyId { get; set; }
        public string studentCode { get; set; }
        public string loaiHinh { get; set; }
        public List<AnswerSaveVM> baiLam { get; set; }
    }
}
