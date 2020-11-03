using nuce.web.data;
using System.Data;
using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ad_ctsv_studentgiadinhinsert : CoreHandlerCommonAdminCTSV
    {
        public override void WriteData(HttpContext context)
        {
            string strID = context.Request["StudentID"].ToString();
            string strCode = context.Request["StudentCode"].ToString();
            string strMoiQuanHe = context.Request["MoiQuanHe"].ToString();
            string strHoVaTen = context.Request["HoVaTen"].ToString();
            string strNamSinh = context.Request["NamSinh"].ToString();
            string strQuocTich = context.Request["QuocTich"].ToString();
            string strDanToc = context.Request["DanToc"].ToString();
            string strTonGiao = context.Request["TonGiao"].ToString();
            string strNgheNghiep = context.Request["NgheNghiep"].ToString();
            string strChucVu = context.Request["ChucVu"].ToString();
            string strNoiCongTac = context.Request["NoiCongTac"].ToString();
            string strNoiOHienTai = context.Request["NoiOHienTai"].ToString();
            
            string strCount = context.Request["Count"].ToString();
            int count = 1000;
            int.TryParse(strCount, out count);
            context.Response.ContentType = "text/plain";
            string sql = string.Format(@"INSERT INTO [dbo].[AS_Academy_Student_GiaDinh]
           ([StudentID]
           ,[StudentCode]
           ,[MoiQuanHe]
           ,[HoVaTen]
           ,[NamSinh]
           ,[QuocTich]
           ,[DanToc]
           ,[TonGiao]
           ,[NgheNghiep]
           ,[ChucVu]
           ,[NoiCongTac]
           ,[NoiOHienNay]
           ,[Count]) VALUES( {11},N'{12}' ,N'{0}'
      , N'{1}'
      , N'{2}'
      , N'{3}'
      , N'{4}'
      , N'{5}'
      , N'{6}'
      , N'{7}'
      , N'{8}'
      , N'{9}'
      , {10})", strMoiQuanHe, strHoVaTen, strNamSinh, strQuocTich, strDanToc, strTonGiao,strNgheNghiep,strChucVu,strNoiCongTac,strNoiOHienTai,count,strID,strCode);
            int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
            context.Response.Write(iReturn.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
