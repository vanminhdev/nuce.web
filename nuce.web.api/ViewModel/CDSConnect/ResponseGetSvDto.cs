using System;
using System.Text.Json.Serialization;

namespace nuce.web.api.ViewModel.CDSConnect
{

    public class ResponseGetSvDto : BaseResponseCDSConnect
    {
        [JsonPropertyName("data")]
        public ViewGetSvDto Data { get; set; }
    }

    public class ViewGetSvDto
    {
        [JsonPropertyName("maSinhVien")]
        public string MaSinhVien { get; set; }

        [JsonPropertyName("hoDem")]
        public string HoDem { get; set; }

        [JsonPropertyName("ten")]
        public string Ten { get; set; }

        [JsonPropertyName("ngaySinh2")]
        public string NgaySinh2 { get; set; }

        [JsonPropertyName("noiSinh")]
        public int? NoiSinh { get; set; }

        [JsonPropertyName("soDienThoai")]
        public string SoDienThoai { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("tenHkttTinh")]
        public string TenHkttTinh { get; set; }

        [JsonPropertyName("tenHkttHuyen")]
        public string TenHkttHuyen { get; set; }

        [JsonPropertyName("tenHkttPhuongXa")]
        public string TenHkttPhuongXa { get; set; }

        [JsonPropertyName("hkttSoNha")]
        public string HkttSoNha { get; set; }

        [JsonPropertyName("tenDanToc")]
        public string TenDanToc { get; set; }

        [JsonPropertyName("tenTonGiao")]
        public string TenTonGiao { get; set; }

        [JsonPropertyName("noiCapCmnd")]
        public string NoiCapCmnd { get; set; }

        [JsonPropertyName("soCmnd")]
        public string SoCmnd { get; set; }

        [JsonPropertyName("ngayCapCmnd")]
        public DateTime? NgayCapCmnd { get; set; }

        [JsonPropertyName("tenNganh")]
        public string TenNganh { get; set; }

        [JsonPropertyName("tenNghe")]
        public string TenNghe { get; set; }

        [JsonPropertyName("tenLoaiHinhDt")]
        public string TenLoaiHinhDt { get; set; }

        [JsonPropertyName("tenHeDaoTao")]
        public string TenHeDaoTao { get; set; }

        [JsonPropertyName("tenLop")]
        public string TenLop { get; set; }

        [JsonPropertyName("maLopChu")]
        public string MaLopChu { get; set; }

        [JsonPropertyName("gioiTinh")]
        public string GioiTinh { get; set; }

        [JsonPropertyName("tenPhongBan")]
        public string TenPhongBan { get; set; }

        [JsonPropertyName("trangThai")]
        public int? TrangThai { get; set; }

        public string NienKhoa { get; set; }
        public string DiaChiCuThe { get; set; }
        public string EmailNhaTruong { get; set; }
        public string File1 { get; set; }

        
    }

}
