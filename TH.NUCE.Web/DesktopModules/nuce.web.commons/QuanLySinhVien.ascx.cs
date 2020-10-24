using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Globalization;
namespace nuce.web.commons
{
    public partial class QuanLySinhVien : PortalModuleBase
    {
        //http://www.mikesdotnetting.com/article/278/a-better-way-to-export-gridviews-to-excel
        protected override void OnInit(EventArgs e)
        {
            #region Datetime

            lnkNgayBatDau.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtNgayBatDau).Replace("M/d/yyyy", "MM/dd/yyyy").Replace("d/MM/yyyy", "dd/MM/yyyy").Replace("MM/dd/yyyy", "dd/MM/yyyy");
            //Điền dữ liệu vào các control
            imgNgayBatDau.ImageUrl = string.Format("{0}/images/calendar.png", this.ModulePath);

            lnkNgayKetThuc.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtNgayKetThuc).Replace("M/d/yyyy", "MM/dd/yyyy").Replace("d/MM/yyyy", "dd/MM/yyyy").Replace("MM/dd/yyyy", "dd/MM/yyyy");
            //Điền dữ liệu vào các control
            imgNgayKetThuc.ImageUrl = string.Format("{0}/images/calendar.png", this.ModulePath);

            lnkNgaySinh.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtNgaySinh).Replace("M/d/yyyy", "MM/dd/yyyy").Replace("d/MM/yyyy", "dd/MM/yyyy").Replace("MM/dd/yyyy", "dd/MM/yyyy");
            //Điền dữ liệu vào các control
            imgNgaySinh.ImageUrl = string.Format("{0}/images/calendar.png", this.ModulePath);
            #endregion
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ma"] != null && Request.QueryString["ma"] != "")
                {
                    string malop = Request.QueryString["ma"].Trim();
                    txtMaLop.Text = malop;
                    // Kiểm tra xem có lớp
                    int iLopQuanLyID = data.dnn_NuceCommon_LopQuanLy.getIDByMa(malop);
                    if (iLopQuanLyID > 0)
                    {
                        txtLopQuanLyID.Text = iLopQuanLyID.ToString();
                        if (Request.QueryString["EditType"] != null && Request.QueryString["EditType"] != "")
                        {
                            divFilter.Visible = false;
                            string strType = Request.QueryString["EditType"];
                            txtType.Text = strType;
                            divAnnouce.InnerHtml = "";
                            if (strType == "edit" || strType == "insert")
                            {
                                //Lấy dữ liệu từ server để nhét vào các control
                                if (strType == "insert")
                                {
                                    divAnnouce.InnerHtml = "Thêm mới bộ môn";
                                }
                                else
                                {
                                    divAnnouce.InnerHtml = "Chỉnh sửa thông tin của sinh viên";
                                    int iID = int.Parse(Request.QueryString["id"]);
                                    DataTable tblTable = data.dnn_NuceCommon_SinhVien.get(iID);
                                    if (tblTable.Rows.Count > 0)
                                    {
                                        txtMaSV.Text = tblTable.Rows[0]["MaSV"].ToString();
                                        txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                                        txtHo.Text = tblTable.Rows[0]["Ho"].ToString();
                                        txtQueQuan.Text = tblTable.Rows[0]["QueQuan"].ToString();
                                        txtNgaySinh.Text =
                                            DateTime.Parse(tblTable.Rows[0]["NgayThangNamSinh"].ToString())
                                                .ToString("dd/MM/yyyy");
                                        txtNgayBatDau.Text =
                                            DateTime.Parse(tblTable.Rows[0]["NgayBatDau"].ToString()).ToString("dd/MM/yyyy");
                                        txtNgayKetThuc.Text =
                                            DateTime.Parse(tblTable.Rows[0]["NgayKetThuc"].ToString())
                                                .ToString("dd/MM/yyyy");
                                        txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                        txtOrder.Text = tblTable.Rows[0].IsNull("Order") ? "-1" : tblTable.Rows[0]["Order"].ToString();
                                        txtID.Text = iID.ToString();
                                    }
                                }

                            }
                            else if (strType == "delete")
                            {
                                int iID = int.Parse(Request.QueryString["id"]);
                                data.dnn_NuceCommon_SinhVien.updateStatus(iID, 4);
                                returnMain(Request.QueryString["khoaid"]);
                            }
                            else if (strType == "changefinish")
                            {
                                int iID = int.Parse(Request.QueryString["id"]);
                                data.dnn_NuceCommon_SinhVien.updateStatus(iID, 3);
                                returnMain(Request.QueryString["khoaid"]);
                            }
                            else if (strType == "changeactive")
                            {
                                int iID = int.Parse(Request.QueryString["id"]);
                                data.dnn_NuceCommon_SinhVien.updateStatus(iID, 1);
                                returnMain(Request.QueryString["khoaid"]);
                            }
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "";
                            divEdit.Visible = false;
                            displayList(malop);
                        }
                    }
                    else
                    {
                        divAnnouce.InnerHtml = "Không tồn tại lớp có mã này đề nghị bạn chọn lại !";
                        divEdit.Visible = false;
                    }
                }
                else
                {
                    divAnnouce.InnerHtml = "Không có mã !";
                    divEdit.Visible = false;
                }
            }
        }

        private void displayList(string malop)
        {
            DataTable tblTable = data.dnn_NuceCommon_SinhVien.getByMaLopQuanLy(malop, -1, -1);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách lớp</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable += "<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "Mã SV");
            strTable += string.Format("<div class='nd_t22 fl'>{0}</div>", "Họ và Tên");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Thứ tự");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Có vân tay");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Trạng Thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chuyển");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&ma={1}'>Thêm mới</a></div>", this.TabId,txtMaLop.Text);
            strTable += "</div>";
            strTable += "<div class='nd_b fl'>";
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                strTable += " <div class='nd_b1 fl'>";
                strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", tblTable.Rows[i]["MaSV"].ToString());
                strTable += string.Format("<div class='nd_t22 fl' >{0} {1}</div>", tblTable.Rows[i]["Ho"].ToString(), tblTable.Rows[i]["Ten"].ToString());
                strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", tblTable.Rows[i].IsNull("Order")?-1:int.Parse(tblTable.Rows[i]["Order"].ToString()));
                if (tblTable.Rows[i].IsNull("InsertedUser"))
                    strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chưa có");
                else
                    strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Đã có");

                int iStatus = int.Parse(tblTable.Rows[i]["status"].ToString());
                if (iStatus.Equals(1))
                {
                    strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Hoạt động");
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}&&ma={2}'>Kết thúc</a></div>", this.TabId, tblTable.Rows[i]["SinhVienID"].ToString(), txtMaLop.Text);
                }
                else
                {
                    strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Kết thúc");
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}&&ma={2}'>Hoạt động</a></div>", this.TabId, tblTable.Rows[i]["SinhVienID"].ToString(), txtMaLop.Text);
                }

                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&ma={2}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["SinhVienID"].ToString(), txtMaLop.Text);
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&ma={2}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["SinhVienID"].ToString(), txtMaLop.Text);
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&ma={1}'>Thêm mới</a></div>", this.TabId,txtMaLop.Text);
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
            string strMaSV = txtMaSV.Text;
            string strTen = txtTen.Text;
            string strHo = txtHo.Text;
            string strQueQuan = txtQueQuan.Text;
            int iOrder = 1;
            int.TryParse(txtOrder.Text.Trim(), out iOrder);

            DateTime dtNgaySinh;
            try
            {
                dtNgaySinh = DateTime.ParseExact(txtNgaySinh.Text, "dd/mm/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                dtNgaySinh = DateTime.Now;
            }

            string strMoTa = txtMoTa.Text;

            DateTime dtNgayBatDau;
            try
            {
                dtNgayBatDau = DateTime.ParseExact(txtNgayBatDau.Text, "dd/mm/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                dtNgayBatDau = DateTime.Now;
            }
            DateTime dtNgayKetThuc;
            try
            {
                dtNgayKetThuc = DateTime.ParseExact(txtNgayKetThuc.Text, "dd/mm/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                dtNgayKetThuc = DateTime.Now;
            }

            int iMaLopQuanLy = int.Parse(txtLopQuanLyID.Text)
            ;
            if (strMaSV.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để mã trắng";
                return;
            }
            if (strTen.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên trắng";
                return;
            }
            int iLopQuanLy = int.Parse(txtLopQuanLyID.Text);
            if (strType == "insert")
            {
                data.dnn_NuceCommon_SinhVien.insert(iLopQuanLy, strMaSV, strTen, strHo, strQueQuan, dtNgaySinh, strMoTa,
                    dtNgayBatDau, dtNgayKetThuc,iOrder);
                //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                returnMain(txtMaLop.Text);
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceCommon_SinhVien.update(iID, iLopQuanLy, strMaSV, strTen, strHo, strQueQuan, dtNgaySinh, strMoTa,
                    dtNgayBatDau, dtNgayKetThuc, iOrder);
                    returnMain(txtMaLop.Text);
                }
            }
        }

        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtMaLop.Text)
            ;
        }
        private void returnMain(string malop)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?ma={1}", this.TabId, malop));
        }
        protected void btnTimLop_Click(object sender, EventArgs e)
        {
            returnMain(txtMaLop.Text.Trim());
        }
    }
}