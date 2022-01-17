using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.EduData
{
    public class DepartmentRepository
    {
        private readonly SurveyContext _context;
        public DepartmentRepository(SurveyContext _context)
        {
            this._context = _context;
        }
        public Task<AsAcademyDepartment> FindByCode(string code)
        {
            return _context.AsAcademyDepartment.FirstOrDefaultAsync(d => d.Code == code);
        }
        public IQueryable<AsAcademyLecturer> GetLecturer(string code)
        {
            return _context.AsAcademyLecturer.Where(l => l.DepartmentCode == code);
        }
    }
}
