using System;
using System.Collections.Generic;
using System.Web.UI;

namespace nuce.web.sinhvien
{
    public partial class XacThucMac : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string strData = "-1";
            try
            {
                if (Session[Utils.session_ca_lophoc_sinhvien] == null)
                {
                    strData = "NotAuthenticated";
                }
                else
                {
                    if (((Request.QueryString["calophocsinhvien"] != null) && (Request.QueryString["mac"] != null) && (Request.QueryString["action"] != null)) ||
                        ((Request.Form["calophocsinhvien"] != null) && (Request.Form["mac"] != null) && (Request.Form["action"] != null)))
                    {
                        int calophocsinhvien = -1;
                        string mac = "";
                        string action = "xacthuc";
                        if (Request.QueryString["calophocsinhvien"] != null)
                        {
                            calophocsinhvien = int.Parse(Request.QueryString["calophocsinhvien"]);
                            mac = Request.QueryString["mac"];
                            action = Request.QueryString["action"];
                        }
                        else
                        {
                            calophocsinhvien = int.Parse(Request.Form["calophocsinhvien"]);
                            mac = Request.Form["mac"];
                            action = Request.Form["action"];
                        }
                        Dictionary<int, model.CaLopHocSinhVien> m_CaLopHocSinhViens = (Dictionary<int, model.CaLopHocSinhVien>)Session[Utils.session_ca_lophoc_sinhvien];
                        if (!m_CaLopHocSinhViens.ContainsKey(calophocsinhvien))
                        {
                            strData = "Khong ton tai id";
                        }
                        else
                        {
                            DateTime dtNopBai = DateTime.Now;
                            model.CaLopHocSinhVien CaLopHocSinhVien = m_CaLopHocSinhViens[calophocsinhvien];


                            if (action.Equals("xacthuc"))
                            {
                                if (mac == "")
                                    strData = "-3";
                                else
                                { 
                                    strData= data.dnn_NuceQLHPM_Ca_LopHoc_SinhVien.xacthucmac(CaLopHocSinhVien.Ca_LopHoc_SinhVienID, CaLopHocSinhVien.Ca_LopHocID, CaLopHocSinhVien.SinhVienID, mac).ToString();
                                    if(strData=="1")
                                        CaLopHocSinhVien.Status = 2;
                                }
                            }
                            else
                            {
                                if (action.Equals("huyxacthuc"))
                                {
                                    data.dnn_NuceQLHPM_Ca_LopHoc_SinhVien.HuyXacThucMac(CaLopHocSinhVien.Ca_LopHoc_SinhVienID);
                                }
                                strData = "1";
                                CaLopHocSinhVien.Status = 1;
                            }
                            m_CaLopHocSinhViens[calophocsinhvien] = CaLopHocSinhVien;
                            Session[Utils.session_ca_lophoc_sinhvien] = m_CaLopHocSinhViens;
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