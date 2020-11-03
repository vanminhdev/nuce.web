using System;

namespace nuce.web.model
{
   public class CaLopHocSinhVien
    {
        public int Ca_LopHoc_SinhVienID { get; set; }
        public int Ca_LopHocID { get; set; }
        public int SinhVienID { get; set; }
        public string Mac { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public string TenMonHoc { get; set; }
        public string MaMonHoc { get; set; }
        public string TenCa { get; set; }
        public DateTime Ngay { get; set; }
        public int GioBatDau { get; set; }
        public int GioKetThuc { get; set; }
        public int PhutBatDau { get; set; }
        public int PhutKetThuc { get; set; }
    }
}
