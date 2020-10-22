using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyCauTrucDe
    {
        public Guid Id { get; set; }
        public Guid DeThi { get; set; }
        public Guid CauHoiId { get; set; }
        public int Count { get; set; }
    }
}
