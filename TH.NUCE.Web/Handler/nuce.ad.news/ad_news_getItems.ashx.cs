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
    public class ad_news_getItems : CoreHandlerCommonAdminNews
    {
        public override void WriteData(HttpContext context)
        {
           // DataSet ds = data.dnn_NuceCommon_Khoa.getName(-1).DataSet;
            context.Response.ContentType = "text/plain";
            string search = context.Request["search"].ToString();
            string danhmuc = context.Request["danhmuc"].ToString();
            string sql = string.Format(@"select top 100 a.*,b.Name as DanhMuc,FORMAT(a.update_datetime, 'dd/MM/yyyy hh:mm') as NgayCapNhat  from [dbo].[AS_News_Items] a
               left join AS_News_Cats b on a.CatID=b.ID where (title like N'%{0}%' or a.description like N'%{0}%') and  a.Status<>4 and (a.CatID ={1} or {1}=-1) order by [update_datetime] desc", search,danhmuc);
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
            context.Response.Write(DataTableToJSONWithJavaScriptSerializer(dt));
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
