using Newtonsoft.Json;
using nuce.web.shared;
using nuce.web.survey.student.Common;
using nuce.web.survey.student.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    public class AccountController : BaseController
    {
        //đăng nhập cho sinh viên
        [HttpGet]
        public ActionResult Login(string target)
        {
            ViewData["target"] = target;
            return View(new LoginModel() {
                LoginUserType = 1
            });
        }

        [HttpGet]
        public ActionResult LoginCanBo(int type, string target)
        {
            ViewData["target"] = target;
            if (type != (int)LoginType.Faculty && type != (int)LoginType.Department && type != (int)LoginType.Lecturer)
            {
                return Redirect($"/error?message={HttpUtility.UrlEncode("Trang không hợp lệ")}&code={(int)HttpStatusCode.NotFound}");
            }

            return View(new LoginModel()
            {
                LoginUserType = type
            });
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(new ChangePassword());
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await MakeRequestAuthorizedAsync("Put", "/api/user/ChangePassword", content);

            if (response.IsSuccessStatusCode)
            {
                ViewData["success"] = "Đổi mật khẩu thành công";
                return View(new ChangePassword());
            }
            else
            {
                var message = "Đổi mật khẩu không thành công";
                try
                {
                    var resStr = await response.Content.ReadAsStringAsync();
                    message = (JsonConvert.DeserializeObject<ResponseMessage>(resStr)).message;
                }
                catch
                {
                }
                ViewData["fail"] = message;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel login, string target)
        {
            if (!ModelState.IsValid)
            {
                ViewData["target"] = target;
                return View("login", login);
            }

            var userNamePasswordJsonString = JsonConvert.SerializeObject(new
            {
                username = login.Username,
                password = login.Password,
                loginUserType = login.LoginUserType
            });

            var content = new StringContent(userNamePasswordJsonString, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/user/login", content);

            IEnumerable<Cookie> responseCookies = base.GetAllCookies();

            var accessToken = responseCookies.FirstOrDefault(c => c.Name == UserParameters.JwtAccessToken);
            var refreshToken = responseCookies.FirstOrDefault(c => c.Name == UserParameters.JwtRefreshToken);

            var roles = new List<string>();
            if (accessToken != null)
            {
                Response.Cookies[UserParameters.JwtAccessToken].Value = accessToken.Value;
                Response.Cookies[UserParameters.JwtAccessToken].HttpOnly = true;
                Response.Cookies[UserParameters.JwtAccessToken].Expires = accessToken.Expires;

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(accessToken.Value);
                roles = jwtSecurityToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();
            }

            if (refreshToken != null)
            {
                Response.Cookies[UserParameters.JwtRefreshToken].Value = refreshToken.Value;
                Response.Cookies[UserParameters.JwtRefreshToken].HttpOnly = true;
                Response.Cookies[UserParameters.JwtRefreshToken].Expires = refreshToken.Expires;
            }


            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    if(target == null)
                    {
                        if (roles.Contains(RoleNames.Student) && !roles.Contains(RoleNames.FakeStudent))
                        {
                            return Redirect("/home/index");
                        } 
                        else if (roles.Contains(RoleNames.UndergraduateStudent) && !roles.Contains(RoleNames.FakeStudent))
                        {
                            return Redirect("/home/indexundergraduate");
                        }
                        return Redirect("/default");
                    }
                    else
                    {
                        return Redirect(target);
                    }
                case HttpStatusCode.NotFound:
                    ViewData["LoginMessage"] = "Tài khoản không tồn tại";
                    break;
                case HttpStatusCode.BadGateway:
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["LoginMessage"] = "Hiện tại không thể kết nối đến Đào tạo";
                    ViewData["LoginFailed"] = jsonString;
                    break;
                case HttpStatusCode.InternalServerError:
                    jsonString = await response.Content.ReadAsStringAsync();
                    message = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["LoginMessage"] = message.message;
                    ViewData["LoginFailed"] = jsonString;
                    break;
                case HttpStatusCode.Unauthorized:
                    ViewData["LoginMessage"] = "Tài khoản hoặc mật khẩu không chính xác";
                    break;
                default:
                    jsonString = await response.Content.ReadAsStringAsync();
                    ViewData["LoginMessage"] = "Đăng nhập không thành công";
                    ViewData["LoginFailed"] = jsonString;
                    break;
            }
            ViewData["target"] = target;

            if (login.LoginUserType == (int)LoginType.Faculty || login.LoginUserType == (int)LoginType.Department || login.LoginUserType == (int)LoginType.Lecturer)
            {
                return View("loginCanBo", login);
            }
            return View("login", new LoginModel());
        }

        [HttpGet]
        public ActionResult LoginUndergraduate(string target)
        {
            ViewData["target"] = target;
            return View(new LoginModel() {
                LoginUserType = 1
            });
        }

        [HttpGet]
        public ActionResult LoginGraduate(string target)
        {
            ViewData["target"] = target;
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<ActionResult> LoginGraduate(LoginModel login, string target)
        {
            if (!ModelState.IsValid)
            {
                ViewData["target"] = target;
                return View("logingraduate", new LoginModel());
            }

            var userNamePasswordJsonString = JsonConvert.SerializeObject(new
            {
                username = login.Username,
                password = login.Password,
                loginUserType = 1
            });

            var content = new StringContent(userNamePasswordJsonString, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/user/logingraduate", content);

            IEnumerable<Cookie> responseCookies = base.GetAllCookies();

            var accessToken = responseCookies.FirstOrDefault(c => c.Name == UserParameters.JwtAccessToken);
            var refreshToken = responseCookies.FirstOrDefault(c => c.Name == UserParameters.JwtRefreshToken);

            if (accessToken != null)
            {
                Response.Cookies[UserParameters.JwtAccessToken].Value = accessToken.Value;
                Response.Cookies[UserParameters.JwtAccessToken].HttpOnly = true;
                Response.Cookies[UserParameters.JwtAccessToken].Expires = accessToken.Expires;
            }

            if (refreshToken != null)
            {
                Response.Cookies[UserParameters.JwtRefreshToken].Value = refreshToken.Value;
                Response.Cookies[UserParameters.JwtRefreshToken].HttpOnly = true;
                Response.Cookies[UserParameters.JwtRefreshToken].Expires = refreshToken.Expires;
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    if (target == null)
                    {
                        return Redirect("/default");
                    }
                    else
                    {
                        return Redirect(target);
                    }
                case HttpStatusCode.NotFound:
                    ViewData["LoginMessage"] = "Tài khoản không tồn tại";
                    break;
                case HttpStatusCode.BadGateway:
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["LoginMessage"] = "Hiện tại không thể kết nối đến Đào tạo";
                    ViewData["LoginFailed"] = jsonString;
                    break;
                case HttpStatusCode.InternalServerError:
                    jsonString = await response.Content.ReadAsStringAsync();
                    message = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["LoginMessage"] = message.message;
                    ViewData["LoginFailed"] = jsonString;
                    break;
                case HttpStatusCode.Unauthorized:
                    ViewData["LoginMessage"] = "Tài khoản hoặc mật khẩu không chính xác";
                    break;
                default:
                    jsonString = await response.Content.ReadAsStringAsync();
                    ViewData["LoginMessage"] = "Đăng nhập không thành công";
                    ViewData["LoginFailed"] = jsonString;
                    break;
            }
            ViewData["target"] = target;
            return View("logingraduate", new LoginModel());
        }

        [HttpGet]
        public async Task<ActionResult> Logout(string returnUrl = null)
        {
            await _client.PostAsync($"{API_URL}/api/user/logout", new StringContent(""));
            Response.Cookies[UserParameters.JwtAccessToken].Expires = DateTime.Now.AddDays(-100);
            Response.Cookies[UserParameters.JwtRefreshToken].Expires = DateTime.Now.AddDays(-100);
            Session.Abandon();
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("/default");
            }
        }
    }
}