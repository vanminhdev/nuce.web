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
        public virtual DbSet<AsEduSurveyGraduateBaiKhaoSat> AsEduSurveyGraduateBaiKhaoSat { get; set; }
        public virtual DbSet<AsEduSurveyGraduateBaiKhaoSatSinhVien> AsEduSurveyGraduateBaiKhaoSatSinhVien { get; set; }
        public virtual DbSet<AsEduSurveyGraduateCauHoi> AsEduSurveyGraduateCauHoi { get; set; }
        public virtual DbSet<AsEduSurveyGraduateCauTrucDe> AsEduSurveyGraduateCauTrucDe { get; set; }
        public virtual DbSet<AsEduSurveyGraduateDapAn> AsEduSurveyGraduateDapAn { get; set; }
        public virtual DbSet<AsEduSurveyGraduateDeThi> AsEduSurveyGraduateDeThi { get; set; }
        public virtual DbSet<AsEduSurveyGraduateStudent> AsEduSurveyGraduateStudent { get; set; }
        public virtual DbSet<AsEduSurveyGraduateSurveyRound> AsEduSurveyGraduateSurveyRound { get; set; }
        public virtual DbSet<AsEduSurveyReportTotal> AsEduSurveyReportTotal { get; set; }
        public virtual DbSet<AsEduSurveyUndergraduateBaiKhaoSat> AsEduSurveyUndergraduateBaiKhaoSat { get; set; }
        public virtual DbSet<AsEduSurveyUndergraduateBaiKhaoSatSinhVien> AsEduSurveyUndergraduateBaiKhaoSatSinhVien { get; set; }
        public virtual DbSet<AsEduSurveyUndergraduateCauHoi> AsEduSurveyUndergraduateCauHoi { get; set; }
        public virtual DbSet<AsEduSurveyUndergraduateCauTrucDe> AsEduSurveyUndergraduateCauTrucDe { get; set; }
        public virtual DbSet<AsEduSurveyUndergraduateDapAn> AsEduSurveyUndergraduateDapAn { get; set; }
        public virtual DbSet<AsEduSurveyUndergraduateDeThi> AsEduSurveyUndergraduateDeThi { get; set; }
        public virtual DbSet<AsEduSurveyUndergraduateReportTotal> AsEduSurveyUndergraduateReportTotal { get; set; }
        public virtual DbSet<AsEduSurveyUndergraduateStudent> AsEduSurveyUndergraduateStudent { get; set; }
        public virtual DbSet<AsEduSurveyUndergraduateSurveyRound> AsEduSurveyUndergraduateSurveyRound { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=NUCE_SURVEY;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsEduSurveyBaiKhaoSat>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_BaiKhaoSat");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeThiId).HasColumnName("DeThiID");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.DotKhaoSatId).HasColumnName("DotKhaoSatID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(100);
            });

            modelBuilder.Entity<AsEduSurveyBaiKhaoSatSinhVien>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_BaiKhaoSat_SinhVien");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.Status })
                    .HasName("index_baikhaosatsinhvien_baiks_trangthai");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.LecturerCode, e.ClassRoomCode })
                    .HasName("index_baikhaosatsinhvien_baiks_giangvien_lop");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.SubjectCode, e.LecturerCode, e.Status })
                    .HasName("IX_baikhaosatsinhvien_ThongKe_GiangVien");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.SubjectCode, e.StudentCode, e.Status })
                    .HasName("IX_baikhaosatsinhvien_ThongKe_SinhVien");

                entity.HasIndex(e => new { e.StudentCode, e.ClassRoomCode, e.Nhhk, e.Status })
                    .HasName("index_As_Edu_survey_baikhaosatsinhvien");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BaiKhaoSatId).HasColumnName("BaiKhaoSatID");

                entity.Property(e => e.BaiLam).IsRequired();

                entity.Property(e => e.ClassRoomCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeThi).IsRequired();

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

                entity.Property(e => e.Nhhk)
                    .IsRequired()
                    .HasColumnName("NHHK")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

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

                entity.Property(e => e.ParentCode).HasMaxLength(50);

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
                    .HasMaxLength(50);

                entity.Property(e => e.CauHoiId).HasColumnName("CauHoiID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.HideQuestion).HasMaxLength(2000);

                entity.Property(e => e.ShowQuestion).HasMaxLength(2000);
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

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<AsEduSurveyDotKhaoSat>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_DotKhaoSat");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(100);
            });

            modelBuilder.Entity<AsEduSurveyGraduateBaiKhaoSat>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Graduate_BaiKhaoSat");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeThiId).HasColumnName("DeThiID");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.DotKhaoSatId).HasColumnName("DotKhaoSatID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(2000);
            });

            modelBuilder.Entity<AsEduSurveyGraduateBaiKhaoSatSinhVien>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Graduate_BaiKhaoSat_SinhVien");

                entity.HasIndex(e => new { e.StudentCode, e.BaiKhaoSatId, e.Status })
                    .HasName("index_Graduate_BaiKhaoSat_SinhVien");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BaiKhaoSatId).HasColumnName("BaiKhaoSatID");

                entity.Property(e => e.BaiLam).IsRequired();

                entity.Property(e => e.DeThi).IsRequired();

                entity.Property(e => e.DepartmentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LogIp)
                    .HasColumnName("LogIP")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NgayGioBatDau).HasColumnType("datetime");

                entity.Property(e => e.NgayGioNopBai).HasColumnType("datetime");

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AsEduSurveyGraduateCauHoi>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Graduate_CauHoi");

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

                entity.Property(e => e.ParentCode).HasMaxLength(50);

                entity.Property(e => e.Type)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AsEduSurveyGraduateCauTrucDe>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Graduate_CauTrucDe");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CauHoiId).HasColumnName("CauHoiID");

                entity.Property(e => e.DeThiId).HasColumnName("DeThiID");
            });

            modelBuilder.Entity<AsEduSurveyGraduateDapAn>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Graduate_DapAn");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CauHoiCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CauHoiId).HasColumnName("CauHoiID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.HideQuestion).HasMaxLength(2000);

                entity.Property(e => e.ShowQuestion).HasMaxLength(2000);
            });

            modelBuilder.Entity<AsEduSurveyGraduateDeThi>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Graduate_DeThi");

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

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<AsEduSurveyGraduateStudent>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Graduate_Student");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Bangclc)
                    .HasColumnName("bangclc")
                    .HasMaxLength(50);

                entity.Property(e => e.Checksum)
                    .HasColumnName("checksum")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Dottotnghiep)
                    .HasColumnName("dottotnghiep")
                    .HasMaxLength(100);

                entity.Property(e => e.Dtoc)
                    .HasColumnName("dtoc")
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email1)
                    .HasColumnName("email1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email2)
                    .HasColumnName("email2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExMasv)
                    .IsRequired()
                    .HasColumnName("ex_masv")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GhichuThi)
                    .HasColumnName("ghichu_thi")
                    .HasMaxLength(200);

                entity.Property(e => e.Gioitinh)
                    .HasColumnName("gioitinh")
                    .HasMaxLength(20);

                entity.Property(e => e.Hedaotao)
                    .HasColumnName("hedaotao")
                    .HasMaxLength(50);

                entity.Property(e => e.K)
                    .HasColumnName("k")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Khoahoc)
                    .HasColumnName("khoahoc")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Lop12)
                    .HasColumnName("lop12")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Lopqd)
                    .HasColumnName("lopqd")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Manganh)
                    .HasColumnName("manganh")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Masv)
                    .IsRequired()
                    .HasColumnName("masv")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile1)
                    .HasColumnName("mobile1")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile2)
                    .HasColumnName("mobile2")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Namtn)
                    .HasColumnName("namtn")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ngaysinh)
                    .HasColumnName("ngaysinh")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Noisiti)
                    .HasColumnName("noisiti")
                    .HasMaxLength(200);

                entity.Property(e => e.Psw)
                    .IsRequired()
                    .HasColumnName("psw")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Quoctich)
                    .HasColumnName("quoctich")
                    .HasMaxLength(30);

                entity.Property(e => e.Sobaodanh)
                    .HasColumnName("sobaodanh")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Sohieuba)
                    .HasColumnName("sohieuba")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Soqdtn)
                    .HasColumnName("soqdtn")
                    .HasMaxLength(100);

                entity.Property(e => e.Sovaoso)
                    .HasColumnName("sovaoso")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Tbcht)
                    .HasColumnName("tbcht")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Tcong)
                    .HasColumnName("tcong")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Tenchnga)
                    .HasColumnName("tenchnga")
                    .HasMaxLength(200);

                entity.Property(e => e.Tennganh)
                    .HasColumnName("tennganh")
                    .HasMaxLength(100);

                entity.Property(e => e.Tensinhvien)
                    .HasColumnName("tensinhvien")
                    .HasMaxLength(100);

                entity.Property(e => e.Thongtinthem)
                    .HasColumnName("thongtinthem")
                    .HasMaxLength(1000);

                entity.Property(e => e.Thongtinthem1)
                    .HasColumnName("thongtinthem1")
                    .HasMaxLength(1000);

                entity.Property(e => e.Tinh)
                    .HasColumnName("tinh")
                    .HasMaxLength(50);

                entity.Property(e => e.Tkhau)
                    .HasColumnName("tkhau")
                    .HasMaxLength(100);

                entity.Property(e => e.Truong)
                    .HasColumnName("truong")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Xeploai)
                    .HasColumnName("xeploai")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AsEduSurveyGraduateSurveyRound>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Graduate_SurveyRound");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(100);
            });

            modelBuilder.Entity<AsEduSurveyReportTotal>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_ReportTotal");

                entity.HasIndex(e => new { e.TheSurveyId, e.QuestionCode, e.AnswerCode })
                    .HasName("IX_ReportTotal_QuestionCode_AnswerCode");

                entity.HasIndex(e => new { e.TheSurveyId, e.LecturerCode, e.ClassRoomCode, e.QuestionCode, e.AnswerCode })
                    .HasName("IX_ReportTotal_ketxuat_tho");

                entity.HasIndex(e => new { e.TheSurveyId, e.LecturerCode, e.ClassRoomCode, e.Nhhk, e.QuestionCode, e.AnswerCode })
                    .HasName("IX_ReportTotal_thongke_tho");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AnswerCode).HasMaxLength(50);

                entity.Property(e => e.ClassRoomCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LecturerCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nhhk)
                    .IsRequired()
                    .HasColumnName("NHHK")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((20001))");

                entity.Property(e => e.QuestionCode)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AsEduSurveyUndergraduateBaiKhaoSat>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Undergraduate_BaiKhaoSat");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeThiId).HasColumnName("DeThiID");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(2000);
            });

            modelBuilder.Entity<AsEduSurveyUndergraduateBaiKhaoSatSinhVien>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien");

                entity.HasIndex(e => new { e.StudentCode, e.BaiKhaoSatId })
                    .HasName("index_Undergraduate_BaiKhaoSat_SinhVien");

                entity.HasIndex(e => new { e.StudentCode, e.BaiKhaoSatId, e.Status })
                    .HasName("index_Undergraduate_BaiKhaoSat_SinhVien_TrangThai");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BaiKhaoSatId).HasColumnName("BaiKhaoSatID");

                entity.Property(e => e.BaiLam).IsRequired();

                entity.Property(e => e.ChuyenNganh)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DeThi).IsRequired();

                entity.Property(e => e.LogIp)
                    .HasColumnName("LogIP")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nganh)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NgayGioBatDau).HasColumnType("datetime");

                entity.Property(e => e.NgayGioNopBai).HasColumnType("datetime");

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AsEduSurveyUndergraduateCauHoi>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Undergraduate_CauHoi");

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

                entity.Property(e => e.ParentCode).HasMaxLength(50);

                entity.Property(e => e.Type)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AsEduSurveyUndergraduateCauTrucDe>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Undergraduate_CauTrucDe");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CauHoiId).HasColumnName("CauHoiID");

                entity.Property(e => e.DeThiId).HasColumnName("DeThiID");
            });

            modelBuilder.Entity<AsEduSurveyUndergraduateDapAn>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Undergraduate_DapAn");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CauHoiCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CauHoiId).HasColumnName("CauHoiID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.HideQuestion).HasMaxLength(2000);

                entity.Property(e => e.ShowQuestion).HasMaxLength(2000);
            });

            modelBuilder.Entity<AsEduSurveyUndergraduateDeThi>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Undergraduate_DeThi");

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

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<AsEduSurveyUndergraduateReportTotal>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Undergraduate_ReportTotal");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AnswerCode).HasMaxLength(50);

                entity.Property(e => e.ChuyenNganh)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.QuestionCode)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AsEduSurveyUndergraduateStudent>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Undergraduate_Student");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Bangclc)
                    .HasColumnName("bangclc")
                    .HasMaxLength(50);

                entity.Property(e => e.Checksum)
                    .HasColumnName("checksum")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CnOrder)
                    .HasColumnName("cn_order")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.Dottotnghiep)
                    .HasColumnName("dottotnghiep")
                    .HasMaxLength(100);

                entity.Property(e => e.Dtoc)
                    .HasColumnName("dtoc")
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email1)
                    .HasColumnName("email1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email2)
                    .HasColumnName("email2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExMasv)
                    .IsRequired()
                    .HasColumnName("ex_masv")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GhichuThi)
                    .HasColumnName("ghichu_thi")
                    .HasMaxLength(200);

                entity.Property(e => e.Ghichuphatbang)
                    .HasColumnName("ghichuphatbang")
                    .HasMaxLength(500);

                entity.Property(e => e.Gioitinh)
                    .HasColumnName("gioitinh")
                    .HasMaxLength(20);

                entity.Property(e => e.Hedaotao)
                    .HasColumnName("hedaotao")
                    .HasMaxLength(50);

                entity.Property(e => e.K)
                    .HasColumnName("k")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.KeyAuthorize)
                    .HasColumnName("keyAuthorize")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Khoahoc)
                    .HasColumnName("khoahoc")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Lop12)
                    .HasColumnName("lop12")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Lopqd)
                    .HasColumnName("lopqd")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Makhoa)
                    .HasColumnName("makhoa")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Malop)
                    .HasColumnName("malop")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Manganh)
                    .HasColumnName("manganh")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Masv)
                    .IsRequired()
                    .HasColumnName("masv")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile1)
                    .HasColumnName("mobile1")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile2)
                    .HasColumnName("mobile2")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Namtn)
                    .HasColumnName("namtn")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ngayraqd)
                    .HasColumnName("ngayraqd")
                    .HasColumnType("datetime");

                entity.Property(e => e.Ngaysinh)
                    .HasColumnName("ngaysinh")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nguoiphatbang).HasColumnName("nguoiphatbang");

                entity.Property(e => e.Noisiti)
                    .HasColumnName("noisiti")
                    .HasMaxLength(200);

                entity.Property(e => e.Quoctich)
                    .HasColumnName("quoctich")
                    .HasMaxLength(30);

                entity.Property(e => e.Sobaodanh)
                    .HasColumnName("sobaodanh")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Sohieuba)
                    .HasColumnName("sohieuba")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Soqdtn)
                    .HasColumnName("soqdtn")
                    .HasMaxLength(100);

                entity.Property(e => e.Sovaoso)
                    .HasColumnName("sovaoso")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Tbcht)
                    .HasColumnName("tbcht")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Tcong)
                    .HasColumnName("tcong")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Tenchnga)
                    .HasColumnName("tenchnga")
                    .HasMaxLength(200);

                entity.Property(e => e.Tennganh)
                    .HasColumnName("tennganh")
                    .HasMaxLength(100);

                entity.Property(e => e.Tensinhvien)
                    .HasColumnName("tensinhvien")
                    .HasMaxLength(100);

                entity.Property(e => e.Thongtinthem)
                    .HasColumnName("thongtinthem")
                    .HasMaxLength(1000);

                entity.Property(e => e.Thongtinthem1)
                    .HasColumnName("thongtinthem1")
                    .HasMaxLength(1000);

                entity.Property(e => e.Tinh)
                    .HasColumnName("tinh")
                    .HasMaxLength(50);

                entity.Property(e => e.Tkhau)
                    .HasColumnName("tkhau")
                    .HasMaxLength(100);

                entity.Property(e => e.Truong)
                    .HasColumnName("truong")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Xeploai)
                    .HasColumnName("xeploai")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AsEduSurveyUndergraduateSurveyRound>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Undergraduate_SurveyRound");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
