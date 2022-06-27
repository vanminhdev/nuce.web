using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class LoaiDichVuRepository : ILoaiDichVuRepository
    {
        private readonly CTSVNUCE_DATAContext _context;
        public LoaiDichVuRepository(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }

        public List<AsAcademyStudentSvLoaiDichVu> GetAllInUse()
        {
            return _context.AsAcademyStudentSvLoaiDichVu.AsNoTracking()
                        .ToList();
        }
    }
}
