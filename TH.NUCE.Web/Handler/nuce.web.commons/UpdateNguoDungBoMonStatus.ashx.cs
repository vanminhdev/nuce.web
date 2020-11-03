using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UpdateNguoDungBoMonStatus : CoreHandlerCommonAdmin
    {
        public override void WriteData(HttpContext context)
        {
                //@BoMonID int,
                //@Ma varchar(10),
                //@Ten nvarchar(100),
                //@TenTiengAnh nvarchar(100),
                //@SoTinChi int,
                //@MoTa nvarchar(500)
            context.Response.ContentType = "text/plain";
            if ((context.Request["nguoidung_bomonid"] != null) && 
                (context.Request["status"] != null))
            {
                int nguoidung_bomonid = int.Parse(context.Request["nguoidung_bomonid"]);
                int status = int.Parse(context.Request["status"]);
                data.dnn_NuceCommon_NguoiDung_BoMon.updateStatus(nguoidung_bomonid, status);
                context.Response.Write("1");
            }
            else
            {
                context.Response.Write("-1");
            }
            context.Response.End();
        }
    }
}
