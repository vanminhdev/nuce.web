using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsAcademyStudentGiaDinh
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string MoiQuanHe { get; set; }
        public string HoVaTen { get; set; }
        public string NamSinh { get; set; }
        public string QuocTich { get; set; }
        public string DanToc { get; set; }
        public string TonGiao { get; set; }
        public string NgheNghiep { get; set; }
        public string ChucVu { get; set; }
        public string NoiCongTac { get; set; }
        public string NoiOhienNay { get; set; }
        public int Count { get; set; }
    }
}
