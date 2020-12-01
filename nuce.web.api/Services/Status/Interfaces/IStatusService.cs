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
        public Task<int> GetCurrentSemesterId();
        public Task<int> GetLastSemesterId();
        public Task DoNotStatusTableTask(string tableName, string messageNotFound);
        public Task DoingStatusTableTask(string tableName, string messageNotFound, string messageBusy);
        public Task DoneStatusTableTask(string tableName, string messageNotFound, string messageIfError = null);
    }
}
