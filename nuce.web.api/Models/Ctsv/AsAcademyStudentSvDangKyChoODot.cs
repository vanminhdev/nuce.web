using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsAcademyStudentSvDangKyChoODot
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
    }
}
