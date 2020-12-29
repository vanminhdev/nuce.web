using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Core
{
    public partial class NewsCats
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Type { get; set; }
        public int Parent { get; set; }
        public int Count { get; set; }
        public int? Status { get; set; }
        public string Role { get; set; }
        public bool? OnMenu { get; set; }
        public bool? DivideAfter { get; set; }
        public bool? AllowChildren { get; set; }
    }
}
