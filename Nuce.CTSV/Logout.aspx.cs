using System;
using System.Data;
using System.Web;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class Logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            foreach (var cookie in Request.Cookies.AllKeys)
            {
                var clearCookie = new HttpCookie(cookie);
                clearCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Set(clearCookie);
            }
            Response.Redirect("/Login.aspx");
        }
    }
}