using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.commons
{
    public partial class QuanLyNamHoc : PortalModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            #region Datetime

            lnkNgayBatDau.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtNgayBatDau).Replace("M/d/yyyy", "MM/dd/yyyy").Replace("d/MM/yyyy", "dd/MM/yyyy").Replace("MM/dd/yyyy", "dd/MM/yyyy");
            //Điền dữ liệu vào các control
            imgNgayBatDau.ImageUrl = string.Format("{0}/images/calendar.png", this.ModulePath);

            lnkNgayKetThuc.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtNgayKetThuc).Replace("M/d/yyyy", "MM/dd/yyyy").Replace("d/MM/yyyy", "dd/MM/yyyy").Replace("MM/dd/yyyy", "dd/MM/yyyy");
            //Điền dữ liệu vào các control
            imgNgayKetThuc.ImageUrl = string.Format("{0}/images/calendar.png", this.ModulePath);
            #endregion
        }

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
                            divAnnouce.InnerHtml = "Thêm mới năm học";
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin của năm học";
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceCommon_NamHoc.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {
                                txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                                txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                txtNgayBatDau.Text = DateTime.Parse(tblTable.Rows[0]["NgayBatDau"].ToString()).ToString("dd/MM/yyyy");
                                txtNgayKetThuc.Text = DateTime.Parse(tblTable.Rows[0]["NgayKetThuc"].ToString()).ToString("dd/MM/yyyy");
                                txtID.Text = iID.ToString();
                            }
                        }
                    }
                    else if (strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_NamHoc.updateStatus(iID, 4);
                        returnMain();
                    }
                    else if (strType == "changefinish")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_NamHoc.updateStatus(iID, 3);
                        returnMain();
                    }
                    else if (strType == "changeactive")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_NamHoc.updateStatus(iID, 1);
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
            DataTable tblTable = data.dnn_NuceCommon_NamHoc.get(-1);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách năm học</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable += "<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Ngày bắt đầu");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Ngày kết thúc");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Mô tả");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Trạng thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chuyển ");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Thêm mới");
            strTable += "</div>";
            strTable += "<div class='nd_b fl'>";
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                strTable += " <div class='nd_b1 fl'>";
                strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["Ten"].ToString());
                strTable += string.Format("<div class='nd_t33 fl'>{0:dd/MM/yyyy}</div>", DateTime.Parse(tblTable.Rows[i]["NgayBatDau"].ToString()));
                strTable += string.Format("<div class='nd_t33 fl'>{0:dd/MM/yyyy}</div>", DateTime.Parse(tblTable.Rows[i]["NgayKetThuc"].ToString()));
                strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", tblTable.Rows[i]["MoTa"].ToString());
                int iStatus = int.Parse(tblTable.Rows[i]["status"].ToString());
                if (iStatus.Equals(1))
                {
                    strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Hoạt động");
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}'>Kết thúc</a></div>", this.TabId, tblTable.Rows[i]["NamHocID"].ToString());
                }
                else
                {
                    strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Kết thúc");
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}'>Hoạt động</a></div>", this.TabId, tblTable.Rows[i]["NamHocID"].ToString());
                }
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["NamHocID"].ToString());
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["NamHocID"].ToString());
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
            DateTime dtNgayBatDau;
            try
            {
                dtNgayBatDau = DateTime.ParseExact(txtNgayBatDau.Text, "dd/MM/yyyy", null);
            }
            catch (Exception)
            {
                dtNgayBatDau = DateTime.Now;
            }
            DateTime dtNgayKetThuc;
            try
            {
                dtNgayKetThuc = DateTime.ParseExact(txtNgayKetThuc.Text, "dd/MM/yyyy", null);
            }
            catch (Exception)
            {
                dtNgayKetThuc = DateTime.Now;
            }
            int iTruongID = int.Parse(ddlTruong.SelectedValue.ToString())
            ;
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
                    data.dnn_NuceCommon_NamHoc.insert( strTen,  strMoTa,dtNgayBatDau,dtNgayKetThuc);
                    //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                    returnMain();
                }
                else
                {
                    if (strType == "edit")
                    {
                        int iID = int.Parse(txtID.Text);
                        data.dnn_NuceCommon_NamHoc.update(iID, strTen,  strMoTa,dtNgayBatDau,dtNgayKetThuc);
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