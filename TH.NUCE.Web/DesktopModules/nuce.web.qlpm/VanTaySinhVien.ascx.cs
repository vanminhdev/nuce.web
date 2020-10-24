using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.qlpm
{
    public partial class VanTaySinhVien : PortalModuleBase
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
                txtNgayBatDau.Text = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                txtNgayKetThuc.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            }

            string parameter = Request["__EVENTARGUMENT"];
            if (parameter != null)
            {
                if (parameter.Equals("btnCapNhat"))
                {
                    ChuyenTrangThaiLayMau();
                }
                else if (parameter.Equals("btnCapNhatMaSV"))
                {
                    CapNhatMaSV();
                }
                else if (parameter.Equals("btnXoa"))
                {
                    Xoa();
                }
            }
        }
        private void bindData(string alert)
        {
            bindData();
            string strHtml = divContent.InnerHtml;
            strHtml = strHtml + string.Format("<script>alert('{0}');</script>", alert);
            divContent.InnerHtml = strHtml;
        }
        private void bindData()
        {
            DateTime dtNgayBatDau;
            DateTime dtNgayKetThuc;
            int iStatus = -1;
            string strMaSV = "-1";
            //string strTest ="";
            try
            {
                dtNgayBatDau = DateTime.ParseExact(txtNgayBatDau.Text, "dd/MM/yyyy", null);
                try
                {
                    dtNgayKetThuc = DateTime.ParseExact(txtNgayKetThuc.Text, "dd/MM/yyyy", null);
                    // xu ly
                    if (dtNgayKetThuc < dtNgayBatDau)
                    {
                        divAnnouce.InnerText = "Không được để ngày bắt đầu lớn hơn ngày kết thúc";
                    }
                    else
                    {
                        int.TryParse(ddlStatus.SelectedValue, out iStatus);
                        if (!txtMaSV.Text.Trim().Equals(""))
                            strMaSV = txtMaSV.Text.Trim();
                        // Lay du lieu tu database
                        DataTable dtSinhVienVanTay = data.dnn_NuceQLPM_SinhVien_VanTay.search(dtNgayBatDau, dtNgayKetThuc, iStatus, strMaSV);
                        if (dtSinhVienVanTay.Rows.Count > 0)
                        {

                            string strContent = "<table  border='1' width='100%'>";
                            strContent = strContent + string.Format("<tr style='font-size: 14;font-weight: bold;'>");
                            strContent = strContent + string.Format("<td>{0}</td>", "STT");
                            strContent = strContent + string.Format("<td>{0}</td>", "MASV");
                            strContent = strContent + string.Format("<td>{0}</td>", "Tên");
                            strContent = strContent + string.Format("<td>{0}</td>", "Lớp quản lý");
                            strContent = strContent + string.Format("<td>{0}</td>", "Ngày cập nhật");
                            strContent = strContent + string.Format("<td>{0}</td>", "Người cập nhật");
                            strContent = strContent + string.Format("<td>{0}</td>", "Trạng thái");
                            strContent = strContent + string.Format("<td>{0}</td>", "Trạng thái lấy mẫu");
                            strContent = strContent + string.Format("<td>{0}</td>", "Chuyển Trạng thái lấy mẫu");
                            strContent = strContent + string.Format("<td>{0}</td>", "Xóa");
                            strContent = strContent + "</tr>";
                            for (int i = 0; i < dtSinhVienVanTay.Rows.Count; i++)
                            {
                                strContent = strContent + string.Format("<tr>");
                                strContent = strContent + string.Format("<td>{0}</td>", (i + 1));
                                strContent = strContent + string.Format("<td>{0}</td>", dtSinhVienVanTay.Rows[i]["MaSV1"].ToString());
                                strContent = strContent + string.Format("<td>{0} {1}</td>", dtSinhVienVanTay.Rows[i]["Ho"].ToString(), dtSinhVienVanTay.Rows[i]["Ten"].ToString());
                                strContent = strContent + string.Format("<td>{0}</td>", dtSinhVienVanTay.Rows[i]["MaLop"].ToString());
                                strContent = strContent + string.Format("<td>{0:dd/MM/yyyy}</td>", DateTime.Parse(dtSinhVienVanTay.Rows[i]["UpdatedDate"].ToString()));
                                strContent = strContent + string.Format("<td>{0}</td>", dtSinhVienVanTay.Rows[i]["DisplayName"].ToString());
                                if (dtSinhVienVanTay.Rows[i].IsNull("SinhVienID"))
                                {
                                    strContent = strContent + string.Format("<td>{0} - ", "Chưa tạo sinh viên");
                                    strContent = strContent + string.Format("<input  type='button' value='Sửa mã sv' onclick='suamasv(\"{0}\");'/></td>", dtSinhVienVanTay.Rows[i]["MaSV1"].ToString());
                                }
                                else
                                {
                                    strContent = strContent + string.Format("<td>{0}</td>", "Đã tạo sinh viên");
                                }
                                if (dtSinhVienVanTay.Rows[i]["Status1"].ToString().Equals("1"))
                                {
                                    strContent = strContent + string.Format("<td>{0}</td>", "Bình thường");
                                }
                                else
                                {
                                    strContent = strContent + string.Format("<td>{0}</td>", "Lấy mẫu lại");
                                }
                                strContent = strContent + string.Format("<td><input  type='button' value='Chuyển' onclick='chuyentrangthailaymau(\"{0}\",{1});'/></td>", dtSinhVienVanTay.Rows[i]["MaSV1"].ToString(), dtSinhVienVanTay.Rows[i]["Status1"].ToString());
                                if (dtSinhVienVanTay.Rows[i].IsNull("SinhVienID"))
                                {
                                    strContent = strContent + string.Format("<td><input  type='button' value='xóa' onclick='xoadulieulaymau(\"{0}\",{1});'/></td>", dtSinhVienVanTay.Rows[i]["MaSV1"].ToString(), dtSinhVienVanTay.Rows[i]["Status1"].ToString());
                                }
                                else
                                {
                                    strContent = strContent + string.Format("<td>{0}</td>", " --- ");
                                }
                                strContent = strContent + string.Format("</tr>");
                            }
                            strContent = strContent + "</table>";
                            divContent.InnerHtml = strContent;
                            divAnnouce.InnerText = "";
                        }
                        else
                        {
                            divAnnouce.InnerText = "Không có dữ liệu !!!";
                            divContent.InnerHtml = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    divAnnouce.InnerText = "Sai định dạng ngày tháng " + ex.ToString();
                }
            }
            catch (Exception ex)
            {
                divAnnouce.InnerText = "Sai định dạng ngày tháng " + ex.ToString();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {

        }
        protected void ChuyenTrangThaiLayMau()
        {
            string strMaSV = txtMASVUpdate.Text.Trim();
            int iStatus = int.Parse(txtStatus.Text.Trim());
            iStatus = iStatus == 1 ? 2 : 1;
            data.dnn_NuceQLPM_SinhVien_VanTay.updateStatus(strMaSV, iStatus);
            bindData();
        }
        protected void CapNhatMaSV()
        {
            string strMaSV = txtMASVUpdate.Text.Trim();
            string strNewMaSV=txtStatus.Text.Trim();
            data.dnn_NuceQLPM_SinhVien_VanTay.updateMaSV(strMaSV, strNewMaSV);
            bindData();
        }
        protected void Xoa()
        {
            string strMaSV = txtMASVUpdate.Text.Trim();
            data.dnn_NuceQLPM_SinhVien_VanTay.delete(strMaSV);
            bindData(string.Format("Xóa sinh viên {0} dữ liệu thành công !", strMaSV)) ;
        }
    }
}