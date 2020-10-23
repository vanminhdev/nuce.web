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
    public class ad_ctsv_studentsquatrinhhoctapupdate : CoreHandlerCommonAdminCTSV
    {
        public override void WriteData(HttpContext context)
        {
            string strID = context.Request["ID"].ToString();
            string strThoiGian = context.Request["ThoiGian"].ToString();
            string strTenTruong = context.Request["TenTruong"].ToString();
            string strCount = context.Request["Count"].ToString();
            int count = 1000;
            int.TryParse(strCount, out count);
            context.Response.ContentType = "text/plain";
            string sql = string.Format(@"UPDATE [dbo].[AS_Academy_Student_QuaTrinhHocTap]
   SET  [ThoiGian] = N'{0}'
      ,[TenTruong] = N'{1}'
      ,count={2}
 WHERE ID={3}", strThoiGian,strTenTruong,count,strID);
            int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
            context.Response.Write(iReturn.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
