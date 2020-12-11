using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Core
{
    public partial class ActiviyLogs
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string UserCode { get; set; }
        public string RoleId { get; set; }
        public int? Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}