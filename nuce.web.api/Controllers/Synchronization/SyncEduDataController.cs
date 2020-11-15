using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class SyncEduDataController : ControllerBase
    {
        private readonly ILogger<SyncEduDataController> _logger;
        private readonly ISyncEduDatabaseService _syncEduDatabaseService;

        public SyncEduDataController(ILogger<SyncEduDataController> logger, ISyncEduDatabaseService syncEduDatabaseService)
        {
            _logger = logger;
            _syncEduDatabaseService = syncEduDatabaseService;
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
        public async Task<IActionResult> SyncDepartment()
        {
            try
            {
                await _syncEduDatabaseService.SyncDepartment();
                return Ok();
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ bộ môn" });
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

        [HttpPut]
        public async Task<IActionResult> SyncSubject()
        {
            try
            {
                await _syncEduDatabaseService.SyncSubject();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ môn học" });
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
        public async Task<IActionResult> SyncLecturer()
        {
            try
            {
                await _syncEduDatabaseService.SyncLecturer();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ giảng viên" });
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

        #region kỳ trước
        [HttpPut]
        public async Task<IActionResult> SyncLastClassRoom()
        {
            try
            {
                await _syncEduDatabaseService.SyncLastClassRoom();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp môn học kỳ trước" });
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
        public async Task<IActionResult> SyncLastLecturerClassRoom()
        {
            try
            {
                await _syncEduDatabaseService.SyncLastLecturerClassRoom();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp môn học giảng viên kỳ trước" });
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
        public async Task<IActionResult> SyncLastStudentClassRoom()
        {
            try
            {
                var message = await _syncEduDatabaseService.SyncLastStudentClassRoom();
                return Ok(new { message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp môn học sinh viên kỳ trước" });
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
        #endregion

        #region kỳ hiện tại
        [HttpPut]
        public async Task<IActionResult> SyncCurrentClassRoom()
        {
            try
            {
                await _syncEduDatabaseService.SyncCurrentClassRoom();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp môn học kỳ hiện tại", detailMessage = e.Message });
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
        public async Task<IActionResult> SyncCurrentLecturerClassRoom()
        {
            try
            {
                await _syncEduDatabaseService.SyncCurrentLecturerClassRoom();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp môn học giảng viên kỳ hiện tại", detailMessage = e.Message });
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
        public async Task<IActionResult> SyncCurrentStudentClassRoom()
        {
            try
            {
                var message = await _syncEduDatabaseService.SyncCurrentStudentClassRoom();
                return Ok(new { message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp môn học sinh viên kỳ hiện tại", detailMessage = e.Message });
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
        public async Task<IActionResult> SyncUpdateFromDateEndDateCurrentClassRoom()
        {
            try
            {
                var message = await _syncEduDatabaseService.SyncUpdateFromDateEndDateCurrentClassRoom();
                return Ok(new { message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ thời gian bắt đầu và kết thúc tuần của lớp môn học kỳ hiện tại", detailMessage = e.Message });
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
        #endregion
    }
}
