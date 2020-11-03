using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_NuceCommon_PhongHoc1
    {
        public static DataTable get(int PhongHocID)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_NuceCommon_PhongHoc_get");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, PhongHocID).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getName(int PhongHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_PhongHoc_getName", PhongHocID).Tables[0];
        }
        public static DataTable getByToaNha(int ToaNhaID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_PhongHoc_getByToaNha", ToaNhaID).Tables[0];
        }
        public static void update(int iID, int toaNhaID, string ten, string ip1, string subnetmask1, string defaultgateway1, string proxy1, string ip2, string subnetmask2, string defaultgateway2, string proxy2, string mota)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_PhongHoc_update", iID,toaNhaID, ten, ip1, subnetmask1,defaultgateway1,proxy1, ip2, subnetmask2,defaultgateway2,proxy2, mota);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_PhongHoc_update_status", iID, status);
        }
        public static int insert(int toaNhaID, string ten, string ip1, string subnetmask1,string defaultgateway1,string proxy1,
           string ip2, string subnetmask2,string defaultgateway2,string proxy2,string mota )
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_PhongHoc_insert", toaNhaID, ten, ip1, subnetmask1,defaultgateway1,proxy1, ip2, subnetmask2,defaultgateway2,proxy2, mota);
        }
    }
}
