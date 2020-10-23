using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_Nuce_ICO_Authen
    {
        public static string get(string ID)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_ICO_Authen_get");
                    return (string)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, ID);

                }
                catch
                {
                    return "-1";
                }
            }
        }

        public static void update(string ID, string Value)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_ICO_Authen_update");
                     Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, strSql, ID, Value);

                }
                catch(System.Exception ex)
                {
                  
                }
            }
        }
    }
}
