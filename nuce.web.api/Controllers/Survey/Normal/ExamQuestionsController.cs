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
using nuce.web.api.Services.Survey.Implements;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using nuce.web.shared;

namespace nuce.web.api.Controllers.Survey.Normal
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
    public class ExamQuestionsController : ControllerBase
    {
        private readonly ILogger<ExamQuestionsController> _logger;
        private readonly AsEduSurveyDeThiService _deThiService;

        public ExamQuestionsController(ILogger<ExamQuestionsController> logger, AsEduSurveyDeThiService asEduSurveyDeThiService)
        {
            _logger = logger;
            _deThiService = asEduSurveyDeThiService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] DataTableRequest request)
        {
            var filter = new ExamQuestionsFilter();
            if (request.Columns != null)
            {
                filter.Name = request.Columns.FirstOrDefault(c => c.Data == "name" || c.Name == "name")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _deThiService.GetAll(filter, skip, take);
            return Ok(
                new DataTableResponse<ExamQuestions>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lst = await _deThiService.GetAll();
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
                var lst = await _deThiService.GetExamStructure(examQuestionId);
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
                await _deThiService.AddQuestion(data.ExamQuestionId.Value, data.QuestionCode, data.Order.Value);
            }
            catch (RecordNotFoundException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tìm thấy câu hỏi" });
            }
            catch (InvalidInputDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
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
        public async Task<IActionResult> GenerateExam(GenerateExam generateExam)
        {
            try
            {
                await _deThiService.GenerateExam(generateExam);
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
                var jsonString = await _deThiService.GetExamDetailJsonString(examQuestionId);
                return Ok(jsonString);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không lấy được đề khảo sát", detailMessage = "Không tìm thấy đề khảo sát" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lấy được đề khảo sát", detailMessage = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExamQuestionsCreate exam)
        {
            try
            {
                await _deThiService.Create(exam);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Tạo không thành công", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Tạo không thành công", detailMessage = e.Message });
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestionFromStructure(
            [Required(AllowEmptyStrings = false)]
            string id)
        {
            try
            {
                await _deThiService.DeleteQuestionFromStructure(id);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Xoá không thành công", detailMessage = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Xoá không thành công", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Xoá không thành công", detailMessage = e.Message });
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([Required(AllowEmptyStrings = false)] Guid? id)
        {
            try
            {
                await _deThiService.Delete(id.Value);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy phiếu khảo sát" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không xoá được phiếu khảo sát", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message, detailMessage = mainMessage });
            }
            return Ok();
        }
    }
}
