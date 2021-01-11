using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.EduData;
using nuce.web.api.Repositories.EduData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.EduData.Implements
{
    public class FacultyRepository : IFacultyRepository
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
    }
}
