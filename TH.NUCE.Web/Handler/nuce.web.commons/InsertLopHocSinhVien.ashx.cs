using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class InsertLopHocSinhVien : CoreHandlerCommonAdmin
    {
        public override void WriteData(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if ((context.Request["lophocid"] != null) && (context.Request["masv"] != null) 
                && (context.Request["ghichu"] != null))
            {
                int lophocid = int.Parse(context.Request["lophocid"]);
                string masv = context.Request["masv"];
                string ghichu = context.Request["ghichu"];
                int iOrder = 1;
                int.TryParse(context.Request["order"], out iOrder);
                data.dnn_NuceCommon_LopHoc_SinhVien.insert(lophocid, masv, ghichu, iOrder);
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
