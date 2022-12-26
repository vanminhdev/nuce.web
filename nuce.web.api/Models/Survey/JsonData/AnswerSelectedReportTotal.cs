using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Models.Survey.JsonData
{
    public class AnswerSelectedReportTotal
    {
        public Guid TheSurveyId { get; set; }

        public string QuestionCode { get; set; }
        public string QuestionContent { get; set; }

        public string AnswerCode { get; set; }

        public int? Total { get; set; }

        public string Content { get; set; }
        public string StudentCode { get; set; }
        public DateTime? NgayBatDauLamBai { get; set; }
    }
}
