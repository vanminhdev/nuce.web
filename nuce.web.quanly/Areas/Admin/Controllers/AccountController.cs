using Newtonsoft.Json;
using nuce.web.quanly.Areas.Admin.Models;
using nuce.web.quanly.Common;
using nuce.web.quanly.Controllers;
using nuce.web.quanly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var userNamePasswordJsonString = JsonConvert.SerializeObject(new
            {
                username = login.Username,
                password = login.Password
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
            }

            if (refreshToken != null)
            {
                Response.Cookies[UserParameters.JwtRefreshToken].Value = refreshToken.Value;
                Response.Cookies[UserParameters.JwtRefreshToken].HttpOnly = true;
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Response.Cookies["username"].Value = login.Username;
                    Response.Cookies["fullname"].Value = login.Username;
                    return Redirect("/admin/home");
                case HttpStatusCode.Unauthorized:
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var message = (JsonConvert.DeserializeObject<ResponseMessage>(jsonString))?.message ?? "";
                    ViewData["LoginMessage"] = message;
                    break;
                default:
                    break;
            }
            return View();
        }

        public async Task<ActionResult> Logout(string returnUrl = null)
        {
            await _client.PostAsync($"{API_URL}/api/user/logout", new StringContent(""));
            Response.Cookies[UserParameters.JwtAccessToken].Expires = DateTime.Now.AddDays(-100);
            Response.Cookies[UserParameters.JwtRefreshToken].Expires = DateTime.Now.AddDays(-100);
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("/admin/account/login");
            }
        }
    }
}