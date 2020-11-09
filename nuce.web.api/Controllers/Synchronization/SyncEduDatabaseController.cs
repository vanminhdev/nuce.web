using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Synchronization.Interfaces;

namespace nuce.web.api.Controllers.Synchronization
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SyncEduDatabaseController : ControllerBase
    {
        private readonly ILogger<SyncEduDatabaseController> _logger;
        private readonly ISyncEduDatabaseService _syncEduDatabaseService;

        public SyncEduDatabaseController(ILogger<SyncEduDatabaseController> logger, ISyncEduDatabaseService syncEduDatabaseService)
        {
            _logger = logger;
            _syncEduDatabaseService = syncEduDatabaseService;
        }

        [HttpPut]
        public async Task<IActionResult> SyncSubjects()
        {
            try
            {
                await _syncEduDatabaseService.SyncSubjects();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể đồng bộ dữ liệu Khoa" });
            }
            catch (CannotCallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
    }
}
