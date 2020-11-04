using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.api.ViewModel;
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

        [Route("all-type-info")]
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
                var addDichVuResult = await _dichVuService.AddDichVu(model);
                if (addDichVuResult != null && addDichVuResult.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return Conflict(addDichVuResult);
                }
                if (addDichVuResult != null)
                {
                    return BadRequest(addDichVuResult);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("admin/search-request")]
        [HttpPost]
        public async Task<IActionResult> SearchRequest([FromBody] QuanLyDichVuDetailModel model)
        {
            return Ok(await _dichVuService.GetRequestForAdmin(model));
        }

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
    }
}