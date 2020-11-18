using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface IManagerBackupService
    {
        public Task BackupSurveyDataBase();

        public Task RestoreSurveyDataBase();
    }
}
