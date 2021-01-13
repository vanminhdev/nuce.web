using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.Services.Survey.BackgroundTasks;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Normal.Statistic;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly ILogger<StatisticController> _logger;
        private readonly IAsEduSurveyReportTotalService _asEduSurveyReportTotalService;
        private readonly SurveyStatisticBackgroundTask _surveyStatisticBackgroundTask;
        private readonly IStatusService _statusService;

        public StatisticController(ILogger<StatisticController> logger, IAsEduSurveyReportTotalService asEduSurveyReportTotalService, 
            IStatusService statusService, SurveyStatisticBackgroundTask surveyStatisticBackgroundTask)
        {
            _logger = logger;
            _asEduSurveyReportTotalService = asEduSurveyReportTotalService;
            _statusService = statusService;
            _surveyStatisticBackgroundTask = surveyStatisticBackgroundTask;
        }

        #region thống kê sinh viên thường
        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> GetStatusReportTotalNormalSurveyTask()
        {
            try
            {
                var status = await _statusService.GetStatusTableTask(TableNameTask.AsEduSurveyReportTotal);
                return Ok(new { status.Status, status.IsSuccess, status.Message });
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = "Không lấy được trạng thái", detailMessage = e.Message });
            }
        }

        [HttpPost]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> ReportTotalNormalSurvey([Required] Guid? surveyRoundId)
        {
            try
            {
                await _surveyStatisticBackgroundTask.ReportTotalNormalSurvey(surveyRoundId.Value);
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = "Không thống kê được đợt khảo sát", detailMessage = e.Message });
            }
            catch (InvalidInputDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (TableBusyException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thống kê được đợt khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpPost]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> GetRawReportTotalNormalSurvey([FromBody] DataTableRequest request)
        {
            var filter = new ReportTotalNormalFilter();
            if (request.Columns != null)
            {
                
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _asEduSurveyReportTotalService.GetRawReportTotalNormalSurvey(filter, skip, take);
            return Ok(
                new DataTableResponse<ReportTotalNormal>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }
        #endregion

        #region thống kê tạm
        [HttpPut]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> GetTempDataNormalSurvey([Required] Guid? surveyRoundId)
        {
            try
            {
                await _surveyStatisticBackgroundTask.TempDataNormalSurvey(surveyRoundId.Value);
                return Ok();
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (TableBusyException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thống kê tạm được", detailMessage = mainMessage });
            }
        }

        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> GetStatusTempDataNormalSurveyTask()
        {
            try
            {
                var status = await _statusService.GetStatusTableTaskNotResetMessage(TableNameTask.TempDataNormalSurvey);
                return Ok(new { status.Status, status.IsSuccess, status.Message });
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = "Không lấy được trạng thái", detailMessage = e.Message });
            }
        }

        [HttpPut]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> SendUrgingEmail()
        {
            try
            {
                await _asEduSurveyReportTotalService.SendUrgingEmail();
                return Ok();
            }
            catch (SendEmailException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không gửi được email", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không gửi được email", detailMessage = mainMessage });
            }
        }
        #endregion

        #region kết xuất báo cáo ks sv thường
        [HttpPost]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> ExportReportTotalNormalSurvey([Required] List<Guid> surveyRoundIds)
        {
            try
            {
                await _surveyStatisticBackgroundTask.ExportReportTotalNormalSurvey(surveyRoundIds);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (InvalidInputDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (TableBusyException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không kết xuất được báo cáo", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> GetStatusExportReportTotalNormalSurveyTask()
        {
            try
            {
                var status = await _statusService.GetStatusTableTaskNotResetMessage(TableNameTask.ExportReportTotalNormalSurvey);
                return Ok(new { status.Status, status.IsSuccess, status.Message });
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = "Không lấy được trạng thái", detailMessage = e.Message });
            }
        }

        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> CheckExistReportTotalNormalSurvey()
        {
            try
            {
                var status = await _statusService.GetStatusTableTask(TableNameTask.ExportReportTotalNormalSurvey);
                var path = System.IO.Path.GetTempPath() + status.Message;
                if (!System.IO.File.Exists(path))
                {
                    return NotFound(new { message = "File kết xuất không tồn tại" });
                }
                return Ok();
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = "Không lấy được trạng thái", detailMessage = e.Message });
            }
        }

        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> DownloadReportTotalNormalSurvey()
        {
            try
            {
                var status = await _statusService.GetStatusTableTask(TableNameTask.ExportReportTotalNormalSurvey);
                var path = System.IO.Path.GetTempPath() + status.Message;
                if (!System.IO.File.Exists(path))
                {
                    return NotFound(new { message = "File kết xuất không tồn tại" });
                }
                var templateContent = await System.IO.File.ReadAllBytesAsync(path);
                return File(templateContent, ContentTypes.Xlsx, Path.GetFileName(path));
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = "Không lấy được trạng thái", detailMessage = e.Message });
            }
        }
        #endregion

        #region thống kê sinh viên trước tốt nghiệp
        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<IActionResult> GetStatusReportTotalUndergraduateSurveyTask()
        {
            try
            {
                var status = await _statusService.GetStatusTableTask(TableNameTask.AsEduSurveyUndergraduateReportTotal);
                return Ok(new { status.Status, status.IsSuccess, status.Message });
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = "Không lấy được trạng thái", detailMessage = e.Message });
            }
        }

        [HttpPost]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<IActionResult> ReportTotalUndergraduateSurvey([Required] Guid? surveyRoundId)
        {
            try
            {
                await _surveyStatisticBackgroundTask.ReportTotalUndergraduateSurvey(surveyRoundId.Value);
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = e.Message });
            }
            catch (InvalidInputDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (TableBusyException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thống kê được đợt khảo sát", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thống kê được đợt khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }
        #endregion
    }
}
