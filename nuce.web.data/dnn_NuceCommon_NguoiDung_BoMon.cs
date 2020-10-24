using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_NguoiDung_BoMon
    {
        public static DataTable get(int BoMonID,int RoleID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_NguoiDung_BoMon_get", BoMonID, RoleID).Tables[0];
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_NguoiDung_BoMon_update_status", iID, status);
        }
        public static int insert(int BoMonID, int UserID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_NguoiDung_BoMon_insert", BoMonID, UserID);
        }
    }
}
