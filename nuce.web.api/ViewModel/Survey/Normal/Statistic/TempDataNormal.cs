using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey.Normal.Statistic
{
    public class TotalFaculty
    {
        public string FacultyCode { get; set; }
        public string FacultyName { get; set; }
        public int TotalDaLam { get; set; }
        public int TotalChuaLam { get; set; }
        public int TotalSinhVien { get; set; }
        public string Percent { get; set; }
    }

    public class TempDataNormal
    {
        public DateTime ThoiGianKetThuc { get; set; }
        public DateTime NgayHienTai { get; set; }
        public int SoSVKhaoSat { get; set; }
        public string ChiemTiLe { get; set; }
        public List<TotalFaculty> TongHopKhoa { get; set; }
    }
}
