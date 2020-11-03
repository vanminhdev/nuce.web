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
    public class GetNguoiDungByMonHoc : CoreHandlerCommon
    {
        public override void WriteData(HttpContext context)
        {
            string strData = "";
            if (context.Request["monhocid"] != null && context.Request["role"] != null)
            {
                int monhocid = int.Parse(context.Request["monhocid"]);
                int role = int.Parse(context.Request["role"]);
                strData = Utils.DataTableToJSONWithJavaScriptSerializer(data.dnn_NuceCommon_NguoiDung_MonHoc.get(monhocid,role));
            }
            
            context.Response.ContentType = "text/plain";
            context.Response.Write(strData);
            context.Response.End();
        }
    }
}
