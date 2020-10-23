using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_BoCauHoi
    {
        public static DataTable get(int BoCauHoiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoCauHoi_get", BoCauHoiID).Tables[0];
        }
        public static DataTable getByMaAndUser(int UserID, string Ma)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoCauHoi_getByMaAndUser", UserID,Ma).Tables[0];
        }
        public static DataTable getByNguoiDung_MonHoc(int NguoiDung_MonHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoCauHoi_getByNguoiDung_MonHoc", NguoiDung_MonHocID).Tables[0];
        }
        public static DataTable getNameByNguoiDung_MonHoc(int NguoiDung_MonHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoCauHoi_getNameByNguoiDung_MonHoc", NguoiDung_MonHocID).Tables[0];
        }
        public static void update(int iID, int NguoiDung_MonHocID, string txtMa, string txtTen,int Type,int ThangDiemID)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_BoCauHoi_update", iID, NguoiDung_MonHocID, txtMa, txtTen, Type,ThangDiemID);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_BoCauHoi_update_status", iID, status);
        }
        public static int insert(int NguoiDung_MonHocID, string txtMa, string txtTen,int Type,int ThangDiemID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_BoCauHoi_insert", NguoiDung_MonHocID, txtMa, txtTen, Type, ThangDiemID);
        }
    }
}
