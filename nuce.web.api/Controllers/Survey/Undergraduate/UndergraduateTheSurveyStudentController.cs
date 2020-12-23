using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;
using nuce.web.api.ViewModel.Survey.Undergraduate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace nuce.web.api.Controllers.Survey.Graduate
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UndergraduateTheSurveyStudentController : ControllerBase
    {
        private readonly ILogger<UndergraduateTheSurveyStudentController> _logger;
        private readonly IAsEduSurveyUndergraduateBaiKhaoSatSinhVienService _asEduSurveyUndergraduateBaiKhaoSatSinhVienService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UndergraduateTheSurveyStudentController(ILogger<UndergraduateTheSurveyStudentController> logger, 
            IConfiguration configuration, IUserService userService,
            IAsEduSurveyUndergraduateBaiKhaoSatSinhVienService asEduSurveyUndergraduateBaiKhaoSatSinhVienService)
        {
            _logger = logger;
            _configuration = configuration;
            _userService = userService;
            _asEduSurveyUndergraduateBaiKhaoSatSinhVienService = asEduSurveyUndergraduateBaiKhaoSatSinhVienService;
        }

        [HttpGet]
        [Authorize(Roles = "UndergraduateStudent")]
        public async Task<IActionResult> GetTheSurvey()
        {
            var studentCode = _userService.GetCurrentStudentCode();
            try
            {
                var result = await _asEduSurveyUndergraduateBaiKhaoSatSinhVienService.GetTheSurvey(studentCode);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "UndergraduateStudent")]
        public async Task<IActionResult> GetTheSurveyContent(
            [Required(AllowEmptyStrings = false)]
            Guid? id)
        {
            try
            {
                //mã sinh viên kiểm tra sinh viên có bài khảo sát đó thật không
                var studentCode = _userService.GetCurrentStudentCode();
                var result = await _asEduSurveyUndergraduateBaiKhaoSatSinhVienService.GetTheSurveyContent(studentCode, id.Value);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "UndergraduateStudent")]
        public async Task<IActionResult> GetSelectedAnswerAutoSave([Required] Guid? theSurveyId)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var result = await _asEduSurveyUndergraduateBaiKhaoSatSinhVienService.GetSelectedAnswerAutoSave(theSurveyId.Value, studentCode);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "UndergraduateStudent")]
        public async Task<IActionResult> AutoSave([FromBody] GraduateSelectedAnswerAutoSave content)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                await _asEduSurveyUndergraduateBaiKhaoSatSinhVienService.AutoSave(content.TheSurveyId.Value, studentCode, content.QuestionCode, content.AnswerCode,
                    content.AnswerCodeInMulSelect, content.AnswerContent, content.IsAnswerCodesAdd == null || content.IsAnswerCodesAdd.Value);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tự lưu được bài làm", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
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
        [Authorize(Roles = "UndergraduateStudent")]
        public async Task<IActionResult> SaveSelectedAnswer([Required] Guid? theSurveyId)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                await _asEduSurveyUndergraduateBaiKhaoSatSinhVienService.SaveSelectedAnswer(theSurveyId.Value, studentCode, ip);
            }
            catch (InvalidDataException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được bài khảo sát", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được bài khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }

        #region generate bài ks sv
        [HttpGet]
        [Authorize(Roles = "P_KhaoThi")]
        public async Task<IActionResult> GetGenerateTheSurveyStudentStatus()
        {
            try
            {
                var status = await _asEduSurveyUndergraduateBaiKhaoSatSinhVienService.GetGenerateTheSurveyStudentStatus();
                return Ok(status);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
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
        public async Task<IActionResult> GenerateTheSurveyStudent(
            [Required]
            Guid? theSurveyId)
        {
            try
            {
                await _asEduSurveyUndergraduateBaiKhaoSatSinhVienService.GenerateTheSurveyStudent(theSurveyId.Value);
            }
            catch (TableBusyException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tạo được bài khảo sát cho từng sinh viên", detailMessage = mainMessage });
            }
            return Ok();
        }
        #endregion

        #region xác thực hoàn thành bài ks
        [HttpPost]
        [Authorize(Roles = "UndergraduateStudent")]
        public async Task<IActionResult> Verification([FromBody] VerificationStudent verification)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var token = await _asEduSurveyUndergraduateBaiKhaoSatSinhVienService.Verification(studentCode, verification);
                var clientUrl = _configuration.GetValue<string>("StudentVerificationUrl");
                var url = $"{clientUrl}?studentCode={HttpUtility.UrlEncode(studentCode)}&token={HttpUtility.UrlEncode(token)}";
                await _asEduSurveyUndergraduateBaiKhaoSatSinhVienService.SendEmailVerify(verification.Email, url);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được thông tin xác thực", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (HttpRequestException e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không gửi được email xác thực", detailMessage = mainMessage });
            }
            catch (SendEmailException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không gửi được email xác thực", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được thông tin xác thực", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> VerifyByToken(string studentCode, string token)
        {
            try
            {
                if(await _asEduSurveyUndergraduateBaiKhaoSatSinhVienService.VerifyByToken(studentCode, token))
                {
                    return Ok();
                }
                return Unauthorized(new { message = "Token không hợp lệ" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không xác thực được thông tin", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không xác thực được thông tin", detailMessage = mainMessage });
            }
        }
        #endregion
    }
}
