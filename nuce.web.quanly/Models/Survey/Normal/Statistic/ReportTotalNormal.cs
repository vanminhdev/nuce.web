using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Models.Survey.Normal.Statistic
{
    public class ReportTotalNormal
    {
        public string id { get; set; }
        public string surveyRoundId { get; set; }
        public string surveyRoundName { get; set; }
        public string theSurveyId { get; set; }
        public string theSurveyName { get; set; }
        public string classRoomCode { get; set; }
        public string lecturerCode { get; set; }
        public string questionCode { get; set; }
        public string answerCode { get; set; }
        public int? total { get; set; }
        public string content { get; set; }
    }
}