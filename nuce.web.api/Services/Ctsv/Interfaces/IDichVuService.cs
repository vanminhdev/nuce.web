using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Base;
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
        public Dictionary<int, AllTypeDichVuModel> GetAllLoaiDichVuInfo();
        public Task<DataTableResponse<QuanLyDichVuDetailResponse>> GetRequestForAdmin(QuanLyDichVuDetailModel model);
        public Task<ResponseBody> UpdateRequestStatus(UpdateRequestStatusModel model);
    }
}
