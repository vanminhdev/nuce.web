using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsAcademyStudentQuaTrinhHocTap
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string ThoiGian { get; set; }
        public string TenTruong { get; set; }
        public int Count { get; set; }
    }
}
