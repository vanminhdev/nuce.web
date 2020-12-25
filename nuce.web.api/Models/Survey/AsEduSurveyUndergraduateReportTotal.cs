using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyUndergraduateReportTotal
    {
        public Guid Id { get; set; }
        public Guid SurveyRoundId { get; set; }
        public Guid TheSurveyId { get; set; }
        public string QuestionCode { get; set; }
        public string AnswerCode { get; set; }
        public int? Total { get; set; }
        public string Content { get; set; }
    }
}
