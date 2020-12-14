using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyUndergraduateBaiKhaoSatSinhVien
    {
        public Guid Id { get; set; }
        public Guid BaiKhaoSatId { get; set; }
        public string StudentCode { get; set; }
        public string DepartmentCode { get; set; }
        public string DeThi { get; set; }
        public string BaiLam { get; set; }
        public DateTime NgayGioBatDau { get; set; }
        public DateTime NgayGioNopBai { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public string LogIp { get; set; }
    }
}
