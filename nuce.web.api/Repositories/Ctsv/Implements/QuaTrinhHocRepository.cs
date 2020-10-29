using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class QuaTrinhHocRepository : IQuaTrinhHocRepository
    {
        private readonly CTSVNUCE_DATAContext _context;
        public QuaTrinhHocRepository(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }
        public async Task<List<AsAcademyStudentQuaTrinhHocTap>> FindByCodeAsync(string studentCode)
        {
            return await _context.AsAcademyStudentQuaTrinhHocTap.AsNoTracking().Where(s => s.StudentCode == studentCode).ToListAsync();
        }
    }
}
