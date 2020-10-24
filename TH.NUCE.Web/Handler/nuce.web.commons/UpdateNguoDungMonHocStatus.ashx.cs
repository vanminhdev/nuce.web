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
    public class UpdateNguoDungMonHocStatus : CoreHandlerCommonAdmin
    {
        public override void WriteData(HttpContext context)
        {
                //@BoMonID int,
                //@Ma varchar(10),
                //@Ten nvarchar(100),
                //@TenTiengAnh nvarchar(100),
                //@SoTinChi int,
                //@MoTa nvarchar(500)

            if ((context.Request["nguoidung_monhocid"] != null) && 
                (context.Request["status"] != null))
            {
                int nguoidung_monhocid = int.Parse(context.Request["nguoidung_monhocid"]);
                int status = int.Parse(context.Request["status"]);
                data.dnn_NuceCommon_NguoiDung_MonHoc.updateStatus(nguoidung_monhocid, status);
            }
            
            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
            context.Response.End();
        }
    }
}
