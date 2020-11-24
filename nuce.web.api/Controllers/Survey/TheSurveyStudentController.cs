using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey
{
    /// <summary>
    /// Bài khảo sát sinh viên
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TheSurveyStudentController : ControllerBase
    {
        private readonly ILogger<TheSurveyStudentController> _logger;
        private readonly IAsEduSurveyBaiKhaoSatSinhVienService _asEduSurveyBaiKhaoSatSinhVienService;
        private readonly IUserService _userService;

        public TheSurveyStudentController(ILogger<TheSurveyStudentController> logger, IAsEduSurveyBaiKhaoSatSinhVienService asEduSurveyBaiKhaoSatSinhVienService, IUserService userService)
        {
            _logger = logger;
            _asEduSurveyBaiKhaoSatSinhVienService = asEduSurveyBaiKhaoSatSinhVienService;
            _userService = userService;
        }

        [HttpPut]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SaveTask(string classroomCode, string task)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var id = await _asEduSurveyBaiKhaoSatSinhVienService.GetIdByCode(studentCode, classroomCode);
                await _asEduSurveyBaiKhaoSatSinhVienService.SaveTask(id.ToString(), task, ip);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được bài làm", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lưu được bài làm", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được bài làm", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "P_KhaoThi")]
        public async Task<IActionResult> GenerateTheSurveyStudent()
        {
            try
            {
                await _asEduSurveyBaiKhaoSatSinhVienService.GenerateTheSurveyStudent();
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không tạo được bài khảo sát cho từng sinh viên", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tạo được bài khảo sát cho từng sinh viên", detailMessage = mainMessage });
            }
            return Ok();
        }
    }
}
