using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nuce.CTSV
{
    public partial class LoginMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var refreshToken = Request.Cookies["JWT-refresh-token"];
            if (refreshToken != null && Session[Utils.session_sinhvien] != null)
            {
                Response.Redirect("/dichvusinhvien.aspx");
            }
        }
    }
}