using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace nuce.web.data
{
    public static class DataUtils
    {
        #region LoaiDichVuSinhVien
        public static Dictionary<int, Nuce.CTSV.Model.LoaiDichVu> getLoaiDichVuSinhVien()
        {
            Dictionary<int, Nuce.CTSV.Model.LoaiDichVu> temp = new Dictionary<int, Nuce.CTSV.Model.LoaiDichVu>();
            temp.Add(1, new Nuce.CTSV.Model.LoaiDichVu(1, "", "Dịch vụ xin giấy xác nhận", "", "AS_Academy_Student_SV_XacNhan", "/tabid/41/default.aspx", ""));
            temp.Add(2, new Nuce.CTSV.Model.LoaiDichVu(2, "", "Dịch vụ xin giấy giới thiệu", "", "AS_Academy_Student_SV_GioiThieu", "/tabid/42/default.aspx", ""));
            temp.Add(3, new Nuce.CTSV.Model.LoaiDichVu(3, "", "Dịch vụ xin cấp lại thẻ sinh viên", "", "AS_Academy_Student_SV_CapLaiTheSinhVien", "/tabid/43/default.aspx", ""));
            temp.Add(4, new Nuce.CTSV.Model.LoaiDichVu(4, "", "Dịch vụ xin giấy xác nhận ưu đãi trong giáo dục", "", "AS_Academy_Student_SV_XacNhanUuDaiTrongGiaoDuc", "/tabid/44/default.aspx", ""));
            temp.Add(5, new Nuce.CTSV.Model.LoaiDichVu(5, "", "Dịch vụ mượn học bạ gốc", "", "AS_Academy_Student_SV_MuonHocBaGoc", "/tabid/45/default.aspx", ""));
            temp.Add(6, new Nuce.CTSV.Model.LoaiDichVu(6, "", "Dịch vụ vay vốn ngân hàng", "", "AS_Academy_Student_SV_VayVonNganHang", "/tabid/48/default.aspx", ""));
            temp.Add(7, new Nuce.CTSV.Model.LoaiDichVu(7, "", "Dịch vụ thuê nhà ở sinh viên Pháp Vân - Tứ Hiệp", "", "AS_Academy_Student_SV_ThueNha", "/tabid/47/default.aspx", ""));
            return temp;
        }
        public static Dictionary<int, Nuce.CTSV.Model.LoaiDichVu> LoaiDichVuSinhViens = getLoaiDichVuSinhVien();

        public static Dictionary<int, Nuce.CTSV.Model.TrangThaiDichVu> getTrangThaiDichVuSinhVien()
        {
            Dictionary<int, Nuce.CTSV.Model.TrangThaiDichVu> temp = new Dictionary<int, Nuce.CTSV.Model.TrangThaiDichVu>();
            temp.Add(1, new Nuce.CTSV.Model.TrangThaiDichVu(1, "", "Đang chờ xác nhận", "", "", "", ""));
            temp.Add(2, new Nuce.CTSV.Model.TrangThaiDichVu(2, "", "Đã gửi lên nhà trường", "", "", "", ""));
            temp.Add(3, new Nuce.CTSV.Model.TrangThaiDichVu(3, "", "Đã tiếp nhận và đang xử lý", "", "", "", ""));
            temp.Add(4, new Nuce.CTSV.Model.TrangThaiDichVu(4, "", "Đã xử lý và có lịch hẹn", "", "", "", ""));
            temp.Add(5, new Nuce.CTSV.Model.TrangThaiDichVu(5, "", "Từ chôi dịch vụ", "", "", "", ""));
            temp.Add(6, new Nuce.CTSV.Model.TrangThaiDichVu(6, "", "Hoàn thành", "", "", "", ""));
            return temp;
        }
        public static Dictionary<int, Nuce.CTSV.Model.TrangThaiDichVu> TrangThaiDichVuSinhViens = getTrangThaiDichVuSinhVien();
        #endregion
    }
}
