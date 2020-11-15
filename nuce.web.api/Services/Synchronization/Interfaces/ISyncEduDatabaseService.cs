using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Synchronization.Interfaces
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
        /// Đồng bộ sinh viên lớp học phần kỳ trước
        /// </summary>
        /// <returns></returns>
        public Task<string> SyncLastStudentClassRoom();

        /// <summary>
        /// Đồng bộ cập nhật lớp môn học hiện tại, cập nhật trường FromDate và EndDate
        /// Tác động trên bảng C ClassRoom
        /// </summary>
        /// <returns></returns>
        public Task<string> SyncUpdateFromDateEndDateCurrentClassRoom();

        /// <summary>
        /// Đồng bộ cập nhật lớp môn học hiện tại, cập nhật trường FromDate và EndDate
        /// Tác động trên bảng Edu QA Week
        /// </summary>
        /// <returns></returns>
        public Task<string> SyncQAWeek();
    }
}
