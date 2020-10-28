using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.manager.Common;
using nuce.web.manager.Pages.Base;

namespace nuce.web.manager.Pages.Admin.Core
{
    public class LoginModel : PageModelBase<LoginModel>
    {
        public LoginModel(ILogger<LoginModel> logger, IConfiguration configuration) : base (logger, configuration)
        {
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập không được bỏ trống")]
            public string Username { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được bỏ trống")]
            public string Password { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            var userNamePasswordJsonString = JsonSerializer.Serialize(new
            {
                username = Input.Username,
                password = Input.Password
            });

            var content = new StringContent(userNamePasswordJsonString, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await _client.PostAsync("/api/user/login", content);
            
            IEnumerable<Cookie> responseCookies = base.GetAllCookies();

            var accessToken = responseCookies.FirstOrDefault(c => c.Name == UserParameters.JwtAccessToken);
            var refreshToken = responseCookies.FirstOrDefault(c => c.Name == UserParameters.JwtRefreshToken);

            var cookieOptions = new CookieOptions() { HttpOnly = true };
            if (accessToken != null)
                Response.Cookies.Append(UserParameters.JwtAccessToken, accessToken.Value, cookieOptions);
            if (refreshToken != null)
                Response.Cookies.Append(UserParameters.JwtRefreshToken, refreshToken.Value, cookieOptions);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Redirect("/admin/survey/index");
                case HttpStatusCode.Unauthorized:
                    ViewData["LoginMessage"] = "Tài khoản hoặc mật khẩu không chính xác";
                    break;
                default:
                    break;
            }
            return Page();
        }
    }
}
