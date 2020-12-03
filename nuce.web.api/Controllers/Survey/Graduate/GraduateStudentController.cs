using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Graduate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey.Graduate
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "P_KhaoThi")]
    public class GraduateStudentController : ControllerBase
    {
        private readonly ILogger<GraduateStudentController> _logger;
        private readonly IAsEduSurveyGraduateStudentService _asEduSurveyGraduateStudentService;

        public GraduateStudentController(ILogger<GraduateStudentController> logger, IAsEduSurveyGraduateStudentService asEduSurveyGraduateStudentService)
        {
            _logger = logger;
            _asEduSurveyGraduateStudentService = asEduSurveyGraduateStudentService;
        }

        [HttpPost]
        public async Task<IActionResult> GetGraduateStudent([FromBody] DataTableRequest request)
        {
            var filter = new GraduateStudentFilter();
            if (request.Columns != null)
            {
                filter.Masv = request.Columns.FirstOrDefault(c => c.Data == "masv" || c.Name == "masv")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _asEduSurveyGraduateStudentService.GetAll(filter, skip, take);
            return Ok(
                new DataTableResponse<AsEduSurveyGraduateStudent>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetGraduateStudentById(
            [Required(AllowEmptyStrings = false)]
            string id)
        {
            try
            {
                var surveyRound = await _asEduSurveyGraduateStudentService.GetById(id);
                return Ok(surveyRound);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lấy được sinh viên", detailMessage = mainMessage });
            }
        }
    }
}
