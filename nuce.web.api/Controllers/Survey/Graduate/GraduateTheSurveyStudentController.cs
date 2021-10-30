using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Survey.Implements;
using nuce.web.api.ViewModel.Survey;
using nuce.web.shared;
using nuce.web.shared.Common;
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
        private readonly AsEduSurveyGraduateBaiKhaoSatSinhVienService _asEduSurveyGraduateBaiKhaoSatSinhVienService;
        private readonly AsEduSurveyGraduateDotKhaoSatService _asEduSurveyGraduateDotKhaoSatService;
        private readonly IUserService _userService;

        public GraduateTheSurveyStudentController(ILogger<GraduateTheSurveyStudentController> logger, IUserService userService,
            AsEduSurveyGraduateDotKhaoSatService asEduSurveyGraduateDotKhaoSatService,
            AsEduSurveyGraduateBaiKhaoSatSinhVienService asEduSurveyGraduateBaiKhaoSatSinhVienService)
        {
            _logger = logger;
            _userService = userService;
            _asEduSurveyGraduateDotKhaoSatService = asEduSurveyGraduateDotKhaoSatService;
            _asEduSurveyGraduateBaiKhaoSatSinhVienService = asEduSurveyGraduateBaiKhaoSatSinhVienService;
        }

        #region sv tự làm bài
        [HttpGet]
        [AppAuthorize(RoleNames.GraduateStudent)]
        public async Task<IActionResult> GetTheSurvey()
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var result = await _asEduSurveyGraduateBaiKhaoSatSinhVienService.GetTheSurvey(studentCode);

                var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetCurrentSurveyRound();
                if (surveyRound == null || DateTime.Now >= surveyRound.EndDate)
                {
                    throw new RecordNotFoundException("Không có đợt khảo sát nào được mở");
                }
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet]
        [AppAuthorize(RoleNames.GraduateStudent, RoleNames.KhaoThi_Survey_KhoaBan)]
        public async Task<IActionResult> GetTheSurveyContent([Required(AllowEmptyStrings = false)] Guid? id, string studentCode)
        {
            try
            {
                //mã sinh viên kiểm tra sinh viên có bài khảo sát đó thật không
                var curStudentCode = _userService.GetCurrentStudentCode();
                if (HttpContext.User.IsInRole(RoleNames.KhaoThi_Survey_KhoaBan))
                {
                    curStudentCode = studentCode;
                }
                else
                {
                    var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetCurrentSurveyRound();
                    if(surveyRound == null || DateTime.Now >= surveyRound.EndDate)
                    {
                        throw new RecordNotFoundException("Không có đợt khảo sát nào được mở");
                    }
                }
                var result = await _asEduSurveyGraduateBaiKhaoSatSinhVienService.GetTheSurveyContent(curStudentCode, id.Value);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet]
        [AppAuthorize(RoleNames.GraduateStudent, RoleNames.KhaoThi_Survey_KhoaBan)]
        public async Task<IActionResult> GetSelectedAnswerAutoSave([Required] Guid? theSurveyId, string studentCode)
        {
            try
            {
                var curStudentCode = _userService.GetCurrentStudentCode();
                if (HttpContext.User.IsInRole(RoleNames.KhaoThi_Survey_KhoaBan))
                {
                    curStudentCode = studentCode;
                    var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetCurrentSurveyRound();
                    if (surveyRound == null || !(DateTime.Now >= surveyRound.EndDate)) //đợt đã kết thúc mới lấy được danh sách, nếu chưa kết thúc thì không trả về gì
                    {
                        throw new RecordNotFoundException("Đợt khảo sát chưa kết thúc không lấy được danh sách sinh viên");
                    }
                }
                else
                {
                    var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetCurrentSurveyRound();
                    if (surveyRound == null || DateTime.Now >= surveyRound.EndDate)
                    {
                        throw new RecordNotFoundException("Không có đợt khảo sát nào được mở");
                    }
                }
                var result = await _asEduSurveyGraduateBaiKhaoSatSinhVienService.GetSelectedAnswerAutoSave(theSurveyId.Value, curStudentCode);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpPut]
        [AppAuthorize(RoleNames.GraduateStudent, RoleNames.KhaoThi_Survey_KhoaBan)]
        public async Task<IActionResult> AutoSave(string studentCode, [FromBody] GraduateSelectedAnswerAutoSave content)
        {
            try
            {
                var curStudentCode = _userService.GetCurrentStudentCode();
                if (HttpContext.User.IsInRole(RoleNames.KhaoThi_Survey_KhoaBan))
                {
                    curStudentCode = studentCode;
                    var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetCurrentSurveyRound();
                    if (surveyRound == null || !(DateTime.Now >= surveyRound.EndDate)) //đợt đã kết thúc mới lấy được danh sách, nếu chưa kết thúc thì không trả về gì
                    {
                        throw new RecordNotFoundException("Đợt khảo sát chưa kết thúc không lấy được danh sách sinh viên");
                    }
                }
                else
                {
                    var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetCurrentSurveyRound();
                    if (surveyRound == null || DateTime.Now >= surveyRound.EndDate)
                    {
                        throw new RecordNotFoundException("Không có đợt khảo sát nào được mở");
                    }
                }
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                await _asEduSurveyGraduateBaiKhaoSatSinhVienService.AutoSave(content.TheSurveyId.Value, curStudentCode, content.QuestionCode, content.AnswerCode,
                    content.AnswerCodeInMulSelect, content.AnswerContent, content.NumStar, content.City, content.IsAnswerCodesAdd == null || content.IsAnswerCodesAdd.Value);
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
        [AppAuthorize(RoleNames.GraduateStudent, RoleNames.KhaoThi_Survey_KhoaBan)]
        public async Task<IActionResult> SaveSelectedAnswer([Required] Guid? theSurveyId, string studentCode, string loaiHinh)
        {
            try
            {
                var curStudentCode = _userService.GetCurrentStudentCode();
                if (HttpContext.User.IsInRole(RoleNames.KhaoThi_Survey_KhoaBan))
                {
                    curStudentCode = studentCode;
                    var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetCurrentSurveyRound();
                    if (surveyRound == null || !(DateTime.Now >= surveyRound.EndDate)) //đợt đã kết thúc mới lấy được danh sách, nếu chưa kết thúc thì không trả về gì
                    {
                        throw new RecordNotFoundException("Đợt khảo sát chưa kết thúc không lấy được danh sách sinh viên");
                    }
                }
                else
                {
                    loaiHinh = GraduateBaiLamType.Online;
                    var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetCurrentSurveyRound();
                    if (surveyRound == null || DateTime.Now >= surveyRound.EndDate)
                    {
                        throw new RecordNotFoundException("Không có đợt khảo sát nào được mở");
                    }
                }
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                await _asEduSurveyGraduateBaiKhaoSatSinhVienService.SaveSelectedAnswer(theSurveyId.Value, curStudentCode, ip, loaiHinh);
            }
            catch (InvalidInputDataException e)
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

        [HttpPost]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<IActionResult> GenerateTheSurveyStudent([Required] Guid? theSurveyId)
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
        #endregion

        #region khoa làm hộ bài sv
        /// <summary>
        /// lấy bài ks
        /// </summary>
        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_KhoaBan)]
        public async Task<IActionResult> GetTheSurveyStudent(string studentCode)
        {
            try
            {
                var facultyCode = _userService.GetClaimByKey(UserParameters.UserCode);
                var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetCurrentSurveyRound();
                if (surveyRound == null || !(DateTime.Now >= surveyRound.EndDate)) //đợt đã kết thúc mới lấy được danh sách, nếu chưa kết thúc thì không trả về gì
                {
                    throw new RecordNotFoundException("Đợt khảo sát chưa kết thúc không lấy được danh sách sinh viên");
                }
                var result = await _asEduSurveyGraduateBaiKhaoSatSinhVienService.GetTheSurvey(facultyCode, studentCode);
                return Ok(result.FirstOrDefault());
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        #endregion
    }
}
