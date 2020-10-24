using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsAcademyStudentTinNhan
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public int SenderId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string Receiver { get; set; }
        public string ReceiverEmail { get; set; }
        public string Content { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
        public bool? Deleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public string Title { get; set; }
    }
}
