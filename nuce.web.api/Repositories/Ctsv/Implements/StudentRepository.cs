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
            var result = await _context.AsAcademyStudent.AsNoTracking()
                        .Join(_context.AsAcademyClass.AsNoTracking(),
                                student => student.ClassId,
                                aClass => aClass.Id,
                                (student, aClass) => new { student, aClass })
                        .Join(_context.AsAcademyFaculty.AsNoTracking(),
                               tmp => tmp.aClass.FacultyId,
                               faculty => faculty.Id,
                               (tmp, faculty) => new { tmp.student, tmp.aClass, faculty })
                        .Join(_context.AsAcademyAcademics.AsNoTracking(),
                                tmp => tmp.aClass.AcademicsId,
                                academic => academic.Id,
                                (tmp, academic) => new StudentDichVuModel
                                {
                                    Student = tmp.student,
                                    AcademyClass = tmp.aClass,
                                    Faculty = tmp.faculty,
                                    Academics = academic
                                })
                        .FirstOrDefaultAsync(s => s.Student.Code == studentCode);
            return result;
        }
    }
}
