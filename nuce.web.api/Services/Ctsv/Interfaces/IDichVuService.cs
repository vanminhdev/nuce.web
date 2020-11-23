using GemBox.Document;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static nuce.web.api.Common.Ctsv;

namespace nuce.web.api.Services.Ctsv.Interfaces
{
    public interface IDichVuService
    {
        public IQueryable GetAllByStudent(int dichVuType);
        public Task AddDichVu(DichVuModel model);
        public Dictionary<int, AllTypeDichVuModel> GetAllLoaiDichVuInfo();
        public Task<DataTableResponse<QuanLyDichVuDetailResponse>> GetRequestForAdmin(QuanLyDichVuDetailModel model);
        public Task<ResponseBody> UpdateRequestStatus(UpdateRequestStatusModel model);
        public Task UpdateMultiRequestToFourStatus(DichVu loaiDichVu, List<DichVuExport> dichVuList);
        public Task<byte[]> ExportWordAsync(DichVu dichVu, int id);
        public Task<byte[]> ExportWordListAsync(DichVu dichVu, List<DichVuExport> dichVuList);
        public Task<byte[]> ExportExcelAsync(DichVu loaiDichVu, List<DichVuExport> dichVuList);
        public Task<byte[]> ExportExcelOverviewAsync();
    }
}
