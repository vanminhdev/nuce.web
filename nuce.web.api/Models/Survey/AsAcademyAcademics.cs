using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsAcademyAcademics
    {
        public int Id { get; set; }
        public int? SemesterId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
