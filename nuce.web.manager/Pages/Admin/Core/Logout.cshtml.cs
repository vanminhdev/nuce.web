using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.manager.Pages.Base;

namespace nuce.web.manager.Pages.Admin.Core
{
    public class LogoutModel : PageModelBase<LogoutModel>
    {
        public LogoutModel(ILogger<LogoutModel> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            await _client.PostAsync($"{API_URL}/api/user/logout", new StringContent(""));
            Response.Cookies.Delete("JWT-token");
            Response.Cookies.Delete("JWT-refresh-token");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Redirect("/admin/login");
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            await _client.PostAsync($"{API_URL}/api/user/logout", new StringContent(""));
            Response.Cookies.Delete("JWT-token");
            Response.Cookies.Delete("JWT-refresh-token");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Redirect("/admin/login");
            }
        }
    }
}
