using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_NuceCommon_May1
    {
        public static DataTable get(int MayID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_May_get", MayID).Tables[0];
        }
        public static DataTable getName(int MayID)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_NuceCommon_May_getName");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, MayID).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
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
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return ;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_NuceCommon_May_update_mac");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, strSql, iID, Mac);

                }
                catch
                {
                    return;
                }
            }
        }
    }
}
