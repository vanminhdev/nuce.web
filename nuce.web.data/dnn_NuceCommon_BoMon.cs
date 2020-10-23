using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_BoMon
    {
        public static DataTable get(int BoMonID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_BoMon_get", BoMonID).Tables[0];
        }
        public static DataTable getByKhoa(int KhoaID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_BoMon_getByKhoa", KhoaID).Tables[0];
        }
        public static DataTable getNameByKhoa(int KhoaID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_BoMon_getNameByKhoa", KhoaID).Tables[0];
        }
        public static void update(int iID,int KhoaID,string txtMa,string txtTen,string txtTenTiengAnh,string txtDiaChi,string txtMoTa)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_BoMon_update", iID, KhoaID, txtMa, txtTen, txtTenTiengAnh, txtDiaChi, txtMoTa);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_BoMon_update_status", iID, status);
        }
        public static int insert(int KhoaID, string txtMa, string txtTen, string txtTenTiengAnh, string txtDiaChi, string txtMoTa)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_BoMon_insert", KhoaID, txtMa, txtTen, txtTenTiengAnh, txtDiaChi, txtMoTa);
        }
        public static int insert(int BoMonID,int KhoaID, string txtMa,string txtMaKhoa, string txtTen, string txtTenTiengAnh, string txtDiaChi, string txtMoTa)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_BoMon_insertSyn", BoMonID, KhoaID, txtMa, txtMaKhoa, txtTen, txtTenTiengAnh, txtDiaChi, txtMoTa);
        }
    }
}
