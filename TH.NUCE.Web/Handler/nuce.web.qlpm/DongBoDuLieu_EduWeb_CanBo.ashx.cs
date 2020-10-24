using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Services;
namespace nuce.web.qlpm
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DongBoDuLieu_EduWeb_CanBo : CoreHandlerQLPMAdmin
    {
        public override void WriteData(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if ((context.Request["hockyid"] != null) && (context.Request["namhocid"] != null))
            {
                DataTable dtCanBo = data.Nuce_Eduweb.getCanBo();



                string MaCanBo = "";
                string Ten = "";
                string MaBM = "";
                string CanBoTG = "";
                string EmailCB1 = "";
                string EmailCB2 = "";
                string TelCB1 = "";
                string TelCB2 = "";
                string NgaySinh = "";
                string ngsinhcb = "";
                int isNhanVien = -1;
                for (int i = 0; i < dtCanBo.Rows.Count; i++)
                {
                    MaCanBo = dtCanBo.Rows[i]["MaCB"].ToString();
                    Ten = dtCanBo.Rows[i]["TenCB"].ToString();
                    MaBM = dtCanBo.Rows[i]["MaBM"].ToString();

                    CanBoTG = dtCanBo.Rows[i]["CanBoTG"].ToString();
                    EmailCB1 = dtCanBo.Rows[i]["EmailCB1"].ToString();
                    EmailCB2 = dtCanBo.Rows[i]["EmailCB2"].ToString();

                    TelCB1 = dtCanBo.Rows[i]["TelCB1"].ToString();
                    TelCB2 = dtCanBo.Rows[i]["TelCB2"].ToString();
                    NgaySinh = dtCanBo.Rows[i]["ngaysinh"].ToString();
                    ngsinhcb = dtCanBo.Rows[i]["ngsinhcb"].ToString();

                    isNhanVien = int.Parse(dtCanBo.Rows[i]["IsNhanVien"].ToString());

                    nuce.web.data.dnn_NuceCommon_CanBo.insert((i + 1), MaCanBo, Ten, MaBM, CanBoTG, EmailCB1, EmailCB2, TelCB1, TelCB2, NgaySinh, ngsinhcb, isNhanVien);
                }


                context.Response.Write("1");
                //http://localhost:8055/Handler/nuce.web.qlpm/DongBoDuLieu_EduWeb_Khoa.ashx?hockyid=1&&namhocid=1

            }
            else
            {
                context.Response.Write("-1");
            }
            context.Response.Flush(); // Sends all currently buffered output to the client.
            context.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            context.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            context.Response.End();
        }
    }
}
