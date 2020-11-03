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
    public class DongBoDuLieu_EduWeb_MonHoc : CoreHandlerQLPMAdmin
    {
        public override void WriteData(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if ((context.Request["hockyid"] != null) && (context.Request["namhocid"] != null))
            {

                DataTable dtBoMon = data.dnn_NuceCommon_BoMon.get(-1);
                DataTable dtMonHoc = data.Nuce_Eduweb.getMonHoc();

                int BoMonID = -1;
                string MaBoMon = "";
                string Ma = "";
                string Ten = "";
                string TenTiengAnh = "";
                int type = 1;
                int status = 1;
                int SoTinChi = -1;
                string MoTa = "";
                for (int i = 0; i < dtMonHoc.Rows.Count; i++)
                {
                    MaBoMon = dtMonHoc.Rows[i]["MaBM"].ToString();
                    Ma= dtMonHoc.Rows[i]["MaMH"].ToString();
                    Ten= dtMonHoc.Rows[i]["TenMH"].ToString();
                     SoTinChi = int.Parse(dtMonHoc.Rows[i]["SoTC"].ToString());
                    BoMonID = getMonID(dtBoMon, MaBoMon);
                    nuce.web.data.dnn_NuceCommon_MonHoc.insert((i + 1),BoMonID,MaBoMon, Ma,  Ten, TenTiengAnh,SoTinChi, MoTa);
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
        public int getMonID(DataTable dt,string MaKhoa)
        {
            for(int i=0;i< dt.Rows.Count;i++)
            {
                if(dt.Rows[i]["Ma"].ToString().Equals(MaKhoa))
                {
                    return int.Parse(dt.Rows[i]["BoMonID"].ToString());
                }
            }
            return -1;
        }
    }
}
