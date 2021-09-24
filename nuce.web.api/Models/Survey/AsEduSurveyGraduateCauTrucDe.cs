using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyGraduateCauTrucDe
    {
        public Guid Id { get; set; }
        public Guid DeThiId { get; set; }
        public Guid CauHoiId { get; set; }
        public int Order { get; set; }
    }
}
