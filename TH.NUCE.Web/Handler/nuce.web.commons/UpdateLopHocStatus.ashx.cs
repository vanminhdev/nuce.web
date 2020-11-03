using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UpdateLopHocStatus : CoreHandlerCommonAdmin
    {
        public override void WriteData(HttpContext context)
        {
                //@BoMonID int,
                //@Ma varchar(10),
                //@Ten nvarchar(100),
                //@TenTiengAnh nvarchar(100),
                //@SoTinChi int,
                //@MoTa nvarchar(500)

            if ((context.Request["lophocid"] != null) && 
                (context.Request["status"] != null))
            {
                int lophocid = int.Parse(context.Request["lophocid"]);
                int status = int.Parse(context.Request["status"]);
                data.dnn_NuceCommon_LopHoc.updateStatus(lophocid, status);
            }
            
            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
            context.Response.End();
        }
    }
}
