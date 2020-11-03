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
    public class UpdateMonHoc : CoreHandlerCommonAdmin
    {
        public override void WriteData(HttpContext context)
        {
                //@BoMonID int,
                //@Ma varchar(10),
                //@Ten nvarchar(100),
                //@TenTiengAnh nvarchar(100),
                //@SoTinChi int,
                //@MoTa nvarchar(500)

            if ((context.Request["monhocid"] != null) &&
                (context.Request["bomonid"] != null) && 
                (context.Request["ma"] != null)&&
                (context.Request["ten"] != null)&&
                (context.Request["tentienganh"] != null) &&
                (context.Request["sotinchi"] != null)&&
                 (context.Request["mota"] != null))
            {
                int monhocid = int.Parse(context.Request["monhocid"]);
                int bomonid = int.Parse(context.Request["bomonid"]);
                string ma = context.Request["ma"];
                string ten = context.Request["ten"];
                string tentienganh = context.Request["tentienganh"];
                int sotinchi = int.Parse(context.Request["sotinchi"]);
                string mota = context.Request["mota"];
                data.dnn_NuceCommon_MonHoc.update(monhocid,bomonid, ma, ten, tentienganh, sotinchi, mota);
            }
            
            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
            context.Response.End();
        }
    }
}
