using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Survey.Base;

using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Graduate;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements.Graduate
{
    public class AsEduSurveyGraduateReportTotalService : ThongKeServiceBase
    {
        private readonly ILogger<AsEduSurveyGraduateReportTotalService> _logger;
        private readonly SurveyContext _context;
        private readonly EduDataContext _eduContext;

        public AsEduSurveyGraduateReportTotalService(ILogger<AsEduSurveyGraduateReportTotalService> logger, SurveyContext context, EduDataContext eduContext)
        {
            _logger = logger;
            _context = context;
            _eduContext = eduContext;
        }

        public async Task<byte[]> ExportReportTotalGraduateSurvey(Guid surveyRoundId)
        {
            var surveyRound = await _context.AsEduSurveyGraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            //đợt khảo sát chưa kết thúc
            //if (surveyRound.Status != (int)SurveyRoundStatus.Closed && surveyRound.Status != (int)SurveyRoundStatus.End)
            //{
            //    throw new HandleException.InvalidInputDataException("Đợt khảo sát chưa đóng hoặc chưa kết thúc");
            //}

            var theSurvey = await _context.AsEduSurveyGraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.DotKhaoSatId == surveyRound.Id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            _logger.LogInformation("export report Graduate is start.");

            var noiDungDe = JsonSerializer.Deserialize<List<QuestionJson>>(theSurvey.NoiDungDeThi);

            ExcelPackage excel = new ExcelPackage();
            var worksheet = excel.Workbook.Worksheets.Add("Báo cáo tình hình việc làm");

            #region style
            worksheet.DefaultRowHeight = 14.25;
            worksheet.DefaultColWidth = 12;

            worksheet.Cells.Style.WrapText = true;
            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //worksheet.Cells.Style.Font.Name = "Arial";
            worksheet.Cells.Style.Font.Size = 11;

            worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Row(1).Height = 60;

            worksheet.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(2).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Row(2).Height = 60;

            worksheet.Column(1).Width = 7.29;
            worksheet.Column(2).Width = 15.57;
            worksheet.Column(3).Width = 30;
            worksheet.Column(4).Width = 30;
            worksheet.Column(5).Width = 14.71;
            worksheet.Column(6).Width = 18.57;
            worksheet.Column(7).Width = 7.86;
            worksheet.Column(8).Width = 19.86;
            worksheet.Column(9).Width = 30;
            worksheet.Column(10).Width = 11;

            worksheet.Cells["A1"].Value = "TT"; worksheet.Cells["A1:A2"].Merge = true;
            worksheet.Cells["B1"].Value = "Mã ngành"; worksheet.Cells["B1:B2"].Merge = true;
            worksheet.Cells["C1"].Value = "Tên ngành đào tạo"; worksheet.Cells["C1:C2"].Merge = true;
            worksheet.Cells["D1"].Value = "Tên chuyên ngành đào tạo"; worksheet.Cells["D1:D2"].Merge = true;
            worksheet.Cells["E1"].Value = "Mã sinh viên"; worksheet.Cells["E1:E2"].Merge = true;
            worksheet.Cells["F1"].Value = "Họ và tên"; worksheet.Cells["F1:F2"].Merge = true;
            worksheet.Cells["G1"].Value = "Giới tính"; worksheet.Cells["G1:G2"].Merge = true;
            worksheet.Cells["H1"].Value = "Thông tin liên hệ"; worksheet.Cells["H1:I1"].Merge = true;
            worksheet.Cells["H2"].Value = "Điện thoại"; 
            worksheet.Cells["I2"].Value = "Email";

            worksheet.Cells["J1"].Value = "Hình thức khảo sát"; worksheet.Cells["J1:J2"].Merge = true;
            #endregion

            int row = 3;
            int rowCauHoi = 1;
            int rowDapAn = 2;
            int col = 1;

            //sinh viên trong đợt
            var students = _context.AsEduSurveyGraduateStudent.Where(o => o.DotKhaoSatId == surveyRoundId);

            //join bài làm được xét, => loại sinh viên không có bài được xét
            var sinhVienBaiLam = students
                .GroupJoin(_context.AsEduSurveyGraduateBaiKhaoSatSinhVien.Where(o => o.BaiKhaoSatId == theSurvey.Id), o => o.ExMasv, o => o.StudentCode, (sv, bailam) => new { sv, bailam })
                .SelectMany(svBaiLam => svBaiLam.bailam.DefaultIfEmpty(), (svBaiLam, bailam) => new { svBaiLam.sv, bailam });

            //thống kê theo đợt và bài ks
            int tt = 1;
            foreach (var svBaiLam in sinhVienBaiLam)
            {
                col = 1;
                worksheet.Cells[row, col++].Value = tt++;
                worksheet.Cells[row, col++].Value = "";
                worksheet.Cells[row, col++].Value = svBaiLam.sv.Tennganh;
                worksheet.Cells[row, col++].Value = svBaiLam.sv.Tenchnga;
                worksheet.Cells[row, col++].Value = svBaiLam.sv.ExMasv;
                worksheet.Cells[row, col++].Value = svBaiLam.sv.Tensinhvien;
                worksheet.Cells[row, col++].Value = svBaiLam.sv.Gioitinh;
                worksheet.Cells[row, col++].Value = svBaiLam.sv.Mobile;
                worksheet.Cells[row, col++].Value = svBaiLam.sv.Email;
                worksheet.Cells[row, col++].Value = svBaiLam.bailam != null ? svBaiLam.bailam.LoaiHinh : "";

                List<SelectedAnswer> bailam = new List<SelectedAnswer>();
                if(svBaiLam.bailam != null)
                {
                    try
                    {
                        bailam = JsonSerializer.Deserialize<List<SelectedAnswer>>(svBaiLam.bailam.BaiLam);
                    }
                    catch
                    {
                        bailam = new List<SelectedAnswer>();
                    }
                }

                int index = 0;
                foreach (var cauhoi in noiDungDe)
                {
                    if (cauhoi.Type == QuestionType.SC)
                    {
                        var colStart = col;
                        worksheet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoi.Content}";
                        foreach (var dapan in cauhoi.Answers)
                        {
                            worksheet.Cells[rowDapAn, col].Value = dapan.Content;
                            if(bailam.FirstOrDefault(o => o.QuestionCode == cauhoi.Code && o.AnswerCode == dapan.Code) != null)
                            {
                                worksheet.Cells[row, col].Value = "x";
                                //bỏ những câu không cần thiết
                                if (dapan.HideQuestion != null && dapan.HideQuestion.Count > 0)
                                {
                                    dapan.HideQuestion.ForEach(hideCode =>
                                    {
                                        var rmQ = bailam.FirstOrDefault(o => o.QuestionCode == hideCode);
                                        if (rmQ != null)
                                        {
                                            bailam.Remove(rmQ);
                                        }
                                    });
                                }
                            }
                            col++;
                        }
                        var colEnd = col - 1;
                        worksheet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                    }
                    else if (cauhoi.Type == QuestionType.MC)
                    {
                        var colStart = col;
                        worksheet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoi.Content}";
                        foreach (var dapan in cauhoi.Answers)
                        {
                            worksheet.Cells[rowDapAn, col].Value = dapan.Content;
                            if (bailam.FirstOrDefault(o => o.QuestionCode == cauhoi.Code && o.AnswerCodes.Contains(dapan.Code)) != null)
                            {
                                worksheet.Cells[row, col].Value = "x";
                            }

                            //câu hỏi con của đáp án
                            if (dapan.AnswerChildQuestion != null)
                            {
                                var test = $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}";
                                col++;
                                worksheet.Cells[rowDapAn, col].Value = dapan.AnswerChildQuestion.Content;
                                var selected = bailam.FirstOrDefault(o => o.QuestionCode == dapan.AnswerChildQuestion.Code);
                                if (selected != null)
                                {
                                    worksheet.Cells[row, col].Value = selected.AnswerContent;
                                }
                            }
                            col++;
                        }
                        var colEnd = col - 1;
                        worksheet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                    }
                    else if (cauhoi.Type == QuestionType.SA)
                    {
                        worksheet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoi.Content}";
                        worksheet.Column(col).Width = 48;
                        var selected = bailam.FirstOrDefault(o => o.QuestionCode == cauhoi.Code);
                        if (selected != null)
                        {
                            worksheet.Cells[row, col].Value = selected.AnswerContent;
                        }
                        col++;
                    }
                    else if (cauhoi.Type == QuestionType.CityC)
                    {
                        worksheet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoi.Content}";
                        worksheet.Column(col).Width = 25;
                        var selected = bailam.FirstOrDefault(o => o.QuestionCode == cauhoi.Code);
                        if (selected != null)
                        {
                            worksheet.Cells[row, col].Value = selected.AnswerContent;
                        }
                        col++;
                    }
                    else if (cauhoi.Type == QuestionType.GQ)
                    {
                        int indexChild = 0;
                        index++;
                        foreach (var childQuestion in cauhoi.ChildQuestion)
                        {
                            if (childQuestion.Type == QuestionType.SC)
                            {
                                var colStart = col;
                                worksheet.Cells[rowCauHoi, col].Value = $"Câu {index}.{++indexChild}: {childQuestion.Content}";
                                foreach (var dapan in childQuestion.Answers)
                                {
                                    worksheet.Cells[rowDapAn, col].Value = dapan.Content;
                                    if (bailam.FirstOrDefault(o => o.QuestionCode == childQuestion.Code && o.AnswerCode == dapan.Code) != null)
                                    {
                                        worksheet.Cells[row, col].Value = "x";
                                    }
                                    col++;
                                }
                                var colEnd = col - 1;
                                worksheet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                            }
                            else if (childQuestion.Type == QuestionType.MC)
                            {
                                var colStart = col;
                                worksheet.Cells[rowCauHoi, col].Value = $"Câu {index}.{++indexChild}: {childQuestion.Content}";
                                worksheet.Cells[rowCauHoi, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[rowCauHoi, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                                foreach (var dapan in childQuestion.Answers)
                                {
                                    worksheet.Cells[rowDapAn, col].Value = dapan.Content;
                                    if (bailam.FirstOrDefault(o => o.QuestionCode == childQuestion.Code && o.AnswerCodes.Contains(dapan.Code)) != null)
                                    {
                                        worksheet.Cells[row, col].Value = "x";
                                    }

                                    //câu hỏi con của đáp án
                                    if (dapan.AnswerChildQuestion != null)
                                    {
                                        var test = $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}";
                                        col++;
                                        worksheet.Cells[rowDapAn, col].Value = dapan.AnswerChildQuestion.Content;
                                        var selected = bailam.FirstOrDefault(o => o.QuestionCode == dapan.AnswerChildQuestion.Code);
                                        if (selected != null)
                                        {
                                            worksheet.Cells[row, col].Value = selected.AnswerContent;
                                        }
                                    }
                                    col++;
                                }
                                var colEnd = col - 1;
                                worksheet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                            }
                            else if (childQuestion.Type == QuestionType.SA)
                            {
                                worksheet.Cells[rowCauHoi, col].Value = $"Câu {index}.{++indexChild}: {childQuestion.Content}";
                                worksheet.Column(col).Width = 48;
                                var selected = bailam.FirstOrDefault(o => o.QuestionCode == childQuestion.Code);
                                if (selected != null)
                                {
                                    worksheet.Cells[row, col].Value = selected.AnswerContent;
                                }
                                col++;
                            }
                        }
                    }
                }

                worksheet.Cells[rowCauHoi, col].Value = "CCCD/CMND";
                worksheet.Column(col).Width = 20;
                worksheet.Cells[row, col++].Value = svBaiLam.sv.Cmnd;

                worksheet.Cells[rowCauHoi, col].Value = "Số quyết định tốt nghiệp";
                worksheet.Column(col).Width = 44.57;
                worksheet.Cells[row, col++].Value = svBaiLam.sv.Soqdtn;

                worksheet.Cells[rowCauHoi, col].Value = "ngày ra quyết định tốt nghiệp";
                worksheet.Column(col).Style.Numberformat.Format = "dd-MM-yy";
                worksheet.Column(col).Width = 44.57;
                worksheet.Cells[row, col++].Value = svBaiLam.sv.Ngayraqd?.ToString("dd-MM-yyyy");

                worksheet.Cells[rowCauHoi, col].Value = "xếp loại tốt nghiệp";
                worksheet.Column(col).Width = 12;
                worksheet.Cells[row, col++].Value = svBaiLam.sv.Xeploai;

                worksheet.Cells[rowCauHoi, col].Value = "Lớp quản lý";
                worksheet.Column(col).Width = 12;
                worksheet.Cells[row, col++].Value = svBaiLam.sv.Lopqd;

                row++;
            }
            _logger.LogInformation("export report Graduate is done.");
            using MemoryStream ms = new MemoryStream();
            excel.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<List<TempDataModel>> TempDataGraduateSurvey(Guid surveyRoundId)
        {
            var surveyRound = await _context.AsEduSurveyGraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            var theSurvey = await _context.AsEduSurveyGraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.DotKhaoSatId == surveyRoundId && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            var result = new List<TempDataModel>();
            var students = _context.AsEduSurveyGraduateStudent.Where(o => o.DotKhaoSatId == surveyRoundId);

            #region thống kê theo chuyên ngành
            //var groupChuyenNganh = students.Select(o => o.Tenchnga ?? o.Tennganh).Distinct().ToList();

            ////join bài làm được xét, => loại sinh viên không có bài được xét
            //var sinhVienBaiLam = students.Join(_context.AsEduSurveyGraduateBaiKhaoSatSinhVien.Where(o => o.BaiKhaoSatId == theSurvey.Id), o => o.ExMasv, o => o.StudentCode, (sv, bailam) => new { sv, bailam });
            ////thống kê theo đợt và bài ks
            //foreach (var chuyenNganh in groupChuyenNganh)
            //{
            //    _logger.LogInformation($"thong ke tam {chuyenNganh}");
            //    var bailamsv = sinhVienBaiLam.Where(o => o.sv.Tenchnga == chuyenNganh || (o.sv.Tenchnga == null && o.sv.Tennganh == chuyenNganh));
            //    var soPhieuPhatRa = bailamsv.Count();
            //    var soPhieuThuVe = bailamsv.Where(o => o.bailam.Status == (int)SurveyStudentStatus.Done).Count();

            //    var tyLeThamGia = "0%";
            //    if (soPhieuThuVe > 0)
            //    {
            //        tyLeThamGia = $"{(double)soPhieuThuVe / soPhieuPhatRa * 100:0}%";
            //    }

            //    result.Add(new TempDataModel
            //    {
            //        ChuyenNganh = chuyenNganh,
            //        SoPhieuPhatRa = soPhieuPhatRa,
            //        SoPhieuThuVe = soPhieuThuVe,
            //        TyLeThamGia = tyLeThamGia
            //    });
            //}
            #endregion

            #region thống kê theo khoa
            var maKhoas = students.Select(o => o.Makhoa).Distinct().ToList();
            //join bài làm được xét, => loại sinh viên không có bài được xét
            var sinhVienBaiLam = students.Join(_context.AsEduSurveyGraduateBaiKhaoSatSinhVien.Where(o => o.BaiKhaoSatId == theSurvey.Id), o => o.ExMasv, o => o.StudentCode, (sv, bailam) => new { sv, bailam });
            foreach (var maKhoa in maKhoas)
            {
                var khoa = _eduContext.AsAcademyFaculty.FirstOrDefault(o => o.Code == maKhoa);

                _logger.LogInformation($"thong ke tam {maKhoa}");
                var bailamsv = sinhVienBaiLam.Where(o => o.sv.Makhoa == maKhoa);
                var soPhieuPhatRa = bailamsv.Count();
                var soPhieuThuVe = bailamsv.Where(o => o.bailam.Status == (int)SurveyStudentStatus.Done).Count();

                var tyLeThamGia = "0%";
                if (soPhieuThuVe > 0)
                {
                    tyLeThamGia = $"{(double)soPhieuThuVe / soPhieuPhatRa * 100:0}%";
                }

                result.Add(new TempDataModel
                {
                    MaKhoa = maKhoa,
                    TenKhoa = khoa?.Name,
                    SoPhieuPhatRa = soPhieuPhatRa,
                    SoPhieuThuVe = soPhieuThuVe,
                    TyLeThamGia = tyLeThamGia
                });
            }
            #endregion
            _logger.LogInformation($"thong ke so luong hoan thanh");
            return result;
        }
    }
}
