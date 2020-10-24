using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.commons
{
    public partial class SuaThongTinTruong : PortalModuleBase
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
                    if (strType == "edit")
                    {
                        //Lấy dữ liệu từ server để nhét vào các control
                        //THCore_CM_GetTag
                        divAnnouce.InnerHtml = "Chỉnh sửa thông tin trường"; 
                        int iID = int.Parse(Request.QueryString["id"]);
                        DataTable tblTable = data.dnn_NuceCommon_Truong.get(iID);
                        if (tblTable.Rows.Count > 0)
                        {
                            txtMa.Text = tblTable.Rows[0]["Ma"].ToString();
                            txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                            txtTenTiengAnh.Text = tblTable.Rows[0]["TenTiengAnh"].ToString();
                            txtDiaChi.Text = tblTable.Rows[0]["DiaChi"].ToString();
                            txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                            txtID.Text = iID.ToString();
                        }
                    }
                    else if(strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_UpdateStatusTags", iID,0);
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
            DataTable tblTable = data.dnn_NuceCommon_Truong.get(-1);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Thông tin trường</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable +="<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "Mã");
            strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", "Tên Tiếng anh");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Địa chỉ");
            strTable += string.Format("<div class='nd_t22 fl'>{0}</div>", "Mô tả");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += "</div>";
                    strTable += "<div class='nd_b fl'>";
                    for (int i = 0; i < tblTable.Rows.Count; i++)
                    {
                        strTable += " <div class='nd_b1 fl'>";
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", tblTable.Rows[i]["Ma"].ToString());
                        strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", tblTable.Rows[i]["Ten"].ToString());
                        strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", tblTable.Rows[i]["TenTiengAnh"].ToString());
                        strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", tblTable.Rows[i]["DiaChi"].ToString());
                        strTable += string.Format("<div class='nd_t22 fl'>{0}</div>", tblTable.Rows[i]["MoTa"].ToString());
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["TruongID"].ToString());
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
            string strMa = txtMa.Text;
            string strTen = txtTen.Text;
            string strTenTiengAnh = txtTenTiengAnh.Text;
            string strDiaChi = txtDiaChi.Text;
            string strMoTa = txtMoTa.Text;
            bool blCheck = true;
            if (strMa.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để mã trắng";
                blCheck = false;
            }
            if (strTen.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên";
                blCheck = false;
            }
            if (blCheck)
            {
                if (strType == "insert")
                {
                    //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                    returnMain();
                }
                else
                {
                    if (strType == "edit")
                    {
                        int iID = int.Parse(txtID.Text);
                        data.dnn_NuceCommon_Truong.update(iID, strMa, strTen, strTenTiengAnh, strDiaChi, strMoTa);
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