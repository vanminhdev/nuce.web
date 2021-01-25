using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Models.Survey.Undergraduate
{
    public class ReportTotalUndergraduate
    {
        public Guid id { get; set; }
        public Guid surveyRoundId { get; set; }
        public string surveyRoundName { get; set; }
        public Guid theSurveyId { get; set; }
        public string theSurveyName { get; set; }
        public string chuyenNganh { get; set; }
        public string questionCode { get; set; }
        public string answerCode { get; set; }
        public int? total { get; set; }
        public string content { get; set; }
    }
}