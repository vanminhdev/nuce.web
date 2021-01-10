using nuce.web.api.Common;
using nuce.web.api.Models.Core;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core;
using nuce.web.shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface IManagerRestoreService
    {
        /// <summary>
        /// Lấy lịch sử backup theo loại
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filter"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public Task<PaginationModel<ManagerBackup>> HistoryBackup(BackupTypes.Survey type, ManagerBackupFilter filter, int skip, int take);

        public Task BackupSurveyDataBase(BackupTypes.Survey type);

        public Task RestoreSurveyDataBase(Guid idBackup);
        /// <summary>
        /// Check role của user có tương ứng loại backup không
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool isBackupTypeValid(List<string> userRoles, BackupTypes.Survey type);
    }
}
