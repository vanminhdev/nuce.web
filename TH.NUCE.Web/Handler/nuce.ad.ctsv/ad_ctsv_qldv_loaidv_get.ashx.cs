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
    public class ad_ctsv_qldv_loaidv_get : CoreHandlerCommonAdminCTSV
    {
        public override void WriteData(HttpContext context)
        {
            // DataSet ds = data.dnn_NuceCommon_Khoa.getName(-1).DataSet;
            context.Response.ContentType = "text/plain";

            string sql = $@"SELECT * FROM dbo.AS_Academy_Student_SV_LoaiDichVu AS AASSLDV";

            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
            context.Response.Write(DataTableToJSONWithJavaScriptSerializer(dt));
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
