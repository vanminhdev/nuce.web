using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Graduate;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey.Graduate
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate, RoleNames.GraduateStudent)]
    public class GraduateSurveyRoundController : ControllerBase
    {
        private readonly ILogger<GraduateSurveyRoundController> _logger;
        private readonly IAsEduSurveyGraduateDotKhaoSatService _asEduSurveyGraduateDotKhaoSatService;

        public GraduateSurveyRoundController(ILogger<GraduateSurveyRoundController> logger, IAsEduSurveyGraduateDotKhaoSatService asEduSurveyGraduateDotKhaoSatService)
        {
            _logger = logger;
            _asEduSurveyGraduateDotKhaoSatService = asEduSurveyGraduateDotKhaoSatService;
        }

        #region đợt khảo sát
        [HttpPost]
        public async Task<IActionResult> GetSurveyRound([FromBody] DataTableRequest request)
        {
            var filter = new GraduateSurveyRoundFilter();
            if (request.Columns != null)
            {
                filter.Name = request.Columns.FirstOrDefault(c => c.Data == "name" || c.Name == "name")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _asEduSurveyGraduateDotKhaoSatService.GetSurveyRound(filter, skip, take);
            return Ok(
                new DataTableResponse<AsEduSurveyGraduateSurveyRound>
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
                var result = await _asEduSurveyGraduateDotKhaoSatService.GetSurveyRoundActive();
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
                var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetSurveyRoundById(id.Value);
                return Ok(surveyRound);
            }
            catch(RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch(Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lấy được đợt khảo sát", detailMessage = mainMessage });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSurveyRound([FromBody] GraduateSurveyRoundCreate surveyRound)
        {
            try
            {
                await _asEduSurveyGraduateDotKhaoSatService.Create(surveyRound);
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
        public async Task<IActionResult> UpdateSurveyRound([Required(AllowEmptyStrings = false)] Guid? id, [FromBody] GraduateSurveyRoundUpdate surveyRound)
        {
            try
            {
                await _asEduSurveyGraduateDotKhaoSatService.Update(id.Value, surveyRound);
            }
            catch (InvalidDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không cập nhật được đợt khảo sát", detailMessage = e.Message });
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
                await _asEduSurveyGraduateDotKhaoSatService.Delete(id.Value);
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
                await _asEduSurveyGraduateDotKhaoSatService.Close(id.Value);
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
