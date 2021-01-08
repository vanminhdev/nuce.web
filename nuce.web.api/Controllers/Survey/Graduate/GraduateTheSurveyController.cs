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
    [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate)]
    public class GraduateTheSurveyController : ControllerBase
    {
        private readonly ILogger<GraduateTheSurveyController> _logger;
        private readonly IAsEduSurveyGraduateBaiKhaoSatService _asEduSurveyGraduateBaiKhaoSatService;

        public GraduateTheSurveyController(ILogger<GraduateTheSurveyController> logger, IAsEduSurveyGraduateBaiKhaoSatService asEduSurveyGraduateBaiKhaoSatService)
        {
            _logger = logger;
            _asEduSurveyGraduateBaiKhaoSatService = asEduSurveyGraduateBaiKhaoSatService;
        }

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
            Guid? id)
        {
            try
            {
                var theSurvey = await _asEduSurveyGraduateBaiKhaoSatService.GetTheSurveyById(id.Value);
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
        public async Task<IActionResult> UpdateTheSurvey([Required(AllowEmptyStrings = false)] Guid? id, [FromBody] GraduateTheSurveyUpdate theSurvey)
        {
            try
            {
                await _asEduSurveyGraduateBaiKhaoSatService.Update(id.Value, theSurvey);
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
                await _asEduSurveyGraduateBaiKhaoSatService.Deactive(id.Value);
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
                await _asEduSurveyGraduateBaiKhaoSatService.Delete(id.Value);
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
