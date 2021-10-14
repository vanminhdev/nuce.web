using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CTSVNUCE_DATAContext _context;
        public StudentRepository(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
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
    }
}
