using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Interfaces
{
    public interface ISyncEduDataCtsv
    {
        public Task SyncFaculty();
        public Task SyncAcademics();
        public Task SyncClass();
        public Task SyncStudent();
    }
}
