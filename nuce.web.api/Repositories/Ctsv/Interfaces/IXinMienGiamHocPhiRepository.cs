using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel.Ctsv;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IXinMienGiamHocPhiRepository : IBaseStudentServiceRepository<AsAcademyStudentSvXinMienGiamHocPhi>
    {
        Task<GetAllForAdminResponseRepo<AsAcademyStudentSvXinMienGiamHocPhi>> GetAllForAdminDangKy(QuanLyDichVuDetailModel model);
        IQueryable<AsAcademyStudentSvXinMienGiamHocPhi> GetAllDangKyChoO(string studentCode);
        Task AddDangKy(AsAcademyStudentSvXinMienGiamHocPhi model);
        Task<IEnumerable<YeuCauDichVuStudentModel<AsAcademyStudentSvXinMienGiamHocPhi>>> GetAllYeuCauDichVuTheoDot(long dotDangKy);
    }
}
