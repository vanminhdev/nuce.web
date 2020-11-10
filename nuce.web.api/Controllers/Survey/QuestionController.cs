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

namespace nuce.web.api.Controllers.Survey
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
            filter.Ma = request.Columns.FirstOrDefault(c => c.Data == "ma")?.Search.Value ?? null;
            filter.Content = request.Columns.FirstOrDefault(c => c.Data == "content")?.Search.Value ?? null;
            filter.Type = request.Columns.FirstOrDefault(c => c.Data == "type")?.Search.Value ?? null;
            var skip = request.Start;
            var pageSize = request.Length;
            var result = await _asEduSurveyCauHoiService.GetAllActiveStatus(filter, skip, pageSize);
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
        public async Task<IActionResult> GetById([Required(AllowEmptyStrings = false)] string id)
        {
            var question = await _asEduSurveyCauHoiService.GetById(id);
            if (question == null)
                return NotFound(new { message = "Không tìm thấy bản ghi" });
            return Ok(question);
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (InvalidDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([Required(AllowEmptyStrings = false)] string id, [FromBody] QuestionUpdateModel question)
        {
            try
            {
                await _asEduSurveyCauHoiService.Update(id, question);
            }
            catch(DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (InvalidDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message});
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new {message = "Không tìm thấy bản ghi"});
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([Required(AllowEmptyStrings = false)] string id)
        {
            try
            {
                await _asEduSurveyCauHoiService.Delete(id);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            return Ok();
        }
    }
}
