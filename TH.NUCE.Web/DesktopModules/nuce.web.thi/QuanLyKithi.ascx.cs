using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.thi
{
    public partial class QuanLyKithi : PortalModuleBase
    {
        private int m_iBlockHocID;
        private int m_NguoiDung_MonHocID;
        protected override void OnInit(EventArgs e)
        {
            #region dropdownlist
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
                        DataTable dtBoDe = data.dnn_NuceThi_BoDe.getNameBoDeDungThiByNguoiDung_MonHoc(m_NguoiDung_MonHocID);
                        ddlBoDe.DataTextField = "MaF";
                        ddlBoDe.DataValueField = "BoDeID";
                        ddlBoDe.DataSource = dtBoDe;
                        ddlBoDe.DataBind();
                        txtNguoiDung_MonHocID.Text = m_NguoiDung_MonHocID.ToString();
                        txtBlockHocID.Text = m_iBlockHocID.ToString();
                        if (strType == "insert")
                        {
                            divAnnouce.InnerHtml = "Thêm mới kì thi";
                            trAddLopHoc.Visible = false;
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin kì thi";
                            trAddLopHoc.Visible = true;
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceThi_KiThi.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {
                                txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                                txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                ddlBoDe.SelectedValue = tblTable.Rows[0]["BoDeID"].ToString();
                                ddlBaoMat.SelectedValue = tblTable.Rows[0]["Type"].ToString();
                                txtStatus.Text= tblTable.Rows[0]["Status"].ToString();
                                txtID.Text = iID.ToString();
                                bindDataDanhSachLopHocTrongKiThi(iID);
                                bindDataDanhSachLopHocChuaTrongKiThi(iID);
                            }
                        }

                    }
                    else if (strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceThi_KiThi.updateStatus(iID, 4);
                        returnMain(m_NguoiDung_MonHocID.ToString(), m_iBlockHocID.ToString());
                    }
                    else if (strType == "changefinish")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        Utils.ketThucKiThi(iID);
                        data.dnn_NuceThi_KiThi.updateStatus(iID, 3);
                        returnMain(m_NguoiDung_MonHocID.ToString(), m_iBlockHocID.ToString());
                    }
                    else if (strType == "changebegin")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceThi_KiThi.updateStatus(iID, 1);
                        returnMain(m_NguoiDung_MonHocID.ToString(), m_iBlockHocID.ToString());
                    }
                    else if (strType == "changeactive")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        Utils.chuyenThi(iID);
                        data.dnn_NuceThi_KiThi.updateStatus(iID, 2);
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
            DataTable tblTable = data.dnn_NuceThi_KiThi.getByNguoiDungMonHocAndBlockHoc(NguoiDung_MonHocID, BlockHocID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách kì thi</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable += "<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Bộ đề");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Loại");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Mô tả");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Trạng thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chuyển ");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&blockhocid={1}&&NguoiDung_MonHocid={2}'>Thêm mới</a></div>", this.TabId, BlockHocID, NguoiDung_MonHocID);
            strTable += "</div>";
            strTable += "<div class='nd_b fl'>";
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                strTable += " <div class='nd_b1 fl'>";
                strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["Ten"].ToString());
                strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["BoDe"].ToString());
                int iType = int.Parse(tblTable.Rows[i]["Type"].ToString());
                if (iType.Equals(1))
                {
                    strTable += string.Format("<div class='nd_t4 fl'>Bình thường</div>");
                }
                else
                    strTable += string.Format("<div class='nd_t4 fl'>Bảo mật</div>");

                strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", tblTable.Rows[i]["MoTa"].ToString());
                int iStatus = int.Parse(tblTable.Rows[i]["status"].ToString());
                if (iStatus.Equals(1))
                {
                    strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Gốc");
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}&&blockhocid={2}&&NguoiDung_MonHocid={3}'>Thi</a></div>", this.TabId, tblTable.Rows[i]["KiThiID"].ToString(), m_iBlockHocID, m_NguoiDung_MonHocID);
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&blockhocid={2}&&NguoiDung_MonHocid={3}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["KiThiID"].ToString(), m_iBlockHocID, m_NguoiDung_MonHocID);
                }
                else
                {
                    if (iStatus.Equals(2))
                    {
                        strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Thi");
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}&&blockhocid={2}&&NguoiDung_MonHocid={3}'>Kết thúc thi</a></div>", this.TabId, tblTable.Rows[i]["KiThiID"].ToString(), m_iBlockHocID, m_NguoiDung_MonHocID);
                    }
                    else
                    {
                        strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Kết thúc thi");
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changebegin&&id={1}&&blockhocid={2}&&NguoiDung_MonHocid={3}'>bắt đầu</a></div>", this.TabId, tblTable.Rows[i]["KiThiID"].ToString(), m_iBlockHocID, m_NguoiDung_MonHocID);
                    }
                    strTable += string.Format("<div class='nd_t4 fl'>---</div>");
                }

                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&&blockhocid={2}&&NguoiDung_MonHocid={3}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["KiThiID"].ToString(), m_iBlockHocID, m_NguoiDung_MonHocID);
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
            int iBodeID = int.Parse(ddlBoDe.SelectedValue);
            int iType = int.Parse(ddlBaoMat.SelectedValue);
            int NguoiDungMonHocID = int.Parse(txtNguoiDung_MonHocID.Text);
            int BlockHocID = int.Parse(txtBlockHocID.Text);
            ;
            if (strTen.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên trắng";
                return;
            }

            if (strType == "insert")
            {
                data.dnn_NuceThi_KiThi.insert1(NguoiDungMonHocID, BlockHocID, iBodeID, strTen, strMoTa,iType);
                returnMain();
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceThi_KiThi.update1(iID, NguoiDungMonHocID, BlockHocID, iBodeID, strTen, strMoTa, iType);
                    returnMain();
                }
            }
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
                divAnnounce1.InnerHtml = "Lớp học đã hoặc đang thi nên không thay đổi !!!";
            }
            else
            {
                if (lsbDanhSachLopHocTrongKiThi.SelectedIndex >= 0)
                {
                    int KiThi_LopHocID = int.Parse(lsbDanhSachLopHocTrongKiThi.SelectedValue);
                    data.dnn_NuceThi_KiThi_LopHoc.delete(KiThi_LopHocID);
                    int iKiThiID = int.Parse(txtID.Text);
                    bindDataDanhSachLopHocTrongKiThi(iKiThiID);
                    bindDataDanhSachLopHocChuaTrongKiThi(iKiThiID);
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
                    string strPhongThi = txtPhongThi.Text;
                    int iKiThiID = int.Parse(txtID.Text);
                    data.dnn_NuceThi_KiThi_LopHoc.insert(iKiThiID, iLopHocID, strPhongThi, strMoTa);
                    bindDataDanhSachLopHocTrongKiThi(iKiThiID);
                    bindDataDanhSachLopHocChuaTrongKiThi(iKiThiID);
                    //divAnnounce1.InnerHtml = string.Format("{0} - {1} - {2}", strIDLopHoc, strMoTa, strPhongThi);
                }
                else
                    divAnnounce1.InnerHtml = "Chưa chọn lớp học nào !!!";
            }
        }
        private void bindDataDanhSachLopHocTrongKiThi(int KiThiID)
        {
            DataTable dtLopHocIn = data.dnn_NuceThi_KiThi_LopHoc.getByKiThi(KiThiID);
            lsbDanhSachLopHocTrongKiThi.DataTextField = "Ten1";
            lsbDanhSachLopHocTrongKiThi.DataValueField = "KiThi_LopHocID";
            lsbDanhSachLopHocTrongKiThi.DataSource = dtLopHocIn;
            lsbDanhSachLopHocTrongKiThi.DataBind();
            if(dtLopHocIn.Rows.Count>0)
            {
                lsbDanhSachLopHocTrongKiThi.SelectedIndex = 0;
            }
        }
        private void bindDataDanhSachLopHocChuaTrongKiThi(int KiThiID)
        {
            DataTable dtLopHocNotIn = data.dnn_NuceThi_KiThi_LopHoc.getNotInByKiThi(KiThiID);
            lsbDanhSachLopHocChuaTrongKiThi.DataTextField = "Ten";
            lsbDanhSachLopHocChuaTrongKiThi.DataValueField = "LopHocID";
            lsbDanhSachLopHocChuaTrongKiThi.DataSource = dtLopHocNotIn;
            lsbDanhSachLopHocChuaTrongKiThi.DataBind();
        }
    }
}