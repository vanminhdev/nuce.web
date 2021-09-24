using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.EduData;
using nuce.web.api.Repositories.EduData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.EduData.Implements
{
    public class LecturerRepository : ILecturerRepository
    {
        private readonly EduDataContext _context;
        public LecturerRepository(EduDataContext _context)
        {
            this._context = _context;
        }
        public async Task<AsAcademyLecturer> FindByCode(string code)
        {
            return await _context.AsAcademyLecturer.FirstOrDefaultAsync(l => l.Code == code);
        }
    }
}
