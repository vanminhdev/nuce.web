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
    public class GetLopHoc : CoreHandlerCommon
    {
        public override void WriteData(HttpContext context)
        {
            string strData = "";
            if (context.Request["lophocid"] != null)
            {
                int lophocid = int.Parse(context.Request["lophocid"]);
                strData = Utils.DataTableToJSONWithJavaScriptSerializer(data.dnn_NuceCommon_LopHoc.get(lophocid));
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(strData);
            context.Response.End();
        }
    }
}
