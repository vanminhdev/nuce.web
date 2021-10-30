using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.EduData;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.EduData
{
    public class LecturerRepository
    {
        private readonly EduDataContext _context;
        public LecturerRepository(EduDataContext _context)
        {
            this._context = _context;
        }

        public async Task<AsAcademyLecturer> FindByCode(string code)
        {
            return await _context.AsAcademyLecturer.FirstOrDefaultAsync(l => l.Code == code);
        }

        /// <summary>
        /// phòng ban được gv đó quản lý
        /// </summary>
        /// <param name="lectureCode"></param>
        /// <returns></returns>
        public string GetDepartmentCodeIsManaged(string lectureCode)
        {
            var lecture = _context.AsAcademyLecturer.FirstOrDefault(l => l.Code == lectureCode);
            if (lecture != null && !string.IsNullOrWhiteSpace(lecture.DepartmentCode))
            {
                var department = _context.AsAcademyDepartment.FirstOrDefault(d => d.Code == lecture.DepartmentCode);
                if (department != null && !string.IsNullOrWhiteSpace(department.ChefsDepartmentCode))
                {
                    var arrManager = department.ChefsDepartmentCode.Split(',');
                    if (arrManager.Contains(lectureCode))
                    {
                        return lecture.DepartmentCode;
                    }
                }
            }
            return null;
        }
    }
}
