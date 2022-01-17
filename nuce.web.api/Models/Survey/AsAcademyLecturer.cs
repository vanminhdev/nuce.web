using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsAcademyLecturer
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public long? DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DateOfBirth { get; set; }
        public string NameOrder { get; set; }
        public string Email { get; set; }
    }
}
