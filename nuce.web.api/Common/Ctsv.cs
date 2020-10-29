﻿using System;
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
        }

        public static Dictionary<int, DichVuStructure> DichVuDictionary = new Dictionary<int, DichVuStructure>
        {
            { 1, new DichVuStructure { ID = 1, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ xin giấy xác nhận sinh viên", TenDichVu = "dịch vụ xin giấy xác nhận sinh viên", TinNhanCode = "XAC_NHAN_THEM_MOI" } },
            { 2, new DichVuStructure { ID = 2, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ xin giấy giới thiệu", TenDichVu = "dịch vụ xin giấy giới thiệu", TinNhanCode = "GIOI_THIEU_THEM_MOI" } },
            { 3, new DichVuStructure { ID = 3, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ cấp lại thẻ sinh viên", TenDichVu = "dịch vụ xin cấp lại thẻ sinh viên", TinNhanCode = "CAP_LAI_THE_THEM_MOI" } },
            { 4, new DichVuStructure { ID = 4, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ ưu đãi trong giáo dục", TenDichVu = "dịch vụ xin xác nhận ưu đãi trong giáo dục", TinNhanCode = "UU_DAI_THEM_MOI" } },
            { 5, new DichVuStructure { ID = 5, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ mượn học bạ gốc", TenDichVu = "dịch vụ mượn học bạ gốc", TinNhanCode = "MUON_HOC_BA_GOC_THEM_MOI" } },
            { 6, new DichVuStructure { ID = 6, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ vay vốn ngân hàng chính sách - xã hội", TenDichVu = "dịch vụ vay vốn ngân hàng chính sách - xã hội", TinNhanCode = "VAY_VON_THEM_MOI" } },
            { 7, new DichVuStructure { ID = 7, TieuDeTinNhan = "Xác nhận yêu cầu dịch vụ thuê ký túc xá Pháp Vân - Tứ Hiệp", TenDichVu = "dịch vụ thuê ký túc xá Pháp Vân - Tứ Hiệp", TinNhanCode = "THUE_KTX_THEM_MOI" } }
        };
    }
    public class DichVuStructure
    {
        public int ID { get; set; }
        public string TenDichVu { get; set; }
        public string TieuDeTinNhan { get; set; }
        public string TinNhanCode { get; set; }
    }
}
