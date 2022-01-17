using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsAcademyCStudentClassRoom
    {
        public long Id { get; set; }
        public int? SemesterId { get; set; }
        public long? ClassRoomId { get; set; }
        public string ClassRoomCode { get; set; }
        public long? StudentId { get; set; }
        public string StudentCode { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
    }
}
