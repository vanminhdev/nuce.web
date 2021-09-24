using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Models.Core
{
    public partial class NuceCoreIdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole[] {
                new ApplicationRole{Id = "Admin", Name = "Admin", NormalizedName = "Admin" },
            });
        }
    }
}
