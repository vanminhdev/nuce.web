using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Status
{
    public partial class AsAcademySemester
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
        public bool IsCurrent { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}
