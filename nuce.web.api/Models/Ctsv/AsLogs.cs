using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsLogs
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string UserCode { get; set; }
        public int? Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
