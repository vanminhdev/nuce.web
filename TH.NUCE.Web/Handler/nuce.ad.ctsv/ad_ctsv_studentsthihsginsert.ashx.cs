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
    public class ad_ctsv_studentsthihsginsert : CoreHandlerCommonAdminCTSV
    {
        public override void WriteData(HttpContext context)
        {
            string strID = context.Request["StudentID"].ToString();
            string strCode = context.Request["StudentCode"].ToString();
            string strCapThi = context.Request["CapThi"].ToString();
            string strMonThi= context.Request["MonThi"].ToString();
            string strDatGiai = context.Request["DatGiai"].ToString();
            string strCount = context.Request["Count"].ToString();
            int count = 1000;
            int.TryParse(strCount, out count);
            context.Response.ContentType = "text/plain";
            string sql = string.Format(@"INSERT INTO [dbo].[AS_Academy_Student_ThiHSG]
           ([StudentID]
           ,[StudentCode]
           ,[CapThi]
           ,[MonThi]
           ,[DatGiai]
           ,[Count]) VALUES( {0},N'{1}' ,N'{2}'
      , N'{3}'
      ,N'{4}'
      , {5})", strID,strCode,strCapThi,strMonThi,strDatGiai,count);
            int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
            context.Response.Write(iReturn.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
