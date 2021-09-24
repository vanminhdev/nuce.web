using nuce.web.api.Repositories.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.Ctsv;
using Microsoft.EntityFrameworkCore;
using nuce.web.api.HandleException;
using nuce.web.api.ViewModel.Ctsv;
using nuce.web.api.ViewModel.Base;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class DotDeNghiHoTroChiPhiRepository : BaseStudentServiceRepository<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot>, IDotDeNghiHoTroChiPhiRepository
    {
        public DotDeNghiHoTroChiPhiRepository(CTSVNUCE_DATAContext _context) : base(_context)
        {
        }

        public async Task Add(AddDotDeNghiHoTroChiPhi model)
        {
            if (model.TuNgay > model.DenNgay)
            {
                throw new Exception("Từ ngày phải nhỏ hơn đến ngày");
            }
            var all = await _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot.ToListAsync();
            all.ForEach(item =>
            {
                item.IsActive = false;
            });
            _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot.Add(new AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot { 
                Name = model.Name,
                TuNgay = model.TuNgay.Value,
                DenNgay = model.DenNgay.Value,
                IsActive = true,
            });
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var dotDK = await _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot.FirstOrDefaultAsync(o => o.Id == id);
            if (dotDK == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt đăng ký");
            }
            if (dotDK.IsActive)
            {
                throw new Exception("Đợt đang mở không thể xóa");
            }
            _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot.Remove(dotDK);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginationModel<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot>> GetAll(int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var query = _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot
                .OrderByDescending(d => d.IsActive)
                .ThenByDescending(d => d.DenNgay);
            var recordsTotal = query.Count();

            var recordsFiltered = query.Count();

            var querySkip = query
                .Skip(skip).Take(take);

            var data = await querySkip
                .ToListAsync();

            return new PaginationModel<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot> GetDotActive()
        {
            var dotDK = await _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot.FirstOrDefaultAsync(o => o.IsActive);
            if (dotDK == null)
            {
                throw new RecordNotFoundException("Không có đợt đăng ký được mở");
            }
            return dotDK;
        }

        public async Task Update(int id, AddDotDeNghiHoTroChiPhi model)
        {
            var dotDK = await _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot.FirstOrDefaultAsync(o => o.Id == id);
            if (dotDK == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt đăng ký");
            }

            if (model.TuNgay > model.DenNgay)
            {
                throw new Exception("Từ ngày phải nhỏ hơn đến ngày");
            }

            dotDK.Name = model.Name;
            dotDK.TuNgay = model.TuNgay.Value;
            dotDK.DenNgay = model.DenNgay.Value;
            await _context.SaveChangesAsync();
        }
    }
}
