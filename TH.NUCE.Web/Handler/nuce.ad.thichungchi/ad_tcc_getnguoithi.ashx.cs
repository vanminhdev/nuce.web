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
    public class d_tcc_getnguoithi : CoreHandlerCommonAdminThiChungChi
    {
        public override void WriteData(HttpContext context)
        {
           // DataSet ds = data.dnn_NuceCommon_Khoa.getName(-1).DataSet;
            context.Response.ContentType = "text/plain";
            string search = context.Request["search"].ToString();
            string danhmuc = context.Request["danhmuc"].ToString();
            string sql = string.Format(@"select top 100 a.*,CONVERT(VARCHAR(10), NgaySinh, 103) as NgaySinhVN,b.Ten as DanhMuc from [dbo].[NUCE_ThiChungChi_NguoiThi] a
               left join Nuce_ThiChungChi_DanhMuc b on a.DanhMucID=b.ID where (CMT like N'%{0}%' or a.Ma like N'%{0}%' or a.Ten like N'%{0}%' or a.Ho like N'%{0}%') and  a.Status<>4 and (a.DanhMucID ={1} or {1}=-1) order by [UpdatedDate] desc", search,danhmuc);
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_ThiChungChi.ConnectionString, CommandType.Text, sql).Tables[0];
            context.Response.Write(DataTableToJSONWithJavaScriptSerializer(dt));
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
