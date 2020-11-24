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
    /// <summary>
    /// Đợt khảo sát
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SurveyRoundController : ControllerBase
    {
        private readonly ILogger<SurveyRoundController> _logger;
        private readonly IAsEduSurveyDotKhaoSatService _asEduSurveyDotKhaoSatService;
        private readonly IAsEduSurveyBaiKhaoSatService _asEduSurveyBaiKhaoSatService;

        public SurveyRoundController(ILogger<SurveyRoundController> logger, IAsEduSurveyDotKhaoSatService asEduSurveyDotKhaoSatService, IAsEduSurveyBaiKhaoSatService asEduSurveyBaiKhaoSatService)
        {
            _logger = logger;
            _asEduSurveyDotKhaoSatService = asEduSurveyDotKhaoSatService;
            _asEduSurveyBaiKhaoSatService = asEduSurveyBaiKhaoSatService;
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
        public async Task<IActionResult> CreateSurveyRound([FromBody] SurveyRoundCreate surveyRound)
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tạo được đợt khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSurveyRound([Required(AllowEmptyStrings = false)] string id, [FromBody] SurveyRoundUpdate surveyRound)
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không cập nhật được đợt khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSurveyRound([Required(AllowEmptyStrings = false)] string id)
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không xoá được đợt khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetTheSurvey([FromBody] DataTableRequest request)
        {
            var filter = new TheSurveyFilter();
            if (request.Columns != null)
            {
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _asEduSurveyBaiKhaoSatService.GetTheSurvey(filter, skip, take);
            return Ok(
                new DataTableResponse<AsEduSurveyBaiKhaoSat>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreateTheSurvey([FromBody] TheSurveyCreate theSurvey)
        {
            try
            {
                await _asEduSurveyBaiKhaoSatService.Create(theSurvey);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không tạo được bài khảo sát", detailMessage = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tạo được bài khảo sát", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tạo được bài khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTheSurvey([Required(AllowEmptyStrings = false)] string id, [FromBody] TheSurveyUpdate theSurvey)
        {
            try
            {
                await _asEduSurveyBaiKhaoSatService.Update(id, theSurvey);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không cập nhật được bài khảo sát", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không cập nhật được bài khảo sát", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không cập nhật được bài khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTheSurvey([Required(AllowEmptyStrings = false)] string id)
        {
            try
            {
                await _asEduSurveyBaiKhaoSatService.Delete(id);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy bài khảo sát" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không xoá được đợt bài khảo sát", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không xoá được bài khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }
    }
}
