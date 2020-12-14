using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "P_KhaoThi")]
    public class SurveyRoundController : ControllerBase
    {
        private readonly ILogger<SurveyRoundController> _logger;
        private readonly IAsEduSurveyDotKhaoSatService _asEduSurveyDotKhaoSatService;

        public SurveyRoundController(ILogger<SurveyRoundController> logger, IAsEduSurveyDotKhaoSatService asEduSurveyDotKhaoSatService)
        {
            _logger = logger;
            _asEduSurveyDotKhaoSatService = asEduSurveyDotKhaoSatService;
        }

        #region đợt khảo sát
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

        [HttpGet]
        public async Task<IActionResult> GetSurveyRoundActive()
        {
            try
            {
                var result = await _asEduSurveyDotKhaoSatService.GetSurveyRoundActive();
                return Ok(result);
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lấy được đợt khảo sát", detailMessage = mainMessage });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSurveyRoundById(
            [Required(AllowEmptyStrings = false)]
            Guid? id)
        {
            try
            {
                var surveyRound = await _asEduSurveyDotKhaoSatService.GetSurveyRoundById(id.Value);
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lấy được đợt khảo sát", detailMessage = mainMessage });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSurveyRound([FromBody] SurveyRoundCreate surveyRound)
        {
            try
            {
                await _asEduSurveyDotKhaoSatService.Create(surveyRound);
            }
            catch (InvalidDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { message = e.Message });
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
        public async Task<IActionResult> UpdateSurveyRound([Required(AllowEmptyStrings = false)] Guid? id, [FromBody] SurveyRoundUpdate surveyRound)
        {
            try
            {
                await _asEduSurveyDotKhaoSatService.Update(id.Value, surveyRound);
            }
            catch (InvalidDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
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
        public async Task<IActionResult> DeleteSurveyRound([Required(AllowEmptyStrings = false)] Guid? id)
        {
            try
            {
                await _asEduSurveyDotKhaoSatService.Delete(id.Value);
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

        [HttpPut]
        public async Task<IActionResult> CloseSurveyRound([Required(AllowEmptyStrings = false)] Guid? id)
        {
            try
            {
                await _asEduSurveyDotKhaoSatService.Close(id.Value);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy đợt khảo sát" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không kết thúc được đợt khảo sát", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không kết thúc được đợt khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }
        #endregion
    }
}
