using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.commons
{
    public partial class QuanLyKhoa : PortalModuleBase
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
                            divAnnouce.InnerHtml = "Thêm mới khoa";
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin của khoa";
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceCommon_Khoa.get(iID);
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
                    }
                    else if(strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_Khoa.updateStatus(iID,4);
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
            DataTable tblTable = data.dnn_NuceCommon_Khoa.get(-1);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách khoa</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable +="<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "Mã");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Tên Tiếng anh");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Địa chỉ");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Mô tả");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Thêm mới");
            strTable += "</div>";
                    strTable += "<div class='nd_b fl'>";
                    for (int i = 0; i < tblTable.Rows.Count; i++)
                    {
                        strTable += " <div class='nd_b1 fl'>";
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i+1));
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", tblTable.Rows[i]["Ma"].ToString());
                        strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["Ten"].ToString());
                        strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["TenTiengAnh"].ToString());
                        strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", tblTable.Rows[i]["DiaChi"].ToString());
                        strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", tblTable.Rows[i]["MoTa"].ToString());
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["KhoaID"].ToString());
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["KhoaID"].ToString());
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
            string strMa = txtMa.Text;
            string strTen = txtTen.Text;
            string strTenTiengAnh = txtTenTiengAnh.Text;
            string strDiaChi = txtDiaChi.Text;
            string strMoTa = txtMoTa.Text;
            int iTruongID=int.Parse(ddlTruong.SelectedValue.ToString())
            ;
            bool blCheck = true;
            if (strMa.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để mã trắng";
                blCheck = false;
            }
            if (strTen.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên trắng";
                blCheck = false;
            }
            if (blCheck)
            {
                if (strType == "insert")
                {
                    data.dnn_NuceCommon_Khoa.insert(iTruongID, strMa, strTen, strTenTiengAnh, strDiaChi, strMoTa);
                    //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                    returnMain();
                }
                else
                {
                    if (strType == "edit")
                    {
                        int iID = int.Parse(txtID.Text);
                        data.dnn_NuceCommon_Khoa.update(iID, iTruongID, strMa, strTen, strTenTiengAnh, strDiaChi,
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