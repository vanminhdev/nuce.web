using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Status.Implements;
using nuce.web.api.Services.Survey.BackgroundTasks;
using nuce.web.api.Services.Survey.Implements.Graduate;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Undergraduate.Statistic;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey.Graduate
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatisticGraduateController : ControllerBase
    {
        private readonly ILogger<StatisticGraduateController> _logger;
        private readonly AsEduSurveyGraduateReportTotalService _reportTotalService;
        private readonly StatusService _statusService;

        public StatisticGraduateController(ILogger<StatisticGraduateController> logger, AsEduSurveyGraduateReportTotalService reportTotalService, 
            StatusService statusService)
        {
            _logger = logger;
            _reportTotalService = reportTotalService;
            _statusService = statusService;
        }

        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> GetTempDataGraduateSurvey([Required] Guid? surveyRoundId)
        {
            try
            {
                var result = await _reportTotalService.TempDataGraduateSurvey(surveyRoundId.Value);
                return Ok(result);
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thống kê được", detailMessage = mainMessage });
            }
        }

        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<IActionResult> ExportReportTotalGraduateSurvey([Required] Guid? surveyRoundId)
        {
            try
            {
                var data = await _reportTotalService.ExportReportTotalGraduateSurvey(surveyRoundId.Value);
                return File(data, ContentTypes.Xlsx);
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
        }
    }
}
