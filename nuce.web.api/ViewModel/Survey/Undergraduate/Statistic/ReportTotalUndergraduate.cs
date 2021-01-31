using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey.Undergraduate.Statistic
{
    public class ReportTotalUndergraduate
    {
        public Guid Id { get; set; }
        public Guid SurveyRoundId { get; set; }
        public string SurveyRoundName { get; set; }
        public Guid TheSurveyId { get; set; }
        public string TheSurveyName { get; set; }
        public string ChuyenNganh { get; set; }
        public string QuestionCode { get; set; }
        public string AnswerCode { get; set; }
        public int? Total { get; set; }
        public string Content { get; set; }
    }
}
