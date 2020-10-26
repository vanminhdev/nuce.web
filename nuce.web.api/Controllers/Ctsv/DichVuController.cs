using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nuce.web.api.Services.Ctsv.Interfaces;
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
            return Ok(_dichVuService.GetAll(type));
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}