using Microsoft.EntityFrameworkCore;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class HoTroHocTapRepository : BaseStudentServiceRepository<AsAcademyStudentSvDangKyHoTroHocTap>, IHoTroHocTapRepository
    {
        public HoTroHocTapRepository(CTSVNUCE_DATAContext _context) : base(_context)
        {
        }

        /// <summary>
        /// Lấy theo đợt mới nhất
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public IQueryable<AsAcademyStudentSvDangKyHoTroHocTap> GetAllDangKyChoO(long studentId)
        {
            var dotActive = _context.AsAcademyStudentSvDangKyHoTroHocTapDot.FirstOrDefault(d => d.IsActive);
            long dotDangKy = 0;
            if (dotActive != null)
            {
                dotDangKy = dotActive.Id;
            }
            return _context.AsAcademyStudentSvDangKyHoTroHocTap.AsNoTracking()
                    .Where(item => item.StudentId == studentId && item.DotDangKy == dotDangKy && (item.Deleted != null || !item.Deleted.Value))
                    .OrderByDescending(item => item.LastModifiedTime)
                    .AsQueryable();
        }

        /// <summary>
        /// Thêm và kiểm tra xem đã thêm chưa
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddDangKyNhaO(AsAcademyStudentSvDangKyHoTroHocTap model)
        {
            var dotActive = await _context.AsAcademyStudentSvDangKyHoTroHocTapDot.FirstOrDefaultAsync(d => d.IsActive);
            if (dotActive == null)
            {
                throw new RecordNotFoundException("Không có đợt đăng ký");
            }

            if (DateTime.Now > dotActive.DenNgay)
            {
                throw new Exception("Ngoài thời gian đăng ký");
            }

            if (_context.AsAcademyStudentSvDangKyHoTroHocTap.Any(dk => dk.StudentCode == model.StudentCode && dk.DotDangKy == dotActive.Id))
            {
                throw new Exception("Đã đăng ký trong đợt hiện tại, không thể đăng ký thêm");
            }

            await _context.AsAcademyStudentSvDangKyHoTroHocTap.AddAsync(model);
        }

        public async Task<IEnumerable<YeuCauDichVuStudentModel<AsAcademyStudentSvDangKyHoTroHocTap>>> GetAllYeuCauDichVuTheoDot(long dotDangKy)
        {
            var year = _context.AsAcademyYear.AsNoTracking().AsEnumerable()
                            .OrderByDescending(yr => yr.Id)
                            .FirstOrDefault(yr => (yr.Enabled ?? false) || (yr.IsCurrent ?? false));
            var joinStudent = (await _context.Set<AsAcademyStudentSvDangKyHoTroHocTap>().AsNoTracking().Where(dk => dk.DotDangKy == dotDangKy).ToListAsync())
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
                                tmp => tmp.aClass.AcademicsCode,
                                academic => academic.Code,
                                (tmp, academics) => new { tmp, academics });
#if DEBUG
            var test4 = joinNganh.Count();
#endif
            var result = joinNganh.SelectMany(o => o.academics.DefaultIfEmpty(),
                (r, academic) => new YeuCauDichVuStudentModel<AsAcademyStudentSvDangKyHoTroHocTap>
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

        public async Task<GetAllForAdminResponseRepo<AsAcademyStudentSvDangKyHoTroHocTap>> GetAllForAdminDangKyChoO(QuanLyDichVuDetailModel model)
        {
            var dotActive = await _context.AsAcademyStudentSvDangKyHoTroHocTapDot.FirstOrDefaultAsync(d => d.IsActive);
            long dotActiveId = 0;
            if (dotActive != null)
            {
                dotActiveId = dotActive.Id;
            }
            DateTime? dtCompare = null;
            if (model.DayRange != null)
            {
                dtCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dtCompare = dtCompare?.AddDays(-1 * model.DayRange ?? 0);
            }
            model.SearchText = model.SearchText?.Trim()?.ToLower();

            var beforeFilteredData = (await _context.AsAcademyStudentSvDangKyHoTroHocTap.AsNoTracking().ToListAsync())
                                        .Where(item => item.DotDangKy == dotActiveId && item.Status > 1 && (item.Deleted == null || !item.Deleted.Value));

            var finalData = beforeFilteredData
                        .Where(item => (
                                            string.IsNullOrEmpty(model.SearchText) ||
                                            getValueString(item, "Id").Contains(model.SearchText) ||
                                            getValueString(item, "StudentCode").Contains(model.SearchText) ||
                                            getValueString(item, "StudentName").Contains(model.SearchText) ||
                                            getValueString(item, "PhanHoi").Contains(model.SearchText)
                                        ) &&
                                        (dtCompare == null || DateTime.Parse(getValueString(item, "LastModifiedTime")) >= dtCompare))
                        .OrderBy(r => r.Status)
                        .ThenByDescending(r => r.LastModifiedTime);
            return new GetAllForAdminResponseRepo<AsAcademyStudentSvDangKyHoTroHocTap>
            {
                FinalData = finalData.AsQueryable()
            };
        }
    }
}
