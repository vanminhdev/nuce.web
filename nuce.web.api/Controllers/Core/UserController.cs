using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Core;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Core.NuceIdentity;

namespace nuce.web.api.Controllers.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly NuceCoreIdentityContext _identityContext;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public object Configuration { get; private set; }

        public UserController(UserManager<ApplicationUser> userManager, NuceCoreIdentityContext identityContext,
            ILogger<UserController> logger, IUserService _userService, IConfiguration _configuration)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _logger = logger;
            this._userService = _userService;
            this._configuration = _configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.FindByNameAsync(model.Username);
            bool userIsValid = await _userService.UserIsvalidAsync(model, user);
            if (userIsValid)
            {
                var authClaims = await _userService.AddClaimsAsync(model, user);
                var accessToken = _userService.CreateJWTAccessToken(authClaims);
                var refreshToken = _userService.CreateJWTRefreshToken(authClaims);

                //send token to http only cookies
                Response.Cookies.Append(UserParameters.JwtAccessToken, new JwtSecurityTokenHandler().WriteToken(accessToken),
                    new CookieOptions() { HttpOnly = true, Expires = accessToken.ValidTo });
                Response.Cookies.Append(UserParameters.JwtRefreshToken, new JwtSecurityTokenHandler().WriteToken(refreshToken),
                    new CookieOptions() { HttpOnly = true, Expires = refreshToken.ValidTo });

                return Ok();
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBody { Status = ResponseBody.ERROR_STATUS, Message = "Tài khoản đã tồn tại!" });

            var user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            using (var transaction = _identityContext.Database.BeginTransaction())
            {
                try
                {
                    //tạo tài khoản
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBody { Status = ResponseBody.ERROR_STATUS, Message = "Không tạo được tài khoản" });

                    //thêm vai trò
                    var resultAddRole = await _userManager.AddToRoleAsync(user, model.Role);

                    transaction.Commit();
                    _logger.LogInformation($"Create success user id: {user.Id}");
                    return Ok(new ResponseBody { Status = ResponseBody.SUCCESS_STATUS, Message = "Tạo tài khoản thành công!" });
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogWarning(e, e.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBody { Status = ResponseBody.ERROR_STATUS, Message = e.Message });
                }
            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordModel model)
        {
            
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(new ResponseBody { Status = ResponseBody.SUCCESS_STATUS, Message = "Đổi mật khẩu thành công." });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new ResponseBody { Status = ResponseBody.ERROR_STATUS, Message = "Không đổi được mật khẩu." });
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ClockSkew = TimeSpan.FromDays(999) //expiration token
            };

            SecurityToken validatedToken;
            var refreshToken = HttpContext.Request.Cookies[UserParameters.JwtRefreshToken];
            if(refreshToken == null)
                return Unauthorized();
            var principle = new JwtSecurityTokenHandler().ValidateToken(refreshToken, tokenValidationParameters, out validatedToken);

            JwtSecurityToken jwtValidatedToken = validatedToken as JwtSecurityToken;
            if (validatedToken != null && jwtValidatedToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                var claimName = jwtValidatedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (claimName == null)
                {
                    return Unauthorized();
                }
                string username = claimName.Value;
                var claimStudent = jwtValidatedToken.Claims.FirstOrDefault(c => c.Type == UserParameters.MSSV);
                bool isStudent = claimStudent != null;
                var model = new LoginModel { Username = username, IsStudent = isStudent };
                var user = await _userService.FindByNameAsync(username);
                var claims = await _userService.AddClaimsAsync(model, user);
                
                var accessToken = _userService.CreateJWTAccessToken(claims);
                Response.Cookies.Append(UserParameters.JwtAccessToken, new JwtSecurityTokenHandler().WriteToken(accessToken),
                        new CookieOptions() { HttpOnly = true, Expires = accessToken.ValidTo });
                return Ok();
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(UserParameters.JwtAccessToken);
            Response.Cookies.Delete(UserParameters.JwtRefreshToken);
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser(
            [Range(1, int.MaxValue)]
            int pageNumber = 1,
            [Range(1, int.MaxValue)]
            int pageSize = 20)
        {
            var result = await _userService.GetAllAsync(new UserFilter(), pageNumber, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetUserById([Required(AllowEmptyStrings = false)] string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Record not found" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("ActiveUser")]
        public async Task<IActionResult> ActiveUser([Required(AllowEmptyStrings = false)] string id)
        {
            try
            {
                await _userService.ActiveUserAsync(id);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Record not found" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("DeactiveUser")]
        public async Task<IActionResult> DeactiveUser([Required(AllowEmptyStrings = false)] string id)
        {
            try
            {
                await _userService.DeactiveUserAsync(id);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Record not found" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser([Required(AllowEmptyStrings = false)] string id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Record not found" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(
            [Required(AllowEmptyStrings = false)] string id,
            [FromBody] UserUpdateModel user)
        {
            try
            {
                await _userService.UpdateUserAsync(id, user);
                return Ok();
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Record not found" });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(
            [Required(AllowEmptyStrings = false)] string id,
            [Required(AllowEmptyStrings = false)] string newPassword)
        {
            try
            {
                await _userService.ResetPasswordAsync(id, newPassword);
                return Ok();
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Record not found" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
    }
}
