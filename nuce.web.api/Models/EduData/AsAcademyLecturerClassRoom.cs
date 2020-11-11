using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.EduData
{
    public partial class AsAcademyLecturerClassRoom
    {
        public long Id { get; set; }
        public int? SemesterId { get; set; }
        public long? ClassRoomId { get; set; }
        public string ClassRoomCode { get; set; }
        public long? LecturerId { get; set; }
        public string LecturerCode { get; set; }
    }
}
