using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
namespace nuce.web.sinhvien
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class NopBai : CoreHandlerSinhVien
    {
        public override void WriteData(HttpContext context)
        {
            if ((context.Request["kithilophocsinhvien"] != null) && (context.Request["bailam"] != null))
            {
                int kithilophocsinhvien = int.Parse(context.Request["kithilophocsinhvien"]);

                Dictionary<int, model.KiThiLopHocSinhVien> m_KiThiLopHocSinhViens = (Dictionary<int, model.KiThiLopHocSinhVien>)context.Session[Utils.session_kithi_lophoc_sinhvien];
                if (!m_KiThiLopHocSinhViens.ContainsKey(kithilophocsinhvien))
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("-1");
                    context.Response.End();
                    return;
                }
                string bailam = context.Request["bailam"];
                DateTime dtNopBai = DateTime.Now;
                model.KiThiLopHocSinhVien KiThiLopHocSinhVien = m_KiThiLopHocSinhViens[kithilophocsinhvien];
                TimeSpan ts = dtNopBai.Subtract(KiThiLopHocSinhVien.NgayGioBatDau);

                data.dnn_NuceThi_KiThi_LopHoc_SinhVien.update_bailam(kithilophocsinhvien, bailam, KiThiLopHocSinhVien.NgayGioBatDau, DateTime.Now, KiThiLopHocSinhVien.TongThoiGianConLai - (ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds), Utils.GetIPAddress(), 5);
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
            context.Response.End();
        }
    }
}
