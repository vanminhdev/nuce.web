using GemBox.Document;
using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static nuce.web.api.Common.Ctsv;

namespace nuce.web.api.ViewModel.Ctsv
{
    /// <summary>
    /// Model tạo mới yêu cầu dịch vụ
    /// Dùng chung cho các dịch vụ
    /// </summary>
    public class DichVuModel
    {
        public int Type { get; set; }
        public string LyDo { get; set; }
        public string KyLuat { get; set; }
        public string ThuocDien { get; set; }
        public string ThuocDoiTuong { get; set; }
        public string DonVi { get; set; }
        public string DenGap { get; set; }
        public string VeViec { get; set; }
        public string MaXacNhan { get; set; }
        public string PhanHoi { get; set; }
        /// <summary>
        /// xe bus
        /// </summary>
        public int VeBusTuyenType { get; set; }
        /// <summary>
        /// xe bus
        /// </summary>
        public string VeBusTuyenCode { get; set; }
        /// <summary>
        /// xe bus
        /// </summary>
        public string VeBusTuyenName { get; set; }
        /// <summary>
        /// xe bus
        /// </summary>
        public string VeBusNoiNhanThe { get; set; }
        /// <summary>
        /// mượn học bạ gốc
        /// </summary>
        public string ThoiGianMuon { get; set; }
        /// <summary>
        /// Option gửi mail cho sv khi tạo mới yêu cầu
        /// </summary>
        public bool NotSendEmail { get; set; }
        public string Sdt { get; set; }
        /// <summary>
        /// nhà ở
        /// </summary>
        public string NhuCauNhaO { get; set; }
        /// <summary>
        /// nhà ở
        /// </summary>
        public string DoiTuongUuTienNhaO { get; set; }
        /// <summary>
        /// xin miễn giảm học phí
        /// </summary>
        public string DoiTuongHuongMienGiam { get; set; }
        /// <summary>
        /// đề nghị hỗ trợ chi phí học tập
        /// </summary>
        public string DoiTuongDeNghiHoTro { get; set; }

        /// <summary>
        /// chon mau in uu dai giao duc
        /// </summary>
        public int MauSo { get; set; }
    }

    public class AllTypeDichVuModel
    {
        public string TenDichVu { get; set; }
        public string LinkDichVu { get; set; }
        public int TongSo { get; set; }
        public int MoiGui { get; set; }
        public int DaXuLy { get; set; }
        public int DangXuLy { get; set; }
        public int Stt { get; set; }
    }

    public class QuanLyDichVuDetailModel
    {
        public int Type { get; set; }
        public string SearchText { get; set; }
        public int? DayRange { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
    }

    public class QuanLyDichVuDetailResponse
    {
        public object DichVu { get; set; }
        public AsAcademyStudent Student { get; set; }
    }
    /// <summary>
    /// Model cập nhật trạng thái yêu cầu
    /// </summary>
    public class UpdateRequestStatusModel
    {
        public int Type { get; set; }
        public int RequestID { get; set; }
        public string PhanHoi { get; set; }
        public DateTime? NgayHenBatDau { get; set; }
        public DateTime? NgayHenKetThuc { get; set; }
        public bool AutoUpdateNgayHen { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// Chứa các yêu cầu dịch vụ khi cập nhật nhiều yêu cầu sang trạng thái 4 | Đã xử lý có lịch hẹn
        /// </summary>
        public List<DichVuExport> DichVuList { get; set; }
        public UpdateRequestStatusMuonHocBaGocModel MuonHocBaGoc { get; set; }
    }

    public class UpdateRequestStatusMuonHocBaGocModel
    {
        public int Id { get; set; }
        public DateTime? NgayMuonThucTe { get; set; }
        public DateTime? NgayTraDuKien { get; set; }
        public string Notice { get; set; }
        public string Description { get; set; }
    }

    public class GetAllForAdminResponseRepo<T>
    {
        public IQueryable<T> FinalData { get; set; }
        public IQueryable<T> BeforeFilteredData { get; set; }
    }

    public class ExportModel
    {
        public List<DichVuExport> DichVuList { get; set; }
        public DichVu DichVuType { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }

    public class DichVuExport
    {
        public int ID { get; set; }
    }

    public class StudentDichVuModel
    {
        public AsAcademyStudent Student { get; set; }
        public AsAcademyClass AcademyClass { get; set; }
        public AsAcademyFaculty Faculty { get; set; }
        public AsAcademyDepartment Department { get; set; }
        public AsAcademyAcademics Academics { get; set; }
        public AsAcademyYear Year { get; set; }
    }

    public class YeuCauDichVuStudentModel<T> : StudentDichVuModel
    {
        public T YeuCauDichVu { get; set; }
    }

    public class ExportFileOutputModel
    {
        public DocumentModel document { get; set; }
        public string filePath { get; set; }
    }

    public class UpdateStatusMultiFourModel
    {
        public DichVu LoaiDichVu { get; set; }
        public List<DichVuExport> YeuCauList { get; set; }
    }

    public class GetUpdateStatusNgayHenModel
    {
        public DateTime? NgayHenBatDau { get; set; }
        public DateTime? NgayHenKetThuc { get; set; }
    }

    public class AddDotDangKyChoOModel
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Từ ngày không được để trống")]
        public DateTime? TuNgay { get; set; }
        [Required(ErrorMessage = "Đến ngày không được để trống")]
        public DateTime? DenNgay { get; set; }
    }

    public class AddDotXinMienGiamHocPhi
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Từ ngày không được để trống")]
        public DateTime? TuNgay { get; set; }
        [Required(ErrorMessage = "Đến ngày không được để trống")]
        public DateTime? DenNgay { get; set; }
    }

    public class AddDotDeNghiHoTroChiPhi
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Từ ngày không được để trống")]
        public DateTime? TuNgay { get; set; }
        [Required(ErrorMessage = "Đến ngày không được để trống")]
        public DateTime? DenNgay { get; set; }
    }

    public class AddDotHoTroHocTap
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Từ ngày không được để trống")]
        public DateTime? TuNgay { get; set; }
        [Required(ErrorMessage = "Đến ngày không được để trống")]
        public DateTime? DenNgay { get; set; }
    }
}
