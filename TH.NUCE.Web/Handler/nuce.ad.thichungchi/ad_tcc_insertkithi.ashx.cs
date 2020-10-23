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
    public class ad_tcc_insertkithi : CoreHandlerCommonAdminThiChungChi
    {
        public override void WriteData(HttpContext context)
        {
            string strTen = context.Request["Ten"].ToString();
            string strMoTa = context.Request["MoTa"].ToString();
            string strBoDe = context.Request["BoDe"].ToString();
            string strPhongThi = context.Request["PhongThi"].ToString();
            context.Response.ContentType = "text/plain";
            string sql = string.Format(@"INSERT INTO [dbo].[NuceThi_KiThi]
           ([MonHocID]
           ,[BlockHocID]
           ,[BoDeID]
           ,[Ten]
           ,[MoTa]
           ,[InsertedDate]
           ,[UpdatedDate]
           ,[Type]
           ,[Status]
,[PhongThi])
     VALUES
           (-1
           ,-1
           ,{0}
           ,'{1}'
           ,'{2}'
            ,getDate()
            ,getDate()
            ,1,1,'{3}')", strBoDe,strTen,strMoTa,strPhongThi);
            int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_ThiChungChi.ConnectionString, CommandType.Text, sql);
            context.Response.Write(iReturn.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
