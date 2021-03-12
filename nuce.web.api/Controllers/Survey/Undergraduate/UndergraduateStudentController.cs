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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey.Undergraduate
{
    [Route("api/[controller]/[action]")]
    [ApiController]
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
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
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
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
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
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
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
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<IActionResult> UploadFile( [Required] IFormFile fileUpload, [Required] Guid? surveyRoundId)
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
            //int maxSize = 100;

            for (int i = 2; i <= rows; i++)
            {
                var masv = worksheet.Cells[i, 2].Value?.ToString();
                var hoten = worksheet.Cells[i, 3].Value?.ToString();
                var lop = worksheet.Cells[i, 4].Value?.ToString();

                DateTime? ngaySinh = null;
                if (worksheet.Cells[i, 5].Value != null)
                {
                    try
                    {
                        ngaySinh = (DateTime)(worksheet.Cells[i, 5].Value);
                    }
                    catch
                    {
                        try
                        {
                            ngaySinh = DateTime.ParseExact((string)worksheet.Cells[i, 5].Value, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                        }
                    }
                }

                var gioi = worksheet.Cells[i, 6].Value?.ToString();
                var tichluy = worksheet.Cells[i, 7].Value?.ToString();
                var xepLoai = worksheet.Cells[i, 8].Value?.ToString();
                var tenNganh = worksheet.Cells[i, 9].Value?.ToString();
                var tenChuyenNganh = worksheet.Cells[i, 10].Value?.ToString();

                var heTotNghiep = worksheet.Cells[i, 11].Value?.ToString();
                var maKhoa = worksheet.Cells[i, 12].Value?.ToString();
                var soQuyetDinhVaNgayRaQuyetDinh = worksheet.Cells[i, 13].Value?.ToString();

                DateTime? ngayRaQd = null;
                if (worksheet.Cells[i, 14].Value != null)
                {
                    try
                    {
                        ngayRaQd = (DateTime)(worksheet.Cells[i, 14].Value);
                    }
                    catch
                    {
                        try
                        {
                            ngayRaQd = DateTime.ParseExact((string)worksheet.Cells[i, 14].Value, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                        }
                    }
                }

                string masvFormated;
                if (masv != null)
                {
                    masvFormated = $"{masv.Trim():0000000}";
                    
                    var student = new AsEduSurveyUndergraduateStudent
                    {
                        Id = Guid.NewGuid(),
                        Masv = masvFormated,
                        Tensinhvien = hoten?.Trim() == "" ? null : hoten?.Trim(),
                        Lopqd = lop?.Trim() == "" ? null : lop?.Trim(),
                        Ngaysinh = ngaySinh,
                        Gioitinh = gioi?.Trim() == "" ? null : gioi?.Trim(),
                        Tbcht = tichluy?.Trim() == "" ? null : tichluy?.Trim(),
                        Xeploai = xepLoai?.Trim() == "" ? null : xepLoai?.Trim(),
                        Tennganh = tenNganh?.Trim() == "" ? null : tenNganh?.Trim(),
                        Tenchnga = tenChuyenNganh?.Trim() == "" ? null : tenChuyenNganh?.Trim(),
                        Makhoa = maKhoa?.Trim() == "" ? null : maKhoa?.Trim(),
                        Hedaotao = heTotNghiep?.Trim() == "" ? null : heTotNghiep?.Trim(),
                        ExMasv = masv.Trim(),
                        DotKhaoSatId = surveyRoundId,
                        Soqdtn = soQuyetDinhVaNgayRaQuyetDinh?.Trim() == "" ? null : soQuyetDinhVaNgayRaQuyetDinh?.Trim(),
                        Ngayraqd = ngayRaQd,
                        Type = 1,
                        Status = 1
                    };
                    students.Add(student);
                }
            }
            await _asEduSurveyUndergraduateStudentService.CreateAll(students);
        }

        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<IActionResult> DownloadListStudent(DateTime? fromDate, DateTime? toDate)
        {
            var data = await _asEduSurveyUndergraduateStudentService.DownloadListStudent(fromDate, toDate);
            return File(data, ContentTypes.Xlsx, "list_student.xlsx");
        }

        [HttpDelete]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<IActionResult> Delete([Required(AllowEmptyStrings = false)] string studentCode)
        {
            try
            {
                await _asEduSurveyUndergraduateStudentService.Delete(studentCode);
                return Ok();
            }
            catch (RecordNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lấy được sinh viên", detailMessage = mainMessage });
            }
        }

        [HttpDelete]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Undergraduate)]
        public IActionResult DeleteAll()
        {
            try
            {
                //await _asEduSurveyUndergraduateStudentService.TruncateTable();
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
