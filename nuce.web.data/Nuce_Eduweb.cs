using System;
using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class Nuce_Eduweb
    {
        #region Conection
        private static string m_strConnectionString = System.Configuration.ConfigurationSettings.AppSettings["EDUWEB"];
        public static string ConnectionString
        {
            get
            {
                return m_strConnectionString;
            }
        }

        private static string m_strPoolingConnectionString = System.Configuration.ConfigurationSettings.AppSettings["EDUWEB"];
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
        #region Dong BO
        public static DataTable getTKB(string Prefix)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT  [MaDK]
      ,[MaCB]
      ,[Thu]
      ,[TietBD]
      ,[SoTiet]
      ,[MaPH]
      ,[BuoiHoc]
      ,[MaCoSo]
      ,[TKBTH]
      ,[TuanHoc]
      ,[tuan1]
      ,[tuan2]
      ,[ToHop]
      ,[NHHK]
      ,[keytohop]
      ,[ngaygd]
      ,[giobd]
  FROM[eduweb].[dbo].[TKB]
  Where MAPH like '%May%'
  ");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection,CommandType.Text, strSql).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getToDK(string Prefix)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT [MaDK]
      ,[MaMH]
      ,[MaNh]
      ,[MaTo]
      ,[TenMH]
      ,[SoTC]
      ,[SoLGCP]
      ,[Khgcpdk]
      ,[Malop]
      ,[Kgcpdk]
      ,[chotrung]
      ,[MaHeDT]
      ,[ngaythi]
      ,[tietbd]
      ,[sotiet]
      ,[macoso]
      ,[GhiChu]
      ,[NHHK]
      ,[ngaythidp]
      ,[cathidp]
      ,[ngaythildp]
      ,[cathildp]
      ,[dcthi]
      ,[dchoc]
      ,[dsngayhoc]
  FROM [dbo].[ToDK]
  where MADK in 
  (
  SELECT  [MaDK]
  FROM [dbo].[TKB]
  Where MAPH like '%May%')
  ");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getTongSoSVDK(string Prefix)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"Select MaDK,Count(MASV) as SoSV from [dbo].[KQDK]
  where MADK in 
  (
  SELECT  [MaDK]
  FROM [dbo].[TKB]
  Where MAPH like '%May%')
  Group by MaDK
  Order by MADK
  ");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getLichThi(string Prefix)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT [KeyThi]
      ,[MaMH]
      ,[GhepThi]
      ,[ToThi]
      ,[SoLuong]
      ,[NgayThi]
      ,[TietBD]
      ,[SoTiet]
      ,[MaPh]
      ,[TietC]
      ,[GhepPHG]
      ,[Tietbd1]
      ,[SoTiet1]
      ,[NHHK]
      ,[DotThi]
      ,[ghichult]
  FROM [eduweb].[dbo].[LichThi]
  Where MaPh like '%May%'
  ");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getCanBo(string Prefix)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT [MaCB]
      ,[TenCB]
      ,[MaBM]
      ,[CanBoTG]
      ,[EmailCB1]
      ,[EmailCB2]
      ,[TelCB1]
      ,[TelCB2]
      ,[ngaysinh]
      ,[ngsinhcb]
      ,[IsNhanVien]
  FROM [eduweb].[dbo].[CanBo]
  where MaCB in (
		SELECT distinct([MaCB])
  FROM [dbo].[TKB]
  Where MAPH like '%May%'
  )
  ");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion
        #region Dong Bo Thong Tin Chung
        // Khoa
        public static DataTable getKhoa()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT  [MaKH]
      ,[TenKhoa]
  FROM [dbo].[Khoa]
  ");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }

        // Bo Mon
        public static DataTable getBoMon()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT  [MaBM]
      ,[MaKH]
      ,[TenBM]
      ,[TruongBM]
  FROM [dbo].[BoMon]
  ");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        // Bo mon

        public static DataTable getMonHoc()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT [MaMH]
      ,[TenMH]
      ,[MaBM]
      ,[SoTC]
      ,[SoTCHP]
      ,[MucHP]
      ,[HesoHP]
      ,[Ghtcdat]
      ,[GHTCDK]
      ,[KhgDTB]
      ,[khongmg]
      ,[SoTCTL]
      ,[ts]
  FROM [dbo].[MonHoc]
  ");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        // Can bo

        // Bo mon

        public static DataTable getCanBo()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT [MaCB]
      ,[TenCB]
      ,[MaBM]
      ,[CanBoTG]
      ,[EmailCB1]
      ,[EmailCB2]
      ,[TelCB1]
      ,[TelCB2]
      ,[ngaysinh]
      ,[ngsinhcb]
      ,[IsNhanVien]
  FROM [dbo].[CanBo]
  ");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
