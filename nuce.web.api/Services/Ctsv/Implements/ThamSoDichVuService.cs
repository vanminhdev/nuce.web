using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Services.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static nuce.web.api.Common.Ctsv;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class ThamSoDichVuService : IThamSoDichVuService
    {
        private readonly CTSVNUCE_DATAContext _context;
        public ThamSoDichVuService(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }

        public IQueryable<AsAcademyStudentSvThietLapThamSoDichVu> GetParameters(DichVu dichvu)
        {
            var DichVu = (long)dichvu;
            var result = _context.AsAcademyStudentSvThietLapThamSoDichVu.AsNoTracking()
                            .Where(p => p.DichVuId == DichVu);
            return result;
        }
        public IQueryable<AsAcademyStudentSvThietLapThamSoDichVu> GetParameters(IEnumerable<long> idList)
        {
            var result = _context.AsAcademyStudentSvThietLapThamSoDichVu
                            .Where(p => idList.Contains(p.Id));
            return result;
        }
        public IQueryable<AsAcademyStudentSvLoaiDichVu> GetLoaiDichVu()
        {
            var result = _context.AsAcademyStudentSvLoaiDichVu.AsNoTracking();
            return result;
        }
    }
}
