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
        public NuceCoreIdentityContext(DbContextOptions<NuceCoreIdentityContext> options) : base(options)
        {
        }

        public virtual DbSet<ActiviyLogs> ActiviyLogs { get; set; }
        public virtual DbSet<ManagerBackup> ManagerBackup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ActiviyLogs>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.RoleId).HasMaxLength(400);

                entity.Property(e => e.UserCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ManagerBackup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DatabaseName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Type).HasDefaultValueSql("((1))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
