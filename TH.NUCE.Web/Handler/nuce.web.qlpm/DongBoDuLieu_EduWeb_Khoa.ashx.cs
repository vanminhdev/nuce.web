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
    public class DongBoDuLieu_EduWeb_Khoa : CoreHandlerQLPMAdmin
    {
        public override void WriteData(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if ((context.Request["hockyid"] != null) && (context.Request["namhocid"] != null))
            {
           
                DataTable dtKhoa = data.Nuce_Eduweb.getKhoa();
                int truongID = 2;
                string tenTiengAnh = "";
                string diaChi = "";
                string moTa = "";
                int type = 1;
                int status = 1;
                string ma = "";
                string ten = "";
                for (int i = 0; i < dtKhoa.Rows.Count; i++)
                {
                    ma = dtKhoa.Rows[i]["MaKH"].ToString();
                    ten = dtKhoa.Rows[i]["TenKhoa"].ToString();
                    nuce.web.data.dnn_NuceCommon_Khoa.insert((i+1), truongID, ma, ten, tenTiengAnh, diaChi, moTa);
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
