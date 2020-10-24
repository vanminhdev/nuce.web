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
    public class ad_tcc_insertkhoathi : CoreHandlerCommonAdminThiChungChi
    {
        public override void WriteData(HttpContext context)
        {
            string strMa = context.Request["MonThi"].ToString();
            string strTen = context.Request["Ten"].ToString();
            string strMoTa = context.Request["MoTa"].ToString();
            context.Response.ContentType = "text/plain";
            string sql = string.Format(@"INSERT INTO [dbo].[Nuce_ThiChungChi_KhoaThi]
           ([MonThiID]
           ,[Ten]
           ,[MoTa]
           ,[Type]
           ,[Status])
     VALUES
           ('{0}'
           ,'{1}'
           ,'{2}'
           ,1
           ,1)", strMa,strTen,strMoTa);
            int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_ThiChungChi.ConnectionString, CommandType.Text, sql);
            context.Response.Write(iReturn.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
