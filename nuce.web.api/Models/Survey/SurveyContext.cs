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

        public virtual DbSet<AsEduSurveyBaiKhaoSat> AsEduSurveyBaiKhaoSat { get; set; }
        public virtual DbSet<AsEduSurveyBaiKhaoSatSinhVien> AsEduSurveyBaiKhaoSatSinhVien { get; set; }
        public virtual DbSet<AsEduSurveyCauHoi> AsEduSurveyCauHoi { get; set; }
        public virtual DbSet<AsEduSurveyCauTrucDe> AsEduSurveyCauTrucDe { get; set; }
        public virtual DbSet<AsEduSurveyDapAn> AsEduSurveyDapAn { get; set; }
        public virtual DbSet<AsEduSurveyDeThi> AsEduSurveyDeThi { get; set; }
        public virtual DbSet<AsEduSurveyDotKhaoSat> AsEduSurveyDotKhaoSat { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsEduSurveyBaiKhaoSat>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_BaiKhaoSat");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeThiId).HasColumnName("DeThiID");

                entity.Property(e => e.DotKhaoSatId).HasColumnName("DotKhaoSatID");

                entity.Property(e => e.GhiChu)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.MoTa)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.NgayBatDau).HasColumnType("date");

                entity.Property(e => e.NgayKetThuc).HasColumnType("date");
            });

            modelBuilder.Entity<AsEduSurveyBaiKhaoSatSinhVien>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_BaiKhaoSat_SinhVien");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BaiKhaoSatId).HasColumnName("BaiKhaoSatID");

                entity.Property(e => e.BaiLam).IsRequired();

                entity.Property(e => e.ClassRoomCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LecturerCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LecturerName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LogIp)
                    .HasColumnName("LogIP")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NgayGioBatDau).HasColumnType("datetime");

                entity.Property(e => e.NgayGioNopBai).HasColumnType("datetime");

                entity.Property(e => e.SinhVienId).HasColumnName("SinhVienID");

                entity.Property(e => e.SubjectCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AsEduSurveyCauHoi>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_CauHoi");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BoCauHoiId).HasColumnName("BoCauHoiID");

                entity.Property(e => e.CheckImport).HasDefaultValueSql("('')");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DoKhoId).HasColumnName("DoKhoID");

                entity.Property(e => e.ExpiredDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.InsertedDate).HasColumnType("datetime");

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

                entity.Property(e => e.DeThiId).HasColumnName("DeThiID");
            });

            modelBuilder.Entity<AsEduSurveyDapAn>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_DapAn");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CauHoiCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CauHoiId).HasColumnName("CauHoiID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10);
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

            modelBuilder.Entity<AsEduSurveyDotKhaoSat>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_DotKhaoSat");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(2000);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
