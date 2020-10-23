using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Web.UI.WebControls;

namespace nuce.web.qlpm
{
    public partial class New_LichSuDangKi_For_GV : PortalModuleBase
    {
        static DataTable dtPhongHoc;
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
                dtPhongHoc = data.dnn_NuceCommon_PhongHoc.getByToaNha(3);
                ddlPhongHoc.DataTextField = "Ten";
                ddlPhongHoc.DataValueField = "PhongHocID";
                ddlPhongHoc.DataSource = dtPhongHoc;
                ddlPhongHoc.DataBind();
                ListItem liTemp = new ListItem("Tất cả các phòng", "-1");
                ddlPhongHoc.Items.Insert(0, liTemp);
                txtNgayBatDau.Text = DateTime.Now.AddDays(-200).ToString("dd/MM/yyyy");
                txtNgayKetThuc.Text = DateTime.Now.AddDays(21).ToString("dd/MM/yyyy");
                bindData();
            }
        }
        private void bindData()
        {
            string strPhongHocID = ddlPhongHoc.SelectedValue;
            int iPhongHocID = int.Parse(strPhongHocID);

            DateTime dtNgayBatDau;
            DateTime dtNgayKetThuc;
            string strTrangThai = "";
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
                        // Lay du lieu tu database
                        DataTable dtTuanHienTai = data.dnn_NuceQLPM_LichPhongMay.getByUpdatedDate(dtNgayBatDau, dtNgayKetThuc, iPhongHocID, -1, -1, this.UserId);
                        if (dtTuanHienTai.Rows.Count > 0)
                        {
                            string strContent = "<table  border='1' width='100%'>";
                            strContent = strContent + string.Format("<tr style='font-size: 12;font-weight: bold;'>");
                            strContent = strContent + string.Format("<td align='center'>STT</td>");
                            strContent = strContent + string.Format("<td align='center'>MADK</td>");
                            strContent = strContent + string.Format("<td align='center'>CB</td>");
                            strContent = strContent + string.Format("<td align='center'>Lop</td>");
                            strContent = strContent + string.Format("<td align='center'>MonHoc</td>");
                            strContent = strContent + string.Format("<td align='center'>Phòng học</td>");
                            strContent = strContent + string.Format("<td align='center'>Ngày học</td>");
                            strContent = strContent + string.Format("<td align='center'>CaHoc</td>");
                            strContent = strContent + string.Format("<td align='center'>Thông tin thêm</td>");
                            strContent = strContent + string.Format("<td align='center'>Trạng Thái</td>");
                            strContent = strContent + string.Format("</tr>");

                            for (int i = 0; i < dtTuanHienTai.Rows.Count; i++)
                            {
                                strContent = strContent + string.Format("<tr style='font-size: 12;'>");
                                strContent = strContent + string.Format("<td align='center'>{0}</td>",(i+1));
                                strContent = strContent + string.Format("<td align='center'>{0}</td>", dtTuanHienTai.Rows[i]["MADK"].ToString());
                                strContent = strContent + string.Format("<td align='center'>{0} - {1}</td>", dtTuanHienTai.Rows[i]["MaCB"].ToString(), dtTuanHienTai.Rows[i]["HoVaTenCB"].ToString());
                                strContent = strContent + string.Format("<td align='center'>{0}</td>", dtTuanHienTai.Rows[i]["Lop"].ToString());
                                strContent = strContent + string.Format("<td align='center'>{0}</td>", dtTuanHienTai.Rows[i]["MonHoc"].ToString());
                                strContent = strContent + string.Format("<td align='center'>{0}</td>", dtTuanHienTai.Rows[i]["MaPH"].ToString());
                                strContent = strContent + string.Format("<td align='center'>{0:dd/MM/yyyy}</td>", DateTime.Parse(dtTuanHienTai.Rows[i]["Ngay"].ToString()));
                                strContent = strContent + string.Format("<td align='center'>{0}</td>", dtTuanHienTai.Rows[i]["CaHocID"].ToString());

                                strContent = strContent + string.Format(@"<td align='center'>- Người đăng kí: {0} </br> - Ngày đăng kí: {1:dd/MM/yyyy}</br>
                                                                                             - Người cập nhật: {2} </br> - Ngày cập nhật: {3:dd/MM/yyyy}</br>
                                                                                             - Người duyệt: {4} </br> - Ngày duyệt: {5:dd/MM/yyyy}</td>", dtTuanHienTai.Rows[i]["CreatedName"].ToString(),DateTime.Parse(dtTuanHienTai.Rows[i]["CreatedDate"].ToString()),dtTuanHienTai.Rows[i]["UpdatedName"].ToString(), DateTime.Parse(dtTuanHienTai.Rows[i]["UpdatedDate"].ToString()),dtTuanHienTai.Rows[i]["ApprovedName"].ToString(), DateTime.Parse(dtTuanHienTai.Rows[i]["ApprovedDate"].ToString()));
                                switch(dtTuanHienTai.Rows[i]["Status"].ToString())
                                {
                                    case "1": strTrangThai = "Vừa đăng kí";
                                        break;
                                    case "2":
                                        strTrangThai = "Đang chờ duyệt";
                                        break;
                                    case "3":
                                        strTrangThai = "Đã duyệt";
                                        break;
                                    case "4":
                                        strTrangThai = "Huỷ";
                                        break;
                                }
                                strContent = strContent + string.Format("<td align='center'>{0}</td>",strTrangThai);
                                strContent = strContent + string.Format("</tr>");
                            }
                            // Noi dung
                            strContent = strContent + "</table>";
                            divContent.InnerHtml = strContent;
                            divAnnouce.InnerText = "";
                        }
                        else
                        {
                            divAnnouce.InnerText = "Không có dữ liệu !!!";
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
    }
}