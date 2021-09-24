using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Core
{
    public partial class NewsItems
    {
        public int Id { get; set; }
        public int? CatId { get; set; }
        public string Title { get; set; }
        public string Avatar { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public string NewContent { get; set; }
        public string NewSource { get; set; }
        public int TotalView { get; set; }
        public bool IsTinlq { get; set; }
        public bool IsComment { get; set; }
        public bool IsShared { get; set; }
        public bool IsHome { get; set; }
        public double? Score { get; set; }
        public bool ActiveFlg { get; set; }
        public string EntryUsername { get; set; }
        public DateTime EntryDatetime { get; set; }
        public string UpdateUsername { get; set; }
        public DateTime UpdateDatetime { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDesciption { get; set; }
        public int Status { get; set; }
        public int? Order { get; set; }
    }
}
