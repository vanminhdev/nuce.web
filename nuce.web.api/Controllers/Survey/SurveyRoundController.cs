using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SurveyRoundController : ControllerBase
    {
        private readonly ILogger<SurveyRoundController> _logger;
        private readonly IAsEduSurveyDotKhaoSatService _asEduSurveyDotKhaoSatService;

        public SurveyRoundController(ILogger<SurveyRoundController> logger, IAsEduSurveyDotKhaoSatService asEduSurveyDotKhaoSatService)
        {
            _logger = logger;
            _asEduSurveyDotKhaoSatService = asEduSurveyDotKhaoSatService;
        }

        [HttpPost]
        public async Task<IActionResult> GetSurveyRound([FromBody] DataTableRequest request)
        {
            var filter = new SurveyRoundFilter();
            if (request.Columns != null)
            {
                filter.Name = request.Columns.FirstOrDefault(c => c.Data == "name" || c.Name == "name")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _asEduSurveyDotKhaoSatService.GetSurveyRound(filter, skip, take);
            return Ok(
                new DataTableResponse<AsEduSurveyDotKhaoSat>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SurveyRoundCreateModel surveyRound)
        {
            try
            {
                await _asEduSurveyDotKhaoSatService.Create(surveyRound);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tạo được đợt khảo sát", detailMessage = e.Message });
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
        public async Task<IActionResult> Update([Required(AllowEmptyStrings = false)] string id, [FromBody] SurveyRoundUpdateModel surveyRound)
        {
            try
            {
                await _asEduSurveyDotKhaoSatService.Update(id, surveyRound);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không cập nhật được đợt khảo sát", detailMessage = e.Message });
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy đợt khảo sát cần sửa" });
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
        public async Task<IActionResult> Delete([Required(AllowEmptyStrings = false)] string id)
        {
            try
            {
                await _asEduSurveyDotKhaoSatService.Delete(id);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy đợt khảo sát" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không xoá được đợt khảo sát", detailMessage = e.Message });
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
