using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nuce.CTSV.Extent
{
    public partial class LoginWithGoogle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string clientid = "747341024576-mud1ao0e5jij2dkm56sfu0i0fqv9ggc0.apps.googleusercontent.com";
            ////your client secret  
            //string clientsecret = "0TQze0o2lGXb4gUw77S2vw7l";
            ////your redirection url  
            //string redirection_url = "http://localhost:9000/Login";
            string url = "https://accounts.google.com/o/oauth2/v2/auth?scope=email profile&include_granted_scopes=true&redirect_uri=" +Utils.lwg_redirection_url + "&response_type=code&client_id=" + Utils.lwg_clientid + "";
            Response.Redirect(url);
        }
    }
}