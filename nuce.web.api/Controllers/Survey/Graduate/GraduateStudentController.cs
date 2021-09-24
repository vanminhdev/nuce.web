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
using nuce.web.api.ViewModel.Survey.Graduate;
using nuce.web.shared;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey.Graduate
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GraduateStudentController : ControllerBase
    {
        private readonly ILogger<GraduateStudentController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPathProvider _pathProvider;
        private readonly IAsEduSurveyGraduateStudentService _asEduSurveyGraduateStudentService;
        private readonly IAsEduSurveyGraduateDotKhaoSatService _asEduSurveyGraduateDotKhaoSatService;

        public GraduateStudentController(ILogger<GraduateStudentController> logger, IPathProvider pathProvider, 
            IConfiguration configuration,
            IAsEduSurveyGraduateDotKhaoSatService asEduSurveyGraduateDotKhaoSatService,
            IAsEduSurveyGraduateStudentService asEduSurveyGraduateStudentService)
        {
            _logger = logger;
            _pathProvider = pathProvider;
            _configuration = configuration;
            _asEduSurveyGraduateDotKhaoSatService = asEduSurveyGraduateDotKhaoSatService;
            _asEduSurveyGraduateStudentService = asEduSurveyGraduateStudentService;
        }

        [HttpPost]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate, RoleNames.KhaoThi_Survey_KhoaBan)]
        public async Task<IActionResult> GetGraduateStudent([FromBody] DataTableRequest request)
        {
            var filter = new GraduateStudentFilter();
            if (request.Columns != null)
            {
                filter.Masv = request.Columns.FirstOrDefault(c => c.Data == "masv" || c.Name == "masv")?.Search.Value ?? null;
                filter.LopQL = request.Columns.FirstOrDefault(c => c.Data == "lopqd" || c.Name == "lopqd")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;

            var maKhoa = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(UserParameters.UserCode)?.Value;
            if (HttpContext.User.IsInRole(RoleNames.KhaoThi_Survey_KhoaBan) && maKhoa != null) //đợt khảo sát chưa đóng k lấy được danh sách sinh viên
            {
                var surveyRound = await _asEduSurveyGraduateDotKhaoSatService.GetCurrentSurveyRound();
                if(surveyRound == null || !(DateTime.Now >= surveyRound.EndDate)) //đợt đã kết thúc mới lấy được danh sách, nếu chưa kết thúc thì không trả về gì
                {
                    return Ok(new DataTableResponse<GraduateStudent>
                    {
                        Draw = ++request.Draw,
                        RecordsTotal = 0,
                        RecordsFiltered = 0,
                        Data = new List<GraduateStudent>()
                    });
                }
                filter.MaKhoa = maKhoa;
            }

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
        [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate)]
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

        [HttpPut]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<IActionResult> TransferDataFromUndergraduate([Required] Guid? surveyRoundId, [Required] [FromBody] TransferDataUndergraduateModel filter)
        {
            try
            {
                await _asEduSurveyGraduateStudentService.TransferDataFromUndergraduate(surveyRoundId.Value, filter);
                return Ok();
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không chuyển được dữ liệu", detailMessage = mainMessage });
            }
        }

        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate)]
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
        [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate)]
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
            FileInfo fileInfo = new FileInfo(filepath);

            ExcelPackage package = new ExcelPackage(fileInfo);
            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

            // get number of rows and columns in the sheet
            int rows = worksheet.Dimension.Rows;

            for (int i = 2; i <= rows; i++)
            {
                if (worksheet.Cells[i, 2].Value == null)
                {
                    rows = i - 1; // dòng cuối cùng
                    break;
                }
            }

            List<AsEduSurveyGraduateStudent> students = new List<AsEduSurveyGraduateStudent>();
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
                        ngaySinh = DateTime.FromOADate((double)worksheet.Cells[i, 5].Value);
                        if (ngaySinh == null) throw new Exception();
                    }
                    catch
                    {
                        try
                        {
                            ngaySinh = (DateTime)worksheet.Cells[i, 5].Value;
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
                        ngayRaQd = DateTime.FromOADate((double)worksheet.Cells[i, 14].Value);
                        if (ngayRaQd == null) throw new Exception();
                    }
                    catch
                    {
                        try
                        {
                            ngayRaQd = (DateTime)worksheet.Cells[i, 14].Value;
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
                }

                string masvFormated = null;
                if (masv != null)
                {
                    masvFormated = $"{masv.Trim():0000000}";

                    var student = new AsEduSurveyGraduateStudent
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
                        Psw = !string.IsNullOrWhiteSpace(hoten) ? StringHelper.ConvertToLatin(hoten.Trim()).Replace(" ", "").ToLower() : "",
                        Type = 1,
                        Status = 1
                    };
                    students.Add(student);
                }
            }
            await _asEduSurveyGraduateStudentService.CreateAll(students);
        }

        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<IActionResult> DownloadListStudent([Required] Guid? surveyRoundId)
        {
            var data = await _asEduSurveyGraduateStudentService.DownloadListStudent(surveyRoundId.Value);
            return File(data, ContentTypes.Xlsx, "list_student.xlsx");
        }

        [HttpDelete]
        [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<IActionResult> Delete([Required(AllowEmptyStrings = false)] string studentCode)
        {
            try
            {
                await _asEduSurveyGraduateStudentService.Delete(studentCode);
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
        [AppAuthorize(RoleNames.KhaoThi_Survey_Graduate)]
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
