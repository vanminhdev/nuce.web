using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyBaiKhaoSatSinhVien
    {
        public Guid Id { get; set; }
        public Guid BaiKhaoSatId { get; set; }
        public long SinhVienId { get; set; }
        public string LecturerCode { get; set; }
        public string LecturerName { get; set; }
        public string ClassRoomCode { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int SubjectType { get; set; }
        public string DepartmentCode { get; set; }
        public string BaiLam { get; set; }
        public DateTime NgayGioBatDau { get; set; }
        public DateTime NgayGioNopBai { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public string LogIp { get; set; }
    }
}
