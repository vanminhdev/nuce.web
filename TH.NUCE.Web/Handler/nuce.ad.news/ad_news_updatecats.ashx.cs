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
    public class ad_news_updatecats : CoreHandlerCommonAdminNews
    {
        public override void WriteData(HttpContext context)
        {
            string strID = context.Request["ID"].ToString();
            string strName = context.Request["Name"].ToString();
            string strDes = context.Request["Des"].ToString();
            string strType = context.Request["Type"].ToString();
            string strCount = context.Request["Count"].ToString();
            int count = 1000;
            int.TryParse(strCount, out count);
            string strDisplay = context.Request["Display"].ToString();
            context.Response.ContentType = "text/plain";
            string sql = string.Format(@"UPDATE [dbo].[AS_News_Cats]
   SET [Name] = N'{0}'
      ,[Des] =N'{1}'
      ,[Type] ={2}
      ,[Count] ={3}
     ,[Display] =N'{4}'
 WHERE ID={5}", strName, strDes, strType, count, strDisplay, strID);
            int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
            context.Response.Write(iReturn.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
