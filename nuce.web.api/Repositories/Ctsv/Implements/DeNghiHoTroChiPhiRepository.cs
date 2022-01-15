using nuce.web.api.Repositories.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.Ctsv;
using Microsoft.EntityFrameworkCore;
using nuce.web.api.ViewModel.Ctsv;
using nuce.web.api.HandleException;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class DeNghiHoTroChiPhiRepository : BaseStudentServiceRepository<AsAcademyStudentSvDeNghiHoTroChiPhiHocTap>, IDeNghiHoTroChiPhiRepository
    {
        public DeNghiHoTroChiPhiRepository(CTSVNUCE_DATAContext _context) : base(_context)
        {
        }

        /// <summary>
        /// Lấy theo đợt mới nhất
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public IQueryable<AsAcademyStudentSvDeNghiHoTroChiPhiHocTap> GetAllDangKyChoO(long studentId)
        {
            var dotActive = _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot.FirstOrDefault(d => d.IsActive);
            long dotDangKy = 0;
            if (dotActive != null)
            {
                dotDangKy = dotActive.Id;
            }
            return _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTap.AsNoTracking()
                    .Where(item => item.StudentId == studentId && item.DotDangKy == dotDangKy && (item.Deleted != null || !item.Deleted.Value))
                    .OrderByDescending(item => item.LastModifiedTime)
                    .AsQueryable();
        }

        /// <summary>
        /// Thêm và kiểm tra xem đã thêm chưa
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddDangKy(AsAcademyStudentSvDeNghiHoTroChiPhiHocTap model)
        {
            var dotActive = await _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot.FirstOrDefaultAsync(d => d.IsActive);
            if (dotActive == null)
            {
                throw new RecordNotFoundException("Không có đợt được mở");
            }

            if (DateTime.Now > dotActive.DenNgay)
            {
                throw new Exception("Ngoài thời gian yêu cầu");
            }

            if (_context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTap.Any(dk =>dk.StudentCode == model.StudentCode && dk.DotDangKy == dotActive.Id))
            {
                throw new Exception("Đã yêu cầu trong đợt hiện tại, không thể gửi yêu cầu thêm thêm");
            }

            await _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTap.AddAsync(model);
        }

        public async Task<IEnumerable<YeuCauDichVuStudentModel<AsAcademyStudentSvDeNghiHoTroChiPhiHocTap>>> GetAllYeuCauDichVuTheoDot(long dotDangKy)
        {
            var year = _context.AsAcademyYear.AsNoTracking().AsEnumerable()
                            .OrderByDescending(yr => yr.Id)
                            .FirstOrDefault(yr => (yr.Enabled ?? false) || (yr.IsCurrent ?? false));
            var joinStudent = (await _context.Set<AsAcademyStudentSvDeNghiHoTroChiPhiHocTap>().AsNoTracking()
                            //.Where(dk => dk.DotDangKy == dotDangKy)
                            .ToListAsync())
                            .Join(_context.AsAcademyStudent.AsNoTracking(),
                                    e => getValueString(e, "StudentCode"),
                                    student => student.Code.ToString(),
                                    (entity, student) => new { entity, student });
#if DEBUG
            //var test1 = joinStudent.Count();
#endif
            var joinClass = joinStudent
                        .Join(_context.AsAcademyClass.AsNoTracking(),
                                tmp => tmp.student.ClassCode,
                                aClass => aClass.Code,
                                (tmp, aClass) => new { tmp.student, tmp.entity, aClass });
#if DEBUG
            //var test2 = joinClass.Count();
#endif
            var joinFaculty = joinClass.Join(_context.AsAcademyFaculty.AsNoTracking(),
                               tmp => tmp.aClass.FacultyCode,
                               faculty => faculty.Code,
                               (tmp, faculty) => new { tmp.entity, tmp.student, tmp.aClass, faculty });
#if DEBUG
            //var test3 = joinFaculty.Count();
#endif
            var joinNganh = joinFaculty.GroupJoin(_context.AsAcademyAcademics.AsNoTracking(),
                                tmp => tmp.aClass.FacultyCode,
                                academic => academic.Code,
                                (tmp, academics) => new { tmp, academics });
#if DEBUG
            var test4 = joinNganh.Count();
#endif
            var result = joinNganh.SelectMany(o => o.academics.DefaultIfEmpty(), 
                (r, academic) => new YeuCauDichVuStudentModel<AsAcademyStudentSvDeNghiHoTroChiPhiHocTap>
                {
                    Year = year,
                    Student = r.tmp.student,
                    AcademyClass = r.tmp.aClass,
                    Faculty = r.tmp.faculty,
                    Academics = academic,
                    YeuCauDichVu = r.tmp.entity
                });
            return result;
        }

        public async Task<GetAllForAdminResponseRepo<AsAcademyStudentSvDeNghiHoTroChiPhiHocTap>> GetAllForAdminDangKy(QuanLyDichVuDetailModel model)
        {
            var dotActive = await _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot.FirstOrDefaultAsync(d => d.IsActive);
            long dotActiveId = 0;
            if (dotActive != null)
            {
                dotActiveId = dotActive.Id;
            }
            //quay lại 7 ngày gần đây
            DateTime? dtCompare = null;
            if (model.DayRange != null)
            {
                dtCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dtCompare = dtCompare?.AddDays(-1 * model.DayRange ?? 0);
            }
            model.SearchText = model.SearchText?.Trim()?.ToLower();

            var beforeFilteredData = (await _context.AsAcademyStudentSvDeNghiHoTroChiPhiHocTap.AsNoTracking().ToListAsync())
                                        .Where(item => item.DotDangKy == dotActiveId && item.Status > 1 && (item.Deleted == null || !item.Deleted.Value));

            var finalData = beforeFilteredData
                        .Where(item => (
                                            string.IsNullOrEmpty(model.SearchText) ||
                                            getValueString(item, "Id").Contains(model.SearchText) ||
                                            getValueString(item, "StudentCode").Contains(model.SearchText) ||
                                            getValueString(item, "StudentName").Contains(model.SearchText) ||
                                            getValueString(item, "PhanHoi").Contains(model.SearchText)
                                        ))
                        .OrderBy(r => r.Status)
                        .ThenByDescending(r => r.LastModifiedTime);
            return new GetAllForAdminResponseRepo<AsAcademyStudentSvDeNghiHoTroChiPhiHocTap>
            {
                FinalData = finalData.AsQueryable()
            };
        }
    }
}
