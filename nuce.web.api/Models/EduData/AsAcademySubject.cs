using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.EduData
{
    public partial class AsAcademySubject
    {
        public int Id { get; set; }
        public int? SemesterId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
    }
}
