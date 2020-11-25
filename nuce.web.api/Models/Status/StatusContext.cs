using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace nuce.web.api.Models.Status
{
    public partial class StatusContext : DbContext
    {
        public StatusContext()
        {
        }

        public StatusContext(DbContextOptions<StatusContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AsStatusTableTask> AsStatusTableTask { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsStatusTableTask>(entity =>
            {
                entity.ToTable("AS_Status_Table_Task");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.TableName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
