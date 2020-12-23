using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Core
{
    public class UpdateClientParameterModel
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public int? Status { get; set; }
    }
}
