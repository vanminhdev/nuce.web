using System;
using System.Collections.Generic;
using System.Web.UI;

namespace nuce.web.sinhvien
{
    public partial class HocUpdateStatus : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string strData = "-1";
            try
            {
                if (Session[Utils.session_ca_lophoc_sinhvien] == null)
                {
                    strData = "NotAuthenticated";
                }
                else
                {
                    if (((Request.QueryString["status"] != null) && (Request.QueryString["id"] != null) && (Request.QueryString["action"] != null)) ||
                        ((Request.Form["status"] != null) && (Request.Form["id"] != null) && (Request.Form["action"] != null)))
                    {
                        int status = -1;
                        int id = -1;
                        string action = "capnhat";
                        if (Request.QueryString["id"] != null)
                        {
                            status = int.Parse(Request.QueryString["status"]);
                            id = int.Parse(Request.QueryString["id"]);
                            action = Request.QueryString["action"];
                        }
                        else
                        {
                            status = int.Parse(Request.Form["status"]);
                            id = int.Parse(Request.Form["id"]);
                            action = Request.Form["action"];
                        }
                        strData = "-1";
                        if (action.Equals("capnhat"))
                        {
                            data.dnn_NuceQLHPM_CaHoc_Items.updateStatus(id, status);
                            strData = "1";
                        }
                        else
                        {
                        }
                    }
                }
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