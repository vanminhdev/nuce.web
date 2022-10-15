using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel.Ctsv;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IDeNghiHoTroChiPhiRepository : IBaseStudentServiceRepository<AsAcademyStudentSvDeNghiHoTroChiPhiHocTap>
    {
        Task<GetAllForAdminResponseRepo<AsAcademyStudentSvDeNghiHoTroChiPhiHocTap>> GetAllForAdminDangKy(QuanLyDichVuDetailModel model);
        IQueryable<AsAcademyStudentSvDeNghiHoTroChiPhiHocTap> GetAllDangKyChoO(string studentCode);
        Task AddDangKy(AsAcademyStudentSvDeNghiHoTroChiPhiHocTap model);
        Task<IEnumerable<YeuCauDichVuStudentModel<AsAcademyStudentSvDeNghiHoTroChiPhiHocTap>>> GetAllYeuCauDichVuTheoDot(long dotDangKy);
    }
}
