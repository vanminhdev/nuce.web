using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IHoTroHocTapRepository : IBaseStudentServiceRepository<AsAcademyStudentSvDangKyHoTroHocTap>
    {
        Task<GetAllForAdminResponseRepo<AsAcademyStudentSvDangKyHoTroHocTap>> GetAllForAdminDangKyChoO(QuanLyDichVuDetailModel model);
        IQueryable<AsAcademyStudentSvDangKyHoTroHocTap> GetAllDangKyChoO(string studentCode);
        Task AddDangKyNhaO(AsAcademyStudentSvDangKyHoTroHocTap model);
        Task<IEnumerable<YeuCauDichVuStudentModel<AsAcademyStudentSvDangKyHoTroHocTap>>> GetAllYeuCauDichVuTheoDot(long dotDangKy);
    }
}
