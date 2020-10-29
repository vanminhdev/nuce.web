using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Models.Core
{
    public partial class NuceCoreIdentityContext : IdentityDbContext<ApplicationUser>
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole[] {
                new IdentityRole{Id = "Admin", Name = "Admin", NormalizedName = "Admin" },
                new IdentityRole{Id = "Department", Name = "Department", NormalizedName = "Department" },
                new IdentityRole{Id = "Faculty", Name = "Faculty", NormalizedName = "Faculty" },
            });
        }
    }
}
