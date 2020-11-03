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
    public class DongBoDuLieu_EduWeb_BoMon : CoreHandlerQLPMAdmin
    {
        public override void WriteData(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if ((context.Request["hockyid"] != null) && (context.Request["namhocid"] != null))
            {

                DataTable dtKhoa = data.dnn_NuceCommon_Khoa.get(-1);
                DataTable dtBoMon = data.Nuce_Eduweb.getBoMon();

                int KhoaID = -1;
                string MaKhoa = "";
                string Ma = "";
                string Ten = "";
                string TenTiengAnh = "";
                int type = 1;
                int status = 1;
                string DiaChi = "";
                string MoTa = "";
                for (int i = 0; i < dtBoMon.Rows.Count; i++)
                {
                    MaKhoa = dtBoMon.Rows[i]["MaKH"].ToString();
                    Ma= dtBoMon.Rows[i]["MaBM"].ToString();
                    Ten= dtBoMon.Rows[i]["TenBM"].ToString();
                    MoTa = string.Format("Bo mon co truong bo mon la: {0}", dtBoMon.Rows[i]["TruongBM"].ToString());
                    KhoaID = getKhoaID(dtKhoa, MaKhoa);
                    nuce.web.data.dnn_NuceCommon_BoMon.insert((i + 1), KhoaID, Ma, MaKhoa, Ten, TenTiengAnh, DiaChi, MoTa);
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
        public int getKhoaID(DataTable dt,string MaKhoa)
        {
            for(int i=0;i< dt.Rows.Count;i++)
            {
                if(dt.Rows[i]["Ma"].ToString().Equals(MaKhoa))
                {
                    return int.Parse(dt.Rows[i]["KhoaID"].ToString());
                }
            }
            return -1;
        }
    }
}
