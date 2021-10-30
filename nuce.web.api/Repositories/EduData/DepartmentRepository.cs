using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.EduData;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.EduData
{
    public class DepartmentRepository
    {
        private readonly EduDataContext _context;
        public DepartmentRepository(EduDataContext _context)
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
