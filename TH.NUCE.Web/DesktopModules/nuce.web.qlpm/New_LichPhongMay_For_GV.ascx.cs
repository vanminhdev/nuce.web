using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Web.UI.WebControls;

namespace nuce.web.qlpm
{
    public partial class New_LichPhongMay_For_GV : PortalModuleBase
    {
        static int iPhong;
        static int iTuan;
        static int iTuanHienTai;
        static DateTime dtNgayBatDau;
        static DataTable dtPhongHoc;
        protected override void OnInit(EventArgs e)
        {

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

                for (int count = 1; count < 49; count++)
                {
                    ListItem li = new ListItem("Tuần " + count.ToString(), count.ToString());
                    ddlTuan.Items.Add(li);
                }
                // Tuần hiện tại
                DataTable dtNamHoc = data.dnn_NuceCommon_NamHoc.getByStatus(1);
                dtNgayBatDau = DateTime.Parse(dtNamHoc.Rows[0]["NgayBatDau"].ToString());
                iTuanHienTai = iTuan = 1 + DateTime.Now.Subtract(dtNgayBatDau).Days / 7;

                if (Request.QueryString["tuan"] != null)
                {
                    iTuan = int.Parse(Request.QueryString["tuan"].ToString());
                }
                else
                    iTuan = iTuanHienTai;

                if (Request.QueryString["phong"] != null)
                {
                    iPhong = int.Parse(Request.QueryString["phong"].ToString());
                }
                else
                    iPhong = -1;

                ddlTuan.SelectedValue = iTuan.ToString();
                ddlPhongHoc.SelectedValue = iPhong.ToString();
                bindData();
            }



        }

        private void bindData()
        {
            DateTime NgayBatDau = dtNgayBatDau.AddDays(7 * (iTuan - 1) - 1);
            DateTime NgayKetThuc = NgayBatDau.AddDays(7);
            DataTable dtLichPhongMay = data.dnn_NuceQLPM_LichPhongMay.get(NgayBatDau, NgayKetThuc);
            string strHtmlOut = string.Format("<div style='text-align:center;font-weight:bold; font-size:20px;padding-top:10px;padding-bottom:2px;'>LỊCH PHÒNG MÁY</div>");
            strHtmlOut += string.Format("<div style='text-align:center;font-style:italic; font-size:16px;padding-bottom:5px;'>(Tuần {0} Ngày {1:dd/MM} đến {2:dd/MM} năm {3:yyyy})</div>", iTuan, NgayBatDau.AddDays(1), NgayKetThuc.AddDays(-1), NgayBatDau);
            if (iPhong > 0)
            {
                strHtmlOut += getHtml(NgayBatDau, iPhong, dtLichPhongMay);
            }
            else
            {
                for (int i = 0; i < dtPhongHoc.Rows.Count; i++)
                    strHtmlOut += getHtml(NgayBatDau, int.Parse(dtPhongHoc.Rows[i]["PhongHocID"].ToString()), dtLichPhongMay);
            }
            //string strTest ="";
            //strHtmlOut += string.Format("<div style='text-align:right;font-weight:bold; font-size:12px;padding-top:10px;padding-bottom:2px;'><a href='/ExportExcel.aspx?tuan={0}&type=6'>Export excel</a></div>",iTuan);
            divContent.InnerHtml = strHtmlOut;
            try
            {

                try
                {

                }
                catch (Exception ex)
                {
                    //divAnnouce.InnerText = "Sai định dạng ngày tháng " + ex.ToString();
                }
            }
            catch (Exception ex)
            {
                //divAnnouce.InnerText = "Sai định dạng ngày tháng " + ex.ToString();
            }
        }
        private bool checkEdit(DataRow[] drs, DateTime NgayBatDau)
        {
            //if (drs.Length > 0)
            //{
            //    if (drs[0]["MaCB"].Equals(this.UserInfo.Username))
            //   {
            //        // Kiem tra ngay thang co hop le
            //        if(checkEdit(NgayBatDau))
            //        {
            //            return true;
            //        }
            //    }
            //}
            return false;
        }
        private bool checkEdit( DateTime NgayBatDau)
        {
                    // Kiem tra ngay thang co hop le
                    //if (DateTime.Now.AddDays(14) > NgayBatDau.AddDays(-1) && DateTime.Now.AddDays(14) < NgayBatDau.AddDays(6))
                    //{
                    //    return true;
                    //}
            return false;
        }
        private string getHtml(DateTime NgayBatDau, int Phong, DataTable dt)
        {
            string strReturn = string.Format("<div style='text-align:center;font-weight:bold; font-size:16px;padding-top:2px;padding-bottom:2px;height:30px;'>PHÒNG MÁY SÔ {0}</div>", Phong);
            strReturn += "<table border='1px' style='width: 100%;font-size:14px;'>";
            strReturn += "<tr style='font-weight:bold;text-align:center;vertical-align:central;height:30px;'>";
            strReturn += string.Format("<td rowspan='2'>PHÒNG<br>{0}</td>", Phong);
            strReturn += string.Format("<td colspan='2'>6h45 - 9h05</td>");
            strReturn += string.Format("<td colspan='2'>9h25 - 11h50</td>");
            strReturn += string.Format("<td colspan='2'>12h15 - 14h35</td>");
            strReturn += string.Format("<td colspan='2'>14h55 - 17h20</td>");
            strReturn += string.Format("<td colspan='2'>18h00 - 20h30</td>");
            strReturn += string.Format("<td rowspan='2'>GHI CHÚ</td>");
            strReturn += "</tr>";
            strReturn += "<tr style='font-weight:bold;text-align:center;vertical-align:central;height:50px;'>";
            strReturn += string.Format("<td style='width:9%'>Lớp</td>");
            strReturn += string.Format("<td style='width:9%'>Thầy<br>hướng dẫn</td>");
            strReturn += string.Format("<td style='width:9%'>Lớp</td>");
            strReturn += string.Format("<td style='width:9%'>Thầy<br>hướng dẫn</td>");
            strReturn += string.Format("<td style='width:9%'>Lớp</td>");
            strReturn += string.Format("<td style='width:9%'>Thầy<br>hướng dẫn</td>");
            strReturn += string.Format("<td style='width:9%'>Lớp</td>");
            strReturn += string.Format("<td style='width:9%'>Thầy<br>hướng dẫn</td>");
            strReturn += string.Format("<td style='width:9%'>Lớp</td>");
            strReturn += string.Format("<td style='width:9%'>Thầy<br>hướng dẫn</td>");
            strReturn += "</tr>";
            DataRow[] drs;
            for (int i = 2; i < 8; i++)
            {
                strReturn += "<tr style='text-align:center;vertical-align:central;height:35px;'>";
                strReturn += string.Format("<td>Thứ {0}</td>", i);
                drs = dt.Select(string.Format("PhongHocID={0} and Thu='{1}' and CaHocID={2}", Phong, i, 1));
                // Lấy dữ liệu ngày thứ 2
                if (drs.Length > 0)
                {
                    if(drs[0]["status"].ToString().Equals("1"))
                        strReturn += string.Format("<td style='color:red;'>{0}<br>{1} <br>Chờ duyệt</td>", drs[0]["Lop"].ToString(), drs[0]["MonHoc"].ToString());
                    else
                        strReturn += string.Format("<td>{0}<br>{1}</td>", drs[0]["Lop"].ToString(), drs[0]["MonHoc"].ToString());
                    if (checkEdit(drs, NgayBatDau))
                        strReturn += string.Format("<td>{0}<br><a href='{1}'>sửa</a> - <a href='{2}'>Huỷ</a></td>", drs[0]["HoVaTenCB"].ToString(), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit", "mid/" + this.ModuleId.ToString(), "type/edit", "id/" + drs[0]["ID"].ToString(), "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString()), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "delete", "mid/" + this.ModuleId.ToString(), "type/delete", "id/" + drs[0]["ID"].ToString(), "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString()));
                    else
                        strReturn += string.Format("<td>{0}<br></td>", drs[0]["HoVaTenCB"].ToString());
                }
                else
                {
                    strReturn += string.Format("<td></td>");
                    if (checkEdit(NgayBatDau))
                    {
                        strReturn += string.Format("<td><a href='{0}'>Đăng kí</a></td>", DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit", "mid/" + this.ModuleId.ToString(), "type/new", "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString(), "ngay/" + NgayBatDau.AddDays(i - 1).ToFileTimeUtc().ToString(), "ca/1"));
                    }
                    else
                        strReturn += string.Format("<td>{0}</td>", "---");
                }
                drs = dt.Select(string.Format("PhongHocID={0} and Thu='{1}' and CaHocID={2}", Phong, i, 2));
                // Lấy dữ liệu ngày thứ 2
                if (drs.Length > 0)
                {
                    if (drs[0]["status"].ToString().Equals("1"))
                        strReturn += string.Format("<td style='color:red;'>{0}<br>{1} <br>Chờ duyệt</td>", drs[0]["Lop"].ToString(), drs[0]["MonHoc"].ToString());
                    else
                        strReturn += string.Format("<td>{0}<br>{1}</td>", drs[0]["Lop"].ToString(), drs[0]["MonHoc"].ToString());
                    if (checkEdit(drs, NgayBatDau))
                        strReturn += string.Format("<td>{0}<br><a href='{1}'>sửa</a> - <a href='{2}'>Huỷ</a></td>", drs[0]["HoVaTenCB"].ToString(), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit", "mid/" + this.ModuleId.ToString(), "type/edit", "id/" + drs[0]["ID"].ToString(), "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString()), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "delete", "mid/" + this.ModuleId.ToString(), "type/delete", "id/" + drs[0]["ID"].ToString(), "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString()));
                    else
                        strReturn += string.Format("<td>{0}<br></td>", drs[0]["HoVaTenCB"].ToString());
                }
                else
                {
                    strReturn += string.Format("<td></td>");
                    if (checkEdit(NgayBatDau))
                    {
                        strReturn += string.Format("<td><a href='{0}'>Đăng kí</a></td>", DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit", "mid/" + this.ModuleId.ToString(), "type/new", "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString(), "ngay/" + NgayBatDau.AddDays(i - 1).ToFileTimeUtc().ToString(), "ca/2"));
                    }
                    else
                        strReturn += string.Format("<td>{0}</td>", "---");
                }
                drs = dt.Select(string.Format("PhongHocID={0} and Thu='{1}' and CaHocID={2}", Phong, i, 3));
                // Lấy dữ liệu ngày thứ 2
                if (drs.Length > 0)
                {
                    if (drs[0]["status"].ToString().Equals("1"))
                        strReturn += string.Format("<td style='color:red;'>{0}<br>{1} <br>Chờ duyệt</td>", drs[0]["Lop"].ToString(), drs[0]["MonHoc"].ToString());
                    else
                        strReturn += string.Format("<td>{0}<br>{1}</td>", drs[0]["Lop"].ToString(), drs[0]["MonHoc"].ToString());
                    if (checkEdit(drs, NgayBatDau))
                        strReturn += string.Format("<td>{0}<br><a href='{1}'>sửa</a> - <a href='{2}'>Huỷ</a></td>", drs[0]["HoVaTenCB"].ToString(), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit", "mid/" + this.ModuleId.ToString(), "type/edit", "id/" + drs[0]["ID"].ToString(), "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString()), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "delete", "mid/" + this.ModuleId.ToString(), "type/delete", "id/" + drs[0]["ID"].ToString(), "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString()));
                    else
                        strReturn += string.Format("<td>{0}<br></td>", drs[0]["HoVaTenCB"].ToString());
                }
                else
                {
                    strReturn += string.Format("<td></td>");
                    if (checkEdit(NgayBatDau))
                    {
                        strReturn += string.Format("<td><a href='{0}'>Đăng kí</a></td>", DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit", "mid/" + this.ModuleId.ToString(), "type/new", "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString(), "ngay/" + NgayBatDau.AddDays(i - 1).ToFileTimeUtc().ToString(), "ca/3"));
                    }
                    else
                        strReturn += string.Format("<td>{0}</td>", "---");
                }
                drs = dt.Select(string.Format("PhongHocID={0} and Thu='{1}' and CaHocID={2}", Phong, i, 4));
                // Lấy dữ liệu ngày thứ 2
                if (drs.Length > 0)
                {
                    if (drs[0]["status"].ToString().Equals("1"))
                        strReturn += string.Format("<td style='color:red;'>{0}<br>{1} <br>Chờ duyệt</td>", drs[0]["Lop"].ToString(), drs[0]["MonHoc"].ToString());
                    else
                        strReturn += string.Format("<td>{0}<br>{1}</td>", drs[0]["Lop"].ToString(), drs[0]["MonHoc"].ToString());
                    if (checkEdit(drs, NgayBatDau))
                        strReturn += string.Format("<td>{0}<br><a href='{1}'>sửa</a> - <a href='{2}'>Huỷ</a></td>", drs[0]["HoVaTenCB"].ToString(), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit", "mid/" + this.ModuleId.ToString(), "type/edit", "id/" + drs[0]["ID"].ToString(), "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString()), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "delete", "mid/" + this.ModuleId.ToString(), "type/delete", "id/" + drs[0]["ID"].ToString(), "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString()));
                    else
                        strReturn += string.Format("<td>{0}<br></td>", drs[0]["HoVaTenCB"].ToString());
                }
                else
                {
                    strReturn += string.Format("<td></td>");
                    if (checkEdit(NgayBatDau))
                    {
                        strReturn += string.Format("<td><a href='{0}'>Đăng kí</a></td>", DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit", "mid/" + this.ModuleId.ToString(), "type/new", "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString(), "ngay/" + NgayBatDau.AddDays(i - 1).ToFileTimeUtc().ToString(), "ca/4"));
                    }
                    else
                        strReturn += string.Format("<td>{0}</td>", "---");
                }

                drs = dt.Select(string.Format("PhongHocID={0} and Thu='{1}' and CaHocID={2}", Phong, i, 5));
                // Lấy dữ liệu ngày thứ 2
                if (drs.Length > 0)
                {
                    if (drs[0]["status"].ToString().Equals("1"))
                        strReturn += string.Format("<td style='color:red;'>{0}<br>{1} <br>Chờ duyệt</td>", drs[0]["Lop"].ToString(), drs[0]["MonHoc"].ToString());
                    else
                        strReturn += string.Format("<td>{0}<br>{1}</td>", drs[0]["Lop"].ToString(), drs[0]["MonHoc"].ToString());
                    if (checkEdit(drs, NgayBatDau))
                        strReturn += string.Format("<td>{0}<br><a href='{1}'>sửa</a> - <a href='{2}'>Huỷ</a></td>", drs[0]["HoVaTenCB"].ToString(), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit", "mid/" + this.ModuleId.ToString(), "type/edit", "id/" + drs[0]["ID"].ToString(), "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString()), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "delete", "mid/" + this.ModuleId.ToString(), "type/delete", "id/" + drs[0]["ID"].ToString(), "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString()));
                    else
                        strReturn += string.Format("<td>{0}<br></td>", drs[0]["HoVaTenCB"].ToString());
                }
                else
                {
                    strReturn += string.Format("<td></td>");
                    if (checkEdit(NgayBatDau))
                    {
                        strReturn += string.Format("<td><a href='{0}'>Đăng kí</a></td>", DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit", "mid/" + this.ModuleId.ToString(), "type/new", "phong/" + Phong.ToString(), "tuan/" + iTuan.ToString(), "ngay/" + NgayBatDau.AddDays(i - 1).ToFileTimeUtc().ToString(), "ca/5"));
                    }
                    else
                        strReturn += string.Format("<td>{0}</td>", "---");
                }

                strReturn += string.Format("<td></td>");
                strReturn += "</tr>";
            }
            strReturn += "</table>";
            //DataRow[] drs = dt.Select(string.Format("PhongHocID={0}", Phong));
            //DataRow [] drPhongHocs=dtPhongHoc.Select(string.Format("PhongHocID={0}", Phong));
            //if (drPhongHocs != null && drPhongHocs.Length > 0)
            //{
            //    strReturn+= string.Format("<tr><td style='font-size:18px;'>Phòng máy số {0}</tr></td>", drPhongHocs[0]["SubnetMask2"].ToString());
            //    strReturn += "</table>";
            //    return strReturn;
            //}
            strReturn += string.Format("<div style='text-align:right;font-weight:bold; font-size:12px;padding-top:10px;padding-bottom:2px;'><a href='/ExportExcel.aspx?tuan={0}&type=6'>Export excel</a></div>", iTuan);
            strReturn += string.Format("<div style='padding-top:3px;padding-bottom:5px;height:10px;'></div>");
            return strReturn;
        }

        protected void ddlPhongHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?phong={1}&&tuan={2}", this.TabId, ddlPhongHoc.SelectedValue, ddlTuan.SelectedValue));
        }

        protected void ddlTuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?phong={1}&&tuan={2}", this.TabId, ddlPhongHoc.SelectedValue, ddlTuan.SelectedValue));
        }

        protected void btnTuanTruoc_Click(object sender, EventArgs e)
        {
            if (iTuan > 1)
                iTuan = iTuan - 1;
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?phong={1}&&tuan={2}", this.TabId, ddlPhongHoc.SelectedValue, iTuan));
        }

        protected void btnTuanHienTai_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?phong={1}&&tuan={2}", this.TabId, ddlPhongHoc.SelectedValue, iTuanHienTai));
        }

        protected void btnTuanSau_Click(object sender, EventArgs e)
        {
            if (iTuan < 48)
                iTuan = iTuan + 1;
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?phong={1}&&tuan={2}", this.TabId, ddlPhongHoc.SelectedValue, iTuan));
        }
    }
}