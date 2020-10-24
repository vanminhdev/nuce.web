using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsNewsCats
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Display { get; set; }
        public string Des { get; set; }
        public int Type { get; set; }
        public int Parent { get; set; }
        public int Count { get; set; }
        public int Status { get; set; }
    }
}
