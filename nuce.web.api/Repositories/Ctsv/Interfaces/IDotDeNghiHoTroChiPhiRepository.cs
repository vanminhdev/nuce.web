using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Ctsv;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IDotDeNghiHoTroChiPhiRepository : IBaseStudentServiceRepository<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot>
    {
        Task<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot> GetDotActive();
        Task<PaginationModel<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot>> GetAll(int skip = 0, int take = 20);
        Task Add(AddDotDeNghiHoTroChiPhi model);
        Task Update(int id, AddDotDeNghiHoTroChiPhi model);
        Task Delete(int id);
    }
}
