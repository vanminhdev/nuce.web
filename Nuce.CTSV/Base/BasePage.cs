using System;
using System.Web.UI;

namespace Nuce.CTSV
{
    public class BasePage: Page
    {
        public nuce.web.model.SinhVien m_SinhVien;
        protected override void OnInit(EventArgs e)
        {
          
                if (Session[Utils.session_sinhvien] == null)
                {
                    //Chuyển đến trang đăng nhập
                    Response.Redirect(string.Format("/Login.aspx"));
                }
                else
                    m_SinhVien = (nuce.web.model.SinhVien)Session[Utils.session_sinhvien];
            base.OnInit(e);
        }
      
    }
}