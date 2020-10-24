using System;
using System.Collections.Generic;
using System.Web.UI;

namespace nuce.web.qlhpm
{
    public partial class CapNhatTrangThaiBaiNopSinhVien : Page
    {
        protected DotNetNuke.Entities.Users.UserInfo objUserInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strData = "-1";
            try
            {
                objUserInfo = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo();

                if (((Request.QueryString["userid"] != null)&&(Request.QueryString["status"] != null) && (Request.QueryString["id"] != null) && (Request.QueryString["action"] != null)) ||
                        ((Request.Form["userid"] != null)&&(Request.Form["status"] != null) && (Request.Form["id"] != null) && (Request.Form["action"] != null)))
                {
                    int userid = -1;
                    int status = -1;
                    int id = -1;
                    string action = "capnhat";
                    if (Request.QueryString["id"] != null)
                    {
                        userid = int.Parse(Request.QueryString["userid"]);
                        status = int.Parse(Request.QueryString["status"]);
                        id = int.Parse(Request.QueryString["id"]);
                        action = Request.QueryString["action"];
                    }
                    else
                    {
                        userid = int.Parse(Request.Form["userid"]);
                        status = int.Parse(Request.Form["status"]);
                        id = int.Parse(Request.Form["id"]);
                        action = Request.Form["action"];
                    }
                    if (userid.Equals(objUserInfo.UserID))
                    {
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
                    else
                    {
                        strData = "-2";
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