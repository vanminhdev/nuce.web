using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Core
{
    public class ApplicationRole : IdentityRole<string>
    {
        public string Parent { get; set; }
        public string Description { get; set; }
    }
}
