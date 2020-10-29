using nuce.web.api.Models.Ctsv;
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
        public string PhuongXa { get; set; }
        public string QuanHuyen { get; set; }
        public string TinhThanhPho { get; set; }
    }
    public class FullStudentModel
    {
        public AsAcademyStudent Student { get; set; }
        public List<AsAcademyStudentGiaDinh> GiaDinh { get; set; }
        public List<AsAcademyStudentThiHsg> ThiHSG { get; set; }
        public List<AsAcademyStudentQuaTrinhHocTap> QuaTrinhHoc { get; set; }
    }
}
