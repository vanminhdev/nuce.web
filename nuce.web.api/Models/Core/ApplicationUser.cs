using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Models.Core
{
    public class ApplicationUser : IdentityUser
    {
        public int Status { get; set; }
        public string ExCode { get; set; }
    }
}
