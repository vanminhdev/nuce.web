using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Ctsv
{
    [Route("api/ctsv/sync")]
    [ApiController]
    //[AppAuthorize(RoleNames.Admin, RoleNames.CTSV)]
    public class SyncEduDataCtsvController : ControllerBase
    {
        private readonly ILogger<SyncEduDataCtsvController> _logger;
        private readonly ISyncEduDataCtsv _syncEduDatabaseService;
        public SyncEduDataCtsvController(ILogger<SyncEduDataCtsvController> logger, ISyncEduDataCtsv syncEduService)
        {
            _logger = logger;
            _syncEduDatabaseService = syncEduService;
        }

        [HttpPut]
        [Route("student")]
        public async Task<IActionResult> SyncStudent()
        {
            try
            {
                await _syncEduDatabaseService.SyncStudent();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ sinh viên" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        [Route("class")]
        public async Task<IActionResult> SyncClass()
        {
            try
            {
                await _syncEduDatabaseService.SyncClass();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        [Route("faculty")]
        public async Task<IActionResult> SyncFaculty()
        {
            try
            {
                await _syncEduDatabaseService.SyncFaculty();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ Khoa" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        [Route("academic")]
        public async Task<IActionResult> SyncAcademics()
        {
            try
            {
                await _syncEduDatabaseService.SyncAcademics();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ ngành học" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
    }
}
