using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Status
{
    public partial class AsStatusTableTask
    {
        public Guid Id { get; set; }
        public string TableName { get; set; }
        public int Status { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
