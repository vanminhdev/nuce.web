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
using nuce.web.api.ViewModel.Core;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core.NuceIdentity;

using System.Collections.Generic;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.shared;
using nuce.web.api.Services.EduData.Interfaces;
using nuce.web.api.Services.Survey.Implements;
using nuce.web.shared.Common;
using nuce.web.api.Services.EduData.Implements;

namespace nuce.web.api.Controllers.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly NuceCoreIdentityContext _identityContext;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogService _logService;
        private readonly AsEduSurveyGraduateStudentService _asEduSurveyGraduateStudentService;
        private readonly AsEduSurveyUndergraduateStudentService _asEduSurveyUndergraduateStudentService;
        private readonly StudentEduDataService _studentEduDataService;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, NuceCoreIdentityContext identityContext,
            ILogger<UserController> logger, IUserService userService, IConfiguration configuration, ILogService logService, 
            AsEduSurveyGraduateStudentService asEduSurveyGraduateStudentService, AsEduSurveyUndergraduateStudentService asEduSurveyUndergraduateStudentService,
            StudentEduDataService studentEduDataService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityContext = identityContext;
            _logger = logger;
            _userService = userService;
            _configuration = configuration;
            _logService = logService;
            _asEduSurveyGraduateStudentService = asEduSurveyGraduateStudentService;
            _asEduSurveyUndergraduateStudentService = asEduSurveyUndergraduateStudentService;
            _studentEduDataService = studentEduDataService;
        }

        [HttpGet]
        [Route("TestAPI")]
        public IActionResult TestAPI()
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            return Ok(new { message = "Hello world!!", your_ip = ip });
        }

        [HttpGet]
        [Authorize]
        [Route("GetAllRole")]
        public async Task<IActionResult> GetAllRole()
        {
            var loggedUserRoles = _userService.GetClaimListByKey(ClaimTypes.Role);
            var roles = new List<ApplicationRole>();
            if (loggedUserRoles.Contains(RoleNames.Admin))
            {
                roles = await _roleManager.Roles.ToListAsync();
            }
            else
            {
                roles = await _roleManager.Roles.Where(r => loggedUserRoles.Contains(r.Parent)).ToListAsync();
            }            
            return Ok(roles);
        }

        private IActionResult FakeStudent(LoginModel model)
        {
            var student = _studentEduDataService.FindByCode(model.Username);
            var authClaims = new List<Claim>
            {
                //new Claim(ClaimTypes.Role, RoleNames.FakeStudent), //vai trò fake student
                new Claim(ClaimTypes.Role, RoleNames.Student),
                new Claim(ClaimTypes.Role, RoleNames.UndergraduateStudent),
                new Claim(ClaimTypes.Role, RoleNames.GraduateStudent),
                new Claim(UserParameters.UserCode, model.Username),
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.GivenName, student?.FullName ?? "ktdb")
            };

            var accessToken = _userService.CreateJWTAccessToken(authClaims);
            var refreshToken = _userService.CreateJWTRefreshToken(authClaims);

            //send token to http only cookies
            Response.Cookies.Append(UserParameters.JwtAccessToken, new JwtSecurityTokenHandler().WriteToken(accessToken),
                new CookieOptions() { HttpOnly = true, Expires = accessToken.ValidTo });
            Response.Cookies.Append(UserParameters.JwtRefreshToken, new JwtSecurityTokenHandler().WriteToken(refreshToken),
                new CookieOptions() { HttpOnly = true, Expires = refreshToken.ValidTo });

            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                #region fake student
                //var fakeStudentPassword = _configuration["FakeStudent:Readonly"];
                var fakeStudentPassword = _configuration.GetSection("FakeStudent:Readonly").AsEnumerable().Select(o => o.Value).ToArray();
                if (fakeStudentPassword.Contains(model.Password) && model.LoginUserType == LoginType.Student)
                {
                    await _logService.WriteLog(new ActivityLogModel
                    {
                        Username = model.Username,
                        LogCode = ActivityLogParameters.CODE_LOGIN,
                        LogMessage = "fake student login"
                    });
                    return FakeStudent(model);
                }
                #endregion

                var isLoginAlwaysSuccess = _configuration.GetValue<bool>("LoginAlwaysSuccess");
                var isSuccess = await _userService.UserLogin(model);
                if (isSuccess || isLoginAlwaysSuccess)
                {
                    var user = await _userService.FindByName(model.Username);
                    var authClaims = await _userService.AddClaimsAsync(model, user);
                    #region xử lý add thêm role cho tài khoản
                    if (model.LoginUserType == LoginType.Student)
                    {
                        //nếu là sinh viên sắp tôt nghiệp thêm role là sắp tốt nghiệp
                        var undergraduateStudent = await _asEduSurveyUndergraduateStudentService.GetByStudentCode(model.Username);
                        if (undergraduateStudent != null)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, RoleNames.UndergraduateStudent));

                            await _logService.WriteLog(new ActivityLogModel
                            {
                                Username = model.Username,
                                LogCode = ActivityLogParameters.CODE_LOGIN,
                                LogMessage = "Sinh viên sắp tốt nghiệp đăng nhập"
                            });
                        }
                        else
                        {
                            await _logService.WriteLog(new ActivityLogModel
                            {
                                Username = model.Username,
                                LogCode = ActivityLogParameters.CODE_LOGIN,
                                LogMessage = "Sinh viên đăng nhập"
                            });
                        }
                    }
                    else if(model.LoginUserType == LoginType.Common)
                    {
                        await _logService.WriteLog(new ActivityLogModel
                        {
                            Username = model.Username,
                            LogCode = ActivityLogParameters.CODE_LOGIN,
                            LogMessage = "Quản trị đăng nhập"
                        });
                    }
                    else if(model.LoginUserType == LoginType.Faculty)
                    {
                        await _logService.WriteLog(new ActivityLogModel
                        {
                            Username = model.Username,
                            LogCode = ActivityLogParameters.CODE_LOGIN,
                            LogMessage = "Khoa đăng nhập"
                        });
                    }
                    else if (model.LoginUserType == LoginType.Department)
                    {
                        await _logService.WriteLog(new ActivityLogModel
                        {
                            Username = model.Username,
                            LogCode = ActivityLogParameters.CODE_LOGIN,
                            LogMessage = "Bộ môn đăng nhập"
                        });
                    }
                    else if (model.LoginUserType == LoginType.Lecturer)
                    {
                        await _logService.WriteLog(new ActivityLogModel
                        {
                            Username = model.Username,
                            LogCode = ActivityLogParameters.CODE_LOGIN,
                            LogMessage = "Giảng viên đăng nhập"
                        });
                    }
                    #endregion

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
            catch (CallEduWebServiceException e)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new { message = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (InvalidInputDataException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Có lỗi xảy ra", detailMessage = mainMessage });
            }
        }

        [HttpPost]
        [Route("Logingraduate")]
        public async Task<IActionResult> LoginGraduate([FromBody] LoginModel model)
        {
            try
            {
                var resultLogin = await _asEduSurveyGraduateStudentService.Login(model.Username, model.Password);
                if (resultLogin.IsSuccess)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.GivenName, resultLogin.HoVaTen),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role, RoleNames.GraduateStudent),
                        new Claim(UserParameters.UserCode, model.Username),
                    };

                    var accessToken = _userService.CreateJWTAccessToken(authClaims);
                    var refreshToken = _userService.CreateJWTRefreshToken(authClaims);

                    //send token to http only cookies
                    Response.Cookies.Append(UserParameters.JwtAccessToken, new JwtSecurityTokenHandler().WriteToken(accessToken),
                        new CookieOptions() { HttpOnly = true, Expires = accessToken.ValidTo });
                    Response.Cookies.Append(UserParameters.JwtRefreshToken, new JwtSecurityTokenHandler().WriteToken(refreshToken),
                        new CookieOptions() { HttpOnly = true, Expires = refreshToken.ValidTo });

                    await _logService.WriteLog(new ActivityLogModel
                    {
                        Username = model.Username,
                        LogCode = ActivityLogParameters.CODE_LOGIN,
                        LogMessage = "Cựu sinh viên đăng nhập"
                    });
                    return Ok();
                }
                return Unauthorized();
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Có lỗi xảy ra", detailMessage = mainMessage });
            }
        }

        [HttpPost]
        [Route("LoginStudentEduEmail")]
        public async Task<IActionResult> LoginStudentEduEmail([FromBody] LoginModel model)
        {
            string email = model.Username;
            var student = _userService.GetStudentByEmail(email);
            if (student == null)
            {
                return NotFound("Sinh viên không tồn tại");
            }
            model.Username = student.Code;
            model.LoginUserType = LoginType.Student;

            var user = new ApplicationUser { UserName = model.Username };

            var authClaims = await _userService.AddClaimsAsync(model, user);
            var accessToken = _userService.CreateJWTAccessToken(authClaims);
            var refreshToken = _userService.CreateJWTRefreshToken(authClaims);

            //send token to http only cookies
            Response.Cookies.Append(UserParameters.JwtAccessToken, new JwtSecurityTokenHandler().WriteToken(accessToken),
                new CookieOptions() { HttpOnly = true, Expires = accessToken.ValidTo });
            Response.Cookies.Append(UserParameters.JwtRefreshToken, new JwtSecurityTokenHandler().WriteToken(refreshToken),
                new CookieOptions() { HttpOnly = true, Expires = refreshToken.ValidTo });

            await _logService.WriteLog(new ActivityLogModel
            {
                Username = model.Username,
                LogCode = ActivityLogParameters.CODE_LOGIN_STUDENT_EDU_EMAIL,
                LogMessage = $"Login bằng email {email}"
            });
            return Ok(student);
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateModel model)
        {
            try
            {
                ApplicationUser user = await _userService.CreateUser(model);
                _logger.LogInformation($"Create success user id: {user.Id}");
                await _logService.WriteLog(new ActivityLogModel
                {
                    LogCode = ActivityLogParameters.CODE_REGISTER,
                    LogMessage = "1",
                });
                return Ok(new { id = user.Id });
            }
            catch (DbUpdateException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBody { Status = ResponseBody.ERROR_STATUS, Message = e.Message });
            }
            catch (SystemException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBody { Status = ResponseBody.ERROR_STATUS, Message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Tạo tài khoản không thành công" });
            }
        }

        [HttpPut]
        [Route("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordModel model)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if(! await _userManager.CheckPasswordAsync(user, model.Password.Trim()))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Mật khẩu cũ không đúng" });
                }
                var result = await _userManager.ChangePasswordAsync(user, model.Password.Trim(), model.NewPassword.Trim());
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không đổi được mật khẩu." });
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

            var refreshToken = HttpContext.Request.Cookies[UserParameters.JwtRefreshToken];
            if (refreshToken == null)
                return Unauthorized();
            var principle = new JwtSecurityTokenHandler().ValidateToken(refreshToken, tokenValidationParameters, out SecurityToken validatedToken);

            JwtSecurityToken jwtValidatedRefreshToken = validatedToken as JwtSecurityToken;
            if (validatedToken != null && jwtValidatedRefreshToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                var claimName = jwtValidatedRefreshToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (claimName == null)
                {
                    return Unauthorized();
                }
                string username = claimName.Value;

                var userInThisSystem = await _userManager.FindByNameAsync(username);
                if (userInThisSystem != null) //dành cho user trong hệ thống
                {
                    if (userInThisSystem.Status == (int)UserStatus.Deactive || userInThisSystem.Status == (int)UserStatus.Deleted)
                    {
                        return Unauthorized();
                    }
                }

                var roles = jwtValidatedRefreshToken.Claims.Where(o => o.Type == ClaimTypes.Role).ToList();
                if (roles.FirstOrDefault(o => o.Value == RoleNames.FakeStudent) != null) //la fake student
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, RoleNames.FakeStudent), //vai trò fake student
                        new Claim(ClaimTypes.Role, RoleNames.Student),
                        new Claim(ClaimTypes.Role, RoleNames.UndergraduateStudent),
                        new Claim(ClaimTypes.Role, RoleNames.GraduateStudent),
                        new Claim(UserParameters.UserCode, username),
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.GivenName, "ktdb")
                    };
                    var accessToken = _userService.CreateJWTAccessToken(authClaims);
                    Response.Cookies.Append(UserParameters.JwtAccessToken, new JwtSecurityTokenHandler().WriteToken(accessToken),
                            new CookieOptions() { HttpOnly = true, Expires = accessToken.ValidTo });
                }
                else
                {
                    LoginType loginUserType = 0;
                    var userType = jwtValidatedRefreshToken.Claims.FirstOrDefault(c => c.Type == UserParameters.UserType);
                    if(userType != null)
                    {
                        Enum.TryParse(userType.Value, out loginUserType);
                    }

                    var model = new LoginModel { Username = username, LoginUserType = loginUserType };
                    var user = await _userService.FindByName(username);
                    var claims = await _userService.AddClaimsAsync(model, user);
                    var accessToken = _userService.CreateJWTAccessToken(claims);
                    Response.Cookies.Append(UserParameters.JwtAccessToken, new JwtSecurityTokenHandler().WriteToken(accessToken),
                            new CookieOptions() { HttpOnly = true, Expires = accessToken.ValidTo });
                }
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

        [AppAuthorize(ApiRole.Account)]
        [HttpPost]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser(
            [FromBody] DataTableRequest request)
        {
            var filter = new UserFilter();
            filter.Username = request.Columns.FirstOrDefault(c => c.Data == "username")?.Search.Value ?? null;
            var skip = request.Start;
            var pageSize = request.Length;
            var result = await _userService.GetAllAsync(filter, skip, pageSize);
            return Ok(
                new DataTableResponse<UserModel>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [AppAuthorize(ApiRole.Account)]
        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById([Required(AllowEmptyStrings = false)] string id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Có lỗi xảy ra" });
            }
        }

        [AppAuthorize(RoleNames.Admin, RoleNames.KhaoThi_Add_Account,
                    RoleNames.KhaoThi_Lock_Account, RoleNames.KhaoThi_Pick_Role)]
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
                return NotFound(new { message = "Không tìm thấy bản ghi" });
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

        [AppAuthorize(RoleNames.Admin, RoleNames.KhaoThi_Add_Account,RoleNames.KhaoThi_Pick_Role)]
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
                return NotFound(new { message = "Không tìm thấy bản ghi" });
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

        [AppAuthorize(RoleNames.Admin, RoleNames.KhaoThi_Delete_Account)]
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
                return NotFound(new { message = "Không tìm thấy bản ghi" });
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

        [AppAuthorize(RoleNames.Admin, RoleNames.KhaoThi_Pick_Role)]
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
                return NotFound(new { message = "Không tìm thấy bản ghi" });
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

        [AppAuthorize(RoleNames.Admin, RoleNames.KhaoThi_Reset_Password)]
        [HttpPut]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(
            [Required(AllowEmptyStrings = false)] string id,
            [FromBody] ResetPasswordModel resetPassword)
        {
            try
            {
                await _userService.ResetPasswordAsync(id, resetPassword.NewPassword);
                return Ok();
            }
            catch (RecordNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy tài khoản" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }


        [Authorize]
        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var username = _userService.GetClaimByKey(ClaimTypes.Name);
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return NotFound(new { message = "Không tìm thấy tài khoản" });
                }
                return Ok(new { user.Email, user.PhoneNumber });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfile profile)
        {
            try
            {
                var username = _userService.GetClaimByKey(ClaimTypes.Name);
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return NotFound(new { message = "Không tìm thấy tài khoản" });
                }
                user.Email = profile.Email;
                user.PhoneNumber = profile.PhoneNumber;
                await _userManager.UpdateAsync(user);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
    }
}
