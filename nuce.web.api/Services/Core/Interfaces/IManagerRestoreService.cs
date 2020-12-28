using nuce.web.api.Models.Core;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface IManagerRestoreService
    {
        public Task<PaginationModel<ManagerBackup>> HistoryBackup(ManagerBackupFilter filter, int skip = 0, int take = 20);

        public Task BackupSurveyDataBase();

        public Task RestoreSurveyDataBase(Guid idBackup);
    }
}
