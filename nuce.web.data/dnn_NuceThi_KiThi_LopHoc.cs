using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_KiThi_LopHoc
    {
        public static DataTable get(int KiThi_LopHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_LopHoc_get", KiThi_LopHocID).Tables[0];
        }
        public static DataTable getByKiThi(int KiThiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_LopHoc_getByKiThi", KiThiID).Tables[0];
        }
        public static DataTable getByKiThi1(int KiThiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_LopHoc_getByKiThi1", KiThiID).Tables[0];
        }
        public static DataTable getByNguoiDung(int UserId,int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_LopHoc_getByNguoiDung", UserId,Status).Tables[0];
        }
        public static DataTable getNotInByKiThi(int KiThiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_LopHoc_getNotInByKiThi", KiThiID).Tables[0];
        }
        public static void update(int iID, int KiThiID, int LopHocID, string PhongThi, string MoTa)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_update", iID, KiThiID, LopHocID, PhongThi, MoTa);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_update_status", iID, status);
        }
        public static int insert(int KiThiID, int LopHocID, string PhongThi, string MoTa)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_KiThi_LopHoc_insert", KiThiID, LopHocID, PhongThi, MoTa);
        }
        public static void delete(int iID)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_delete", iID);
        }
    }
}
