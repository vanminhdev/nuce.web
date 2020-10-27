using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class TinNhanRepository : ITinNhanRepository
    {
        private readonly CTSVNUCE_DATAContext _context;
        public TinNhanRepository(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }
        public async Task addTinNhanAsync(AsAcademyStudentTinNhan tinNhan)
        {
            await _context.AsAcademyStudentTinNhan.AddAsync(tinNhan);
        }
    }
}
