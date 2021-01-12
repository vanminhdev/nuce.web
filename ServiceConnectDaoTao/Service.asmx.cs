using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace ServiceConnectDaoTao
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {

        [WebMethod]
        public string Test()
        {
            return "Hello World";
        }
        [WebMethod]
        public DataTable getAllTTSV1()
        {
            string strSql = string.Format(@"select sinhvien.HoTenSV,sinhvien.NgaySinh,sinhvien.phai,hssv.socmnd,hssv.EMAIL1,hssv.DIENTHTT,NTDQG.TenQGVN
,THANHPHO.TENTPVN,QUANHUYEN.TENQHVN,ntdpx.tenpxvn,hssv.DCThonXom,hssv.MASV
from sinhvien,HSSV,NTDQG,QUANHUYEN,THANHPHO,ntdpx
where sinhvien.MaSV=hssv.MASV 
and NTDQG.MaQG=hssv.QuocGia and QUANHUYEN.MAQH=hssv.HKHUYEN and THANHPHO.MATP=HSSV.HKTINH
and ntdpx.mapx=HSSV.dcgd1");

            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataTKB";
            return dataTable;
        }
        [WebMethod]
        public DataTable getAllTTSV2_HoKhauTamTru()
        {
            string strSql = string.Format(@"select DCTT1,QUANHUYEN.TENQHVN,THANHPHO.TENTPVN,MASV from HSSV,THANHPHO,QUANHUYEN
where  HSSV.TTHUYEN= QUANHUYEN.MAQH and THANHPHO.MATP=HSSV.DCTT3");

            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataTKB";
            return dataTable;
        }
        [WebMethod]
        public DataTable getAllTKB1JoinToDk1()
        {
            string strSql = string.Format(@"select a.MaDK as MaDK,a.MaCB as MaCB,a.Thu as Thu,a.TietBD as TietDB,
a.SoTiet as SoTiet,a.MaPH as MaPH,a.TuanHoc as TuanHoc,b.MaMH as MaMH from tkb1 a inner join todk1 b
on a.MaDK=b.MaDK");

            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataTKB";
            return dataTable;
        }
        [WebMethod]
        public DataTable getAllTKB1JoinToDk()
        {
            string strSql = string.Format(@"select a.MaDK as MaDK,a.MaCB as MaCB,a.Thu as Thu,a.TietBD as TietDB,
a.SoTiet as SoTiet,a.MaPH as MaPH,a.TuanHoc as TuanHoc,b.MaMH as MaMH from tkb a inner join todk b
on a.MaDK=b.MaDK");

            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataTKB";
            return dataTable;
        }
        [WebMethod]
        public DataTable getTKB(string Prefix)
        {
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
  FROM [dbo].[TKB]
  Where MAPH like '%May%'
  ");

            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataTKB";
            return dataTable;
        }
        [WebMethod]
        public DataTable getTKB1(string Prefix)
        {
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
  FROM [dbo].[TKB]
  Where MAPH like '%{0}%'
  ",Prefix);

            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataTKB";
            return dataTable;
        }

        [WebMethod]
        public DataTable getTKB_bangkytruoc(string Prefix)
        {
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
  FROM [dbo].[TKB1]
  Where MAPH like '%May%'
  ");

            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataTKB";
            return dataTable;
        }
        [WebMethod]
        public DataTable getTKB1_bangkytruoc(string Prefix)
        {
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
  FROM [dbo].[TKB1]
  Where MAPH like '%{0}%'
  ", Prefix);

            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataTKB";
            return dataTable;
        }

        [WebMethod]
        public DataTable getToDK(string Prefix)
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
  Where MAPH like '%{0}%')
  ", Prefix);
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataToDangKy";
            return dataTable;
        }
        [WebMethod]
        public DataTable getToDK_bangkytruoc(string Prefix)
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
  FROM [dbo].[ToDK1]
  where MADK in 
  (
  SELECT  [MaDK]
  FROM [dbo].[TKB1]
  Where MAPH like '%{0}%')
  ", Prefix);
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataToDangKy";
            return dataTable;
        }
        [WebMethod]
        public DataTable getAllToDKKyHienTai()
        {
            //Execute select command
            string strSql = string.Format(@"select * from [dbo].[ToDK]");
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataToDangKy";
            return dataTable;
        }
        [WebMethod]
        public DataTable getAllToDKKyTruoc()
        {
            //Execute select command
            string strSql = string.Format(@"select * from [dbo].[todk1]");
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataToDangKy";
            return dataTable;
        }
        [WebMethod]
        public int countToDK1()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select count(1) from [dbo].[todk1]
  ");
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        [WebMethod]
        public DataTable getAllToDK()
        {
            //Execute select command
            string strSql = string.Format(@"select * from [dbo].[ToDK]");
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataToDangKy";
            return dataTable;
        }
        
        //private class KQDK
        //{
        //    public string MaSV { get; set; }
        //    public string MaMH { get; set; }
        //    public string MaDK { get; set; }
        //}

        [WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public DataTable getKQDKKyHienTai(int skip, int take)
        {
            //Execute select command
            DataTable dataTable = new DataTable();
            dataTable.TableName = "dataKQDK";
            //string strSql = string.Format(@"select * from (SELECT ROW_NUMBER() OVER ( ORDER BY [MaDK],MaSV) AS RowNum,[MaSV],[MaMH],[MaDK] FROM  [dbo].[KQDK1]) a where RowNum>={0} and RowNum<={1}", from, to);

            string strSql = $"select [MaSV],[MaMH],[MaDK] from [dbo].[KQDK] order by [dbo].[KQDK].Guid offset {skip} rows fetch next {take} rows only";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            cmd.CommandTimeout = 0;
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //var result = new List<object>();
            //var arr = new object[take];
            //using (SqlDataReader oReader = cmd.ExecuteReader())
            //{
            //    int i = 0;
            //    while (oReader.Read())
            //    {
            //        //arr[i++] = new
            //        //{
            //        //    MaSV = oReader["MaSV"].ToString(),
            //        //    MaMH = oReader["MaMH"].ToString(),
            //        //    MaDK = oReader["MaDK"].ToString()
            //        //};
            //        result.Add(new
            //        {
            //            MaSV = oReader["MaSV"].ToString(),
            //            MaMH = oReader["MaMH"].ToString(),
            //            MaDK = oReader["MaDK"].ToString()
            //        });
            //    }
            //}

            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            da.Dispose();
            conn.Close();

            //JavaScriptSerializer js = new JavaScriptSerializer();
            //Context.Response.Write(js.Serialize(result));
            //Context.Response.Write(js.Serialize(arr));

            return dataTable;
        }
        [WebMethod]
        public DataTable getTongSoSVDK(string Prefix)
        {
            //Execute select command
            string strSql = string.Format(@"Select MaDK,Count(MASV) as SoSV from [dbo].[KQDK]
  where MADK in 
  (
  SELECT  [MaDK]
  FROM [dbo].[TKB]
  Where MAPH like '%{0}%')
  Group by MaDK
  Order by MADK
  ",Prefix);
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataTongSoSVDK";
            return dataTable;

        }
        [WebMethod]
        public DataTable getTongSoSVDK_bangkytruoc(string Prefix)
        {
            //Execute select command
            string strSql = string.Format(@"Select MaDK,Count(MASV) as SoSV from [dbo].[KQDK1]
  where MADK in 
  (
  SELECT  [MaDK]
  FROM [dbo].[TKB1]
  Where MAPH like '%{0}%')
  Group by MaDK
  Order by MADK
  ", Prefix);
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataTongSoSVDK";
            return dataTable;

        }
        [WebMethod]
        public DataTable getLichThi(string Prefix)
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
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataLichThi";
            return dataTable;
        }
        [WebMethod]
        public DataTable getCanBo(string Prefix)
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
  Where MAPH like '%{0}%'
  )
  ", Prefix);
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataCanBo";
            return dataTable;
        }
        [WebMethod]
        public DataTable getAllCanBo()
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
  FROM [dbo].[CanBo]");
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataCanBo";
            return dataTable;
        }
        [WebMethod]
        public int countCanBo()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select count(1) from [dbo].[CanBo]
  ");
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        [WebMethod]
        public DataTable getKhoa()
        {
            //Execute select command
            string strSql = string.Format(@"SELECT  [MaKH]
      ,[TenKhoa]
  FROM [dbo].[Khoa]
  ");
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataKhoa";
            return dataTable;

        }
        [WebMethod]
        public int countKhoa()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select count(1) from [dbo].[Khoa]
  ");
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        // Bo Mon
        [WebMethod]
        public DataTable getBoMon()
        {
            //Execute select command
            string strSql = string.Format(@"SELECT  [MaBM]
      ,[MaKH]
      ,[TenBM]
      ,[TruongBM]
  FROM [dbo].[BoMon]
  ");
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            dataTable.TableName = "dataBoMon";
            return dataTable;
        }
        [WebMethod]
        public int countBoMon()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select count(1) from [dbo].[BoMon]
  ");
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        [WebMethod]
        public DataTable getMonHoc()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
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
     ,[TinhChatMH]
  FROM [dbo].[MonHoc]
  ");
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(strConnectionString, CommandType.Text, strSql).Tables[0];
        }
        [WebMethod]
        public int countMonHoc()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select count(1) from [dbo].[MonHoc]
  ");
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        [WebMethod]
        public DataTable getNganh()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select * from [dbo].[Nganh]
  ");
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(strConnectionString, CommandType.Text, strSql).Tables[0];
        }
        [WebMethod]
        public int countNganh()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select count(1) from [dbo].[Nganh]
  ");
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        [WebMethod]
        public DataTable getLop()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select * from [dbo].[Lop]
  ");
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(strConnectionString, CommandType.Text, strSql).Tables[0];
        }
        [WebMethod]
        public int countLop()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select count(1) from [dbo].[Lop]
  ");
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        [WebMethod]
        public DataTable getSinhVien()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select * from [dbo].[SinhVien]
  ");
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(strConnectionString, CommandType.Text, strSql).Tables[0];
        }
        [WebMethod]
        public int countSinhVien()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select count(1) from [dbo].[SinhVien]
  ");
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        [WebMethod]
        public DataTable getKQDKKyTruoc(int skip, int take)
        {
            //Execute select command
            DataTable dataTable = new DataTable();
            dataTable.TableName = "dataKQDK";
            string strSql = $"select [MaSV],[MaMH],[MaDK] from [dbo].[KQDK1] order by [dbo].[KQDK1].Guid offset {skip} rows fetch next {take} rows only";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(strSql, conn);
            cmd.CommandTimeout = 0;
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            da.Dispose();
            conn.Close();
            return dataTable;
        }
        [WebMethod]
        public int countKqdk1()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select count(1) from [dbo].kqdk1");
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        [WebMethod]
        public int countKqdk1ExistSV()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select count(1) from [dbo].[kqdk1] where MaSV in (select MaSV from [dbo].[SinhVien])");
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        [WebMethod]
        public DataTable getTkb1()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select * from [dbo].[tkb1]");
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(strConnectionString, CommandType.Text, strSql).Tables[0];
        }
        [WebMethod]
        public int countTkb1()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"select count(1) from [dbo].[tkb1]");
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        [WebMethod]
        public int authen(string masv, string pass)
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"SELECT count(MaND) 
  FROM [dbo].[NguoiDung] where MaND='{0}' and (MatKhau='{1}'
  or (exists (select  MaND from NguoiDungOnline where  MaND='{0}') and '{1}'='23fbf23c921b754adcb2fcac8e4b19b8a7c740'))", masv, PasswordUtil.HashPassword(pass));
            return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(strConnectionString, CommandType.Text, strSql);
        }
        [WebMethod]
        public DataTable getMatKhauNguoiDung(string danhsachma)
        {
            //'0115318','0115517'
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;
            //Execute select command
            string strSql = string.Format(@"SELECT  MaND
      ,MatKhau
  FROM [dbo].[NguoiDung]
  where MaND in ({0})", danhsachma);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(strConnectionString, CommandType.Text, strSql).Tables[0];
        }
    }
}
