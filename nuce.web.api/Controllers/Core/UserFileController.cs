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
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.shared;

namespace nuce.web.api.Controllers.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFileController : ControllerBase
    {
        private readonly IUserFileService _userFileService;
        private readonly IUserService _userService;
        private readonly ILogger<UserFileController> _logger;
        public UserFileController(IUserFileService _userFileService, IUserService _userService, ILogger<UserFileController> _logger)
        {
            this._userFileService = _userFileService;
            this._userService = _userService;
            this._logger = _logger;
        }

        
        #region client
        /// <summary>
        /// Lấy ảnh cho website 
        /// </summary>
        /// <param name="filecode"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("image/{filecode}")]
        public async Task<IActionResult> GetImage(string filecode, [FromQuery] int? width, [FromQuery] int? height)
        {
            try
            {
                var result = await _userFileService.GetUserImage(filecode, width, height);
                if (result != null)
                {
                    return File(result.Data, $"image/{result.Extension}");
                }
                return null;
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }
        #endregion

        #region admin
        /// <summary>
        /// Admin up ảnh lên cho website
        /// </summary>
        /// <param name="code"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [AppAuthorize(RoleNames.KhaoThi_Upload_WebImage)]
        [HttpPost]
        [Route("upload/image/{code}")]
        public async Task<IActionResult> UploadImage(string code, IFormFile file)
        {
            try
            {
                var loggedUserRoles = _userService.GetClaimListByKey(ClaimTypes.Role);
                string username = _userService.GetUserName();
                await _userFileService.UploadUserImage(file, code, username, loggedUserRoles);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new ResponseBody
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = ex,
                    Message = ex.Message
                });
            }
        }
        #endregion
    }
}