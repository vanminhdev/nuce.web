using System;
using System.Collections.Generic;
using System.Web.UI;
using Ionic.Zip;
using System.IO;

namespace nuce.web.qlhpm
{
    public partial class Export : Page
    {
        protected DotNetNuke.Entities.Users.UserInfo objUserInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strData = "-1";
            try
            {
                objUserInfo = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo();

                if (((Request.QueryString["userid"] != null) && (Request.QueryString["id"] != null) && (Request.QueryString["action"] != null)) ||
                        ((Request.Form["userid"] != null) && (Request.Form["id"] != null) && (Request.Form["action"] != null)))
                {
                    int userid = -1;
                    int id = -1;
                    string action = "filedottraodoi";
                    if (Request.QueryString["id"] != null)
                    {
                        userid = int.Parse(Request.QueryString["userid"]);
                        id = int.Parse(Request.QueryString["id"]);
                        action = Request.QueryString["action"];
                    }
                    else
                    {
                        userid = int.Parse(Request.Form["userid"]);
                        id = int.Parse(Request.Form["id"]);
                        action = Request.Form["action"];
                    }
                    if (userid.Equals(objUserInfo.UserID))
                    {
                        if (action.Equals("filedottraodoi"))
                        {
                            try
                            {
                                string pathname = Server.MapPath("~/NuceDataUpload/" + id.ToString() + "/");
                                string[] filename = Directory.GetFiles(pathname);
                                using (ZipFile zip = new ZipFile())
                                {
                                    zip.AddFiles(filename, "file");
                                    string path = Server.MapPath("~/NuceDataUpload/" + id.ToString() + ".zip");
                                    FileInfo file = new FileInfo(path);
                                    if (file.Exists)//check file exsit or not
                                    {
                                        file.Delete();
                                    }
                                    zip.Save(path);
                                    Response.Redirect("/NuceDataUpload/" + id.ToString() + ".zip");
                                }
                            }
                            catch (Exception ex)
                            {
                                strData = ex.ToString();
                            }
                        }
                        else
                        {
                            strData = "Khong dung hanh dong";
                        }
                    }
                    else
                    {
                        strData = "Chua dang nhap";
                    }
                }
            }
            catch (Exception ex)
            {
                strData = ex.Message;
            }
            divThongBao.InnerHtml = strData;
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}