using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Interfaces
{
    public interface IDichVuService
    {
        public IQueryable GetAllByStudent(int dichVuType);
        public Task<ResponseBody> AddDichVu(DichVuModel model);
        public List<AllTypeDichVuModel> GetAllLoaiDichVuInfo();
        public Task<IQueryable> GetRequestForAdmin(QuanLyDichVuDetailModel model);
        public Task<ResponseBody> UpdateRequestStatus(UpdateRequestStatusModel model);
    }
}
