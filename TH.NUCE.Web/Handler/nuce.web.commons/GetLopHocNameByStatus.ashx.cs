using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetLopHocNameByStatus : CoreHandlerCommonAdmin
    {
        public override void WriteData(HttpContext context)
        {
            string strData = "";
            if (context.Request["status"] != null)
            {
                int status = int.Parse(context.Request["status"]);
                if (this.objUserInfo.IsInRole(web.Utils.role_Admin) || this.objUserInfo.IsInRole(web.Utils.role_QuanTriThongTinChung))
                    strData = Utils.DataTableToJSONWithJavaScriptSerializer(data.dnn_NuceCommon_LopHoc.getNameByStatus(status));
                else
                    strData = Utils.DataTableToJSONWithJavaScriptSerializer(data.dnn_NuceCommon_LopHoc.getNameByStatus(status,this.objUserInfo.UserID));
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(strData);
            context.Response.Flush(); // Sends all currently buffered output to the client.
            context.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            context.ApplicationInstance.CompleteRequest();
        }
    }
}
