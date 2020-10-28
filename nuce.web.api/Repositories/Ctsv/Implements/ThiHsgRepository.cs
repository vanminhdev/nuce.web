using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class ThiHsgRepository : IThiHsgRepository
    {
        private readonly CTSVNUCE_DATAContext _context;
        public ThiHsgRepository(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }

        public async Task<List<AsAcademyStudentThiHsg>> FindByCodeAsync(string studentCode)
        {
            return await _context.AsAcademyStudentThiHsg.AsNoTracking().Where(s => s.StudentCode == studentCode).ToListAsync();
        }
    }
}
