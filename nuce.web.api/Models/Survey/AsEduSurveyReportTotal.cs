using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyReportTotal
    {
        public Guid Id { get; set; }
        public int SemesterId { get; set; }
        public Guid CampaignId { get; set; }
        public string ClassRoomCode { get; set; }
        public string LecturerCode { get; set; }
        public string QuestionType { get; set; }
        public string QuestionCode { get; set; }
        public int? QuestionOrder { get; set; }
        public string AnswerCode { get; set; }
        public int? Total { get; set; }
        public string Content { get; set; }
    }
}
