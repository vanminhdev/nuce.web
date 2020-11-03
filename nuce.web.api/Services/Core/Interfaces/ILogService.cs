using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface ILogService
    {
        public Task WriteLog(ActivityLogModel log);
    }
}
