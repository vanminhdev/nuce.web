using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nuce.CTSV.ApiModels
{
    #region Model
    public class XacNhanModel
    {
        public long Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public int? Status { get; set; }
        public string LyDo { get; set; }
        public string PhanHoi { get; set; }
        public bool? Deleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public DateTime? NgayGui { get; set; }
        public DateTime? NgayHenTuNgay { get; set; }
        public DateTime? NgayHenDenNgay { get; set; }
        public string MaXacNhan { get; set; }
    }
    [Serializable]
    public class StudentModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string FulName { get; set; }
        public long? ClassId { get; set; }
        public string ClassCode { get; set; }
        public string DateOfBirth { get; set; }
        public string BirthPlace { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Guid? KeyAuthorize { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string DanToc { get; set; }
        public string TonGiao { get; set; }
        public string HkttSoNha { get; set; }
        public string HkttPho { get; set; }
        public string HkttPhuong { get; set; }
        public string HkttQuan { get; set; }
        public string HkttTinh { get; set; }
        public string Cmt { get; set; }
        public DateTime? CmtNgayCap { get; set; }
        public string CmtNoiCap { get; set; }
        public string NamTotNghiepPtth { get; set; }
        public DateTime? NgayVaoDoan { get; set; }
        public DateTime? NgayVaoDang { get; set; }
        public string DiemThiPtth { get; set; }
        public string KhuVucHktt { get; set; }
        public string DoiTuongUuTien { get; set; }
        public bool? DaTungLamCanBoLop { get; set; }
        public bool? DaTungLamCanBoDoan { get; set; }
        public bool? DaThamGiaDoiTuyenThiHsg { get; set; }
        public string BaoTinDiaChi { get; set; }
        public string BaoTinHoVaTen { get; set; }
        public string BaoTinDiaChiNguoiNhan { get; set; }
        public string BaoTinSoDienThoai { get; set; }
        public string BaoTinEmail { get; set; }
        public bool? LaNoiTru { get; set; }
        public string DiaChiCuThe { get; set; }
        public string File1 { get; set; }
        public string File2 { get; set; }
        public string File3 { get; set; }
        public string Mobile1 { get; set; }
        public string Email1 { get; set; }
        public string GioiTinh { get; set; }
        public string EmailNhaTruong { get; set; }
        public bool? DaXacThucEmailNhaTruong { get; set; }
    }
    public class StudentBasicUpdateModel
    {
        public string email { get; set; }
        public string mobile { get; set; }
        public string diaChiBaotin { get; set; }
        public string diaChiNguoiNhanBaotin { get; set; }
        public string mobileBaoTin { get; set; }
        public string emailBaoTin { get; set; }
        public string hoTenBaoTin { get; set; }
        public bool coNoiOCuThe { get; set; }
        public string diaChiCuThe { get; set; }
    }
    public class GioiThieuModel
    {
        public long Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public int? Status { get; set; }
        public string DonVi { get; set; }
        public string DenGap { get; set; }
        public string VeViec { get; set; }
        public DateTime? CoGiaTriDenNgay { get; set; }
        public string PhanHoi { get; set; }
        public bool? Deleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public DateTime? NgayGui { get; set; }
        public DateTime? NgayHenTuNgay { get; set; }
        public DateTime? NgayHenDenNgay { get; set; }
        public string MaXacNhan { get; set; }
    }
    public class ThueNhaModel
    {
        public long Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public int? Status { get; set; }
        public string LyDo { get; set; }
        public string PhanHoi { get; set; }
        public bool? Deleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public DateTime? NgayGui { get; set; }
        public DateTime? NgayHenTuNgay { get; set; }
        public DateTime? NgayHenDenNgay { get; set; }
        public string MaXacNhan { get; set; }
    }
    public class VayVonModel
    {
        public long Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public int? Status { get; set; }
        public string LyDo { get; set; }
        public string PhanHoi { get; set; }
        public bool? Deleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public DateTime? NgayGui { get; set; }
        public DateTime? NgayHenTuNgay { get; set; }
        public DateTime? NgayHenDenNgay { get; set; }
        public string MaXacNhan { get; set; }
        public string ThuocDien { get; set; }
        public string ThuocDoiTuong { get; set; }
    }
    public class UuDaiModel
    {
        public long Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public int? Status { get; set; }
        public string LyDo { get; set; }
        public string PhanHoi { get; set; }
        public bool? Deleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public DateTime? NgayGui { get; set; }
        public DateTime? NgayHenTuNgay { get; set; }
        public DateTime? NgayHenDenNgay { get; set; }
        public string MaXacNhan { get; set; }
        public string KyLuat { get; set; }
    }
    public class AddDichVuModel
    {
        public int type { get; set; }
        public string lyDo { get; set; }
        public string kyLuat { get; set; }
        public string thuocDien { get; set; }
        public string thuocDoiTuong { get; set; }
        public string donVi { get; set; }
        public string denGap { get; set; }
        public string veViec { get; set; }
        public string maXacNhan { get; set; }
        public string phanHoi { get; set; }
    }
    public class ThiHsgModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string CapThi { get; set; }
        public string MonThi { get; set; }
        public string DatGiai { get; set; }
        public int Count { get; set; }
    }
    public partial class QuaTrinhHocModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string ThoiGian { get; set; }
        public string TenTruong { get; set; }
        public int Count { get; set; }
    }
    public class GiaDinhModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string MoiQuanHe { get; set; }
        public string HoVaTen { get; set; }
        public string NamSinh { get; set; }
        public string QuocTich { get; set; }
        public string DanToc { get; set; }
        public string TonGiao { get; set; }
        public string NgheNghiep { get; set; }
        public string ChucVu { get; set; }
        public string NoiCongTac { get; set; }
        public string NoiOhienNay { get; set; }
        public int Count { get; set; }
    }
    public class FullStudentModel
    {
        public StudentModel Student { get; set; }
        public List<GiaDinhModel> GiaDinh { get; set; }
        public List<ThiHsgModel> ThiHSG { get; set; }
        public List<QuaTrinhHocModel> QuaTrinhHoc { get; set; }
    }
    public class CapNhatHoSoModel
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string DiaChiBaotin { get; set; }
        public string DiaChiNguoiNhanBaotin { get; set; }
        public string MobileBaoTin { get; set; }
        public string EmailBaoTin { get; set; }
        public string HoTenBaoTin { get; set; }
        public bool CoNoiOCuThe { get; set; }
        public string DiaChiCuThe { get; set; }
        public string PhuongXa { get; set; }
        public string QuanHuyen { get; set; }
        public string TinhThanhPho { get; set; }
    }
    public class TinTucModel
    {
        public int Id { get; set; }
        public int? CatId { get; set; }
        public string Title { get; set; }
        public string Avatar { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public string NewContent { get; set; }
        public string NewSource { get; set; }
        public int TotalView { get; set; }
        public bool? IsTinlq { get; set; }
        public bool? IsComment { get; set; }
        public bool? IsShared { get; set; }
        public bool? IsHome { get; set; }
        public double? Score { get; set; }
        public bool ActiveFlg { get; set; }
        public string EntryUsername { get; set; }
        public DateTime EntryDatetime { get; set; }
        public string UpdateUsername { get; set; }
        public DateTime UpdateDatetime { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDesciption { get; set; }
        public int Status { get; set; }
    }
    public class StudentAllowUpdateModel
    {
        public StudentModel Student { get; set; }
        public bool Enabled { get; set; }
    }
    #endregion

    #region api
    public static class ApiEndPoint
    {
        public static string PostLoginEduEmail = "api/User/LoginStudentEduEmail";
        public static string PostStudentBasicUpdate = "api/Student/basic-update";
        public static string GetStudentInfo = "api/Student";
        public static string PutStudentUpdate = "api/Student/update";
        public static string AddDichVu = "api/DichVu/add";
        public static string GetDichVu = "api/DichVu/type";
        public static string GetFullStudent = "api/Student/full-student";
        public static string GetAllowUpdateStudent = "api/Student/allow-update-student";
        public static string GetTinTuc = "api/News/get-news-items";
    }
    #endregion
    #region ma dich vu
    public enum DichVu
    {
        XacNhan = 1,
        GioiThieu = 2,
        CapLaiThe = 3,
        UuDaiGiaoDuc = 4,
        MuonHocBaGoc = 5,
        VayVonNganHang = 6,
        ThueNha = 7,
    }
    #endregion
}