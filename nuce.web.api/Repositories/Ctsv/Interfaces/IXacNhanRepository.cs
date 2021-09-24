using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IXacNhanRepository : IBaseStudentServiceRepository<AsAcademyStudentSvXacNhan>
    {
        public GetAllForAdminResponseRepo<AsAcademyStudentSvXacNhan> GetAllForAdminCustom(QuanLyDichVuDetailModel model);
    }
}
