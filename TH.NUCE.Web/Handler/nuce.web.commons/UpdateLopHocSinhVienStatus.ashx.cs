using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UpdateLopHocSinhVienStatus : CoreHandlerCommonAdmin
    {
        public override void WriteData(HttpContext context)
        {
                //@BoMonID int,
                //@Ma varchar(10),
                //@Ten nvarchar(100),
                //@TenTiengAnh nvarchar(100),
                //@SoTinChi int,
                //@MoTa nvarchar(500)

            if ((context.Request["lophoc_sinhvienid"] != null) && 
                (context.Request["status"] != null))
            {
                int lophoc_sinhvienid = int.Parse(context.Request["lophoc_sinhvienid"]);
                int status = int.Parse(context.Request["status"]);
                data.dnn_NuceCommon_LopHoc_SinhVien.updateStatus(lophoc_sinhvienid, status);
            }
            
            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
            context.Response.End();
        }
    }
}
