using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_May
    {
        public static DataTable get(int MayID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_May_get", MayID).Tables[0];
        }
        public static DataTable getName(int MayID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_May_getName", MayID).Tables[0];
        }
        public static DataTable getByPhongHoc(int PhongID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_May_getByPhongHoc", PhongID).Tables[0];
        }
        public static void update(int iID, int phongID, string ma, string mac, string mota, string ghichu)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_May_update", iID,phongID, ma, mac, mota, ghichu);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_May_update_status", iID, status);
        }
        public static int insert(int phongID, string ma, string mac, string mota, string ghichu,int type,int status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_May_insert", phongID, ma, mac, mota, ghichu,type,status);
        }
        public static void updateMac(int iID, string Mac)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_May_update_mac", iID, Mac);
        }
    }
}
