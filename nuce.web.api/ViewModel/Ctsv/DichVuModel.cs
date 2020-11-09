﻿using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Ctsv
{
    public class DichVuModel
    {
        public int Type { get; set; }
        public string LyDo { get; set; }
        public string KyLuat { get; set; }
        public string ThuocDien { get; set; }
        public string ThuocDoiTuong { get; set; }
        public string DonVi { get; set; }
        public string DenGap { get; set; }
        public string VeViec { get; set; }
        public string MaXacNhan { get; set; }
        public string PhanHoi { get; set; }
    }

    public class AllTypeDichVuModel
    {
        public string TenDichVu { get; set; }
        public string LinkDichVu { get; set; }
        public int TongSo { get; set; }
        public int MoiGui { get; set; }
        public int DaXuLy { get; set; }
        public int DangXuLy { get; set; }
        public int Stt { get; set; }
    }

    public class QuanLyDichVuDetailModel
    {
        public int Type { get; set; }
        public string SearchText { get; set; }
        public int DayRange { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
    }

    public class QuanLyDichVuDetailResponse
    {
        public object DichVu { get; set; }
        public AsAcademyStudent Student { get; set; }
    }

    public class UpdateRequestStatusModel
    {
        public int Type { get; set; }
        public int RequestID { get; set; }
        public string PhanHoi { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public bool AutoUpdateNgayHen { get; set; }
        public int Status { get; set; }
    }

    public class GetAllForAdminResponseRepo<T>
    {
        public IQueryable<T> FinalData { get; set; }
        public IQueryable<T> BeforeFilteredData { get; set; }
    }
}
