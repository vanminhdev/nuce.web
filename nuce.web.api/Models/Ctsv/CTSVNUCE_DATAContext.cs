using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace nuce.web.api.Models.Ctsv
{
    public partial class CTSVNUCE_DATAContext : DbContext
    {
        public CTSVNUCE_DATAContext()
        {
        }

        public CTSVNUCE_DATAContext(DbContextOptions<CTSVNUCE_DATAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AsAcademyAcademics> AsAcademyAcademics { get; set; }
        public virtual DbSet<AsAcademyClass> AsAcademyClass { get; set; }
        public virtual DbSet<AsAcademyDepartment> AsAcademyDepartment { get; set; }
        public virtual DbSet<AsAcademyFaculty> AsAcademyFaculty { get; set; }
        public virtual DbSet<AsAcademyLecturer> AsAcademyLecturer { get; set; }
        public virtual DbSet<AsAcademySemester> AsAcademySemester { get; set; }
        public virtual DbSet<AsAcademyStudent> AsAcademyStudent { get; set; }
        public virtual DbSet<AsAcademyStudentGiaDinh> AsAcademyStudentGiaDinh { get; set; }
        public virtual DbSet<AsAcademyStudentQuaTrinhHocTap> AsAcademyStudentQuaTrinhHocTap { get; set; }
        public virtual DbSet<AsAcademyStudentSvCapLaiTheSinhVien> AsAcademyStudentSvCapLaiTheSinhVien { get; set; }
        public virtual DbSet<AsAcademyStudentSvGioiThieu> AsAcademyStudentSvGioiThieu { get; set; }
        public virtual DbSet<AsAcademyStudentSvLoaiDichVu> AsAcademyStudentSvLoaiDichVu { get; set; }
        public virtual DbSet<AsAcademyStudentSvMuonHocBaGoc> AsAcademyStudentSvMuonHocBaGoc { get; set; }
        public virtual DbSet<AsAcademyStudentSvThietLapThamSoDichVu> AsAcademyStudentSvThietLapThamSoDichVu { get; set; }
        public virtual DbSet<AsAcademyStudentSvThueNha> AsAcademyStudentSvThueNha { get; set; }
        public virtual DbSet<AsAcademyStudentSvVayVonNganHang> AsAcademyStudentSvVayVonNganHang { get; set; }
        public virtual DbSet<AsAcademyStudentSvXacNhan> AsAcademyStudentSvXacNhan { get; set; }
        public virtual DbSet<AsAcademyStudentSvXacNhanUuDaiTrongGiaoDuc> AsAcademyStudentSvXacNhanUuDaiTrongGiaoDuc { get; set; }
        public virtual DbSet<AsAcademyStudentThiHsg> AsAcademyStudentThiHsg { get; set; }
        public virtual DbSet<AsAcademyStudentTinNhan> AsAcademyStudentTinNhan { get; set; }
        public virtual DbSet<AsAcademySubject> AsAcademySubject { get; set; }
        public virtual DbSet<AsAcademySyncLog> AsAcademySyncLog { get; set; }
        public virtual DbSet<AsAcademyYear> AsAcademyYear { get; set; }
        public virtual DbSet<AsAppNotification> AsAppNotification { get; set; }
        public virtual DbSet<AsLogs> AsLogs { get; set; }
        public virtual DbSet<AsNewsCats> AsNewsCats { get; set; }
        public virtual DbSet<AsNewsItems> AsNewsItems { get; set; }
        public virtual DbSet<GsSetting> GsSetting { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=CTSVNUCE_DATA;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsAcademyAcademics>(entity =>
            {
                entity.ToTable("AS_Academy_Academics");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AsAcademyClass>(entity =>
            {
                entity.ToTable("AS_Academy_Class");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AcademicsCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AcademicsId)
                    .HasColumnName("AcademicsID")
                    .HasComment("Mã Ngành - Cột MaNg trong bảng Lop");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FacultyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacultyId)
                    .HasColumnName("FacultyID")
                    .HasComment("ID Khoa - Cột MaKH trong bảng Lop");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SchoolYear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AsAcademyDepartment>(entity =>
            {
                entity.ToTable("AS_Academy_Department");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChefsDepartmentCode)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Trưởng bộ môn (List Code Giảng Viên ngăn cách nhau bởi dấu phẩy) - Cộ TruongBM trong bảng BoMon");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FacultyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Mã Khoa: ");

                entity.Property(e => e.FacultyId)
                    .HasColumnName("FacultyID")
                    .HasComment("ID Khoa -");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
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

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AsAcademyLecturer>(entity =>
            {
                entity.ToTable("AS_Academy_Lecturer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateOfBirth)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("DepartmentID")
                    .HasComment("ID Bộ môn");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(200);

                entity.Property(e => e.NameOrder).HasMaxLength(30);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AsAcademySemester>(entity =>
            {
                entity.ToTable("AS_Academy_Semester");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.Enabled)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FromYear).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.ToYear).HasColumnType("datetime");

                entity.Property(e => e.YearId).HasColumnName("YearID");
            });

            modelBuilder.Entity<AsAcademyStudent>(entity =>
            {
                entity.ToTable("AS_Academy_Student");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BaoTinDiaChi)
                    .HasColumnName("BaoTin_DiaChi")
                    .HasMaxLength(200);

                entity.Property(e => e.BaoTinDiaChiNguoiNhan)
                    .HasColumnName("BaoTin_DiaChiNguoiNhan")
                    .HasMaxLength(200);

                entity.Property(e => e.BaoTinEmail)
                    .HasColumnName("BaoTin_Email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BaoTinHoVaTen)
                    .HasColumnName("BaoTin_HoVaTen")
                    .HasMaxLength(100);

                entity.Property(e => e.BaoTinSoDienThoai)
                    .HasColumnName("BaoTin_SoDienThoai")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BirthPlace).HasMaxLength(500);

                entity.Property(e => e.ClassCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.Cmt)
                    .HasColumnName("CMT")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CmtNgayCap)
                    .HasColumnName("CMT_NgayCap")
                    .HasColumnType("datetime");

                entity.Property(e => e.CmtNoiCap)
                    .HasColumnName("CMT_NoiCap")
                    .HasMaxLength(150);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DaThamGiaDoiTuyenThiHsg).HasColumnName("DaThamGiaDoiTuyenThiHSG");

                entity.Property(e => e.DanToc).HasMaxLength(10);

                entity.Property(e => e.DateOfBirth)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiaChiCuThe).HasMaxLength(200);

                entity.Property(e => e.DiemThiPtth)
                    .HasColumnName("DiemThiPTTH")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DoiTuongUuTien).HasMaxLength(30);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email1)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EmailNhaTruong)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.File1)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.File2)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.File3)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FulName).HasMaxLength(200);

                entity.Property(e => e.GioiTinh).HasMaxLength(5);

                entity.Property(e => e.HkttPho)
                    .HasColumnName("HKTT_Pho")
                    .HasMaxLength(500);

                entity.Property(e => e.HkttPhuong)
                    .HasColumnName("HKTT_Phuong")
                    .HasMaxLength(50);

                entity.Property(e => e.HkttQuan)
                    .HasColumnName("HKTT_Quan")
                    .HasMaxLength(50);

                entity.Property(e => e.HkttSoNha)
                    .HasColumnName("HKTT_SoNha")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HkttTinh)
                    .HasColumnName("HKTT_Tinh")
                    .HasMaxLength(50);

                entity.Property(e => e.KeyAuthorize).HasColumnName("keyAuthorize");

                entity.Property(e => e.KhuVucHktt)
                    .HasColumnName("KhuVucHKTT")
                    .HasMaxLength(20);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile1)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NamTotNghiepPtth)
                    .HasColumnName("NamTotNghiepPTTH")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.NgaySinh).HasColumnType("datetime");

                entity.Property(e => e.NgayVaoDang).HasColumnType("datetime");

                entity.Property(e => e.NgayVaoDoan).HasColumnType("datetime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TonGiao).HasMaxLength(20);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AsAcademyStudentGiaDinh>(entity =>
            {
                entity.ToTable("AS_Academy_Student_GiaDinh");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChucVu).HasMaxLength(50);

                entity.Property(e => e.Count).HasDefaultValueSql("((1))");

                entity.Property(e => e.DanToc).HasMaxLength(10);

                entity.Property(e => e.HoVaTen).HasMaxLength(100);

                entity.Property(e => e.MoiQuanHe)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.NamSinh)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NgheNghiep).HasMaxLength(50);

                entity.Property(e => e.NoiCongTac).HasMaxLength(200);

                entity.Property(e => e.NoiOhienNay)
                    .HasColumnName("NoiOHienNay")
                    .HasMaxLength(200);

                entity.Property(e => e.QuocTich).HasMaxLength(30);

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.TonGiao).HasMaxLength(20);
            });

            modelBuilder.Entity<AsAcademyStudentQuaTrinhHocTap>(entity =>
            {
                entity.ToTable("AS_Academy_Student_QuaTrinhHocTap");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Count).HasDefaultValueSql("((1))");

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.TenTruong)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ThoiGian)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AsAcademyStudentSvCapLaiTheSinhVien>(entity =>
            {
                entity.ToTable("AS_Academy_Student_SV_CapLaiTheSinhVien");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.MaXacNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NgayGui).HasColumnType("datetime");

                entity.Property(e => e.NgayHenDenNgay)
                    .HasColumnName("NgayHen_DenNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.NgayHenTuNgay)
                    .HasColumnName("NgayHen_TuNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AsAcademyStudentSvGioiThieu>(entity =>
            {
                entity.ToTable("AS_Academy_Student_SV_GioiThieu");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CoGiaTriDenNgay).HasColumnType("datetime");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.DenGap).HasMaxLength(500);

                entity.Property(e => e.DonVi).HasMaxLength(500);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.MaXacNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NgayGui).HasColumnType("datetime");

                entity.Property(e => e.NgayHenDenNgay)
                    .HasColumnName("NgayHen_DenNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.NgayHenTuNgay)
                    .HasColumnName("NgayHen_TuNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.VeViec).HasMaxLength(1000);
            });

            modelBuilder.Entity<AsAcademyStudentSvLoaiDichVu>(entity =>
            {
                entity.ToTable("AS_Academy_Student_SV_LoaiDichVu");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Param1)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Param2)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Param3)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AsAcademyStudentSvMuonHocBaGoc>(entity =>
            {
                entity.ToTable("AS_Academy_Student_SV_MuonHocBaGoc");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.MaXacNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NgayGui).HasColumnType("datetime");

                entity.Property(e => e.NgayHenDenNgay)
                    .HasColumnName("NgayHen_DenNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.NgayHenTuNgay)
                    .HasColumnName("NgayHen_TuNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.NgayTra).HasColumnType("datetime");

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AsAcademyStudentSvThietLapThamSoDichVu>(entity =>
            {
                entity.ToTable("AS_Academy_Student_SV_ThietLapThamSoDichVu");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DichVuId).HasColumnName("DichVuID");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Value).HasMaxLength(4000);
            });

            modelBuilder.Entity<AsAcademyStudentSvThueNha>(entity =>
            {
                entity.ToTable("AS_Academy_Student_SV_ThueNha");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.MaXacNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NgayGui).HasColumnType("datetime");

                entity.Property(e => e.NgayHenDenNgay)
                    .HasColumnName("NgayHen_DenNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.NgayHenTuNgay)
                    .HasColumnName("NgayHen_TuNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AsAcademyStudentSvVayVonNganHang>(entity =>
            {
                entity.ToTable("AS_Academy_Student_SV_VayVonNganHang");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.MaXacNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NgayGui).HasColumnType("datetime");

                entity.Property(e => e.NgayHenDenNgay)
                    .HasColumnName("NgayHen_DenNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.NgayHenTuNgay)
                    .HasColumnName("NgayHen_TuNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ThuocDien).HasMaxLength(10);

                entity.Property(e => e.ThuocDoiTuong).HasMaxLength(10);
            });

            modelBuilder.Entity<AsAcademyStudentSvXacNhan>(entity =>
            {
                entity.ToTable("AS_Academy_Student_SV_XacNhan");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.MaXacNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NgayGui).HasColumnType("datetime");

                entity.Property(e => e.NgayHenDenNgay)
                    .HasColumnName("NgayHen_DenNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.NgayHenTuNgay)
                    .HasColumnName("NgayHen_TuNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AsAcademyStudentSvXacNhanUuDaiTrongGiaoDuc>(entity =>
            {
                entity.ToTable("AS_Academy_Student_SV_XacNhanUuDaiTrongGiaoDuc");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.KyLuat).HasMaxLength(50);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.MaXacNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NgayGui).HasColumnType("datetime");

                entity.Property(e => e.NgayHenDenNgay)
                    .HasColumnName("NgayHen_DenNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.NgayHenTuNgay)
                    .HasColumnName("NgayHen_TuNgay")
                    .HasColumnType("datetime");

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AsAcademyStudentThiHsg>(entity =>
            {
                entity.ToTable("AS_Academy_Student_ThiHSG");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CapThi)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Count).HasDefaultValueSql("((1))");

                entity.Property(e => e.DatGiai)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.MonThi)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");
            });

            modelBuilder.Entity<AsAcademyStudentTinNhan>(entity =>
            {
                entity.ToTable("AS_Academy_Student_TinNhan");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.Receiver)
                    .IsRequired()
                    .HasColumnName("receiver")
                    .HasMaxLength(500);

                entity.Property(e => e.ReceiverEmail)
                    .IsRequired()
                    .HasColumnName("receiverEmail")
                    .HasMaxLength(500);

                entity.Property(e => e.SenderId).HasColumnName("SenderID");

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(1000);
            });

            modelBuilder.Entity<AsAcademySubject>(entity =>
            {
                entity.ToTable("AS_Academy_Subject");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DepartmentCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AsAcademySyncLog>(entity =>
            {
                entity.ToTable("AS_Academy_Sync_Log");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Mã table/module đồng bộ");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Message).HasMaxLength(2000);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            });

            modelBuilder.Entity<AsAcademyYear>(entity =>
            {
                entity.ToTable("AS_Academy_Year");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.Enabled)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FromYear).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.ToYear).HasColumnType("datetime");
            });

            modelBuilder.Entity<AsAppNotification>(entity =>
            {
                entity.ToTable("AS_App_Notification");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.LasdModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.StudentCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.Property(e => e.TypeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeId).HasColumnName("TypeID");
            });

            modelBuilder.Entity<AsLogs>(entity =>
            {
                entity.ToTable("AS_Logs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.UserCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AsNewsCats>(entity =>
            {
                entity.ToTable("AS_News_Cats");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Count).HasDefaultValueSql("((1))");

                entity.Property(e => e.Des).IsRequired();

                entity.Property(e => e.Display)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Parent).HasDefaultValueSql("((1))");

                entity.Property(e => e.Type).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<AsNewsItems>(entity =>
            {
                entity.ToTable("AS_News_Items");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActiveFlg).HasColumnName("active_flg");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .HasMaxLength(200);

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(2000);

                entity.Property(e => e.EntryDatetime)
                    .HasColumnName("entry_datetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EntryUsername)
                    .IsRequired()
                    .HasColumnName("entry_username")
                    .HasMaxLength(50);

                entity.Property(e => e.File)
                    .HasColumnName("file")
                    .HasMaxLength(200);

                entity.Property(e => e.IsComment)
                    .IsRequired()
                    .HasColumnName("is_comment")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsHome)
                    .IsRequired()
                    .HasColumnName("is_home")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.IsShared)
                    .IsRequired()
                    .HasColumnName("is_shared")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsTinlq)
                    .IsRequired()
                    .HasColumnName("is_tinlq")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MetaDesciption)
                    .HasColumnName("meta_desciption")
                    .HasMaxLength(500);

                entity.Property(e => e.MetaKeyword)
                    .HasColumnName("meta_keyword")
                    .HasMaxLength(500);

                entity.Property(e => e.NewContent).HasColumnName("new_content");

                entity.Property(e => e.NewSource)
                    .HasColumnName("new_source")
                    .HasMaxLength(200);

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(500);

                entity.Property(e => e.TotalView)
                    .HasColumnName("total_view")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnName("update_datetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateUsername)
                    .IsRequired()
                    .HasColumnName("update_username")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GsSetting>(entity =>
            {
                entity.ToTable("GS_Setting");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
