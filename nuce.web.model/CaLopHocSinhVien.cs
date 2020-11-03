using System;

namespace nuce.web.model
{
   public class KiThiLopHocSinhVien
    {
        public int BoDeID { get; set; }
        public int DeThiID { get; set; }
        public int KiThi_LopHoc_SinhVien { get; set; }
        public int TongThoiGianThi { get; set; }
        public int TongThoiGianConLai { get; set; }
        public int Status { get; set; }
        public int StatusKiThi { get; set; }
        public int LoaiKiThi { get; set; }
        public string TenBlockHoc { get; set; }
        public string TenKiThi { get; set; }
        public string TenMonHoc { get; set; }
        public string NoiDungDeThi { get; set; }
        public string DapAn { get; set; }
        public string BaiLam { get; set; }
        public string MaDe { get; set; }
        public DateTime NgayGioBatDau { get; set; }
        public float Diem { get; set; }
        public string Mota { get; set; }
        public string LecturerCode { get; set; }
        public string LecturerName { get; set; }
        public string ClassRoomCode { get; set; }
        public string SubjectCode { get; set; }
        //public string SubjectName { get; set; }
        public int SubjectType { get; set; }
        public string DepartmentCode { get; set; }
    }
}
