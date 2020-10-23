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
    public class ad_ctsv_studentsearch : CoreHandlerCommonAdminCTSV
    {
        public override void WriteData(HttpContext context)
        {
           // DataSet ds = data.dnn_NuceCommon_Khoa.getName(-1).DataSet;
            context.Response.ContentType = "text/plain";
            string search = context.Request["search"].ToString();
            if (search.Equals(""))
            {
                context.Response.Write("");
            }
            else
            {
                string sql = string.Format(@"SELECT top 100 ID, FulName,ClassCode,Code FROM [dbo].[AS_Academy_Student]
              where FulName like N'%{0}%' or ClassCode like N'%{0}%' or Code like N'%{0}%' or Email like N'%{0}%' or Mobile like N'%{0}%' order by FulName", search);
                DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                context.Response.Write(DataTableToJSONWithJavaScriptSerializer(dt));
            }
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
