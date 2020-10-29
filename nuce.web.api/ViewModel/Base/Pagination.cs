using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Base
{
    public class Pagination<T>
    {
        public List<T> PayLoad { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }
}
