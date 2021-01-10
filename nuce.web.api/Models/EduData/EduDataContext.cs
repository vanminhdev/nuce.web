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

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExamAttemptDate).HasMaxLength(100);

                entity.Property(e => e.GroupCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");

                entity.Property(e => e.SubjectCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
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

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<AsAcademyLecturer>(entity =>
            {
                entity.ToTable("AS_Academy_Lecturer");

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

                entity.Property(e => e.ClassRoomId).HasColumnName("ClassRoomID");

                entity.Property(e => e.LecturerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LecturerId).HasColumnName("LecturerID");

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            });

            modelBuilder.Entity<AsAcademySemester>(entity =>
            {
                entity.ToTable("AS_Academy_Semester");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<AsAcademyStudent>(entity =>
            {
                entity.ToTable("AS_Academy_Student");

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
                entity.HasNoKey();

                entity.ToTable("AS_Academy_Subject_Extend");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(300);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
