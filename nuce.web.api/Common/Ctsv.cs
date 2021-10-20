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
            VeBus = 8,
            DangKyChoO = 9,
            XinMienGiamHocPhi = 10,
            DeNghiHoTroChiPhiHocTap = 11,
            HoTroHocTap = 12
        }

        public enum TrangThaiYeuCau
        {
            DaGuiLenNhaTruong = 2,
            DangXuLy = 3,
            DaXuLyVaCoLichHen = 4,
            TuChoi = 5,
            HoanThanh = 6,
        }

        public enum DichVuXeBusLoaiTuyen
        {
            MotTuyen = 1,
            LienTuyen = 2
        }

        public class NhuCauNhaO
        {
            public const string KTX = "KTX";
            public const string PHAP_VAN = "PHAP_VAN";
            public static readonly List<string> All = new List<string> { KTX, PHAP_VAN };
        }

        public class DoiTuongUuTienNhaO
        {
            public const string NHOM_1 = "NHOM_1";
            public const string NHOM_2 = "NHOM_2";
            public static readonly List<string> All = new List<string> { NHOM_1, NHOM_2 };
        }

        public class DoiTuongXinMienGiamHocPhi
        {
            public const string CO_CONG_CACH_MANG = "CO_CONG_CACH_MANG";
            public const string SV_VAN_BANG_1 = "SV_VAN_BANG_1";
            public const string TAN_TAT_KHO_KHAN_KINH_TE = "TAN_TAT_KHO_KHAN_KINH_TE";
            public const string DAN_TOC_HO_NGHEO = "DAN_TOC_HO_NGHEO";
            public const string DAN_TOC_IT_NGUOI_VUNG_KHO_KHAN = "DAN_TOC_IT_NGUOI_VUNG_KHO_KHAN";
            public const string DAN_TOC_VUNG_KHO_KHAN = "DAN_TOC_VUNG_KHO_KHAN";
            public const string CHA_ME_TAI_NAN_DUOC_TRO_CAP = "CHA_ME_TAI_NAN_DUOC_TRO_CAP";
            public static readonly List<string> All = new List<string> { CO_CONG_CACH_MANG, SV_VAN_BANG_1, TAN_TAT_KHO_KHAN_KINH_TE,
                DAN_TOC_HO_NGHEO, DAN_TOC_IT_NGUOI_VUNG_KHO_KHAN, DAN_TOC_VUNG_KHO_KHAN, CHA_ME_TAI_NAN_DUOC_TRO_CAP };
        }

        public class DoiTuongDeNghiHoTroChiPhi
        {
            public const string DAN_TOC_HO_NGHEO = "DAN_TOC_HO_NGHEO";
            public const string DAN_TOC_HO_CAN_NGHEO = "DAN_TOC_HO_CAN_NGHEO";
            public static readonly List<string> All = new List<string> { DAN_TOC_HO_NGHEO, DAN_TOC_HO_CAN_NGHEO };
        }

        public static Dictionary<int, DichVuStructure> DichVuDictionary = new Dictionary<int, DichVuStructure>
        {
            { 1, new DichVuStructure { ID = 1, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ xin giấy xác nhận sinh viên", TenDichVu = "dịch vụ xin giấy xác nhận sinh viên", TinNhanCode = "XAC_NHAN", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_XAC_NHAN" } },
            { 2, new DichVuStructure { ID = 2, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ xin giấy giới thiệu", TenDichVu = "dịch vụ xin giấy giới thiệu", TinNhanCode = "GIOI_THIEU", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_GIOI_THIEU" } },
            { 3, new DichVuStructure { ID = 3, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ cấp lại thẻ sinh viên", TenDichVu = "dịch vụ xin cấp lại thẻ sinh viên", TinNhanCode = "CAP_LAI_THE", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_CAP_LAI_THE" } },
            { 4, new DichVuStructure { ID = 4, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ ưu đãi trong giáo dục", TenDichVu = "dịch vụ xin xác nhận ưu đãi trong giáo dục", TinNhanCode = "UU_DAI", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_UU_DAI" } },
            { 5, new DichVuStructure { ID = 5, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ mượn học bạ gốc", TenDichVu = "dịch vụ mượn học bạ gốc", TinNhanCode = "MUON_HOC_BA_GOC", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_MUON_HOC_BA_GOC" } },
            { 6, new DichVuStructure { ID = 6, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ vay vốn ngân hàng chính sách - xã hội", TenDichVu = "dịch vụ vay vốn ngân hàng chính sách - xã hội", TinNhanCode = "VAY_VON", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_XAC_NHAN_VAY_VON" } },
            { 7, new DichVuStructure { ID = 7, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ thuê ký túc xá Pháp Vân - Tứ Hiệp", TenDichVu = "dịch vụ thuê ký túc xá Pháp Vân - Tứ Hiệp", TinNhanCode = "THUE_KTX", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_THUE_KTX" } },
            { 8, new DichVuStructure { ID = 8, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ làm vé tháng xe bus", TenDichVu = "dịch vụ làm vé tháng xe bus", TinNhanCode = "VE_XE_BUS", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_VE_XE_BUS" } },
            { 9, new DichVuStructure { ID = 9, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ đăng ký chỗ ở", TenDichVu = "dịch vụ đăng ký chỗ ở", TinNhanCode = "DANG_KY_CHO_O", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_DANG_KY_CHO_O" } },
            { 10, new DichVuStructure { ID = 10, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ xin miễn giảm học phí", TenDichVu = "xin miễn giảm học phí", TinNhanCode = "XIN_MIEN_GIAM_HOC_PHI", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_XIN_MIEN_GIAM_HOC_PHI" } },
            { 11, new DichVuStructure { ID = 11, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ đề nghị hỗ trợ chi phí học tập", TenDichVu = "đề nghị hỗ trợ chi phí học tập", TinNhanCode = "DE_NGHI_HO_TRO_CHI_PHI_HOC_TAP", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_DE_NGHI_HO_TRO_CHI_PHI_HOC_TAP" } },
            { 12, new DichVuStructure { ID = 12, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ đề nghị hỗ trợ học tập", TenDichVu = "đề nghị hỗ trợ học tập", TinNhanCode = "DE_NGHI_HO_TRO_HOC_TAP", LogCodeSendEmail = "GUI_MAIL_XAC_NHAN_DE_NGHI_HO_TRO_HOC_TAP" } }
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
