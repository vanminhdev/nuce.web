using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Ctsv;

namespace nuce.web.api.Controllers.Ctsv
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DichVuController : ControllerBase
    {
        private readonly IDichVuService _dichVuService;
        private readonly IThamSoDichVuService _thamSoDichVuService;
        
        public DichVuController(IDichVuService _dichVuService, IThamSoDichVuService _thamSoDichVuService)
        {
            this._dichVuService = _dichVuService;
            this._thamSoDichVuService = _thamSoDichVuService;
        }

        [Route("type/{type}")]
        [HttpGet]
        public IActionResult GetDichVu(int type)
        {
            return Ok(_dichVuService.GetAllByStudent(type));
        }

        [Authorize(Roles = "P_CTSV")]
        [Route("all-type-info")]
        [HttpGet]
        public IActionResult GetAllLoaiDichVuInfo(DateTime? start, DateTime? end)
        {
            return Ok(_dichVuService.GetAllLoaiDichVuInfo(start, end));
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddDichVu([FromBody] DichVuModel model)
        {
            try
            {
                await _dichVuService.AddDichVu(model);
                return Ok();
            }
            catch (DuplicateWaitObjectException ex)
            {
                return Conflict(new ResponseBody { Data = ex, StatusCode = HttpStatusCode.Conflict, Message = "Trùng yêu cầu dịch vụ" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message ?? "Lỗi hệ thống" });
            }
        }

        [AllowAnonymous]
        [Route("export-word/muon-hoc-ba/{studentCode}")]
        [HttpGet]
        public async Task<FileStreamResult> ExportWordMuonHocBa(string studentCode)
        {
            var result = await _dichVuService.ExportWordMuonHocBaAsync(studentCode);
            return new FileStreamResult(new MemoryStream(result), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

        [AllowAnonymous]
        [Route("export-word/uu-dai/{mauSo}")]
        [HttpGet]
        public async Task<FileStreamResult> ExportWordUuDaiSample(int mauSo)
        {
            var result = await _dichVuService.ExportWordUudaiSampleAsync(mauSo);
            return new FileStreamResult(new MemoryStream(result), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

        [Route("admin/muon-hoc-ba/update")]
        [HttpPut]
        public async Task<IActionResult> UpdateMuonHocBa([FromBody] UpdateRequestStatusMuonHocBaGocModel model)
        {
            try
            {
                await _dichVuService.UpdatePartialInfoMuonHocBa(model);
                return Ok();
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message ?? "Lỗi hệ thống" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message ?? "Lỗi hệ thống" });
            }

        }

        //[Authorize(Roles = "P_CTSV")]
        [AllowAnonymous]
        [HttpPost]
        [Route("admin/search-request")]
        public async Task<IActionResult> SearchRequest([FromBody] DataTableRequest request)
        {
            string jsonModel = request.Search.Value;
            try
            {
                var arr = Encoding.UTF8.GetBytes(jsonModel);
                var streamModel = new MemoryStream(arr);

                var model = JsonConvert.DeserializeObject<QuanLyDichVuDetailModel>(jsonModel);
                model.Page = request.Start;
                model.Size = request.Length;

                var rs = await _dichVuService.GetRequestForAdmin(model);
                rs.Draw = ++request.Draw;

                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = "Lỗi hệ thống khi tìm kiếm" });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [Route("admin/update-status")]
        [HttpPut]
        public async Task<IActionResult> UpdateRequestStatus([FromBody] UpdateRequestStatusModel model)
        {
            try
            {
                await _dichVuService.UpdateRequestStatus(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [Route("admin/update-status/multi/four")]
        [HttpPut]
        public async Task<IActionResult> UpdateStatusMultiFour([FromBody] UpdateRequestStatusModel model)
        {
            try
            {
                await _dichVuService.UpdateMultiRequestToFourStatus(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpPost]
        [Route("admin/search-parameters")]
        public async Task<IActionResult> SearchParams([FromBody] DataTableRequest request)
        {
            string id = request.Search.Value;
            try
            {
                int iId = Convert.ToInt32(id);

                var rs = await _dichVuService.GetThamSoByDichVu(iId);
                rs.Draw = ++request.Draw;

                return Ok(rs);
            }
            catch (FormatException ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = "Mã dịch vụ phải là một số" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = "Lỗi hệ thống khi tìm kiếm" });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpPut]
        [Route("admin/update-parameters")]
        public async Task<IActionResult> SearchParams(Dictionary<long, string> model)
        {
            try
            {
                await _dichVuService.UpdateThamSoDichVu(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = "Lỗi hệ thống khi cập nhật tham số" });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpGet]
        [Route("admin/get-types")]
        public IActionResult GetServiceTypes()
        {
            return Ok(_thamSoDichVuService.GetLoaiDichVu());
        }

        [Authorize(Roles = "P_CTSV")]
        [Route("admin/export-word")]
        [HttpPost]
        public async Task<FileStreamResult> ExportWord([FromBody] ExportModel model)
        {
            try
            {
                var result = await _dichVuService.ExportWordAsync(model.DichVuType, model.DichVuList[0].ID);
                return new FileStreamResult(new MemoryStream(result), "application/octet-stream");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [Route("admin/export-word-list")]
        [HttpPost]
        public async Task<FileStreamResult> ExportWordList([FromBody] ExportModel model)
        {
            var result = await _dichVuService.ExportWordListAsync(model.DichVuType, model.DichVuList);
            return new FileStreamResult(new MemoryStream(result), "application/octet-stream");
        }

        [Authorize(Roles = "P_CTSV")]
        [Route("admin/export-excel")]
        [HttpPost]
        public async Task<FileStreamResult> ExportExcel([FromBody] ExportModel model)
        {
            byte[] result;
            if (model.DichVuType == Common.Ctsv.DichVu.DangKyChoO)
            {
                var dotDangKy = await _dichVuService.GetDotDangKyChoOActive();
                result = await _dichVuService.ExportExcelAsync(model.DichVuType, model.DichVuList, dotDangKy.Id);
            }
            else if (model.DichVuType == Common.Ctsv.DichVu.XinMienGiamHocPhi)
            {
                var dotDangKy = await _dichVuService.GetDotXinMienGiamHocPhiActive();
                result = await _dichVuService.ExportExcelAsync(model.DichVuType, model.DichVuList, dotDangKy.Id);
            }
            else if (model.DichVuType == Common.Ctsv.DichVu.DeNghiHoTroChiPhiHocTap)
            {
                var dotDangKy = await _dichVuService.GetDotDeNghiHoTroChiPhiActive();
                result = await _dichVuService.ExportExcelAsync(model.DichVuType, model.DichVuList, dotDangKy.Id);
            }
            else if (model.DichVuType == Common.Ctsv.DichVu.HoTroHocTap)
            {
                var dotDangKy = await _dichVuService.GetDotHoTroHocTapActive();
                result = await _dichVuService.ExportExcelAsync(model.DichVuType, model.DichVuList, dotDangKy.Id);
            }
            else
            {
                result = await _dichVuService.ExportExcelAsync(model.DichVuType, model.DichVuList);
            }
            return new FileStreamResult(new MemoryStream(result), "application/octet-stream");
        }

        [Authorize(Roles = "P_CTSV")]
        [Route("admin/export-excel-overview")]
        [HttpPost]
        public async Task<FileStreamResult> ExportExcelOverView([FromBody] ExportModel model)
        {
            var result = await _dichVuService.ExportExcelOverviewAsync(model.Start, model.End);
            return new FileStreamResult(new MemoryStream(result), "application/octet-stream");
        }

        #region đợt đăng ký chỗ ở
        [Authorize(Roles = "P_CTSV")]
        [HttpPost]
        [Route("admin/dang-ky-cho-o/get-all")]
        public async Task<IActionResult> GetAllDotDangKyNhaO([FromBody] DataTableRequest request)
        {
            var skip = request.Start;
            var take = request.Length;
            var result = await _dichVuService.GetAllDotDangKyChoO(skip, take);
            return Ok(
                new DataTableResponse<AsAcademyStudentSvDangKyChoODot>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpPost]
        [Route("admin/dang-ky-cho-o/add")]
        public async Task<IActionResult> AddDotDangKyNhaO([FromBody] AddDotDangKyChoOModel model)
        {
            try
            {
                await _dichVuService.AddDotDangKyChoO(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpPut]
        [Route("admin/dang-ky-cho-o/update")]
        public async Task<IActionResult> UpdateDotDangKyNhaO(int id, [FromBody] AddDotDangKyChoOModel model)
        {
            try
            {
                await _dichVuService.UpdateDotDangKyChoO(id, model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpDelete]
        [Route("admin/dang-ky-cho-o/delete")]
        public async Task<IActionResult> DeleteDotDangKyNhaO(int id)
        {
            try
            {
                await _dichVuService.DeleteDotDangKyChoO(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }
        #endregion

        #region đợt xin miễn giảm học phí
        [Authorize(Roles = "P_CTSV")]
        [HttpPost]
        [Route("admin/xin-mien-giam-hoc-phi/get-all")]
        public async Task<IActionResult> AllDotXinMienGiamHocPhi([FromBody] DataTableRequest request)
        {
            var skip = request.Start;
            var take = request.Length;
            var result = await _dichVuService.GetAllDotXinMienGiamHocPhi(skip, take);
            return Ok(
                new DataTableResponse<AsAcademyStudentSvXinMienGiamHocPhiDot>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpPost]
        [Route("admin/xin-mien-giam-hoc-phi/add")]
        public async Task<IActionResult> AddDotXinMienGiamHocPhi([FromBody] AddDotXinMienGiamHocPhi model)
        {
            try
            {
                await _dichVuService.AddDotXinMienGiamHocPhi(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpPut]
        [Route("admin/xin-mien-giam-hoc-phi/update")]
        public async Task<IActionResult> UpdateXinMienGiamHocPhi(int id, [FromBody] AddDotXinMienGiamHocPhi model)
        {
            try
            {
                await _dichVuService.UpdateDotXinMienGiamHocPhi(id, model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpDelete]
        [Route("admin/xin-mien-giam-hoc-phi/delete")]
        public async Task<IActionResult> DeleteXinMienGiamHocPhi(int id)
        {
            try
            {
                await _dichVuService.DeleteDotXinMienGiamHocPhi(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }
        #endregion

        #region đợt đề nghị giảm chi phí
        [Authorize(Roles = "P_CTSV")]
        [HttpPost]
        [Route("admin/de-nghi-ho-tro-chi-phi/get-all")]
        public async Task<IActionResult> GetAllDotDeNghiHoTroChiPhi([FromBody] DataTableRequest request)
        {
            var skip = request.Start;
            var take = request.Length;
            var result = await _dichVuService.GetAllDotDeNghiHoTroChiPhi(skip, take);
            return Ok(
                new DataTableResponse<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpPost]
        [Route("admin/de-nghi-ho-tro-chi-phi/add")]
        public async Task<IActionResult> AddDotDeNghiHoTroChiPhi([FromBody] AddDotDeNghiHoTroChiPhi model)
        {
            try
            {
                await _dichVuService.AddDotDeNghiHoTroChiPhi(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpPut]
        [Route("admin/de-nghi-ho-tro-chi-phi/update")]
        public async Task<IActionResult> UpdateDeNghiHoTroChiPhi(int id, [FromBody] AddDotDeNghiHoTroChiPhi model)
        {
            try
            {
                await _dichVuService.UpdateDotDeNghiHoTroChiPhi(id, model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpDelete]
        [Route("admin/de-nghi-ho-tro-chi-phi/delete")]
        public async Task<IActionResult> DeleteDeNghiHoTroChiPhi(int id)
        {
            try
            {
                await _dichVuService.DeleteDotDeNghiHoTroChiPhi(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }
        #endregion

        #region đợt hỗ trợ học tập
        [Authorize(Roles = "P_CTSV")]
        [HttpPost]
        [Route("admin/ho-tro-hoc-tap/get-all")]
        public async Task<IActionResult> GetAllDotHoTroHocTap([FromBody] DataTableRequest request)
        {
            var skip = request.Start;
            var take = request.Length;
            var result = await _dichVuService.GetAllDotHoTroHocTap(skip, take);
            return Ok(
                new DataTableResponse<AsAcademyStudentSvDangKyHoTroHocTapDot>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpPost]
        [Route("admin/ho-tro-hoc-tap/add")]
        public async Task<IActionResult> AddDotHoTroHocTap([FromBody] AddDotHoTroHocTap model)
        {
            try
            {
                await _dichVuService.AddDotHoTroHocTap(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpPut]
        [Route("admin/ho-tro-hoc-tap/update")]
        public async Task<IActionResult> UpdateDotHoTroHocTap(int id, [FromBody] AddDotHoTroHocTap model)
        {
            try
            {
                await _dichVuService.UpdateDotHoTroHocTap(id, model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [HttpDelete]
        [Route("admin/ho-tro-hoc-tap/delete")]
        public async Task<IActionResult> DeleteDotHoTroHocTap(int id)
        {
            try
            {
                await _dichVuService.DeleteDotHoTroHocTap(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Data = ex });
            }
        }
        #endregion
    }
}