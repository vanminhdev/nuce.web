using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.qlpm
{
    public partial class LichSuDangKi : PortalModuleBase
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
                DataTable dtPhongHoc = data.dnn_NuceCommon_PhongHoc.getName(-1);
                ddlPhongHoc.DataTextField = "Ten";
                ddlPhongHoc.DataValueField = "PhongHocID";
                ddlPhongHoc.DataSource = dtPhongHoc;
                ddlPhongHoc.DataBind();
            }
        }
        private void bindData()
        {
            string strPhongHocID = ddlPhongHoc.SelectedValue;
            int iPhongHocID = int.Parse(strPhongHocID);

            DateTime dtNgayBatDau;
            DateTime dtNgayKetThuc;
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
                            DataTable dtTuanHienTai = data.dnn_NuceQLPM_TuanLichSu.search(dtNgayBatDau, dtNgayKetThuc,
                                iPhongHocID);
                            if (dtTuanHienTai.Rows.Count > 0)
                            {
                                DataTable dtCaHoc = data.dnn_NuceCommon_CaHoc.get(-1);
                                DataTable dtTableLopHoc = data.dnn_NuceCommon_LopHoc.get1(-1);

                                int iRowCaHoc = dtCaHoc.Rows.Count;

                                 dtNgayBatDau = DateTime.Parse(dtTuanHienTai.Rows[0]["Ngay"].ToString());
                                 dtNgayKetThuc = DateTime.Parse(dtTuanHienTai.Rows[dtTuanHienTai.Rows.Count - 1]["Ngay"].ToString());

                                string strContent = "<table  border='1' width='100%'>";
                                strContent = strContent + string.Format("<tr style='font-size: 20;font-weight: bold;'>");
                                strContent = strContent + string.Format("<td colspan='{0}' align='center'>LỊCH SỬ ĐĂNG KÍ GIỜ MÁY</td>", iRowCaHoc * 2 + 1);
                                strContent = strContent + string.Format("</tr>");
                                strContent = strContent + string.Format("<tr style='font-size: 12;'>");
                                strContent = strContent + string.Format("<td colspan='{0}' align='center' valign='top'><i>{1} Ngày {2}/{3} đến {4}/{5} năm {6}</i></td>", iRowCaHoc * 3 + 1, "", dtNgayBatDau.Day, dtNgayBatDau.Month, dtNgayKetThuc.Day, dtNgayKetThuc.Month, dtNgayKetThuc.Year);
                                strContent = strContent + string.Format("</tr>");
                                strContent = strContent + string.Format("<tr style='font-weight: bold;color: red'>");
                                strContent = strContent + string.Format("<td rowspan='2' align='center' valign='top' width='10%'>Phòng {0}</td>", ddlPhongHoc.SelectedItem.Text);

                                string strHeaderLopThay = "<tr style='font-weight: bold;color: red'>";

                                for (int i = 0; i < dtCaHoc.Rows.Count; i++)
                                {
                                    DataRow dtRow = dtCaHoc.Rows[i];
                                    strContent = strContent + string.Format("<td colspan='2' align='center' valign='top'>{0}h{1} - {2}h{3}</td>", dtRow["GioBatDau"], dtRow["PhutBatDau"], dtRow["GioKetThuc"], dtRow["PhutKetThuc"]);
                                    strHeaderLopThay = strHeaderLopThay + string.Format("<td align='center' valign='top' width='9%'>Lớp - thầy</td>");
                                    strHeaderLopThay = strHeaderLopThay + string.Format("<td align='center' valign='top' width='9%'>Ghi chú</td>");
                                    //strHeaderLopThay = strHeaderLopThay + string.Format("<td align='center' valign='top' width='4%'>Tuần</td>");
                                }
                                strHeaderLopThay = strHeaderLopThay + "</tr>";
                                strContent = strContent + string.Format("</tr>");
                                strContent = strContent + strHeaderLopThay;

                                // Noi dung

                                while (dtNgayBatDau <= dtNgayKetThuc)
                                {
                                    strContent = strContent + string.Format("<tr align='center' valign='top'>");
                                    strContent = strContent + string.Format("<td style='font-weight: bold;color: red'>{0}", Utils.getNgayFromDate(dtNgayBatDau));
                                    string strTenTuan = "";
                                    string strNgay = "";
                                    string strCoreContent = "";
                                    bool blCheckGetDataTuanNgay = false;
                                    for (int i = 0; i < dtCaHoc.Rows.Count; i++)
                                    {
                                        DataRow[] drDataRows = dtTuanHienTai.Select(string.Format("Ngay='{0}' and PhongHocID={1} and CaHocID={2}", dtNgayBatDau, iPhongHocID, dtCaHoc.Rows[i]["CaHocID"].ToString()));
                                        if (drDataRows.Length > 0)
                                        {
                                            DataRow[] drDataLopHocRows =
                                                dtTableLopHoc.Select(string.Format("LopHocID={0}", drDataRows[0]["LopHocID"]));
                                            if (drDataLopHocRows.Length > 0)
                                            {
                                                strCoreContent = strCoreContent + string.Format("<td align='center'>{0}</td>", drDataLopHocRows[0]["Ten"]);
                                            }
                                            else
                                            {
                                                strCoreContent = strCoreContent + string.Format("<td align='center' style='color: blue'>{0}</td>", "Chưa đăng kí");
                                            }

                                            strCoreContent = strCoreContent + string.Format("<td valign='top' align='center'>{0}</td>", drDataRows[0]["GhiChu"]);
                                            //strContent = strContent + string.Format("<td align='center'>{0}</td>", drDataRows[0]["TenTuan"]);
                                            if (!blCheckGetDataTuanNgay)
                                            {
                                                strTenTuan = drDataRows[0]["TenTuan"].ToString();
                                                strNgay = dtNgayBatDau.ToString("dd/MM/yyyy");
                                                blCheckGetDataTuanNgay = true;
                                            }
                                        }
                                        else
                                        {
                                            strCoreContent = strCoreContent + string.Format("<td valign='top' align='center'>{0}</td>", "---");
                                            strCoreContent = strCoreContent + string.Format("<td valign='top' align='center'>{0}</td>", "---");
                                           // strContent = strContent + string.Format("<td valign='top' align='center'><a>{0}</a></td>", "Sửa");
                                        }
                                    }
                                    dtNgayBatDau = dtNgayBatDau.AddDays(1);
                                    strContent = strContent + string.Format(" - {0} - {1}</td>", strTenTuan, strNgay);
                                    strContent = strContent + strCoreContent;
                                    strContent = strContent + string.Format("</tr>");
                                }
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