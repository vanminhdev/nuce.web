using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Background;
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IStatusService _statusService;

        public StatisticController(ILogger<StatisticController> logger, IAsEduSurveyReportTotalService asEduSurveyReportTotalService, IStatusService statusService)
        {
            _logger = logger;
            _asEduSurveyReportTotalService = asEduSurveyReportTotalService;
            _statusService = statusService;
        }

        public async Task<IActionResult> GetStatusReportTotalNormalSurveyTask()
        {
            try
            {
                var status = await _statusService.GetStatusTableTask(TableNameTask.AsEduSurveyReportTotal);
                //nếu đã xong k cần biết đúng sai chỉ hiển message 1 lần rồi chuyển về trạng thái chưa làm gì
                if(status.Status == (int)TableTaskStatus.Done)
                {
                    await _statusService.DoNotStatusTableTask(TableNameTask.AsEduSurveyReportTotal, "Không tìm thấy bản ghi trạng thái của bảng thống kê khảo sát");
                }
                return Ok(new { status.Status, status.IsSuccess, status.Message });
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = "Không lấy được trạng thái", detailMessage = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReportTotalNormalSurvey()
        {
            try
            {
                await _statusService.DoingStatusTableTask(TableNameTask.AsEduSurveyReportTotal, "Không tìm thấy bản ghi trạng thái của bảng thống kê khảo sát", "Đang thống kê khảo sát");
                await _asEduSurveyReportTotalService.ReportTotalNormalSurvey();
                await _statusService.DoneStatusTableTask(TableNameTask.AsEduSurveyReportTotal, "Không tìm thấy bản ghi trạng thái của bảng thống kê khảo sát");
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
                await _statusService.DoneStatusTableTask(TableNameTask.AsEduSurveyReportTotal, "Không tìm thấy bản ghi trạng thái của bảng thống kê khảo sát", e.Message);
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thống kê được đợt khảo sát", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                await _statusService.DoneStatusTableTask(TableNameTask.AsEduSurveyReportTotal, "Không tìm thấy bản ghi trạng thái của bảng thống kê khảo sát", mainMessage);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thống kê được đợt khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }
    }
}
