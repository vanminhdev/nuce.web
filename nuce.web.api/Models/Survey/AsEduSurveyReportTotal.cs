using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyReportTotal
    {
        public Guid Id { get; set; }
        public Guid SurveyRoundId { get; set; }
        public Guid TheSurveyId { get; set; }
        public string ClassRoomCode { get; set; }
        public string Nhhk { get; set; }
        public string LecturerCode { get; set; }
        public string QuestionCode { get; set; }
        public string AnswerCode { get; set; }
        public int? Total { get; set; }
        public string Content { get; set; }
    }
}
