using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetTheSurvey()
        {
            var studentCode = _userService.GetCurrentStudentCode();
            var result = await _asEduSurveyBaiKhaoSatSinhVienService.GetTheSurvey(studentCode);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetTheSurveyContent(
            [Required(AllowEmptyStrings = false)]
            [NotContainWhiteSpace]
            string id)
        {
            try
            {
                var result = await _asEduSurveyBaiKhaoSatSinhVienService.GetTheSurveyJsonStringByBaiKhaoSatId(id);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lấy được nội dung bài khảo sát", detailMessage = e.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetSelectedAnswerAutoSave(
            [Required(AllowEmptyStrings = false)]
            [NotContainWhiteSpace]
            string classRoomCode)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var result = await _asEduSurveyBaiKhaoSatSinhVienService.GetSelectedAnswerAutoSave(studentCode, classRoomCode);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lấy được câu trả lời đã chọn", detailMessage = e.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AutoSave([FromBody] SelectedAnswerAutoSave content)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var id = await _asEduSurveyBaiKhaoSatSinhVienService.GetIdByCode(studentCode, content.ClassRoomCode);
                await _asEduSurveyBaiKhaoSatSinhVienService.AutoSave(id.ToString(), content.QuestionCode, content.AnswerCode, content.answerCodeInMulSelect, content.isAnswerCodesAdd, content.AnswerContent);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tự lưu được bài làm", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không tự lưu được bài làm", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tự lưu được bài làm", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SaveTask([FromBody]
            [Required(AllowEmptyStrings = false)]
            [NotContainWhiteSpace]
            string classRoomCode)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var id = await _asEduSurveyBaiKhaoSatSinhVienService.GetIdByCode(studentCode, classRoomCode);
                await _asEduSurveyBaiKhaoSatSinhVienService.SaveSelectedAnswer(id.ToString(), ip);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được bài khảo sát", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lưu được bài làm", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được bài khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "P_KhaoThi")]
        public async Task<IActionResult> GetGenerateTheSurveyStudentStatus()
        {
            try
            {
                var status = await _asEduSurveyBaiKhaoSatSinhVienService.GetGenerateTheSurveyStudentStatus();
                return Ok(status);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lấy được trạng thái", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lấy được trạng thái", detailMessage = mainMessage });
            }
        }

        [HttpPost]
        [Authorize(Roles = "P_KhaoThi")]
        public async Task<IActionResult> GenerateTheSurveyStudent()
        {
            try
            {
                await _asEduSurveyBaiKhaoSatSinhVienService.GenerateTheSurveyStudent();
            }
            catch (TableBusyException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tạo được bài khảo sát cho từng sinh viên", detailMessage = e.Message });
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
