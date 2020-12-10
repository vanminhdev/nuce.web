using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using nuce.web.api.Common;
using nuce.web.api.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Middlewares
{
    public class UserStatusMiddleware
    {
        private readonly RequestDelegate _next;

        public UserStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> _userManager)
        {
            if (context.User.Identity.Name != null)
            {
                var user = await _userManager.FindByNameAsync(context.User.Identity.Name);
                if (user != null)
                {
                    context.Items["UserStatus"] = user.Status;
                }
            }
            await _next(context);
        }
    }
}
