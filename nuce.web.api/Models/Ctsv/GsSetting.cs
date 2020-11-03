using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class GsSetting
    {
        public long Id { get; set; }
        public long? Type { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool? Enabled { get; set; }
        public bool? Deleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public DateTime? LastModifiedTime { get; set; }
    }
}
