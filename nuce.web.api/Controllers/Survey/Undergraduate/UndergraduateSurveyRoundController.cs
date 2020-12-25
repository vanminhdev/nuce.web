using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Undergraduate;
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
    public class UndergraduateSurveyRoundController : ControllerBase
    {
        private readonly ILogger<GraduateSurveyRoundController> _logger;
        private readonly IAsEduSurveyUndergraduateDotKhaoSatService _asEduSurveyUndergraduateDotKhaoSatService;

        public UndergraduateSurveyRoundController(ILogger<GraduateSurveyRoundController> logger, IAsEduSurveyUndergraduateDotKhaoSatService asEduSurveyUndergraduateDotKhaoSatService)
        {
            _logger = logger;
            _asEduSurveyUndergraduateDotKhaoSatService = asEduSurveyUndergraduateDotKhaoSatService;
        }

        #region đợt khảo sát
        [HttpPost]
        public async Task<IActionResult> GetSurveyRound([FromBody] DataTableRequest request)
        {
            var filter = new UndergraduateSurveyRoundFilter();
            if (request.Columns != null)
            {
                filter.Name = request.Columns.FirstOrDefault(c => c.Data == "name" || c.Name == "name")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _asEduSurveyUndergraduateDotKhaoSatService.GetSurveyRound(filter, skip, take);
            return Ok(
                new DataTableResponse<AsEduSurveyUndergraduateSurveyRound>
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
                var result = await _asEduSurveyUndergraduateDotKhaoSatService.GetSurveyRoundActive();
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
        public async Task<IActionResult> GetAllSurveyRound()
        {
            try
            {
                var result = await _asEduSurveyUndergraduateDotKhaoSatService.GetAllSurveyRound();
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
            string id)
        {
            try
            {
                var surveyRound = await _asEduSurveyUndergraduateDotKhaoSatService.GetSurveyRoundById(id);
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
        public async Task<IActionResult> CreateSurveyRound([FromBody] UndergraduateSurveyRoundCreate surveyRound)
        {
            try
            {
                await _asEduSurveyUndergraduateDotKhaoSatService.Create(surveyRound);
            }
            catch (InvalidDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
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
        public async Task<IActionResult> UpdateSurveyRound([Required(AllowEmptyStrings = false)] Guid? id, [FromBody] UndergraduateSurveyRoundUpdate surveyRound)
        {
            try
            {
                await _asEduSurveyUndergraduateDotKhaoSatService.Update(id.Value, surveyRound);
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
                await _asEduSurveyUndergraduateDotKhaoSatService.Delete(id.Value);
            }
            catch (InvalidDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
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
        public async Task<IActionResult> OpenSurveyRound([Required(AllowEmptyStrings = false)] Guid? id)
        {
            try
            {
                await _asEduSurveyUndergraduateDotKhaoSatService.Open(id.Value);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy đợt khảo sát" });
            }
            catch (InvalidDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không mở cửa được đợt khảo sát", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không mở cửa được đợt khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> CloseSurveyRound([Required(AllowEmptyStrings = false)] Guid? id)
        {
            try
            {
                await _asEduSurveyUndergraduateDotKhaoSatService.Close(id.Value);
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
