using System;
using System.Data;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class Logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("/Default.aspx");
        }
    }
}