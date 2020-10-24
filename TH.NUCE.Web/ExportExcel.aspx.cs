using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using nuce.web;

namespace TH.NUCE.Web
{
    public partial class ExportExcel : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            int iType = int.Parse(Request.QueryString["type"]);
            switch (iType)
            {
                case 1:
                    ProcessDanhSachKhachHangDangKiEmail();
                    break;
                case 2:
                    int ituanhientaiid = int.Parse(Request.QueryString["tuanhientaiid"]);
                    ProcessDanhSachDiemDanh(ituanhientaiid);
                    break;
                case 3:
                    int ikithilophocid = int.Parse(Request.QueryString["kithilophocid"]);
                    ProcessKetQuaKiThiLopHoc(ikithilophocid);
                    break;
                case 4:
                    int ikithilophocid1 = int.Parse(Request.QueryString["kithilophocid"]);
                    ProcessXuatDanhSachMatKhauSinhVienDangThi(ikithilophocid1);
                    break;
                case 5:
                    int iBoCauHoiID = int.Parse(Request.QueryString["bocauhoiid"]);
                    ProcessThongKeTraLoiVoiBoCauHoi(iBoCauHoiID);
                    break;
                case 6:
                    int ituan = int.Parse(Request.QueryString["tuan"]);
                    ProcessXuatRaFileLichPhongMay(ituan);
                    break;
            }
            base.OnInit(e);
        }
        #region  Xuat ra Excel Lich Phong may
        public void ProcessXuatRaFileLichPhongMay(int iTuan)
        {
            DataTable dtNamHoc = nuce.web.data.dnn_NuceCommon_NamHoc.getByStatus(1);
            DateTime dtNgayBatDau = DateTime.Parse(dtNamHoc.Rows[0]["NgayBatDau"].ToString());
            DateTime NgayBatDau = dtNgayBatDau.AddDays(7 * (iTuan - 1) - 1);
            DateTime NgayKetThuc = NgayBatDau.AddDays(7);
            DataTable dtContent = nuce.web.data.dnn_NuceQLPM_LichPhongMay.get(NgayBatDau, NgayKetThuc, -1, 3);

            string strTenFile = string.Format("Lich_phong_may_tuan_{0}_{1}", iTuan, DateTime.Now.ToFileTimeUtc());
            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("tuan_" + iTuan.ToString());
                #region column
                ws.Column(1).Width = 9;
                ws.Column(2).Width = 9;
                ws.Column(3).Width = 14;
                ws.Column(4).Width = 9;
                ws.Column(5).Width = 14;
                ws.Column(6).Width = 9;
                ws.Column(7).Width = 14;
                ws.Column(8).Width = 9;
                ws.Column(9).Width = 14;
                ws.Column(10).Width = 9;
                ws.Column(11).Width = 14;
                #endregion
                ws = ProcessXuatRaHeader(iTuan, NgayBatDau, NgayKetThuc, 0, ws);
                ws = ProcessXuatRa1Phong(1, dtContent, 3, ws);
                ws = ProcessXuatRaHeader(iTuan, NgayBatDau, NgayKetThuc, 16, ws);
                ws = ProcessXuatRa1Phong(2, dtContent, 19, ws);
                ws = ProcessXuatRaHeader(iTuan, NgayBatDau, NgayKetThuc, 32, ws);
                ws = ProcessXuatRa1Phong(3, dtContent, 35, ws);
                ws = ProcessXuatRaHeader(iTuan, NgayBatDau, NgayKetThuc, 48, ws);
                ws = ProcessXuatRa1Phong(4, dtContent, 51, ws);
                ws = ProcessXuatRaHeader(iTuan, NgayBatDau, NgayKetThuc, 64, ws);
                ws = ProcessXuatRa1Phong(5, dtContent, 67, ws);
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", strTenFile));
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        public IXLWorksheet ProcessXuatRaHeader(int iTuan,DateTime NgayBatDau, DateTime NgayKetThuc,int RowStart, IXLWorksheet ws)
        {
            #region row 1
            ws.Row(RowStart+1).Style.Font.FontName = "Times New Roman";
            ws.Row(RowStart+1).Style.Font.FontSize = 22;
            ws.Row(RowStart+1).Height = 30;
            ws.Row(RowStart+1).Style.Font.Bold = true;
            ws.Cell("A"+ (RowStart + 1).ToString()).Value = string.Format("LỊCH ĐĂNG KÍ PHÒNG MÁY");
            ws.Cell("A" + (RowStart + 1).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("A" + (RowStart + 1).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("A" + (RowStart + 1).ToString()+":K" + (RowStart + 1).ToString()).Row(1).Merge();
            #endregion
            #region row 2
            ws.Row(RowStart+2).Style.Font.FontName = "Times New Roman";
            ws.Row(RowStart+2).Style.Font.FontSize = 14;
            ws.Row(RowStart+2).Height = 18;
            ws.Row(RowStart+2).Style.Font.Italic = true;
            ws.Cell("A" + (RowStart + 2).ToString()).Value = string.Format("Tuần {0} {4}Ngày {1:dd/MM} đến {2:dd/MM} năm {3:yyyy}", iTuan, NgayBatDau.AddDays(1), NgayKetThuc.AddDays(-1), NgayBatDau, iTuan < 23 ? "" : "(tuần thứ " + (iTuan - 19).ToString() + " của kỳ 2) ");
            ws.Cell("A" + (RowStart + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("A" + (RowStart + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("A" + (RowStart + 2).ToString() + ":K" + (RowStart + 2).ToString()).Row(1).Merge();
            return ws;

            #endregion
        }
        public IXLWorksheet ProcessXuatRa1Phong(int PhongID, DataTable dt, int RowStart, IXLWorksheet ws)
        {
            ws.Row(RowStart).Style.Font.FontName = "Times New Roman";
            ws.Row(RowStart).Style.Font.FontSize = 16;
            ws.Row(RowStart).Height = 30;
            ws.Row(RowStart).Style.Font.Bold = true;
            ws.Cell("A" + RowStart.ToString()).Value = string.Format("PHÒNG MÁY SỐ {0}", PhongID);
            ws.Cell("A" + RowStart.ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("A" + RowStart.ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("A" + RowStart.ToString() + ":K" + RowStart.ToString()).Row(1).Merge();

            ws.Row(RowStart + 1).Height = 20;
            ws.Row(RowStart + 1).Style.Font.Bold = true;
            ws.Row(RowStart + 1).Style.Font.FontName = "Times New Roman";
            ws.Cell("A" + (RowStart + 1)).Style.Font.FontSize = 13;
            ws.Cell("A" + (RowStart + 1).ToString()).Value = string.Format("PHÒNG");
            ws.Cell("A" + (RowStart + 1).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("A" + (RowStart + 1).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A" + (RowStart + 1).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
            ws.Cell("A" + (RowStart + 1).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("A" + (RowStart + 1).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("A" + (RowStart + 1).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("A" + (RowStart + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Double;
            ws.Cell("A" + (RowStart + 1).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("A" + (RowStart + 1).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.None;
            //ws.Cell("A" + (RowStart + 1).ToString()).Style.Border.BottomBorderColor = XLColor.Black;

            ws.Range("B" + (RowStart + 1).ToString() + ":C" + (RowStart + 1).ToString()).Row(1).Merge();
            ws.Cell("B" + (RowStart + 1).ToString()).Style.Font.FontSize = 14;
            ws.Cell("B" + (RowStart + 1).ToString()).Value = string.Format("6h45 - 9h05");
            ws.Cell("B" + (RowStart + 1).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("B" + (RowStart + 1).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("B" + (RowStart + 1).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
            ws.Cell("B" + (RowStart + 1).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("B" + (RowStart + 1).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("B" + (RowStart + 1).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("B" + (RowStart + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Double;
            ws.Cell("B" + (RowStart + 1).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("B" + (RowStart + 1).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("B" + (RowStart + 1).ToString()).Style.Border.BottomBorderColor = XLColor.Black;

            ws.Cell("C" + (RowStart + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Double;
            ws.Cell("C" + (RowStart + 1).ToString()).Style.Border.TopBorderColor = XLColor.Black;


            ws.Cell("D" + (RowStart + 1).ToString()).Style.Font.FontSize = 14;
            ws.Cell("D" + (RowStart + 1).ToString()).Value = string.Format("9h25 - 11h50");
            ws.Cell("D" + (RowStart + 1).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("D" + (RowStart + 1).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("D" + (RowStart + 1).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
            ws.Cell("D" + (RowStart + 1).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("D" + (RowStart + 1).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("D" + (RowStart + 1).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("D" + (RowStart + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Double;
            ws.Cell("D" + (RowStart + 1).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("E" + (RowStart + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Double;
            ws.Cell("E" + (RowStart + 1).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("D" + (RowStart + 1).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("D" + (RowStart + 1).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            ws.Range("D" + (RowStart + 1).ToString() + ":E" + (RowStart + 1).ToString()).Row(1).Merge();

            ws.Cell("F" + (RowStart + 1).ToString()).Style.Font.FontSize = 14;
            ws.Cell("F" + (RowStart + 1).ToString()).Value = string.Format("12h15 - 14h35");
            ws.Cell("F" + (RowStart + 1).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("F" + (RowStart + 1).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("F" + (RowStart + 1).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
            ws.Cell("F" + (RowStart + 1).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("F" + (RowStart + 1).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("F" + (RowStart + 1).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("F" + (RowStart + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Double;
            ws.Cell("F" + (RowStart + 1).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("G" + (RowStart + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Double;
            ws.Cell("G" + (RowStart + 1).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("F" + (RowStart + 1).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("F" + (RowStart + 1).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            ws.Range("F" + (RowStart + 1).ToString() + ":G" + (RowStart + 1).ToString()).Row(1).Merge();


            ws.Cell("H" + (RowStart + 1).ToString()).Style.Font.FontSize = 14;
            ws.Cell("H" + (RowStart + 1).ToString()).Value = string.Format("14h55 - 17h20");
            ws.Cell("H" + (RowStart + 1).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("H" + (RowStart + 1).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("H" + (RowStart + 1).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
            ws.Cell("H" + (RowStart + 1).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("H" + (RowStart + 1).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("H" + (RowStart + 1).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("H" + (RowStart + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Double;
            ws.Cell("H" + (RowStart + 1).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("I" + (RowStart + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Double;
            ws.Cell("I" + (RowStart + 1).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("H" + (RowStart + 1).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("H" + (RowStart + 1).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            ws.Range("H" + (RowStart + 1).ToString() + ":I" + (RowStart + 1).ToString()).Row(1).Merge();

            ws.Cell("J" + (RowStart + 1).ToString()).Style.Font.FontSize = 14;
            ws.Cell("J" + (RowStart + 1).ToString()).Value = string.Format("18h00 - 20h30");
            ws.Cell("J" + (RowStart + 1).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("J" + (RowStart + 1).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("J" + (RowStart + 1).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
            ws.Cell("J" + (RowStart + 1).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("J" + (RowStart + 1).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("J" + (RowStart + 1).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("J" + (RowStart + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Double;
            ws.Cell("J" + (RowStart + 1).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("K" + (RowStart + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Double;
            ws.Cell("K" + (RowStart + 1).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("K" + (RowStart + 1).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("J" + (RowStart + 1).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("J" + (RowStart + 1).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            ws.Range("J" + (RowStart + 1).ToString() + ":K" + (RowStart + 1).ToString()).Row(1).Merge();

            //var rngJK = ws.Range("J" + (RowStart + 1).ToString() + ":K" + (RowStart + 3).ToString());
            //rngJK.Merge();
            //rngJK.Value = string.Format("GHI CHÚ");
            //rngJK.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            //rngJK.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //rngJK.Style.Border.LeftBorder = XLBorderStyleValues.Double;
            //rngJK.Style.Border.LeftBorderColor = XLColor.Black;
            //rngJK.Style.Border.RightBorder = XLBorderStyleValues.Double;
            //rngJK.Style.Border.RightBorderColor = XLColor.Black;
            //rngJK.Style.Border.TopBorder = XLBorderStyleValues.Double;
            //rngJK.Style.Border.TopBorderColor = XLColor.Black;
            //rngJK.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            //rngJK.Style.Border.BottomBorderColor = XLColor.Black;


            var rngA2 = ws.Range("A" + (RowStart + 2).ToString() + ":A" + (RowStart + 3).ToString());
            rngA2.Merge();

            rngA2.Style.Font.FontName = "Times New Roman";
            rngA2.Style.Font.FontSize = 18;
            rngA2.Style.Font.Bold = true;
            rngA2.Value = string.Format("{0}", PhongID);
            rngA2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            rngA2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngA2.Style.Border.LeftBorder = XLBorderStyleValues.Double;
            rngA2.Style.Border.LeftBorderColor = XLColor.Black;
            rngA2.Style.Border.RightBorder = XLBorderStyleValues.Double;
            rngA2.Style.Border.RightBorderColor = XLColor.Black;
            rngA2.Style.Border.TopBorder = XLBorderStyleValues.None;
            //ws.Cell("A" + (RowStart + 2).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            rngA2.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            rngA2.Style.Border.BottomBorderColor = XLColor.Black;

            var rngB2 = ws.Range("B" + (RowStart + 2).ToString() + ":B" + (RowStart + 3).ToString());
            rngB2.Merge();
            rngB2.Style.Font.FontName = "Times New Roman";
            rngB2.Style.Font.FontSize = 16;
            rngB2.Style.Font.Bold = true;
            rngB2.Value = string.Format("LỚP");
            rngB2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            rngB2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngB2.Style.Border.LeftBorder = XLBorderStyleValues.Double;
            rngB2.Style.Border.LeftBorderColor = XLColor.Black;
            rngB2.Style.Border.RightBorder = XLBorderStyleValues.Medium;
            rngB2.Style.Border.RightBorderColor = XLColor.Black;
            rngB2.Style.Border.TopBorder = XLBorderStyleValues.Thick;
            rngB2.Style.Border.TopBorderColor = XLColor.Black;
            rngB2.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            rngB2.Style.Border.BottomBorderColor = XLColor.Black;

            ws.Row(RowStart + 2).Style.Font.FontName = "Times New Roman";
            ws.Row(RowStart + 2).Height = 15;
            ws.Row(RowStart + 2).Style.Font.Bold = true;
            ws.Row(RowStart + 3).Style.Font.FontName = "Times New Roman";
            ws.Row(RowStart + 3).Height = 15;
            ws.Row(RowStart + 3).Style.Font.Bold = true;


            ws.Cell("C" + (RowStart + 2).ToString()).Style.Font.FontSize = 11;
            ws.Cell("C" + (RowStart + 2).ToString()).Value = string.Format("THẦY");
            ws.Cell("C" + (RowStart + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("C" + (RowStart + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("C" + (RowStart + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            ws.Cell("C" + (RowStart + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("C" + (RowStart + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("C" + (RowStart + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("C" + (RowStart + 2).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("C" + (RowStart + 2).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("C" + (RowStart + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.None;
            //ws.Cell("C" + (RowStart + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            //ws.Range("C" + (RowStart + 2).ToString() + ":C" + (RowStart + 3).ToString()).Row(1).Merge();

            ws.Cell("C" + (RowStart + 3).ToString()).Style.Font.FontSize = 11;
            ws.Cell("C" + (RowStart + 3).ToString()).Value = string.Format("HƯỚNG DẪN");
            ws.Cell("C" + (RowStart + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("C" + (RowStart + 3).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("C" + (RowStart + 3).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            ws.Cell("C" + (RowStart + 3).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("C" + (RowStart + 3).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("C" + (RowStart + 3).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("C" + (RowStart + 3).ToString()).Style.Border.TopBorder = XLBorderStyleValues.None;
            //ws.Cell("C" + (RowStart + 3).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("C" + (RowStart + 3).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("C" + (RowStart + 3).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            //ws.Range("C" + (RowStart + 2).ToString() + ":C" + (RowStart + 3).ToString()).Row(1).Merge();


            var rngD2 = ws.Range("D" + (RowStart + 2).ToString() + ":D" + (RowStart + 3).ToString());
            rngD2.Merge();
            rngD2.Style.Font.FontName = "Times New Roman";
            rngD2.Style.Font.FontSize = 16;
            rngD2.Style.Font.Bold = true;
            rngD2.Value = string.Format("LỚP");
            rngD2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            rngD2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngD2.Style.Border.LeftBorder = XLBorderStyleValues.Double;
            rngD2.Style.Border.LeftBorderColor = XLColor.Black;
            rngD2.Style.Border.RightBorder = XLBorderStyleValues.Medium;
            rngD2.Style.Border.RightBorderColor = XLColor.Black;
            rngD2.Style.Border.TopBorder = XLBorderStyleValues.Thick;
            rngD2.Style.Border.TopBorderColor = XLColor.Black;
            rngD2.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            rngD2.Style.Border.BottomBorderColor = XLColor.Black;

            ws.Cell("E" + (RowStart + 2).ToString()).Style.Font.FontSize = 11;
            ws.Cell("E" + (RowStart + 2).ToString()).Value = string.Format("THẦY");
            ws.Cell("E" + (RowStart + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("E" + (RowStart + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("E" + (RowStart + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            ws.Cell("E" + (RowStart + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("E" + (RowStart + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("E" + (RowStart + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("E" + (RowStart + 2).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("E" + (RowStart + 2).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("E" + (RowStart + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.None;
            //ws.Cell("E" + (RowStart + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            //ws.Range("E" + (RowStart + 2).ToString() + ":C" + (RowStart + 3).ToString()).Row(1).Merge();

            ws.Cell("E" + (RowStart + 3).ToString()).Style.Font.FontSize = 11;
            ws.Cell("E" + (RowStart + 3).ToString()).Value = string.Format("HƯỚNG DẪN");
            ws.Cell("E" + (RowStart + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("E" + (RowStart + 3).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("E" + (RowStart + 3).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            ws.Cell("E" + (RowStart + 3).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("E" + (RowStart + 3).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("E" + (RowStart + 3).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("E" + (RowStart + 3).ToString()).Style.Border.TopBorder = XLBorderStyleValues.None;
            //ws.Cell("E" + (RowStart + 3).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("E" + (RowStart + 3).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("E" + (RowStart + 3).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            //ws.Range("C" + (RowStart + 2).ToString() + ":C" + (RowStart + 3).ToString()).Row(1).Merge();



            var rngF2 = ws.Range("F" + (RowStart + 2).ToString() + ":F" + (RowStart + 3).ToString());
            rngF2.Merge();
            rngF2.Style.Font.FontName = "Times New Roman";
            rngF2.Style.Font.FontSize = 16;
            rngF2.Style.Font.Bold = true;
            rngF2.Value = string.Format("LỚP");
            rngF2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            rngF2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngF2.Style.Border.LeftBorder = XLBorderStyleValues.Double;
            rngF2.Style.Border.LeftBorderColor = XLColor.Black;
            rngF2.Style.Border.RightBorder = XLBorderStyleValues.Medium;
            rngF2.Style.Border.RightBorderColor = XLColor.Black;
            rngF2.Style.Border.TopBorder = XLBorderStyleValues.Thick;
            rngF2.Style.Border.TopBorderColor = XLColor.Black;
            rngF2.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            rngF2.Style.Border.BottomBorderColor = XLColor.Black;

            ws.Cell("G" + (RowStart + 2).ToString()).Style.Font.FontSize = 11;
            ws.Cell("G" + (RowStart + 2).ToString()).Value = string.Format("THẦY");
            ws.Cell("G" + (RowStart + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("G" + (RowStart + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("G" + (RowStart + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            ws.Cell("G" + (RowStart + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("G" + (RowStart + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("G" + (RowStart + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("G" + (RowStart + 2).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("G" + (RowStart + 2).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("G" + (RowStart + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.None;
            //ws.Cell("G" + (RowStart + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            //ws.Range("G" + (RowStart + 2).ToString() + ":C" + (RowStart + 3).ToString()).Row(1).Merge();

            ws.Cell("G" + (RowStart + 3).ToString()).Style.Font.FontSize = 11;
            ws.Cell("G" + (RowStart + 3).ToString()).Value = string.Format("HƯỚNG DẪN");
            ws.Cell("G" + (RowStart + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("G" + (RowStart + 3).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("G" + (RowStart + 3).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            ws.Cell("G" + (RowStart + 3).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("G" + (RowStart + 3).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("G" + (RowStart + 3).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("G" + (RowStart + 3).ToString()).Style.Border.TopBorder = XLBorderStyleValues.None;
            //ws.Cell("G" + (RowStart + 3).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("G" + (RowStart + 3).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("G" + (RowStart + 3).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            //ws.Range("C" + (RowStart + 2).ToString() + ":C" + (RowStart + 3).ToString()).Row(1).Merge();


            var rngH2 = ws.Range("H" + (RowStart + 2).ToString() + ":H" + (RowStart + 3).ToString());
            rngH2.Merge();
            rngH2.Style.Font.FontName = "Times New Roman";
            rngH2.Style.Font.FontSize = 16;
            rngH2.Style.Font.Bold = true;
            rngH2.Value = string.Format("LỚP");
            rngH2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            rngH2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngH2.Style.Border.LeftBorder = XLBorderStyleValues.Double;
            rngH2.Style.Border.LeftBorderColor = XLColor.Black;
            rngH2.Style.Border.RightBorder = XLBorderStyleValues.Medium;
            rngH2.Style.Border.RightBorderColor = XLColor.Black;
            rngH2.Style.Border.TopBorder = XLBorderStyleValues.Thick;
            rngH2.Style.Border.TopBorderColor = XLColor.Black;
            rngH2.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            rngH2.Style.Border.BottomBorderColor = XLColor.Black;

            ws.Cell("I" + (RowStart + 2).ToString()).Style.Font.FontSize = 11;
            ws.Cell("I" + (RowStart + 2).ToString()).Value = string.Format("THẦY");
            ws.Cell("I" + (RowStart + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("I" + (RowStart + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("I" + (RowStart + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            ws.Cell("I" + (RowStart + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("I" + (RowStart + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("I" + (RowStart + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("I" + (RowStart + 2).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("I" + (RowStart + 2).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("I" + (RowStart + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.None;
            //ws.Cell("I" + (RowStart + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            //ws.Range("I" + (RowStart + 2).ToString() + ":C" + (RowStart + 3).ToString()).Row(1).Merge();

            ws.Cell("I" + (RowStart + 3).ToString()).Style.Font.FontSize = 11;
            ws.Cell("I" + (RowStart + 3).ToString()).Value = string.Format("HƯỚNG DẪN");
            ws.Cell("I" + (RowStart + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("I" + (RowStart + 3).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("I" + (RowStart + 3).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            ws.Cell("I" + (RowStart + 3).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("I" + (RowStart + 3).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("I" + (RowStart + 3).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("I" + (RowStart + 3).ToString()).Style.Border.TopBorder = XLBorderStyleValues.None;
            //ws.Cell("I" + (RowStart + 3).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("I" + (RowStart + 3).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("I" + (RowStart + 3).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            //ws.Range("C" + (RowStart + 2).ToString() + ":C" + (RowStart + 3).ToString()).Row(1).Merge();



            var rngJ2 = ws.Range("J" + (RowStart + 2).ToString() + ":J" + (RowStart + 3).ToString());
            rngJ2.Merge();
            rngJ2.Style.Font.FontName = "Times New Roman";
            rngJ2.Style.Font.FontSize = 16;
            rngJ2.Style.Font.Bold = true;
            rngJ2.Value = string.Format("LỚP");
            rngJ2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            rngJ2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngJ2.Style.Border.LeftBorder = XLBorderStyleValues.Double;
            rngJ2.Style.Border.LeftBorderColor = XLColor.Black;
            rngJ2.Style.Border.RightBorder = XLBorderStyleValues.Medium;
            rngJ2.Style.Border.RightBorderColor = XLColor.Black;
            rngJ2.Style.Border.TopBorder = XLBorderStyleValues.Thick;
            rngJ2.Style.Border.TopBorderColor = XLColor.Black;
            rngJ2.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            rngJ2.Style.Border.BottomBorderColor = XLColor.Black;

            ws.Cell("K" + (RowStart + 2).ToString()).Style.Font.FontSize = 11;
            ws.Cell("K" + (RowStart + 2).ToString()).Value = string.Format("THẦY");
            ws.Cell("K" + (RowStart + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("K" + (RowStart + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("K" + (RowStart + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            ws.Cell("K" + (RowStart + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("K" + (RowStart + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("K" + (RowStart + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("K" + (RowStart + 2).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("K" + (RowStart + 2).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("K" + (RowStart + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.None;
            //ws.Cell("I" + (RowStart + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            //ws.Range("I" + (RowStart + 2).ToString() + ":C" + (RowStart + 3).ToString()).Row(1).Merge();

            ws.Cell("K" + (RowStart + 3).ToString()).Style.Font.FontSize = 11;
            ws.Cell("K" + (RowStart + 3).ToString()).Value = string.Format("HƯỚNG DẪN");
            ws.Cell("K" + (RowStart + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("K" + (RowStart + 3).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("K" + (RowStart + 3).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            ws.Cell("K" + (RowStart + 3).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell("K" + (RowStart + 3).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
            ws.Cell("K" + (RowStart + 3).ToString()).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell("K" + (RowStart + 3).ToString()).Style.Border.TopBorder = XLBorderStyleValues.None;
            //ws.Cell("I" + (RowStart + 3).ToString()).Style.Border.TopBorderColor = XLColor.Black;
            ws.Cell("K" + (RowStart + 3).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("K" + (RowStart + 3).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
            //ws.Range("C" + (RowStart + 2).ToString() + ":C" + (RowStart + 3).ToString()).Row(1).Merge();

            DataRow[] drs;
            for (int i = 2; i < 8; i++)
            {
                ws.Row(RowStart + i + 2).Style.Font.FontName = "Times New Roman";
                ws.Row(RowStart + i + 2).Height = 55;

                ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Font.FontSize = 14;
                ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Font.Italic = true;
                ws.Cell("A" + (RowStart + i + 2).ToString()).Value = string.Format("Thứ {0}", i);
                ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
                ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
                ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
                if (i.Equals(7))
                {
                    ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                else
                {
                    ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                    ws.Cell("A" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }

                ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Font.FontSize = 12;
                //ws.Cell("B" + (RowStart + i + 2).ToString()).Value = string.Format("Thứ {0}", i);
                ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Alignment.WrapText = true;
                ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
                ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
                if (i.Equals(7))
                {
                    ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                else
                {
                    ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                    ws.Cell("B" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Font.FontSize = 10;
                //ws.Cell("C" + (RowStart + i + 2).ToString()).Value = string.Format("Thứ {0}", i);
                ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Alignment.WrapText = true;
                ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
                ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
                if (i.Equals(7))
                {
                    ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                else
                {
                    ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                    ws.Cell("C" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }

                ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Font.FontSize = 12;
                //ws.Cell("D" + (RowStart + i + 2).ToString()).Value = string.Format("Thứ {0}", i);
                ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Alignment.WrapText = true;
                ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
                ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
                if (i.Equals(7))
                {
                    ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                else
                {
                    ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                    ws.Cell("D" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Font.FontSize = 10;
                //ws.Cell("E" + (RowStart + i + 2).ToString()).Value = string.Format("Thứ {0}", i);
                ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Alignment.WrapText = true;
                ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
                ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
                if (i.Equals(7))
                {
                    ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                else
                {
                    ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                    ws.Cell("E" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }

                ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Font.FontSize = 12;
                //ws.Cell("F" + (RowStart + i + 2).ToString()).Value = string.Format("Thứ {0}", i);
                ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Alignment.WrapText = true;
                ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
                ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
                if (i.Equals(7))
                {
                    ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                else
                {
                    ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                    ws.Cell("F" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Font.FontSize = 10;
                //ws.Cell("G" + (RowStart + i + 2).ToString()).Value = string.Format("Thứ {0}", i);
                ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Alignment.WrapText = true;
                ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
                ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
                if (i.Equals(7))
                {
                    ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                else
                {
                    ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                    ws.Cell("G" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }

                ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Font.FontSize = 12;
                //ws.Cell("H" + (RowStart + i + 2).ToString()).Value = string.Format("Thứ {0}", i);
                ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Alignment.WrapText = true;
                ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
                ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
                if (i.Equals(7))
                {
                    ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                else
                {
                    ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                    ws.Cell("H" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Font.FontSize = 10;
                //ws.Cell("I" + (RowStart + i + 2).ToString()).Value = string.Format("Thứ {0}", i);
                ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Alignment.WrapText = true;
                ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
                ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
                if (i.Equals(7))
                {
                    ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                else
                {
                    ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                    ws.Cell("I" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }

                ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Font.FontSize = 12;
                //ws.Cell("H" + (RowStart + i + 2).ToString()).Value = string.Format("Thứ {0}", i);
                ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Alignment.WrapText = true;
                ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Double;
                ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
                if (i.Equals(7))
                {
                    ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                else
                {
                    ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                    ws.Cell("J" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Font.FontSize = 10;
                //ws.Cell("I" + (RowStart + i + 2).ToString()).Value = string.Format("Thứ {0}", i);
                ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Alignment.WrapText = true;
                ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Double;
                ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Border.RightBorderColor = XLColor.Black;
                if (i.Equals(7))
                {
                    ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }
                else
                {
                    ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                    ws.Cell("K" + (RowStart + i + 2).ToString()).Style.Border.BottomBorderColor = XLColor.Black;
                }


                //var rngJkRepeat = ws.Range("J" + (RowStart + i + 2).ToString() + ":K" + (RowStart + i + 2).ToString());
                //rngJkRepeat.Merge();
                //rngJkRepeat.Style.Border.LeftBorder = XLBorderStyleValues.Double;
                //rngJkRepeat.Style.Border.LeftBorderColor = XLColor.Black;
                //rngJkRepeat.Style.Border.RightBorder = XLBorderStyleValues.Double;
                //rngJkRepeat.Style.Border.RightBorderColor = XLColor.Black;
                //if (i.Equals(7))
                //{
                //    rngJkRepeat.Style.Border.BottomBorder = XLBorderStyleValues.Double;
                //    rngJkRepeat.Style.Border.BottomBorderColor = XLColor.Black;
                //} 
                //else
                //{
                //    rngJkRepeat.Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
                //    rngJkRepeat.Style.Border.BottomBorderColor = XLColor.Black;
                //}


                // ca 1
                drs = dt.Select(string.Format("PhongHocID={0} and Thu='{1}' and CaHocID={2}", PhongID, i, 1));
                // 
                if (drs.Length > 0)
                {
                    ws.Cell("B" + (RowStart + i + 2).ToString()).Value = string.Format("{0}", drs[0]["Lop"].ToString());
                    ws.Cell("C" + (RowStart + i + 2).ToString()).Value = string.Format("{0}\n({1})", drs[0]["HoVaTenCB"].ToString(), drs[0]["MonHoc"].ToString());
                    //ws.Cell("C" + (RowStart + i + 2).ToString()).Value = string.Format("{0}\n({1})", "Hoang Nam Thang", drs[0]["MonHoc"].ToString());
                }

                // ca 2
                drs = dt.Select(string.Format("PhongHocID={0} and Thu='{1}' and CaHocID={2}", PhongID, i, 2));
                // 
                if (drs.Length > 0)
                {
                    ws.Cell("D" + (RowStart + i + 2).ToString()).Value = string.Format("{0}", drs[0]["Lop"].ToString());
                    ws.Cell("E" + (RowStart + i + 2).ToString()).Value = string.Format("{0}\n({1})", drs[0]["HoVaTenCB"].ToString(), drs[0]["MonHoc"].ToString());
                    //ws.Cell("C" + (RowStart + i + 2).ToString()).Value = string.Format("{0}\n({1})", "Hoang Nam Thang", drs[0]["MonHoc"].ToString());
                }
                // ca 3
                drs = dt.Select(string.Format("PhongHocID={0} and Thu='{1}' and CaHocID={2}", PhongID, i, 3));
                // 
                if (drs.Length > 0)
                {
                    ws.Cell("F" + (RowStart + i + 2).ToString()).Value = string.Format("{0}", drs[0]["Lop"].ToString());
                    ws.Cell("G" + (RowStart + i + 2).ToString()).Value = string.Format("{0}\n({1})", drs[0]["HoVaTenCB"].ToString(), drs[0]["MonHoc"].ToString());
                    //ws.Cell("C" + (RowStart + i + 2).ToString()).Value = string.Format("{0}\n({1})", "Hoang Nam Thang", drs[0]["MonHoc"].ToString());
                }
                // ca 4
                drs = dt.Select(string.Format("PhongHocID={0} and Thu='{1}' and CaHocID={2}", PhongID, i, 4));
                // 
                if (drs.Length > 0)
                {
                    ws.Cell("H" + (RowStart + i + 2).ToString()).Value = string.Format("{0}", drs[0]["Lop"].ToString());
                    ws.Cell("I" + (RowStart + i + 2).ToString()).Value = string.Format("{0}\n({1})", drs[0]["HoVaTenCB"].ToString(), drs[0]["MonHoc"].ToString());
                    //ws.Cell("C" + (RowStart + i + 2).ToString()).Value = string.Format("{0}\n({1})", "Hoang Nam Thang", drs[0]["MonHoc"].ToString());
                }
                // ca 5
                drs = dt.Select(string.Format("PhongHocID={0} and Thu='{1}' and CaHocID={2}", PhongID, i, 5));
                // 
                if (drs.Length > 0)
                {
                    ws.Cell("J" + (RowStart + i + 2).ToString()).Value = string.Format("{0}", drs[0]["Lop"].ToString());
                    ws.Cell("K" + (RowStart + i + 2).ToString()).Value = string.Format("{0}\n({1})", drs[0]["HoVaTenCB"].ToString(), drs[0]["MonHoc"].ToString());
                    //ws.Cell("C" + (RowStart + i + 2).ToString()).Value = string.Format("{0}\n({1})", "Hoang Nam Thang", drs[0]["MonHoc"].ToString());
                }
            }

            return ws;
        }
        #endregion
        public void ProcessThongKeTraLoiVoiBoCauHoi(int BoCauHoiID)
        {
            Utils.thongKe();
            DataTable dtContent = nuce.web.data.dnn_NuceThi_CauHoi_ThongKe.getByBoCauHoi(BoCauHoiID);

            string strTenFile = string.Format("export_{0}_{1}", BoCauHoiID, DateTime.Now.ToFileTimeUtc());
            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Ket_qua_Thong_ke");
                #region column
                ws.Column(1).Width = 5;
                ws.Column(2).Width = 60;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                #endregion
                #region row 1
                ws.Row(1).Style.Font.FontName = "Tahoma";
                ws.Row(1).Style.Font.FontSize = 10;
                ws.Row(1).Height = 30;
                ws.Row(1).Style.Font.Bold = true;

                ws.Cell("A1").Value = string.Format("{0}", "STT");
                ws.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A1").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("A1").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("A1").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("A1").Style.Border.RightBorderColor = XLColor.Black;
                ws.Cell("A1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("A1").Style.Border.TopBorderColor = XLColor.Black;
                ws.Cell("A1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell("A1").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Cell("B1").Value = string.Format("{0}", "Câu hỏi");
                ws.Cell("B1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("B1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("B1").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("B1").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("B1").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("B1").Style.Border.RightBorderColor = XLColor.Black;
                ws.Cell("B1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("B1").Style.Border.TopBorderColor = XLColor.Black;
                ws.Cell("B1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell("B1").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Cell("C1").Value = string.Format("{0}", "Số người tham gia");
                ws.Cell("C1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("C1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("C1").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("C1").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("C1").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("C1").Style.Border.RightBorderColor = XLColor.Black;
                ws.Cell("C1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("C1").Style.Border.TopBorderColor = XLColor.Black;
                ws.Cell("C1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell("C1").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Cell("D1").Value = string.Format("{0}", "Số người trả lời đúng");
                ws.Cell("D1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("D1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("D1").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("D1").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("D1").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("D1").Style.Border.RightBorderColor = XLColor.Black;
                ws.Cell("D1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("D1").Style.Border.TopBorderColor = XLColor.Black;
                ws.Cell("D1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell("D1").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Cell("E1").Value = string.Format("{0}", "Số người trả lời sai");
                ws.Cell("E1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("E1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("E1").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("E1").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("E1").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("E1").Style.Border.RightBorderColor = XLColor.Black;
                ws.Cell("E1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("E1").Style.Border.TopBorderColor = XLColor.Black;
                ws.Cell("E1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell("E1").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Cell("F1").Value = string.Format("{0}", "Độ khó");
                ws.Cell("F1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("F1").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("F1").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("F1").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("F1").Style.Border.RightBorderColor = XLColor.Black;
                ws.Cell("F1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("F1").Style.Border.TopBorderColor = XLColor.Black;
                ws.Cell("F1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell("F1").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Cell("G1").Value = string.Format("{0}", "Độ phân biệt");
                ws.Cell("G1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("G1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("G1").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("G1").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("G1").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("G1").Style.Border.RightBorderColor = XLColor.Black;
                ws.Cell("G1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("G1").Style.Border.TopBorderColor = XLColor.Black;
                ws.Cell("G1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell("G1").Style.Border.BottomBorderColor = XLColor.Black;

                #endregion

                int iRowCount = dtContent.Rows.Count;

                string strContent = "";
                int iTongSoTraLoiDung = 0;
                int iTongSoTraLoiSai = 0;
                int iTongSoTraLoi = 0;
                float fDoKho = 0;
                int iDoPhanBiet = 0;
                for (int i = 0; i < iRowCount; i++)
                {
                    strContent = dtContent.Rows[i]["Content"].ToString();
                    iTongSoTraLoiDung = int.Parse(dtContent.Rows[i]["TongSoTraLoiDung"].ToString());
                    iTongSoTraLoiSai = int.Parse(dtContent.Rows[i]["TongSoTraLoiSai"].ToString());
                    iTongSoTraLoi = iTongSoTraLoiDung + iTongSoTraLoiSai;
                    fDoKho = (float)iTongSoTraLoiDung / iTongSoTraLoi;
                    iDoPhanBiet = iTongSoTraLoiDung - iTongSoTraLoiSai;
                    #region Row element
                    ws.Row(i + 10).Style.Font.FontName = "Tahoma";
                    ws.Row(i + 10).Style.Font.FontSize = 10;
                    ws.Row(i + 10).Height = 19;

                    ws.Cell(string.Format("A{0}", (i + 2))).Value = string.Format("{0}", (i + 1));
                    ws.Cell(string.Format("A{0}", (i + 2))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(string.Format("A{0}", (i + 2))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(string.Format("A{0}", (i + 2))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("A{0}", (i + 2))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Cell(string.Format("A{0}", (i + 2))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("A{0}", (i + 2))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Cell(string.Format("A{0}", (i + 2))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("A{0}", (i + 2))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Cell(string.Format("A{0}", (i + 2))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("A{0}", (i + 2))).Style.Border.BottomBorderColor = XLColor.Black;


                    ws.Cell(string.Format("B{0}", (i + 2))).Value = strContent;
                    ws.Cell(string.Format("B{0}", (i + 2))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(string.Format("B{0}", (i + 2))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Cell(string.Format("B{0}", (i + 2))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("B{0}", (i + 2))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Cell(string.Format("B{0}", (i + 2))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("B{0}", (i + 2))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Cell(string.Format("B{0}", (i + 2))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("B{0}", (i + 2))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Cell(string.Format("B{0}", (i + 2))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("B{0}", (i + 2))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Cell(string.Format("C{0}", (i + 2))).Value = string.Format("{0}", iTongSoTraLoi);
                    ws.Cell(string.Format("C{0}", (i + 2))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(string.Format("C{0}", (i + 2))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(string.Format("C{0}", (i + 2))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 2))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Cell(string.Format("C{0}", (i + 2))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 2))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Cell(string.Format("C{0}", (i + 2))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 2))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Cell(string.Format("C{0}", (i + 2))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 2))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Cell(string.Format("D{0}", (i + 2))).Value = string.Format("{0}", iTongSoTraLoiDung);
                    ws.Cell(string.Format("D{0}", (i + 2))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(string.Format("D{0}", (i + 2))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(string.Format("D{0}", (i + 2))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("D{0}", (i + 2))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Cell(string.Format("D{0}", (i + 2))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("D{0}", (i + 2))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Cell(string.Format("D{0}", (i + 2))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("D{0}", (i + 2))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Cell(string.Format("D{0}", (i + 2))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("D{0}", (i + 2))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Cell(string.Format("E{0}", (i + 2))).Value = string.Format("{0}", iTongSoTraLoiSai);
                    ws.Cell(string.Format("E{0}", (i + 2))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(string.Format("E{0}", (i + 2))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(string.Format("E{0}", (i + 2))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("E{0}", (i + 2))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Cell(string.Format("E{0}", (i + 2))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("E{0}", (i + 2))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Cell(string.Format("E{0}", (i + 2))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("E{0}", (i + 2))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Cell(string.Format("E{0}", (i + 2))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("E{0}", (i + 2))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Cell(string.Format("F{0}", (i + 2))).Value = fDoKho.ToString("N2");
                    ws.Cell(string.Format("F{0}", (i + 2))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(string.Format("F{0}", (i + 2))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(string.Format("F{0}", (i + 2))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("F{0}", (i + 2))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Cell(string.Format("F{0}", (i + 2))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("F{0}", (i + 2))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Cell(string.Format("F{0}", (i + 2))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("F{0}", (i + 2))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Cell(string.Format("F{0}", (i + 2))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("F{0}", (i + 2))).Style.Border.BottomBorderColor = XLColor.Black;


                    ws.Cell(string.Format("G{0}", (i + 2))).Value = string.Format("{0}", iDoPhanBiet);
                    ws.Cell(string.Format("G{0}", (i + 2))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(string.Format("G{0}", (i + 2))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(string.Format("G{0}", (i + 2))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("G{0}", (i + 2))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Cell(string.Format("G{0}", (i + 2))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("G{0}", (i + 2))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Cell(string.Format("G{0}", (i + 2))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("G{0}", (i + 2))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Cell(string.Format("G{0}", (i + 2))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("G{0}", (i + 2))).Style.Border.BottomBorderColor = XLColor.Black;

                    #endregion

                }
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", strTenFile));
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        public void ProcessKetQuaKiThiLopHoc(int KiThiLopHocID)
        {
            DataTable dtContent = nuce.web.data.dnn_NuceThi_KiThi_LopHoc_SinhVien.getByKiThi_LopHoc(KiThiLopHocID);
            DataTable dtKiThiLopHoc = nuce.web.data.dnn_NuceThi_KiThi_LopHoc.get(KiThiLopHocID);
            string strTenFile = string.Format("export_{0}", DateTime.Now.ToFileTimeUtc());
            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Ket_qua");

                #region column
                ws.Column(1).Width = 2;
                ws.Column(2).Width = 1;
                ws.Column(3).Width = 10;
                ws.Column(4).Width = 1;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 1;
                ws.Column(7).Width = 1;
                ws.Column(8).Width = 8;
                ws.Column(9).Width = 1;
                ws.Column(10).Width = 1;
                ws.Column(11).Width = 6;
                ws.Column(12).Width = 3;
                ws.Column(13).Width = 2;
                ws.Column(14).Width = 1;
                ws.Column(15).Width = 5;
                ws.Column(16).Width = 1;
                ws.Column(17).Width = 15;
                ws.Column(18).Width = 1;
                #endregion
                #region Row1
                ws.Row(1).Height = 3;
                ws.Row(1).Style.Font.FontName = "Tahoma";
                ws.Range("A1:R1").Row(1).Merge();
                #endregion
                #region Row2
                ws.Row(2).Style.Font.FontName = "Tahoma";
                ws.Row(2).Style.Font.FontSize = 10;
                ws.Row(2).Height = 15;
                ws.Cell("A2").Value = string.Format("Trường Đại học Xây dựng");
                ws.Cell("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A2").Style.Font.Bold = true;
                ws.Range("A2:E2").Row(1).Merge();
                #endregion
                #region Row3
                ws.Row(3).Style.Font.FontName = "Tahoma";
                ws.Row(3).Style.Font.FontSize = 10;
                ws.Cell("A3").Value = string.Format("Phòng Đào tạo");
                ws.Cell("A3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A3").Style.Font.Bold = true;
                ws.Range("A3:E3").Row(1).Merge();
                #endregion
                #region Row4
                ws.Row(4).Style.Font.FontName = "Tahoma";
                ws.Row(4).Style.Font.FontSize = 12;
                ws.Row(4).Height = 25;
                ws.Cell("A4").Value = string.Format("BẢNG ĐIỂM");
                ws.Cell("A4").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A4").Style.Font.Bold = true;
                ws.Range("A4:R4").Row(1).Merge();
                #endregion
                #region Row5
                ws.Row(5).Style.Font.FontName = "Tahoma";
                ws.Row(5).Style.Font.FontSize = 10;
                ws.Row(5).Height = 15;
                ws.Cell("A5").Value = string.Format("{0}", dtKiThiLopHoc.Rows[0]["TenKiThi"].ToString());
                ws.Cell("A5").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("A5:R5").Row(1).Merge();
                #endregion
                #region Row6
                ws.Row(6).Style.Font.FontName = "Tahoma";
                ws.Row(6).Style.Font.FontSize = 10;
                ws.Row(6).Height = 15;

                ws.Cell("B6").Value = string.Format("{0}", "Môn học/Nhóm:");
                ws.Cell("B6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("B6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range("B6:D6").Row(1).Merge();

                ws.Cell("E6").Value = string.Format("{0}", dtKiThiLopHoc.Rows[0]["TenLopHoc"].ToString());
                ws.Cell("E6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("E6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("E6").Style.Font.Bold = true;
                ws.Range("E6:P6").Row(1).Merge();

                ws.Cell("Q6").Value = string.Format("Số tín chỉ: {0}", dtKiThiLopHoc.Rows[0]["SoChi"].ToString());
                ws.Cell("Q6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("Q6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range("Q6:R6").Row(1).Merge();
                #endregion
                #region Row7
                ws.Row(7).Style.Font.FontName = "Tahoma";
                ws.Row(7).Style.Font.FontSize = 10;
                ws.Row(7).Height = 18;

                ws.Cell("B7").Value = string.Format("{0}", "Ngày thi:");
                ws.Cell("B7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("B7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range("B7:D7").Row(1).Merge();

                DateTime dtNgayKetThuc = DateTime.Parse(dtKiThiLopHoc.Rows[0]["UpdatedDate"].ToString());
                ws.Cell("E7").Value = string.Format(" {0} / {1} / {2} ", dtNgayKetThuc.Day, dtNgayKetThuc.Month, dtNgayKetThuc.Year);
                ws.Cell("E7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("E7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("E7").Style.Font.Bold = true;
                ws.Range("E7:G7").Row(1).Merge();

                ws.Cell("H7").Value = string.Format("Phòng thi: ");
                ws.Cell("H7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("H7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("E7").Style.Font.Bold = true;
                ws.Range("H7:J7").Row(1).Merge();

                ws.Cell("K7").Value = string.Format("{0}", dtKiThiLopHoc.Rows[0]["PhongThi"].ToString());
                ws.Cell("K7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("K7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range("K7:R7").Row(1).Merge();
                #endregion
                #region Row8
                ws.Row(8).Height = 25;
                ws.Row(8).Style.Font.FontName = "Tahoma";
                #endregion
                #region Row9
                ws.Row(9).Style.Font.FontName = "Tahoma";
                ws.Row(9).Style.Font.FontSize = 10;
                ws.Row(9).Height = 30;
                ws.Row(9).Style.Font.Bold = true;

                ws.Range("A9:B9").Row(1).Merge();
                ws.Range("A9:B9").Value = string.Format("{0}", "STT");
                ws.Range("A9:B9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("A9:B9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("A9:B9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("A9:B9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Range("A9:B9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("A9:B9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Range("A9:B9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("A9:B9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Range("A9:B9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("A9:B9").Style.Border.BottomBorderColor = XLColor.Black;


                ws.Cell("C9").Value = string.Format("{0}", "Mã SV");
                ws.Cell("C9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("C9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("C9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("C9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("C9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("C9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Cell("C9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("C9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Cell("C9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell("C9").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Range("D9:H9").Row(1).Merge();
                ws.Range("D9:H9").Value = string.Format("{0}", "Họ và Tên");
                ws.Range("D9:H9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("D9:H9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("D9:H9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("D9:H9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Range("D9:H9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("D9:H9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Range("D9:H9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("D9:H9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Range("D9:H9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("D9:H9").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Range("I9:K9").Row(1).Merge();
                ws.Range("I9:K9").Value = string.Format("{0}", "Lớp Q.Lý");
                ws.Range("I9:K9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("I9:K9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("I9:K9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("I9:K9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Range("I9:K9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("I9:K9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Range("I9:K9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("I9:K9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Range("I9:K9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("I9:K9").Style.Border.BottomBorderColor = XLColor.Black;


                ws.Range("L9:N9").Row(1).Merge();
                ws.Range("L9:N9").Value = string.Format("{0}", "Điểm");
                ws.Range("L9:N9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("L9:N9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("L9:N9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("L9:N9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Range("L9:N9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("L9:N9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Range("L9:N9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("L9:N9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Range("L9:N9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("L9:N9").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Range("O9:R9").Row(1).Merge();
                ws.Range("O9:R9").Value = string.Format("{0}", "Ghi chú");
                ws.Range("O9:R9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("O9:R9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("O9:R9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("O9:R9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Range("O9:R9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("O9:R9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Range("O9:R9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("O9:R9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Range("O9:R9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("O9:R9").Style.Border.BottomBorderColor = XLColor.Black;
                #endregion

                float fDiem = 0;
                string strDiem = "";
                int iStatus = -1;
                int iRowCount = dtContent.Rows.Count;
                for (int i = 0; i < iRowCount; i++)
                {
                    #region Row element
                    ws.Row(i + 10).Style.Font.FontName = "Tahoma";
                    ws.Row(i + 10).Style.Font.FontSize = 10;
                    ws.Row(i + 10).Height = 19;

                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Value = string.Format("{0}", (i + 1));
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;


                    ws.Cell(string.Format("C{0}", (i + 10))).Value = string.Format("{0}", dtContent.Rows[i]["MaSV"].ToString());
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Value = string.Format("{0}", dtContent.Rows[i]["Ho"].ToString());
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Value = string.Format("{0}", dtContent.Rows[i]["TenSinhVien"].ToString());
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Value = string.Format("{0}", dtContent.Rows[i]["TenLop"].ToString());
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;

                    fDiem = float.Parse(dtContent.Rows[i]["Diem"].ToString());
                    iStatus = int.Parse(dtContent.Rows[i]["Status"].ToString());
                    switch (iStatus)
                    {
                        case 4:
                            strDiem = fDiem.ToString("N1");
                            break;
                        case 8:
                        case 9:
                            fDiem = 0;
                            strDiem = fDiem.ToString("N1");
                            break;
                        default: break;

                    }
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Value = string.Format("{0}", strDiem);
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Value = string.Format("{0}", dtContent.Rows[i]["MoTa"].ToString());
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;
                    #endregion
                }

                #region Footer 1
                ws.Row(iRowCount + 10).Height = 3;
                ws.Row(iRowCount + 10).Style.Font.FontName = "Tahoma";
                #endregion
                #region Footer main
                ws.Row(iRowCount + 11).Style.Font.FontName = "Tahoma";
                ws.Row(iRowCount + 11).Style.Font.FontSize = 10;
                ws.Cell(string.Format("A{0}", (iRowCount + 11))).Value = string.Format("Ghi chú :");
                ws.Cell(string.Format("A{0}", (iRowCount + 11))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell(string.Format("A{0}", (iRowCount + 11))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell(string.Format("A{0}", (iRowCount + 11))).Style.Font.Underline = XLFontUnderlineValues.Single;
                ws.Range(string.Format("A{0}:E{0}", (iRowCount + 11))).Row(1).Merge();

                ws.Cell(string.Format("M{0}", (iRowCount + 11))).Value = string.Format("Ngày ........ Tháng ........ Năm.........");
                ws.Cell(string.Format("M{0}", (iRowCount + 11))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("M{0}", (iRowCount + 11))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(string.Format("M{0}:R{1}", (iRowCount + 11), (iRowCount + 12))).Merge();

                ws.Row(iRowCount + 12).Style.Font.FontName = "Tahoma";
                ws.Row(iRowCount + 12).Style.Font.FontSize = 10;
                ws.Row(iRowCount + 12).Height = 3;
                ws.Cell(string.Format("A{0}", (iRowCount + 12))).Value = string.Format("- Điểm quá trình (ĐQT)\n- Điểm kết thúc(ĐKT)");
                //ws.Cell(string.Format("A{0}", (iRowCount + 12))).Value = string.Format("- Điểm quá trình (ĐQT)");
                //ws.Cell(string.Format("A{0}", (iRowCount + 13))).Value = string.Format("- Điểm kết thúc(ĐKT)");
                //ws.Cell(string.Format("A{0}", (iRowCount + 12))).RichText.AddText(string.Format("- Điểm quá trình (ĐQT) \n\r"));
                //ws.Cell(string.Format("A{0}", (iRowCount + 12))).RichText.AddText(string.Format("- Điểm kết thúc(ĐKT)"));
                ws.Cell(string.Format("A{0}", (iRowCount + 12))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("A{0}", (iRowCount + 12))).Style.Alignment.WrapText = true;

                ws.Cell(string.Format("A{0}", (iRowCount + 12))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range(string.Format("A{0}:E{1}", (iRowCount + 12), (iRowCount + 14))).Merge();

                ws.Row(iRowCount + 14).Style.Font.FontName = "Tahoma";
                ws.Row(iRowCount + 14).Style.Font.FontSize = 10;

                ws.Cell(string.Format("J{0}", (iRowCount + 14))).Value = string.Format("Giảng viên đánh giá");
                ws.Cell(string.Format("J{0}", (iRowCount + 14))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("J{0}", (iRowCount + 14))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(string.Format("J{0}:N{0}", (iRowCount + 14))).Merge();

                ws.Cell(string.Format("O{0}", (iRowCount + 14))).Value = string.Format("Trưởng bộ môn");
                ws.Cell(string.Format("O{0}", (iRowCount + 14))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("O{0}", (iRowCount + 14))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(string.Format("O{0}:R{0}", (iRowCount + 14))).Merge();

                ws.Row(iRowCount + 15).Style.Font.FontName = "Tahoma";
                ws.Row(iRowCount + 15).Style.Font.FontSize = 10;

                ws.Cell(string.Format("A{0}", (iRowCount + 15))).Value = string.Format("(Tính theo thang điểm 10, làm tròn đến 0.1)");
                ws.Cell(string.Format("A{0}", (iRowCount + 15))).Style.Font.Italic = true;
                ws.Cell(string.Format("A{0}", (iRowCount + 15))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("A{0}", (iRowCount + 15))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range(string.Format("A{0}:G{0}", (iRowCount + 15))).Merge();

                ws.Cell(string.Format("J{0}", (iRowCount + 15))).Value = string.Format("(Ký và ghi rõ họ tên)");
                ws.Cell(string.Format("J{0}", (iRowCount + 15))).Style.Font.Italic = true;
                ws.Cell(string.Format("J{0}", (iRowCount + 15))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("J{0}", (iRowCount + 15))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(string.Format("J{0}:N{0}", (iRowCount + 15))).Merge();

                ws.Cell(string.Format("O{0}", (iRowCount + 15))).Value = string.Format("(Ký và ghi rõ họ tên)");
                ws.Cell(string.Format("O{0}", (iRowCount + 15))).Style.Font.Italic = true;
                ws.Cell(string.Format("O{0}", (iRowCount + 15))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("O{0}", (iRowCount + 15))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(string.Format("O{0}:R{0}", (iRowCount + 15))).Merge();



                #endregion
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", strTenFile));
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        public void ProcessXuatDanhSachMatKhauSinhVienDangThi(int KiThiLopHocID)
        {
            DataTable dtContent = nuce.web.data.dnn_NuceThi_KiThi_LopHoc_SinhVien.getByKiThi_LopHoc(KiThiLopHocID);
            DataTable dtKiThiLopHoc = nuce.web.data.dnn_NuceThi_KiThi_LopHoc.get(KiThiLopHocID);
            string strTenFile = string.Format("export_{0}", DateTime.Now.ToFileTimeUtc());
            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("danh_sach");

                #region column
                ws.Column(1).Width = 2;
                ws.Column(2).Width = 1;
                ws.Column(3).Width = 10;
                ws.Column(4).Width = 1;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 1;
                ws.Column(7).Width = 1;
                ws.Column(8).Width = 8;
                ws.Column(9).Width = 1;
                ws.Column(10).Width = 1;
                ws.Column(11).Width = 6;
                ws.Column(12).Width = 3;
                ws.Column(13).Width = 2;
                ws.Column(14).Width = 1;
                ws.Column(15).Width = 5;
                ws.Column(16).Width = 1;
                ws.Column(17).Width = 15;
                ws.Column(18).Width = 1;
                #endregion
                #region Row1
                ws.Row(1).Height = 3;
                ws.Row(1).Style.Font.FontName = "Tahoma";
                ws.Range("A1:R1").Row(1).Merge();
                #endregion
                #region Row2
                ws.Row(2).Style.Font.FontName = "Tahoma";
                ws.Row(2).Style.Font.FontSize = 10;
                ws.Row(2).Height = 15;
                ws.Cell("A2").Value = string.Format("Trường Đại học Xây dựng");
                ws.Cell("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A2").Style.Font.Bold = true;
                ws.Range("A2:E2").Row(1).Merge();
                #endregion
                #region Row3
                ws.Row(3).Style.Font.FontName = "Tahoma";
                ws.Row(3).Style.Font.FontSize = 10;
                ws.Cell("A3").Value = string.Format("Phòng Đào tạo");
                ws.Cell("A3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A3").Style.Font.Bold = true;
                ws.Range("A3:E3").Row(1).Merge();
                #endregion
                #region Row4
                ws.Row(4).Style.Font.FontName = "Tahoma";
                ws.Row(4).Style.Font.FontSize = 12;
                ws.Row(4).Height = 25;
                ws.Cell("A4").Value = string.Format("DANH SÁCH SINH VIÊN");
                ws.Cell("A4").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A4").Style.Font.Bold = true;
                ws.Range("A4:R4").Row(1).Merge();
                #endregion
                #region Row5
                ws.Row(5).Style.Font.FontName = "Tahoma";
                ws.Row(5).Style.Font.FontSize = 10;
                ws.Row(5).Height = 15;
                ws.Cell("A5").Value = string.Format("{0}", dtKiThiLopHoc.Rows[0]["TenKiThi"].ToString());
                ws.Cell("A5").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("A5:R5").Row(1).Merge();
                #endregion
                #region Row6
                ws.Row(6).Style.Font.FontName = "Tahoma";
                ws.Row(6).Style.Font.FontSize = 10;
                ws.Row(6).Height = 15;

                ws.Cell("B6").Value = string.Format("{0}", "Môn học/Nhóm:");
                ws.Cell("B6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("B6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range("B6:D6").Row(1).Merge();

                ws.Cell("E6").Value = string.Format("{0}", dtKiThiLopHoc.Rows[0]["TenLopHoc"].ToString());
                ws.Cell("E6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("E6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("E6").Style.Font.Bold = true;
                ws.Range("E6:P6").Row(1).Merge();

                ws.Cell("Q6").Value = string.Format("Số tín chỉ: {0}", dtKiThiLopHoc.Rows[0]["SoChi"].ToString());
                ws.Cell("Q6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("Q6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range("Q6:R6").Row(1).Merge();
                #endregion
                #region Row7
                ws.Row(7).Style.Font.FontName = "Tahoma";
                ws.Row(7).Style.Font.FontSize = 10;
                ws.Row(7).Height = 18;

                ws.Cell("B7").Value = string.Format("{0}", "Ngày thi:");
                ws.Cell("B7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("B7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range("B7:D7").Row(1).Merge();

                DateTime dtNgayKetThuc = DateTime.Parse(dtKiThiLopHoc.Rows[0]["UpdatedDate"].ToString());
                ws.Cell("E7").Value = string.Format(" {0} / {1} / {2} ", dtNgayKetThuc.Day, dtNgayKetThuc.Month, dtNgayKetThuc.Year);
                ws.Cell("E7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("E7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("E7").Style.Font.Bold = true;
                ws.Range("E7:G7").Row(1).Merge();

                ws.Cell("H7").Value = string.Format("Phòng thi: ");
                ws.Cell("H7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("H7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("E7").Style.Font.Bold = true;
                ws.Range("H7:J7").Row(1).Merge();

                ws.Cell("K7").Value = string.Format("{0}", dtKiThiLopHoc.Rows[0]["PhongThi"].ToString());
                ws.Cell("K7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("K7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range("K7:R7").Row(1).Merge();
                #endregion
                #region Row8
                ws.Row(8).Height = 25;
                ws.Row(8).Style.Font.FontName = "Tahoma";
                #endregion
                #region Row9
                ws.Row(9).Style.Font.FontName = "Tahoma";
                ws.Row(9).Style.Font.FontSize = 10;
                ws.Row(9).Height = 30;
                ws.Row(9).Style.Font.Bold = true;

                ws.Range("A9:B9").Row(1).Merge();
                ws.Range("A9:B9").Value = string.Format("{0}", "STT");
                ws.Range("A9:B9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("A9:B9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("A9:B9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("A9:B9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Range("A9:B9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("A9:B9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Range("A9:B9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("A9:B9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Range("A9:B9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("A9:B9").Style.Border.BottomBorderColor = XLColor.Black;


                ws.Cell("C9").Value = string.Format("{0}", "Mã SV");
                ws.Cell("C9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("C9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("C9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("C9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Cell("C9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("C9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Cell("C9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("C9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Cell("C9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell("C9").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Range("D9:H9").Row(1).Merge();
                ws.Range("D9:H9").Value = string.Format("{0}", "Họ và Tên");
                ws.Range("D9:H9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("D9:H9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("D9:H9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("D9:H9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Range("D9:H9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("D9:H9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Range("D9:H9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("D9:H9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Range("D9:H9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("D9:H9").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Range("I9:K9").Row(1).Merge();
                ws.Range("I9:K9").Value = string.Format("{0}", "Lớp Q.Lý");
                ws.Range("I9:K9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("I9:K9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("I9:K9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("I9:K9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Range("I9:K9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("I9:K9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Range("I9:K9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("I9:K9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Range("I9:K9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("I9:K9").Style.Border.BottomBorderColor = XLColor.Black;


                ws.Range("L9:N9").Row(1).Merge();
                ws.Range("L9:N9").Value = string.Format("{0}", "Mật khẩu");
                ws.Range("L9:N9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("L9:N9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("L9:N9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("L9:N9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Range("L9:N9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("L9:N9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Range("L9:N9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("L9:N9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Range("L9:N9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("L9:N9").Style.Border.BottomBorderColor = XLColor.Black;

                ws.Range("O9:R9").Row(1).Merge();
                ws.Range("O9:R9").Value = string.Format("{0}", "Ghi chú");
                ws.Range("O9:R9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("O9:R9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("O9:R9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("O9:R9").Style.Border.LeftBorderColor = XLColor.Black;
                ws.Range("O9:R9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("O9:R9").Style.Border.RightBorderColor = XLColor.Black;
                ws.Range("O9:R9").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("O9:R9").Style.Border.TopBorderColor = XLColor.Black;
                ws.Range("O9:R9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("O9:R9").Style.Border.BottomBorderColor = XLColor.Black;
                #endregion

                float fDiem = 0;
                string strDiem = "";
                int iStatus = -1;
                int iRowCount = dtContent.Rows.Count;
                for (int i = 0; i < iRowCount; i++)
                {
                    #region Row element
                    ws.Row(i + 10).Style.Font.FontName = "Tahoma";
                    ws.Row(i + 10).Style.Font.FontSize = 10;
                    ws.Row(i + 10).Height = 19;

                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Value = string.Format("{0}", (i + 1));
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("A{0}:B{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;


                    ws.Cell(string.Format("C{0}", (i + 10))).Value = string.Format("{0}", dtContent.Rows[i]["MaSV"].ToString());
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("C{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Value = string.Format("{0}", dtContent.Rows[i]["Ho"].ToString());
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("D{0}:F{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Value = string.Format("{0}", dtContent.Rows[i]["TenSinhVien"].ToString());
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("G{0}:H{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Value = string.Format("{0}", dtContent.Rows[i]["TenLop"].ToString());
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("I{0}:K{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;

                    strDiem = dtContent.Rows[i]["MatKhau"].ToString();

                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Value = string.Format("{0}", strDiem);
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("L{0}:N{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;

                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Row(1).Merge();
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Value = string.Format("{0}", dtContent.Rows[i]["MoTa"].ToString());
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.LeftBorderColor = XLColor.Black;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range(string.Format("O{0}:R{0}", (i + 10))).Style.Border.BottomBorderColor = XLColor.Black;
                    #endregion
                }

                #region Footer 1
                ws.Row(iRowCount + 10).Height = 3;
                ws.Row(iRowCount + 10).Style.Font.FontName = "Tahoma";
                #endregion
                #region Footer main
                ws.Row(iRowCount + 11).Style.Font.FontName = "Tahoma";
                ws.Row(iRowCount + 11).Style.Font.FontSize = 10;
                ws.Cell(string.Format("A{0}", (iRowCount + 11))).Value = string.Format("Ghi chú :");
                ws.Cell(string.Format("A{0}", (iRowCount + 11))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell(string.Format("A{0}", (iRowCount + 11))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell(string.Format("A{0}", (iRowCount + 11))).Style.Font.Underline = XLFontUnderlineValues.Single;
                ws.Range(string.Format("A{0}:E{0}", (iRowCount + 11))).Row(1).Merge();

                ws.Cell(string.Format("M{0}", (iRowCount + 11))).Value = string.Format("Ngày ........ Tháng ........ Năm.........");
                ws.Cell(string.Format("M{0}", (iRowCount + 11))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("M{0}", (iRowCount + 11))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(string.Format("M{0}:R{1}", (iRowCount + 11), (iRowCount + 12))).Merge();

                ws.Row(iRowCount + 12).Style.Font.FontName = "Tahoma";
                ws.Row(iRowCount + 12).Style.Font.FontSize = 10;
                ws.Row(iRowCount + 12).Height = 3;
                ws.Cell(string.Format("A{0}", (iRowCount + 12))).Value = string.Format("- Điểm quá trình (ĐQT)\n- Điểm kết thúc(ĐKT)");
                //ws.Cell(string.Format("A{0}", (iRowCount + 12))).Value = string.Format("- Điểm quá trình (ĐQT)");
                //ws.Cell(string.Format("A{0}", (iRowCount + 13))).Value = string.Format("- Điểm kết thúc(ĐKT)");
                //ws.Cell(string.Format("A{0}", (iRowCount + 12))).RichText.AddText(string.Format("- Điểm quá trình (ĐQT) \n\r"));
                //ws.Cell(string.Format("A{0}", (iRowCount + 12))).RichText.AddText(string.Format("- Điểm kết thúc(ĐKT)"));
                ws.Cell(string.Format("A{0}", (iRowCount + 12))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("A{0}", (iRowCount + 12))).Style.Alignment.WrapText = true;

                ws.Cell(string.Format("A{0}", (iRowCount + 12))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range(string.Format("A{0}:E{1}", (iRowCount + 12), (iRowCount + 14))).Merge();

                ws.Row(iRowCount + 14).Style.Font.FontName = "Tahoma";
                ws.Row(iRowCount + 14).Style.Font.FontSize = 10;

                ws.Cell(string.Format("J{0}", (iRowCount + 14))).Value = string.Format("Giảng viên đánh giá");
                ws.Cell(string.Format("J{0}", (iRowCount + 14))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("J{0}", (iRowCount + 14))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(string.Format("J{0}:N{0}", (iRowCount + 14))).Merge();

                ws.Cell(string.Format("O{0}", (iRowCount + 14))).Value = string.Format("Trưởng bộ môn");
                ws.Cell(string.Format("O{0}", (iRowCount + 14))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("O{0}", (iRowCount + 14))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(string.Format("O{0}:R{0}", (iRowCount + 14))).Merge();

                ws.Row(iRowCount + 15).Style.Font.FontName = "Tahoma";
                ws.Row(iRowCount + 15).Style.Font.FontSize = 10;

                ws.Cell(string.Format("A{0}", (iRowCount + 15))).Value = string.Format("(Tính theo thang điểm 10, làm tròn đến 0.1)");
                ws.Cell(string.Format("A{0}", (iRowCount + 15))).Style.Font.Italic = true;
                ws.Cell(string.Format("A{0}", (iRowCount + 15))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("A{0}", (iRowCount + 15))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range(string.Format("A{0}:G{0}", (iRowCount + 15))).Merge();

                ws.Cell(string.Format("J{0}", (iRowCount + 15))).Value = string.Format("(Ký và ghi rõ họ tên)");
                ws.Cell(string.Format("J{0}", (iRowCount + 15))).Style.Font.Italic = true;
                ws.Cell(string.Format("J{0}", (iRowCount + 15))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("J{0}", (iRowCount + 15))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(string.Format("J{0}:N{0}", (iRowCount + 15))).Merge();

                ws.Cell(string.Format("O{0}", (iRowCount + 15))).Value = string.Format("(Ký và ghi rõ họ tên)");
                ws.Cell(string.Format("O{0}", (iRowCount + 15))).Style.Font.Italic = true;
                ws.Cell(string.Format("O{0}", (iRowCount + 15))).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(string.Format("O{0}", (iRowCount + 15))).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(string.Format("O{0}:R{0}", (iRowCount + 15))).Merge();



                #endregion
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", strTenFile));
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        public void ProcessDanhSachDiemDanh(int ituanhientaiid)
        {
            DataTable dtTableDiemDanh = nuce.web.data.dnn_NuceQLPM_SinhVien_DiemDanh.GetDataHByTuanHienTai(ituanhientaiid);
            DataTable dtTuanHientai = nuce.web.data.dnn_NuceQLPM_TuanHienTai.getByType1(ituanhientaiid, 2);
            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                //wb.Worksheets.Add(dt, sheetName);
                string strTenTuan = dtTuanHientai.Rows[0]["TenTuan"].ToString();
                var ws = wb.Worksheets.Add(strTenTuan);
                ws.Column(1).Width = 5;
                ws.Column(2).Width = 12;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 30;
                ws.Cell("A1").Value = string.Format("Kết quả buổi điểm danh ngày {0} {1} ca học {2} tại phòng {3}", DateTime.Parse(dtTuanHientai.Rows[0]["Ngay"].ToString()).ToString("dd/MM/yyyy"), dtTuanHientai.Rows[0]["TenTuan"].ToString(), dtTuanHientai.Rows[0]["TenCaHoc"].ToString(), dtTuanHientai.Rows[0]["TenPhongHoc"].ToString());
                ws.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("A1:G1").Row(1).Merge();
                ws.Cell("A2").Value = "STT";
                ws.Cell("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A2").Style.Alignment.ShrinkToFit = true;

                ws.Cell("B2").Value = "Mã sinh viên";
                ws.Cell("B2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("B2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("B2").Style.Alignment.ShrinkToFit = true;

                ws.Cell("C2").Value = "Họ và tên";
                ws.Cell("C2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("C2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("C2").Style.Alignment.ShrinkToFit = true;

                ws.Cell("D2").Value = "Mã lớp quản lý";
                ws.Cell("D2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("D2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("D2").Style.Alignment.ShrinkToFit = true;

                ws.Cell("E2").Value = "Điểm danh";
                ws.Cell("E2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("E2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("E2").Style.Alignment.ShrinkToFit = true;

                ws.Cell("F2").Value = "Điểm danh tự động";
                ws.Cell("F2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("F2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("F2").Style.Alignment.ShrinkToFit = true;

                ws.Cell("G2").Value = "Ghi Chú";
                ws.Cell("G2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("G2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                for (int i = 0; i < dtTableDiemDanh.Rows.Count; i++)
                {
                    ws.Cell("A" + (i + 3).ToString()).Value = (i + 1).ToString();
                    ws.Cell("A" + (i + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                    ws.Cell("A" + (i + 3).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    ws.Cell("B" + (i + 3).ToString()).Value = dtTableDiemDanh.Rows[i]["MaSV"].ToString();
                    ws.Cell("B" + (i + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                    ws.Cell("B" + (i + 3).ToString()).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    ws.Cell("C" + (i + 3).ToString()).Value = dtTableDiemDanh.Rows[i]["HoVaTen"].ToString();
                    ws.Cell("C" + (i + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    ws.Cell("D" + (i + 3).ToString()).Value = dtTableDiemDanh.Rows[i]["LopQuanLy"].ToString();
                    ws.Cell("D" + (i + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    string strDiemDanh = "Chưa điểm danh"
                    ;
                    if (bool.Parse(dtTableDiemDanh.Rows[i]["IsDiemDanh"].ToString()))
                        strDiemDanh = "Đã điểm danh";
                    ws.Cell("E" + (i + 3).ToString()).Value = strDiemDanh
                    ;
                    ws.Cell("E" + (i + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    strDiemDanh = "Chưa điểm danh"
                    ;
                    if (bool.Parse(dtTableDiemDanh.Rows[i]["IsDiemDanhTuDong"].ToString()))
                        strDiemDanh = "Đã điểm danh";

                    ws.Cell("F" + (i + 3).ToString()).Value = strDiemDanh;
                    ws.Cell("F" + (i + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    ws.Cell("G" + (i + 3).ToString()).Value = dtTableDiemDanh.Rows[i]["GhiChu"].ToString();
                    ws.Cell("G" + (i + 3).ToString()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", strTenTuan));
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        public void ProcessDanhSachKhachHangDangKiEmail()
        {
            string constr = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT [Status],[Subject],[SenderEmail],[Message] ,[Comment],CreatedOnDate FROM [dbo].[dnn_THCommon_RequestServices] where CategoryId=343 and status<>4 order by CreatedOnDate desc"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            ExportToExcel(string.Format("DanhSachKhachHangDangKiEmail_{0}", DateTime.Now.ToFileTimeUtc()), "DanhSachKhachHangDangKiEmail", dt);
                        }
                    }
                }
            }
        }
        public void ExportToExcel(string fileName, string sheetName, DataTable dt)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, sheetName);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", fileName));
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        public void ExportToExcel(ref string html, string fileName)
        {
            //html = html.Replace("&gt;", ">");
            //html = html.Replace("&lt;", "<");
            //HttpContext.Current.Response.ClearContent();
            //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + "_" + DateTime.Now.ToString("M_dd_yyyy_H_M_s") + ".xls");
            //HttpContext.Current.Response.ContentType = "application/xls";
            //HttpContext.Current.Response.Write(html);
            //HttpContext.Current.Response.End();
            string constr = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from dnn_Users"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt, "Customers");

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}