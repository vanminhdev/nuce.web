using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Core;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Implements
{
    public class ManagerBackUpService : IManagerBackupService
    {
        private readonly NuceCoreIdentityContext _coreContext;
        private readonly IConfiguration _configuration;
        public ManagerBackUpService(NuceCoreIdentityContext coreContext, IConfiguration configuration)
        {
            _coreContext = coreContext;
            _configuration = configuration;
        }

        public async Task BackupSurveyDataBase()
        {
            var desDirectory = _configuration.GetValue<string>("FolderDataBaseBackUp");
            if(!Directory.Exists(desDirectory))
            {
                Directory.CreateDirectory(desDirectory);
            }
            var now = DateTime.Now;
            var path = $"{desDirectory}/NUCE_SURVEY_{now.Year}{now.Month}{now.Day}_{now.Hour}{now.Minute}{now.Second}.bak";
            await _coreContext.Database.ExecuteSqlRawAsync($"BACKUP DATABASE NUCE_SURVEY TO DISK = '{path}'");
            _coreContext.ManagerBackup.Add(new ManagerBackup
            {
                Id = Guid.NewGuid(),
                DatabaseName = "NUCE_SURVEY",
                Type = (int)BackupType.Backup,
                Date = DateTime.Now,
                Path = path
            });
            await _coreContext.SaveChangesAsync();
        }

        public async Task<PaginationModel<ManagerBackup>> HistoryBackup(ManagerBackupFilter filter, int skip = 0, int take = 20)
        {
            _coreContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<ManagerBackup> query = _coreContext.ManagerBackup;
            var recordsTotal = query.Count();

            if (!string.IsNullOrWhiteSpace(filter.DatabaseName))
            {
                query = query.Where(u => u.DatabaseName.Contains(filter.DatabaseName));
            }
            if (filter.Type != null)
            {
                query = query.Where(u => u.Type == filter.Type);
            }

            var recordsFiltered = query.Count();

            var querySkip = query
                .OrderBy(u => u.Id)
                .Skip(skip).Take(take);

            var data = await querySkip.ToListAsync();

            return new PaginationModel<ManagerBackup>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task RestoreSurveyDataBase()
        {
            var query = _coreContext.ManagerBackup.Where(b => b.Type == (int)BackupType.Backup);
            if (query.Count() == 0)
            {
                throw new RecordNotFoundException("Không có file backup database khảo thí");
            }
            var maxDate = await query.MaxAsync(b => b.Date);
            var backupFile = await _coreContext.ManagerBackup.FirstOrDefaultAsync(b => b.Date == maxDate);
            var path = backupFile.Path;

            if(!File.Exists(path))
            {
                throw new FileNotFoundException("Không tìm thấy file backup mới nhất database khảo thí");
            }
            await _coreContext.Database.ExecuteSqlRawAsync($"RESTORE DATABASE NUCE_SURVEY FROM DISK = '{path}'");
            _coreContext.ManagerBackup.Add(new ManagerBackup
            {
                Id = Guid.NewGuid(),
                DatabaseName = "NUCE_SURVEY",
                Type = (int)BackupType.Restore,
                Date = DateTime.Now,
                Path = path
            });
        }
    }
}
