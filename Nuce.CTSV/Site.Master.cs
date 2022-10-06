using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Nuce.CTSV;

namespace Nuce.CTSV
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var refreshToken = Request.Cookies["JWT-refresh-token"].Values;
            if (Session[Utils.session_sinhvien] == null)
            {
                //Chuyển đến trang đăng nhập
                Session.RemoveAll();
                foreach (var cookie in Request.Cookies.AllKeys)
                {
                    var clearCookie = new HttpCookie(cookie);
                    clearCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Set(clearCookie);
                }
                Response.Redirect(string.Format("/Login.aspx"));
            }
            else
            {
                nuce.web.model.SinhVien SinhVien = (nuce.web.model.SinhVien)Session[Utils.session_sinhvien];
                divLargeMssv.InnerHtml = divMobileMssv.InnerHtml = $"ID: {SinhVien.MaSV}";
                divLargeUsername.InnerHtml = divMobileUsername.InnerHtml = SinhVien.Ho;
                //avatar.Src = SinhVien.IMG;

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