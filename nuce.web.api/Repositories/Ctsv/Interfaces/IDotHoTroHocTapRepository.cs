using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IDotHoTroHocTapRepository : IBaseStudentServiceRepository<AsAcademyStudentSvDangKyHoTroHocTapDot>
    {
        Task<AsAcademyStudentSvDangKyHoTroHocTapDot> GetDotActive();
        Task<PaginationModel<AsAcademyStudentSvDangKyHoTroHocTapDot>> GetAll(int skip = 0, int take = 20);
        Task Add(AddDotHoTroHocTap model);
        Task Update(int id, AddDotHoTroHocTap model);
        Task Delete(int id);
    }
}
