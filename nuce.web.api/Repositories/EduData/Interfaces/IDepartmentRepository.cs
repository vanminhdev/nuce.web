using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.EduData;

namespace nuce.web.api.Repositories.EduData.Interfaces
{
    public interface IDepartmentRepository
    {
        public Task<AsAcademyDepartment> FindByCode(string code);
        public IQueryable<AsAcademyLecturer> GetLecturer(string code);
    }
}
