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

namespace nuce.web.api.Controllers.Survey.Normal
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "P_KhaoThi")]
    public class TheSurveyController : ControllerBase
    {
        private readonly ILogger<TheSurveyController> _logger;
        private readonly IAsEduSurveyBaiKhaoSatService _asEduSurveyBaiKhaoSatService;

        public TheSurveyController(ILogger<TheSurveyController> logger, IAsEduSurveyBaiKhaoSatService asEduSurveyBaiKhaoSatService)
        {
            _logger = logger;
            _asEduSurveyBaiKhaoSatService = asEduSurveyBaiKhaoSatService;
        }

        #region bài khảo sát
        [HttpPost]
        public async Task<IActionResult> GetTheSurvey([FromBody] DataTableRequest request)
        {
            var filter = new TheSurveyFilter();
            if (request.Columns != null)
            {
                var strDotKhaoSatId = request.Columns.FirstOrDefault(c => c.Data == "surveyRoundName" || c.Name == "surveyRoundName")?.Search.Value ?? null;
                if(strDotKhaoSatId != null)
                {
                    filter.DotKhaoSatId = Guid.Parse(strDotKhaoSatId);
                }
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _asEduSurveyBaiKhaoSatService.GetTheSurvey(filter, skip, take);
            return Ok(
                new DataTableResponse<TheSurvey>
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
            Guid? id)
        {
            try
            {
                var theSurvey = await _asEduSurveyBaiKhaoSatService.GetTheSurveyById(id.Value);
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
        public async Task<IActionResult> CreateTheSurvey([FromBody] TheSurveyCreate theSurvey)
        {
            try
            {
                await _asEduSurveyBaiKhaoSatService.Create(theSurvey);
            }
            catch (InvalidDataException e)
            {
                return Conflict(new { message = e.Message });
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
        public async Task<IActionResult> UpdateTheSurvey([Required(AllowEmptyStrings = false)] Guid? id, [FromBody] TheSurveyUpdate theSurvey)
        {
            try
            {
                await _asEduSurveyBaiKhaoSatService.Update(id.Value, theSurvey);
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

        [HttpPut]
        public async Task<IActionResult> CloseTheSurvey([Required(AllowEmptyStrings = false)] Guid? id)
        {
            try
            {
                await _asEduSurveyBaiKhaoSatService.Deactive(id.Value);
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
        public async Task<IActionResult> DeleteTheSurvey([Required(AllowEmptyStrings = false)] Guid? id)
        {
            try
            {
                await _asEduSurveyBaiKhaoSatService.Delete(id.Value);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy bài khảo sát" });
            }
            catch (InvalidDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
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
