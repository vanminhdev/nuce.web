using System;
using System.Text.Json.Serialization;

namespace nuce.web.api.ViewModel.CDSConnect
{

    public class ResponseValidateSvLoginDto : BaseResponseCDSConnect
    {
        [JsonPropertyName("data")]
        public ViewSvDto Data { get; set; }
    }

    public class ViewSvDto
    {
        [JsonPropertyName("maSinhVien")]
        public string MaSinhVien { get; set; }

        [JsonPropertyName("hoDem")]
        public string HoDem { get; set; }

        [JsonPropertyName("ten")]
        public string Ten { get; set; }

        [JsonPropertyName("gioiTinh")]
        public bool GioiTinh { get; set; }

        [JsonPropertyName("ngaySinh")]
        public DateTime? NgaySinh { get; set; }

        [JsonPropertyName("ngaySinh2")]
        public string NgaySinh2 { get; set; }

        [JsonPropertyName("noiSinh")]
        public int? NoiSinh { get; set; }

        [JsonPropertyName("soDienThoai")]
        public string SoDienThoai { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
