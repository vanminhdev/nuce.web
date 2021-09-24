using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Attributes.ValidationAttributes
{
    public class AppAuthorizeAttribute : AuthorizeAttribute
    {
        public AppAuthorizeAttribute(params string[] roles)
        {
            base.Roles = string.Join(',', roles);
        }
    }
    
}
