using ClosedXML.Excel;
using DotNetNuke.Entities.Modules;
using GemBox.Document;
using GemBox.Document.CustomMarkups;
using GemBox.Document.Drawing;
using GemBox.Document.Tables;
using Ionic.Zip;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using nuce.web.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;

namespace nuce.ad.ctsv
{
    public partial class QuanLyDichVu_VayVonNganHang : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spScriptInit.InnerHtml = string.Format("<script>var tabid={0};</script>", this.TabId);
                if (Request.QueryString["search"] != null)
                {
                    string strSearch = Request.QueryString["search"];
                    spScript.InnerHtml = string.Format("<script> $('#myInput').val('{0}');</script>", strSearch);
                    if (Request.QueryString["act"] != null)
                    {
                        string strAct = Request.QueryString["act"];
                        switch (strAct)
                        {
                            case "print":
                                print(strSearch);
                                break;
                            default: break;
                        }
                    }
                }
            }
        }
        private void exportExcel(string jsonData)
        {
            var data = JsonConvert.DeserializeObject<List<JsonData>>(jsonData);
            Nuce_CTSV.updateStudentList(data);

            string xacNhanIds = "";
            foreach (var item in data)
            {
                xacNhanIds += $"{item.ID},";
            }
            xacNhanIds = xacNhanIds.Remove(xacNhanIds.LastIndexOf(","));
            string sql = $@"select xacnhan.*, DateOfBirth,NgaySinh, HKTT_SoNha, HKTT_Pho, HKTT_Phuong, HKTT_Quan, HKTT_Tinh, a.EmailNhaTruong, a.Mobile
                        ,[LaNoiTru],[DiaChiCuThe],[ClassCode], c.Name as TenKhoa, b.SchoolYear as NienKhoa
                        ,a.CMT, a.CMT_NgayCap, a.CMT_NoiCap, a.GioiTinh, aaa.Name as NganhHoc
                        FROM [dbo].[AS_Academy_Student_SV_VayVonNganHang] xacnhan
                        LEFT JOIN [dbo].[AS_Academy_Student] a ON xacnhan.StudentID = a.id
                        left JOIN [dbo].[AS_Academy_Class] b on a.ClassID = b.ID 
                        LEFT join[dbo].[AS_Academy_Faculty] c ON b.FacultyID = c.ID
                        LEFT JOIN dbo.AS_Academy_Academics AS AAA ON b.AcademicsID = aaa.ID
                        WHERE xacnhan.ID IN ({xacNhanIds});

                        SELECT * FROM dbo.AS_Academy_Student_SV_ThietLapThamSoDichVu AS AASSTLTSDV 
                        WHERE AASSTLTSDV.DichVuID = 6 AND AASSTLTSDV.Name = N'HocPhiThang'";
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql);
            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                var xacNhanTbl = ds.Tables[0];
                var paramTbl = ds.Tables[1];
                string hocPhiThang = "";
                if (paramTbl.Rows.Count > 0)
                {
                    hocPhiThang = Nuce_CTSV.firstOrDefault(paramTbl.Rows, 0, "Value");
                }
                int i = 0;
                int firstRow = 1;
                #region title
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Ngày sinh");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Giới tính");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Lớp");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Khoa/Ban quản lý");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Số CMTND");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Ngày cấp");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Nơi cấp");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Mã trường");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Tên trường");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Ngành học");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Hệ đào tạo");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Khoá");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Loại hình đào tạo");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Năm nhập học");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Thời gian ra trường");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Số tiền học phí hàng tháng");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Ngày ký");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Tháng ký");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Năm ký");
                Nuce_CTSV.initHeaderCell(ws, firstRow, ++i, "Số điện thoại");

                ws.Row(firstRow).Height = 32;
                string tenTruong = "ĐẠI HỌC XÂY DỰNG";

                int colNum = i;
                #endregion
                #region value
                int recordLen = xacNhanTbl.Rows.Count;
                int col = 0;
                for (int j = 0; j < recordLen; j++)
                {
                    DateTime ngaySinh;
                    if (xacNhanTbl.Rows[j].IsNull("DateOfBirth"))
                    {
                        ngaySinh = DateTime.Parse("1/1/1900");
                    }
                    else
                    {
                        ngaySinh = DateTime.ParseExact(xacNhanTbl.Rows[j]["DateOfBirth"].ToString(), "dd/MM/yy", CultureInfo.InvariantCulture);
                    }
                    DateTime cmtNgayCap;
                    string tmpCmtNgayCap = xacNhanTbl.Rows[0].IsNull("CMT_NgayCap") ? "1/1/1980" : xacNhanTbl.Rows[0]["CMT_NgayCap"].ToString();
                    try
                    {
                        cmtNgayCap = DateTime.ParseExact(tmpCmtNgayCap, "dd/MM/yy", CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        cmtNgayCap = DateTime.Parse(tmpCmtNgayCap);
                    }
                    string studentCode = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "StudentCode");
                    string studentName = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "StudentName");
                    string email = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "EmailNhaTruong");
                    string phuong = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "HKTT_Phuong");
                    string quan = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "HKTT_Quan");
                    string tinh = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "HKTT_Tinh");
                    string classCode = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "ClassCode");
                    string nienKhoa = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "NienKhoa");
                    string tenKhoa = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "TenKhoa");
                    string gioiTinh = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "GioiTinh");
                    gioiTinh = Nuce_CTSV.getGender(gioiTinh);
                    string cmtNoiCap = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "CMT_NoiCap");
                    string cmt = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "CMT");
                    string soDienThoai = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "Mobile");
                    string nganhHoc = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "NganhHoc");
                    DateTime now = DateTime.Now;
                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(studentCode);
                    ws.Cell(row, ++col).SetValue(studentName);
                    ws.Cell(row, ++col).SetValue(ngaySinh.ToString("dd/MM/yyyy"));
                    ws.Cell(row, ++col).SetValue(gioiTinh);
                    ws.Cell(row, ++col).SetValue(classCode);
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(cmt);
                    ws.Cell(row, ++col).SetValue(cmtNgayCap.ToString("dd/MM/yyyy"));
                    ws.Cell(row, ++col).SetValue(cmtNoiCap);
                    ws.Cell(row, ++col).SetValue("XD1");
                    ws.Cell(row, ++col).SetValue(tenTruong);
                    ws.Cell(row, ++col).SetValue(nganhHoc);
                    ws.Cell(row, ++col).SetValue("ĐẠI HỌC");
                    ws.Cell(row, ++col).SetValue(getKhoa(classCode));
                    ws.Cell(row, ++col).SetValue("Chính quy");
                    ws.Cell(row, ++col).SetValue(Nuce_CTSV.getNamNhapHoc(nienKhoa));
                    ws.Cell(row, ++col).SetValue(Nuce_CTSV.getNamRaTruong(nienKhoa));
                    ws.Cell(row, ++col).SetValue(hocPhiThang);
                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);
                    ws.Cell(row, ++col).SetValue(soDienThoai);
                }
                for (int j = 0; j < col; j++)
                {
                    ws.Column(j + 1).AdjustToContents();
                }
                #endregion
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", $"danh_sach_vay_von_ngan_hang_{DateTime.Now.ToFileTime()}"));
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        private void exportDoc(string jsonData)
        {
            var data = JsonConvert.DeserializeObject<List<JsonData>>(jsonData);
            Nuce_CTSV.updateStudentList(data);
            List<string> dirs = new List<string>();
            foreach (var item in data)
            {
                string dir = print(item.ID.ToString(), false);
                dirs.Add(dir.Replace("~", ""));
            }
            string zipName = "";
            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.AddFiles(dirs, false, "");
                zipName = Server.MapPath("NuceDataUpload/Templates/vay_von_ngan_hang_" + DateTime.Now.ToFileTime() + ".zip");
                zipFile.Save(zipName);
            }
            foreach (var dir in dirs)
            {
                File.Delete(dir);
            }
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", "attachment;filename=vay_von_ngan_hang" + DateTime.Now.ToFileTime() + ".zip");
            Response.WriteFile(zipName);
            Response.Flush();
            Response.Close();
            File.Delete(zipName);
        }
        private string print(string ID, bool saveFile = true)
        {
            string sql = string.Format(@"select * from [dbo].[AS_Academy_Student_SV_VayVonNganHang] where ID={0};
select DateOfBirth,NgaySinh, HKTT_SoNha, HKTT_Pho, HKTT_Phuong, HKTT_Quan, HKTT_Tinh
,[LaNoiTru],[DiaChiCuThe],[ClassCode], c.Name as TenKhoa, b.SchoolYear as NienKhoa
,a.CMT, a.CMT_NgayCap, a.CMT_NoiCap, a.GioiTinh, aaa.Name AS NganhHoc 
from[dbo].[AS_Academy_Student] a left join
[dbo].[AS_Academy_Class] b on a.ClassID = b.ID left join[dbo].[AS_Academy_Faculty] c
on b.FacultyID = c.ID 
LEFT JOIN dbo.AS_Academy_Academics AS AAA ON b.AcademicsID = aaa.ID
where a.ID in (select StudentID from[dbo].[AS_Academy_Student_SV_VayVonNganHang] where ID = {0});
select * from AS_Academy_Student_SV_ThietLapThamSoDichVu
where DichVuID = 6;", ID);
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql);
            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            DataTable dtParams = ds.Tables[2];

            string hocPhi = "";
            string stkNhaTruong = "";
            string nganHangNhaTruong = "";
            foreach (DataRow row in dtParams.Rows)
            {
                string paraName = row.IsNull("Name") ? "" : row["Name"].ToString();
                switch (paraName)
                {
                    case "TaiKhoanTruong":
                        stkNhaTruong = row.IsNull("Value") ? "" : row["Value"].ToString().Replace("\r", "").Replace("\n", "");
                        break;
                    case "NganHangTruong":
                        nganHangNhaTruong = row.IsNull("Value") ? "" : row["Value"].ToString().Replace("\r", "").Replace("\n", "");
                        break;
                    case "HocPhiThang":
                        hocPhi = row.IsNull("Value") ? "" : row["Value"].ToString().Replace("\r", "").Replace("\n", "");
                        break;
                    default:
                        break;
                }
            }

            if (dt1.Rows.Count > 0 && dt2.Rows.Count > 0)
            {
                DateTime NgaySinh;
                string tmpNgaySinh = dt2.Rows[0].IsNull("DateOfBirth") ? "1/1/1980" : dt2.Rows[0]["DateOfBirth"].ToString();
                try
                {
                    NgaySinh = DateTime.ParseExact(tmpNgaySinh, "dd/MM/yy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    NgaySinh = DateTime.Parse(tmpNgaySinh);
                }
                DateTime CmtNgayCap;
                string tmpCmtNgayCap = dt2.Rows[0].IsNull("CMT_NgayCap") ? "1/1/1980" : dt2.Rows[0]["CMT_NgayCap"].ToString();
                try
                {
                    CmtNgayCap = DateTime.ParseExact(tmpCmtNgayCap, "dd/MM/yy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    CmtNgayCap = DateTime.Parse(tmpCmtNgayCap);
                }

                string HKTT_SoNha = dt2.Rows[0].IsNull("HKTT_SoNha") ? "" : dt2.Rows[0]["HKTT_SoNha"].ToString();
                string HKTT_Pho = dt2.Rows[0].IsNull("HKTT_Pho") ? "" : dt2.Rows[0]["HKTT_Pho"].ToString();
                string HKTT_Phuong = dt2.Rows[0].IsNull("HKTT_Phuong") ? "" : dt2.Rows[0]["HKTT_Phuong"].ToString();
                string HKTT_Quan = dt2.Rows[0].IsNull("HKTT_Quan") ? "" : dt2.Rows[0]["HKTT_Quan"].ToString();
                string HKTT_Tinh = dt2.Rows[0].IsNull("HKTT_Tinh") ? "" : dt2.Rows[0]["HKTT_Tinh"].ToString();
                bool LaNoiTru = dt2.Rows[0].IsNull("LaNoiTru") ? false : bool.Parse(dt2.Rows[0]["LaNoiTru"].ToString());
                string DiaChiCuThe = dt2.Rows[0].IsNull("DiaChiCuThe") ? "" : dt2.Rows[0]["DiaChiCuThe"].ToString();
                string MaSV = dt1.Rows[0].IsNull("StudentCode") ? "" : dt1.Rows[0]["StudentCode"].ToString();
                string HoVaTen = dt1.Rows[0].IsNull("StudentName") ? "" : dt1.Rows[0]["StudentName"].ToString();
                string Class = dt2.Rows[0].IsNull("ClassCode") ? "" : dt2.Rows[0]["ClassCode"].ToString();
                string TenKhoa = dt2.Rows[0].IsNull("TenKhoa") ? "" : dt2.Rows[0]["TenKhoa"].ToString();
                string LoaiDaoTao = "Chính quy";
                string NienKhoa = dt2.Rows[0].IsNull("NienKhoa") ? "" : dt2.Rows[0]["NienKhoa"].ToString();
                string LyDoXacNhan = dt1.Rows[0].IsNull("LyDo") ? "" : dt1.Rows[0]["LyDo"].ToString();
                string cmt = dt2.Rows[0].IsNull("CMT") ? "" : dt2.Rows[0]["CMT"].ToString();
                string cmtNoiCap = dt2.Rows[0].IsNull("CMT_NoiCap") ? "" : dt2.Rows[0]["CMT_NoiCap"].ToString();
                string cmtNgayCap = CmtNgayCap.ToString("dd/MM/yyyy");
                string nghanhHoc = Nuce_CTSV.firstOrDefault(dt2.Rows, 0, "NganhHoc");
                string thuocDien = Nuce_CTSV.firstOrDefault(dt1.Rows, 0, "ThuocDien");
                string thuocDoiTuong = Nuce_CTSV.firstOrDefault(dt1.Rows, 0, "ThuocDoiTuong");
                string gioiTinh = Nuce_CTSV.firstOrDefault(dt2.Rows, 0, "GioiTinh");
                gioiTinh = Nuce_CTSV.getGender(gioiTinh);
                string matruong = "XD1";
                string tenTruong = "TRƯỜNG ĐẠI HỌC XÂY DỰNG";
                string heDaoTao = "ĐẠI HỌC";
                string khoa = getKhoa(Class);
                string namNhapHoc = Nuce_CTSV.getNamNhapHoc(NienKhoa);
                string namRaTruong = Nuce_CTSV.getNamRaTruong(NienKhoa);

                var Dien = new Dictionary<string, string>
                {
                    {"1", "☐" },
                    {"2", "☐" },
                    {"3", "☐" },
                };
                Dien[thuocDien] = "☒";

                var DoiTuong = new Dictionary<string, string>
                {
                    {"1", "☐" },
                    {"2", "☐" },
                    {"3", "☐" },
                };
                DoiTuong[thuocDoiTuong] = "☒";

                var GioiTinh = new Dictionary<string, string>
                {
                    {"Nam", "☐" },
                    {"Nữ", "☐" },
                };
                GioiTinh[gioiTinh] = "☒";



                //ComponentInfo.SetLicense("DTZX-HTZ5-B7Q6-2GA6");
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                DocumentModel document = new DocumentModel();
                document.DefaultCharacterFormat.Size = 12;
                document.DefaultCharacterFormat.FontName = "Times New Roman";

                Section section;
                section = new Section(document);
                var pageSetup = section.PageSetup;
                #region Cộng hòa xã hội chủ nghĩa việt nam
                Table table = new Table(document);
                table.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
                table.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
                var tableBorders = table.TableFormat.Borders;
                tableBorders.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
                table.Columns.Add(new TableColumn() { PreferredWidth = 35 });
                table.Columns.Add(new TableColumn() { PreferredWidth = 65 });
                TableRow rowT1 = new TableRow(document);
                table.Rows.Add(rowT1);

                rowT1.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "BỘ GIÁO DỤC VÀ ĐÀO TẠO"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = 12
                    }
                }
                )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Center
                    }
                })
                {
                    CellFormat = new TableCellFormat()
                    {
                        VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                    }
                }
                );
                rowT1.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                }
               )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Center
                    }
                })
                {
                    CellFormat = new TableCellFormat()
                    {
                        VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                    }
                }
               );
                TableRow rowT2 = new TableRow(document);
                table.Rows.Add(rowT2);

                rowT2.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "TRƯỜNG ĐẠI HỌC XÂY DỰNG"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                }
                )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Center
                    }
                })
                {
                    CellFormat = new TableCellFormat()
                    {
                        VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                    }
                }
                );
                rowT2.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "Độc lập – Tự do – Hạnh phúc"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                }
               )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Center
                    }
                })
                {
                    CellFormat = new TableCellFormat()
                    {
                        VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                    }
                }
               );
                var paragraph = new Paragraph(document);

                var horizontalLine1 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                     new HorizontalPosition(1.45, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                    new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                    new Size(80, 0)));
                horizontalLine1.Outline.Width = 1;
                horizontalLine1.Outline.Fill.SetSolid(Color.Black);
                paragraph.Inlines.Add(horizontalLine1);

                var horizontalLine2 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                    new HorizontalPosition(8.78, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                   new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                   new Size(151, 0)));
                horizontalLine2.Outline.Width = 1;
                horizontalLine2.Outline.Fill.SetSolid(Color.Black);
                paragraph.Inlines.Add(horizontalLine2);

                section.Blocks.Add(table);
                section.Blocks.Add(paragraph);
                #endregion
                #region TieuDe
                Paragraph paragraphTieuDe = new Paragraph(document,
                //new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, string.Format("{0}", "GIẤY XÁC NHẬN"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 18
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
              )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Center,
                        LineSpacing = 1.15
                    }
                };
                section.Blocks.Add(paragraphTieuDe);
                #endregion
                #region NoiDung
                Paragraph paragraphNoiDung = new Paragraph(document,
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Họ và tên sinh viên: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, HoVaTen) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Ngày sinh: ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                    new Run(document, $"{NgaySinh.ToString("dd/MM/yyyy")}  ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new Run(document, "Giới tính:    ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                    new Run(document, "Nam: ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                    new InlineContentControl(document, ContentControlType.CheckBox,
                        new Run(document, GioiTinh["Nam"]) { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                    new Run(document, "   Nữ: ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                    new InlineContentControl(document, ContentControlType.CheckBox,
                        new Run(document, GioiTinh["Nữ"]) { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                    //new Field(document, FieldType.FormCheckBox) { FormData = { Name = "Nữ" } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "CMND số: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{cmt} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new Run(document, "ngày cấp: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{cmtNgayCap} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new Run(document, "Nơi cấp: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{cmtNoiCap}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Mã trường theo học (mã quy ước trong  tuyển sinh ĐH): ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, matruong) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Tên trường: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, tenTruong) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Ngành học: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, nghanhHoc) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Hệ đào tạo (Đại học, cao đẳng, dạy nghề): ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, heDaoTao) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Khoá: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{khoa}  ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new Run(document, "Loại hình đào tạo: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{LoaiDaoTao}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Lớp: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{Class}  ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new Run(document, "Mã số SV: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{MaSV}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Khoa/Ban: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{TenKhoa}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Ngày nhập học:...../...../") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{namNhapHoc} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new Run(document, "Thời gian ra trường (tháng/năm):...../...../") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{namRaTruong}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "- Số tiền học phí hàng tháng: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{hocPhi}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new Run(document, " đồng") { CharacterFormat = new CharacterFormat { Size = 13 } }
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25, SpaceAfter = 0 } };
                section.Blocks.Add(paragraphNoiDung);
                #endregion
                #region Thuoc Dien/Doi Tuong
                Table tableDienDoiTuong = new Table(document);
                tableDienDoiTuong.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
                tableDienDoiTuong.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
                tableDienDoiTuong.TableFormat.AutomaticallyResizeToFitContents = false;
                var tblDienDoiTuongBorder = tableDienDoiTuong.TableFormat.Borders;
                tblDienDoiTuongBorder.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
                tableDienDoiTuong.Columns.Add(new TableColumn(30));
                tableDienDoiTuong.Columns.Add(new TableColumn(40));
                tableDienDoiTuong.Columns.Add(new TableColumn(30));
                TableRow rowDien = new TableRow(document);
                tableDienDoiTuong.Rows.Add(rowDien);
                TableRow rowDoiTuong = new TableRow(document);
                tableDienDoiTuong.Rows.Add(rowDoiTuong);
                rowDien.Cells.Add(
                    new TableCell(document, new Paragraph(document,
                        new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new Run(document, "Thuộc diện:") { CharacterFormat = new CharacterFormat { Size = 13 } }
                    )
                    { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
                );
                rowDoiTuong.Cells.Add(
                    new TableCell(document, new Paragraph(document,
                        new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new Run(document, "Thuộc đối tượng:") { CharacterFormat = new CharacterFormat { Size = 13 } }
                    )
                    { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
                );
                rowDien.Cells.Add(
                    new TableCell(document, new Paragraph(document,
                        //new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new Run(document, "- Không miễn giảm") { CharacterFormat = new CharacterFormat { Size = 13 } },
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        //new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new Run(document, "- Giảm học phí") { CharacterFormat = new CharacterFormat { Size = 13 } },
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        //new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new Run(document, "- Miễn học phí") { CharacterFormat = new CharacterFormat { Size = 13 } }
                    )
                    { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
                );
                rowDien.Cells.Add(
                    new TableCell(document, new Paragraph(document,
                        //new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new InlineContentControl(document, ContentControlType.CheckBox,
                        new Run(document, $"{Dien["1"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        //new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new InlineContentControl(document, ContentControlType.CheckBox,
                        new Run(document, $"{Dien["2"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        //new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new InlineContentControl(document, ContentControlType.CheckBox,
                        new Run(document, $"{Dien["3"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } })
                    )
                    { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
                );
                rowDoiTuong.Cells.Add(
                    new TableCell(document, new Paragraph(document,
                        //new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new Run(document, "- Mồ côi") { CharacterFormat = new CharacterFormat { Size = 13 } },
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        //new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new Run(document, "- Không mồ côi") { CharacterFormat = new CharacterFormat { Size = 13 } }
                    )
                    { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
                );
                rowDoiTuong.Cells.Add(
                    new TableCell(document, new Paragraph(document,
                        //new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new InlineContentControl(document, ContentControlType.CheckBox,
                        new Run(document, $"{DoiTuong["1"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        //new SpecialCharacter(document, SpecialCharacterType.Tab),
                        new InlineContentControl(document, ContentControlType.CheckBox,
                        new Run(document, $"{DoiTuong["2"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } })
                    )
                    { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
                );
                section.Blocks.Add(tableDienDoiTuong);
                #endregion
                #region Ket
                Paragraph paragraphKet = new Paragraph(document, 
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "- Trong thời gian theo học tại trường, anh (chị) ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new Run(document, $"{HoVaTen} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new Run(document, "không bị xử phạt hành chính trở lên về các hành vi: cờ bạc, nghiện hút, trộm cắp, buôn lậu.") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, $"- Số tài khoản của nhà trường: {stkNhaTruong}, tại ngân hàng{nganHangNhaTruong}") { CharacterFormat = new CharacterFormat { Size = 13 } }
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } };
                section.Blocks.Add(paragraphKet);
                #endregion
                #region Chu ky
                Table tableCK = new Table(document);
                tableCK.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
                tableCK.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
                tableCK.TableFormat.AutomaticallyResizeToFitContents = false;
                var tableBordersCK = tableCK.TableFormat.Borders;
                tableBordersCK.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
                tableCK.Columns.Add(new TableColumn(55));
                tableCK.Columns.Add(new TableColumn(45));
                TableRow rowT1CK = new TableRow(document);
                tableCK.Rows.Add(rowT1CK);

                rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", " ")))));

                rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document,
                    new Run(document, string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                    {
                        CharacterFormat = new CharacterFormat()
                        {
                            Italic = true,
                            Size = 13
                        }
                    }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new Run(document, string.Format("TL. HIỆU TRƯỞNG"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 13
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new Run(document, string.Format("(Ký tên, đóng dấu)"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true,
                        Size = 13
                    }
                }
               )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Center
                    }
                })
                {
                    CellFormat = new TableCellFormat()
                    {
                        VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                    }
                }
               );
                section.Blocks.Add(tableCK);
                #endregion
                document.Sections.Add(section);
                //var formFieldsData = document.Content.FormFieldsData;
                //var marriedFieldData = (FormCheckBoxData)formFieldsData["Nam"];
                //marriedFieldData.Value = true;


                if (saveFile)
                {
                    document.Save(Response, "vay_von_ngan_hang_" + MaSV + "_" + DateTime.Now.ToFileTime() + ".docx");
                }
                else
                {
                    string directoryPath = Server.MapPath("~/NuceDataUpload/Templates/" + "vay_von_ngan_hang_" + MaSV + "_" + DateTime.Now.ToFileTime() + ".docx");
                    document.Save(directoryPath);
                    return directoryPath;
                }
            }
            return "";
        }
        private string getKhoa(string classCode)
        {
            return $"K{classCode.Substring(0, 2)}";
        }
        protected void btnExportData_Click(object sender, EventArgs e)
        {
            exportDoc(txtInputData.Text ?? "");
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            exportExcel(txtInputData.Text ?? "");
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            var data = JsonConvert.DeserializeObject<JsonData>(txtInputData.Text);
            Nuce_CTSV.updateStudent(data);
            print(data.ID.ToString());
        }
    }
}