using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using nuce.web.api.Common;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly NuceCoreIdentityContext _identityContext;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public object Configuration { get; private set; }

        public UserController(UserManager<IdentityUser> userManager, NuceCoreIdentityContext identityContext,
            ILogger<UserController> logger, IUserService _userService, IConfiguration _configuration)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _logger = logger;
            this._userService = _userService;
            this._configuration = _configuration;
        }

        [HttpPost]
        [Route("login")]
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
                    new CookieOptions() { HttpOnly = true, Expires = accessToken.ValidTo });

                return Ok();
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBody { Status = ResponseBody.ERROR_STATUS, Message = "Tài khoản đã tồn tại!" });

            var user = new IdentityUser()
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
        [Route("changepassword")]
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
        [Route("refreshToken")]
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
            var token = HttpContext.Request.Cookies[UserParameters.JwtRefreshToken];
            
            var principle = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out validatedToken);

            JwtSecurityToken jwtValidatedToken = validatedToken as JwtSecurityToken;

            if (validatedToken != null && jwtValidatedToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                string username = _userService.GetClaimByKey(ClaimTypes.Name);
                bool isStudent = !string.IsNullOrEmpty(_userService.GetCurrentStudentCode());
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
        [Route("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JWT-token");
            return Ok();
        }
    }
}
