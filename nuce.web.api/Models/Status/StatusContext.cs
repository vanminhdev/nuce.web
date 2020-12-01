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

        public virtual DbSet<AsAcademySemester> AsAcademySemester { get; set; }
        public virtual DbSet<AsStatusTableTask> AsStatusTableTask { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsAcademySemester>(entity =>
            {
                entity.ToTable("AS_Academy_Semester");

                entity.Property(e => e.CreatedTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<AsStatusTableTask>(entity =>
            {
                entity.ToTable("AS_Status_Table_Task");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Message).HasMaxLength(4000);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
