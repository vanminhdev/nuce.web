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
    public class XinMienGiamHocPhiRepository : BaseStudentServiceRepository<AsAcademyStudentSvXinMienGiamHocPhi>, IXinMienGiamHocPhiRepository
    {
        public XinMienGiamHocPhiRepository(CTSVNUCE_DATAContext _context) : base(_context)
        {
        }

        /// <summary>
        /// Lấy theo đợt mới nhất
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public IQueryable<AsAcademyStudentSvXinMienGiamHocPhi> GetAllDangKyChoO(long studentId)
        {
            var dotActive = _context.AsAcademyStudentSvXinMienGiamHocPhiDot.FirstOrDefault(d => d.IsActive);
            long dotDangKy = 0;
            if (dotActive != null)
            {
                dotDangKy = dotActive.Id;
            }
            return _context.AsAcademyStudentSvXinMienGiamHocPhi.AsNoTracking()
                    .Where(item => item.StudentId == studentId && item.DotDangKy == dotDangKy && (item.Deleted != null || !item.Deleted.Value))
                    .OrderByDescending(item => item.LastModifiedTime)
                    .AsQueryable();
        }

        /// <summary>
        /// Thêm và kiểm tra xem đã thêm chưa
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddDangKy(AsAcademyStudentSvXinMienGiamHocPhi model)
        {
            var dotActive = await _context.AsAcademyStudentSvXinMienGiamHocPhiDot.FirstOrDefaultAsync(d => d.IsActive);
            if (dotActive == null)
            {
                throw new RecordNotFoundException("Không có đợt được mở");
            }

            if (DateTime.Now > dotActive.DenNgay)
            {
                throw new Exception("Ngoài thời gian yêu cầu");
            }

            if (_context.AsAcademyStudentSvXinMienGiamHocPhi.Any(dk =>dk.StudentCode == model.StudentCode && dk.DotDangKy == dotActive.Id))
            {
                throw new Exception("Đã yêu cầu trong đợt hiện tại, không thể gửi thêm");
            }

            await _context.AsAcademyStudentSvXinMienGiamHocPhi.AddAsync(model);
        }

        public async Task<IEnumerable<YeuCauDichVuStudentModel<AsAcademyStudentSvXinMienGiamHocPhi>>> GetAllYeuCauDichVuTheoDot(long dotDangKy)
        {
            var year = _context.AsAcademyYear.AsNoTracking().AsEnumerable()
                            .OrderByDescending(yr => yr.Id)
                            .FirstOrDefault(yr => (yr.Enabled ?? false) || (yr.IsCurrent ?? false));
            var joinStudent = (await _context.Set<AsAcademyStudentSvXinMienGiamHocPhi>().AsNoTracking().Where(dk => dk.DotDangKy == dotDangKy).ToListAsync())
                        .Join(_context.AsAcademyStudent.AsNoTracking(),
                                e => getValueString(e, "StudentId"),
                                student => student.Id.ToString(),
                                (entity, student) => new { entity, student });
#if DEBUG
            //var test1 = joinStudent.Count();
#endif
            var joinClass = joinStudent
                        .Join(_context.AsAcademyClass.AsNoTracking(),
                                tmp => tmp.student.ClassId,
                                aClass => aClass.Id,
                                (tmp, aClass) => new { tmp.student, tmp.entity, aClass });
#if DEBUG
            //var test2 = joinClass.Count();
#endif
            var joinFaculty = joinClass.Join(_context.AsAcademyFaculty.AsNoTracking(),
                               tmp => tmp.aClass.FacultyId,
                               faculty => faculty.Id,
                               (tmp, faculty) => new { tmp.entity, tmp.student, tmp.aClass, faculty });
#if DEBUG
            //var test3 = joinFaculty.Count();
#endif
            var joinNganh = joinFaculty.GroupJoin(_context.AsAcademyAcademics.AsNoTracking(),
                                tmp => tmp.aClass.AcademicsId,
                                academic => academic.Id,
                                (tmp, academics) => new { tmp, academics });
#if DEBUG
            var test4 = joinNganh.Count();
#endif
            var result = joinNganh.SelectMany(o => o.academics.DefaultIfEmpty(), 
                (r, academic) => new YeuCauDichVuStudentModel<AsAcademyStudentSvXinMienGiamHocPhi>
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

        public async Task<GetAllForAdminResponseRepo<AsAcademyStudentSvXinMienGiamHocPhi>> GetAllForAdminDangKy(QuanLyDichVuDetailModel model)
        {
            var dotActive = await _context.AsAcademyStudentSvXinMienGiamHocPhiDot.FirstOrDefaultAsync(d => d.IsActive);
            long dotActiveId = 0;
            if (dotActive != null)
            {
                dotActiveId = dotActive.Id;
            }
            //quay lại 7 ngày gần đây
            DateTime dtCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtCompare = dtCompare.AddDays(-1 * model.DayRange);
            model.SearchText = model.SearchText?.Trim()?.ToLower();

            var beforeFilteredData = (await _context.AsAcademyStudentSvXinMienGiamHocPhi.AsNoTracking().ToListAsync())
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
            return new GetAllForAdminResponseRepo<AsAcademyStudentSvXinMienGiamHocPhi>
            {
                FinalData = finalData.AsQueryable()
            };
        }
    }
}
