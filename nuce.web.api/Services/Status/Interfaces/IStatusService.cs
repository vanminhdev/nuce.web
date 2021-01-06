using nuce.web.api.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Status.Interfaces
{
    public interface IStatusService
    {
        public Task<AsStatusTableTask> GetStatusTableTask(string tableName);
        public Task<AsStatusTableTask> GetStatusTableTaskNotResetMessage(string tableName);
    }
}
