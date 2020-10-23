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
    //[Authorize]
    public class AnswerController : ControllerBase
    {
        private readonly ILogger<AnswerController> _logger;
        private readonly IAsEduSurveyDapAnService _asEduSurveyDapAnService;

        public AnswerController(ILogger<AnswerController> logger, IAsEduSurveyDapAnService asEduSurveyDapAnService)
        {
            _logger = logger;
            _asEduSurveyDapAnService = asEduSurveyDapAnService;
        }

        [HttpGet]
        public IActionResult GetByQuestionId([Required(AllowEmptyStrings = false)] string questionId)
        {
            var answers = _asEduSurveyDapAnService.GetByQuestionIdActiveStatus(questionId).Result;
            return Ok(answers);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([Required(AllowEmptyStrings = false)] string id)
        {
            var answer = await _asEduSurveyDapAnService.GetById(id);
            if (answer == null)
                return NotFound(new { message = "Record not found" });
            return Ok(answer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AnswerCreate answer)
        {
            try
            {
                await _asEduSurveyDapAnService.Create(answer);
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
        public async Task<IActionResult> Update([Required(AllowEmptyStrings = false)] string id, [FromBody] AnswerUpdate answer)
        {
            try
            {
                await _asEduSurveyDapAnService.Update(id, answer);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Record not found" });
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
                await _asEduSurveyDapAnService.Delete(id);
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
