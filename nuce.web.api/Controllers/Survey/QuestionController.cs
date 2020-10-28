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
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;

namespace nuce.web.api.Controllers.Survey
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly IAsEduSurveyCauHoiService _asEduSurveyCauHoiService;

        public QuestionController(ILogger<QuestionController> logger, IAsEduSurveyCauHoiService asEduSurveyCauHoiService)
        {
            _logger = logger;
            _asEduSurveyCauHoiService = asEduSurveyCauHoiService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var questions = _asEduSurveyCauHoiService.GetAllActiveStatus().Result;
            return Ok(questions);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([Required(AllowEmptyStrings = false)] string id)
        {
            var question = await _asEduSurveyCauHoiService.GetById(id);
            if (question == null)
                return NotFound(new { message = "Record not found" });
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
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                return NotFound(new {message = "Record not found"});
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                return NotFound(new { message = "Record not found" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
