using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Synchronization.Interfaces
{
    public interface ISyncEduDatabaseService
    {
        public Task SyncSubjects();
        public Task SyncFaculty();
    }
}
