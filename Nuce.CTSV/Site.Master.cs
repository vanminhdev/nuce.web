using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Nuce.CTSV
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Utils.session_sinhvien] == null)
            {
                //Chuyển đến trang đăng nhập
                Response.Redirect(string.Format("/Login.aspx"));
            }
            else
            {
                nuce.web.model.SinhVien SinhVien = (nuce.web.model.SinhVien)Session[Utils.session_sinhvien];
                divLargeMssv.InnerHtml = divMobileMssv.InnerHtml = $"ID: {SinhVien.MaSV}";
                divLargeUsername.InnerHtml = divMobileUsername.InnerHtml = SinhVien.Ho;
                avatar.Src = SinhVien.IMG;

                string URL = Request.RawUrl.ToUpper();
                upateActiveMenu(linkDichVuSV, URL.Contains("DICHVU"));
                upateActiveMenu(linkCapNhatHoSoSV, URL.Contains("CAPNHATHOSO"));
                upateActiveMenu(linkHoSoSV, URL.Contains("HOSOSINHVIEN"));
                upateActiveMenu(linkTinTuc, URL.Contains("DEFAULT") || URL.Contains("CHITIETBAITIN"));

                btnLargeCapNhatHoSo.Disabled = btnMobileCapNhatHoSo.Disabled = URL.Contains("CAPNHATHOSO");
            }
        }
        private void upateActiveMenu(HtmlAnchor anchor, bool isActive)
        {
            string cls = anchor.Attributes["class"].ToString();
            if (isActive)
            {
                cls += " menu-item-active";
            } 
            else
            {
                cls = cls.Replace("menu-item-active", "");
            }
            anchor.Attributes["class"] = cls;
        }
    }
}