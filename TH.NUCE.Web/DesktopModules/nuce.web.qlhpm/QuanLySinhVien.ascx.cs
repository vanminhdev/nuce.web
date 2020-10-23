using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Web.UI.WebControls;

namespace nuce.web.qlhpm
{
    public partial class QuanLySinhVien : PortalModuleBase
    {
        int iCaLopHocID;
        int iPhongHoc;
        protected override void OnInit(EventArgs e)
        {
            #region dropdownlist
            #endregion
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["calophoc"] != null && Request.QueryString["calophoc"] != "" && int.TryParse(Request.QueryString["calophoc"], out iCaLopHocID))
            {
                // Lấy thông tin của ca học
                DataTable dtCaHoc = nuce.web.data.dnn_NuceQLHPM_Ca_LopHoc.get(iCaLopHocID);
                if (dtCaHoc.Rows.Count > 0)
                {
                    txtCaLopHoc.Text = iCaLopHocID.ToString();
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
                                    divAnnouce.InnerHtml = "Thêm mới sinh viên";
                                    bindMay("");
                                }
                                else
                                {
                                    divAnnouce.InnerHtml = "Cập nhật thông tin";
                                    int iID = int.Parse(Request.QueryString["id"]);
                                    DataTable tblTable = data.dnn_NuceQLHPM_Ca_LopHoc_SinhVien.get(iID);
                                    if (tblTable.Rows.Count > 0)
                                    {
                                        string mac= tblTable.Rows[0]["MacAddress"].ToString();
                                        bindMay(mac);
                                        ddlMay.SelectedValue =  mac;
                                        txtMaSV.Text= tblTable.Rows[0]["MaSV"].ToString(); 
                                        ddlTrangThai.SelectedValue= tblTable.Rows[0]["Status"].ToString();
                                        txtID.Text = iID.ToString();
                                        txtCaLopHoc.Text = tblTable.Rows[0]["Ca_LopHocID"].ToString();
                                        
                                    }
                                }
                            }
                            else if (strType == "delete")
                            {
                                int iID = int.Parse(Request.QueryString["id"]);
                                data.dnn_NuceQLHPM_Ca_LopHoc_SinhVien.updateStatus(iID, 4);
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
                else
                {
                    Response.Redirect("/tabid/2125/default.aspx");
                }
            }
            else
            {
                Response.Redirect("/tabid/2125/default.aspx");
            }
        }
        public void bindMay(string mac)
        {
            DataTable dt = data.dnn_NuceQLHPM_Ca_LopHoc_SinhVien.getMay(iCaLopHocID);

            ddlMay.DataValueField = "MacAddress";
            ddlMay.DataTextField = "Ma";
            ddlMay.DataSource = dt;
            ddlMay.DataBind();
            ListItem li = new ListItem("Chưa có máy", "");
            ListItem li1 = new ListItem("Máy hiện tại", mac);
            if(mac!="")
            {
                ddlMay.Items.Insert(0, li);
                ddlMay.Items.Insert(1, li1);
            }
            else
                ddlMay.Items.Insert(0, li);
        }
        private void displayList()
        {
            DataTable tblTable = data.dnn_NuceQLHPM_Ca_LopHoc_SinhVien.getByCaLopHoc(iCaLopHocID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách sinh viên trong lớp học</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable += "<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Mã SV");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Họ và tên");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Máy");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Trạng thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&calophoc={1}'>Thêm mới</a></div>", this.TabId, iCaLopHocID);
            strTable += "</div>";
            strTable += "<div class='nd_b fl'>";
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                strTable += " <div class='nd_b1 fl'>";
                strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", tblTable.Rows[i]["MaSV"].ToString());
                strTable += string.Format("<div class='nd_t222 fl'>{0} {1}</div>", tblTable.Rows[i]["Ho"].ToString(), tblTable.Rows[i]["Ten"].ToString());

                strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", tblTable.Rows[i].IsNull("MaMay")? "Chưa có máy" : tblTable.Rows[i]["MaMay"].ToString());
                
                int iStatus = int.Parse(tblTable.Rows[i]["status"].ToString());
                if (iStatus.Equals(1))
                {
                    strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Bình thường");
                }
                else
                {
                    if (iStatus.Equals(2))
                    {
                        strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Khoá trao đổi");
                    }
                    else
                    {
                        strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Hoàn thành");
                    }
                }
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&calophoc={2}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["Ca_LopHoc_SinhVienID"].ToString(),iCaLopHocID);
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&&calophoc={2}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["Ca_LopHoc_SinhVienID"].ToString(), iCaLopHocID);
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&calophoc={1}'>Thêm mới</a></div>", this.TabId, iCaLopHocID);
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
            int iStatus= int.Parse(ddlTrangThai.SelectedValue);
            string strMac = ddlMay.SelectedValue;
            string strMaSV = txtMaSV.Text.Trim();
            bool blCheck = true;

            if (strMaSV.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để mã sinh viên trắng";
                blCheck = false;
            }
            int iReturn = 0;
            if (blCheck)
            {
                if (strType == "insert")
                {
                    iReturn=data.dnn_NuceQLHPM_Ca_LopHoc_SinhVien.insert(iCaLopHocID, strMaSV, strMac, 1, iStatus);
                    if(iReturn.Equals(-2))
                    {
                        divAnnouce.InnerText = "Không tồn tại mã sinh viên (-2)";
                    }
                    else if(iReturn.Equals(-1))
                    {
                        divAnnouce.InnerText = "Không cập nhật thành công (-1)";
                    }
                    else
                    { 
                        returnMain();
                    }
                }
                else
                {
                    if (strType == "edit")
                    {
                        int iID = int.Parse(txtID.Text);
                        data.dnn_NuceQLHPM_Ca_LopHoc_SinhVien.update(iID, iCaLopHocID, strMaSV, strMac, 1, iStatus);
                        if (iReturn.Equals(-2))
                        {
                            divAnnouce.InnerText = "Không tồn tại mã sinh viên (-2)";
                        }
                        else if (iReturn.Equals(-1))
                        {
                            divAnnouce.InnerText = "Không cập nhật thành công (-1)";
                        }
                        else
                        {
                            returnMain();
                        }
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
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?calophoc={1}", this.TabId,iCaLopHocID));
        }
    }
}