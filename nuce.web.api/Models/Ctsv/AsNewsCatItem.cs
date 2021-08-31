using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsNewsCatItem
    {
        public int Id { get; set; }
        public int? CatId { get; set; }
        public string CatName { get; set; }
        public int? ItemId { get; set; }
        public string ItemTitle { get; set; }
    }
}
