using System;
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
        
        public DichVuController(IDichVuService _dichVuService)
        {
            this._dichVuService = _dichVuService;
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
        public IActionResult GetAllLoaiDichVuInfo()
        {
            return Ok(_dichVuService.GetAllLoaiDichVuInfo());
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

        [Authorize(Roles = "P_CTSV")]
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
                var rs = await _dichVuService.UpdateRequestStatus(model);
                if (rs != null)
                {
                    return BadRequest(rs);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Message = "Lỗi hệ thống", Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [Route("admin/update-status/multi/four")]
        [HttpPut]
        public async Task<IActionResult> UpdateStatusMultiFour([FromBody] UpdateStatusMultiFourModel model)
        {
            try
            {
                await _dichVuService.UpdateMultiRequestToFourStatus(model.LoaiDichVu, model.YeuCauList);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Message = ex.Message, Data = ex });
            }
        }

        [Authorize(Roles = "P_CTSV")]
        [Route("admin/export-word")]
        [HttpPost]
        public async Task<FileStreamResult> ExportWord([FromBody] ExportModel model)
        {
            var result = await _dichVuService.ExportWordAsync(model.DichVuType, model.DichVuList[0].ID);
            return new FileStreamResult(new MemoryStream(result), "application/octet-stream");
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
            var result = await _dichVuService.ExportExcelAsync(model.DichVuType, model.DichVuList);
            return new FileStreamResult(new MemoryStream(result), "application/octet-stream");
        }
    }
}