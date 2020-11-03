using DotNetNuke.Entities.Modules;
using GemBox.Document;
using GemBox.Document.Tables;
using nuce.web.data;
using System;
using System.Data;

namespace nuce.ad.ctsv
{
    public partial class QuanLyDichVu_MuonHocBaGoc : PortalModuleBase
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
            string sql = string.Format(@"select * from [dbo].[AS_Academy_Student_SV_MuonHocBaGoc] where ID={0};
select DateOfBirth,NgaySinh, HKTT_SoNha, HKTT_Pho, HKTT_Phuong, HKTT_Quan, HKTT_Tinh
,[LaNoiTru],[DiaChiCuThe],[ClassCode], c.Name as TenKhoa, b.SchoolYear as NienKhoa from[dbo].[AS_Academy_Student] a left join
[dbo].[AS_Academy_Class] b on a.ClassID = b.ID left join[dbo].[AS_Academy_Faculty] c
on b.FacultyID = c.ID where a.ID in (select StudentID from[dbo].[AS_Academy_Student_SV_MuonHocBaGoc] where ID = {0}); ", ID);
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql);
            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            if (dt1.Rows.Count > 0 && dt2.Rows.Count > 0)
            {
                DateTime NgaySinh = dt2.Rows[0].IsNull("NgaySinh") ? DateTime.Parse("1/1/1900") : DateTime.Parse(dt2.Rows[0]["NgaySinh"].ToString());
                DateTime NgayTra = dt1.Rows[0].IsNull("NgayTra") ? DateTime.Parse("1/1/1900") : DateTime.Parse(dt1.Rows[0]["NgayTra"].ToString());

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
                //string directoryPath = Server.MapPath("~/NuceDataUpload/Templates/Don-xin-muon-hoc-ba-goc.docx");
                //var document = DocumentModel.Load(directoryPath);
                DocumentModel document = new DocumentModel();
                document.DefaultCharacterFormat.Size = 13;
                document.DefaultCharacterFormat.FontName = "Times New Roman";
                Section section;
                section = new Section(document);
                #region Cộng hòa xã hội chủ nghĩa việt nam
                Paragraph paragraphHeader = new Paragraph(document,
                    new Run(document, "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                    {
                        CharacterFormat = new CharacterFormat()
                        {
                            Size = 12,
                            Bold = true,
                        },
                    },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new Run(document, "Độc lập - Tự do - Hạnh phúc")
                    {
                        CharacterFormat = new CharacterFormat()
                        {
                            UnderlineStyle = UnderlineType.Single,
                            Bold = true,
                        }
                    },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new Run(document, "ĐƠN ĐỀ NGHỊ MƯỢN HỌC BẠ GỐC")
                    {
                        CharacterFormat = new CharacterFormat()
                        {
                            Bold = true,
                            Size = 14,
                        }
                    },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new Run(document, "Kính gửi: Phòng Công tác chính trị và Quản lý sinh viên."),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = HorizontalAlignment.Center,
                        SpaceAfter = 0
                    }
                };
                section.Blocks.Add(paragraphHeader);
                #endregion
                #region NoiDung
                Paragraph paragraphNoiDung = new Paragraph(document,
                   new Run(document, "Tên em là: "),
                   new Run(document, HoVaTen) { CharacterFormat = new CharacterFormat { Bold = true } },
                   new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                   new Run(document, "Ngày sinh: "),
                   new Run(document, NgaySinh.ToString("dd/MM/yyyy")) { CharacterFormat = new CharacterFormat { Bold = true } },
                   new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                   new Run(document, "Lớp: "),
                   new Run(document, Class) { CharacterFormat = new CharacterFormat { Bold = true } },
                   new SpecialCharacter(document, SpecialCharacterType.Tab),
                   new SpecialCharacter(document, SpecialCharacterType.Tab),
                   new Run(document, "Khoá: "),
                   new Run(document, NienKhoa) { CharacterFormat = new CharacterFormat { Bold = true } },
                   new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                   new Run(document, "Khoa/Ban: "),
                   new Run(document, TenKhoa) { CharacterFormat = new CharacterFormat { Bold = true } },
                   new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                   new Run(document, "Lý do mượn hồ sơ: "),
                   new Run(document, LyDoXacNhan) { CharacterFormat = new CharacterFormat { Bold = true } },
                   new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                   new Run(document, "Ngày trả: "),
                   new Run(document, NgayTra.ToString("dd/MM/yyyy")) { CharacterFormat = new CharacterFormat { Bold = true } }
                )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = HorizontalAlignment.Left,
                        LineSpacing = 1.25,
                        SpaceAfter = 0
                    }
                };
                section.Blocks.Add(paragraphNoiDung);
                #endregion
                #region Cảm ơn
                Paragraph paragraphDeNghi = new Paragraph(document,
                    new Run(document, "Em làm đơn này kính mong Quý Phòng xem xét và đồng ý cho em được mượn học bạ gốc.")
                )
                {
                    ParagraphFormat = new ParagraphFormat
                    {
                        Alignment = HorizontalAlignment.Justify,
                        SpaceAfter = 0
                    }
                };
                section.Blocks.Add(paragraphDeNghi);
                Paragraph paragraphCamOn = new Paragraph(document,
                    new Run(document, "Em xin chân thành cảm ơn.")
                    {
                        CharacterFormat = new CharacterFormat
                        {
                            Italic = true,
                        }
                    },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        SpaceAfter = 0,
                    }
                };
                section.Blocks.Add(paragraphCamOn);
                #endregion
                #region Chu ky
                Table tableCK = new Table(document)
                {
                    TableFormat = new TableFormat()
                    {
                        PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage),
                        Alignment = HorizontalAlignment.Center,
                        AutomaticallyResizeToFitContents = false,
                    }
                };
                var tableBordersCK = tableCK.TableFormat.Borders;
                tableBordersCK.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
                tableCK.Columns.Add(new TableColumn(55));
                tableCK.Columns.Add(new TableColumn(45));
                TableRow rowT1CK = new TableRow(document);
                tableCK.Rows.Add(rowT1CK);

                rowT1CK.Cells.Add(
                    new TableCell(document, new Paragraph(
                        document,
                        new Run(document, "Nhận Đơn ngày   /  /    ") { CharacterFormat = new CharacterFormat { Size = 12 } },
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        new Run(document, "CÁN BỘ NHẬN ĐƠN") { CharacterFormat = new CharacterFormat { Bold = true, Size = 12 } },
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        new Run(document, "(Ký, ghi rõ họ tên)") { CharacterFormat = new CharacterFormat { Italic = true, Size = 12 } },
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                        new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                    )
                    {
                        ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Center }
                    })
                );

                rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document,
                    new Run(document, string.Format("Hà Nội, ngày {0}, tháng {1}, năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                    {
                        CharacterFormat = new CharacterFormat
                        {
                            Italic = true,
                            Size = 12,
                        }
                    }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new Run(document, string.Format("SINH VIÊN"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 12
                    }
                },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, string.Format("(Ký, ghi rõ họ tên)"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true,
                        Size = 12
                    }
                }
               )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Center,
                    }
                })
               );

                TableRow rowT2CK = new TableRow(document);
                tableCK.Rows.Add(rowT2CK);
                rowT2CK.Cells.Add(new TableCell(document, new Paragraph(
                    document,
                    new Run(document, "PHÊ DUYỆT CỦA")
                    {
                        CharacterFormat = new CharacterFormat { Size = 12, Bold = true },
                    },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new Run(document, "PHÒNG CTCT & QLSV")
                    {
                        CharacterFormat = new CharacterFormat { Size = 12, Bold = true },
                    },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                )
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Alignment = GemBox.Document.HorizontalAlignment.Center,
                    }
                }));
                section.Blocks.Add(tableCK);
                #endregion
                #region ghi chú
                Paragraph paragraphGhiChu = new Paragraph(document,
                    new Run(document, "Ghi chú: Khi nộp đơn mượn hồ sơ, sinh viên cần phải nộp kèm theo sổ Hộ khẩu gốc.")
                    {
                        CharacterFormat = new CharacterFormat
                        {
                            Italic = true,
                            Size = 12
                        }
                    });
                section.Blocks.Add(paragraphGhiChu);
                #endregion
                document.Sections.Add(section);
                document.Save(this.Response, "muon_hoc_ba_goc_" + MaSV + "_" + DateTime.Now.ToFileTime() + ".docx");
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