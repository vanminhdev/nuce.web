using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Helper;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Undergraduate;
using nuce.web.shared;
using OfficeOpenXml;
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
    [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate, RoleNames.UndergraduateStudent)]
    public class UndergraduateStudentController : ControllerBase
    {
        private readonly ILogger<UndergraduateStudentController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPathProvider _pathProvider;
        private readonly IAsEduSurveyUndergraduateStudentService _asEduSurveyUndergraduateStudentService;

        public UndergraduateStudentController(ILogger<UndergraduateStudentController> logger, IPathProvider pathProvider, 
            IConfiguration configuration,
            IAsEduSurveyUndergraduateStudentService asEduSurveyUndergraduateStudentService)
        {
            _logger = logger;
            _pathProvider = pathProvider;
            _configuration = configuration;
            _asEduSurveyUndergraduateStudentService = asEduSurveyUndergraduateStudentService;
        }

        [HttpPost]
        public async Task<IActionResult> GetUndergraduateStudent([FromBody] DataTableRequest request)
        {
            var filter = new UndergraduateStudentFilter();
            if (request.Columns != null)
            {
                filter.Masv = request.Columns.FirstOrDefault(c => c.Data == "masv" || c.Name == "masv")?.Search.Value ?? null;

                var dotKhaoSatId = request.Columns.FirstOrDefault(c => c.Data == "dotKhaoSatId" || c.Name == "dotKhaoSatId")?.Search.Value;
                if(dotKhaoSatId != null)
                {
                    filter.DotKhaoSatId = Guid.Parse(dotKhaoSatId);
                }
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _asEduSurveyUndergraduateStudentService.GetAll(filter, skip, take);
            return Ok(
                new DataTableResponse<UndergraduateStudent>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetUndergraduateStudentById(
            [Required(AllowEmptyStrings = false)]
            string id)
        {
            try
            {
                var surveyRound = await _asEduSurveyUndergraduateStudentService.GetById(id);
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
            var path = _pathProvider.MapPath("Templates/Survey/MauUploadTruocTotNghiep.xlsx");
            if (!System.IO.File.Exists(path))
            {
                return NotFound(new { message = "Không tìm thấy file mẫu" });
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
                return BadRequest(new { message = "Phải tải lên file có phần mở rộng .xlsx" });
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
                return BadRequest(new { message = $"File lớn hơn {(long)(maxSize/1024)} KB" });
            }
        }

        private async Task ReadFileUpload(string filepath, Guid surveyRoundId)
        {
            FileInfo fileInfo = new FileInfo(filepath);

            ExcelPackage package = new ExcelPackage(fileInfo);
            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

            // get number of rows and columns in the sheet
            int rows = worksheet.Dimension.Rows;

            for(int i = 2; i <= rows; i++)
            {
                if(worksheet.Cells[i,2].Value == null)
                {
                    rows = i - 1; // dòng cuối cùng
                    break;
                }
            }

            List<AsEduSurveyUndergraduateStudent> students = new List<AsEduSurveyUndergraduateStudent>();
            int maxSize = 100;

            for (int i = 2; i <= rows; i++)
            {
                var masv = worksheet.Cells[i, 2].Value?.ToString();
                var hoten = worksheet.Cells[i, 3].Value?.ToString();
                var lop = worksheet.Cells[i, 4].Value?.ToString();
                var ngaySinh = worksheet.Cells[i, 5].Value?.ToString();
                var gioi = worksheet.Cells[i, 6].Value?.ToString();
                var tichluy = worksheet.Cells[i, 7].Value?.ToString();
                var xepLoai = worksheet.Cells[i, 8].Value?.ToString();
                var tenNganh = worksheet.Cells[i, 9].Value?.ToString();
                var tenChuyenNganh = worksheet.Cells[i, 10].Value?.ToString();

                var heTotNghiep = worksheet.Cells[i, 11].Value?.ToString();
                var maKhoa = worksheet.Cells[i, 12].Value?.ToString();
                var soQuyetDinhVaNgayRaQuyetDinh = worksheet.Cells[i, 13].Value?.ToString();

                string masvFormated = null;
                if (masv != null)
                {
                    masvFormated = $"{masv:0000000}";

                    var student = new AsEduSurveyUndergraduateStudent
                    {
                        Id = Guid.NewGuid(),
                        Masv = masvFormated,
                        Tensinhvien = hoten,
                        Lopqd = lop,
                        Ngaysinh = ngaySinh,
                        Gioitinh = gioi,
                        Tbcht = tichluy,
                        Xeploai = xepLoai,
                        Tennganh = tenNganh,
                        Tenchnga = tenChuyenNganh,
                        Makhoa = maKhoa,
                        Hedaotao = heTotNghiep,
                        ExMasv = masv,
                        DotKhaoSatId = surveyRoundId,
                        Soqdtn = soQuyetDinhVaNgayRaQuyetDinh,
                        Type = 1,
                        Status = 1
                    };
                    students.Add(student);
                }

                if (students.Count == maxSize || i == rows) // đủ 100 hoặc là phần tử cuối cùng
                {
                    await _asEduSurveyUndergraduateStudentService.CreateAll(students);
                    students.Clear();
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                await _asEduSurveyUndergraduateStudentService.TruncateTable();
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
