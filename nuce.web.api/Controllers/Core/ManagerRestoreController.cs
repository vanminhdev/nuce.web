using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
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
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Core
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AppAuthorize(RoleNames.Admin, RoleNames.KhaoThi)]
    public class ManagerRestoreController : ControllerBase
    {
        private readonly IManagerRestoreService _managerBackupService;
        private readonly ILogger<ManagerRestoreController> _logger;
        private readonly ILogService _logService;
        private readonly IUserService _userService;

        public ManagerRestoreController(ILogger<ManagerRestoreController> logger, ILogService logService, IManagerRestoreService managerBackupService, IUserService _userService)
        {
            _logger = logger;
            _logService = logService;
            _managerBackupService = managerBackupService;
            this._userService = _userService;
        }

        [HttpPost]
        [AppAuthorize(RoleNames.Admin, RoleNames.KhaoThi_Survey_Normal, RoleNames.KhaoThi_Survey_Undergraduate, RoleNames.KhaoThi_Survey_Graduate)]
        [Route("{type}")]
        public async Task<IActionResult> GetHistoryBackup(BackupTypes.Survey type, [FromBody] DataTableRequest request)
        {
            var userRoles = _userService.GetClaimListByKey(ClaimTypes.Role);
            if (!_managerBackupService.isBackupTypeValid(userRoles, type))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Bạn không đủ quyền" });
            }
            var filter = new ManagerBackupFilter();
            if (request.Columns != null)
            {
                filter.DatabaseName = request.Columns.FirstOrDefault(c => c.Data == "databaseName" || c.Name == "databaseName")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _managerBackupService.HistoryBackup(type, filter, skip, take);
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
        [Route("{type}")]
        [AppAuthorize(RoleNames.Admin, RoleNames.KhaoThi_Survey_Normal, RoleNames.KhaoThi_Survey_Undergraduate, RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<IActionResult> BackupDatabaseSurvey(BackupTypes.Survey type)
        {
            var userRoles = _userService.GetClaimListByKey(ClaimTypes.Role);
            if (!_managerBackupService.isBackupTypeValid(userRoles, type))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Bạn không đủ quyền" });
            }
            try
            {
                await _managerBackupService.BackupSurveyDataBase(type);
                await _logService.WriteLog(new ActivityLogModel
                {
                    Username = HttpContext.User.Identity.Name,
                    LogCode = ActivityLogParameters.CODE_BACKUP_DATABASE,
                    LogMessage = $"Backup database NUCE_SURVEY {type.ToString()}"
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
        public async Task<IActionResult> RestoreDatabaseSurvey([Required] Guid? id)
        {
            try
            {
                await _managerBackupService.RestoreSurveyDataBase(id.Value);
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

        [HttpGet]
        public async Task<IActionResult> DownloadFileBackupDatabaseSurvey([Required] Guid? id)
        {
            try
            {
                var file = await _managerBackupService.DownloadFileBackupSurveyDataBase(id.Value);
                await _logService.WriteLog(new ActivityLogModel
                {
                    Username = HttpContext.User.Identity.Name,
                    LogCode = ActivityLogParameters.CODE_DOWNLOAD_FILE_BACKUP,
                    LogMessage = $"Download file backup database NUCE_SURVEY"
                });
                return File(file, MediaTypeNames.Application.Octet);
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
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteHistoryBackupDatabaseSurvey([Required] Guid? id)
        {
            try
            {
                await _managerBackupService.DeleteHistoryBackupDatabaseSurvey(id.Value);
                await _logService.WriteLog(new ActivityLogModel
                {
                    Username = HttpContext.User.Identity.Name,
                    LogCode = ActivityLogParameters.CODE_DELETE_FILE_BACKUP,
                    LogMessage = $"Delete file backup database NUCE_SURVEY"
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
