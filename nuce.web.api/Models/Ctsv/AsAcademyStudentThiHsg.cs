using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Ctsv
{
    public partial class AsAcademyStudentThiHsg
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string CapThi { get; set; }
        public string MonThi { get; set; }
        public string DatGiai { get; set; }
        public int Count { get; set; }
    }
}
