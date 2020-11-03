using System;
using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_NuceQLPM_Log_Syn1
    {
        public static DataTable getByType(int Type,int Status)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_NuceQLPM_Log_Syn_getByType");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, Type, Status).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static void Insert(string KeyCheck, int Status, int Type, string Description)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_NuceQLPM_Log_Syn_insert");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, strSql, KeyCheck, Status, Type, Description);

                }
                catch
                {
                    return;
                }
            }

        }
    }
}
