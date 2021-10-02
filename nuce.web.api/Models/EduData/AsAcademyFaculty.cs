using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.EduData
{
    public partial class AsAcademyFaculty
    {
        public int Id { get; set; }
        public int? SemesterId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int COrder { get; set; }
        public string Email { get; set; }
        public int? Order { get; set; }
    }
}
