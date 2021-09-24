using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.shared;

namespace nuce.web.api.Controllers.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitController : ControllerBase
    {
        private readonly IInitializeService _initializeServce;
        public InitController(IInitializeService _initializeServce)
        {
            this._initializeServce = _initializeServce;
        }

        [AppAuthorize(RoleNames.KhaoThi)]
        [HttpPost]
        [Route("/auto/falculty-deparment/account")]
        public async Task<IActionResult> AutoCreteFalcutyDepartmentAccount()
        {
            try
            {
                await _initializeServce.CreateFacultyDepartmentUsers();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody
                {
                    Data = ex,
                    Message = ex.Message
                });
            }
        }
    }
}