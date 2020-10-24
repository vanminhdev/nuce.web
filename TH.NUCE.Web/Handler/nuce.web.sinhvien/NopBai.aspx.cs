using System;
using System.Collections.Generic;
using System.Web.UI;

namespace nuce.web.sinhvien
{
    public partial class NopBai : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string strData = "-1";
            try
            {
                if (Session[Utils.session_kithi_lophoc_sinhvien] == null)
                {
                    strData = "NotAuthenticated";
                }
                else
                {
                    if (((Request.QueryString["kithilophocsinhvien"] != null) && (Request.QueryString["bailam"] != null) && (Request.QueryString["action"] != null)) ||
                        ((Request.Form["kithilophocsinhvien"] != null) && (Request.Form["bailam"] != null) && (Request.Form["action"] != null)))
                    {
                        int kithilophocsinhvien = -1;
                        string bailam = "";
                        string action = "nopbai";
                        if (Request.QueryString["kithilophocsinhvien"] != null)
                        {
                            kithilophocsinhvien = int.Parse(Request.QueryString["kithilophocsinhvien"]);
                            bailam = Request.QueryString["bailam"];
                            action = Request.QueryString["action"];
                        }
                        else
                        {
                            kithilophocsinhvien = int.Parse(Request.Form["kithilophocsinhvien"]);
                            bailam = Request.Form["bailam"];
                            action = Request.Form["action"];
                        }
                        Dictionary<int, model.KiThiLopHocSinhVien> m_KiThiLopHocSinhViens = (Dictionary<int, model.KiThiLopHocSinhVien>)Session[Utils.session_kithi_lophoc_sinhvien];
                        if (!m_KiThiLopHocSinhViens.ContainsKey(kithilophocsinhvien))
                        {
                            strData = "Khong ton tai id";
                        }
                        else
                        {
                            DateTime dtNopBai = DateTime.Now;
                            model.KiThiLopHocSinhVien KiThiLopHocSinhVien = m_KiThiLopHocSinhViens[kithilophocsinhvien];

                            TimeSpan ts = dtNopBai.Subtract(KiThiLopHocSinhVien.NgayGioBatDau);
                            int iTongThoiGianConLai = KiThiLopHocSinhVien.TongThoiGianConLai - (ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds);
                            KiThiLopHocSinhVien.Status = 5;
                            KiThiLopHocSinhVien.TongThoiGianConLai = iTongThoiGianConLai > 0 ? iTongThoiGianConLai : 0;
                            KiThiLopHocSinhVien.BaiLam = bailam;

                            if (action.Equals("chamdiem"))
                            {
                                KiThiLopHocSinhVien = Utils.chamBai(KiThiLopHocSinhVien, bailam);
                                strData = KiThiLopHocSinhVien.Mota;
                                data.dnn_NuceThi_KiThi_LopHoc_SinhVien.update_bailam1(kithilophocsinhvien, bailam, KiThiLopHocSinhVien.Diem, KiThiLopHocSinhVien.NgayGioBatDau, DateTime.Now, iTongThoiGianConLai, Utils.GetIPAddress(), KiThiLopHocSinhVien.Status);
                            }
                            else
                            {
                                if (action.Equals("dangthi"))
                                {
                                    KiThiLopHocSinhVien.Status = 3;
                                }
                                data.dnn_NuceThi_KiThi_LopHoc_SinhVien.update_bailam(kithilophocsinhvien, bailam, KiThiLopHocSinhVien.NgayGioBatDau, DateTime.Now, iTongThoiGianConLai, Utils.GetIPAddress(), KiThiLopHocSinhVien.Status);
                                strData = "1";
                            }
                            KiThiLopHocSinhVien.NgayGioBatDau = dtNopBai;
                            m_KiThiLopHocSinhViens[kithilophocsinhvien] = KiThiLopHocSinhVien;
                            Session[Utils.session_kithi_lophoc_sinhvien] = m_KiThiLopHocSinhViens;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strData = ex.Message;
            }
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Write(strData);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}