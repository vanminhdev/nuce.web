using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.qlhpm
{
    public partial class QuanLyCaHoc : PortalModuleBase
    {
        private int m_iBlockHocID;
        private int m_NguoiDung_MonHocID;
        protected override void OnInit(EventArgs e)
        {
            #region datetime

            lnkNgay.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtNgay).Replace("M/d/yyyy", "MM/dd/yyyy").Replace("d/MM/yyyy", "dd/MM/yyyy").Replace("MM/dd/yyyy", "dd/MM/yyyy");
            //Điền dữ liệu vào các control
            imgNgay.ImageUrl = string.Format("{0}/images/calendar.png", this.ModulePath);
            #endregion
            #region dropdownlist
            for(int i=1;i<25;i++)
            {
                ddlGioBatDau.Items.Add(i.ToString());
                ddlGioKetThuc.Items.Add(i.ToString());
            }
            for (int i = 1; i < 61; i++)
            {
                ddlPhutBatDau.Items.Add(i.ToString());
                ddlPhutKetThuc.Items.Add(i.ToString());
            }

            DataTable dtBlockHoc = data.dnn_NuceCommon_BlockHoc.getNameActive();
            ddlBlockHoc.DataValueField = "BlockHocID";
            ddlBlockHoc.DataTextField = "TenDayDu";
            ddlBlockHoc.DataSource = dtBlockHoc;
            ddlBlockHoc.DataBind();
            if (dtBlockHoc.Rows.Count > 0)
                m_iBlockHocID = int.Parse(dtBlockHoc.Rows[0]["BlockHocID"].ToString());
            else
                m_iBlockHocID = -1;

            DataTable tblTable = data.dnn_NuceCommon_MonHoc.getByNguoiDung(this.UserId);
            ddlNguoiDung_MonHoc.DataSource = tblTable;
            ddlNguoiDung_MonHoc.DataTextField = "Ten";
            ddlNguoiDung_MonHoc.DataValueField = "NguoiDung_MonHocID";
            ddlNguoiDung_MonHoc.DataBind();
            if (tblTable.Rows.Count > 0)
            {
                m_NguoiDung_MonHocID = int.Parse(tblTable.Rows[0]["NguoiDung_MonHocID"].ToString());
            }
            else
                m_NguoiDung_MonHocID = -1;
            #endregion
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["NguoiDung_MonHocid"] == null || Request.QueryString["blockhocid"] == null)
                {
                    Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}&&blockhocid={2}", this.TabId, m_NguoiDung_MonHocID, m_iBlockHocID));
                    return;
                }
                else
                {
                    m_iBlockHocID = int.Parse(Request.QueryString["blockhocid"]);
                    m_NguoiDung_MonHocID = int.Parse(Request.QueryString["NguoiDung_MonHocid"]);
                    ddlBlockHoc.SelectedValue = m_iBlockHocID.ToString();
                    ddlNguoiDung_MonHoc.SelectedValue = m_NguoiDung_MonHocID.ToString();
                }
                if (Request.QueryString["EditType"] != null && Request.QueryString["EditType"] != "")
                {
                    divFilter.Visible = false;
                    string strType = Request.QueryString["EditType"];
                    txtType.Text = strType;
                    divAnnouce.InnerHtml = "";
                    if (strType == "edit" || strType == "insert")
                    {
                        //Lấy dữ liệu từ server để nhét vào các control
                        //THCore_CM_GetTag
                        lblInfoEdit.Text = string.Format("Môn học {0} - block học {1}", ddlNguoiDung_MonHoc.SelectedItem.Text, ddlBlockHoc.SelectedItem.Text);
                        txtNguoiDung_MonHocID.Text = m_NguoiDung_MonHocID.ToString();
                        txtBlockHocID.Text = m_iBlockHocID.ToString();
                        if (strType == "insert")
                        {
                            divAnnouce.InnerHtml = "Thêm mới ca học";
                            trAddLopHoc.Visible = false;
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin ca học";
                            trAddLopHoc.Visible = true;
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceQLHPM_CaHoc.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {
                                txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                                txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                txtStatus.Text= tblTable.Rows[0]["Status"].ToString();
                                txtNgay.Text= DateTime.Parse(tblTable.Rows[0]["Ngay"].ToString()).ToString("dd/MM/yyyy");
                                ddlBaoMat.SelectedValue = tblTable.Rows[0]["Type"].ToString();
                                ddlGioBatDau.SelectedValue= tblTable.Rows[0]["GioBatDau"].ToString();
                                ddlPhutBatDau.SelectedValue= tblTable.Rows[0]["PhutBatDau"].ToString();
                                ddlGioKetThuc.SelectedValue= tblTable.Rows[0]["GioKetThuc"].ToString();
                                ddlPhutKetThuc.SelectedValue= tblTable.Rows[0]["PhutKetThuc"].ToString();
                                txtID.Text = iID.ToString();
                                bindDataDanhSachLopHocTrongKiThi(iID);
                                bindDataDanhSachLopHocChuaTrongKiThi(iID);
                            }
                        }

                    }
                    else if (strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceQLHPM_CaHoc.updateStatus(iID, 4);
                        returnMain(m_NguoiDung_MonHocID.ToString(), m_iBlockHocID.ToString());
                    }
                    else if (strType == "changefinish")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceQLHPM_CaHoc.updateStatus(iID, 3);
                        returnMain(m_NguoiDung_MonHocID.ToString(), m_iBlockHocID.ToString());
                    }
                    else if (strType == "changebegin")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceQLHPM_CaHoc.updateStatus(iID, 1);
                        returnMain(m_NguoiDung_MonHocID.ToString(), m_iBlockHocID.ToString());
                    }
                    else if (strType == "changeactive")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceQLHPM_CaHoc.updateStatus(iID, 2);
                        returnMain(m_NguoiDung_MonHocID.ToString(), m_iBlockHocID.ToString());
                    }
                }
                else
                {
                    //Bindata
                    divAnnouce.InnerHtml = "";
                    divEdit.Visible = false;
                    displayList(m_NguoiDung_MonHocID, m_iBlockHocID);
                }
            }
        }

        private void displayList(int NguoiDung_MonHocID, int BlockHocID)
        {
            DataTable tblTable = data.dnn_NuceQLHPM_CaHoc.getByNguoiDungMonHocAndBlockHoc(NguoiDung_MonHocID, BlockHocID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách ca học ("+ string.Format("<a href='/tabid/{0}/default.aspx?EditType=insert&&blockhocid={1}&&NguoiDung_MonHocid={2}'>Thêm mới</a>", this.TabId, BlockHocID, NguoiDung_MonHocID)+")</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable += "<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Mô tả");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Ngày");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Trạng thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chuyển ");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Đợt trao đổi");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += "</div>";
            strTable += "<div class='nd_b fl'>";
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                strTable += " <div class='nd_b1 fl'>";
                strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["Ten"].ToString());

                strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", tblTable.Rows[i]["MoTa"].ToString());
                strTable += string.Format("<div class='nd_t222 fl'>{0} (<b>{1}h:{2}mm</b> Đến <b>{3}h:{4}mm</b>)</span></div>", DateTime.Parse(tblTable.Rows[i]["Ngay"].ToString()).ToString("dd/MM/yyyy"),
                    tblTable.Rows[i]["GioBatDau"].ToString()
                    , tblTable.Rows[i]["PhutBatDau"].ToString()
                    , tblTable.Rows[i]["GioKetThuc"].ToString()
                    , tblTable.Rows[i]["PhutKetThuc"].ToString());
                int iStatus = int.Parse(tblTable.Rows[i]["status"].ToString());
                if (iStatus.Equals(1))
                {
                    strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Gốc");
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}&&blockhocid={2}&&NguoiDung_MonHocid={3}'>Học</a></div>", this.TabId, tblTable.Rows[i]["CaHocID"].ToString(), m_iBlockHocID, m_NguoiDung_MonHocID);
                    strTable += string.Format("<div class='nd_t4 fl'>---</div>");
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&blockhocid={2}&&NguoiDung_MonHocid={3}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["CaHocID"].ToString(), m_iBlockHocID, m_NguoiDung_MonHocID);
                }
                else
                {
                    if (iStatus.Equals(2))
                    {
                        strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Học");
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}&&blockhocid={2}&&NguoiDung_MonHocid={3}'>Kết thúc học</a></div>", this.TabId, tblTable.Rows[i]["CaHocID"].ToString(), m_iBlockHocID, m_NguoiDung_MonHocID);
                        strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'><a href='/tabid/2126/default.aspx?cahoc={0}'>Đợt trao đổi</a></div>", tblTable.Rows[i]["CaHocID"].ToString());

                    }
                    else
                    {
                        strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Kết thúc học");
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changebegin&&id={1}&&blockhocid={2}&&NguoiDung_MonHocid={3}'>Gốc</a></div>", this.TabId, tblTable.Rows[i]["CaHocID"].ToString(), m_iBlockHocID, m_NguoiDung_MonHocID);
                        strTable += string.Format("<div class='nd_t4 fl'>---</div>");
                    }
                    //strTable += string.Format("<div class='nd_t4 fl'>---</div>");
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&blockhocid={2}&&NguoiDung_MonHocid={3}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["CaHocID"].ToString(), m_iBlockHocID, m_NguoiDung_MonHocID);
                }

                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&&blockhocid={2}&&NguoiDung_MonHocid={3}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["CaHocID"].ToString(), m_iBlockHocID, m_NguoiDung_MonHocID);
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&blockhocid={1}&&NguoiDung_MonHocid={2}'>Thêm mới</a></div>", this.TabId, m_iBlockHocID, m_NguoiDung_MonHocID);
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
            int NguoiDungMonHocID = int.Parse(txtNguoiDung_MonHocID.Text);
            int BlockHocID = int.Parse(txtBlockHocID.Text);
            ;
            if (strTen.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên trắng";
                return;
            }
            DateTime dtNgay;
            try
            {
                dtNgay = DateTime.ParseExact(txtNgay.Text, "dd/MM/yyyy", null);
            }
            catch (Exception)
            {
                dtNgay = DateTime.Now;
            }
            int iGioBatDau = int.Parse(ddlGioBatDau.SelectedValue);
            int iPhutBatDau = int.Parse(ddlPhutBatDau.SelectedValue);
            int iGioKetThuc = int.Parse(ddlGioKetThuc.SelectedValue);
            int iPhutKetThuc = int.Parse(ddlPhutKetThuc.SelectedValue);
            int iType = int.Parse(ddlBaoMat.SelectedValue);
            if (strType == "insert")
            {
                data.dnn_NuceQLHPM_CaHoc.insert(NguoiDungMonHocID, BlockHocID, strTen, strMoTa,dtNgay, iGioBatDau, iPhutBatDau, iGioKetThuc, iPhutKetThuc, iType);
                returnMain();
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceQLHPM_CaHoc.update(iID, NguoiDungMonHocID, BlockHocID, strTen, strMoTa,dtNgay, iGioBatDau, iPhutBatDau, iGioKetThuc, iPhutKetThuc, iType);
                    returnMain();
                }
            }
        }

        protected void btnQLSV_Click(object sender, EventArgs e)
        {
            if (lsbDanhSachLopHocTrongKiThi.SelectedIndex >= 0)
            {
                int Ca_LopHocID = int.Parse(lsbDanhSachLopHocTrongKiThi.SelectedValue);
                Response.Redirect(string.Format("/tabid/{0}/default.aspx?calophoc={1}", 2128, Ca_LopHocID));
            }
            else
                divAnnounce1.InnerHtml = "Chưa chọn lớp học nào !!!";
        }
        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtNguoiDung_MonHocID.Text, txtBlockHocID.Text)
            ;
        }
        protected void returnMain()
        {
            returnMain(txtNguoiDung_MonHocID.Text, txtBlockHocID.Text)
           ;
        }
        private void returnMain(string NguoiDungMonHocID, string BlockHocID)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?blockhocid={1}&&NguoiDung_MonHocid={2}", this.TabId, BlockHocID, NguoiDungMonHocID));
        }

        protected void ddlNguoiDung_MonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnMain(ddlNguoiDung_MonHoc.SelectedValue, ddlBlockHoc.SelectedValue);
        }

        protected void ddlBlockHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnMain(ddlNguoiDung_MonHoc.SelectedValue, ddlBlockHoc.SelectedValue);
        }

        protected void btnChuyenPhaiQuaTrai_Click(object sender, EventArgs e)
        {
            int iStatus = int.Parse(txtStatus.Text);
            if (iStatus > 1)
            {
                divAnnounce1.InnerHtml = "Lớp học đã đang học nên không thay đổi !!!";
            }
            else
            {
                if (lsbDanhSachLopHocTrongKiThi.SelectedIndex >= 0)
                {
                    int Ca_LopHocID = int.Parse(lsbDanhSachLopHocTrongKiThi.SelectedValue);
                    data.dnn_NuceQLHPM_Ca_LopHoc.delete(Ca_LopHocID);
                    int iCaHocID = int.Parse(txtID.Text);
                    bindDataDanhSachLopHocTrongKiThi(iCaHocID);
                    bindDataDanhSachLopHocChuaTrongKiThi(iCaHocID);
                    //divAnnounce1.InnerHtml = string.Format("{0} - {1} - {2}", strIDLopHoc, strMoTa, strPhongThi);
                }
                else
                    divAnnounce1.InnerHtml = "Chưa chọn lớp học nào !!!";
            }
        }
        protected void btnChuyenTraiQuanPhai_Click(object sender, EventArgs e)
        {
            int iStatus = int.Parse(txtStatus.Text);
            if (iStatus > 2)
            {
                divAnnounce1.InnerHtml = "Kì thi đã kết thúc !!!";
            }
            else
            {
                if (lsbDanhSachLopHocChuaTrongKiThi.SelectedIndex >= 0)
                {
                    int iLopHocID = int.Parse(lsbDanhSachLopHocChuaTrongKiThi.SelectedValue);
                    string strMoTa = txtMoTaLopHocTrongKiThi.Text;
                    string strPhongThi = ddlPhongHoc.SelectedValue;// txtPhongThi.Text;
                    int iCaHocID = int.Parse(txtID.Text);
                    data.dnn_NuceQLHPM_Ca_LopHoc.insert(iCaHocID, iLopHocID, strPhongThi, strMoTa);
                    bindDataDanhSachLopHocTrongKiThi(iCaHocID);
                    bindDataDanhSachLopHocChuaTrongKiThi(iCaHocID);
                    //divAnnounce1.InnerHtml = string.Format("{0} - {1} - {2}", strIDLopHoc, strMoTa, strPhongThi);
                }
                else
                    divAnnounce1.InnerHtml = "Chưa chọn lớp học nào !!!";
            }
        }
        private void bindDataDanhSachLopHocTrongKiThi(int CaHocID)
        {
            DataTable dtLopHocIn = data.dnn_NuceQLHPM_Ca_LopHoc.getByCaHoc(CaHocID);
            lsbDanhSachLopHocTrongKiThi.DataTextField = "Ten1";
            lsbDanhSachLopHocTrongKiThi.DataValueField = "Ca_LopHocID";
            lsbDanhSachLopHocTrongKiThi.DataSource = dtLopHocIn;
            lsbDanhSachLopHocTrongKiThi.DataBind();
            if(dtLopHocIn.Rows.Count>0)
            {
                lsbDanhSachLopHocTrongKiThi.SelectedIndex = 0;
            }
        }
        private void bindDataDanhSachLopHocChuaTrongKiThi(int CaHocID)
        {
            DataTable dtLopHocNotIn = data.dnn_NuceQLHPM_Ca_LopHoc.getNotInByCaHoc(CaHocID);
            lsbDanhSachLopHocChuaTrongKiThi.DataTextField = "Ten";
            lsbDanhSachLopHocChuaTrongKiThi.DataValueField = "LopHocID";
            lsbDanhSachLopHocChuaTrongKiThi.DataSource = dtLopHocNotIn;
            lsbDanhSachLopHocChuaTrongKiThi.DataBind();
        }
    }
}