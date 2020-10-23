using DotNetNuke.Entities.Modules;
using GemBox.Document;
using GemBox.Document.Tables;
using nuce.web.data;
using System;
using System.Data;

namespace nuce.ad.ctsv
{
    public partial class StudentSearch : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
        private void print(string code)
        {
            //Lay du lieu trong bang sinh vien
            string sql = "select * from AS_Academy_Student where Status<>4 and Code='" + code + "'";
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string Email = dt.Rows[0].IsNull("Email") ? "" : dt.Rows[0]["Email"].ToString();
                string Mobile = dt.Rows[0].IsNull("Mobile") ? "" : dt.Rows[0]["Mobile"].ToString();
                string NgaySinh = dt.Rows[0].IsNull("NgaySinh") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgaySinh"].ToString()).ToString("dd/MM/yyyy");
                string BirthPlace = dt.Rows[0].IsNull("BirthPlace") ? "" : dt.Rows[0]["BirthPlace"].ToString();
                string DanToc = dt.Rows[0].IsNull("DanToc") ? "" : dt.Rows[0]["DanToc"].ToString();
                string TonGiao = dt.Rows[0].IsNull("TonGiao") ? "" : dt.Rows[0]["TonGiao"].ToString();
                string HKTT_SoNha = dt.Rows[0].IsNull("HKTT_SoNha") ? "" : dt.Rows[0]["HKTT_SoNha"].ToString();
                string HKTT_Pho = dt.Rows[0].IsNull("HKTT_Pho") ? "" : dt.Rows[0]["HKTT_Pho"].ToString();
                string HKTT_Phuong = dt.Rows[0].IsNull("HKTT_Phuong") ? "" : dt.Rows[0]["HKTT_Phuong"].ToString();
                string HKTT_Quan = dt.Rows[0].IsNull("HKTT_Quan") ? "" : dt.Rows[0]["HKTT_Quan"].ToString();
                string HKTT_Tinh = dt.Rows[0].IsNull("HKTT_Tinh") ? "" : dt.Rows[0]["HKTT_Tinh"].ToString();
                string CMT = dt.Rows[0].IsNull("CMT") ? "" : dt.Rows[0]["CMT"].ToString();
                string CMT_NoiCap = dt.Rows[0].IsNull("CMT_NoiCap") ? "" : dt.Rows[0]["CMT_NoiCap"].ToString();
                string CMT_NgayCap = dt.Rows[0].IsNull("CMT_NgayCap") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["CMT_NgayCap"].ToString()).ToString("dd/MM/yyyy");
                string NamTotNghiepPTTH = dt.Rows[0].IsNull("NamTotNghiepPTTH") ? "" : dt.Rows[0]["NamTotNghiepPTTH"].ToString();
                string NgayVaoDoan = dt.Rows[0].IsNull("NgayVaoDoan") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgayVaoDoan"].ToString()).ToString("dd/MM/yyyy");
                string NgayVaoDang = dt.Rows[0].IsNull("NgayVaoDang") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgayVaoDang"].ToString()).ToString("dd/MM/yyyy");
                string DiemThiPTTH = dt.Rows[0].IsNull("DiemThiPTTH") ? "" : dt.Rows[0]["DiemThiPTTH"].ToString();
                string KhuVucHKTT = dt.Rows[0].IsNull("KhuVucHKTT") ? "" : dt.Rows[0]["KhuVucHKTT"].ToString();
                string DoiTuongUuTien = dt.Rows[0].IsNull("DoiTuongUuTien") ? "" : dt.Rows[0]["DoiTuongUuTien"].ToString();
                bool DaTungLamCanBoLop = dt.Rows[0].IsNull("DaTungLamCanBoLop") ? false : bool.Parse(dt.Rows[0]["DaTungLamCanBoLop"].ToString());
                bool DaTungLamCanBoDoan = dt.Rows[0].IsNull("DaTungLamCanBoLop") ? false : bool.Parse(dt.Rows[0]["DaTungLamCanBoDoan"].ToString());
                bool DaThamGiaDoiTuyenThiHSG = dt.Rows[0].IsNull("DaThamGiaDoiTuyenThiHSG") ? false : bool.Parse(dt.Rows[0]["DaThamGiaDoiTuyenThiHSG"].ToString());
                string BaoTin_DiaChi = dt.Rows[0].IsNull("BaoTin_DiaChi") ? "" : dt.Rows[0]["BaoTin_DiaChi"].ToString();
                string BaoTin_HoVaTen = dt.Rows[0].IsNull("BaoTin_HoVaTen") ? "" : dt.Rows[0]["BaoTin_HoVaTen"].ToString();
                string BaoTin_DiaChiNguoiNhan = dt.Rows[0].IsNull("BaoTin_DiaChiNguoiNhan") ? "" : dt.Rows[0]["BaoTin_DiaChiNguoiNhan"].ToString();
                string BaoTin_SoDienThoai = dt.Rows[0].IsNull("BaoTin_SoDienThoai") ? "" : dt.Rows[0]["BaoTin_SoDienThoai"].ToString();
                string BaoTin_Email = dt.Rows[0].IsNull("BaoTin_Email") ? "" : dt.Rows[0]["BaoTin_Email"].ToString();
                bool LaNoiTru = dt.Rows[0].IsNull("LaNoiTru") ? false : bool.Parse(dt.Rows[0]["LaNoiTru"].ToString());
                string DiaChiCuThe = dt.Rows[0].IsNull("DiaChiCuThe") ? "" : dt.Rows[0]["DiaChiCuThe"].ToString();

                string File1 = dt.Rows[0].IsNull("File1") ? "" : dt.Rows[0]["File1"].ToString();
                string File2 = dt.Rows[0].IsNull("File2") ? "" : dt.Rows[0]["File2"].ToString();
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
                #region HO SO SINH VIEN
                Paragraph paragraphHoSoSinhVien = new Paragraph(document, new Run(document, string.Format("{0}", "HỒ SƠ SINH VIÊN"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 15
                    }
                }
               )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Center
                    }
                };
                section.Blocks.Add(paragraphHoSoSinhVien);
                #endregion
                #region I. THÔNG TIN CÁ NHÂN	
                Paragraph paragraphThongTinCaNhan = new Paragraph(document, new Run(document, string.Format("{0}", "I. THÔNG TIN CÁ NHÂN"))
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
                        Alignment = GemBox.Document.HorizontalAlignment.Left
                    }
                };
                section.Blocks.Add(paragraphThongTinCaNhan);
                #endregion
                #region Anh
                Table tableAnh = new Table(document);
                tableAnh.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
                tableAnh.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Left;
                var tableBordersAnh = tableAnh.TableFormat.Borders;
                tableBordersAnh.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
                tableAnh.Columns.Add(new TableColumn() { PreferredWidth = 80 });
                tableAnh.Columns.Add(new TableColumn() { PreferredWidth = 20 });
                SpecialCharacter xuongdong = new SpecialCharacter(document, SpecialCharacterType.LineBreak);
                Paragraph paragraphHoVaTen = new Paragraph(document)
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left,
                        LineSpacing = 1.5
                    }
                };
                Run runHoVaTen1 = new Run(document, "Họ và tên (chữ in hoa): ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runHoVaTen2 = new Run(document, dt.Rows[0]["FulName"].ToString().ToUpper());

                Run runMaSV1 = new Run(document, "     Mã số SV: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runMaSV2 = new Run(document, code);
                paragraphHoVaTen.Inlines.Add(runHoVaTen1);
                paragraphHoVaTen.Inlines.Add(runHoVaTen2);
                paragraphHoVaTen.Inlines.Add(runMaSV1);
                paragraphHoVaTen.Inlines.Add(runMaSV2);
                Paragraph paragraphNgayThangNamSinh = new Paragraph(document)
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left,
                        LineSpacing = 1.5
                    }
                };
                Run runNgayThangNamSinh1 = new Run(document, "Ngày/tháng/năm sinh: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runNgayThangNamSinh2 = new Run(document, NgaySinh);
                paragraphNgayThangNamSinh.Inlines.Add(runNgayThangNamSinh1);
                paragraphNgayThangNamSinh.Inlines.Add(runNgayThangNamSinh2);
                Paragraph paragraphNoiSinh = new Paragraph(document)
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left,
                        LineSpacing = 1.5
                    }
                };
                Run runNoiSinh1 = new Run(document, "Nơi sinh: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runNoiSinh2 = new Run(document, BirthPlace);
                paragraphNoiSinh.Inlines.Add(runNoiSinh1);
                paragraphNoiSinh.Inlines.Add(runNoiSinh2);
                Paragraph paragraphDanTocTonGiao = new Paragraph(document)
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left,
                        LineSpacing = 1.5
                    }
                };
                Run runDanToc1 = new Run(document, "Dân tộc: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runDanToc2 = new Run(document, DanToc);
                Run runTonGiao1 = new Run(document, "   Tôn giáo: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runTonGiao2 = new Run(document, TonGiao);
                paragraphDanTocTonGiao.Inlines.Add(runDanToc1);
                paragraphDanTocTonGiao.Inlines.Add(runDanToc2);
                paragraphDanTocTonGiao.Inlines.Add(runTonGiao1);
                paragraphDanTocTonGiao.Inlines.Add(runTonGiao2);
                Paragraph paragraphHKTT1 = new Paragraph(document)
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left,
                        LineSpacing = 1.5
                    }
                };
                Run runHKTTSN11 = new Run(document, "Hộ khẩu TT:")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };

                Run runHKTTSN12 = new Run(document, "- Số nhà: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runHKTTSN2 = new Run(document, HKTT_SoNha);
                Run runHKTTPHO1 = new Run(document, "   Phố/Thôn: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runHKTTPHO2 = new Run(document, HKTT_Pho);
                Run runHKTTPHUONG1 = new Run(document, "   Phường/Xã: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runHKTTPHUONG2 = new Run(document, HKTT_Phuong);

                paragraphHKTT1.Inlines.Add(runHKTTSN11);
                paragraphHKTT1.Inlines.Add(xuongdong);
                paragraphHKTT1.Inlines.Add(runHKTTSN12);
                paragraphHKTT1.Inlines.Add(runHKTTSN2);
                paragraphHKTT1.Inlines.Add(runHKTTPHO1);
                paragraphHKTT1.Inlines.Add(runHKTTPHO2);
                paragraphHKTT1.Inlines.Add(runHKTTPHUONG1);
                paragraphHKTT1.Inlines.Add(runHKTTPHUONG2);

                Paragraph paragraphHKTT2 = new Paragraph(document)
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left,
                        LineSpacing = 1.5
                    }
                };
                Run runHKTTQuan1 = new Run(document, "- Quận/Huyện: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runHKTTQuan2 = new Run(document, HKTT_Quan);
                Run runHKTTTinh1 = new Run(document, "   Thành phố/Tỉnh: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runHKTTTinh2 = new Run(document, HKTT_Tinh);
                paragraphHKTT2.Inlines.Add(runHKTTQuan1);
                paragraphHKTT2.Inlines.Add(runHKTTQuan2);
                paragraphHKTT2.Inlines.Add(runHKTTTinh1);
                paragraphHKTT2.Inlines.Add(runHKTTTinh2);



                Paragraph paragraphAnh = new Paragraph(document)
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Right
                    }
                };
                string directoryPath = Server.MapPath("~/Data/images");
                
                string path = directoryPath + "\\chua_co_anh.png";
                if (File1 != "")
                {
                    string[] avatars = File1.Split(new char[] { '/'});
                    path = directoryPath + "\\"+ avatars[avatars.Length-1];
                }
                Picture picture1 = new Picture(document, path, 150, 200, LengthUnit.Pixel);
                paragraphAnh.Inlines.Add(picture1);
                TableRow rowAnh = new TableRow(document);
                tableAnh.Rows.Add(rowAnh);
                rowAnh.Cells.Add(new TableCell(document, paragraphHoVaTen, paragraphNgayThangNamSinh, paragraphNoiSinh, paragraphDanTocTonGiao,
                    paragraphHKTT1, paragraphHKTT2));
                rowAnh.Cells.Add(new TableCell(document, paragraphAnh));
                section.Blocks.Add(tableAnh);
                #endregion
                #region Cac thong tin khac cua phan 1
                Paragraph paragraphCMT = new Paragraph(document)
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left,
                        LineSpacing = 1.5
                    }
                };
                Run runCMT1 = new Run(document, "Số CMND: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runCMT2 = new Run(document, CMT);
                Run runCMT3 = new Run(document, "   Ngày cấp: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runCMT4 = new Run(document, CMT_NgayCap);


                Run runCMT5 = new Run(document, "   Nơi cấp: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runCMT6 = new Run(document, CMT_NoiCap);
                paragraphCMT.Inlines.Add(runCMT1);
                paragraphCMT.Inlines.Add(runCMT2);
                paragraphCMT.Inlines.Add(runCMT3);
                paragraphCMT.Inlines.Add(runCMT4);
                paragraphCMT.Inlines.Add(runCMT5);
                paragraphCMT.Inlines.Add(runCMT6);
                #region Dang Doan
                Run runDoan1 = new Run(document, "Ngày vào Đoàn: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runDoan2 = new Run(document, NgayVaoDoan);
                Run runDang1 = new Run(document, "   Ngày vào Đảng: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runDang2 = new Run(document, NgayVaoDang);
                paragraphCMT.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphCMT.Inlines.Add(runDoan1);
                paragraphCMT.Inlines.Add(runDoan2);
                paragraphCMT.Inlines.Add(runDang1);
                paragraphCMT.Inlines.Add(runDang2);
                #endregion
                #region Thi THPTQG
                Run runTNTHPTQG1 = new Run(document, "Tốt nghiệp THPT(BTVH) năm: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runTNTHPTQG2 = new Run(document, NamTotNghiepPTTH);
                Run runDiemTHPTQG1 = new Run(document, "   Điểm thi PTTH Quốc gia: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runDiemTHPTQG2 = new Run(document, DiemThiPTTH);
                paragraphCMT.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphCMT.Inlines.Add(runTNTHPTQG1);
                paragraphCMT.Inlines.Add(runTNTHPTQG2);
                paragraphCMT.Inlines.Add(runDiemTHPTQG1);
                paragraphCMT.Inlines.Add(runDiemTHPTQG2);
                #endregion
                #region HKTTKV
                Run runHKTTKV1 = new Run(document, "Hộ khẩu TT thuộc khu vực: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runHKTTKV2 = new Run(document, KhuVucHKTT);
                Run runDoiTuongUuTien1 = new Run(document, "   Đối tượng ưu tiên: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runDoiTuongUuTien2 = new Run(document, DoiTuongUuTien);
                paragraphCMT.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphCMT.Inlines.Add(runHKTTKV1);
                paragraphCMT.Inlines.Add(runHKTTKV2);
                paragraphCMT.Inlines.Add(runDoiTuongUuTien1);
                paragraphCMT.Inlines.Add(runDoiTuongUuTien2);
                Run runQuaTrinhHocTapH1 = new Run(document, "Quá trình học tập ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runQuaTrinhHocTapH2 = new Run(document, "(Ghi rõ thời gian/ trường học PTTH)")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true
                    }
                };
                paragraphCMT.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphCMT.Inlines.Add(runQuaTrinhHocTapH1);
                paragraphCMT.Inlines.Add(runQuaTrinhHocTapH2);
                #endregion
                section.Blocks.Add(paragraphCMT);
                #endregion
                #region Bang qua trinh hoc tap

                sql = "select * from AS_Academy_Student_QuaTrinhHocTap where StudentCode='" + code + "' order by Count";
                DataTable dtQTHT = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                if (dtQTHT.Rows.Count > 0)
                {
                    Table tableQTHT = new Table(document);
                    tableQTHT.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
                    tableQTHT.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Left;
                    //var tableBordersQTHT = tableQTHT.TableFormat.Borders;
                    //tableBordersQTHT.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
                    tableQTHT.Columns.Add(new TableColumn() { PreferredWidth = 35 });
                    tableQTHT.Columns.Add(new TableColumn() { PreferredWidth = 65 });
                    TableRow rowQTHTH = new TableRow(document) { RowFormat = { Height = new TableRowHeight(20, TableRowHeightRule.AtLeast) } };

                    rowQTHTH.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("Thời gian"))
                    {
                        CharacterFormat = new CharacterFormat()
                        {
                            Bold = true
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
                    rowQTHTH.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("Trường"))
                    {
                        CharacterFormat = new CharacterFormat()
                        {
                            Bold = true
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
                    tableQTHT.Rows.Add(rowQTHTH);
                    for (int j = 0; j < dtQTHT.Rows.Count; j++)
                    {
                        TableRow rowQTHT = new TableRow(document) { RowFormat = { Height = new TableRowHeight(20, TableRowHeightRule.AtLeast) } };

                        rowQTHT.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", dtQTHT.Rows[j]["ThoiGian"].ToString()))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                //Bold = true
                            }
                        }
                            )
                        {
                            ParagraphFormat = new ParagraphFormat()
                            {
                                Alignment = GemBox.Document.HorizontalAlignment.Left
                            }
                        })
                        {
                            CellFormat = new TableCellFormat()
                            {
                                VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                            }
                        }
                            );
                        rowQTHT.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", dtQTHT.Rows[j]["TenTruong"].ToString()))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                //Bold = true
                            }
                        }
                            )
                        {
                            ParagraphFormat = new ParagraphFormat()
                            {
                                Alignment = GemBox.Document.HorizontalAlignment.Left
                            }
                        })
                        {
                            CellFormat = new TableCellFormat()
                            {
                                VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                            }
                        }
                            );
                        tableQTHT.Rows.Add(rowQTHT);
                    }
                    section.Blocks.Add(tableQTHT);
                }
                //
                #endregion
                #region Thong tin cuoi phan 1
                Paragraph paragraphThanhTich = new Paragraph(document)
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left,
                        LineSpacing = 1.5
                    }
                };
                Run runDaTungLamCanBoLop1 = new Run(document, "Đã từng làm cán bộ lớp: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runDaTungLamCanBoLop2 = new Run(document, DaTungLamCanBoLop ? "Có" : "Không");
                Run runDaTungLamCanBoDoan1 = new Run(document, "Đã từng làm cán bộ đoàn: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runDaTungLamCanBoDoan2 = new Run(document, DaTungLamCanBoDoan ? "Có" : "Không");
                Run runDaTungThamGiaThiHSG1 = new Run(document, "Đã tham gia đội tuyển thi HSG: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true
                    }
                };
                Run runDaTungThamGiaThiHSG2 = new Run(document, DaThamGiaDoiTuyenThiHSG ? "Có" : "Không");
                paragraphThanhTich.Inlines.Add(runDaTungLamCanBoLop1);
                paragraphThanhTich.Inlines.Add(runDaTungLamCanBoLop2);
                paragraphThanhTich.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphThanhTich.Inlines.Add(runDaTungLamCanBoDoan1);
                paragraphThanhTich.Inlines.Add(runDaTungLamCanBoDoan2);
                paragraphThanhTich.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphThanhTich.Inlines.Add(runDaTungThamGiaThiHSG1);
                paragraphThanhTich.Inlines.Add(runDaTungThamGiaThiHSG2);
                section.Blocks.Add(paragraphThanhTich);
                if (DaThamGiaDoiTuyenThiHSG)
                {
                    #region Bang Thi Hoc Sinh Gioi

                    sql = "select * from AS_Academy_Student_ThiHSG where StudentCode='" + code + "' order by Count";
                    DataTable dtTHSG = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                    if (dtTHSG.Rows.Count > 0)
                    {
                        Table tableTHSG = new Table(document);
                        tableTHSG.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
                        tableTHSG.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Left;
                        //var tableBordersQTHT = tableQTHT.TableFormat.Borders;
                        //tableBordersQTHT.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
                        tableTHSG.Columns.Add(new TableColumn() { PreferredWidth = 30 });
                        tableTHSG.Columns.Add(new TableColumn() { PreferredWidth = 30 });
                        tableTHSG.Columns.Add(new TableColumn() { PreferredWidth = 40 });
                        TableRow rowTHSGH = new TableRow(document) { RowFormat = { Height = new TableRowHeight(20, TableRowHeightRule.AtLeast) } };

                        rowTHSGH.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("Cấp thi"))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                Bold = true
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
                        rowTHSGH.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("Môn thi"))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                Bold = true
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
                        rowTHSGH.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("Đạt giải"))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                Bold = true
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
                        tableTHSG.Rows.Add(rowTHSGH);
                        for (int j = 0; j < dtTHSG.Rows.Count; j++)
                        {
                            TableRow rowTHSG = new TableRow(document) { RowFormat = { Height = new TableRowHeight(20, TableRowHeightRule.AtLeast) } };

                            rowTHSG.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", dtTHSG.Rows[j]["CapThi"].ToString()))
                            {
                                CharacterFormat = new CharacterFormat()
                                {
                                    //Bold = true
                                }
                            }
                                )
                            {
                                ParagraphFormat = new ParagraphFormat()
                                {
                                    Alignment = GemBox.Document.HorizontalAlignment.Left
                                }
                            })
                            {
                                CellFormat = new TableCellFormat()
                                {
                                    VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                                }
                            }
                                );
                            rowTHSG.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", dtTHSG.Rows[j]["MonThi"].ToString()))
                            {
                                CharacterFormat = new CharacterFormat()
                                {
                                    //Bold = true
                                }
                            }
                                )
                            {
                                ParagraphFormat = new ParagraphFormat()
                                {
                                    Alignment = GemBox.Document.HorizontalAlignment.Left
                                }
                            })
                            {
                                CellFormat = new TableCellFormat()
                                {
                                    VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                                }
                            }
                                );
                            rowTHSG.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", dtTHSG.Rows[j]["DatGiai"].ToString()))
                            {
                                CharacterFormat = new CharacterFormat()
                                {
                                    //Bold = true
                                }
                            }
                               )
                            {
                                ParagraphFormat = new ParagraphFormat()
                                {
                                    Alignment = GemBox.Document.HorizontalAlignment.Left
                                }
                            })
                            {
                                CellFormat = new TableCellFormat()
                                {
                                    VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                                }
                            }
                               );
                            tableTHSG.Rows.Add(rowTHSG);
                        }
                        section.Blocks.Add(tableTHSG);
                    }
                    //
                    #endregion
                }
                #endregion
                #region THÔNG TIN GIA ĐÌNH
                Paragraph paragraphThongTinGiaDinh = new Paragraph(document)
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Left
                    }
                };

                paragraphThongTinGiaDinh.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", "II. THÔNG TIN GIA ĐÌNH"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                }
              );

                #region Thong tin gia dinh
                #region Bang Thi Hoc Sinh Gioi

                sql = "select * from AS_Academy_Student_GiaDinh where StudentCode='" + code + "' order by Count";
                DataTable dtGD = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                if (dtGD.Rows.Count > 0)
                {
                    for (int j = 0; j < dtGD.Rows.Count; j++)
                    {
                        paragraphThongTinGiaDinh.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("+ Họ tên {0}: ", dtGD.Rows[j]["MoiQuanHe"].ToString()))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                Bold = true,
                                Size = 12
                            }
                        });
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", dtGD.Rows[j]["HoVaTen"].ToString())));
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("   Năm sinh: "))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                Bold = true,
                                Size = 12
                            }
                        });
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", dtGD.Rows[j]["NamSinh"].ToString())));
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("   Quốc tịch: "))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                Bold = true,
                                Size = 12
                            }
                        });
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", dtGD.Rows[j]["QuocTich"].ToString())));
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("   Dân tộc: "))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                Bold = true,
                                Size = 12
                            }
                        });
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", dtGD.Rows[j]["DanToc"].ToString())));
                        //paragraphThongTinGiaDinh.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("   Tôn giáo: "))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                Bold = true,
                                Size = 12
                            }
                        });
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", dtGD.Rows[j]["TonGiao"].ToString())));
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("   Nghề nghiệp, chức vụ, nơi công tác: "))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                Bold = true,
                                Size = 12
                            }
                        });
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0} - {1} - {2}", dtGD.Rows[j]["NgheNghiep"].ToString(), dtGD.Rows[j]["ChucVu"].ToString(), dtGD.Rows[j]["NoiCongTac"].ToString())));
                        //paragraphThongTinGiaDinh.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("   Nơi ở hiện nay: "))
                        {
                            CharacterFormat = new CharacterFormat()
                            {
                                Bold = true,
                                Size = 12
                            }
                        });
                        paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", dtGD.Rows[j]["NoiOHienNay"].ToString())));
                    }
                }
                paragraphThongTinGiaDinh.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("Địa chỉ báo tin cho Bố, Mẹ hoặc Người nuôi dưỡng khi cần: "))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Italic=true,
                        Size = 12
                    }
                });
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", BaoTin_DiaChi)));
                paragraphThongTinGiaDinh.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("+ Họ tên người nhận: "))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                });
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", BaoTin_HoVaTen)));
                paragraphThongTinGiaDinh.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("+ Địa chỉ người nhận: "))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                });
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", BaoTin_DiaChiNguoiNhan)));
                paragraphThongTinGiaDinh.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("+ Số điện thoại người nhận: "))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                });
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", BaoTin_SoDienThoai)));
                paragraphThongTinGiaDinh.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("+ SĐT sinh viên: "))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                });
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", Mobile)));
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("   Email: "))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                });
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", Email)));
                paragraphThongTinGiaDinh.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("+ Có ở nội trú không: "))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                });
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}", LaNoiTru?"Có":"Không")));
                paragraphThongTinGiaDinh.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, LaNoiTru?"Địa chỉ nội trú cụ thể":"Địa chỉ bên ngoài cụ thể")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                });
                paragraphThongTinGiaDinh.Inlines.Add(new Run(document, string.Format("{0}",DiaChiCuThe)));
                section.Blocks.Add(paragraphThongTinGiaDinh);
                //
                #endregion
                #endregion
                #endregion
                document.Sections.Add(section);
                document.Save(this.Response, "hoso_" + code + ".docx");
            }
        }
    }
}