using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsAcademySyncLog
    {
        public long Id { get; set; }
        public int? SemesterId { get; set; }
        public string Code { get; set; }
        public int? PageIndex { get; set; }
        public int? BatchSize { get; set; }
        public bool? IsSuccess { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
