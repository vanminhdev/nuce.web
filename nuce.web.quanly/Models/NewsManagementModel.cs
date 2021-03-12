using nuce.web.quanly.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Models
{
    public class NewsCatsModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? type { get; set; }
        public int? parent { get; set; }
        public int? count { get; set; }
        public int? status { get; set; }
        public string role { get; set; }
        public bool? onMenu { get; set; }
        public bool? divideAfter { get; set; }
        public bool? allowChildren { get; set; }
    }

    public class NewsItemModel
    {
        public int id { get; set; }
        public int catId { get; set; }
        public string title { get; set; }
        public int? order { get; set; }
        public string avatar { get; set; }
        public string file { get; set; }
        public string description { get; set; }
        public string newContent { get; set; }
        public string newSource { get; set; }
        public int totalView { get; set; }
        public bool isTinlq { get; set; }
        public bool isComment { get; set; }
        public bool isShared { get; set; }
        public bool isHome { get; set; }
        public float? score { get; set; }
        public bool activeFlg { get; set; }
        public string entryUsername { get; set; }
        public DateTime entryDatetime { get; set; }
        public string updateUsername { get; set; }
        public DateTime updateDatetime { get; set; }
        public string metaKeyword { get; set; }
        public string metaDesciption { get; set; }
        public int? status { get; set; }
    }

    public class GetNewsItemByCatIdModel
    {
        public DataTableRequest body { get; set; }
        public int catId { get; set; }
    }

    public class CreateNewsItemModel
    {
        public int catId { get; set; }
        public string title { get; set; }
        public int? order { get; set; }
        public string description { get; set; }
        public string content { get; set; }
        public object avatar { get; set; }
        public object file { get; set; }
    }

    public class UploadFileModel
    {
        public string FileName { get; set; }
        public string Key { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }


}