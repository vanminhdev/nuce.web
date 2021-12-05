using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Services.Survey.Implements;

using nuce.web.shared;

namespace nuce.web.api.Controllers.Survey.Normal
{
    /// <summary>
    /// Kết quả khảo sát
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyResultController : ControllerBase
    {
        private readonly SurveyResultService _surveyResultService;
        private readonly ILogger _logger;

        public SurveyResultController(SurveyResultService surveyResultService, ILogger<SurveyResultController> logger)
        {
            _surveyResultService = surveyResultService;
            _logger = logger;
        }

        [Route("faculty/{code}")]
        [HttpPost]
        [AppAuthorize(RoleNames.KhaoThi_Survey_KhoaBan)]
        public async Task<IActionResult> FacultyResultAsync(string code)
        {
            try
            {
                return Ok(await _surveyResultService.FacultyResultAsync(code));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
            }
        }

        [Route("department/{code}")]
        [HttpPost]
        public async Task<IActionResult> DepartmentResultAsync(string code)
        {
            try
            {
                return Ok(await _surveyResultService.DepartmentResultAsync(code));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
            }
        }

        [Route("lecturer/{code}")]
        [HttpPost]
        public async Task<IActionResult> LecturerResultAsync(string code)
        {
            try
            {
                return Ok(await _surveyResultService.LecturerResultAsync(code));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
            }
        }
    }
}