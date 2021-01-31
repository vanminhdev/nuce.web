using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey.Normal.Statistic
{
    public class ReportTotalNormal
    {
        public Guid Id { get; set; }
        public Guid SurveyRoundId { get; set; }
        public string SurveyRoundName { get; set; }
        public Guid TheSurveyId { get; set; }
        public string TheSurveyName { get; set; }
        public string ClassRoomCode { get; set; }
        public string NHHK { get; set; }
        public string LecturerCode { get; set; }
        public string QuestionCode { get; set; }
        public string AnswerCode { get; set; }
        public int? Total { get; set; }
        public string Content { get; set; }
    }
}
