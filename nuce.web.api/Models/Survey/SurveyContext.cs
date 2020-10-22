using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace nuce.web.api.Models.Survey
{
    public partial class SurveyContext : DbContext
    {
        public SurveyContext()
        {
        }

        public SurveyContext(DbContextOptions<SurveyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AsEduSurveyCauHoi> AsEduSurveyCauHoi { get; set; }
        public virtual DbSet<AsEduSurveyCauTrucDe> AsEduSurveyCauTrucDe { get; set; }
        public virtual DbSet<AsEduSurveyDapAn> AsEduSurveyDapAn { get; set; }
        public virtual DbSet<AsEduSurveyDeThi> AsEduSurveyDeThi { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=INSPIRON\\SQLEXPRESS;Initial Catalog=NUCE_SURVEY;User ID=sa;Password=1234567");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsEduSurveyCauHoi>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_CauHoi");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BoCauHoiId).HasColumnName("BoCauHoiID");

                entity.Property(e => e.CauHoiId).HasColumnName("CauHoiID");

                entity.Property(e => e.CheckImport).HasDefaultValueSql("('')");

                entity.Property(e => e.DoKhoId).HasColumnName("DoKhoID");

                entity.Property(e => e.ExpiredDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.InsertedDate).HasColumnType("datetime");

                entity.Property(e => e.Ma)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MediaSetting)
                    .HasColumnName("Media_Setting")
                    .HasMaxLength(500);

                entity.Property(e => e.MediaUrl)
                    .HasColumnName("Media_URL")
                    .HasMaxLength(250);

                entity.Property(e => e.Type)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AsEduSurveyCauTrucDe>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_CauTrucDe");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CauHoiId).HasColumnName("CauHoiID");
            });

            modelBuilder.Entity<AsEduSurveyDapAn>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_DapAn");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CauHoiGid).HasColumnName("CauHoiGID");

                entity.Property(e => e.CauHoiId).HasColumnName("CauHoiID");

                entity.Property(e => e.DapAnId).HasColumnName("DapAnID");
            });

            modelBuilder.Entity<AsEduSurveyDeThi>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_DeThi");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DapAn).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NoiDungDeThi).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
