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

        #region đăng ký chỗ ở
        /// <summary>
        /// Lấy đợt đang active nếu không có trả về -1
        /// </summary>
        /// <returns></returns>
        public Task<AsAcademyStudentSvDangKyChoODot> GetDotDangKyChoOActive();
        public Task<PaginationModel<AsAcademyStudentSvDangKyChoODot>> GetAllDotDangKyChoO(int skip = 0, int pageSize = 20);
        public Task AddDotDangKyChoO(AddDotDangKyChoOModel model);
        public Task UpdateDotDangKyChoO(int id, AddDotDangKyChoOModel model);
        public Task DeleteDotDangKyChoO(int id);
        #endregion

        #region xin miễn giảm học phí
        /// <summary>
        /// Lấy đợt đang active nếu không có trả về -1
        /// </summary>
        /// <returns></returns>
        public Task<AsAcademyStudentSvXinMienGiamHocPhiDot> GetDotXinMienGiamHocPhiActive();
        public Task<PaginationModel<AsAcademyStudentSvXinMienGiamHocPhiDot>> GetAllDotXinMienGiamHocPhi(int skip = 0, int pageSize = 20);
        public Task AddDotXinMienGiamHocPhi(AddDotXinMienGiamHocPhi model);
        public Task UpdateDotXinMienGiamHocPhi(int id, AddDotXinMienGiamHocPhi model);
        public Task DeleteDotXinMienGiamHocPhi(int id);
        #endregion

        #region đề nghị hỗ trợ chi phí
        /// <summary>
        /// Lấy đợt đang active nếu không có trả về -1
        /// </summary>
        /// <returns></returns>
        public Task<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot> GetDotDeNghiHoTroChiPhiActive();
        public Task<PaginationModel<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot>> GetAllDotDeNghiHoTroChiPhi(int skip = 0, int pageSize = 20);
        public Task AddDotDeNghiHoTroChiPhi(AddDotDeNghiHoTroChiPhi model);
        public Task UpdateDotDeNghiHoTroChiPhi(int id, AddDotDeNghiHoTroChiPhi model);
        public Task DeleteDotDeNghiHoTroChiPhi(int id);
        #endregion

        #region đề nghị hỗ trợ học tập
        /// <summary>
        /// Lấy đợt đang active nếu không có trả về -1
        /// </summary>
        /// <returns></returns>
        public Task<AsAcademyStudentSvDangKyHoTroHocTapDot> GetDotHoTroHocTapActive();
        public Task<PaginationModel<AsAcademyStudentSvDangKyHoTroHocTapDot>> GetAllDotHoTroHocTap(int skip = 0, int pageSize = 20);
        public Task AddDotHoTroHocTap(AddDotHoTroHocTap model);
        public Task UpdateDotHoTroHocTap(int id, AddDotHoTroHocTap model);
        public Task DeleteDotHoTroHocTap(int id);
        #endregion
    }
}
