using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.EduData
{
    public partial class AsAcademyStudentClassRoom
    {
        public long Id { get; set; }
        public int? SemesterId { get; set; }
        public long? ClassRoomId { get; set; }
        public long? StudentId { get; set; }
        public string StudentCode { get; set; }
    }
}
