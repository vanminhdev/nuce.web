using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Models.Survey.Undergraduate
{
    public class UndergraduateStudent
    {
        public Guid id { get; set; }
        public string dottotnghiep { get; set; }
        public string sovaoso { get; set; }
        public string masv { get; set; }
        public string noisiti { get; set; }
        public string tbcht { get; set; }
        public string xeploai { get; set; }
        public string soqdtn { get; set; }
        public DateTime? ngayraqd { get; set; }
        public string sohieuba { get; set; }
        public string tinh { get; set; }
        public string truong { get; set; }
        public string gioitinh { get; set; }
        public DateTime? ngaysinh { get; set; }
        public string tkhau { get; set; }
        public string lop12 { get; set; }
        public string namtn { get; set; }
        public string sobaodanh { get; set; }
        public string tcong { get; set; }
        public string ghichuThi { get; set; }
        public string lopqd { get; set; }
        public string k { get; set; }
        public string dtoc { get; set; }
        public string quoctich { get; set; }
        public string bangclc { get; set; }
        public string manganh { get; set; }
        public string tenchnga { get; set; }
        public string tennganh { get; set; }
        public string hedaotao { get; set; }
        public string khoahoc { get; set; }
        public string tensinhvien { get; set; }
        public string email { get; set; }
        public string email1 { get; set; }
        public string email2 { get; set; }
        public string mobile { get; set; }
        public string mobile1 { get; set; }
        public string mobile2 { get; set; }
        public string thongtinthem { get; set; }
        public string thongtinthem1 { get; set; }
        public Guid dotKhaoSatId { get; set; }
        public string tenDotKhaoSat { get; set; }
        public string checksum { get; set; }
        public string exMasv { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public string makhoa { get; set; }
        public string malop { get; set; }
        public int? nguoiphatbang { get; set; }
        public string ghichuphatbang { get; set; }
        public int? cnOrder { get; set; }

        public int surveyStudentStatus { get; set; }
    }
}