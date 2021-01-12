using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.EduData
{
    public partial class AsAcademySubjectExtend
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
    }
}
