using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Base
{
    public class DataTableRequest
    {
        public List<DataTableColumn> Columns { get; set; }
        public int Draw { get; set; }

        [Range(0, int.MaxValue)]
        public int Start { get; set; }

        [Range(1, int.MaxValue)]
        public int Length { get; set; }

        public DataTableSearch Search { get; set; }
    }

    public class DataTableSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class DataTableColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DataTableSearch Search { get; set; }
    }
}
