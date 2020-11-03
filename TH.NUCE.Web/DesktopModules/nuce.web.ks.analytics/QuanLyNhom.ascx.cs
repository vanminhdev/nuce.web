using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.ks.analytics
{
    public partial class QuanLyNhom : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["EditType"] != null && Request.QueryString["EditType"] != "")
                {
                    string strType = Request.QueryString["EditType"];
                    txtType.Text = strType;
                    divAnnouce.InnerHtml = "";
                    if (strType == "edit" || strType == "insert")
                    {
                        //Lấy dữ liệu từ server để nhét vào các control
                        if (strType == "insert")
                        {
                            divAnnouce.InnerHtml = "Thêm mới nhóm";
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin của nhóm";
                            int iID = int.Parse(Request.QueryString["ID"]);
                            DataTable tblTable = DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("Nuce_KS_Group_Get", iID).Tables[0];
                            //DataTable tblTable = data.dnn_NuceCommon_Khoa.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {
                                txtTen.Text = tblTable.Rows[0]["Name"].ToString();
                                txtMoTa.Text = tblTable.Rows[0]["Description"].ToString();
                                ddlLoaiNhom.SelectedValue = tblTable.Rows[0]["Type"].ToString();
                                txtID.Text = iID.ToString();
                            }
                        }
                    }
                    else if (strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["ID"]);
                        DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("Nuce_KS_Group_UpdateStatus", iID, 4);
                        returnMain();
                    }
                }
                else
                {
                    divAnnouce.InnerHtml = "";
                    divEdit.Visible = false;
                    displayList();
                }
            }
        }

        private void displayList()
        {
            DataTable tblTable = DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("Nuce_KS_Group_Get", -1).Tables[0];
            string strTable = "<table border='1' width='100%' >";
            strTable += "<tr style='text-align:center; font-weight:bold;'>";
            strTable += string.Format("<td width='3%'>{0}</td>", "STT");
            strTable += string.Format("<td>{0}</td>", "Tên");
            strTable += string.Format("<td>{0}</td>", "Mô tả");
            strTable += string.Format("<td width='8%'>{0}</td>", "Loại nhóm");
            strTable += string.Format("<td width='8%'>{0}</td>", "Chỉnh sửa");
            strTable += string.Format("<td width='3%'>{0}</td>", "Xóa");
            strTable += string.Format("<td width='8%'><a href='/tabid/{0}/default.aspx?EditType=insert'>Thêm mới</a></td>", this.TabId);
            strTable += "</tr>";
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                strTable += " <tr>";
                strTable += string.Format("<td>{0}</td>", (i + 1));
                strTable += string.Format("<td>{0}</td>", tblTable.Rows[i]["Name"].ToString());
                strTable += string.Format("<td>{0}</td>", tblTable.Rows[i]["Description"].ToString());
                strTable += string.Format("<td>Loại {0}</td>", tblTable.Rows[i]["Type"].ToString());
                strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}'>Sửa</a></td>", this.TabId, tblTable.Rows[i]["ID"].ToString());
                strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}'>Xóa</a></td>", this.TabId, tblTable.Rows[i]["ID"].ToString());
                strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=insert'>Thêm mới</a></td>", this.TabId);
                strTable += "</tr>";
            }
            // strTable += string.Format("<tr><td colspan='6' style=' text-align: right;'><a href='/tabid/{0}/default.aspx?EditType=insert'>Thêm mới</a></td></tr>", this.TabId);
            strTable += "</table>";
            divContent.InnerHtml = strTable;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string strType = txtType.Text;
            string strTen = txtTen.Text;
            string strMoTa = txtMoTa.Text;
            bool blCheck = true;

            if (strTen.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên trắng";
                blCheck = false;
            }
            if (blCheck)
            {
                if (strType == "insert")
                {
                    DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("Nuce_KS_Group_Insert1", strTen, strMoTa,ddlLoaiNhom.SelectedValue);
                    returnMain();
                }
                else
                {
                    if (strType == "edit")
                    {
                        int iID = int.Parse(txtID.Text);
                        DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("Nuce_KS_Group_Update1", iID, strTen, strMoTa, ddlLoaiNhom.SelectedValue);
                        returnMain();
                    }
                }
            }
        }

        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain()
            ;
        }
        private void returnMain()
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx", this.TabId));
        }
    }
}