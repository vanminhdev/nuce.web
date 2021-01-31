using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.EduData
{
    public partial class AsAcademyStudentClassRoom
    {
        public long Id { get; set; }
        public string ClassRoomCode { get; set; }
        public string StudentCode { get; set; }
        public string Nhhk { get; set; }
    }
}
