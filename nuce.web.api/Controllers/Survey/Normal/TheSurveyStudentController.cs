using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Background;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Status.Implements;
using nuce.web.api.Services.Survey.BackgroundTasks;
using nuce.web.api.Services.Survey.Implements;
using nuce.web.api.ViewModel.Survey;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey.Normal
{
    /// <summary>
    /// Bài khảo sát sinh viên
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TheSurveyStudentController : ControllerBase
    {
        private readonly ILogger<TheSurveyStudentController> _logger;
        private readonly AsEduSurveyBaiKhaoSatSinhVienService _asEduSurveyBaiKhaoSatSinhVienService;
        private readonly StatusService _statusService;
        private readonly IUserService _userService;
        private readonly BaiKhaoSatSinhVienBackgroundTask _baiKhaoSatSinhVienBackgroundTask;

        public TheSurveyStudentController(ILogger<TheSurveyStudentController> logger, AsEduSurveyBaiKhaoSatSinhVienService asEduSurveyBaiKhaoSatSinhVienService, 
            IUserService userService, StatusService statusService,
             BaiKhaoSatSinhVienBackgroundTask baiKhaoSatSinhVienBackgroundTask)
        {
            _logger = logger;
            _asEduSurveyBaiKhaoSatSinhVienService = asEduSurveyBaiKhaoSatSinhVienService;
            _userService = userService;
            _baiKhaoSatSinhVienBackgroundTask = baiKhaoSatSinhVienBackgroundTask;
            _statusService = statusService;
        }

        #region lấy bài và làm bài ks
        [HttpGet]
        [AppAuthorize(RoleNames.Student)]
        public async Task<IActionResult> GetTheSurvey()
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var result = await _asEduSurveyBaiKhaoSatSinhVienService.GetTheSurvey(studentCode);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet]
        [AppAuthorize(RoleNames.Student)]
        public async Task<IActionResult> GetTheSurveyContent([Required(AllowEmptyStrings = false)] Guid? id, [Required(AllowEmptyStrings = false)] string classroomCode, string nhhk)
        {
            try
            {
                //mã sinh viên kiểm tra sinh viên có bài khảo sát đó thật không
                var studentCode = _userService.GetCurrentStudentCode();
                var result = await _asEduSurveyBaiKhaoSatSinhVienService.GetTheSurveyContent(studentCode, classroomCode, nhhk, id.Value);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet]
        [AppAuthorize(RoleNames.Student)]
        public async Task<IActionResult> GetSelectedAnswerAutoSave(
            [Required(AllowEmptyStrings = false)] [NotContainWhiteSpace] string classRoomCode, [Required(AllowEmptyStrings = false)] string nhhk)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var result = await _asEduSurveyBaiKhaoSatSinhVienService.GetSelectedAnswerAutoSave(studentCode, classRoomCode, nhhk);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lấy được câu trả lời đã chọn", detailMessage = e.Message });
            }
        }

        [HttpPut]
        [AppAuthorize(RoleNames.Student)]
        public async Task<IActionResult> AutoSave([FromBody] SelectedAnswerAutoSave content)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                await _asEduSurveyBaiKhaoSatSinhVienService.AutoSave(studentCode, content.ClassRoomCode, content.NHHK, content.QuestionCode, content.AnswerCode, content.AnswerCodeInMulSelect,
                    content.AnswerContent, content.NumStar, content.City, content.IsAnswerCodesAdd == null || content.IsAnswerCodesAdd.Value);
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
        [AppAuthorize(RoleNames.Student)]
        public async Task<IActionResult> SaveSelectedAnswer([Required(AllowEmptyStrings = false)] [NotContainWhiteSpace] string classRoomCode, [Required(AllowEmptyStrings = false)] string nhhk)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                await _asEduSurveyBaiKhaoSatSinhVienService.SaveSelectedAnswer(studentCode, classRoomCode, nhhk, ip);
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
        #endregion

        #region gán bài ks
        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> GetGenerateTheSurveyStudentStatus()
        {
            try
            {
                var status = await _statusService.GetStatusTableTask(TableNameTask.AsEduSurveyBaiKhaoSatSinhVien);
                return Ok(new { status.Status, status.IsSuccess, status.Message });
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

        /// <summary>
        /// Đếm số lượng bản ghi
        /// </summary>
        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> CountGenerateTheSurveyStudent([Required(AllowEmptyStrings = false)] Guid? surveyRoundId)
        {
            try
            {
                var result = await _asEduSurveyBaiKhaoSatSinhVienService.CountGenerateTheSurveyStudent(surveyRoundId.Value);
                return Ok(new { countTheSurveyStudent = result.Item1,  countStudentClassroom = result.Item2 });
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
        [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<IActionResult> GenerateTheSurveyStudent([Required(AllowEmptyStrings = false)] Guid? surveyRoundId)
        {
            try
            {
                await _baiKhaoSatSinhVienBackgroundTask.GenerateTheSurveyStudent(surveyRoundId.Value);
                return Ok();
            }
            catch (TableBusyException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (InvalidInputDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không gán được bài khảo sát cho từng sinh viên", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không gán được bài khảo sát cho từng sinh viên", detailMessage = mainMessage });
            }
        }
        #endregion
    }
}
