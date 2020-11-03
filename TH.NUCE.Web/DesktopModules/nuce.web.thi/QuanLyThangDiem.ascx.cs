using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.commons
{
    public partial class QuanLyThangDiem : PortalModuleBase
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
                            divAnnouce.InnerHtml = "Thêm mới thang điểm";
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin của thang điểm";
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceThi_ThangDiem.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {
                                txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                                txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                txtID.Text = iID.ToString();
                            }
                        }
                    }
                    else if(strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceThi_ThangDiem.updateStatus(iID,4);
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
            DataTable tblTable = data.dnn_NuceThi_ThangDiem.get(-1);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách ThangDiem</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable +="<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t2 fl'>{0}</div>", "Mô tả");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert'>Thêm mới</a></div>", this.TabId);
            strTable += "</div>";
                    strTable += "<div class='nd_b fl'>";
                    for (int i = 0; i < tblTable.Rows.Count; i++)
                    {
                        strTable += " <div class='nd_b1 fl'>";
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i+1));
                        strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", tblTable.Rows[i]["Ten"].ToString());
                        strTable += string.Format("<div class='nd_t2 fl'>{0}</div>", tblTable.Rows[i]["MoTa"].ToString());
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["ThangDiemID"].ToString());
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["ThangDiemID"].ToString());
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert'>Thêm mới</a></div>", this.TabId);
                        strTable += "</div>";
                    }
                    strTable += "</div>";
                 strTable += "</div>";
                 strTable += "</div>";
            strTable += "</div>";
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
                    data.dnn_NuceThi_ThangDiem.insert( strTen, strMoTa);
                    //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                    returnMain();
                }
                else
                {
                    if (strType == "edit")
                    {
                        int iID = int.Parse(txtID.Text);
                        data.dnn_NuceThi_ThangDiem.update(iID,  strTen, 
                            strMoTa);
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
        private  void returnMain()
        {
              Response.Redirect(string.Format("/tabid/{0}/default.aspx", this.TabId));
        }
    }
}