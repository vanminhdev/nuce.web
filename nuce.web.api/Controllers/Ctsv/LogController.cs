using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.api.ViewModel;

namespace nuce.web.api.Controllers.Ctsv
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ICtsvLogService _ctsvLogService;
        public LogController(ICtsvLogService _ctsvLogService)
        {
            this._ctsvLogService = _ctsvLogService;
        }
        [Route("insert-log")]
        [HttpPost]
        public async Task<IActionResult> InsertLog([FromBody] AsLogs model)
        {
            try
            {
                await _ctsvLogService.WriteLog(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}