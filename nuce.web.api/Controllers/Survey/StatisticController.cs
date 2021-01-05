using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.Services.Survey.BackgroundTasks;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Normal.Statistic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "P_KhaoThi")]
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
            catch (InvalidDataException e)
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

        #region thống kê sinh viên trước tốt nghiệp
        [HttpGet]
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
            catch (InvalidDataException e)
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
