using System;
using System.Collections;
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
    public class GetBoMonByKhoa : CoreHandlerCommon
    {
        public override void WriteData(HttpContext context)
        {
            string strData = "";
            if (context.Request["khoaid"] != null)
            {
                int khoaid = int.Parse(context.Request["khoaid"]);
                strData = Utils.DataTableToJSONWithJavaScriptSerializer(data.dnn_NuceCommon_BoMon.getNameByKhoa(khoaid));
            }
            
            context.Response.ContentType = "text/plain";
            context.Response.Write(strData);
            context.Response.End();
        }
    }
}
