using nuce.web.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace nuce.web.sinhvien
{
    public partial class tcc_logout : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string strData = "-1";
            try
            {
                Session.RemoveAll();
                    strData = "1";
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