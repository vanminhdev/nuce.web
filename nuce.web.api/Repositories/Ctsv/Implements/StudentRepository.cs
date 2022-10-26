using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.ViewModel.CDSConnect;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CTSVNUCE_DATAContext _context;
        private readonly IConfiguration _configuration;
        public StudentRepository(CTSVNUCE_DATAContext _context, IConfiguration _configuration)
        {
            this._context = _context;
            this._configuration = _configuration;
        }

        public AsAcademyStudent FindByCode(string studentCode)
        {
            return _context.AsAcademyStudent.AsNoTracking().FirstOrDefault(student => student.Code == studentCode);
        }

        public AsAcademyStudent FindByEmailNhaTruong(string email)
        {
            return _context.AsAcademyStudent
                        .AsNoTracking()
                        .FirstOrDefault(student => student.EmailNhaTruong == email && 
                                                (student.DaXacThucEmailNhaTruong ?? false));
        }

        public void Update(AsAcademyStudent student)
        {
             _context.AsAcademyStudent.Update(student);
        }
        public DbSet<AsAcademyStudent> GetAll()
        {
            return _context.AsAcademyStudent;
        }
        public async Task<StudentDichVuModel> GetStudentDichVuInfoAsync(string studentCode)
        {
            var result = await _context.AsAcademyStudent
                        .Join(_context.AsAcademyClass,
                                student => student.ClassCode,
                                aClass => aClass.Code,
                                (student, aClass) => new { student, aClass })
                        .GroupJoin(_context.AsAcademyFaculty.AsNoTracking(),
                           tmp => tmp.aClass.FacultyCode,
                           faculty => faculty.Code,
                           (tmp, faculty) => new { tmp, faculty }
                         )
                        .SelectMany(left => left.faculty.DefaultIfEmpty(), (left, f) => new { left.tmp.student, left.tmp.aClass, f })
                        .GroupJoin(_context.AsAcademyAcademics.AsNoTracking(),
                                tmp => tmp.aClass.AcademicsCode,
                                academic => academic.Code,
                                (tmp, academic) => new
                                {
                                    Student = tmp.student,
                                    AcademyClass = tmp.aClass,
                                    Faculty = tmp.f,
                                    Academics = academic
                                })
                        .SelectMany(left => left.Academics.DefaultIfEmpty(),
                                    (left, academic) => new StudentDichVuModel
                                    {
                                        Student = left.Student,
                                        AcademyClass = left.AcademyClass,
                                        Faculty = left.Faculty,
                                        Academics = academic
                                    })
                        .FirstOrDefaultAsync(s => s.Student.Code == studentCode);
            return result;
        }

        public async Task<ViewGetSvDto> GetSinhVienByCodeCDS(string masv)
        {
            var result = new ViewGetSvDto();

            var localStudent = _context.AsAcademyStudent.FirstOrDefault(x => x.Code == masv);

            HttpClient clientAuth = new HttpClient()
            {
                BaseAddress = new Uri(_configuration["CDSConnectUrl"]),
                Timeout = TimeSpan.FromSeconds(60)
            };
            var res = await clientAuth.GetAsync($"api/sv/{masv}");

            if (res.IsSuccessStatusCode)
            {
                var resContent = await res.Content.ReadAsStringAsync();

                try
                {
                    var aaa = JsonSerializer.Deserialize<ResponseGetSvDto>(resContent);
                }
                catch (Exception ex)
                {
                    var ddddat = ex;
                    throw ex;
                }
                var resOjbect = JsonSerializer.Deserialize<ResponseGetSvDto>(resContent);

                var sinhVien = resOjbect?.Data;

                if (sinhVien != null)
                {
                    sinhVien.SoCmnd = localStudent.Cmt;
                    sinhVien.NoiCapCmnd = localStudent.CmtNoiCap;
                    sinhVien.NgayCapCmnd = localStudent.CmtNgayCap;
                    //sinhVien.GioiTinh = localStudent.GioiTinh;
                    sinhVien.Email = localStudent.Email;
                    sinhVien.TenHkttTinh = localStudent.HkttTinh;
                    sinhVien.TenHkttHuyen = localStudent.HkttQuan;
                    sinhVien.TenHkttPhuongXa = localStudent.HkttPhuong;
                    sinhVien.HkttSoNha = localStudent.HkttSoNha;
                    sinhVien.DiaChiCuThe = localStudent.DiaChiCuThe;
                    sinhVien.EmailNhaTruong = localStudent.EmailNhaTruong;
                    sinhVien.File1 = localStudent.File1;

                    var lopHoc = _context.AsAcademyClass.FirstOrDefault(x => x.Code == sinhVien.MaLopChu);
                    sinhVien.NienKhoa = lopHoc?.SchoolYear;
                }

                return sinhVien;
            }

            return result;
        }
    }
}
