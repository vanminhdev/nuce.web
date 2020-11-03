using nuce.web.data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ad_ctsv_qldv_xacnhan_get : CoreHandlerCommonAdminCTSV
    {
        public override void WriteData(HttpContext context)
        {
            // DataSet ds = data.dnn_NuceCommon_Khoa.getName(-1).DataSet;
            context.Response.ContentType = "text/plain";
            //Tham số chia dữ liêu
            //Dữ liệu lấy từ ngày
            #region CountDay
            string strCountDay = "1";
            try
            {
                strCountDay = context.Request["CDay"].ToString();
            }
            catch
            {
                strCountDay = "1";
            }
            int iCDay = 1;
            int.TryParse(strCountDay, out iCDay);
            DateTime dtCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtCompare = dtCompare.AddDays(-1 * iCDay);
            #endregion
            #region Start_Page
            string strSPage = "1";
            try
            {
                strSPage = context.Request["SPage"].ToString();
            }
            catch
            {
                strSPage = "1";
            }
            int iSPage = 1;
            int.TryParse(strSPage, out iSPage);
            #endregion
            #region End_Page
            string strEPage = "1";
            try
            {
                strEPage = context.Request["EPage"].ToString();
            }
            catch
            {
                strEPage = "1";
            }
            int iEPage = 1;
            int.TryParse(strEPage, out iEPage);
            #endregion

            string search = context.Request["search"].ToString();
            string strType = "1";
            try
            {
                strType = context.Request["Type"].ToString();
            }
            catch
            {
                strType = "1";
            }
            int iType = 1;
            int.TryParse(strType, out iType);
            string sql = "";
            if (nuce.web.data.DataUtils.LoaiDichVuSinhViens.ContainsKey(iType))
            {
                sql = string.Format(@"declare @Total int; set @Total= (select count(1) as total from [dbo].[{0}] where Status>1 and Deleted=0 and LastModifiedTime>=@Param0); 
select * from (SELECT ROW_NUMBER() OVER ( ORDER BY dichvu.Status,LastModifiedTime desc ) AS RowNum,@Total as Total, FORMAT(CreatedTime, 'dd/MM/yyyy') as Ngay, 
            FORMAT(NgayHen_TuNgay, 'dd/MM/yyyy') as NgayHen_BatDau_Ngay,
            FORMAT(NgayHen_TuNgay, 'HH') as NgayHen_BatDau_Gio,
            FORMAT(NgayHen_TuNgay, 'mm') as NgayHen_BatDau_Phut,
            FORMAT(NgayHen_DenNgay, 'dd/MM/yyyy') as NgayHen_KetThuc_Ngay,
            FORMAT(NgayHen_DenNgay, 'HH') as NgayHen_KetThuc_Gio,
            FORMAT(NgayHen_DenNgay, 'mm') as NgayHen_KetThuc_Phut,
            dichvu.*
            , student.HKTT_SoNha, student.HKTT_Pho, student.HKTT_Phuong, student.HKTT_Quan, student.HKTT_Tinh
            FROM [dbo].[{0}] dichvu
            left join AS_Academy_Student student on dichvu.StudentID = student.ID
            where dichvu.Status>1 and Deleted=0 and LastModifiedTime>=@Param0 ) AS RowConstrainedResult where RowNum>={1} and RowNum<={2} order by RowNum ", nuce.web.data.DataUtils.LoaiDichVuSinhViens[iType].Param1,iSPage,iEPage);
                if (!search.Equals(""))
                {
                    sql = string.Format(@"declare @Total int; set @Total=(select count(1) as total from [dbo].[{1}] where Status>1 and Deleted=0 and LastModifiedTime>=@Param0 
and ( StudentCode like N'%{0}%' or PhanHoi like N'%{0}%' or StudentName like N'%{0}%' or ID like N'%{0}%')); 
select * from (SELECT ROW_NUMBER() OVER ( ORDER BY dichvu.Status,LastModifiedTime desc ) AS RowNum,@Total as Total,FORMAT(CreatedTime, 'dd/MM/yyyy') as Ngay,
FORMAT(NgayHen_TuNgay, 'dd/MM/yyyy') as NgayHen_BatDau_Ngay,
FORMAT(NgayHen_TuNgay, 'HH') as NgayHen_BatDau_Gio,
FORMAT(NgayHen_TuNgay, 'mm') as NgayHen_BatDau_Phut,
FORMAT(NgayHen_DenNgay, 'dd/MM/yyyy') as NgayHen_KetThuc_Ngay,
FORMAT(NgayHen_DenNgay, 'HH') as NgayHen_KetThuc_Gio,
FORMAT(NgayHen_DenNgay, 'mm') as NgayHen_KetThuc_Phut,
dichvu.*
, student.HKTT_SoNha, student.HKTT_Pho, student.HKTT_Phuong, student.HKTT_Quan, student.HKTT_Tinh
FROM [dbo].[{1}] dichvu
left join AS_Academy_Student student on dichvu.StudentID = student.ID
where dichvu.Status>1 and Deleted=0 and LastModifiedTime>=@Param0 
and ( StudentCode like N'%{0}%' or PhanHoi like N'%{0}%' or StudentName like N'%{0}%' or dichvu.ID like N'%{0}%'))  AS RowConstrainedResult where RowNum>={2} and RowNum<={3} order by RowNum ", search, nuce.web.data.DataUtils.LoaiDichVuSinhViens[iType].Param1, iSPage, iEPage);
                }
            }
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@Param0", dtCompare);
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql,sqlParams).Tables[0];
            context.Response.Write(DataTableToJSONWithJavaScriptSerializer(dt));
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
