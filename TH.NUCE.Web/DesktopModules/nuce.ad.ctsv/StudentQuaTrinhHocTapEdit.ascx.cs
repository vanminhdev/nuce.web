using DotNetNuke.Entities.Modules;
using nuce.web.data;
using System;
using System.Data;

namespace nuce.ad.ctsv
{
    public partial class StudentQuaTrinhHocTapEdit : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] == null)
                    Response.Redirect("/tabid/21/default.aspx");
                else
                {
                    int iID = -1;
                    if (int.TryParse(Request.QueryString["ID"].Trim(), out iID))
                    {
                        string sql = "select ID,Code,FulName from AS_Academy_Student where Status<>4 and ID=" + iID;
                        DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            divSinhVien.InnerHtml = string.Format("Chỉnh sửa thông tin quá trình học tập của sinh viên {0} - {1}", dt.Rows[0]["FulName"].ToString(), dt.Rows[0]["Code"].ToString());
                            spThamSo.InnerHtml = string.Format("<script>var StudentCode='{0}';var StudentID={1};</script>", dt.Rows[0]["Code"].ToString(), iID);
                        }
                        else
                        {
                            Response.Redirect("/tabid/21/default.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("/tabid/21/default.aspx");
                    }
                }
            }
        }
    }
}