using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Ctsv;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IDotDangKyChoORepository : IBaseStudentServiceRepository<AsAcademyStudentSvDangKyChoODot>
    {
        Task<AsAcademyStudentSvDangKyChoODot> GetDotActive();
        Task<PaginationModel<AsAcademyStudentSvDangKyChoODot>> GetAll(int skip = 0, int take = 20);
        Task Add(AddDotDangKyChoOModel model);
        Task Update(int id, AddDotDangKyChoOModel model);
        Task Delete(int id);
    }
}
