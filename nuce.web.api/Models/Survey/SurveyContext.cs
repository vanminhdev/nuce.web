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

        public virtual DbSet<AsAcademyAcademics> AsAcademyAcademics { get; set; }
        public virtual DbSet<AsAcademyCClassRoom> AsAcademyCClassRoom { get; set; }
        public virtual DbSet<AsAcademyCLecturerClassRoom> AsAcademyCLecturerClassRoom { get; set; }
        public virtual DbSet<AsAcademyCStudentClassRoom> AsAcademyCStudentClassRoom { get; set; }
        public virtual DbSet<AsAcademyClass> AsAcademyClass { get; set; }
        public virtual DbSet<AsAcademyClassRoom> AsAcademyClassRoom { get; set; }
        public virtual DbSet<AsAcademyDepartment> AsAcademyDepartment { get; set; }
        public virtual DbSet<AsAcademyFaculty> AsAcademyFaculty { get; set; }
        public virtual DbSet<AsAcademyLecturer> AsAcademyLecturer { get; set; }
        public virtual DbSet<AsAcademyLecturerClassRoom> AsAcademyLecturerClassRoom { get; set; }
        public virtual DbSet<AsAcademySemester> AsAcademySemester { get; set; }
        public virtual DbSet<AsAcademyStudent> AsAcademyStudent { get; set; }
        public virtual DbSet<AsAcademyStudentClassRoom> AsAcademyStudentClassRoom { get; set; }
        public virtual DbSet<AsAcademySubject> AsAcademySubject { get; set; }
        public virtual DbSet<AsAcademySubjectExtend> AsAcademySubjectExtend { get; set; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsAcademyAcademics>(entity =>
            {
                entity.ToTable("AS_Academy_Academics");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            });

            modelBuilder.Entity<AsAcademyCClassRoom>(entity =>
            {
                entity.ToTable("AS_Academy_C_ClassRoom");

                entity.HasIndex(e => e.Code)
                    .HasName("IX_C_Classroom_Code");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ExamAttemptDate).HasMaxLength(100);

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.GroupCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");

                entity.Property(e => e.SubjectCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            });

            modelBuilder.Entity<AsAcademyCLecturerClassRoom>(entity =>
            {
                entity.ToTable("AS_Academy_C_Lecturer_ClassRoom");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassRoomCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClassRoomId).HasColumnName("ClassRoomID");

                entity.Property(e => e.LecturerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LecturerId).HasColumnName("LecturerID");

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            });

            modelBuilder.Entity<AsAcademyCStudentClassRoom>(entity =>
            {
                entity.ToTable("AS_Academy_C_Student_ClassRoom");

                entity.HasIndex(e => e.StudentCode)
                    .HasName("IX_C_Student_ClassRoom_StudentCode");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassRoomCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClassRoomId).HasColumnName("ClassRoomID");

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");

                entity.Property(e => e.StudentCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");
            });

            modelBuilder.Entity<AsAcademyClass>(entity =>
            {
                entity.ToTable("AS_Academy_Class");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AcademicsCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AcademicsId).HasColumnName("AcademicsID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacultyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacultyId).HasColumnName("FacultyID");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SchoolYear)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AsAcademyClassRoom>(entity =>
            {
                entity.ToTable("AS_Academy_ClassRoom");

                entity.HasIndex(e => e.Code)
                    .HasName("IX_Classroom_Code");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nhhk)
                    .HasColumnName("NHHK")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AsAcademyDepartment>(entity =>
            {
                entity.ToTable("AS_Academy_Department");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChefsDepartmentCode)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacultyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacultyId).HasColumnName("FacultyID");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            });

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

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            });

            modelBuilder.Entity<AsAcademyLecturer>(entity =>
            {
                entity.ToTable("AS_Academy_Lecturer");

                entity.HasIndex(e => e.Code)
                    .HasName("IX_Lecturer_Code");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(200);

                entity.Property(e => e.NameOrder).HasMaxLength(30);
            });

            modelBuilder.Entity<AsAcademyLecturerClassRoom>(entity =>
            {
                entity.ToTable("AS_Academy_Lecturer_ClassRoom");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassRoomCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LecturerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nhhk)
                    .HasColumnName("NHHK")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AsAcademySemester>(entity =>
            {
                entity.ToTable("AS_Academy_Semester");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<AsAcademyStudent>(entity =>
            {
                entity.ToTable("AS_Academy_Student");

                entity.HasIndex(e => e.Code)
                    .HasName("IX_Student_Code");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BirthPlace).HasMaxLength(500);

                entity.Property(e => e.ClassCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(200);

                entity.Property(e => e.KeyAuthorize).HasColumnName("keyAuthorize");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<AsAcademyStudentClassRoom>(entity =>
            {
                entity.ToTable("AS_Academy_Student_ClassRoom");

                entity.HasIndex(e => e.StudentCode)
                    .HasName("IX_Student_ClassRoom_StudentCode");

                entity.HasIndex(e => new { e.StudentCode, e.ClassRoomCode, e.Nhhk })
                    .HasName("IX_ClassRoomCode_StudentCode");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassRoomCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nhhk)
                    .HasColumnName("NHHK")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StudentCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AsAcademySubject>(entity =>
            {
                entity.ToTable("AS_Academy_Subject");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            });

            modelBuilder.Entity<AsAcademySubjectExtend>(entity =>
            {
                entity.ToTable("AS_Academy_Subject_Extend");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(300);
            });

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

                entity.HasIndex(e => e.BaiKhaoSatId)
                    .HasName("IX_BaiKhaoSat_SinhVien_BaiKSId");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.ClassRoomCode })
                    .HasName("index_baiks_thong_ke_1");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.Status })
                    .HasName("index_baikhaosatsinhvien_baiks_trangthai");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.ClassRoomCode, e.Status })
                    .HasName("index_baiks_thong_ke_2");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.LecturerCode, e.ClassRoomCode })
                    .HasName("index_baikhaosatsinhvien_baiks_giangvien_lop");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.StudentCode, e.ClassRoomCode, e.Nhhk })
                    .HasName("index_As_Edu_survey_baikhaosatsinhvien_them");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.SubjectCode, e.LecturerCode, e.Status })
                    .HasName("IX_baikhaosatsinhvien_ThongKe_GiangVien");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.SubjectCode, e.StudentCode, e.Status })
                    .HasName("IX_baikhaosatsinhvien_ThongKe_SinhVien");

                entity.HasIndex(e => new { e.BaiKhaoSatId, e.StudentCode, e.ClassRoomCode, e.Nhhk, e.Status })
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

                entity.Property(e => e.ChuyenNganh).HasMaxLength(100);

                entity.Property(e => e.DeThi).IsRequired();

                entity.Property(e => e.LoaiHinh).HasMaxLength(50);

                entity.Property(e => e.LogIp)
                    .HasColumnName("LogIP")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nganh).HasMaxLength(100);

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

                entity.Property(e => e.Cmnd)
                    .HasColumnName("cmnd")
                    .HasMaxLength(12)
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
                    .HasColumnType("date");

                entity.Property(e => e.Ngaysinh)
                    .HasColumnName("ngaysinh")
                    .HasColumnType("date");

                entity.Property(e => e.Nguoiphatbang).HasColumnName("nguoiphatbang");

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

                entity.HasIndex(e => new { e.TheSurveyId, e.ClassRoomCode, e.LecturerCode })
                    .HasName("index_report_thong_ke_2");

                entity.HasIndex(e => new { e.TheSurveyId, e.ClassRoomCode, e.QuestionCode })
                    .HasName("index_report_thong_ke_1");

                entity.HasIndex(e => new { e.TheSurveyId, e.LecturerCode, e.QuestionCode })
                    .HasName("index_report_thong_ke_4");

                entity.HasIndex(e => new { e.TheSurveyId, e.QuestionCode, e.AnswerCode })
                    .HasName("IX_ReportTotal_QuestionCode_AnswerCode");

                entity.HasIndex(e => new { e.TheSurveyId, e.ClassRoomCode, e.QuestionCode, e.AnswerCode })
                    .HasName("index_report_thong_ke_3");

                entity.HasIndex(e => new { e.TheSurveyId, e.LecturerCode, e.ClassRoomCode, e.QuestionCode, e.AnswerCode })
                    .HasName("IX_ReportTotal_ketxuat_tho");

                entity.HasIndex(e => new { e.TheSurveyId, e.LecturerCode, e.ClassRoomCode, e.Nhhk, e.QuestionCode, e.AnswerCode })
                    .HasName("IX_ReportTotal_thongke_tho");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AnswerCode).HasMaxLength(50);

                entity.Property(e => e.ClassRoom)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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

                entity.Property(e => e.SubjectCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
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

                entity.HasIndex(e => e.StudentCode)
                    .HasName("IX_Undergraduate_baiks_sinhvien_student_code");

                entity.HasIndex(e => new { e.StudentCode, e.BaiKhaoSatId, e.Status, e.NgayGioNopBai })
                    .HasName("index_Undergraduate_BaiKhaoSat_SinhVien_TrangThai");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BaiKhaoSatId).HasColumnName("BaiKhaoSatID");

                entity.Property(e => e.BaiLam).IsRequired();

                entity.Property(e => e.ChuyenNganh).HasMaxLength(100);

                entity.Property(e => e.DeThi).IsRequired();

                entity.Property(e => e.LogIp)
                    .HasColumnName("LogIP")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nganh).HasMaxLength(100);

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

                entity.Property(e => e.ChuyenNganh).HasMaxLength(100);

                entity.Property(e => e.QuestionCode)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AsEduSurveyUndergraduateStudent>(entity =>
            {
                entity.ToTable("AS_Edu_Survey_Undergraduate_Student");

                entity.HasIndex(e => e.ExMasv)
                    .HasName("IX_Undergraduate_Student_Ex_masv");

                entity.HasIndex(e => e.Makhoa)
                    .HasName("IX_Undergraduate_Student_MaKhoa");

                entity.HasIndex(e => e.Masv)
                    .HasName("IX_Undergraduate_Student_MaSV");

                entity.HasIndex(e => e.Ngayraqd)
                    .HasName("IX_Undergraduate_Student_ngayraqd");

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

                entity.Property(e => e.Cmnd)
                    .HasColumnName("cmnd")
                    .HasMaxLength(12)
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
                    .HasColumnType("date");

                entity.Property(e => e.Ngaysinh)
                    .HasColumnName("ngaysinh")
                    .HasColumnType("date");

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
