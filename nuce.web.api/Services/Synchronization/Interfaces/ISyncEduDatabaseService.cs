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
        /// đồng bộ lớp học phần
        /// </summary>
        /// <returns></returns>
        public Task SyncClassRoom();
        public Task SyncLecturerClass();
    }
}
