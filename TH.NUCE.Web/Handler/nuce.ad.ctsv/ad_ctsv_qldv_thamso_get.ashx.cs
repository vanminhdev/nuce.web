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
    public class ad_ctsv_qldv_thamso_get : CoreHandlerCommonAdminCTSV
    {
        public override void WriteData(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string id =  context.Request["id"] ?? "";

            if (string.IsNullOrEmpty(id.Trim()))
            {
                id = "0";
            }

            string sql = $@"select * 
                            from AS_Academy_Student_SV_ThietLapThamSoDichVu
                            where DichVuID = {id};";
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
            context.Response.Write(DataTableToJSONWithJavaScriptSerializer(dt));
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
