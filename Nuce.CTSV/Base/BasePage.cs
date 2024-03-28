using System;
using System.Web;
using System.Web.UI;

namespace Nuce.CTSV
{
    public class BasePage: Page
    {
        public nuce.web.model.SinhVien m_SinhVien;
        protected override void OnInit(EventArgs e)
        {
            var refreshToken = Request.Cookies["JWT-refresh-token"];
            var sv = Session[Utils.session_sinhvien];
            if (refreshToken == null || Session[Utils.session_sinhvien] == null)
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
                m_SinhVien = (nuce.web.model.SinhVien)Session[Utils.session_sinhvien];
            }
            //if (Session[Utils.session_sinhvien] == null)
            //{
            //    //Chuyển đến trang đăng nhập
            //    Response.Redirect(string.Format("/Login.aspx"));
            //}
            //else
            //    m_SinhVien = (nuce.web.model.SinhVien)Session[Utils.session_sinhvien];
            base.OnInit(e);
        }
      
    }
}