using Microsoft.EntityFrameworkCore;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class DotHoTroHocTapRepository : BaseStudentServiceRepository<AsAcademyStudentSvDangKyHoTroHocTapDot>, IDotHoTroHocTapRepository
    {
        public DotHoTroHocTapRepository(CTSVNUCE_DATAContext _context) : base(_context)
        {
        }

        public async Task Add(AddDotHoTroHocTap model)
        {
            if (model.TuNgay > model.DenNgay)
            {
                throw new Exception("Từ ngày phải nhỏ hơn đến ngày");
            }
            var all = await _context.AsAcademyStudentSvDangKyHoTroHocTapDot.ToListAsync();
            all.ForEach(item =>
            {
                item.IsActive = false;
            });
            _context.AsAcademyStudentSvDangKyHoTroHocTapDot.Add(new AsAcademyStudentSvDangKyHoTroHocTapDot
            {
                Name = model.Name,
                TuNgay = model.TuNgay.Value,
                DenNgay = model.DenNgay.Value,
                IsActive = true,
            });
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var dotDK = await _context.AsAcademyStudentSvDangKyHoTroHocTapDot.FirstOrDefaultAsync(o => o.Id == id);
            if (dotDK == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt đăng ký");
            }
            if (dotDK.IsActive)
            {
                throw new Exception("Đợt đang mở không thể xóa");
            }
            _context.AsAcademyStudentSvDangKyHoTroHocTapDot.Remove(dotDK);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginationModel<AsAcademyStudentSvDangKyHoTroHocTapDot>> GetAll(int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var query = _context.AsAcademyStudentSvDangKyHoTroHocTapDot
                .OrderByDescending(d => d.IsActive)
                .ThenByDescending(d => d.DenNgay);
            var recordsTotal = query.Count();

            var recordsFiltered = query.Count();

            var querySkip = query
                .Skip(skip).Take(take);

            var data = await querySkip
                .ToListAsync();

            return new PaginationModel<AsAcademyStudentSvDangKyHoTroHocTapDot>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task<AsAcademyStudentSvDangKyHoTroHocTapDot> GetDotActive()
        {
            var dotDK = await _context.AsAcademyStudentSvDangKyHoTroHocTapDot.FirstOrDefaultAsync(o => o.IsActive);
            if (dotDK == null)
            {
                throw new RecordNotFoundException("Không có đợt đăng ký được mở");
            }
            return dotDK;
        }

        public async Task Update(int id, AddDotHoTroHocTap model)
        {
            var dotDK = await _context.AsAcademyStudentSvDangKyHoTroHocTapDot.FirstOrDefaultAsync(o => o.Id == id);
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
