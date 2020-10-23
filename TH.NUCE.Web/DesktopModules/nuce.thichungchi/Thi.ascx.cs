using DotNetNuke.Entities.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using nuce.web.data;
using System.IO;
using nuce.web;
using System.Globalization;
using DotNetNuke.Services.Log.EventLog;
using Newtonsoft.Json;
using System.Web;

namespace nuce.ad.thichungchi
{
    public partial class Thi : PortalModuleBase
    {

        public web.model.SinhVien m_SinhVien;
        public Dictionary<int, web.model.KiThiLopHocSinhVien> m_KiThiLopHocSinhViens;
        web.model.KiThiLopHocSinhVien m_KiThiLopHocSinhVien;
        protected override void OnInit(EventArgs e)
        {
            m_KiThiLopHocSinhViens = new Dictionary<int, web.model.KiThiLopHocSinhVien>();
            if (Session[Utils.session_sinhvien] == null)
            {
                //Chuyển đến trang đăng nhập
                Response.Redirect("/thi/dangnhap");
            }
            m_SinhVien = (web.model.SinhVien)Session[Utils.session_sinhvien];
            if (Session[Utils.session_kithi_lophoc_sinhvien] != null)
                m_KiThiLopHocSinhViens = (Dictionary<int, web.model.KiThiLopHocSinhVien>)Session[Utils.session_kithi_lophoc_sinhvien];



            base.OnInit(e);
        }
        public void writeLog(string type, string log)
        {
            EventLogController eventLog = new EventLogController();
            DotNetNuke.Services.Log.EventLog.LogInfo logInfo = new LogInfo();
            logInfo.LogUserID = UserId;

            logInfo.LogPortalID = PortalSettings.PortalId;
            logInfo.LogTypeKey = EventLogController.EventLogType.ADMIN_ALERT.ToString();
            logInfo.AddProperty("tabid=", this.TabId.ToString());
            logInfo.AddProperty("moduleid=", this.ModuleId.ToString());
            logInfo.AddProperty("Loai=", type);
            logInfo.AddProperty("ThongTin=", log);
            eventLog.AddLog(logInfo);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["kithilophocsinhvien"] == null)
                {
                    Response.Redirect("/thi/DanhSachKiThi");
                    return;
                }
                int iKiThiLopHocSinhVienID = -1;
                if (!int.TryParse(Request.QueryString["kithilophocsinhvien"], out iKiThiLopHocSinhVienID))
                {
                    Response.Redirect("/thi/DanhSachKiThi");
                    return;
                }
                if (!m_KiThiLopHocSinhViens.ContainsKey(iKiThiLopHocSinhVienID))
                {
                    Response.Redirect("/thi/DanhSachKiThi");
                    return;
                }
                web.model.KiThiLopHocSinhVien KiThiLopHocSinhVien = m_KiThiLopHocSinhViens[iKiThiLopHocSinhVienID];
                //Da thong tin chung
                divTenKiThi.InnerHtml = KiThiLopHocSinhVien.TenKiThi;
                divThoiGian.InnerHtml = string.Format("{0} phút", KiThiLopHocSinhVien.TongThoiGianThi);
                divPhongThi.InnerHtml = KiThiLopHocSinhVien.TenBlockHoc;
                divMaThiSinh.InnerHtml = m_SinhVien.MaSV;
                divTenThiSinh.InnerHtml = m_SinhVien.Ho + " " + m_SinhVien.Ten;
                divCMT.InnerHtml = m_SinhVien.CMT;

                string strJsThamSo = string.Format(@"var strCtl='{0}';", txtAnswares.ClientID.Replace("txtAnswares", ""));
                string strScript = "<script>";
                strScript += strJsThamSo;
                /*
                 * 1. Mơi
                 * 2. Đang thi
                 * 3. Tam dung thi
                 * 4. Thi xong
                 * 5. Huy thi
                 * 6. Thi lai
                 */
                strScript += string.Format("var checkOnbeforeunload={0};", 0);
                switch (KiThiLopHocSinhVien.Status)
                {
                    case 1:
                    case 6:
                        //Tao moi bo de
                        DataTable dtDeThi = web.data.dnn_NuceThi_DeThi.getRandomDeByBoDe(KiThiLopHocSinhVien.BoDeID);
                        if (dtDeThi.Rows.Count > 0)
                        {
                            int iDeThi = int.Parse(dtDeThi.Rows[0]["DeThiID"].ToString());
                            string strMa = dtDeThi.Rows[0]["Ma"].ToString();
                            string strNoiDungDeThi = dtDeThi.Rows[0]["NoiDungDeThi"].ToString();
                            string strDapAn = dtDeThi.Rows[0]["DapAn"].ToString();
                            // cap nhat vao csdl
                            web.data.dnn_NuceThi_KiThi_LopHoc_SinhVien.update_dethi(KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien, iDeThi, strNoiDungDeThi, strDapAn, KiThiLopHocSinhVien.TongThoiGianThi, KiThiLopHocSinhVien.TongThoiGianThi * 60, strMa, Utils.GetIPAddress(), 2);
                            // cap nhat vao session
                            KiThiLopHocSinhVien.DeThiID = iDeThi;
                            KiThiLopHocSinhVien.MaDe = strMa;
                            KiThiLopHocSinhVien.NoiDungDeThi = strNoiDungDeThi;
                            KiThiLopHocSinhVien.DapAn = strDapAn;
                            KiThiLopHocSinhVien.Status = 2;
                            KiThiLopHocSinhVien.TongThoiGianConLai = KiThiLopHocSinhVien.TongThoiGianThi * 60;
                        }
                        else
                        {
                            writeLog("Canh Bao", "Khong lay duoc de random cho bo de" + KiThiLopHocSinhVien.BoDeID);
                        }
                        break;
                    case 2:
                    case 3:
                        KiThiLopHocSinhVien.Status = 2;
                        break;
                    default:
                        break;
                }
                if (KiThiLopHocSinhVien.Status < 3)
                {
                    KiThiLopHocSinhVien.NgayGioBatDau = DateTime.Now;
                    m_KiThiLopHocSinhViens[iKiThiLopHocSinhVienID] = KiThiLopHocSinhVien;
                    Session[Utils.session_kithi_lophoc_sinhvien] = m_KiThiLopHocSinhViens;
                    List<web.model.CauHoi> lsCauHois = JsonConvert.DeserializeObject<List<web.model.CauHoi>>(KiThiLopHocSinhVien.NoiDungDeThi);
                    int iSoCauHoi = lsCauHois.Count;

                    strScript += string.Format("var totalTime={0};", KiThiLopHocSinhVien.TongThoiGianConLai);
                    strScript += string.Format("var iIDKiThiLopHocSinhVien={0};", KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien);

                    spMaDe.InnerText = KiThiLopHocSinhVien.MaDe;
                    divTongSoCau.InnerText = iSoCauHoi.ToString();
                    // 
                    string strHtmlMenuCauHoi = "<table style='width:100%; margin: 0 auto; border-color:cornflowerblue;' border='1px'>";
                    int iSoCauHoi6 = iSoCauHoi / 6;
                    if (iSoCauHoi6 > 0)
                    {
                        for (int i = 0; i < iSoCauHoi6; i++)
                        {
                            strHtmlMenuCauHoi += "<tr>";
                            strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='height: 26px;background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;border-color:cornflowerblue;'><span id='sp_cauhoi_{0}'>{1}</span></td>", lsCauHois[i * 6].CauHoiID, i * 6 + 1);
                            strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='height: 26px;background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;border-color:cornflowerblue;'><span id='sp_cauhoi_{0}'>{1}</span></td>", lsCauHois[i * 6 + 1].CauHoiID, i * 6 + 2);
                            strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='height: 26px;background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;border-color:cornflowerblue;'><span id='sp_cauhoi_{0}'>{1}</span></td>", lsCauHois[i * 6 + 2].CauHoiID, i * 6 + 3);
                            strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='height: 26px;background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;border-color:cornflowerblue;'><span id='sp_cauhoi_{0}'>{1}</span></td>", lsCauHois[i * 6 + 3].CauHoiID, i * 6 + 4);
                            strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='height: 26px;background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;border-color:cornflowerblue;'><span id='sp_cauhoi_{0}'>{1}</span></td>", lsCauHois[i * 6 + 4].CauHoiID, i * 6 + 5);
                            strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='height: 26px;background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;border-color:cornflowerblue;'><span id='sp_cauhoi_{0}'>{1}</span></td>", lsCauHois[i * 6 + 5].CauHoiID, i * 6 + 6);
                            strHtmlMenuCauHoi += "</tr>";
                        }
                    }
                    if (iSoCauHoi - iSoCauHoi6 * 6 > 0)
                    {
                        strHtmlMenuCauHoi += "<tr>";
                        for (int i = iSoCauHoi6 * 6 + 1; i <= iSoCauHoi; i++)
                        {
                            strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='height: 26px;background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;border-color:cornflowerblue;'><span id='sp_cauhoi_{0}'>{1}</span></td>", lsCauHois[i - 1].CauHoiID, i);
                        }
                        for (int i = iSoCauHoi + 1; i <= iSoCauHoi6 * 6 + 6; i++)
                        {
                            strHtmlMenuCauHoi += string.Format("<td style='border-color:cornflowerblue;'></td>");
                        }
                        strHtmlMenuCauHoi += "</tr>";
                    }
                    strHtmlMenuCauHoi += "</table>";
                    divMenuCauHoi.InnerHtml = strHtmlMenuCauHoi;
                    strScript += "</script>";
                    divInitData.InnerHtml = strScript;
                    List<web.model.DapAn> lsDapAns = Utils.convertListDapAnFromAnswares(KiThiLopHocSinhVien.BaiLam);
                    string strDanhSachCauHoi = "";
                    string strDanhSachDapAn = "";
                    string strCheckBoxDapAn = "";
                    strScript = "<script>";
                    int l = 0;
                    foreach (web.model.CauHoi cauhoi in lsCauHois)
                    {
                        l++;
                        List<web.model.DapAn> dapAn = lsDapAns.FindAll(x => x.CauHoiID.Equals(cauhoi.CauHoiID));
                        string strType = cauhoi.Type;
                        switch (strType)
                        {
                            case "SC":
                            case "TQ":
                                string strOutScript = "";
                                string strOutHtml = "";
                                string strOutHtml1 = "";
                                strDanhSachCauHoi += getItemHtmlNormal(cauhoi, dapAn, l, l * 20, l.ToString(), "blue", out strOutScript, out strOutHtml, out strOutHtml1);
                                strDanhSachDapAn += strOutHtml;
                                strCheckBoxDapAn += strOutHtml1;
                                strScript += strOutScript;
                                break;
                            default: break;
                        }

                    }
                    strScript += "collectAnswares();</script>";
                    divRunData.InnerHtml = strScript;
                    divDanhSachDapAn.InnerHtml = strDanhSachDapAn;
                    divDanhSachCauHoi.InnerHtml = strDanhSachCauHoi;
                    divCheckBoxDapAn.InnerHtml = strCheckBoxDapAn;
                }
                else
                {
                    // da thi xong hoac huy thi;
                    Response.Redirect("/Thi/DanhSachKiThi");
                }
            }
        }
        string getItemHtmlNormal(web.model.CauHoi cauhoi, List<web.model.DapAn> dapAn, int l, int l1, string index, string color, out string strScript, out string strNoiDungDapAn,out string strCheckboxDapAn)
        {
            #region loai binh thuong
            string strHtml = "";
            strScript = "";
            strNoiDungDapAn = string.Format("<table> <tr id=\"trCauTraLoi{0}\" style=\"border-color:cornflowerblue;text - align: center; font - weight: bold;\">", cauhoi.CauHoiID);
            string strMatch = "";
            if (dapAn != null)
            {
                //string strMatch = dapAn != null ? dapAn.Match : "";
                foreach (web.model.DapAn dapAnTemp in dapAn)
                    strMatch += ";" + dapAnTemp.Match + ";";
            }

            
            string strType = cauhoi.Type;
            strHtml += string.Format("<div id='divCauHoi_{0}'><div style='width: 100%; text-align:left; padding-bottom: 2px; padding-top: 2px;'><span style='font-weight: bold;'>Câu {2}: </span><span>{1}</span></div>", cauhoi.CauHoiID, HttpUtility.HtmlDecode(cauhoi.Content), index);
            if (!(cauhoi.Image.ToUpper().Equals("") || cauhoi.Image.ToUpper().Equals("NULL")))
            {
                strHtml += string.Format("<div style='width: 100%;text-align:center; padding-bottom: 2px; padding-top: 2px;'><image src='/Portals/{0}/{1}' hight='100px;'></image></div>", this.PortalId, cauhoi.Image);
            }

            int iCount = cauhoi.SoCauTraLoi;
            int i = 1;
            string strHtml1 = "";
            string strScript1 = "";

            while (i < (iCount + 1))
            {
                #region old
                switch (i)
                {
                    case 1:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M1)))
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M1), HttpUtility.HtmlDecode(cauhoi.A1), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript1 += "$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M1 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M1), HttpUtility.HtmlDecode(cauhoi.A1), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 2:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M2)))
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M2), HttpUtility.HtmlDecode(cauhoi.A2), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript1 += "$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M2 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M2), HttpUtility.HtmlDecode(cauhoi.A2), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 3:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M3)))
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M3), HttpUtility.HtmlDecode(cauhoi.A3), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript1 += "$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M3 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M3), HttpUtility.HtmlDecode(cauhoi.A3), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 4:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M4)))
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M4), HttpUtility.HtmlDecode(cauhoi.A4), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript1 += "$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M4 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M4), HttpUtility.HtmlDecode(cauhoi.A4), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 5:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M5)))
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M5), HttpUtility.HtmlDecode(cauhoi.A5), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript1 += "$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M5 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M5), HttpUtility.HtmlDecode(cauhoi.A5), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 6:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M6)))
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M6), HttpUtility.HtmlDecode(cauhoi.A6), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript1 += "$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M6 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml1 += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M6), HttpUtility.HtmlDecode(cauhoi.A6), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    default:break;
                }
                #endregion
                #region new
                switch (i)
                {
                    case 1:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M1)))
                        {
                        }
                        else
                        {
                        }
                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span style='padding-left: 5px;padding-right: 5px;'>{0}. {1}</span></div>", "A", HttpUtility.HtmlDecode(cauhoi.A1));
                        strNoiDungDapAn += string.Format("<td id='td_ket_qua_{0}_{1}' onclick=\"clickKetQua('{0}_{1}')\" style=\"height: 36px; background: rgb(230, 230, 230) none repeat scroll 0 % 0 %; text - align: center; font - weight: bold; padding: 3px; color: red; cursor: pointer; border-color:cornflowerblue;\">{2}</td>", cauhoi.CauHoiID, cauhoi.M1, "A");
                        break;
                    case 2:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M2)))
                        {
                        }
                        else
                        {
                        }
                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span style='padding-left: 5px;padding-right: 5px;'>{0}. {1}</span></div>", "B", HttpUtility.HtmlDecode(cauhoi.A2));
                        strNoiDungDapAn += string.Format("<td id='td_ket_qua_{0}_{1}' onclick=\"clickKetQua('{0}_{1}')\" style=\"height: 36px; background: rgb(230, 230, 230) none repeat scroll 0 % 0 %; text - align: center; font - weight: bold; padding: 3px; color: red; cursor: pointer; border-color:cornflowerblue;\">{2}</td>", cauhoi.CauHoiID, cauhoi.M2, "B");
                        break;
                    case 3:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M3)))
                        {
                        }
                        else
                        {
                        }
                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span style='padding-left: 5px;padding-right: 5px;'>{0}. {1}</span></div>", "C", HttpUtility.HtmlDecode(cauhoi.A3));
                        strNoiDungDapAn += string.Format("<td id='td_ket_qua_{0}_{1}' onclick=\"clickKetQua('{0}_{1}')\" style=\"height: 36px; background: rgb(230, 230, 230) none repeat scroll 0 % 0 %; text - align: center; font - weight: bold; padding: 3px; color: red; cursor: pointer;border-color:cornflowerblue; \">{2}</td>", cauhoi.CauHoiID, cauhoi.M3, "C");
                        break;
                    case 4:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M4)))
                        {
                        }
                        else
                        {
                        }
                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span style='padding-left: 5px;padding-right: 5px;'>{0}. {1}</span></div>", "D", HttpUtility.HtmlDecode(cauhoi.A4));
                        strNoiDungDapAn += string.Format("<td id='td_ket_qua_{0}_{1}' onclick=\"clickKetQua('{0}_{1}')\" style=\"height: 36px; background: rgb(230, 230, 230) none repeat scroll 0 % 0 %; text - align: center; font - weight: bold; padding: 3px; color: red; cursor: pointer; border-color:cornflowerblue;\">{2}</td>", cauhoi.CauHoiID, cauhoi.M4, "D");
                        break;
                    case 5:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M5)))
                        {
                        }
                        else
                        {
                        }
                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span style='padding-left: 5px;padding-right: 5px;'>{0}. {1}</span></div>", "E", HttpUtility.HtmlDecode(cauhoi.A5));
                        strNoiDungDapAn += string.Format("<td id='td_ket_qua_{0}_{1}' onclick=\"clickKetQua('{0}_{1}')\" style=\"height: 36px; background: rgb(230, 230, 230) none repeat scroll 0 % 0 %; text - align: center; font - weight: bold; padding: 3px; color: red; cursor: pointer; border-color:cornflowerblue;\">{2}</td>", cauhoi.CauHoiID, cauhoi.M5, "E");
                        break;
                    case 6:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M6)))
                        {
                        }
                        else
                        {
                        }
                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span style='padding-left: 5px;padding-right: 5px;'>{0}. {1}</span></div>", "F", HttpUtility.HtmlDecode(cauhoi.A6));
                        strNoiDungDapAn += string.Format("<td id='td_ket_qua_{0}_{1}' onclick=\"clickKetQua('{0}_{1}')\" style=\"height: 36px; background: rgb(230, 230, 230) none repeat scroll 0 % 0 %; text - align: center; font - weight: bold; padding: 3px; color: red; cursor: pointer;border-color:cornflowerblue; \">{2}</td>", cauhoi.CauHoiID, cauhoi.M6, "F");
                        break;
                    default: break;
                }
                #endregion
                i++;
            }
            strNoiDungDapAn += "</tr></table>";
            strHtml += "</div>";
            #endregion
            strCheckboxDapAn = strHtml1;
            strScript += strScript1;
            return strHtml;
        }

    }
}
