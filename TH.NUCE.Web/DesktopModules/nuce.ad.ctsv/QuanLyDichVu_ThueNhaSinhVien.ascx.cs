using ClosedXML.Excel;
using DotNetNuke.Entities.Modules;
using GemBox.Document;
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
    public partial class QuanLyDichVu_ThueNhaSinhVien : PortalModuleBase
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
                        , a.CMT, a.CMT_NgayCap, a.CMT_NoiCap, a.GioiTinh, a.DoiTuongUuTien
                        FROM [dbo].[AS_Academy_Student_SV_ThueNha] xacnhan
                        LEFT JOIN [dbo].[AS_Academy_Student] a ON xacnhan.StudentID = a.id
                        left JOIN [dbo].[AS_Academy_Class] b on a.ClassID = b.ID 
                        LEFT join[dbo].[AS_Academy_Faculty] c ON b.FacultyID = c.ID
                        WHERE xacnhan.ID IN ({xacNhanIds});";
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql);
            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                var xacNhanTbl = ds.Tables[0];
                int i = 0;
                int firstRow = 1;
                #region title
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Mã số SV");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Họ và tên");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Ngày sinh");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Email");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Xã");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Huyện");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Tỉnh");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Lớp");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Niên khóa");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Khoa quản lý");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Số điện thoại");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Hệ đào tạo");

                Nuce_CTSV.setStyleUniqueCol(ws, firstRow, ++i, "Năm thứ");
                Nuce_CTSV.setStyleUniqueCol(ws, firstRow, ++i, "Giới tính");
                Nuce_CTSV.setStyleUniqueCol(ws, firstRow, ++i, "Đối tượng\nưu tiên");
                ws.Cell(firstRow, i).Style.Alignment.WrapText = true;
                Nuce_CTSV.setStyleUniqueCol(ws, firstRow, ++i, "Số CMTND");
                Nuce_CTSV.setStyleUniqueCol(ws, firstRow, ++i, "Cấp\nngày/tháng/năm");
                ws.Cell(firstRow, i).Style.Alignment.WrapText = true;
                Nuce_CTSV.setStyleUniqueCol(ws, firstRow, ++i, "Nơi cấp");

                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Ngày ký");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Tháng ký");
                Nuce_CTSV.setStyle(ws, firstRow, ++i, "Năm ký");

                ws.Row(firstRow).Height = 32;

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
                    string mobile = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "Mobile");
                    string lyDo = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "LyDo");

                    string gioiTinh = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "GioiTinh");
                    gioiTinh = Nuce_CTSV.getGender(gioiTinh);
                    string doiTuongUuTien = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "DoiTuongUuTien");
                    string cmtNoiCap = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "CMT_NoiCap");
                    string cmt = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "CMT");
                    DateTime now = DateTime.Now;
                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(studentCode);
                    ws.Cell(row, ++col).SetValue(studentName);
                    ws.Cell(row, ++col).SetValue(ngaySinh.ToString("dd/MM/yyyy"));
                    ws.Cell(row, ++col).SetValue(email);
                    ws.Cell(row, ++col).SetValue(phuong);
                    ws.Cell(row, ++col).SetValue(quan);
                    ws.Cell(row, ++col).SetValue(tinh);
                    ws.Cell(row, ++col).SetValue(classCode);
                    ws.Cell(row, ++col).SetValue(nienKhoa);
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(mobile);
                    ws.Cell(row, ++col).SetValue("Chính quy");
                    ws.Cell(row, ++col).SetValue(Nuce_CTSV.getNamThu(nienKhoa));
                    ws.Cell(row, ++col).SetValue(gioiTinh);
                    ws.Cell(row, ++col).SetValue(doiTuongUuTien);
                    ws.Cell(row, ++col).SetValue(cmt);
                    ws.Cell(row, ++col).SetValue(cmtNgayCap.ToString("dd/MM/yyyy"));
                    ws.Cell(row, ++col).SetValue(cmtNoiCap);
                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);
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
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", $"danh_sach_xac_nhan_{DateTime.Now.ToFileTime()}"));
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
                //dirs.Add(dir);
            }
            // Create destination document.
            //ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            //var destination = new DocumentModel();

            //// Merge multiple source documents by importing their content at the end.
            //foreach (var fileDir in dirs)
            //{
            //    var source = DocumentModel.Load(fileDir);
            //    destination.Content.End.InsertRange(source.Content);
            //}

            //// Save joined documents into one file.
            //destination.Save(Response, "Merged Files.docx");
            string zipName = "";
            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.AddFiles(dirs, false, "");
                zipName = Server.MapPath("NuceDataUpload/Templates/xac_nhan_" + DateTime.Now.ToFileTime() + ".zip");
                zipFile.Save(zipName);
            }
            foreach (var dir in dirs)
            {
                File.Delete(dir);
            }
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", "attachment;filename=xac_nhan_" + DateTime.Now.ToFileTime() + ".zip");
            Response.WriteFile(zipName);
            Response.Flush();
            Response.Close();
            File.Delete(zipName);
        }
        private string print(string ID, bool saveFile = true)
        {

            string sql = string.Format(@"select * from [dbo].[AS_Academy_Student_SV_ThueNha] where ID={0};
select DateOfBirth,NgaySinh, HKTT_SoNha, HKTT_Pho, HKTT_Phuong, HKTT_Quan, HKTT_Tinh
,[LaNoiTru],[DiaChiCuThe],[ClassCode], c.Name as TenKhoa, b.SchoolYear as NienKhoa
, a.CMT, a.CMT_NgayCap, a.CMT_NoiCap, a.GioiTinh, a.DoiTuongUuTien
from[dbo].[AS_Academy_Student] a left join
[dbo].[AS_Academy_Class] b on a.ClassID = b.ID left join[dbo].[AS_Academy_Faculty] c
on b.FacultyID = c.ID where a.ID in (select StudentID from[dbo].[AS_Academy_Student_SV_ThueNha] where ID = {0}); 
select * from AS_Academy_Student_SV_ThietLapThamSoDichVu
where DichVuID = 7;", ID);
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql);
            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            DataTable dtParams = ds.Tables[2];

            string ChucDanhNguoiKy = "";
            string TenNguoiKy = "";
            foreach (DataRow row in dtParams.Rows)
            {
                string paraName = row.IsNull("Name") ? "" : row["Name"].ToString();
                switch (paraName)
                {
                    case "ChucDanhNguoiKy":
                        ChucDanhNguoiKy = row.IsNull("Value") ? "" : row["Value"].ToString();
                        break;
                    case "TenNguoiKy":
                        TenNguoiKy = row.IsNull("Value") ? "" : row["Value"].ToString();
                        break;
                    default:
                        break;
                }
            }
            string desChucDanhNguoiKy = ChucDanhNguoiKy.Replace("\r", "_").Replace("\n", "_");
            string desTenNguoiKy = TenNguoiKy.Replace("\r", "_").Replace("\n", "_");

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

                DateTime cmtNgayCap;
                string tmpCmtNgayCap = dt2.Rows[0].IsNull("CMT_NgayCap") ? "1/1/1980" : dt2.Rows[0]["CMT_NgayCap"].ToString();
                try
                {
                    cmtNgayCap = DateTime.ParseExact(tmpCmtNgayCap, "dd/MM/yy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    cmtNgayCap = DateTime.Parse(tmpCmtNgayCap);
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
                string HeDaoTao = "Chính quy";
                string NienKhoa = dt2.Rows[0].IsNull("NienKhoa") ? "" : dt2.Rows[0]["NienKhoa"].ToString();
                string LyDoXacNhan = dt1.Rows[0].IsNull("LyDo") ? "" : dt1.Rows[0]["LyDo"].ToString();
                string gioiTinh = Nuce_CTSV.firstOrDefault(dt2.Rows, 0, "GioiTinh");
                gioiTinh = Nuce_CTSV.getGender(gioiTinh);
                string doiTuongUuTien = Nuce_CTSV.firstOrDefault(dt2.Rows, 0, "DoiTuongUuTien");
                string cmtNoiCap = Nuce_CTSV.firstOrDefault(dt2.Rows, 0, "CMT_NoiCap");
                string cmt = Nuce_CTSV.firstOrDefault(dt2.Rows, 0, "CMT");

                //ComponentInfo.SetLicense("DTZX-HTZ5-B7Q6-2GA6");
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                DocumentModel document = new DocumentModel();
                document.DefaultCharacterFormat.Size = 13;
                document.DefaultCharacterFormat.FontName = "Times New Roman";

                Section section;
                section = new Section(document);
                var pageSetup = section.PageSetup;
                #region Cộng hòa xã hội chủ nghĩa việt nam
                var paragraph = new Paragraph(document,
                    new Run(document, "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new Run(document, "Độc lập - Tự do - Hạnh phúc") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } }
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Center } };


                var horizontalLine1 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                     new HorizontalPosition(5.2, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                    new VerticalPosition(3.8, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                    new Size(160, 0)));
                horizontalLine1.Outline.Width = 1;
                horizontalLine1.Outline.Fill.SetSolid(new Color(91, 155, 213));
                paragraph.Inlines.Add(horizontalLine1);

                section.Blocks.Add(paragraph);
                #endregion
                #region TieuDe
                Paragraph paragraphTieuDe = new Paragraph(document,
                //new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, string.Format("{0}", "ĐƠN ĐỀ NGHỊ THUÊ NHÀ Ở SINH VIÊN"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 14
                    }
                }
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
                    new Run(document, "Kính gửi: Ban Quản lý vận hành Khu nhà ở sinh viên Pháp Vân – Tứ Hiệp."),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Tên tôi là: "),
                    new Run(document, HoVaTen) { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "(Nam/Nữ):  "),
                    new Run(document, gioiTinh) { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "CMTND số: "),
                    new Run(document, cmt) { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "cấp ngày: "),
                    new Run(document, cmtNgayCap.ToString("dd/MM/yyyy")) { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "nơi cấp: "),
                    new Run(document, cmtNoiCap) { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Hộ khẩu thường trú: "),
                    new Run(document, $"{HKTT_Phuong}, {HKTT_Quan}, {HKTT_Tinh}") { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Sinh viên, học sinh năm thứ: "),
                    new Run(document, $"{Nuce_CTSV.getNamThu(NienKhoa)}") { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Lớp: "),
                    new Run(document, Class) { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Khoá: "),
                    new Run(document, NienKhoa) { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Ngành (khoa): "),
                    new Run(document, TenKhoa) { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Trường: "),
                    new Run(document, "Đại học Xây dựng") { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Số thẻ sinh viên, học viên (nếu có): "),
                    new Run(document, MaSV) { CharacterFormat = new CharacterFormat { Bold = true } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Đối tượng ưu tiên (nếu có): "),
                    new Run(document, doiTuongUuTien) { CharacterFormat = new CharacterFormat { Bold = true } }
                )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left,
                        LineSpacing = 1.5,
                        SpaceAfter = 0
                    }
                };
                section.Blocks.Add(paragraphNoiDung);
                #endregion
                #region toi da
                
                Paragraph paragraphToiDa = new Paragraph(document,
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Tôi làm đơn này đề nghị: "),
                    new Run(document, "BQL vận hành Khu nhà ở SV Pháp Vân ") { CharacterFormat = new CharacterFormat { Bold = true } },
                    new Run(document, "xét duyệt cho tôi được thuê nhà ở tại Khu nhà ở sinh viên Pháp Vân – Tứ Hiệp."),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Tôi đã đọc Bản nội quy sử dụng nhà ở sinh viên và cam kết tuân thủ nội quy sử dụng nhà ở sinh viên; cam kết trả tiền thuê nhà đầy đủ, đúng thời hạn khi được thuê nhà ở."),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Tôi cam đoan những lời kê khai trong đơn là đúng sự thật, tôi xin chịu trách nhiệm trước pháp luật về các nội dung kê khai.")
                 )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left,
                        LineSpacing = 1.5,
                        SpaceAfter = 0
                    }
                };
                section.Blocks.Add(paragraphToiDa);
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

                rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document,
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new Run(document, desChucDanhNguoiKy)
                    {
                        CharacterFormat = new CharacterFormat()
                        {
                            Bold = true,
                            Size = 13
                        }
                    }
                    , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                    , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                    , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                    , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                    , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                    , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                    , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                    , new Run(document, desTenNguoiKy)
                )
                {
                    ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Center}
                }));

                rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document,
                    new Run(document, string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                    {
                        CharacterFormat = new CharacterFormat()
                        {
                            Italic = true,
                            Size = 13
                        }
                    }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, "Người viết đơn") { CharacterFormat = new CharacterFormat { Bold = true, Size = 13 } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, "(Ký và ghi rõ họ tên)") { CharacterFormat = new CharacterFormat { Italic = true, Size = 12 } }
               )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Center
                    }
                })
                //{
                //    CellFormat = new TableCellFormat()
                //    {
                //        VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                //    }
                //}
               );
                section.Blocks.Add(tableCK);
                #endregion
                document.Sections.Add(section);
                document.Content.Replace(desChucDanhNguoiKy, ChucDanhNguoiKy);
                document.Content.Replace(desTenNguoiKy, TenNguoiKy);
                //document.Content.Replace(desChucDanhNguoiKy, ChucDanhNguoiKy);
                //document.Content.Replace(desTenNguoiKy, TenNguoiKy);
                if (saveFile)
                {
                    document.Save(Response, "thue_nha_" + MaSV + "_" + DateTime.Now.ToFileTime() + ".docx");
                }
                else
                {
                    string directoryPath = Server.MapPath("~/NuceDataUpload/Templates/" + "thue_nha_" + MaSV + "_" + DateTime.Now.ToFileTime() + ".docx");
                    document.Save(directoryPath);
                    return directoryPath;
                }
            }
            return "";
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