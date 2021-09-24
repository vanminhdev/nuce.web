using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Shared
{
    public class FakerService
    {
        private readonly HttpContext _httpContext;

        public FakerService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        /// <summary>
        /// chỉ dùng cho DbContext
        /// </summary>
        /// <param name="context"></param>
        public void NoTrackingIfFakeStudent(DbContext context)
        {
            var identity = _httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claimList = identity.FindAll(ClaimTypes.Role);
                if(claimList.FirstOrDefault(o => o.Value == RoleNames.FakeStudent) != null)
                {
                    context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                }
            }
        }
    }
}
