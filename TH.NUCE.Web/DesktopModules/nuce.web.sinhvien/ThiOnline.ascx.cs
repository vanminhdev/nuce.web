using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace nuce.web.sinhvien
{
    public partial class ThiOnline : CoreModule
    {
        model.KiThiLopHocSinhVien m_KiThiLopHocSinhVien;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["kithilophocsinhvien"] == null)
            {
                writeLog("Canh Bao", "Url bi sai " + Request.QueryString["kithilophocsinhvien"]);
                Response.Redirect(string.Format("/tabid/{0}/default.aspx", 119));
                return;
            }
            int iKiThiLopHocSinhVienID = -1;
            if (!int.TryParse(Request.QueryString["kithilophocsinhvien"], out iKiThiLopHocSinhVienID))
            {
                writeLog("Canh Bao", "Url bi sai " + Request.QueryString["kithilophocsinhvien"]);
                Response.Redirect(string.Format("/tabid/{0}/default.aspx", 119));
                return;
            }
            if (!m_KiThiLopHocSinhViens.ContainsKey(iKiThiLopHocSinhVienID))
            {
                writeLog("Canh Bao", "Khong co du lieu voi ki thi lop hoc sinh vien " + iKiThiLopHocSinhVienID);
                Response.Redirect(string.Format("/tabid/{0}/default.aspx", 119));
                return;
            }
            model.KiThiLopHocSinhVien KiThiLopHocSinhVien = m_KiThiLopHocSinhViens[iKiThiLopHocSinhVienID];

            string strScript = "<script>";

            if (KiThiLopHocSinhVien.Status.Equals(4))
            {
                strScript += string.Format("var checkOnbeforeunload={0};", 1);
                divMenu.Visible = false;
                writeLog("Canh Bao", "Ket qua thi voi ma de " + KiThiLopHocSinhVien.MaDe + " là: " + KiThiLopHocSinhVien.Mota);
                //divContent.InnerHtml = string.Format("<div style='width: 80%;text-align: center;font-weight: bold;font-size: 20px;color: red;padding-top: 20px;'>{0}</div>", KiThiLopHocSinhVien.Mota);
                divContent.InnerHtml = KiThiLopHocSinhVien.Mota;
            }
            else
            {
                // Cap nhat mat khau neu loai ki thi la 2
                if(KiThiLopHocSinhVien.LoaiKiThi.Equals(2))
                {
                    data.dnn_NuceThi_KiThi_LopHoc_SinhVien.updateMatKhau(KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien,Utils.getRandom(99999999,999999999).ToString());
                }

                strScript += string.Format("var checkOnbeforeunload={0};", 0);
                divMenu.Visible = true;
                // status=1: Bắt đầu
                // status=2: Chuan bi thi
                // status=3: Dang thi
                // status=4: Da thi xong
                // status=5: Thi tiep
                // status=6: thi lai
                // status=9: cấm thi
                // status=8: Không thi
                if (KiThiLopHocSinhVien.Status.Equals(3))
                {
                    // Co the la truong hop refresh
                    TimeSpan ts = DateTime.Now.Subtract(KiThiLopHocSinhVien.NgayGioBatDau);
                    int iTongThoiGianConLai = KiThiLopHocSinhVien.TongThoiGianConLai - (ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds);
                    KiThiLopHocSinhVien.TongThoiGianConLai = iTongThoiGianConLai;
                }
                if (KiThiLopHocSinhVien.Status.Equals(2) || KiThiLopHocSinhVien.Status.Equals(6))
                {
                    // lay de va du lieu tu phia server
                    DataTable dtDeThi = data.dnn_NuceThi_DeThi.getRandomDeByBoDe(KiThiLopHocSinhVien.BoDeID);
                    if (dtDeThi.Rows.Count > 0)
                    {
                        int iDeThi = int.Parse(dtDeThi.Rows[0]["DeThiID"].ToString());
                        string strMa = dtDeThi.Rows[0]["Ma"].ToString();
                        string strNoiDungDeThi = dtDeThi.Rows[0]["NoiDungDeThi"].ToString();
                        string strDapAn = dtDeThi.Rows[0]["DapAn"].ToString();
                        // cap nhat vao csdl
                        data.dnn_NuceThi_KiThi_LopHoc_SinhVien.update_dethi(KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien, iDeThi, strNoiDungDeThi, strDapAn, KiThiLopHocSinhVien.TongThoiGianThi, KiThiLopHocSinhVien.TongThoiGianConLai, strMa, Utils.GetIPAddress(), 3);
                        // cap nhat vao session
                        KiThiLopHocSinhVien.DeThiID = iDeThi;
                        KiThiLopHocSinhVien.MaDe = strMa;
                        KiThiLopHocSinhVien.NoiDungDeThi = strNoiDungDeThi;
                        KiThiLopHocSinhVien.DapAn = strDapAn;
                        KiThiLopHocSinhVien.Status = 3;
                    }
                    else
                    {
                        writeLog("Canh Bao", "Khong lay duoc de random cho bo de" + KiThiLopHocSinhVien.BoDeID);
                    }
                }
                if (KiThiLopHocSinhVien.Status.Equals(5))
                {
                    KiThiLopHocSinhVien.Status = 3;
                    data.dnn_NuceThi_KiThi_LopHoc_SinhVien.updateStatus(iKiThiLopHocSinhVienID, 3);
                }

                KiThiLopHocSinhVien.NgayGioBatDau = DateTime.Now;
                m_KiThiLopHocSinhViens[iKiThiLopHocSinhVienID] = KiThiLopHocSinhVien;
                Session[Utils.session_kithi_lophoc_sinhvien] = m_KiThiLopHocSinhViens;

                strScript += string.Format("var totalTime={0};", KiThiLopHocSinhVien.TongThoiGianConLai);
                strScript += string.Format("var iIDKiThiLopHocSinhVien={0};", KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien);

                List<model.CauHoi> lsCauHois = JsonConvert.DeserializeObject<List<model.CauHoi>>(KiThiLopHocSinhVien.NoiDungDeThi);
                int iSoCauHoi = lsCauHois.Count;
                string strHtmlMenuCauHoi = "<table style='width:90%; margin: 0 auto;' border='1px'>";
                int iSoCauHoi6 = iSoCauHoi / 6;
                if (iSoCauHoi6 > 0)
                {
                    for (int i = 0; i < iSoCauHoi6; i++)
                    {
                        strHtmlMenuCauHoi += "<tr>";
                        strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;'>{0}</td>", (i * 6) + 1);
                        strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;'>{0}</td>", (i * 6) + 2);
                        strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;'>{0}</td>", (i * 6) + 3);
                        strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;'>{0}</td>", (i * 6) + 4);
                        strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;'>{0}</td>", (i * 6) + 5);
                        strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;'>{0}</td>", (i * 6) + 6);
                        strHtmlMenuCauHoi += "</tr>";
                    }
                }
                if (iSoCauHoi - iSoCauHoi6 * 6 > 0)
                {
                    strHtmlMenuCauHoi += "<tr>";
                    for (int i = iSoCauHoi6 * 6 + 1; i <= iSoCauHoi; i++)
                    {
                        strHtmlMenuCauHoi += string.Format("<td id='tdMenuCauHoi_{0}' onclick='gotocauhoi({0});' style='background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;'>{0}</td>", i);
                    }
                    for (int i = iSoCauHoi + 1; i <= iSoCauHoi6 * 6 + 6; i++)
                    {
                        strHtmlMenuCauHoi += string.Format("<td></td>");
                    }
                    strHtmlMenuCauHoi += "</tr>";
                }
                strHtmlMenuCauHoi += "</table>";

                divMenuCauHoi.InnerHtml = strHtmlMenuCauHoi;
                string strOutScript = "";
                divContent.InnerHtml = getHtmlDeThi(KiThiLopHocSinhVien, out strOutScript);
                //strScript += strOutScript;
                divProcessData.InnerHtml = string.Format("<script>{0}</script>", strOutScript);
                m_KiThiLopHocSinhVien = KiThiLopHocSinhVien;
                //divContent.InnerHtml = KiThiLopHocSinhVien.MaDe + "---" + KiThiLopHocSinhVien.NoiDungDeThi + "---" + KiThiLopHocSinhVien.DapAn;
            }
            strScript += "</script>";
            divInitData.InnerHtml = strScript;

            // Khoa tai khoan khi nao thi
        }
        string getDivRowHtmlNormal(model.CauHoi cauhoi, List<model.DapAn> dapAn, int l,int l1, string index,string color,out string strScript)
        {
            #region loai binh thuong
            string strHtml = "";
            strScript = "";
            string strMatch = "";
            if(dapAn != null)
            {
                //string strMatch = dapAn != null ? dapAn.Match : "";
                foreach(model.DapAn dapAnTemp in dapAn)
                strMatch += ";" + dapAnTemp.Match + ";";
            }
           
            
            string strType = cauhoi.Type;
            strHtml += string.Format("<div id='divCauHoi_{0}' style='width: 100%;color:{2}; text-align:left; padding-bottom: 2px; padding-top: 2px;'><span style='font-weight: bold;'>Câu {0}: </span><span>{1}</span></div>", index, HttpUtility.HtmlDecode(cauhoi.Content),color);
            if (!(cauhoi.Image.ToUpper().Equals("") || cauhoi.Image.ToUpper().Equals("NULL")))
            {
                strHtml += string.Format("<div style='width: 100%;text-align:center; padding-bottom: 2px; padding-top: 2px;'><image src='/Portals/{0}/{1}' hight='100px;'></image></div>", this.PortalId, cauhoi.Image);
            }

            int iCount = cauhoi.SoCauTraLoi;
            int i = 1;
            
            while (i < (iCount + 1))
            {
                switch (i)
                {
                    case 1:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M1)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M1), HttpUtility.HtmlDecode(cauhoi.A1), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M1 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M1), HttpUtility.HtmlDecode(cauhoi.A1), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 2:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M2)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M2), HttpUtility.HtmlDecode(cauhoi.A2), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M2 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M2), HttpUtility.HtmlDecode(cauhoi.A2), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 3:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M3)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M3), HttpUtility.HtmlDecode(cauhoi.A3), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M3 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M3), HttpUtility.HtmlDecode(cauhoi.A3), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 4:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M4)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M4), HttpUtility.HtmlDecode(cauhoi.A4), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M4 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M4), HttpUtility.HtmlDecode(cauhoi.A4), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 5:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M5)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M5), HttpUtility.HtmlDecode(cauhoi.A5), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M5 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M5), HttpUtility.HtmlDecode(cauhoi.A5), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 6:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M6)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M6), HttpUtility.HtmlDecode(cauhoi.A6), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M6 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M6), HttpUtility.HtmlDecode(cauhoi.A6), cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 7:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M7)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M7, cauhoi.A7, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M7 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M7, cauhoi.A7, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 8:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M8)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M8, cauhoi.A8, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M8 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M8, cauhoi.A8, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 9:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M9)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M9, cauhoi.A9, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M9 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M9, cauhoi.A9, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                    case 10:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M10)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M10, cauhoi.A10, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M10 + "').prop(\"checked\", true);";
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M10, cauhoi.A10, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                        }
                        break;
                }
                i++;
            }
            #endregion
            return strHtml;
        }
        public string getHtmlDeThi(model.KiThiLopHocSinhVien KiThiLopHocSinhVien, out string strScript)
        {
            strScript = "function InitData() {";
            string strScriptTemp = "";
            string strHtml = string.Format("<div style='width: 100%; text-align:center; padding-bottom: 10px; padding-top: 10px; font-weight: bold; color:red;'>{0} (Môn học {1} block học {2}) Mã Đề: {3}</div>", KiThiLopHocSinhVien.TenKiThi, KiThiLopHocSinhVien.TenMonHoc, KiThiLopHocSinhVien.TenBlockHoc, KiThiLopHocSinhVien.MaDe);
            List<model.CauHoi> lsCauHois = JsonConvert.DeserializeObject<List<model.CauHoi>>(KiThiLopHocSinhVien.NoiDungDeThi);
            //List<model.DapAn> lsDapAns = JsonConvert.DeserializeObject<List<model.DapAn>>(KiThiLopHocSinhVien.DapAn);
            string strAnswares = KiThiLopHocSinhVien.BaiLam;
            List<model.DapAn> lsDapAns = Utils.convertListDapAnFromAnswares(strAnswares);
            int l = 0;
            int l1 = 0;
            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-bottom: 2px; padding-top: 2px;padding-left: 10px;padding-right: 10px;'>");
            foreach (model.CauHoi cauhoi in lsCauHois)
            {
                l++;
                List<model.DapAn> dapAn = lsDapAns.FindAll(x => x.CauHoiID.Equals(cauhoi.CauHoiID));
                //string strMatch = dapAn != null ? dapAn.Match : "";
                //strMatch = ";" + strMatch + ";";
                string strType = cauhoi.Type;
                switch (strType)
                {
                    case "SC":
                    case "MC":
                    case "TQ":
                    case "FQ":
                        strHtml += getDivRowHtmlNormal(cauhoi, dapAn, l,l*20, l.ToString(),"blue", out strScriptTemp);
                        strScript += strScriptTemp;
                        #region loai binh thuong
                        /*
                        strHtml += string.Format("<div id='divCauHoi_{0}' style='width: 100%;color:blue; text-align:left; padding-bottom: 2px; padding-top: 2px;'><span style='font-weight: bold;'>Câu {0}: </span><span>{1}</span></div>", l, cauhoi.Content);
                        int iCount = cauhoi.SoCauTraLoi;
                        int i = 1;
                        while (i < (iCount + 1))
                        {
                            switch (i)
                            {
                                case 1:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M1)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M1, cauhoi.A1, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_"+cauhoi.M1 +"').prop(\"checked\", true);";
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M1, cauhoi.A1, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                    }
                                    break;
                                case 2:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M2)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M2, cauhoi.A2, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M2 + "').prop(\"checked\", true);";
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M2, cauhoi.A2, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                    }
                                    break;
                                case 3:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M3)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M3, cauhoi.A3, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M3 + "').prop(\"checked\", true);";
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M3, cauhoi.A3, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                    }
                                    break;
                                case 4:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M4)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M4, cauhoi.A4, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M4 + "').prop(\"checked\", true);";
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M4, cauhoi.A4, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                    }
                                    break;
                                case 5:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M5)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M5, cauhoi.A5, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M5 + "').prop(\"checked\", true);";
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M5, cauhoi.A5, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                    }
                                    break;
                                case 6:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M6)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M6, cauhoi.A6, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M6 + "').prop(\"checked\", true);";
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M6, cauhoi.A6, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                    }
                                    break;
                                case 7:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M7)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M7, cauhoi.A7, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M7 + "').prop(\"checked\", true);";
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M7, cauhoi.A7, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                    }
                                    break;
                                case 8:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M8)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M8, cauhoi.A8, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M8 + "').prop(\"checked\", true);";
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M8, cauhoi.A8, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                    }
                                    break;
                                case 9:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M9)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M9, cauhoi.A9, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M9 + "').prop(\"checked\", true);";
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M9, cauhoi.A9, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                    }
                                    break;
                                case 10:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M10)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M10, cauhoi.A10, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M10 + "').prop(\"checked\", true);";
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M10, cauhoi.A10, cauhoi.CauHoiID, strType.Equals("MC") ? l1 * 20 + i : l1 * 20);
                                    }
                                    break;
                            }
                            i++;
                        }*/
                        #endregion
                        break;
                    case "TL":
                        strHtml += string.Format("<div id='divCauHoi_{0}' style='width: 100%;color:blue; text-align:left; padding-bottom: 2px; padding-top: 2px;'><span style='font-weight: bold;'>Câu {0}: </span><span>{1}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.Content));
                        if (!(cauhoi.Image.ToUpper().Equals("") || cauhoi.Image.ToUpper().Equals("NULL")))
                        {
                            strHtml += string.Format("<div style='width: 100%;text-align:center; padding-bottom: 2px; padding-top: 2px;'><image src='/Portals/{0}/{1}' hight='100px;'></image></div>", this.PortalId, cauhoi.Image);
                        }
                        if (cauhoi.ChildCauHois != null && cauhoi.ChildCauHois.Count > 0)
                        {
                            l1 = 0;
                            foreach (model.CauHoi cauhoi1 in cauhoi.ChildCauHois)
                            {
                                l1++;
                                strHtml += "<div style='padding-left:10px;'>";
                                dapAn = lsDapAns.FindAll(x => x.CauHoiID == cauhoi1.CauHoiID);
                                strHtml += getDivRowHtmlNormal(cauhoi1, dapAn,l,l*20+l1, string.Format("{0}.{1}", l, l1), "green",out strScriptTemp);
                                strHtml += "</div>";
                                strScript += strScriptTemp;
                            }
                        }
                        break;
                }
            }
            strHtml += "</div>";
            // strScript += " alert (' hi hi');};";
            strScript += " };";
            return strHtml;
        }

        protected void btnNopBai_Click(object sender, EventArgs e)
        {
            string strAnswares = txtAnswares.Text;
            m_KiThiLopHocSinhVien = Utils.chamBai(m_KiThiLopHocSinhVien, strAnswares);
            //m_KiThiLopHocSinhVien.Status = 4;
            //m_KiThiLopHocSinhVien.BaiLam = strAnswares;
            m_KiThiLopHocSinhViens[m_KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien] = m_KiThiLopHocSinhVien;

            // Cap nhat vao database
            data.dnn_NuceThi_KiThi_LopHoc_SinhVien.update_bailam1(m_KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien, m_KiThiLopHocSinhVien.BaiLam, m_KiThiLopHocSinhVien.Diem, m_KiThiLopHocSinhVien.NgayGioBatDau, DateTime.Now, m_KiThiLopHocSinhVien.TongThoiGianConLai, Utils.GetIPAddress(), m_KiThiLopHocSinhVien.Status);
            //data.dnn_NuceThi_KiThi_LopHoc_SinhVien.updateStatus2(m_KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien, m_KiThiLopHocSinhVien.Status, m_KiThiLopHocSinhVien.BaiLam, m_KiThiLopHocSinhVien.Diem);

            Response.Redirect(Request.RawUrl);

        }
    }
}