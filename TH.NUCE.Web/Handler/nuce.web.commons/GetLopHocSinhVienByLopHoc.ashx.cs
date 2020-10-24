using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetLopHocSinhVienByLopHoc : CoreHandlerCommonAdmin
    {
        public override void WriteData(HttpContext context)
        {
            string strData = "";
            if (context.Request["lophocid"] != null)
            {
                int lophocid = int.Parse(context.Request["lophocid"]);
                if (this.objUserInfo.IsInRole(web.Utils.role_Admin) || this.objUserInfo.IsInRole(web.Utils.role_QuanTriThongTinChung))
                    strData = Utils.DataTableToJSONWithJavaScriptSerializer(data.dnn_NuceCommon_LopHoc_SinhVien.getByLopHoc(lophocid));
                else
                    strData = Utils.DataTableToJSONWithJavaScriptSerializer(data.dnn_NuceCommon_LopHoc_SinhVien.getByLopHoc(lophocid,this.objUserInfo.UserID));
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(strData);
            context.Response.Flush(); // Sends all currently buffered output to the client.
            context.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            context.ApplicationInstance.CompleteRequest();
        }
    }
}
