﻿using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsAcademyStudentSvMuonHocBaGoc
    {
        public long Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public int? Status { get; set; }
        public string LyDo { get; set; }
        public DateTime? NgayTra { get; set; }
        public string PhanHoi { get; set; }
        public bool? Deleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public DateTime? NgayGui { get; set; }
        public DateTime? NgayHenTuNgay { get; set; }
        public DateTime? NgayHenDenNgay { get; set; }
        public string MaXacNhan { get; set; }
        public string ThoiGianMuon { get; set; }
        public DateTime? NgayTraDuKien { get; set; }
        public DateTime? NgayMuon { get; set; }
        public string Description { get; set; }
        public string Notice { get; set; }
    }
}
