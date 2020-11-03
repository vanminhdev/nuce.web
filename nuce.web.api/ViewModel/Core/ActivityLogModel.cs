using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Core
{
    public class ActivityLogModel
    {
        public string Username { get; set; }
        public int? Status { get; set; }
        public string LogCode { get; set; }
        public string LogMessage { get; set; }
    }
}
