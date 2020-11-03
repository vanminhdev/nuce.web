using System;
using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class Nuce_Common
    {
        #region Conection
        private static string m_strConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DB"];
        public static string ConnectionString
        {
            get
            {
                return m_strConnectionString;
            }
        }

        private static string m_strPoolingConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DB"];
        public static string PoolingConnectionString
        {
            get
            {
                return m_strPoolingConnectionString;
            }
        }

        /// <summary>
        /// Return a database connection
        /// </summary>
        /// <returns>System.Data.SqlClient.SqlConnection</returns>
        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection objConnection = new SqlConnection();
                try
                {
                    objConnection.ConnectionString = PoolingConnectionString;
                    objConnection.Open();
                }
                catch (Exception)
                {
                    if (objConnection.State != ConnectionState.Closed)
                        objConnection.Close();
                    //Open new connection if all the connections are being used
                    objConnection.ConnectionString = ConnectionString;
                    objConnection.Open();
                }

                return objConnection;
            }
            catch
            {
                return null;
            }
        }
        #endregion
     }
}
