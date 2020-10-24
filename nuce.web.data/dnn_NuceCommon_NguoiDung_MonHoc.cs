using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_NguoiDung_MonHoc
    {
        public static DataTable get(int MonHocID,int RoleID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_NguoiDung_MonHoc_get", MonHocID, RoleID).Tables[0];
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_NguoiDung_MonHoc_update_status", iID, status);
        }
        public static int insert(int MonHocID, int UserID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_NguoiDung_MonHoc_insert", MonHocID, UserID);
        }
    }
}
