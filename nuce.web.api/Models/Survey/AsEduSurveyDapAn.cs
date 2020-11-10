using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyDapAn
    {
        public Guid Id { get; set; }
        public int DapAnId { get; set; }
        public Guid CauHoiGid { get; set; }
        public int CauHoiId { get; set; }
        public int SubQuestionId { get; set; }
        public string Content { get; set; }
        public bool? IsCheck { get; set; }
        public Guid? Matched { get; set; }
        public string Matching { get; set; }
        public double? Mark { get; set; }
        public double? MarkFail { get; set; }
        public int? Order { get; set; }
        public int Status { get; set; }
        public string Explain { get; set; }
    }
}
