using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsAcademyStudentSvThietLapThamSoDichVu
    {
        public long Id { get; set; }
        public long? DichVuId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
