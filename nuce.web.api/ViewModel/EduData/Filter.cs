using nuce.web.api.Models.EduData;
using nuce.web.api.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.EduData
{
    public class AcademicsFilter
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class StudentFilter
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public string ClassCode { get; set; }
    }

    public class LecturerFilter
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public string DepartmentCode { get; set; }
    }

    public class ClassFilter
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FacultyCode { get; set; }
        public string AcademicsCode { get; set; }
        public string SchoolYear { get; set; }
    }

    public class SubjectFilter
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DepartmentCode { get; set; }
    }

    public class ClassRoomFilter
    {
        public string Code { get; set; }
        public string ClassCode { get; set; }
        public string SubjectCode { get; set; }
    }

    public class LecturerClassRoomFilter
    {
        public string ClassRoomCode { get; set; }
        public string LecturerCode { get; set; }
    }

    public class StudentClassRoomFilter
    {
        public string StudentCode { get; set; }
    }
}
