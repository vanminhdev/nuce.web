using ClosedXML.Excel;
using DotNetNuke.Entities.Modules;
using GemBox.Document;
using GemBox.Document.Tables;
using Ionic.Zip;
using Newtonsoft.Json;
using nuce.web.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;

namespace nuce.ad.ctsv
{
    public partial class QuanLyDichVu_GiayGioiThieu : PortalModuleBase
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
                        FROM [dbo].[AS_Academy_Student_SV_GioiThieu] xacnhan
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
                Nuce_CTSV.setStyleUniqueCol(ws, firstRow, ++i, "Kính gửi");
                Nuce_CTSV.setStyleUniqueCol(ws, firstRow, ++i, "Đến gặp");
                Nuce_CTSV.setStyleUniqueCol(ws, firstRow, ++i, "Về việc");
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
                    string donVi = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "DonVi");
                    string denGap = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "DenGap");
                    string veViec = Nuce_CTSV.firstOrDefault(xacNhanTbl.Rows, j, "VeViec");
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
                    ws.Cell(row, ++col).SetValue(donVi);
                    ws.Cell(row, ++col).SetValue(denGap);
                    ws.Cell(row, ++col).SetValue(veViec);
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
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", $"danh_sach_gioi_thieu_{DateTime.Now.ToFileTime()}"));
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
            string sql = string.Format(@"select * from [dbo].[AS_Academy_Student_SV_GioiThieu] where ID={0};
select DateOfBirth,NgaySinh, HKTT_SoNha, HKTT_Pho, HKTT_Phuong, HKTT_Quan, HKTT_Tinh
,[LaNoiTru],[DiaChiCuThe],[ClassCode], c.Name as TenKhoa, b.SchoolYear as NienKhoa from [dbo].[AS_Academy_Student] a left join
[dbo].[AS_Academy_Class] b on a.ClassID = b.ID left join [dbo].[AS_Academy_Faculty] c
on b.FacultyID = c.ID where a.ID in (select StudentID from [dbo].[AS_Academy_Student_SV_GioiThieu] where ID = {0}); ", ID);
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql);
            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            if (dt1.Rows.Count > 0 && dt2.Rows.Count > 0)
            {
                DateTime NgaySinh = dt2.Rows[0].IsNull("NgaySinh") ? DateTime.Parse("1/1/1900") : DateTime.Parse(dt2.Rows[0]["NgaySinh"].ToString());

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
                string DenGap = dt1.Rows[0].IsNull("DenGap") ? "" : dt1.Rows[0]["DenGap"].ToString();
                string KinhGui = dt1.Rows[0].IsNull("DonVi") ? "" : dt1.Rows[0]["DonVi"].ToString();
                string VeViec = dt1.Rows[0].IsNull("VeViec") ? "" : dt1.Rows[0]["VeViec"].ToString();

                //ComponentInfo.SetLicense("DTZX-HTZ5-B7Q6-2GA6");
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                DocumentModel document = new DocumentModel();
                document.DefaultCharacterFormat.Size = 12;
                document.DefaultCharacterFormat.FontName = "Times New Roman";

                Section section;
                section = new Section(document);

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
                TableRow rowT3 = new TableRow(document);
                table.Rows.Add(rowT3);

                rowT3.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "----------------------------------------------------"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 5
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
                rowT3.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "---------------------------------------------------------------"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 5
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

                section.Blocks.Add(table);
                #endregion
                #region TieuDe
                Paragraph paragraphTieuDe = new Paragraph(document,
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, string.Format("{0}", "GIẤY GIỚI THIỆU"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 14
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new Run(document, "Kính gửi: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 13
                    }
                }
                , new Run(document, KinhGui)
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = 13
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new Run(document, "HIỆU TRƯỞNG TRƯỜNG ĐẠI HỌC XÂY DỰNG")
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
                        Alignment = GemBox.Document.HorizontalAlignment.Center,
                        LineSpacing = 1.25
                    }
                };
                section.Blocks.Add(paragraphTieuDe);
                #endregion
                #region NoiDung
                Paragraph paragraphNoiDung = new Paragraph(document,
               // new SpecialCharacter(document, SpecialCharacterType.LineBreak),
               new Run(document, string.Format("{0}", "Trân trọng giới thiệu Anh / chị: "))
               {
                   CharacterFormat = new CharacterFormat()
                   {
                       Bold = true,
                       Size = 12
                   }
               }
               , new Run(document, string.Format("{0}", HoVaTen))
               , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
               , new Run(document, "Là sinh viên đang học tại lớp: ")
               {
                   CharacterFormat = new CharacterFormat()
                   {
                       Bold = true,
                       Size = 12
                   }
               }
                , new Run(document, string.Format("{0} ", Class))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = 12
                    }
                }
                 , new Run(document, " Khóa: ")
                 {
                     CharacterFormat = new CharacterFormat()
                     {
                         Bold = true,
                         Size = 12
                     }
                 }
                , new Run(document, string.Format("{0} ", TenKhoa))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = 12
                    }
                }
               , new Run(document, " Hệ đào tạo:  ")
               {
                   CharacterFormat = new CharacterFormat()
                   {
                       Bold = true,
                       Size = 12
                   }
               }
               , new Run(document, string.Format("{0} ", HeDaoTao))
               {
                   CharacterFormat = new CharacterFormat()
                   {
                       Size = 12
                   }
               }
               , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
               , new Run(document, "Đến gặp: ")
               {
                   CharacterFormat = new CharacterFormat()
                   {
                       Bold = true,
                       Size = 12
                   }
               }
                , new Run(document, string.Format("{0} ", DenGap))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = 12
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                 , new Run(document, "Về việc: ")
                 {
                     CharacterFormat = new CharacterFormat()
                     {
                         Bold = true,
                         Size = 12
                     }
                 }
                , new Run(document, string.Format("{0} ", VeViec))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = 12
                    }
                }
               , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new Run(document, string.Format("Kính mong Quý Cơ quan tạo điều kiện giúp đỡ."))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = 12,
                        Italic = true
                    }
                }
             )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left,
                        LineSpacing = 1.25
                    }
                };
                section.Blocks.Add(paragraphNoiDung);
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

                rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("Giấy này có giá trị đến ngày {0:dd/MM/yyyy}", DateTime.Now.AddDays(15)))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true,
                        Size = 12
                    }
                }
                    )));

                rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document,
                    new Run(document, string.Format("Hà Nội, ngày {0}, tháng {1}, năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                    {
                        CharacterFormat = new CharacterFormat()
                        {
                            Italic = true,
                            Size = 12
                        }
                    }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new Run(document, string.Format("TL. HIỆU TRƯỞNG"))
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
                section.Blocks.Add(tableCK);
                #endregion
                document.Sections.Add(section);
                if (saveFile)
                {
                    document.Save(this.Response, "gioi_thieu_" + MaSV + "_" + DateTime.Now.ToFileTime() + ".docx");
                }
                else
                {
                    string directoryPath = Server.MapPath("~/NuceDataUpload/Templates/" + "gioi_thieu_" + MaSV + "_" + DateTime.Now.ToFileTime() + ".docx");
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