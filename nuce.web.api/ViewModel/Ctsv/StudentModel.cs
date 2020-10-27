using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Ctsv
{
    public class StudentModel
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string DiaChiBaotin { get; set; }
        public string DiaChiNguoiNhanBaotin { get; set; }
        public string MobileBaoTin { get; set; }
        public string EmailBaoTin { get; set; }
        public string HoTenBaoTin { get; set; }
        public bool CoNoiOCuThe { get; set; }
        public string DiaChiCuThe { get; set; }
    }
}
