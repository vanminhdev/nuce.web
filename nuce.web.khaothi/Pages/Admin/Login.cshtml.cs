using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace nuce.web.khaothi.Pages.Admin
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        private readonly string API_URL;

        public LoginModel(ILogger<LoginModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            _configuration = configuration;
            API_URL = configuration.GetValue<string>("API_URL");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Tài khoản không được bỏ trống")]
            public string Username { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được bỏ trống")]
            public string Password { get; set; }
        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userNamePasswordJsonString = JsonSerializer.Serialize(new
            {
                username = Input.Username,
                password = Input.Password
            });

            var content = new StringContent(userNamePasswordJsonString, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{API_URL}/api/user/login", content);

            switch(response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return RedirectToPage("/admin/index");
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
