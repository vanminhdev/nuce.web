using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsAcademyYear
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? FromYear { get; set; }
        public DateTime? ToYear { get; set; }
        public bool? IsCurrent { get; set; }
        public bool? Enabled { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public bool? Deleted { get; set; }
        public long? CreatedBy { get; set; }
        public long? LastModifiedBy { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}
