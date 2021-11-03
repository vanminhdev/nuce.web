using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.EduData;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.EduData
{
    public class FacultyRepository
    {
        private readonly EduDataContext _context;
        public FacultyRepository(EduDataContext _context)
        {
            this._context = _context;
        }

        public Task<AsAcademyFaculty> FindByCode(string code)
        {
            return _context.AsAcademyFaculty.FirstOrDefaultAsync(f => f.Code == code);
        }

        public IQueryable<AsAcademyDepartment> GetDepartment(string code)
        {
            var data = _context.AsAcademyDepartment.AsNoTracking()
                            .Where(d => d.Code == code)
                            .Join(_context.AsAcademyFaculty.AsNoTracking(),
                                d => d.FacultyId,
                                f => f.Id,
                                (d, f) => f)
                            .Join(_context.AsAcademyDepartment.AsNoTracking(),
                                f => f.Code,
                                d => d.FacultyCode,
                                (f, d) => d
                            );
            return data;
        }
    }
}
