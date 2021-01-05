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
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;

namespace nuce.web.api.Controllers.Survey.Normal
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "P_KhaoThi")]
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly IAsEduSurveyCauHoiService _asEduSurveyCauHoiService;

        public QuestionController(ILogger<QuestionController> logger, IAsEduSurveyCauHoiService asEduSurveyCauHoiService)
        {
            _logger = logger;
            _asEduSurveyCauHoiService = asEduSurveyCauHoiService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] DataTableRequest request)
        {
            var filter = new QuestionFilter();
            if(request.Columns != null)
            {
                filter.Code = request.Columns.FirstOrDefault(c => c.Data == "code")?.Search.Value ?? null;
                filter.Content = request.Columns.FirstOrDefault(c => c.Data == "content")?.Search.Value ?? null;
                filter.Type = request.Columns.FirstOrDefault(c => c.Data == "type" || c.Name == "type")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _asEduSurveyCauHoiService.GetAllActiveStatus(filter, skip, take);
            return Ok(
                new DataTableResponse<QuestionModel>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetById([Required(AllowEmptyStrings = false)] Guid? id)
        {
            try
            {
                var question = await _asEduSurveyCauHoiService.GetById(id.Value);                    
                return Ok(question);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy câu hỏi" });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lấy được câu hỏi", detailMessage = mainMessage });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuestionCreateModel question)
        {
            try
            {
                await _asEduSurveyCauHoiService.Create(question);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tạo được câu hỏi", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message, detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([Required(AllowEmptyStrings = false)] Guid? id, [FromBody] QuestionUpdateModel question)
        {
            try
            {
                await _asEduSurveyCauHoiService.Update(id.Value, question);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không sửa được câu hỏi", detailMessage = e.Message });
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new {message = "Không tìm thấy câu hỏi cần sửa"});
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message, detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([Required(AllowEmptyStrings = false)] Guid? id)
        {
            try
            {
                await _asEduSurveyCauHoiService.Delete(id.Value);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy câu hỏi" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không xoá được câu hỏi", detailMessage = e.Message });
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
