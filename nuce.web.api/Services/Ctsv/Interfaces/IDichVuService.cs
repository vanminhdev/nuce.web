using GemBox.Document;
using nuce.web.api.Models.Ctsv;
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
        public Task UpdateMultiRequestToFourStatus(UpdateRequestStatusModel model);
        public Task UpdateRequestStatus(UpdateRequestStatusModel model);
        public Task<byte[]> ExportWordAsync(DichVu dichVu, int id);
        public Task<byte[]> ExportWordListAsync(DichVu dichVu, List<DichVuExport> dichVuList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loaiDichVu"></param>
        /// <param name="dichVuList"></param>
        /// <param name="dotDangKyNhaO">Chỉ dùng cho dịch vụ đăng ký nhà ở</param>
        /// <returns></returns>
        public Task<byte[]> ExportExcelAsync(DichVu loaiDichVu, List<DichVuExport> dichVuList, long dotDangKyNhaO = 0);
        public Task<byte[]> ExportExcelOverviewAsync();
        public Task UpdateThamSoDichVu(Dictionary<long, string> thamSoDictionary);
        public Task<DataTableResponse<AsAcademyStudentSvThietLapThamSoDichVu>> GetThamSoByDichVu(int loaiDichVu);
        public Task UpdatePartialInfoMuonHocBa(UpdateRequestStatusMuonHocBaGocModel model);
        public Task<byte[]> ExportWordMuonHocBaAsync(string studentCode);
        /// <summary>
        /// Lấy đợt đang active nếu không có trả về -1
        /// </summary>
        /// <returns></returns>
        public Task<AsAcademyStudentSvDangKyChoODot> GetDotDangKyChoOActive();
        public Task<PaginationModel<AsAcademyStudentSvDangKyChoODot>> GetAllDotDangKyChoO(int skip = 0, int pageSize = 20);
        public Task AddDotDangKyChoO(AddDotDangKyChoOModel model);
        public Task UpdateDotDangKyChoO(int id, AddDotDangKyChoOModel model);
        public Task DeleteDotDangKyChoO(int id);
    }
}
