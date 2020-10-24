using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_NuceCommon_CaHoc1
    {
        public static DataTable get(int CaHocID)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_NuceCommon_CaHoc_get");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, CaHocID).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
