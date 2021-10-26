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
using nuce.web.api.ViewModel.Survey;
using nuce.web.shared;

namespace nuce.web.api.Controllers.Survey.Normal
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AppAuthorize(RoleNames.KhaoThi_Survey_Normal)]
    public class AnswerController : ControllerBase
    {
        private readonly ILogger<AnswerController> _logger;
        private readonly AsEduSurveyDapAnService _dapAnService;

        public AnswerController(ILogger<AnswerController> logger, AsEduSurveyDapAnService asEduSurveyDapAnService)
        {
            _logger = logger;
            _dapAnService = asEduSurveyDapAnService;
        }

        [HttpGet]
        public IActionResult GetByQuestionId([Required(AllowEmptyStrings = false)] string questionId)
        {
            var answers = _dapAnService.GetByQuestionIdActiveStatus(questionId).Result;
            return Ok(answers);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([Required(AllowEmptyStrings = false)] string id)
        {
            var answer = await _dapAnService.GetById(id);
            if (answer == null)
                return NotFound(new { message = "Không tìm thấy bản ghi" });
            return Ok(answer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AnswerCreateModel answer)
        {
            try
            {
                await _dapAnService.Create(answer);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (InvalidInputDataException e)
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
        public async Task<IActionResult> Update([Required(AllowEmptyStrings = false)] string id, [FromBody] AnswerUpdateModel answer)
        {
            try
            {
                await _dapAnService.Update(id, answer);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi" });
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
                await _dapAnService.Delete(id);
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
