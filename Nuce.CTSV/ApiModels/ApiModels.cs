using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nuce.CTSV.ApiModels
{
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
}