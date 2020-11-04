using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Common
{
    public class Ctsv
    {
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

        public enum TrangThaiYeuCau
        {
            DaGuiLenNhaTruong = 2,
            DangXuLy = 3,
            DaXuLyVaCoLichHen = 4,
            TuChoi = 5,
            HoanThanh = 6,
        }

        public static Dictionary<int, DichVuStructure> DichVuDictionary = new Dictionary<int, DichVuStructure>
        {
            { 1, new DichVuStructure { ID = 1, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ xin giấy xác nhận sinh viên", TenDichVu = "dịch vụ xin giấy xác nhận sinh viên", TinNhanCode = "XAC_NHAN", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_XAC_NHAN" } },
            { 2, new DichVuStructure { ID = 2, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ xin giấy giới thiệu", TenDichVu = "dịch vụ xin giấy giới thiệu", TinNhanCode = "GIOI_THIEU", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_GIOI_THIEU" } },
            { 3, new DichVuStructure { ID = 3, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ cấp lại thẻ sinh viên", TenDichVu = "dịch vụ xin cấp lại thẻ sinh viên", TinNhanCode = "CAP_LAI_THE", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_CAP_LAI_THE" } },
            { 4, new DichVuStructure { ID = 4, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ ưu đãi trong giáo dục", TenDichVu = "dịch vụ xin xác nhận ưu đãi trong giáo dục", TinNhanCode = "UU_DAI", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_UU_DAI" } },
            { 5, new DichVuStructure { ID = 5, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ mượn học bạ gốc", TenDichVu = "dịch vụ mượn học bạ gốc", TinNhanCode = "MUON_HOC_BA_GOC", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_MUON_HOC_BA_GOC" } },
            { 6, new DichVuStructure { ID = 6, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ vay vốn ngân hàng chính sách - xã hội", TenDichVu = "dịch vụ vay vốn ngân hàng chính sách - xã hội", TinNhanCode = "VAY_VON", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_XAC_NHAN_VAY_VON" } },
            { 7, new DichVuStructure { ID = 7, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ thuê ký túc xá Pháp Vân - Tứ Hiệp", TenDichVu = "dịch vụ thuê ký túc xá Pháp Vân - Tứ Hiệp", TinNhanCode = "THUE_KTX", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_THUE_KTX" } }
        };

        public static Dictionary<int, string> TrangThaiYeuCauDictionary = new Dictionary<int, string>
        {
            { (int)TrangThaiYeuCau.DaGuiLenNhaTruong, "Đã gửi lên nhà trường" },
            { (int)TrangThaiYeuCau.DangXuLy, "Đã tiếp nhận và đang xử lý" },
            { (int)TrangThaiYeuCau.DaXuLyVaCoLichHen, "Đã xử lý và có lịch hẹn" },
            { (int)TrangThaiYeuCau.TuChoi, "Từ chôi dịch vụ" },
            { (int)TrangThaiYeuCau.HoanThanh, "Hoàn thành" },
        };
    }
    public class DichVuStructure
    {
        public int ID { get; set; }
        public string TenDichVu { get; set; }
        public string TieuDeTinNhan { get; set; }
        public string TinNhanCode { get; set; }
        public string LogCodeSendEmail { get; set; }
    }
}
