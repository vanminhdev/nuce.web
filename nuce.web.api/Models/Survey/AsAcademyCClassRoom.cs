using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsAcademyCClassRoom
    {
        public long Id { get; set; }
        public int? SemesterId { get; set; }
        public string Code { get; set; }
        public string GroupCode { get; set; }
        public string ClassCode { get; set; }
        public long? SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string ExamAttemptDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
