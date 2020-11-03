using DotNetNuke.Entities.Modules;
using GemBox.Document;
using GemBox.Document.Tables;
using nuce.web.data;
using System;
using System.Data;

namespace nuce.ad.ctsv
{
    public partial class QuanLyDichVu_CapLaiTheSinhVien : PortalModuleBase
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
        private void print(string ID)
        {
            string sql = string.Format(@"select * from [dbo].[AS_Academy_Student_SV_CapLaiTheSinhVien] where ID={0};
select DateOfBirth,NgaySinh, HKTT_SoNha, HKTT_Pho, HKTT_Phuong, HKTT_Quan, HKTT_Tinh
,[LaNoiTru],[DiaChiCuThe],[ClassCode], c.Name as TenKhoa, b.SchoolYear as NienKhoa from [dbo].[AS_Academy_Student] a left join
[dbo].[AS_Academy_Class] b on a.ClassID = b.ID left join [dbo].[AS_Academy_Faculty] c
on b.FacultyID = c.ID where a.ID in (select StudentID from [dbo].[AS_Academy_Student_SV_CapLaiTheSinhVien] where ID = {0}); ", ID);
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
                string LyDoXacNhan = dt1.Rows[0].IsNull("LyDo") ? "" : dt1.Rows[0]["LyDo"].ToString();

                ComponentInfo.SetLicense("DTZX-HTZ5-B7Q6-2GA6");
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
                        Size = 10
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
                        Size = 10
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
                        Size = 10
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
                        Size = 11
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
                new Run(document, string.Format("{0}", "GIẤY XÁC NHẬN"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 13
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new Run(document, "TRƯỜNG ĐẠI HỌC XÂY DỰNG")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new Run(document, "XÁC NHẬN")
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
               new Run(document, string.Format("{0}", "Anh (chị): "))
               {
                   CharacterFormat = new CharacterFormat()
                   {
                       Bold = true,
                       Size = 12
                   }
               }
               , new Run(document, string.Format("{0}", HoVaTen))
               , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
               , new Run(document, "Sinh ngày: ")
               {
                   CharacterFormat = new CharacterFormat()
                   {
                       Bold = true,
                       Size = 12
                   }
               }
                , new Run(document, string.Format("{0} ", NgaySinh.Day))
                 , new Run(document, " tháng: ")
                 {
                     CharacterFormat = new CharacterFormat()
                     {
                         Bold = true,
                         Size = 12
                     }
                 }
                , new Run(document, string.Format("{0} ", NgaySinh.Month))
                 , new Run(document, " năm: ")
                 {
                     CharacterFormat = new CharacterFormat()
                     {
                         Bold = true,
                         Size = 12
                     }
                 }
                , new Run(document, string.Format("{0}", NgaySinh.Year))
               , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
               , new Run(document, "Hộ khẩu thưởng trú: ")
               {
                   CharacterFormat = new CharacterFormat()
                   {
                       Bold = true,
                       Size = 12
                   }
               }
               , new Run(document, string.Format("Số {0}, Phố {1}, Phường (Xã) {2}, Quận (Huyện) {3}, Thành Phố (Tỉnh) {4} ", HKTT_SoNha, HKTT_Pho, HKTT_Phuong, HKTT_Quan, HKTT_Tinh))
                 , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
               , new Run(document, "Địa chỉ tạm trú: ")
               {
                   CharacterFormat = new CharacterFormat()
                   {
                       Bold = true,
                       Size = 12
                   }
               }
               , new Run(document, string.Format("{0}", LaNoiTru ? DiaChiCuThe + ", Ký túc xá Đại học Xây dựng" : DiaChiCuThe))
                 , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                  , new Run(document, "Hiện là sinh viên năm thứ: ")
                  {
                      CharacterFormat = new CharacterFormat()
                      {
                          Bold = true,
                          Size = 12
                      }
                  }
             , new Run(document, string.Format(" {0} ", getNamThu(NienKhoa)))
             , new Run(document, " MASV: ")
             {
                 CharacterFormat = new CharacterFormat()
                 {
                     Bold = true,
                     Size = 12
                 }
             }
             , new Run(document, string.Format(" {0} ", MaSV))
             , new Run(document, " Lớp: ")
             {
                 CharacterFormat = new CharacterFormat()
                 {
                     Bold = true,
                     Size = 12
                 }
             }
             , new Run(document, string.Format(" {0} ", Class))
              , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
               , new Run(document, "Khoa: ")
               {
                   CharacterFormat = new CharacterFormat()
                   {
                       Bold = true,
                       Size = 12
                   }
               }
             , new Run(document, string.Format(" {0} ", TenKhoa))
              , new Run(document, " Niên khóa: ")
              {
                  CharacterFormat = new CharacterFormat()
                  {
                      Bold = true,
                      Size = 12
                  }
              }
             , new Run(document, string.Format(" {0} ", NienKhoa))
              , new Run(document, " Hệ đào tạo: ")
              {
                  CharacterFormat = new CharacterFormat()
                  {
                      Bold = true,
                      Size = 12
                  }
              }
             , new Run(document, string.Format(" {0} ", HeDaoTao))
              , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                 , new Run(document, "Lý do xác nhận: ")
                 {
                     CharacterFormat = new CharacterFormat()
                     {
                         Bold = true,
                         Size = 12
                     }
                 }
             , new Run(document, string.Format(" {0} ", LyDoXacNhan))
               , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                 , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
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

                rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", " ")))));

                rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document,
                    new Run(document, string.Format("Hà Nội, ngày {0}, tháng {1}, năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
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
                document.Save(this.Response, "xac_nhan_" + MaSV + "_" + DateTime.Now.ToFileTime() + ".docx");
            }
        }
        private string getNamThu(string NienKhoa)
        {
            string[] strNamhocs = NienKhoa.Split(new char[] { '-' });
            string nambatdau = strNamhocs[0].Trim();
            int iNamBatDau = int.Parse(nambatdau);
            int iNamHienTai = DateTime.Now.Year;
            if (DateTime.Now.Month > 8)
                iNamHienTai++;
            return (iNamHienTai - iNamBatDau).ToString();
        }
    }
}