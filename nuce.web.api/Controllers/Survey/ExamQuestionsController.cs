using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;

namespace nuce.web.api.Controllers.Survey
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "P_KhaoThi")]
    public class ExamQuestionsController : ControllerBase
    {
        private readonly ILogger<ExamQuestionsController> _logger;
        private readonly IAsEduSurveyDeThiService _asEduSurveyDeThiService;

        public ExamQuestionsController(ILogger<ExamQuestionsController> logger, IAsEduSurveyDeThiService asEduSurveyDeThiService)
        {
            _logger = logger;
            _asEduSurveyDeThiService = asEduSurveyDeThiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lst = await _asEduSurveyDeThiService.GetAll();
            return Ok(lst);
        }

        [HttpGet]
        public async Task<IActionResult> GetExamStructure(
            [Required(AllowEmptyStrings = false)]
            [NotContainWhiteSpace]
            string examQuestionId)
        {
            try
            {
                var lst = await _asEduSurveyDeThiService.GetExamStructure(examQuestionId);
                return Ok(lst);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] AddQuestion data)
        {
            try
            {
                await _asEduSurveyDeThiService.AddQuestion(data.ExamQuestionId, data.QuestionCode, data.Order.Value);
            }
            catch (RecordNotFoundException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tìm thấy câu hỏi" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Thêm không thành công", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Có lỗi xảy ra", detailMessage = e.Message });
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateExam(
            [FromBody]
            [Required(AllowEmptyStrings = false)]
            [NotContainWhiteSpace]
            string examQuestionId)
        {
            try
            {
                await _asEduSurveyDeThiService.GenerateExam(examQuestionId);
            }
            catch (RecordNotFoundException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tìm thấy câu hỏi" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Tạo không thành công", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Có lỗi xảy ra", detailMessage = e.Message });
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetExamDetailJsonString(
            [Required(AllowEmptyStrings = false)]
            [NotContainWhiteSpace]
            string examQuestionId)
        {
            try
            {
                var jsonString = await _asEduSurveyDeThiService.GetExamDetailJsonString(examQuestionId);
                return Ok(jsonString);
            }
            catch (RecordNotFoundException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tìm thấy đề khảo sát" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Có lỗi xảy ra", detailMessage = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExamQuestionsCreate exam)
        {
            try
            {
                await _asEduSurveyDeThiService.CreateExamQuestions(exam.Code, exam.Name);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Tạo không thành công", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Có lỗi xảy ra", detailMessage = e.Message });
            }
            return Ok();
        }
    }
}
