using System;
using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_NuceCommon_NamHoc1
    {
        public static DataTable get(int NamHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_NamHoc_get", NamHocID).Tables[0];
        }
        public static DataTable getByStatus(int Status)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_NuceCommon_NamHoc_get_byStatus");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, Status).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getName(int NamHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_NamHoc_getName", NamHocID).Tables[0];
        }
        public static void update(int iID, string ten, string mota, DateTime ngaybatdau, DateTime ngayketthuc)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_NamHoc_update", iID, ten, mota, ngaybatdau, ngayketthuc);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_NamHoc_update_status", iID, status);
        }
        public static int insert(string ten, string mota, DateTime ngaybatdau, DateTime ngayketthuc)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_NamHoc_insert", ten, mota, ngaybatdau, ngayketthuc);
        }
    }
}
