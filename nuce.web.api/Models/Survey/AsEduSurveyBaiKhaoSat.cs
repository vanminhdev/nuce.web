using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyBaiKhaoSat
    {
        public Guid Id { get; set; }
        public Guid DotKhaoSatId { get; set; }
        public Guid DeThiId { get; set; }
        public string NoiDungDeThi { get; set; }
        public string DapAn { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string MoTa { get; set; }
        public string GhiChu { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }
}
