using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Helper;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Graduate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey.Graduate
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "P_KhaoThi")]
    public class GraduateStudentController : ControllerBase
    {
        private readonly ILogger<GraduateStudentController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPathProvider _pathProvider;
        private readonly IAsEduSurveyGraduateStudentService _asEduSurveyGraduateStudentService;

        public GraduateStudentController(ILogger<GraduateStudentController> logger, IPathProvider pathProvider, 
            IConfiguration configuration,
            IAsEduSurveyGraduateStudentService asEduSurveyGraduateStudentService)
        {
            _logger = logger;
            _pathProvider = pathProvider;
            _configuration = configuration;
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
                new DataTableResponse<GraduateStudent>
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

        [HttpGet]
        public async Task<IActionResult> DownloadTemplateUploadFile()
        {
            var path = _pathProvider.MapPath("Templates/Survey/Template_Graduate_Student.xlsx");
            if (!System.IO.File.Exists(path))
            {
                return NotFound(new { message = "Không tìm thấy file" });
            }
            var templateContent = await System.IO.File.ReadAllBytesAsync(path);
            return File(templateContent, ContentTypes.Xlsx, Path.GetFileName(path));
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(
            [Required]
            IFormFile fileUpload, 
            [Required]
            Guid? surveyRoundId)
        {
            if(fileUpload.ContentType != ContentTypes.Xlsx)
            {
                return BadRequest(new { message = "phải tải lên file có phần mở rộng .xlsx" });
            }
            long maxSize = _configuration.GetValue<long>("MaxSizeFileUpload");
            if (fileUpload.Length <= maxSize)
            {
                try
                {
                    var filePath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".xlsx";
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await fileUpload.CopyToAsync(stream);
                    }
                    await ReadFileUpload(filePath, surveyRoundId.Value);
                    System.IO.File.Delete(filePath);
                }
                catch (Exception e)
                {
                    var mainMessage = UtilsException.GetMainMessage(e);
                    _logger.LogError(e, mainMessage);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tải lên được file", detailMessage = mainMessage });
                }
                return Ok();
            }
            else
            {
                return BadRequest(new { message = $"file lớn hơn {(long)(maxSize/1024)} KB" });
            }
        }

        private async Task ReadFileUpload(string filepath, Guid surveyRoundId)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = ExcelFile.Load(filepath);
            var worksheet = workbook.Worksheets[0];

            /// Create DataTable from an Excel worksheet.
            var dataTable = worksheet.CreateDataTable(new CreateDataTableOptions()
            {
                ColumnHeaders = true,
                StartRow = 0,
                NumberOfColumns = worksheet.Columns.Count,
                NumberOfRows = worksheet.Rows.Count,
                Resolution = ColumnTypeResolution.AutoPreferStringCurrentCulture
            });

            List<AsEduSurveyGraduateStudent> students = new List<AsEduSurveyGraduateStudent>();
            AsEduSurveyGraduateStudent student;
            int maxSize = 100;
            int numRowCount = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                var dottonghiep = row[0].ToString();
                var sovaoso = row[1].ToString();
                var masv = row[2].ToString();

                string addStr = "";
                for(int i = 0; i < 7 - masv.Length; i++)
                {
                    addStr += "0";
                }
                string masvFormated = addStr + masv;

                var hovaten = row[3].ToString();
                var xeploai = row[4].ToString();
                var ngaysinh = row[5].ToString();

                student = new AsEduSurveyGraduateStudent
                {
                    Id = Guid.NewGuid(),
                    Dottotnghiep = dottonghiep,
                    Sovaoso = sovaoso,
                    Masv = masvFormated,
                    Tensinhvien = hovaten,
                    Xeploai = xeploai,
                    Ngaysinh = ngaysinh,
                    ExMasv = masv,
                    Psw = StringHelper.ConvertToLatin(hovaten).Replace(" ", "").ToLower(),
                    DotKhaoSatId = surveyRoundId,
                    Type = 1,
                    Status = 1
                };
                students.Add(student);
                numRowCount++;
                if (students.Count == maxSize || numRowCount == dataTable.Rows.Count) // đủ 100 hoặc là phần tử cuối cùng
                {
                    await _asEduSurveyGraduateStudentService.CreateAll(students);
                    students.Clear();
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                await _asEduSurveyGraduateStudentService.TruncateTable();
                return Ok();
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
