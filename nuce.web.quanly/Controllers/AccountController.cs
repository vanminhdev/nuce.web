﻿using Newtonsoft.Json;
using nuce.web.quanly.Attributes.ActionFilter;
using nuce.web.quanly.Common;
using nuce.web.quanly.Controllers;
using nuce.web.quanly.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Index()
        {
            //Khi access token hết hạn thì vào trang home sẽ được cấp mới
            if(Request.Cookies[UserParameters.JwtRefreshToken] != null)
            {
                return Redirect("/home");
            }
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
                case HttpStatusCode.NotFound:
                    ViewData["LoginMessage"] = "Tài khoản không tồn tại";
                    break;
                case HttpStatusCode.InternalServerError:
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
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
            return View("Index", new LoginModel());
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
                return Redirect("/account");
            }
        }

        [HttpGet]
        [AuthorizeActionFilter]
        public async Task<ActionResult> ProfileDetail()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/User/GetUserProfile");
            return await base.HandleResponseAsync(response,
                action200Async: async (res) =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    var profile = JsonConvert.DeserializeObject<Profile>(jsonString);
                    var profileDetail = new ProfileDetail()
                    {
                        UpdateProfile = profile
                    };
                    return View(profileDetail);
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter]
        public async Task<ActionResult> UpdateProfile(ProfileDetail profileDetail)
        {
            base.RemoveValidMessagePartialModel<ChangePassword>(profileDetail.Password, "Password");
            if (!base.IsValidPartialModel<Profile>(profileDetail.UpdateProfile, "UpdateProfile"))
            {
                return View("ProfileDetail", profileDetail);
            }
            var stringContent = new StringContent(JsonConvert.SerializeObject(profileDetail.UpdateProfile), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/User/UpdateUserProfile", stringContent);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Cập nhật thành công";
                    return View("ProfileDetail", profileDetail);
                },
                action500Async: async res =>
                {
                    ViewData["UpdateErrorMessage"] = "Cập nhật không thành công";
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["UpdateErrorMessageDetail"] = resMess.message;
                    return View("ProfileDetail", profileDetail);
                },
                action400: res =>
                {
                    ViewData["UpdateErrorMessage"] = "Cập nhật không thành công";
                    ViewData["UpdateErrorMessageDetail"] = "Dữ liệu đã nhập không hợp lệ";
                    return View("ProfileDetail", profileDetail);
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter]
        public async Task<ActionResult> ChangePassword(ProfileDetail profileDetail)
        {
            base.RemoveValidMessagePartialModel<Profile>(profileDetail.UpdateProfile, "UpdateProfile");
            if (!base.IsValidPartialModel<ChangePassword>(profileDetail.Password, "Password"))
            {
                return View("ProfileDetail", profileDetail);
            }
            var stringContent = new StringContent(JsonConvert.SerializeObject(profileDetail.Password), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/User/ChangePassword", stringContent);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["ChangePasswordSuccessMessage"] = "Đổi mật khẩu thành công";
                    return View("ProfileDetail", profileDetail);
                },
                action500Async: async res =>
                {
                    ViewData["ChangePasswordErrorMessage"] = "Đổi mật khẩu không thành công";
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["ChangePasswordErrorMessageDetail"] = resMess.message;
                    return View("ProfileDetail", profileDetail);
                },
                action400: res =>
                {
                    ViewData["ChangePasswordErrorMessage"] = "Đổi mật khẩu không thành công";
                    ViewData["ChangePasswordErrorMessageDetail"] = "Dữ liệu đã nhập không hợp lệ";
                    return View("ProfileDetail", profileDetail);
                }
            );
        }
    }
}