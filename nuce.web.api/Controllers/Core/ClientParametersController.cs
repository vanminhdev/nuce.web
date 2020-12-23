using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.Models.Core;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Core;

namespace nuce.web.api.Controllers.Core
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,P_KhaoThi,P_CTSV")]
    public class ClientParametersController : ControllerBase
    {
        private readonly IClientParameterService _clientParameterService;
        private readonly IUserService _userService;
        private readonly ILogger<ClientParametersController> _logger;

        public ClientParametersController(IClientParameterService _clientParameterService, IUserService _userService, ILogger<ClientParametersController> _logger)
        {
            this._clientParameterService = _clientParameterService;
            this._userService = _userService;
            this._logger = _logger;
        }
        #region client
        [HttpGet]
        [Route("{type}")]
        [AllowAnonymous]
        public IActionResult GetParametersByType(string type)
        {
            return Ok(_clientParameterService.FindByTypeActive(type));
        }
        #endregion

        #region admin
        [HttpGet]
        [Route("admin/{type}")]
        public IActionResult GetParametersByTypeAdmin(string type)
        {
            var loggedUserRoles = _userService.GetClaimListByKey(ClaimTypes.Role);
            List<ClientParameters> result = new List<ClientParameters>();
            foreach (var role in loggedUserRoles)
            {
                var tmp = _clientParameterService.FindByType(type, role);
                result.AddRange(tmp);
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("admin/update")]
        public async Task<IActionResult> UpdateParametersAdmin(List<UpdateClientParameterModel> model)
        {
            try
            {
                var loggedUserRoles = _userService.GetClaimListByKey(ClaimTypes.Role);
                string username = _userService.GetUserName();
                await _clientParameterService.UpdateParameter(model, loggedUserRoles, username);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new ResponseBody { 
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = ex,
                    Message = ex.Message
                });
            }
        }
        #endregion
    }
}