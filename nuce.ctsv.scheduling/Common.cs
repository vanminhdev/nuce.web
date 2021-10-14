using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace nuce.ctsv.scheduling
{
    public static class common
    {
        #region Conection
        private static string m_strConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DBLocal"];
        public static string ConnectionString
        {
            get
            {
                return m_strConnectionString;
            }
        }

        private static string m_strPoolingConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DBLocal"];
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

        #region Config Mail
        public static string MailServer = System.Configuration.ConfigurationSettings.AppSettings["Mail:Server"];
        public static string MailUser = System.Configuration.ConfigurationSettings.AppSettings["Mail:User"];
        public static string MailPassword = System.Configuration.ConfigurationSettings.AppSettings["Mail:Password"];
        public static int MailPort = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["Mail:Port"] ?? "25");
        public static string MailFromAddress = System.Configuration.ConfigurationSettings.AppSettings["Mail:From:Address"];
        public static string MailFromName = System.Configuration.ConfigurationSettings.AppSettings["Mail:From:Name"];
        #endregion

        #region Logs
        public static void WriteLogNotFile(string description)
        {
            string strMessage = string.Format("{0:HH:mm:ss}\t{1}", DateTime.Now, description);
            Console.WriteLine(strMessage);
        }
        public static void WriteLog(string description)
        {
            System.IO.StreamWriter objStreamWriter = OpenLogFile();
            string strMessage = string.Format("{0:HH:mm:ss}\t{1}", DateTime.Now, description);
            objStreamWriter.WriteLine(strMessage);
            objStreamWriter.Flush();
            objStreamWriter.Close();
            objStreamWriter.Dispose();
            Console.WriteLine(strMessage);
        }
        public static string LogFolder = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).DirectoryName + "\\Logs";
        public static void ClearLogFiles()
        {
            if (!System.IO.Directory.Exists(LogFolder))
                return;
        }
        public static System.IO.StreamWriter OpenLogFile()
        {
            try
            {
                if (!System.IO.Directory.Exists(LogFolder))
                    System.IO.Directory.CreateDirectory(LogFolder);
                string strLogFile = string.Format("{0}\\{1:yyyyMMdd-HH}.log", LogFolder, DateTime.Now);

                if (!System.IO.File.Exists(strLogFile))
                {
                    System.IO.StreamWriter objStreamWriter = System.IO.File.CreateText(strLogFile);
                    objStreamWriter.WriteLine("Time\tDescription");
                    objStreamWriter.Flush();
                    objStreamWriter.Close();
                    objStreamWriter.Dispose();
                }

                return System.IO.File.AppendText(strLogFile);
            }
            catch
            {
                Thread.Sleep(10);
                return OpenLogFile();
            }
        }
        #endregion
    }
}
