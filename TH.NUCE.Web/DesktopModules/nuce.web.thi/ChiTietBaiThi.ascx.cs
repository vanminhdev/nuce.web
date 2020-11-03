using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace nuce.web.thi
{
    public partial class ChiTietBaiThi : CoreModule
    {
        model.KiThiLopHocSinhVien m_KiThiLopHocSinhVien;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["kithilophocsinhvien"] == null)
            {
                writeLog("Canh Bao", "Url bi sai " + Request.QueryString["kithilophocsinhvien"]);
                //Response.Redirect(Request.UrlReferrer.ToString());
                return;
            }
            int iKiThiLopHocSinhVienID = -1;
            if (!int.TryParse(Request.QueryString["kithilophocsinhvien"], out iKiThiLopHocSinhVienID))
            {
                writeLog("Canh Bao", "Url bi sai " + Request.QueryString["kithilophocsinhvien"]);
                //Response.Redirect(Request.UrlReferrer.ToString());
                return;
            }
            DataTable dtKiThiLopHocSinhVien = data.dnn_NuceThi_KiThi_LopHoc_SinhVien.get(iKiThiLopHocSinhVienID);

            if (dtKiThiLopHocSinhVien.Rows.Count < 0)
            {
                writeLog("Canh Bao", "Khong co du lieu voi ki thi lop hoc sinh vien " + iKiThiLopHocSinhVienID);
                //Response.Redirect(Request.UrlReferrer.ToString());
                return;
            }
            model.KiThiLopHocSinhVien KiThiLopHocSinhVien = new model.KiThiLopHocSinhVien();

            //KiThiLopHocSinhVien.BoDeID = int.Parse(dtKiThiLopHocSinhVien.Rows[0]["BoDeID"].ToString());
            KiThiLopHocSinhVien.DeThiID = int.Parse(dtKiThiLopHocSinhVien.Rows[0]["DeThiID"].ToString());
            KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien = int.Parse(dtKiThiLopHocSinhVien.Rows[0]["KiThi_LopHoc_SinhVienID"].ToString());
            KiThiLopHocSinhVien.Status = int.Parse(dtKiThiLopHocSinhVien.Rows[0]["Status"].ToString());
            //KiThiLopHocSinhVien.TenBlockHoc = dtKiThiLopHocSinhVien.Rows[i]["TenBlockHoc"].ToString();
            //KiThiLopHocSinhVien.TenKiThi = dtKiThiLopHocSinhVien.Rows[i]["TenKiThi"].ToString();
            //KiThiLopHocSinhVien.TenMonHoc = dtKiThiLopHocSinhVien.Rows[i]["TenMonHoc"].ToString();
            KiThiLopHocSinhVien.NoiDungDeThi = dtKiThiLopHocSinhVien.Rows[0]["NoiDungDeThi"].ToString();
            KiThiLopHocSinhVien.DapAn = dtKiThiLopHocSinhVien.Rows[0]["DapAn"].ToString();
            KiThiLopHocSinhVien.Diem = float.Parse(dtKiThiLopHocSinhVien.Rows[0]["Diem"].ToString());
            KiThiLopHocSinhVien.BaiLam = dtKiThiLopHocSinhVien.Rows[0]["BaiLam"].ToString();
            KiThiLopHocSinhVien.MaDe = dtKiThiLopHocSinhVien.Rows[0].IsNull("MaDe") ? "" : dtKiThiLopHocSinhVien.Rows[0]["MaDe"].ToString();

            KiThiLopHocSinhVien.NgayGioBatDau = dtKiThiLopHocSinhVien.Rows[0].IsNull("NgayGioBatDau") ? DateTime.Now : DateTime.Parse(dtKiThiLopHocSinhVien.Rows[0]["NgayGioBatDau"].ToString());
            int iTongThoiGianConLai1 = int.Parse(dtKiThiLopHocSinhVien.Rows[0]["TongThoiGianConLai"].ToString());
            KiThiLopHocSinhVien.TongThoiGianConLai = iTongThoiGianConLai1 < 0 ? 0 : iTongThoiGianConLai1;
            KiThiLopHocSinhVien.TongThoiGianThi = int.Parse(dtKiThiLopHocSinhVien.Rows[0]["TongThoiGianThi"].ToString());

            string strMoTa = "<div style='padding-left:20%;width: 80%;text-align: center;font-weight: bold;font-size: 12px;color: red;padding-top: 20px;'>";
            strMoTa += string.Format("<table style='border-collapse: separate; margin:10px;'>");

            strMoTa += string.Format("<tr><td style='color:red;font-weight: bold;'>Mã SV: </td><td>{0}</td>", Request.QueryString["ma"]);
            strMoTa += string.Format("<td style='color:red;font-weight: bold;'>Tên sinh viên: </td><td>{0}</td></tr>", Request.QueryString["ten"]);
            if (KiThiLopHocSinhVien.Status.Equals(3) || KiThiLopHocSinhVien.Status.Equals(4) || KiThiLopHocSinhVien.Status.Equals(5))
            {
                strMoTa += string.Format("<tr><td style='color:red;font-weight: bold;'>Mã đề: </td><td>{0}</td>", KiThiLopHocSinhVien.MaDe);
                strMoTa += string.Format("<td style='color:red;font-weight: bold;'>Ngày nộp bài: </td><td>{0:dd/MM/yyyy - hh:mm:ss}</td></tr>", KiThiLopHocSinhVien.NgayGioBatDau);
                strMoTa += string.Format("<tr><td style='color:red;font-weight: bold;'>Tổng thời gian thi: </td><td>{0} phút</td>", KiThiLopHocSinhVien.TongThoiGianThi);
                strMoTa += string.Format("<td style='color:red;font-weight: bold;'>Tổng thời gian còn lại: </td><td>{0} phút {1} giây</td></tr>", KiThiLopHocSinhVien.TongThoiGianConLai / 60, KiThiLopHocSinhVien.TongThoiGianConLai % 60);
                strMoTa += string.Format("<tr><td style='color:red;font-weight: bold;'>Điểm: </td><td>{0:N2}</td>", KiThiLopHocSinhVien.Diem);
                strMoTa += string.Format("<td style='color:red;font-weight: bold;'>Địa chỉ nộp bài: </td><td>{0}</td></tr>", dtKiThiLopHocSinhVien.Rows[0]["LogIP"].ToString());
            }
            strMoTa += string.Format("<tr><td style='color:red;font-weight: bold;'>Ghi chú: </td><td colspan='3'>{0}</td></tr>", dtKiThiLopHocSinhVien.Rows[0]["MoTa"].ToString());
            strMoTa += "</table></div>";

            KiThiLopHocSinhVien.Mota = strMoTa;
            //KiThiLopHocSinhVien.Mota = string.Format("Bài thi được {0:N2} điểm", float.Parse(dtKiThiLopHocSinhVien.Rows[i]["Diem"].ToString()));

            string strScript = "<script>";

            if (KiThiLopHocSinhVien.Status.Equals(1) || KiThiLopHocSinhVien.Status.Equals(2) || KiThiLopHocSinhVien.Status.Equals(8)
                || KiThiLopHocSinhVien.Status.Equals(6) || KiThiLopHocSinhVien.Status.Equals(9))
            {
                divMenu.Visible = false;
                writeLog("Canh Bao", "Ket qua thi voi ma de " + KiThiLopHocSinhVien.MaDe + " là: " + KiThiLopHocSinhVien.Mota);
                //divContent.InnerHtml = string.Format("<div style='width: 80%;text-align: center;font-weight: bold;font-size: 20px;color: red;padding-top: 20px;'>{0}</div>", KiThiLopHocSinhVien.Mota);
                divContent.InnerHtml = KiThiLopHocSinhVien.Mota;
            }
            else
            {
                divMenu.Visible = true;

                // status=2: Chuan bi thi
                // status=3: Dang thi
                // status=4: Da thi xong
                // status=5: Thi tiep
                // status=6: thi lai
                if (KiThiLopHocSinhVien.Status.Equals(3))
                {
                    // Co the la truong hop refresh
                    TimeSpan ts = DateTime.Now.Subtract(KiThiLopHocSinhVien.NgayGioBatDau);
                    int iTongThoiGianConLai = KiThiLopHocSinhVien.TongThoiGianConLai - (ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds);
                    KiThiLopHocSinhVien.TongThoiGianConLai = iTongThoiGianConLai;
                }
                strScript += string.Format("var iIDKiThiLopHocSinhVien={0};", KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien);

                List<model.CauHoi> lsCauHois = JsonConvert.DeserializeObject<List<model.CauHoi>>(KiThiLopHocSinhVien.NoiDungDeThi);
                int iSoCauHoi = lsCauHois.Count;
                string strHtmlMenuCauHoi = "<table style='width:90%; margin: 0 auto;' border='1px'>";
                int iSoCauHoi6 = iSoCauHoi / 6;
                if (iSoCauHoi > 0)
                {
                    for (int i = 0; i < iSoCauHoi / 6; i++)
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
        }
        string getDivRowHtmlNormal(model.CauHoi cauhoi, List<model.DapAn> dapAn, model.DapAn answare, int l, int l1, string index, string color, out string strScript)
        {
            #region loai binh thuong
            string strHtml = "";
            strScript = "";
            string strMatch = "";
            if (dapAn != null)
            {
                //string strMatch = dapAn != null ? dapAn.Match : "";
                foreach (model.DapAn dapAnTemp in dapAn)
                    strMatch += ";" + dapAnTemp.Match + ";";
            }

            string strMatchDapAn = answare != null ? answare.Match : "";
            strMatchDapAn = ";" + strMatchDapAn + ";";

            string strType = cauhoi.Type;
            strHtml += string.Format("<div id='divCauHoi_{0}' style='width: 100%;color:{3}; text-align:left; padding-bottom: 2px; padding-top: 2px;'><span style='font-weight: bold;'>Câu {0} (<span style='color:red;'>{1} điểm</span>): </span><span>{2}</span></div>", index, cauhoi.Mark, HttpUtility.HtmlDecode(cauhoi.Content), color);
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
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M1 + "').prop(\"checked\", true);";
                        }
                        if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M1)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M1), HttpUtility.HtmlDecode(cauhoi.A1), cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M1), HttpUtility.HtmlDecode(cauhoi.A1), cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        break;
                    case 2:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M2)))
                        {
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M2 + "').prop(\"checked\", true);";
                        }
                        if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M2)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M2), HttpUtility.HtmlDecode(cauhoi.A2), cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M2), HttpUtility.HtmlDecode(cauhoi.A2), cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        break;
                    case 3:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M3)))
                        {
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M3 + "').prop(\"checked\", true);";
                        }
                        if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M3)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M3), HttpUtility.HtmlDecode(cauhoi.A3), cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M3), HttpUtility.HtmlDecode(cauhoi.A3), cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        break;
                    case 4:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M4)))
                        {
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M4 + "').prop(\"checked\", true);";
                        }
                        if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M4)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M4), HttpUtility.HtmlDecode(cauhoi.A4), cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M4), HttpUtility.HtmlDecode(cauhoi.A4), cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        break;
                    case 5:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M5)))
                        {
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M5 + "').prop(\"checked\", true);";
                        }
                        if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M5)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M5), HttpUtility.HtmlDecode(cauhoi.A5), cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, HttpUtility.HtmlDecode(cauhoi.M5), HttpUtility.HtmlDecode(cauhoi.A5), cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        break;
                    case 6:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M6)))
                        {
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M6 + "').prop(\"checked\", true);";
                        }
                        if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M6)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M6, cauhoi.A6, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M6, cauhoi.A6, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        break;
                    case 7:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M7)))
                        {
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M7 + "').prop(\"checked\", true);";
                        }
                        if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M7)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M7, cauhoi.A7, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M7, cauhoi.A7, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        break;
                    case 8:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M8)))
                        {
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M8 + "').prop(\"checked\", true);";
                        }
                        if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M8)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M8, cauhoi.A8, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M8, cauhoi.A8, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        break;
                    case 9:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M9)))
                        {
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M9 + "').prop(\"checked\", true);";
                        }
                        if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M9)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M9, cauhoi.A9, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M9, cauhoi.A9, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        break;
                    case 10:
                        if (strMatch.Contains(string.Format(";{0};", cauhoi.M10)))
                        {
                            strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M10 + "').prop(\"checked\", true);";
                        }
                        if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M10)))
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M10, cauhoi.A10, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                        }
                        else
                        {
                            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M10, cauhoi.A10, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
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
            List<model.DapAn> dapAn;
            model.DapAn dapAn1;
            strScript = "function InitData() {";
            string strScriptTemp = "";
            string strHtml = string.Format("<div style='width: 100%; text-align:center; padding-bottom: 10px; padding-top: 10px; font-weight: bold; color:red;'>{0}</div>", KiThiLopHocSinhVien.Mota);
            List<model.CauHoi> lsCauHois = JsonConvert.DeserializeObject<List<model.CauHoi>>(KiThiLopHocSinhVien.NoiDungDeThi);
            //List<model.DapAn> lsDapAns = JsonConvert.DeserializeObject<List<model.DapAn>>(KiThiLopHocSinhVien.DapAn);
            string strAnswares = KiThiLopHocSinhVien.BaiLam;
            List<model.DapAn> lsAnswares = Utils.convertListDapAnFromAnswares(strAnswares);
            List<model.DapAn> lsDapAns = JsonConvert.DeserializeObject<List<model.DapAn>>(KiThiLopHocSinhVien.DapAn);
            int l = 0;
            int l1 = 0;
            strHtml += string.Format("<div style='width: 100%; text-align:left; padding-bottom: 2px; padding-top: 2px;padding-left: 10px;padding-right: 10px;'>");
            foreach (model.CauHoi cauhoi in lsCauHois)
            {
                l++;
                dapAn = lsAnswares.FindAll(x => x.CauHoiID.Equals(cauhoi.CauHoiID));
                //string strMatch = dapAn != null ? dapAn.Match : "";
                //strMatch = ";" + strMatch + ";";

                dapAn1 = lsDapAns.Find(x => x.CauHoiID.Equals(cauhoi.CauHoiID));
                //string strMatchDapAn = dapAn1 != null ? dapAn1.Match : "";
                //strMatchDapAn = ";" + strMatchDapAn + ";";

                string strType = cauhoi.Type;
                switch (strType)
                {
                    case "SC":
                    case "MC":
                    case "TQ":
                    case "FQ":
                        strHtml += getDivRowHtmlNormal(cauhoi, dapAn, dapAn1, l, l * 20, l.ToString(), "blue", out strScriptTemp);
                        strScript += strScriptTemp;
                        #region loai binh thuong
                        /*
                        strHtml += string.Format("<div id='divCauHoi_{0}' style='width: 100%;color:blue; text-align:left; padding-bottom: 2px; padding-top: 2px;'><span style='font-weight: bold;'>Câu {0} (<span style='color:red;'>{1} điểm</span>): </span><span>{2}</span></div>", l, cauhoi.Mark, cauhoi.Content);
                        int iCount = cauhoi.SoCauTraLoi;
                        int i = 1;
                        while (i < (iCount + 1))
                        {
                            switch (i)
                            {
                                case 1:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M1)))
                                    {
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M1 + "').prop(\"checked\", true);";
                                    }
                                    if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M1)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M1, cauhoi.A1, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M1, cauhoi.A1, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    break;
                                case 2:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M2)))
                                    {
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M2 + "').prop(\"checked\", true);";
                                    }
                                    if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M2)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M2, cauhoi.A2, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M2, cauhoi.A2, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    break;
                                case 3:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M3)))
                                    {
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M3 + "').prop(\"checked\", true);";
                                    }
                                    if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M3)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M3, cauhoi.A3, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M3, cauhoi.A3, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    break;
                                case 4:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M4)))
                                    {
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M4 + "').prop(\"checked\", true);";
                                    }
                                    if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M4)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M4, cauhoi.A4, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M4, cauhoi.A4, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    break;
                                case 5:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M5)))
                                    {
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M5 + "').prop(\"checked\", true);";
                                    }
                                    if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M5)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M5, cauhoi.A5, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M5, cauhoi.A5, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    break;
                                case 6:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M6)))
                                    {
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M6 + "').prop(\"checked\", true);";
                                    }
                                    if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M6)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M6, cauhoi.A6, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M6, cauhoi.A6, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    break;
                                case 7:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M7)))
                                    {
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M7 + "').prop(\"checked\", true);";
                                    }
                                    if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M7)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M7, cauhoi.A7, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M7, cauhoi.A7, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    break;
                                case 8:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M8)))
                                    {
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M8 + "').prop(\"checked\", true);";
                                    }
                                    if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M8)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M8, cauhoi.A8, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M8, cauhoi.A8, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    break;
                                case 9:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M9)))
                                    {
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M9 + "').prop(\"checked\", true);";
                                    }
                                    if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M9)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M9, cauhoi.A9, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M9, cauhoi.A9, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    break;
                                case 10:
                                    if (strMatch.Contains(string.Format(";{0};", cauhoi.M10)))
                                    {
                                        strScript += "$('#tdMenuCauHoi_" + l + "').css({ \"color\": \"blue\" });$('#id_" + cauhoi.CauHoiID + "_" + cauhoi.M10 + "').prop(\"checked\", true);";
                                    }
                                    if (strMatchDapAn.Contains(string.Format(";{0};", cauhoi.M10)))
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;color:red;'>{2} (Là đáp án)</span></div>", l, cauhoi.M10, cauhoi.A10, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
                                    }
                                    else
                                    {
                                        strHtml += string.Format("<div style='width: 100%; text-align:left; padding-top: 1px;'><span><input type='checkbox' name='nCauHoi_{4}' value='vcauhoi_{0}' id='id_{3}_{1}'></span><span style='padding-left: 5px;padding-right: 5px;'>{2}</span></div>", l, cauhoi.M10, cauhoi.A10, cauhoi.CauHoiID, strType.Equals("MC") ? l * 20 + i : l * 20);
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
                                dapAn = lsAnswares.FindAll(x => x.CauHoiID.Equals(cauhoi1.CauHoiID));

                                dapAn1 = lsDapAns.Find(x => x.CauHoiID.Equals(cauhoi1.CauHoiID));
                                strHtml += getDivRowHtmlNormal(cauhoi1, dapAn, dapAn1, l, l * 20 + l1, string.Format("{0}.{1}", l, l1), "green", out strScriptTemp);
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
    }
}