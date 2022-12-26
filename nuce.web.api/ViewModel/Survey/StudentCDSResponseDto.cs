using System;

namespace nuce.web.api.ViewModel.Survey
{
    public class StudentCDSResponseDto
    {
        public int code { get; set; }
        public string message { get; set; }
        public StudentCDS data { get; set; }
    }

    public class StudentCDS
    {
        public string maSinhVien { get; set; }
        public string hoDem { get; set; }
        public string ten { get; set; }
        public object ngaySinh { get; set; }
        public string ngaySinh2 { get; set; }
        public string soDienThoai { get; set; }
        public string email { get; set; }
        public string tenNganh { get; set; }
        public string tenNghe { get; set; }
        public string tenLoaiHinhDt { get; set; }
        public string tenHeDaoTao { get; set; }
        public string tenLop { get; set; }
        public string maLopChu { get; set; }
        public string gioiTinh { get; set; }
        public string tenPhongBan { get; set; }
        public string emailNhaTruong { get; set; }
        public string msNhaTruong { get; set; }
    }

}
