using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Services.Survey.Implements;

using nuce.web.shared;

namespace nuce.web.api.Controllers.Survey.Normal
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyResultController : ControllerBase
    {
        private readonly SurveyResultService _surveyResultService;
        public SurveyResultController(SurveyResultService _surveyResultService)
        {
            this._surveyResultService = _surveyResultService;
        }

        [Route("faculty/{code}")]
        [HttpPost]
        [AppAuthorize(RoleNames.KhaoThi_Survey_KhoaBan)]
        public async Task<IActionResult> FacultyResultAsync(string code)
        {
            return Ok(await _surveyResultService.FacultyResultAsync(code));
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