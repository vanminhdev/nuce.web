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
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey.Graduate
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GraduateTheSurveyStudentController : ControllerBase
    {
        private readonly ILogger<GraduateTheSurveyStudentController> _logger;
        private readonly IAsEduSurveyGraduateBaiKhaoSatSinhVienService _asEduSurveyGraduateBaiKhaoSatSinhVienService;
        private readonly IUserService _userService;

        public GraduateTheSurveyStudentController(ILogger<GraduateTheSurveyStudentController> logger, IUserService userService,
            IAsEduSurveyGraduateBaiKhaoSatSinhVienService asEduSurveyGraduateBaiKhaoSatSinhVienService)
        {
            _logger = logger;
            _userService = userService;
            _asEduSurveyGraduateBaiKhaoSatSinhVienService = asEduSurveyGraduateBaiKhaoSatSinhVienService;
        }

        [HttpGet]
        [AppAuthorize(RoleNames.GraduateStudent)]
        public async Task<IActionResult> GetTheSurvey()
        {
            var studentCode = _userService.GetCurrentStudentCode();
            var result = await _asEduSurveyGraduateBaiKhaoSatSinhVienService.GetTheSurvey(studentCode);
            return Ok(result);
        }

        [HttpGet]
        [AppAuthorize(RoleNames.GraduateStudent)]
        public async Task<IActionResult> GetTheSurveyContent(
            [Required(AllowEmptyStrings = false)]
            Guid? id)
        {
            try
            {
                //mã sinh viên kiểm tra sinh viên có bài khảo sát đó thật không
                var studentCode = _userService.GetCurrentStudentCode();
                var result = await _asEduSurveyGraduateBaiKhaoSatSinhVienService.GetTheSurveyContent(studentCode, id.Value);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lấy được nội dung bài khảo sát", detailMessage = e.Message });
            }
        }

        [HttpGet]
        [AppAuthorize(RoleNames.GraduateStudent)]
        public async Task<IActionResult> GetSelectedAnswerAutoSave([Required] Guid? theSurveyId)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var result = await _asEduSurveyGraduateBaiKhaoSatSinhVienService.GetSelectedAnswerAutoSave(theSurveyId.Value, studentCode);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lấy được câu trả lời đã chọn", detailMessage = e.Message });
            }
        }

        [HttpPut]
        [AppAuthorize(RoleNames.GraduateStudent)]
        public async Task<IActionResult> AutoSave([FromBody] GraduateSelectedAnswerAutoSave content)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                await _asEduSurveyGraduateBaiKhaoSatSinhVienService.AutoSave(content.TheSurveyId.Value, studentCode, content.QuestionCode, content.AnswerCode,
                    content.AnswerCodeInMulSelect, content.AnswerContent, content.NumStar, content.City, content.IsAnswerCodesAdd == null || content.IsAnswerCodesAdd.Value);
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
        [AppAuthorize(RoleNames.GraduateStudent)]
        public async Task<IActionResult> SaveSelectedAnswer([Required] Guid? theSurveyId)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                await _asEduSurveyGraduateBaiKhaoSatSinhVienService.SaveSelectedAnswer(theSurveyId.Value, studentCode, ip);
            }
            catch (InvalidInputDataException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được bài khảo sát", detailMessage = e.Message });
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
        [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<IActionResult> GetGenerateTheSurveyStudentStatus()
        {
            try
            {
                var status = await _asEduSurveyGraduateBaiKhaoSatSinhVienService.GetGenerateTheSurveyStudentStatus();
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
        [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<IActionResult> GenerateTheSurveyStudent(
            [Required]
            Guid? theSurveyId)
        {
            try
            {
                await _asEduSurveyGraduateBaiKhaoSatSinhVienService.GenerateTheSurveyStudent(theSurveyId.Value);
            }
            catch (TableBusyException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không phát được bài khảo sát cho từng sinh viên", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không phát được bài khảo sát cho từng sinh viên", detailMessage = e.Message });
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
