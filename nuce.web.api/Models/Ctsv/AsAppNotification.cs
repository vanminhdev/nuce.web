using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsAppNotification
    {
        public long Id { get; set; }
        public int? TypeId { get; set; }
        public string TypeCode { get; set; }
        public int? StudentId { get; set; }
        public string StudentCode { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LasdModifiedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public bool? Deleted { get; set; }
    }
}
