using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyGraduateBaiKhaoSatSinhVien
    {
        public Guid Id { get; set; }
        public Guid BaiKhaoSatId { get; set; }
        public string StudentCode { get; set; }
        public string Nganh { get; set; }
        public string ChuyenNganh { get; set; }
        public string DeThi { get; set; }
        public string BaiLam { get; set; }
        public DateTime NgayGioBatDau { get; set; }
        public DateTime NgayGioNopBai { get; set; }
        public string LoaiHinh { get; set; }
        public int Status { get; set; }
        public string LogIp { get; set; }
    }
}
