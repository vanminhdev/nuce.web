using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Web.Security;
namespace nuce.web.test
{
    public partial class test : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divContent.InnerHtml = "";
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            //divContent.InnerHtml = string.Format("email cua {0} da duoc gui den admin", txtTest.Text);


            //Creating object of MembershipUser class which will take username as parameter.
            MembershipUser objUser = Membership.GetUser(txtTest.Text.Trim());

            //Storing password in a string variable
            string strPassword = objUser.GetPassword();
            divContent.InnerHtml = strPassword;
        }
    }
}