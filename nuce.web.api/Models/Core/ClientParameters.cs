using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Core
{
    public partial class ClientParameters
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public int? Status { get; set; }
        public string Value { get; set; }
        public string EntryUsername { get; set; }
        public DateTime? EntryDatetime { get; set; }
        public string UpdateUsername { get; set; }
        public DateTime? UpdateDatetime { get; set; }
        public string Role { get; set; }
    }
}
