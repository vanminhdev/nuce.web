using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class GraduateSurveyRoundController : ControllerBase
    {
        private readonly ILogger<GraduateSurveyRoundController> _logger;
        private readonly IAsEduSurveyGraduateDotKhaoSatService _asEduSurveyGraduateDotKhaoSatService;
        private readonly IAsEduSurveyGraduateBaiKhaoSatService _asEduSurveyGraduateBaiKhaoSatService;

        public GraduateSurveyRoundController(ILogger<GraduateSurveyRoundController> logger,
            IAsEduSurveyGraduateDotKhaoSatService asEduSurveyGraduateDotKhaoSatService, IAsEduSurveyGraduateBaiKhaoSatService asEduSurveyGraduateBaiKhaoSatService)
        {
            _logger = logger;
            _asEduSurveyGraduateDotKhaoSatService = asEduSurveyGraduateDotKhaoSatService;
            _asEduSurveyGraduateBaiKhaoSatService = asEduSurveyGraduateBaiKhaoSatService;
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
        public async Task<IActionResult> GetSurveyRoundById(
            [Required(AllowEmptyStrings = false)]
            string id)
        {
            try
            {
                var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetSurveyRoundById(id);
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
        public async Task<IActionResult> UpdateSurveyRound([Required(AllowEmptyStrings = false)] string id, [FromBody] GraduateSurveyRoundUpdate surveyRound)
        {
            try
            {
                await _asEduSurveyGraduateDotKhaoSatService.Update(id, surveyRound);
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
        public async Task<IActionResult> DeleteSurveyRound([Required(AllowEmptyStrings = false)] string id)
        {
            try
            {
                await _asEduSurveyGraduateDotKhaoSatService.Delete(id);
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
        public async Task<IActionResult> CloseGraduateSurveyRound([Required(AllowEmptyStrings = false)] string id)
        {
            try
            {
                await _asEduSurveyGraduateDotKhaoSatService.Close(id);
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

        [HttpGet]
        public async Task<IActionResult> GetSurveyRoundActive()
        {
            try
            {
                var surveyRounds = await _asEduSurveyGraduateDotKhaoSatService.GetSurveyRoundActive();
                return Ok(surveyRounds);
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
        #endregion

        #region bài khảo sát
        [HttpPost]
        public async Task<IActionResult> GetTheSurvey([FromBody] DataTableRequest request)
        {
            var filter = new GraduateTheSurveyFilter();
            if (request.Columns != null)
            {
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _asEduSurveyGraduateBaiKhaoSatService.GetTheSurvey(filter, skip, take);
            return Ok(
                new DataTableResponse<GraduateTheSurvey>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetTheSurveyById(
            [Required(AllowEmptyStrings = false)]
            string id)
        {
            try
            {
                var theSurvey = await _asEduSurveyGraduateBaiKhaoSatService.GetTheSurveyById(id);
                return Ok(theSurvey);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lấy được bài khảo sát", detailMessage = mainMessage });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTheSurvey([FromBody] GraduateTheSurveyCreate theSurvey)
        {
            try
            {
                await _asEduSurveyGraduateBaiKhaoSatService.Create(theSurvey);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không tạo được bài khảo sát", detailMessage = e.Message });
            }
            catch (InvalidDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { message = "Không tạo được bài khảo sát", detailMessage = e.Message });
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
        public async Task<IActionResult> UpdateTheSurvey([Required(AllowEmptyStrings = false)] string id, [FromBody] GraduateTheSurveyUpdate theSurvey)
        {
            try
            {
                await _asEduSurveyGraduateBaiKhaoSatService.Update(id, theSurvey);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không cập nhật được bài khảo sát", detailMessage = e.Message });
            }
            catch (InvalidDataException e)
            {
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
                await _asEduSurveyGraduateBaiKhaoSatService.Delete(id);
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
        #endregion
    }
}
