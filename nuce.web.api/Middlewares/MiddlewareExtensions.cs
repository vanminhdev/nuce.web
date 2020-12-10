using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseUserStatusMiddleware(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserStatusMiddleware>();
        }
    }
}
