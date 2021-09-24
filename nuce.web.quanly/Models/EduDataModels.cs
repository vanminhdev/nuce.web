using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Models
{
    public class HistoryBackup
    {
        public Guid id { get; set; }
        public string databaseName { get; set; }
        public int type { get; set; }
        public DateTime date { get; set; }
    }

    public class AsAcademyFaculty
    {
        public int id { get; set; }
        public int? semesterId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int cOrder { get; set; }
    }

    public class AsAcademyDepartment
    {
        public int id { get; set; }
        public int? semesterId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public long? facultyId { get; set; }
        public string facultyCode { get; set; }
        public string chefsDepartmentCode { get; set; }
    }

    public partial class AsAcademyAcademics
    {
        public int id { get; set; }
        public int? semesterId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }

    public class AsAcademySubject
    {
        public int id { get; set; }
        public int? semesterId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public long? departmentId { get; set; }
        public string departmentCode { get; set; }
    }

    public class AsAcademyClass
    {
        public long id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public long? facultyId { get; set; }
        public string facultyCode { get; set; }
        public long? academicsId { get; set; }
        public string academicsCode { get; set; }
        public string schoolYear { get; set; }
    }

    public class AsAcademyLecturer
    {
        public long id { get; set; }
        public string code { get; set; }
        public string fullName { get; set; }
        public long? departmentId { get; set; }
        public string departmentCode { get; set; }
        public string dateOfBirth { get; set; }
        public string nameOrder { get; set; }
        public string email { get; set; }
    }

    public class AsAcademyStudent
    {
        public long id { get; set; }
        public string code { get; set; }
        public string fullName { get; set; }
        public long? classId { get; set; }
        public string classCode { get; set; }
        public string dateOfBirth { get; set; }
        public string birthPlace { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public Guid? keyAuthorize { get; set; }
        public int? status { get; set; }
    }

    public class AsAcademyClassRoom
    {
        public long id { get; set; }
        public int? semesterId { get; set; }
        public string code { get; set; }
        public string groupCode { get; set; }
        public string classCode { get; set; }
        public long? subjectId { get; set; }
        public string subjectCode { get; set; }
        public string examAttemptDate { get; set; }
    }

    public class AsAcademyLecturerClassRoom
    {
        public long id { get; set; }
        public int? semesterId { get; set; }
        public long? classRoomId { get; set; }
        public string classRoomCode { get; set; }
        public long? lecturerId { get; set; }
        public string lecturerCode { get; set; }
    }

    public class AsAcademyStudentClassRoom
    {
        public long id { get; set; }
        public int? semesterId { get; set; }
        public string classRoomCode { get; set; }
        public string studentCode { get; set; }
    }

    public partial class AsAcademyCClassRoom
    {
        public long id { get; set; }
        public int? semesterId { get; set; }
        public string code { get; set; }
        public string groupCode { get; set; }
        public string classCode { get; set; }
        public long? subjectId { get; set; }
        public string subjectCode { get; set; }
        public string examAttemptDate { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? endDate { get; set; }
    }

    public partial class AsAcademyCLecturerClassRoom
    {
        public long id { get; set; }
        public int? semesterId { get; set; }
        public long? classRoomId { get; set; }
        public string classRoomCode { get; set; }
        public long? lecturerId { get; set; }
        public string lecturerCode { get; set; }
        public int? template { get; set; }
    }

    public partial class AsAcademyCStudentClassRoom
    {
        public long id { get; set; }
        public int? semesterId { get; set; }
        public long? classRoomId { get; set; }
        public long? studentId { get; set; }
        public string studentCode { get; set; }
        public int? type { get; set; }
        public int? status { get; set; }
    }
}