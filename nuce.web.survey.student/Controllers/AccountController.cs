using Newtonsoft.Json;
using nuce.web.survey.student.Common;
using nuce.web.survey.student.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            //Khi access token hết hạn thì vào trang home sẽ được cấp mới
            //if (Request.Cookies[UserParameters.JwtRefreshToken] != null)
            //{
            //    return Redirect("/home");
            //}
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new LoginModel());
            }

            var userNamePasswordJsonString = JsonConvert.SerializeObject(new
            {
                username = login.Username,
                password = login.Password,
                isStudent = true
            });

            var content = new StringContent(userNamePasswordJsonString, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/user/login", content);

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
                    return Redirect("/home");
                case HttpStatusCode.Unauthorized:
                    var jsonString = await response.Content.ReadAsStringAsync();
                    ViewData["LoginMessage"] = "Đăng nhập không thành công";
                    ViewData["LoginFailed"] = jsonString;
                    break;
                default:
                    jsonString = await response.Content.ReadAsStringAsync();
                    ViewData["LoginMessage"] = "Đăng nhập không thành công";
                    ViewData["LoginFailed"] = jsonString;
                    break;
            }
            return View("login", new LoginModel());
        }

        [HttpGet]
        public ActionResult LoginUndergraduate()
        {
            return View(new LoginModel());
        }

        [HttpGet]
        public ActionResult LoginGraduate()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<ActionResult> LoginGraduate(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return View("logingraduate", new LoginModel());
            }

            var userNamePasswordJsonString = JsonConvert.SerializeObject(new
            {
                username = login.Username,
                password = login.Password
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
                    return Redirect("/graduatehome/index");
                case HttpStatusCode.Unauthorized:
                    var jsonString = await response.Content.ReadAsStringAsync();
                    ViewData["LoginFailed"] = jsonString;
                    ViewData["LoginMessage"] = "Đăng nhập không thành công";
                    break;
                default:
                    jsonString = await response.Content.ReadAsStringAsync();
                    ViewData["LoginMessage"] = "Đăng nhập không thành công";
                    ViewData["LoginFailed"] = jsonString;
                    break;
            }
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