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
            catch (TableBusyException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thống kê được đợt khảo sát", detailMessage = e.Message });
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
    }
}
