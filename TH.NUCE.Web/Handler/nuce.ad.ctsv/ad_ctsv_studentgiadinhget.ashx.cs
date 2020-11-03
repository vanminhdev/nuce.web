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
    public class ad_ctsv_studentgiadinhget : CoreHandlerCommonAdminCTSV
    {
        public override void WriteData(HttpContext context)
        {
           // DataSet ds = data.dnn_NuceCommon_Khoa.getName(-1).DataSet;
            context.Response.ContentType = "text/plain";
            string search = context.Request["StudentID"].ToString();
            if (search.Equals(""))
            {
                context.Response.Write("");
            }
            else
            {
                string sql = string.Format(@"SELECT  * FROM [dbo].[AS_Academy_Student_GiaDinh]
              where StudentID={0}", search);
                DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                context.Response.Write(DataTableToJSONWithJavaScriptSerializer(dt));
            }
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
