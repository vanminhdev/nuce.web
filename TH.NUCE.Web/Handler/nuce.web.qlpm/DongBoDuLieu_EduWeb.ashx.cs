using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Services;
namespace nuce.web.qlpm
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DongBoDuLieu_EduWeb : CoreHandlerQLPMAdmin
    {
        public override void WriteData(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if ((context.Request["hockyid"] != null) && (context.Request["namhocid"] != null))
            {
                int hockyid = int.Parse(context.Request["hockyid"]);
                int namhocid = int.Parse(context.Request["namhocid"]);
                // Lay nam hoc
                DataTable dtNamHoc = data.dnn_NuceCommon_NamHoc.getByStatus(1);
                DateTime dtNgayBatDau = DateTime.Parse(dtNamHoc.Rows[0]["NgayBatDau"].ToString());
                // Lấy tất cả danh sách phòng máy
                DataTable dtTKB = data.Nuce_Eduweb.getTKB("PM");
                // Lay To Dang ki
                DataTable dtToDK = data.Nuce_Eduweb.getToDK("PM");
                // Lay tong so sinh vien dang ki
                DataTable dtTongSoSVDK = data.Nuce_Eduweb.getTongSoSVDK("PM");
                // Lay tat ca log de check
                DataTable dtLog = data.dnn_NuceQLPM_Log_Syn.getByType(Utils.synTypeLichHoc, 3);
                // Lay ta ca cac du lieu ve can bo
                DataTable dtCanBo = data.Nuce_Eduweb.getCanBo("PM");
                // Lay danh sach phòng máy
                DataTable dtPhongMay = data.dnn_NuceCommon_PhongHoc.get(-1);
                // Lay ca hoc
                DataTable dtCaHoc = data.dnn_NuceCommon_CaHoc.get(-1);
                // Duyệt qua tất cả các danh sách
                string MaDK;
                string MaCB;
                string HoVaTenCB;
                string Lop;
                string MonHoc;
                string Thu;
                int iThu;
                string TietBD;
                string SoTiet;
                string MaPH;
                int PhongHocID;
                int BuoiHoc;
                string TuanHoc;
                DateTime Ngay;
                int CahocID;
                int HocKyID;
                int NamHocID;
                int SoSinhVien;
                string TTThemCB;
                string MoTa;
                string GhiChu;
                string Key;
                DataRow drThongTinLop;
                string[] tuans;

                for (int i = 0; i < dtTKB.Rows.Count; i++)
                {
                    Key = dtTKB.Rows[i]["keytohop"].ToString();
                    if (!checkExists(dtLog, Key))
                    {
                        MaDK = dtTKB.Rows[i]["MaDK"].ToString();
                        MaCB = dtTKB.Rows[i]["MaCB"].ToString();
                        HoVaTenCB = getHoVaTenCB(dtCanBo, MaCB);
                        drThongTinLop = getThongTinLopHoc(dtToDK, MaDK);
                        if (drThongTinLop != null)
                        {
                            Lop = drThongTinLop["MaNh"].ToString();
                            MonHoc = drThongTinLop["TenMH"].ToString();
                        }
                        else
                        {
                            Lop = "";
                            MonHoc = "";
                        }
                        Thu = dtTKB.Rows[i]["Thu"].ToString();
                        iThu = int.Parse(Thu.Trim());
                        TietBD = dtTKB.Rows[i]["TietBD"].ToString();
                        SoTiet = dtTKB.Rows[i]["SoTiet"].ToString();
                        MaPH = dtTKB.Rows[i]["MaPH"].ToString();
                        PhongHocID = getPhong(dtPhongMay, MaPH);
                        BuoiHoc = int.Parse(dtTKB.Rows[i]["BuoiHoc"].ToString());
                        TuanHoc = dtTKB.Rows[i]["TuanHoc"].ToString();
                        #region Xu ly tuan hoc

                        #endregion
                        CahocID = getCaHoc(dtCaHoc, TietBD);
                        SoSinhVien = getSoSinhVien(dtTongSoSVDK, MaDK);
                        TTThemCB = "";
                        MoTa = "";
                        GhiChu = "---Dong bo " + MaDK + "-" + Lop + "-" + MonHoc + "-" + HoVaTenCB + "---";
                        // Insert vao bang dang ki va danh dau log
                        for (int j = 0; j < TuanHoc.Length; j++)
                        {
                            if (TuanHoc[j].ToString().Trim().Replace(" ", "") != "")
                            {
                                Ngay = dtNgayBatDau.AddDays(iThu - 2 + (j+19) * 7);
                                if(Ngay>DateTime.Now&&Ngay<DateTime.Now.AddDays(7))
                                {
                                    int test = 1;
                                }
                                // Cap nhat vao csdl
                                try
                                {
                                    data.dnn_NuceQLPM_LichPhongMay.Insert(MaDK, MaCB, HoVaTenCB, Lop, MonHoc, Thu, TietBD, SoTiet, MaPH, PhongHocID, BuoiHoc, TuanHoc,
                                        Ngay, CahocID, hockyid, namhocid, SoSinhVien, TTThemCB, MoTa, GhiChu, Utils.synTypeLichHoc, 1);
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                        }
                        // Danh dau da duyet
                        try
                        {
                            data.dnn_NuceQLPM_Log_Syn.Insert(Key, 3, Utils.synTypeLichHoc, "Dong bo thanh cong lich hoc");
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }

                // Dong bo lich thi
                CultureInfo provider = CultureInfo.InvariantCulture;
                provider = new CultureInfo("fr-FR");
                string dtformat = "dd/MM/yyyy";
                DataTable dtLichThi = data.Nuce_Eduweb.getLichThi("PM");
                DataTable dtLogThi = data.dnn_NuceQLPM_Log_Syn.getByType(Utils.synTypeLichThi, 3);
                for (int i = 0; i < dtLichThi.Rows.Count; i++)
                {
                    Key = dtLichThi.Rows[i]["KeyThi"].ToString();
                    if (!checkExists(dtLogThi, Key))
                    {
                        MaDK = dtLichThi.Rows[i]["KeyThi"].ToString().Replace(" ", "");
                        MaCB = "";
                        HoVaTenCB = "";

                        Lop = dtLichThi.Rows[i]["GhepThi"].ToString().Replace(" ", "");
                        MonHoc = getMonHoc(dtToDK, dtLichThi.Rows[i]["MaMH"].ToString().Replace(" ", ""));


                        Thu = "";
                        //iThu = int.Parse(Thu.Trim());
                        TietBD = dtLichThi.Rows[i]["TietBD"].ToString();
                        SoTiet = dtLichThi.Rows[i]["SoTiet"].ToString();
                        MaPH = dtLichThi.Rows[i]["MaPh"].ToString();
                        PhongHocID = getPhong(dtPhongMay, MaPH);
                        BuoiHoc = int.Parse(dtLichThi.Rows[i]["DotThi"].ToString());
                        TuanHoc = "";
                        #region Xu ly tuan hoc

                        #endregion
                        CahocID = getCaHoc(dtCaHoc, TietBD);
                        SoSinhVien = int.Parse(dtLichThi.Rows[i]["SoLuong"].ToString());
                        TTThemCB = "";
                        MoTa = "";
                        GhiChu = "---Dong bo lich thi" + MaDK + "-" + Lop + "-" + MonHoc + "-" + HoVaTenCB + "---";
                        // Insert vao bang dang ki va danh dau log
                       
                        Ngay = DateTime.ParseExact(dtLichThi.Rows[i]["NgayThi"].ToString(), dtformat, provider); 
                        // Cap nhat vao csdl
                        try
                        {
                            data.dnn_NuceQLPM_LichPhongMay.Insert(MaDK, MaCB, HoVaTenCB, Lop, MonHoc, Thu, TietBD, SoTiet, MaPH, PhongHocID, BuoiHoc, TuanHoc,
                                Ngay, CahocID, hockyid, namhocid, SoSinhVien, TTThemCB, MoTa, GhiChu, Utils.synTypeLichThi,1);
                        }
                        catch (Exception ex)
                        {

                        }

                        // Danh dau da duyet
                        try
                        {
                            data.dnn_NuceQLPM_Log_Syn.Insert(Key, 3,Utils.synTypeLichThi, "Dong bo thanh cong lich thi");
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }

                context.Response.Write("1");
                //http://localhost:8055/Handler/nuce.web.qlpm/DongBoDuLieu_EduWeb.ashx?hockyid=2&&namhocid=1

            }
            else
            {
                context.Response.Write("-1");
            }
            context.Response.Flush(); // Sends all currently buffered output to the client.
            context.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            context.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            context.Response.End();
        }
        private bool checkExists(DataTable dt, string key)
        {
            DataRow[] drs = dt.Select(string.Format("[KeyCheck]='{0}'", key));
            if (drs.Length > 0)
                return true;
            else
                return false;
        }
        private string getHoVaTenCB(DataTable dt, string MaCB)
        {
            DataRow[] drs = dt.Select(string.Format("[MaCB]='{0}'", MaCB));
            if (drs.Length > 0)
                return drs[0]["TenCB"].ToString() + "-" + drs[0]["MaBM"].ToString();
            else
                return "";
        }
        private int getPhong(DataTable dt, string MaPhong)
        {
            string MP = "";
            for (int i = 1; i < 6; i++)
            {
                if (MaPhong.Contains(i.ToString()))
                    MP = "P.MAY " + i.ToString();
            }

            DataRow[] drs = dt.Select(string.Format("[Ip2]='{0}'", MP));
            if (drs.Length > 0)
                return int.Parse(drs[0]["PhongHocID"].ToString());
            else
                return -1;
        }
        private int getCaHoc(DataTable dt, string TBD)
        {
            int TietBatDau = int.Parse(TBD);
            int iTietBatDau = 1;
            int iTietKetThuc = 3;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iTietBatDau = int.Parse(dt.Rows[i]["TietBatDau"].ToString());
                iTietKetThuc = int.Parse(dt.Rows[i]["TietKetThuc"].ToString());

                if (TietBatDau >= iTietBatDau && TietBatDau <= iTietKetThuc)
                {
                    return int.Parse(dt.Rows[i]["CaHocID"].ToString());
                }
            }
            return -1;
        }
        private int getSoSinhVien(DataTable dt, string MaDK)
        {
            DataRow[] drs = dt.Select(string.Format("[MaDK]='{0}'", MaDK));
            if (drs.Length > 0)
                return int.Parse(drs[0]["SoSV"].ToString());
            else
                return -1;
        }
        private DataRow getThongTinLopHoc(DataTable dt, string MaDK)
        {
            DataRow[] drs = dt.Select(string.Format("[MaDK]='{0}'", MaDK));
            if (drs.Length > 0)
                return drs[0];
            else
                return null;
        }
        private string getMonHoc(DataTable dt, string MaMH)
        {
            DataRow[] drs = dt.Select(string.Format("[MaNh]='{0}'", MaMH));
            if (drs.Length > 0)
                return drs[0]["TenMH"].ToString();
            else
                return "";
        }
    }
}
