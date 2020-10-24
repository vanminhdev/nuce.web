using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.qlhpm
{
    public partial class QuanLyDotTraoDoi : PortalModuleBase
    {
        int iCaHocID;
        protected override void OnInit(EventArgs e)
        {
            #region dropdownlist
            for (int i = 1; i < 13; i++)
            {
                ddlGioBatDau.Items.Add(i.ToString());
                ddlGioKetThuc.Items.Add(i.ToString());
            }
            for (int i = 1; i < 61; i++)
            {
                ddlPhutBatDau.Items.Add(i.ToString());
                ddlPhutKetThuc.Items.Add(i.ToString());
            }

            #endregion
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["cahoc"] != null && Request.QueryString["cahoc"] != "" && int.TryParse(Request.QueryString["cahoc"], out iCaHocID))
            {
                // Lấy thông tin của ca học
                DataTable dtCaHoc = nuce.web.data.dnn_NuceQLHPM_CaHoc.getName(iCaHocID);
                if (dtCaHoc.Rows.Count > 0)
                {
                    txtCaHoc.Text = iCaHocID.ToString();
                    displayCa(dtCaHoc);
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
                                    divAnnouce.InnerHtml = "Thêm mới đợt trao đổi";
                                }
                                else
                                {
                                    divAnnouce.InnerHtml = "Chỉnh sửa thông tin của đợt trao đổi";
                                    int iID = int.Parse(Request.QueryString["id"]);
                                    DataTable tblTable = data.dnn_NuceQLHPM_CaHoc_DotTraoDoi.get(iID);
                                    if (tblTable.Rows.Count > 0)
                                    {
                                        txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                                        txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                        ddlLoai.SelectedValue = tblTable.Rows[0]["Type"].ToString();
                                        ddlTrangThai.SelectedValue= tblTable.Rows[0]["Status"].ToString();
                                        txtFileChoPhep.Text = tblTable.Rows[0]["FileChoPhep"].ToString();
                                        txtDungLuong.Text= tblTable.Rows[0].IsNull("DungLuongToiDa")?"100000": tblTable.Rows[0]["DungLuongToiDa"].ToString();
                                        ddlGioBatDau.SelectedValue = tblTable.Rows[0]["GioBatDau"].ToString();
                                        ddlPhutBatDau.SelectedValue = tblTable.Rows[0]["PhutBatDau"].ToString();
                                        ddlGioKetThuc.SelectedValue = tblTable.Rows[0]["GioKetThuc"].ToString();
                                        ddlPhutKetThuc.SelectedValue = tblTable.Rows[0]["PhutKetThuc"].ToString();
                                        txtID.Text = iID.ToString();
                                        txtCaHoc.Text = tblTable.Rows[0]["CaHocID"].ToString();
                                    }
                                }
                            }
                            else if (strType == "delete")
                            {
                                int iID = int.Parse(Request.QueryString["id"]);
                                data.dnn_NuceQLHPM_CaHoc_DotTraoDoi.updateStatus(iID, 4);
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
        private void displayCa(DataTable dt)
        {
            divCaHoc.InnerText = string.Format("Học ky {0} - Môn học {1} - Ca học {2} - Ngày {3} ({4}h:{5} phút --> {6}h:{7} phút)", dt.Rows[0]["TenHocKy"].ToString(), dt.Rows[0]["TenMonHoc"].ToString(), dt.Rows[0]["Ten"].ToString()
                ,DateTime.Parse(dt.Rows[0]["Ngay"].ToString()).ToString("dd/MM/yyyy"), dt.Rows[0]["GioBatDau"].ToString(), dt.Rows[0]["PhutBatDau"].ToString(), dt.Rows[0]["GioKetThuc"].ToString(), dt.Rows[0]["PhutKetThuc"].ToString());
        }
        private void displayList()
        {
            DataTable tblTable = data.dnn_NuceQLHPM_CaHoc_DotTraoDoi.getByCa(iCaHocID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách đợt trao đổi ("
                              + string.Format("<a href='/tabid/{0}/default.aspx?EditType=insert&&cahoc={1}'>Thêm mới</a> - ", this.TabId, iCaHocID)
                              + string.Format("<a href='/tabid/{0}/default.aspx'>Quay lại</a>", 2125)
                              + ")</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable += "<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Mô tả");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Thời gian");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "File cho phép");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "File tối đa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Quản lý");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Trạng thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'></div>");
            strTable += "</div>";
            strTable += "<div class='nd_b fl'>";
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                strTable += " <div class='nd_b1 fl'>";
                strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["Ten"].ToString());
                strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["MoTa"].ToString());
                strTable += string.Format("<div class='nd_t33 fl'><b>{0}h:{1}</b>-<b>{2}h:{3}</b></span></div>",
                   tblTable.Rows[i]["GioBatDau"].ToString()
                   , tblTable.Rows[i]["PhutBatDau"].ToString()
                   , tblTable.Rows[i]["GioKetThuc"].ToString()
                   , tblTable.Rows[i]["PhutKetThuc"].ToString());
                strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["FileChoPhep"].ToString());
                strTable += string.Format("<div class='nd_t33 fl'>{0} B</div>", tblTable.Rows[i].IsNull("DungLuongToiDa") ? "" : tblTable.Rows[i]["DungLuongToiDa"].ToString());

                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?CaHoc_DotTraoDoiID={1}&&cahoc={2}'>Quản lý</a></div>", 2127, tblTable.Rows[i]["CaHoc_DotTraoDoiID"].ToString(), iCaHocID);
                int iStatus = int.Parse(tblTable.Rows[i]["status"].ToString());
                if (iStatus.Equals(1))
                {
                    strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Gốc");
                }
                else
                {
                    if (iStatus.Equals(2))
                    {
                        strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Thực hiện");
                    }
                    else
                    {
                        strTable += string.Format("<div style='color:red;font-weight:bold;' class='nd_t4 fl'>{0}</div>", "Hoàn thành");
                    }
                   // strTable += string.Format("<div class='nd_t4 fl'>---</div>");
                }
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&cahoc={2}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["CaHoc_DotTraoDoiID"].ToString(),iCaHocID);
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&&cahoc={2}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["CaHoc_DotTraoDoiID"].ToString(),iCaHocID);
                if(iStatus.Equals(3))
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/Handler/nuce.web.qlhpm/Export.aspx?userid={0}&&id={1}&&action={2}'  target='_blank'>Xuất file</a></div>", this.UserId, tblTable.Rows[i]["CaHoc_DotTraoDoiID"].ToString(), "filedottraodoi");
                else
                    strTable += string.Format("<div class='nd_t4 fl'>---</div>");
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
            string strFileChoPhep = txtFileChoPhep.Text;
            int iDungLuong = -1;
            int.TryParse(txtDungLuong.Text, out iDungLuong);
            int iGioBatDau = int.Parse(ddlGioBatDau.SelectedValue);
            int iPhutBatDau = int.Parse(ddlPhutBatDau.SelectedValue);
            int iGioKetThuc = int.Parse(ddlGioKetThuc.SelectedValue);
            int iPhutKetThuc = int.Parse(ddlPhutKetThuc.SelectedValue);
            int iStatus= int.Parse(ddlTrangThai.SelectedValue);
            int iType = int.Parse(ddlLoai.SelectedValue);
            int iCaHoc = iCaHocID;
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
                    data.dnn_NuceQLHPM_CaHoc_DotTraoDoi.insert(iCaHoc, strTen, iGioBatDau, iPhutBatDau, iGioKetThuc, iPhutKetThuc, strFileChoPhep,iDungLuong, strMoTa, iType, iStatus);
                    //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                    returnMain();
                }
                else
                {
                    if (strType == "edit")
                    {
                        int iID = int.Parse(txtID.Text);
                        data.dnn_NuceQLHPM_CaHoc_DotTraoDoi.update(iID, iCaHoc, strTen, iGioBatDau, iPhutBatDau, iGioKetThuc, iPhutKetThuc, strFileChoPhep,iDungLuong, strMoTa, iType, iStatus);
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
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?cahoc={1}", this.TabId,iCaHocID));
        }
    }
}