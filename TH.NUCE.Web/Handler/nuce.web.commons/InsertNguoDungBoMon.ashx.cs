using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class InsertNguoDungBoMon : CoreHandlerCommonAdmin
    {
        public override void WriteData(HttpContext context)
        {
                //@BoMonID int,
                //@Ma varchar(10),
                //@Ten nvarchar(100),
                //@TenTiengAnh nvarchar(100),
                //@SoTinChi int,
                //@MoTa nvarchar(500)

            if ((context.Request["bomonid"] != null) && 
                (context.Request["userid"] != null))
            {
                int bomonid = int.Parse(context.Request["bomonid"]);
                int userid = int.Parse(context.Request["userid"]);
                data.dnn_NuceCommon_NguoiDung_BoMon.insert(bomonid, userid);
            }
            
            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
            context.Response.End();
        }
    }
}
