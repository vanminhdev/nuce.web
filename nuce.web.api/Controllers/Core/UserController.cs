using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EduWebService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using nuce.web.api.Models.Core;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Core.NuceIdentity;
using static EduWebService.ServiceSoapClient;

namespace nuce.web.api.Controllers.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly NuceCoreIdentityContext _identityContext;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            NuceCoreIdentityContext identityContext, IConfiguration configuration, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _identityContext = identityContext;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            bool userIsValid = false;
            if (model.IsStudent)
            {
                ServiceSoapClient srvc = new ServiceSoapClient(EndpointConfiguration.ServiceSoap12);
                var result = await srvc.authenAsync(model.Username, model.Password);

                userIsValid = result != 1;
            } else
            {
                userIsValid = user != null && await _userManager.CheckPasswordAsync(user, model.Password);
            }
            if (userIsValid)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddDays(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                //send token to http only cookies
                Response.Cookies.Append("JWT-token", new JwtSecurityTokenHandler().WriteToken(token),
                    new CookieOptions() { HttpOnly = true, Expires = token.ValidTo });

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

        [HttpGet]
        [Route("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JWT-token");
            return Ok();
        }
    }
}
