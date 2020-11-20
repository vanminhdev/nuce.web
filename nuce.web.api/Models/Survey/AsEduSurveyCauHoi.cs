using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyCauHoi
    {
        public Guid Id { get; set; }
        public int BoCauHoiId { get; set; }
        public int DoKhoId { get; set; }
        public string Code { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string MediaUrl { get; set; }
        public string MediaSetting { get; set; }
        public double? Mark { get; set; }
        public double? MarkFail { get; set; }
        public bool? IsOption { get; set; }
        public int? PartId { get; set; }
        public string Explain { get; set; }
        public int? ParentQuestionId { get; set; }
        public int? NoAnswerOnRow { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public int? TimeVisibleAnswer { get; set; }
        public int? InsertedUser { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Order { get; set; }
        public int Level { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public string CheckImport { get; set; }
    }
}
