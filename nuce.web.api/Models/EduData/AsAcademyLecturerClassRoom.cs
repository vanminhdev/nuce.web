using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.EduData
{
    public partial class AsAcademyLecturerClassRoom
    {
        public long Id { get; set; }
        public string ClassRoomCode { get; set; }
        public string LecturerCode { get; set; }
        public string Nhhk { get; set; }
    }
}
