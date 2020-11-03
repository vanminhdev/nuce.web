using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class GiaDinhRepository : IGiaDinhRepository
    {
        private readonly CTSVNUCE_DATAContext _context;
        public GiaDinhRepository(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }
        public async Task<List<AsAcademyStudentGiaDinh>> FindByCodeAsync(string studentCode)
        {
            return await _context.AsAcademyStudentGiaDinh.AsNoTracking().Where(s => s.StudentCode == studentCode).ToListAsync();
        }
    }
}
