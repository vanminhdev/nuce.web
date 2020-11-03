using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.qlpm
{
    public partial class TuanHienTai : PortalModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            divFilter.Visible = false;
            divAction.Visible = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtPhongHoc = data.dnn_NuceCommon_PhongHoc.getName(-1);
                ddlPhongHoc.DataTextField = "Ten";
                ddlPhongHoc.DataValueField = "PhongHocID";
                ddlPhongHoc.DataSource = dtPhongHoc;
                ddlPhongHoc.DataBind();
                if (Request.QueryString["phonghocid"] != null && Request.QueryString["phonghocid"] != "")
                {
                    ddlPhongHoc.SelectedValue = Request.QueryString["phonghocid"];
                }
                bindData();
            }
        }
        private void bindData()
        {
            string strPhongHocID = ddlPhongHoc.SelectedValue;
            int iPhongHocID = int.Parse(strPhongHocID);
            // Lay du lieu tu database
            DataTable dtTuanHienTai = data.dnn_NuceQLPM_TuanHienTai.getByTypeAndPhongHoc(2, iPhongHocID, 1);

            if (dtTuanHienTai.Rows.Count > 0)
            {
                divContent.Visible = true;
                divFilter.Visible = true;
                divAction.Visible = true;

                // Lấy dữ liệu các ngày đã gửi yêu cầu điểm danh hay chưa
                string strTuanHienTai = "";
                for (int i = 0; i < dtTuanHienTai.Rows.Count; i++)
                {
                    strTuanHienTai = strTuanHienTai + dtTuanHienTai.Rows[i]["TuanHienTaiID"].ToString() + ",";
                }
                DataTable dtRequest = data.dnn_NuceCommon_Request.getByCodeAndPartner("DIEM_DANH",strTuanHienTai);
                DataTable dtCaHoc = data.dnn_NuceCommon_CaHoc.get(-1);
                DataTable dtTableLopHoc = data.dnn_NuceCommon_LopHoc.get1(-1);

                int iRowCaHoc = dtCaHoc.Rows.Count;

                DateTime dtNgayBatDau = DateTime.Parse(dtTuanHienTai.Rows[0]["Ngay"].ToString());
                DateTime dtNgayKetThuc = DateTime.Parse(dtTuanHienTai.Rows[dtTuanHienTai.Rows.Count - 1]["Ngay"].ToString());
                string strTenTuan = dtTuanHienTai.Rows[0]["TenTuan"].ToString();

                string strContent = "<table  border='1' width='100%'>";
                strContent = strContent + string.Format("<tr style='font-size: 20;font-weight: bold;'>");
                strContent = strContent + string.Format("<td colspan='{0}' align='center'>LỊCH ĐĂNG KÍ GIỜ MÁY</td>", iRowCaHoc * 3 + 1);
                strContent = strContent + string.Format("</tr>");
                strContent = strContent + string.Format("<tr style='font-size: 12;'>");
                strContent = strContent + string.Format("<td colspan='{0}' align='center' valign='top'><i>{1} Ngày {2}/{3} đến {4}/{5} năm {6}</i></td>", iRowCaHoc * 3 + 1, strTenTuan, dtNgayBatDau.Day, dtNgayBatDau.Month, dtNgayKetThuc.Day, dtNgayKetThuc.Month, dtNgayKetThuc.Year);
                strContent = strContent + string.Format("</tr>");
                strContent = strContent + string.Format("<tr style='font-weight: bold;color: red'>");
                strContent = strContent + string.Format("<td rowspan='2' align='center' valign='top' width='10%'>Phòng {0}</td>", ddlPhongHoc.SelectedItem.Text);

                string strHeaderLopThay = "<tr style='font-weight: bold;color: red'>";

                for (int i = 0; i < dtCaHoc.Rows.Count; i++)
                {
                    DataRow dtRow = dtCaHoc.Rows[i];
                    strContent = strContent + string.Format("<td colspan='3' align='center' valign='top'>{0}h{1} - {2}h{3}</td>", dtRow["GioBatDau"], dtRow["PhutBatDau"], dtRow["GioKetThuc"], dtRow["PhutKetThuc"]);
                    strHeaderLopThay = strHeaderLopThay + string.Format("<td align='center' valign='top' width='7%'>Lớp - thầy</td>");
                    strHeaderLopThay = strHeaderLopThay + string.Format("<td align='center' valign='top' width='7%'>Ghi chú</td>");
                    strHeaderLopThay = strHeaderLopThay + string.Format("<td align='center' valign='top' width='4%'></td>");
                }
                strHeaderLopThay = strHeaderLopThay + "</tr>";
                strContent = strContent + string.Format("</tr>");
                strContent = strContent + strHeaderLopThay;

                // Noi dung

                while (dtNgayBatDau <= dtNgayKetThuc)
                {
                    strContent = strContent + string.Format("<tr align='center' valign='top'>");
                    strContent = strContent + string.Format("<td style='font-weight: bold;color: red'>{0}</td>", Utils.getNgayFromDate(dtNgayBatDau));
                    for (int i = 0; i < dtCaHoc.Rows.Count; i++)
                    {
                        DataRow[] drDataRows = dtTuanHienTai.Select(string.Format("Ngay='{0}' and PhongHocID={1} and CaHocID={2}", dtNgayBatDau, iPhongHocID, dtCaHoc.Rows[i]["CaHocID"].ToString()));
                        if (drDataRows.Length > 0)
                        {
                            // Lấy dữ liệu của gửi yêu cầu điểm danh
                            DataRow[] drDataRequestRows =
                                dtRequest.Select(string.Format("PartnerID={0}", drDataRows[0]["TuanHienTaiID"]));
                            string strColorRequest = "background-color: #d3d3d3;";
                            if (drDataRequestRows.Length > 0)
                            {
                                switch (drDataRequestRows[0]["Status"].ToString())
                                {
                                    case "1":
                                        strColorRequest = "background-color: blue;color:white;";
                                        break;
                                    case "2":
                                        strColorRequest = "background-color: red;color:white;";
                                        break;
                                    case "3":
                                        strColorRequest = "background-color: green;color:white;";
                                        break;
                                    case "5":
                                        strColorRequest = "background-color: yellow;";
                                        break;
                                }
                                
                            }
                            DataRow[] drDataLopHocRows =
                                dtTableLopHoc.Select(string.Format("LopHocID={0}", drDataRows[0]["LopHocID"]));
                            if (drDataLopHocRows.Length > 0)
                            {
                                strContent = strContent + string.Format("<td style='{1}' align='center'>{0}</td>", drDataLopHocRows[0]["Ten"], strColorRequest);
                                strContent = strContent + string.Format("<td style='{1}' valign='top' align='center'>{0}</td>", drDataRows[0]["GhiChu"], strColorRequest);
                                strContent = strContent + string.Format("<td  style='{4}' align='center'>" +
                                        "<div><input style=\"width:50px;\" type=\"button\" onclick=\"goiTrangSuDangKiPhongHoc('/Quan-ly-phong-may/Sua-dang-ki-lop-hoc?tuanhientaiid={1}&type=1&phonghocid={2}&tab={3}&popUp=true');\" value=\"{0}\"/></div>" +
                                        "<div><input title=\"Điểm danh\" style=\"width:50px;\" type=\"button\" onclick=\"goiTrangSuDangKiPhongHoc('/Quan-ly-phong-may/Diem-Danh?tuanhientaiid={1}&popUp=true');\" value=\"{5}\"></div>" +
                                        "</td>", "Sửa", drDataRows[0]["TuanHienTaiID"], strPhongHocID, this.TabId, strColorRequest,"ĐD");
                            }
                            else
                            {
                                strContent = strContent + string.Format("<td  align='center' style='color: blue;{1}'>{0}</td>", "Chưa đăng kí", strColorRequest);
                                strContent = strContent + string.Format("<td style='{1}' valign='top' align='center'>{0}</td>", drDataRows[0]["GhiChu"], strColorRequest);
                                strContent = strContent + string.Format("<td style='{4}' align='center'>" +
                                                                        "<div><input style=\"width:50px;\" type=\"button\" onclick=\"goiTrangSuDangKiPhongHoc('/Quan-ly-phong-may/Sua-dang-ki-lop-hoc?tuanhientaiid={1}&type=1&phonghocid={2}&tab={3}&popUp=true');\" value=\"{0}\"/></div>" +
                                                                        "</td>", "Sửa", drDataRows[0]["TuanHienTaiID"], strPhongHocID, this.TabId, strColorRequest);
                            }

                            
                        }
                        else
                        {
                            strContent = strContent + string.Format("<td valign='top' align='center'>{0}</td>", "---");
                            strContent = strContent + string.Format("<td valign='top' align='center'>{0}</td>", "---");
                            strContent = strContent + string.Format("<td valign='top' align='center'><a>{0}</a></td>", "Sửa");
                        }
                    }
                    dtNgayBatDau = dtNgayBatDau.AddDays(1);
                    strContent = strContent + string.Format("</tr>");
                }

                strContent = strContent + "</table>";
                divContent.InnerHtml = strContent;
            }
            else
            {
                divContent.Visible = false;
                divFilter.Visible = false;
                divAction.Visible = false;
                divAnnouce.InnerText = "Chưa có thông tin tuần hiện tại. Đề nghị cập nhật dữ liệu tuần hiện tại !!!";
            }
        }
        private void returnMain(string PhongHocID)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?phonghocid={1}", this.TabId, PhongHocID));
        }
        protected void ddlPhongHoc_SelectedIndexChanged1(object sender, EventArgs e)
        {
            returnMain(ddlPhongHoc.SelectedValue)
          ;
        }
        protected void btnChotTuanHienTai_Click(object sender, EventArgs e)
        {
            data.dnn_NuceQLPM_TuanHienTai.chottuanhientai();
            bindData();
        }
    }
}