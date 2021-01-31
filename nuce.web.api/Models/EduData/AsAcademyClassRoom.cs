using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.EduData
{
    public partial class AsAcademyClassRoom
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string ClassCode { get; set; }
        public string SubjectCode { get; set; }
        public string Nhhk { get; set; }
    }
}
