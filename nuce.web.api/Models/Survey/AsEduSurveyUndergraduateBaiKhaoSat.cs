using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyUndergraduateBaiKhaoSat
    {
        public Guid Id { get; set; }
        public Guid DotKhaoSatId { get; set; }
        public Guid DeThiId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int? Status { get; set; }
    }
}
