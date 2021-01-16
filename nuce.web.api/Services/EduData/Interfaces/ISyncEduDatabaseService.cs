using nuce.web.api.Models.EduData;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.EduData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.EduData.Interfaces
{
    public interface ISyncEduDatabaseService
    {
        public Task SyncFaculty();
        public Task SyncDepartment();
        public Task SyncAcademics();
        public Task SyncSubject();
        public Task SyncClass();
        public Task SyncLecturer();
        public Task SyncStudent();

        /// <summary>
        /// đồng bộ lớp môn học kỳ trước
        /// </summary>
        /// <returns></returns>
        public Task SyncLastClassRoom();

        /// <summary>
        /// đồng bộ lớp môn học kỳ hiện tại
        /// </summary>
        /// <returns></returns>
        public Task SyncCurrentClassRoom();

        /// <summary>
        /// Đồng bộ giảng viên lớp môn học kỳ trước
        /// </summary>
        /// <returns></returns>
        public Task SyncLastLecturerClassRoom();

        /// <summary>
        /// Đồng bộ giảng viên lớp môn học kỳ hiện tại
        /// </summary>
        /// <returns></returns>
        public Task SyncCurrentLecturerClassRoom();

        /// <summary>
        /// Đồng bộ sinh viên lớp học phần kỳ hiện tại
        /// </summary>
        /// <returns></returns>
        public Task<string> SyncCurrentStudentClassRoom();

        /// <summary>
        /// Đồng bộ cập nhật lớp môn học hiện tại, cập nhật trường FromDate và EndDate
        /// Tác động trên bảng C ClassRoom
        /// </summary>
        /// <returns></returns>
        public Task<string> SyncUpdateFromDateEndDateCurrentClassRoom();

        public Task<CountData> GetCountEduData();

        /// <summary>
        /// Đồng bộ cập nhật lớp môn học hiện tại, cập nhật trường FromDate và EndDate
        /// Tác động trên bảng Edu QA Week
        /// </summary>
        /// <returns></returns>
        public Task<string> SyncQAWeek();

        public Task<PaginationModel<AsAcademyCClassRoom>> GetCurrentClassRoom(ClassRoomFilter filter, int skip = 0, int take = 20);
        
        public Task<PaginationModel<AsAcademyCLecturerClassRoom>> GetCurrentLecturerClassRoom(LecturerClassRoomFilter filter, int skip = 0, int take = 20);
        
        public Task<PaginationModel<AsAcademyCStudentClassRoom>> GetCurrentStudentClassRoom(StudentClassRoomFilter filter, int skip = 0, int take = 20);


        public Task<PaginationModel<AsAcademyClassRoom>> GetLastClassRoom(ClassRoomFilter filter, int skip, int take);

        public Task<PaginationModel<AsAcademyLecturerClassRoom>> GetLastLecturerClassRoom(LecturerClassRoomFilter filter, int skip, int take);

        public Task<PaginationModel<AsAcademyStudentClassRoom>> GetLastStudentClassRoom(StudentClassRoomFilter filter, int skip, int take);


        public Task<List<AsAcademyFaculty>> GetAllFaculties();

        public Task<List<AsAcademyDepartment>> GetAllDepartments();

        public Task<PaginationModel<AsAcademyAcademics>> GetAcademics(AcademicsFilter filter, int skip = 0, int take = 20);

        public Task<PaginationModel<AsAcademySubject>> GetSubject(SubjectFilter filter, int skip = 0, int take = 20);

        public Task<PaginationModel<AsAcademyClass>> GetClass(ClassFilter filter, int skip, int take);

        public Task<PaginationModel<AsAcademyLecturer>> GetLecturer(LecturerFilter filter, int skip, int take);
        
        public Task<PaginationModel<AsAcademyStudent>> GetStudent(StudentFilter filter, int skip, int take);

        public Task TruncateTable(string tableName);
    }
}
