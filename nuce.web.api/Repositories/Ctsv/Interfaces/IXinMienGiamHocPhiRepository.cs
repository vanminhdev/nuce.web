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
        Task<GetAllForAdminResponseRepo<AsAcademyStudentSvXinMienGiamHocPhi>> GetAllForAdminDangKyChoO(QuanLyDichVuDetailModel model);
        IQueryable<AsAcademyStudentSvXinMienGiamHocPhi> GetAllDangKyChoO(long studentId);
        Task AddDangKy(AsAcademyStudentSvXinMienGiamHocPhi model);
        Task<IEnumerable<YeuCauDichVuStudentModel<AsAcademyStudentSvXinMienGiamHocPhi>>> GetAllYeuCauDichVuTheoDot(long dotDangKy);
    }
}
