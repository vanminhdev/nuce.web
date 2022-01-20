using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsAcademyClass
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? FacultyId { get; set; }
        public string FacultyCode { get; set; }
        public long? AcademicsId { get; set; }
        public string AcademicsCode { get; set; }
        public string SchoolYear { get; set; }
    }
}
