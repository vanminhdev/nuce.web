using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static nuce.web.api.Common.Ctsv;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class BaseStudentServiceRepository<Entity> : IBaseStudentServiceRepository<Entity> where Entity : class
    {
        protected readonly CTSVNUCE_DATAContext _context;
        public BaseStudentServiceRepository(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }

        public async Task AddAsync(Entity model)
        {
            await _context.Set<Entity>().AddAsync(model);
        }

        public IQueryable<Entity> GetAll(long studentId)
        {
            return _context.Set<Entity>().AsNoTracking().AsEnumerable()
                    .Where(item => Convert.ToInt32(item.GetType().GetProperty("StudentId").GetValue(item, null)) == studentId &&
                            !Convert.ToBoolean(item.GetType().GetProperty("Deleted").GetValue(item, null)))
                    .OrderByDescending(item => item.GetType().GetProperty("LastModifiedTime").GetValue(item, null))
                    .AsQueryable();
        }

        public IQueryable<Entity> GetAll(string studentCode)
        {
            return _context.Set<Entity>().AsNoTracking().AsEnumerable()
                    .Where(item => item.GetType().GetProperty("StudentCode").GetValue(item, null)?.ToString() == studentCode &&
                            !Convert.ToBoolean(item.GetType().GetProperty("Deleted").GetValue(item, null)))
                    .OrderByDescending(item => item.GetType().GetProperty("LastModifiedTime").GetValue(item, null))
                    .AsQueryable();
        }

        public async Task<GetAllForAdminResponseRepo<Entity>> GetAllForAdmin(QuanLyDichVuDetailModel model)
        {
            DateTime? dtCompare = null;
            if (model.DayRange != null)
            {
                dtCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dtCompare = dtCompare?.AddDays(-1 * model.DayRange ?? 0);
            }
            model.SearchText = model.SearchText?.Trim()?.ToLower();

            var beforeFilteredData = (await _context.Set<Entity>().AsNoTracking().ToListAsync())
                                        .Where(item => (int)getValue(item, "Status") > 1 && !Convert.ToBoolean(getValue(item, "Deleted")));

            var finalData = beforeFilteredData
                        .Where(item => (
                                            string.IsNullOrEmpty(model.SearchText) ||
                                            getValueString(item, "Id").Contains(model.SearchText) ||
                                            getValueString(item, "StudentCode").Contains(model.SearchText) ||
                                            getValueString(item, "StudentName").Contains(model.SearchText) ||
                                            getValueString(item, "PhanHoi").Contains(model.SearchText)
                                        ) &&
                                        (dtCompare == null || DateTime.Parse(getValueString(item, "LastModifiedTime")) >= dtCompare))
                        .OrderBy(r => getValue(r, "Status"))
                        .ThenByDescending(r => getValue(r, "LastModifiedTime"));
            return new GetAllForAdminResponseRepo<Entity>
            {
                FinalData = finalData.AsQueryable()
            };
        }

        public bool IsDuplicated(long studentId, string reason = null)
        {
            return _context.Set<Entity>().AsNoTracking().AsEnumerable()
                    .Any(item => Convert.ToInt32(item.GetType().GetProperty("StudentId").GetValue(item, null)) == studentId &&
                            Convert.ToInt32(item.GetType().GetProperty("Status").GetValue(item, null)) < (int)TrangThaiYeuCau.DaXuLyVaCoLichHen &&
                           (string.IsNullOrEmpty(reason) || item.GetType().GetProperty("LyDo").GetValue(item, null)?.ToString() == reason));
        }

        public async Task<IEnumerable<YeuCauDichVuStudentModel<Entity>>> GetYeuCauDichVuStudent(List<long> ids)
        {
            var year = _context.AsAcademyYear.AsNoTracking().AsEnumerable()
                            .OrderByDescending(yr => yr.Id)
                            .FirstOrDefault(yr => (yr.Enabled ?? false) || (yr.IsCurrent ?? false));
            //var test = (await _context.Set<Entity>().AsNoTracking().ToListAsync())
            //            .Where(e => ids.Contains((long)getValue(e, "Id")))
            //            .Join(_context.AsAcademyStudent.AsNoTracking(),
            //                    e => getValueString(e, "StudentCode"),
            //                    student => student.Code.ToLower(),
            //                    (entity, student) => new { entity, student })
            //            .ToList();
            var result = (await _context.Set<Entity>().AsNoTracking().ToListAsync())
                        .Where(e => ids.Contains((long)getValue(e, "Id")))
                        .Join(_context.AsAcademyStudent.AsNoTracking(),
                                e => getValueString(e, "StudentCode"),
                                student => student.Code.ToLower(),
                                (entity, student) => new { entity, student }).Join(_context.AsAcademyClass.AsNoTracking(),
                                tmp => tmp.student.ClassCode,
                                aClass => aClass.Code,
                                (tmp, aClass) => new { tmp.student, tmp.entity, aClass })
                        .Join(_context.AsAcademyFaculty.AsNoTracking(),
                               tmp => tmp.aClass.FacultyCode,
                               faculty => faculty.Code,
                               (tmp, faculty) => new { tmp.entity, tmp.student, tmp.aClass, faculty })
                        .Join(_context.AsAcademyAcademics.AsNoTracking(),
                                tmp => tmp.aClass.AcademicsCode,
                                academic => academic.Code,
                                (tmp, academic) => new YeuCauDichVuStudentModel<Entity>
                                {
                                    Year = year,
                                    Student = tmp.student,
                                    AcademyClass = tmp.aClass,
                                    Faculty = tmp.faculty,
                                    Academics = academic,
                                    YeuCauDichVu = tmp.entity
                                });
            return result;
        }

        public AllTypeDichVuModel GetRequestInfo()
        {
            var allRequest = _context.Set<Entity>().AsNoTracking().ToList().Where(r => getValue(r, "Status") != null);
            var result = new AllTypeDichVuModel
            {
                TongSo = 0,
                MoiGui = 0,
                DangXuLy = 0,
                DaXuLy = 0,
            };
            foreach (var request in allRequest)
            {
                result.TongSo++;
                switch (getValue(request, "Status"))
                {
                    case (int)TrangThaiYeuCau.DaGuiLenNhaTruong:
                        result.MoiGui++;
                        break;
                    case (int)TrangThaiYeuCau.DangXuLy:
                        result.DangXuLy++;
                        break;
                    case (int)TrangThaiYeuCau.HoanThanh:
                    case (int)TrangThaiYeuCau.TuChoi:
                    case (int)TrangThaiYeuCau.DaXuLyVaCoLichHen:
                        result.DaXuLy++;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        public async Task<Entity> FindByIdAsync(long id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }

        private object getValue(Entity entity, string field)
        {
            return entity.GetType().GetProperty(field).GetValue(entity, null);
        }

        protected string getValueString(Entity entity, string field)
        {
            var value = entity.GetType().GetProperty(field).GetValue(entity, null);
            return value != null ? value.ToString().ToLower() : "";
        }

    }
}
