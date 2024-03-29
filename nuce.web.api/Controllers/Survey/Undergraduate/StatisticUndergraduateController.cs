﻿using Microsoft.AspNetCore.Authorization;
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
using nuce.web.api.ViewModel.Survey.Undergraduate.Statistic;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey.Undergraduate
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatisticUndergraduateController : ControllerBase
    {
        private readonly ILogger<StatisticUndergraduateController> _logger;
        private readonly IAsEduSurveyUndergraduateReportTotalService _reportTotalService;
        private readonly IStatusService _statusService;

        public StatisticUndergraduateController(ILogger<StatisticUndergraduateController> logger, IAsEduSurveyUndergraduateReportTotalService reportTotalService, 
            IStatusService statusService)
        {
            _logger = logger;
            _reportTotalService = reportTotalService;
            _statusService = statusService;
        }

        #region thống kê sinh viên trước tốt nghiệp
        //[HttpPost]
        //[AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
        //public async Task<IActionResult> ReportTotalUndergraduateSurvey([Required] Guid? surveyRoundId, [Required] Guid? theSurveyId)
        //{
        //    try
        //    {
        //          await _reportTotalService.ReportTotalUndergraduateSurvey(surveyRoundId.Value, theSurveyId.Value, fromDate.Value, toDate.Value);
        //    }
        //    catch (RecordNotFoundException e)
        //    {
        //        _logger.LogError(e, e.Message);
        //        return NotFound(new { message = e.Message });
        //    }
        //    catch (InvalidInputDataException e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
        //    }
        //    catch (TableBusyException e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        _logger.LogError(e, e.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thống kê được đợt khảo sát", detailMessage = e.Message });
        //    }
        //    catch (Exception e)
        //    {
        //        var mainMessage = UtilsException.GetMainMessage(e);
        //        _logger.LogError(e, mainMessage);
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thống kê được đợt khảo sát", detailMessage = mainMessage });
        //    }
        //    return Ok();
        //}

        [HttpPost]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<IActionResult> GetRawReportTotalUndergraduateSurvey([FromBody] DataTableRequest request)
        {
            var filter = new ReportTotalUndergraduateFilter();
            if (request.Columns != null)
            {

            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _reportTotalService.GetRawReportTotalUndergraduateSurvey(filter, skip, take);
            return Ok(
                new DataTableResponse<ReportTotalUndergraduate>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<IActionResult> ExportReportTotalUndergraduateSurvey([Required] Guid? surveyRoundId, [Required] Guid? theSurveyId,
            [Required] DateTime? fromDate, [Required] DateTime? toDate)
        {
            try
            {
                _reportTotalService.ReportTotalUndergraduateSurvey(surveyRoundId.Value, theSurveyId.Value, fromDate.Value, toDate.Value);
                var data = await _reportTotalService.ExportReportTotalUndergraduateSurvey(surveyRoundId.Value, theSurveyId.Value, fromDate.Value, toDate.Value);
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
        #endregion
    }
}
