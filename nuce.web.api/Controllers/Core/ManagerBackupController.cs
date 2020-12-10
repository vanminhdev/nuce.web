﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

namespace nuce.web.api.Controllers.Core
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ManagerBackupController : ControllerBase
    {
        private readonly IManagerBackupService _managerBackupService;
        private readonly ILogger<ManagerBackupController> _logger;
        private readonly ILogService _logService;

        public ManagerBackupController(ILogger<ManagerBackupController> logger, ILogService logService, IManagerBackupService managerBackupService)
        {
            _logger = logger;
            _logService = logService;
            _managerBackupService = managerBackupService;
        }

        [HttpPost]
        public async Task<IActionResult> GetHistoryBackup([FromBody] DataTableRequest request)
        {
            var filter = new ManagerBackupFilter();
            if (request.Columns != null)
            {
                filter.DatabaseName = request.Columns.FirstOrDefault(c => c.Data == "databaseName" || c.Name == "databaseName")?.Search.Value ?? null;
                var typeStr = request.Columns.FirstOrDefault(c => c.Data == "type" || c.Name == "type");
                if(typeStr != null && typeStr.Search.Value != null)
                    filter.Type = int.Parse(typeStr.Search.Value);
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _managerBackupService.HistoryBackup(filter, skip, take);
            return Ok(
                new DataTableResponse<ManagerBackup>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> BackupDatabaseSurvey()
        {
            try
            {
                await _managerBackupService.BackupSurveyDataBase();
                await _logService.WriteLog(new ActivityLogModel
                {
                    Username = HttpContext.User.Identity.Name,
                    LogCode = ActivityLogParameters.CODE_BACKUP_DATABASE,
                    LogMessage = $"Backup database NUCE_SURVEY"
                });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không ghi được log backup db", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage );
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Có lỗi xảy ra", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> RestoreDatabaseSurvey()
        {
            try
            {
                await _managerBackupService.RestoreSurveyDataBase();
                await _logService.WriteLog(new ActivityLogModel
                {
                    Username = HttpContext.User.Identity.Name,
                    LogCode = ActivityLogParameters.CODE_RESTORE_DATABASE,
                    LogMessage = $"Restore database NUCE_SURVEY"
                });
            }
            catch (FileNotFoundException e)
            {
                _logger.LogError(e, "Không tìm thấy file backup database khảo thí");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không ghi được log restore db", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Có lỗi xảy ra", detailMessage = mainMessage });
            }
            return Ok();
        }
    }
}
