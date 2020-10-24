using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetMonHocName : CoreHandlerCommonAdmin
    {
        public override void WriteData(HttpContext context)
        {
            string strData = "";
                strData = Utils.DataTableToJSONWithJavaScriptSerializer(data.dnn_NuceCommon_MonHoc.getByNguoiDung(this.objUserInfo.UserID));
            context.Response.ContentType = "text/plain";
            context.Response.Write(strData);

            context.Response.Flush(); // Sends all currently buffered output to the client.
            context.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            context.ApplicationInstance.CompleteRequest(); 
                        //context.Response.End();
        }
    }
}
