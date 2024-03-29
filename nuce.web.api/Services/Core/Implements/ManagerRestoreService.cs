﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Core;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core;
using nuce.web.shared;
using nuce.web.shared.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Implements
{
    public class ManagerRestoreService : IManagerRestoreService
    {
        private readonly NuceCoreIdentityContext _coreContext;
        private readonly IConfiguration _configuration;
        public ManagerRestoreService(NuceCoreIdentityContext coreContext, IConfiguration configuration)
        {
            _coreContext = coreContext;
            _configuration = configuration;
        }

        public async Task BackupSurveyDataBase(BackupTypes.Survey type)
        {
            var desDirectory = _configuration.GetValue<string>("FolderDataBaseBackup");
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
                Date = DateTime.Now,
                Path = path,
                Type = (int)type
            });
            await _coreContext.SaveChangesAsync();
        }
        /// <summary>
        /// Lấy lịch sử backup theo loại
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filter"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<PaginationModel<ManagerBackup>> HistoryBackup(BackupTypes.Survey type, ManagerBackupFilter filter, int skip = 0, int take = 20)
        {
            _coreContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            int backupType = (int)type;
            IQueryable<ManagerBackup> query = _coreContext.ManagerBackup;
            var recordsTotal = query.Count();

            query = query.Where(u => u.Type == backupType);

            if (!string.IsNullOrWhiteSpace(filter.DatabaseName))
            {
                query = query.Where(u => u.DatabaseName.Contains(filter.DatabaseName) && u.Type == backupType);
            }

            var recordsFiltered = query.Count();

            var querySkip = query
                .OrderByDescending(u => u.Date)
                .Skip(skip).Take(take);

            var data = await querySkip.ToListAsync();

            return new PaginationModel<ManagerBackup>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task RestoreSurveyDataBase(Guid idBackup)
        {
            var historyBackup = await _coreContext.ManagerBackup.FirstOrDefaultAsync(o => o.Id == idBackup);
            if (historyBackup == null)
            {
                throw new RecordNotFoundException("Không tìm thấy lịch sử sao lưu");
            }

            var path = historyBackup.Path;

            if(!File.Exists(path))
            {
                throw new FileNotFoundException("File sao lưu không còn tồn tại");
            }

            await _coreContext.Database.ExecuteSqlRawAsync($"RESTORE DATABASE NUCE_SURVEY FROM DISK = '{path}'");
        }
        /// <summary>
        /// Check role của user có tương ứng loại backup không
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool isBackupTypeValid(List<string> userRoles, BackupTypes.Survey type)
        {
            switch (type)
            {
                case BackupTypes.Survey.Normal:
                    return userRoles.Contains(RoleNames.KhaoThi_Survey_Normal);
                case BackupTypes.Survey.Undergraduate:
                    return userRoles.Contains(RoleNames.KhaoThi_Survey_Undergraduate);
                case BackupTypes.Survey.Graduate:
                    return userRoles.Contains(RoleNames.KhaoThi_Survey_Graduate);
                default:
                    return false;
            }
        }

        public async Task<byte[]> DownloadFileBackupSurveyDataBase(Guid idBackup)
        {
            var historyBackup = await _coreContext.ManagerBackup.FirstOrDefaultAsync(o => o.Id == idBackup);
            if (historyBackup == null)
            {
                throw new RecordNotFoundException("Không tìm thấy lịch sử sao lưu");
            }

            var path = historyBackup.Path;

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File sao lưu không còn tồn tại");
            }

            return await File.ReadAllBytesAsync(path);
        }

        public async Task DeleteHistoryBackupDatabaseSurvey(Guid idBackup)
        {
            var historyBackup = await _coreContext.ManagerBackup.FirstOrDefaultAsync(o => o.Id == idBackup);
            if (historyBackup == null)
            {
                throw new RecordNotFoundException("Không tìm thấy lịch sử sao lưu");
            }

            _coreContext.ManagerBackup.Remove(historyBackup);

            var path = historyBackup.Path;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            await _coreContext.SaveChangesAsync();
        }
    }
}
