using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nuce.web.api.Services.Survey.Interfaces;

namespace nuce.web.api.Controllers.Survey.Normal
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyResultController : ControllerBase
    {
        private readonly ISurveyResultService _surveyResultService;
        public SurveyResultController(ISurveyResultService _surveyResultService)
        {
            this._surveyResultService = _surveyResultService;
        }

        [Route("faculty/{code}")]
        [HttpPost]
        public IActionResult FacultyResult(string code)
        {
            return Ok(_surveyResultService.FacultyResult(code));
        }

        [Route("department/{code}")]
        [HttpPost]
        public async Task<IActionResult> DepartmentResultAsync(string code)
        {
            return Ok(await _surveyResultService.DepartmentResultAsync(code));
        }

        [Route("lecturer/{code}")]
        [HttpPost]
        public async Task<IActionResult> LecturerResultAsync(string code)
        {
            return Ok(await _surveyResultService.LecturerResultAsync(code));
        }
    }
}