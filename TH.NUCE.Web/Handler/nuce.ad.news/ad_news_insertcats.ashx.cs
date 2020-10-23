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
    public class ad_news_insertcats : CoreHandlerCommonAdminNews
    {
        public override void WriteData(HttpContext context)
        {
            string strName = context.Request["Name"].ToString();
            string strDes = context.Request["Des"].ToString();
            string strType = context.Request["Type"].ToString();
            string strDisplay = context.Request["Display"].ToString();
            string strCount = context.Request["Count"].ToString();
            int count = 1000;
            int.TryParse(strCount, out count);
            context.Response.ContentType = "text/plain";
            string sql = string.Format(@"INSERT INTO [dbo].[AS_News_Cats]
           ([Name]
           ,[Des]
           ,[Type]
           ,[Parent]
           ,Display
           ,[Count]
           ,[Status])
     VALUES
           (N'{0}'
           ,N'{1}'
           ,{2}
           ,1
           ,N'{3}'
           ,{4}
           ,1)", strName, strDes, strType,strDisplay, count);
            int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
            context.Response.Write(iReturn.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
