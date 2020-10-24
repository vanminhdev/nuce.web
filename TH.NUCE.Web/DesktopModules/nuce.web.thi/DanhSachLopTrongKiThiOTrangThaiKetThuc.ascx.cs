using System;
using System.Data;

namespace nuce.web.thi
{
    public partial class DanhSachLopTrongKiThiOTrangThaiKetThuc : CoreModule
    {
        private int m_iKiThi_LopHocID;
        private DataTable dtContent;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                m_iKiThi_LopHocID = -1;
                if (Request.QueryString["kithi_lophoc"] != null)
                {
                    m_iKiThi_LopHocID = int.Parse(Request.QueryString["kithi_lophoc"]);
                }
                // Lấy danh sách kì thi theo user
                DataTable dtKiThiLopHoc = data.dnn_NuceThi_KiThi_LopHoc.getByNguoiDung(this.UserId, 3);
                if (dtKiThiLopHoc.Rows.Count > 0)
                {
                    if (m_iKiThi_LopHocID.Equals(-1))
                    {
                        m_iKiThi_LopHocID = int.Parse(dtKiThiLopHoc.Rows[0]["KiThi_LopHocID"].ToString());
                    }
                    string strHtmlMenu = "<table>";
                    for (int i = 0; i < dtKiThiLopHoc.Rows.Count; i++)
                    {
                        int iKiThi_LopHocID = int.Parse(dtKiThiLopHoc.Rows[i]["KiThi_LopHocID"].ToString());
                        if (m_iKiThi_LopHocID.Equals(iKiThi_LopHocID))
                            strHtmlMenu += string.Format("<tr><td style='color:red;'>{0} - {1}</td></tr>", (i + 1), dtKiThiLopHoc.Rows[i]["Ten1"].ToString());
                        else
                            strHtmlMenu += string.Format("<tr><td><a href='/tabid/{0}/default.aspx?kithi_lophoc={1}'>{2} - {3}</a></td></tr>", this.TabId, iKiThi_LopHocID, i + 1, dtKiThiLopHoc.Rows[i]["Ten1"].ToString());
                    }
                    strHtmlMenu += "</table>";
                    tdMenu.InnerHtml = strHtmlMenu;
                    divAction.Visible = true;
                    txtKiThiLopHocID.Text = m_iKiThi_LopHocID.ToString();
                    divAction.InnerHtml = string.Format("<a href='/ExportExcel.aspx?type=3&&kithilophocid={0}' target='_blank'>Xuất file excel</a>", m_iKiThi_LopHocID);
                    displayContent();
                }
                else
                {
                    divAction.Visible = false;
                    divMain.InnerHtml = string.Format("Không có lớp nào đang trong trạng thái đã thi xong");
                }
            }
        }
        private void displayContent()
        {
            string strKiThiLopHocID = txtKiThiLopHocID.Text;
            dtContent = data.dnn_NuceThi_KiThi_LopHoc_SinhVien.getByKiThi_LopHoc(int.Parse(strKiThiLopHocID));
            string strHtml = "<table style='width:100%;' border='1'>";
            strHtml += "<tr style='width:100%; height:50px; line-height:20px; background:#F5F5F5; color:#333; font-weight:bold; text-align:center; font-size:12px; border-bottom: 1px solid #ccc;'>";
            strHtml += string.Format("<td style='width: 5%; vertical-align:middle;'>{0}</td>", "<input type='checkbox' onchange='selectAll(this)' name='namesinhviencheckall' value='valuesinhviencheckall' id='idsinhviencheckall'>");
            strHtml += string.Format("<td style='width: 5%; vertical-align:middle;'>{0}</td>", "STT");
            strHtml += string.Format("<td style='width: 10%; vertical-align:middle;'>{0}</td>", "Mã SV");
            strHtml += string.Format("<td style='width: 20%; vertical-align:middle;'>{0}</td>", "Họ và Tên");
            strHtml += string.Format("<td style='width: 35%; vertical-align:middle;'>{0}</td>", "Ghi chú");
            strHtml += string.Format("<td style='width: 5%; vertical-align:middle;'>{0}</td>", "Điểm");
            strHtml += string.Format("<td>{0}</td>", "Chi tiết");
            strHtml += string.Format("<td>{0}</td>", "Trạng thái");
            strHtml += "</tr>";
            for (int i = 0; i < dtContent.Rows.Count; i++)
            {
                string strID = dtContent.Rows[i]["KiThi_LopHoc_SinhVienID"].ToString();
                int iStatus = int.Parse(dtContent.Rows[i]["Status"].ToString());
                string strTenTrangThai = "";
                string strDiem = "---";
                switch (iStatus)
                {
                    case 1:
                        strTenTrangThai = "Gốc";
                        strHtml += "<tr style='vertical-align:top;width:100%; height:32px; line-height:32px; color:#000; font-size:12px; text-align:center;'>";
                        strHtml += "<td>---</td>";
                        break;
                    case 2:
                        strTenTrangThai = "Chuẩn bị thi";
                        strHtml += "<tr style='vertical-align:top;width:100%; height:32px; line-height:32px; color:rgb(255,72,72); font-size:12px; text-align:center;'>";
                        strHtml += "<td>---</td>";
                        break;
                    case 5:
                        strTenTrangThai = "Chuẩn bị thi tiếp";
                        strHtml += "<tr style='vertical-align:top;width:100%; height:32px; line-height:32px; color:rgb(255,72,72); font-size:12px; text-align:center;'>";
                        strHtml += string.Format("<td style='padding-top:6px;'><input type='checkbox' name='namesinhvien' value='valuesinhvien_{0}' id='idsinhvien_{0}'></td>", strID);
                        break;
                    case 6:
                        strTenTrangThai = "Chuẩn bị thi lại";
                        strHtml += "<tr style='vertical-align:top;width:100%; height:32px; line-height:32px; color:rgb(255,72,72); font-size:12px; text-align:center;'>";
                        strHtml += "<td>---</td>";
                        break;
                    case 3:
                        strTenTrangThai = "Đang thi";
                        strHtml += "<tr style='vertical-align:top;width:100%; height:32px; line-height:32px; color:#red; font-size:12px; text-align:center;'>";
                        strHtml += "<td>---</td>";
                        break;
                    case 4:
                        strTenTrangThai = "Đã thi xong";
                        strDiem = float.Parse(dtContent.Rows[i]["Diem"].ToString()).ToString("N2");
                        strHtml += "<tr style='vertical-align:top;width:100%; height:32px; line-height:32px; color:rgb(128,0,128); font-size:12px; text-align:center;'>";
                        strHtml += "<td>---</td>";
                        //strHtml += string.Format("<td style='padding-top:6px;'><input type='checkbox' name='namesinhvien' value='valuesinhvien_{0}' id='idsinhvien_{0}'></td>", strID);
                        break;
                    case 9:
                        strTenTrangThai = "Cấm thi";
                        strHtml += "<tr style='vertical-align:top;width:100%; height:32px; line-height:32px; color:rgb(128,0,98); font-size:12px; text-align:center;'>";
                        strHtml += "<td>---</td>";
                        //strHtml += "<tr style='vertical-align:top;width:100%; height:32px; line-height:32px; color:rgb(128,0,98); font-size:12px; text-align:center;'>";
                        break;
                    case 8:
                        strTenTrangThai = "Bỏ thi";
                        strHtml += "<tr style='vertical-align:top;width:100%; height:32px; line-height:32px; color:rgb(128,0,68); font-size:12px; text-align:center;'>";
                        strHtml += "<td>---</td>";
                        //strHtml += "<tr style='vertical-align:top;width:100%; height:32px; line-height:32px; color:rgb(128,0,68); font-size:12px; text-align:center;'>";
                        break;
                }

                strHtml += string.Format("<td>{0}</td>", (i + 1));
                strHtml += string.Format("<td style='text-align:left;padding-left:5px;'>{0}</td>", dtContent.Rows[i]["MaSV"].ToString());
                strHtml += string.Format("<td style='text-align:left;padding-left:5px;'>{0} {1}</td>", dtContent.Rows[i]["Ho"].ToString(), dtContent.Rows[i]["TenSinhVien"].ToString());
                strHtml += string.Format("<td style='text-align:left;padding-left:5px;'>{0} <a style='color:Rgb(100,0,0);' href='{1}'> (sửa)</a></td>", dtContent.Rows[i]["MoTa"].ToString(), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "EidtMoTa", "mid/" + this.ModuleId.ToString() + "/kithi_lophoc_sinhvien/" + dtContent.Rows[0]["KiThi_LopHoc_SinhVienID"].ToString() + "/kithi_lophoc/" + strKiThiLopHocID));
                strHtml += string.Format("<td style='text-align:right;padding-right:5px;'>{0}</td>", strDiem);
                strHtml += string.Format("<td><a href='/tabid/1121/default.aspx?kithilophocsinhvien={0}&&ten={1} {2}&&ma={3}'>Chi tiết</a></td>", strID, dtContent.Rows[i]["Ho"].ToString(), dtContent.Rows[i]["TenSinhVien"].ToString(), dtContent.Rows[i]["MaSV"].ToString());
                strHtml += string.Format("<td>{0}</td>", strTenTrangThai);
                strHtml += "</tr>";
            }
            strHtml += "</table>";
            divContent.InnerHtml = strHtml;
        }
    }
}