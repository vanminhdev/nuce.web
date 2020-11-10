using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace nuce.web.api.Models.EduData
{
    public partial class EduDataContext : DbContext
    {
        public EduDataContext()
        {
        }

        public EduDataContext(DbContextOptions<EduDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AsAcademyFaculty> AsAcademyFaculty { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsAcademyFaculty>(entity =>
            {
                entity.ToTable("AS_Academy_Faculty");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.COrder)
                    .HasColumnName("c_Order")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
