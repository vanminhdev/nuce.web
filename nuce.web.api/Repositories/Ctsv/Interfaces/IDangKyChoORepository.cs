using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel.Ctsv;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IDangKyChoORepository : IBaseStudentServiceRepository<AsAcademyStudentSvDangKyChoO>
    {
        Task<GetAllForAdminResponseRepo<AsAcademyStudentSvDangKyChoO>> GetAllForAdminDangKyChoO(QuanLyDichVuDetailModel model);
        IQueryable<AsAcademyStudentSvDangKyChoO> GetAllDangKyChoO(string studentCode);
        Task AddDangKyNhaO(AsAcademyStudentSvDangKyChoO model);
        Task<IEnumerable<YeuCauDichVuStudentModel<AsAcademyStudentSvDangKyChoO>>> GetAllYeuCauDichVuTheoDot(long dotDangKy);
    }
}
