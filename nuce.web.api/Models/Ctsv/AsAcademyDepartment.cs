using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsAcademyDepartment
    {
        public int Id { get; set; }
        public int? SemesterId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? FacultyId { get; set; }
        public string FacultyCode { get; set; }
        public string ChefsDepartmentCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
