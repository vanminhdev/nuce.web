using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Survey.Base;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Undergraduate.Statistic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements.Undergraduate
{
    public class AsEduSurveyUndergraduateReportTotalService : ThongKeServiceBase, IAsEduSurveyUndergraduateReportTotalService
    {
        private readonly ILogger<AsEduSurveyUndergraduateReportTotalService> _logger;
        private readonly SurveyContext _context;
        private readonly EduDataContext _eduContext;

        public AsEduSurveyUndergraduateReportTotalService(ILogger<AsEduSurveyUndergraduateReportTotalService> logger, SurveyContext context, EduDataContext eduContext)
        {
            _logger = logger;
            _context = context;
            _eduContext = eduContext;
        }

        public async Task<byte[]> ExportReportTotalUndergraduateSurvey(Guid surveyRoundId, Guid theSurveyId)
        {
            var surveyRound = await _context.AsEduSurveyUndergraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            //đợt khảo sát chưa kết thúc
            if (surveyRound.Status != (int)SurveyRoundStatus.Closed && surveyRound.Status != (int)SurveyRoundStatus.End)
            {
                throw new HandleException.InvalidInputDataException("Đợt khảo sát chưa đóng hoặc chưa kết thúc");
            }

            var theSurvey = await _context.AsEduSurveyUndergraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurveyId && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            _logger.LogInformation("export report undergraduate is start.");

            var noiDungDe = JsonSerializer.Deserialize<List<QuestionJson>>(theSurvey.NoiDungDeThi);

            ExcelPackage excel = new ExcelPackage();
            var worksheet = excel.Workbook.Worksheets.Add("Báo cáo tổng thể toàn trường");

            #region style
            worksheet.DefaultRowHeight = 14.25;
            worksheet.DefaultColWidth = 8.5;

            worksheet.Cells.Style.WrapText = true;
            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //worksheet.Cells.Style.Font.Name = "Arial";
            worksheet.Cells.Style.Font.Size = 11;

            worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Row(2).Height = 37.5;
            worksheet.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(2).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Column(1).Width = 44.86;

            worksheet.Cells["A1"].Value = "Ngành/ chuyên ngành";
            worksheet.Cells["B1"].Value = "Số phiếu phát ra";
            worksheet.Cells["C1"].Value = "Thu về";
            worksheet.Cells["D1"].Value = "Tỷ lệ tham gia khảo sát";
            worksheet.Cells["E1"].Value = "Tỷ lệ kỳ vọng";
            #endregion

            int row = 3;
            int rowCauHoi = 1;
            int rowDapAn = 2;
            int col = 1;

            var tongToanTruong = new Dictionary<int, float>();
            //sinh viên trong đợt
            var students = _context.AsEduSurveyUndergraduateStudent.Where(o => o.DotKhaoSatId == surveyRoundId);
            var groupChuyenNganh = students.Select(o => o.Tenchnga ?? o.Tennganh).Distinct().ToList();

            //join bài làm được xét, => loại sinh viên không có bài được xét
            var sinhVienBaiLam = students.Join(_context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien.Where(o => o.BaiKhaoSatId == theSurveyId), o => o.ExMasv, o => o.StudentCode, (sv, bailam) => new { sv, bailam });

            //thống kê theo đợt và bài ks
            var reportTotal = _context.AsEduSurveyUndergraduateReportTotal.Where(o => o.TheSurveyId == theSurvey.Id && o.SurveyRoundId == surveyRoundId);
            foreach (var chuyenNganh in groupChuyenNganh)
            {
                _logger.LogInformation($"export nganh: {chuyenNganh}");
                col = 1;
                var bailamsv = sinhVienBaiLam.Where(o => o.sv.Tenchnga == chuyenNganh || (o.sv.Tenchnga == null && o.sv.Tennganh == chuyenNganh));
                var soPhieuPhatRa = bailamsv.Count();
                var soPhieuThuVe = bailamsv.Where(o => o.bailam.Status == (int)SurveyStudentStatus.Done).Count();


                var tyLeThamGia = "0%";
                if (soPhieuThuVe > 0)
                {
                    tyLeThamGia = $"{(double)soPhieuThuVe / soPhieuPhatRa * 100:0}%";
                }

                var tyLeKyVong = "80%";

                worksheet.Cells[row, col++].Value = chuyenNganh;
                
                worksheet.Cells[row, col].Value = soPhieuPhatRa;
                if (!tongToanTruong.ContainsKey(col))
                {
                    tongToanTruong.Add(col, soPhieuPhatRa);
                }
                else
                {
                    tongToanTruong[col] += soPhieuPhatRa;
                }
                col++;

                worksheet.Cells[row, col].Value = soPhieuThuVe;
                if (!tongToanTruong.ContainsKey(col))
                {
                    tongToanTruong.Add(col, soPhieuThuVe);
                }
                else
                {
                    tongToanTruong[col] += soPhieuThuVe;
                }
                col++;

                worksheet.Cells[row, col].Value = tyLeThamGia;
                col++;
                worksheet.Cells[row, col++].Value = tyLeKyVong;
                var index = 0;
                foreach (var cauhoi in noiDungDe)
                {
                    if (cauhoi.Type == QuestionType.SC)
                    {
                        var colStart = col;
                        worksheet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoi.Content}";

                        var dTB = 0;
                        var sumTotal = 0;
                        var diem = 0;
                        var colTotal = col + cauhoi.Answers.Count();
                        foreach (var dapan in cauhoi.Answers)
                        {
                            diem++;

                            //tổng toàn trường
                            if (!tongToanTruong.ContainsKey(col))
                            {
                                tongToanTruong.Add(col, 0);
                            }

                            worksheet.Cells[rowDapAn, col].Value = dapan.Content;
                            var ketqua = reportTotal.FirstOrDefault(o => o.ChuyenNganh == chuyenNganh && o.QuestionCode == cauhoi.Code && o.AnswerCode == dapan.Code);
                            if (ketqua != null)
                            {
                                var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                worksheet.Cells[row, col].Value = total;
                                sumTotal += total;

                                //tổng toàn trường
                                tongToanTruong[col] += total;

                                dTB += diem * total;
                            }
                            else
                            {
                                worksheet.Cells[row, col].Value = 0;
                            }
                            col++;
                        }
                        worksheet.Cells[rowDapAn, col].Value = "đTB";

                        if (sumTotal > 0)
                        {
                            worksheet.Cells[row, col].Value = $"{(double)dTB / sumTotal:0.0}";
                        }
                        else
                        {
                            worksheet.Cells[row, col].Value = 0;
                        }

                        //tổng hợp toàn trường
                        float dTBTotalToanTruong = 0;
                        float sumTotalToanTruong = 0;
                        int totalDiem = (colTotal) - (colTotal - cauhoi.Answers.Count);
                        for (int i = colTotal - 1; i >= colTotal - cauhoi.Answers.Count; i--)
                        {
                            sumTotalToanTruong += tongToanTruong[i];
                            dTBTotalToanTruong += tongToanTruong[i] * totalDiem;
                            totalDiem--;
                        }

                        //tổng tb toàn trường
                        if (sumTotalToanTruong > 0)
                        {
                            if (tongToanTruong.ContainsKey(colTotal))
                            {
                                tongToanTruong[colTotal] = (float)dTBTotalToanTruong / sumTotalToanTruong;
                            }
                            else
                            {
                                tongToanTruong.Add(colTotal, (float)dTBTotalToanTruong / sumTotalToanTruong);
                            }
                        }
                        else
                        {
                            if (tongToanTruong.ContainsKey(colTotal))
                            {
                                tongToanTruong[colTotal] = 0;
                            }
                            else
                            {
                                tongToanTruong.Add(colTotal, 0);
                            }
                        }

                        var colEnd = col++;
                        worksheet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                    }
                    else if (cauhoi.Type == QuestionType.MC)
                    {
                        var colStart = col;
                        worksheet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoi.Content}";
                        worksheet.Cells[rowCauHoi, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowCauHoi, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                        foreach (var dapan in cauhoi.Answers)
                        {
                            //tổng toàn trường
                            if (!tongToanTruong.ContainsKey(col))
                            {
                                tongToanTruong.Add(col, 0);
                            }

                            worksheet.Cells[rowDapAn, col].Value = dapan.Content;
                            var ketqua = reportTotal.FirstOrDefault(o => o.ChuyenNganh == chuyenNganh && o.QuestionCode == cauhoi.Code && o.AnswerCode == dapan.Code);
                            if (ketqua != null)
                            {
                                var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                worksheet.Cells[row, col].Value = total;

                                //tổng toàn trường
                                tongToanTruong[col] += total;
                            }
                            else
                            {
                                worksheet.Cells[row, col].Value = 0;
                            }

                            //câu hỏi con của đáp án
                            if (dapan.AnswerChildQuestion != null)
                            {
                                col++;
                                worksheet.Cells[rowDapAn, col].Value = dapan.AnswerChildQuestion.Content;
                                var ketquaCon = reportTotal.FirstOrDefault(o => o.ChuyenNganh == chuyenNganh && o.QuestionCode == $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}" && o.AnswerCode == dapan.Code);
                                worksheet.Cells[row, col].Value = ketquaCon?.Content ?? "";
                            }
                            col++;
                        }
                        var colEnd = col - 1;
                        worksheet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                    }
                    else if (cauhoi.Type == QuestionType.SA)
                    {
                        //worksheet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoi.Content}";
                        //worksheet.Column(col).Width = 48;
                        //var ketqua = reportTotal.FirstOrDefault(o => o.ChuyenNganh == chuyenNganh && o.QuestionCode == cauhoi.Code);
                        //if (ketqua != null && ketqua.Content != null)
                        //{
                        //    var str = "";
                        //    var listStr = JsonSerializer.Deserialize<List<string>>(ketqua.Content);
                        //    listStr.ForEach(s =>
                        //    {
                        //        str += $"{s};";
                        //    });
                        //    worksheet.Cells[row, col].Value = str;
                        //}
                        //col++;
                    }
                    else if (cauhoi.Type == QuestionType.GQ)
                    {
                        #region child question
                        int indexChildQuestion = 0;
                        foreach(var childQuestion in cauhoi.ChildQuestion)
                        {
                            if (childQuestion.Type == QuestionType.SC)
                            {
                                var colStart = col;
                                worksheet.Cells[rowCauHoi, col].Value = $"Câu {index}.{indexChildQuestion}: {childQuestion.Content}";

                                var dTB = 0;
                                var sumTotal = 0;
                                var diem = 0;
                                var colTotal = col + childQuestion.Answers.Count();
                                foreach (var dapan in childQuestion.Answers)
                                {
                                    diem++;

                                    //tổng toàn trường
                                    if (!tongToanTruong.ContainsKey(col))
                                    {
                                        tongToanTruong.Add(col, 0);
                                    }

                                    worksheet.Cells[rowDapAn, col].Value = dapan.Content;
                                    var ketqua = reportTotal.FirstOrDefault(o => o.ChuyenNganh == chuyenNganh && o.QuestionCode == childQuestion.Code && o.AnswerCode == dapan.Code);
                                    if (ketqua != null)
                                    {
                                        var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                        worksheet.Cells[row, col].Value = total;
                                        sumTotal += total;

                                        //tổng toàn trường
                                        tongToanTruong[col] += total;

                                        dTB += diem * total;
                                    }
                                    else
                                    {
                                        worksheet.Cells[row, col].Value = 0;
                                    }
                                    col++;
                                }
                                worksheet.Cells[rowDapAn, col].Value = "đTB";

                                if (sumTotal > 0)
                                {
                                    worksheet.Cells[row, col].Value = $"{(double)dTB / sumTotal:0.0}";
                                }
                                else
                                {
                                    worksheet.Cells[row, col].Value = 0;
                                }

                                //tổng hợp toàn trường
                                float dTBTotalToanTruong = 0;
                                float sumTotalToanTruong = 0;
                                int totalDiem = (colTotal) - (colTotal - childQuestion.Answers.Count);
                                for (int i = colTotal - 1; i >= colTotal - childQuestion.Answers.Count; i--)
                                {
                                    sumTotalToanTruong += tongToanTruong[i];
                                    dTBTotalToanTruong += tongToanTruong[i] * totalDiem;
                                    totalDiem--;
                                }

                                //tổng tb toàn trường
                                if (sumTotalToanTruong > 0)
                                {
                                    if (tongToanTruong.ContainsKey(colTotal))
                                    {
                                        tongToanTruong[colTotal] = (float)dTBTotalToanTruong / sumTotalToanTruong;
                                    }
                                    else
                                    {
                                        tongToanTruong.Add(colTotal, (float)dTBTotalToanTruong / sumTotalToanTruong);
                                    }
                                }
                                else
                                {
                                    if (tongToanTruong.ContainsKey(colTotal))
                                    {
                                        tongToanTruong[colTotal] = 0;
                                    }
                                    else
                                    {
                                        tongToanTruong.Add(colTotal, 0);
                                    }
                                }

                                var colEnd = col++;
                                worksheet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                            }
                            else if (childQuestion.Type == QuestionType.MC)
                            {
                                var colStart = col;
                                worksheet.Cells[rowCauHoi, col].Value = $"Câu {index}.{indexChildQuestion}: {childQuestion.Content}";
                                worksheet.Cells[rowCauHoi, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[rowCauHoi, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                                foreach (var dapan in childQuestion.Answers)
                                {
                                    //tổng toàn trường
                                    if (!tongToanTruong.ContainsKey(col))
                                    {
                                        tongToanTruong.Add(col, 0);
                                    }

                                    worksheet.Cells[rowDapAn, col].Value = dapan.Content;
                                    var ketqua = reportTotal.FirstOrDefault(o => o.ChuyenNganh == chuyenNganh && o.QuestionCode == childQuestion.Code && o.AnswerCode == dapan.Code);
                                    if (ketqua != null)
                                    {
                                        var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                        worksheet.Cells[row, col].Value = total;

                                        //tổng toàn trường
                                        tongToanTruong[col] += total;
                                    }
                                    else
                                    {
                                        worksheet.Cells[row, col].Value = 0;
                                    }

                                    //câu hỏi con của đáp án
                                    if (dapan.AnswerChildQuestion != null)
                                    {
                                        col++;
                                        worksheet.Cells[rowDapAn, col].Value = dapan.AnswerChildQuestion.Content;
                                        var ketquaCon = reportTotal.FirstOrDefault(o => o.ChuyenNganh == chuyenNganh && o.QuestionCode == $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}" && o.AnswerCode == dapan.Code);
                                        worksheet.Cells[row, col].Value = ketquaCon?.Content ?? "";
                                    }
                                    col++;
                                }
                                var colEnd = col - 1;
                                worksheet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                            }
                            //else if (childQuestion.Type == QuestionType.SA)
                            //{
                            //    worksheet.Cells[rowCauHoi, col].Value = $"Câu {index}.{indexChildQuestion}: {childQuestion.Content}";
                            //    worksheet.Column(col).Width = 48;
                            //    var ketqua = reportTotal.FirstOrDefault(o => o.ChuyenNganh == chuyenNganh && o.QuestionCode == childQuestion.Code);
                            //    if (ketqua != null && ketqua.Content != null)
                            //    {
                            //        var str = "";
                            //        var listStr = JsonSerializer.Deserialize<List<string>>(ketqua.Content);
                            //        listStr.ForEach(s =>
                            //        {
                            //            str += $"{s};";
                            //        });
                            //        worksheet.Cells[row, col].Value = str;
                            //    }
                            //    col++;
                            //}
                            index++;
                        }
                        #endregion
                    }
                }

                row++;
            }

            col = 1;
            worksheet.Cells[row, col].Value = "Toàn trường";
            foreach (var item in tongToanTruong)
            {
                var value = item.Value.ToString().Split(".");
                if(value[^1] == "0" || value.Length == 1)
                {
                    worksheet.Cells[row, item.Key].Value = $"{item.Value:0}";
                }
                else
                {
                    worksheet.Cells[row, item.Key].Value = $"{item.Value:0.0}";
                }
            }

            //tỉ lệ tham gia toàn trường
            if (tongToanTruong[2] > 0)
            {
                worksheet.Cells[row, 4].Value = $"{(double)tongToanTruong[3] / tongToanTruong[2] * 100:0}%";
            }

            var fileName = Guid.NewGuid().ToString() + ".xlsx";
            var filePath = System.IO.Path.GetTempPath() + fileName;
            _logger.LogInformation($"{filePath}");
            _logger.LogInformation("export report undergraduate is done.");
            using (MemoryStream ms = new MemoryStream())
            {
                excel.SaveAs(ms);
                return ms.ToArray();
            }
        }

        public async Task<PaginationModel<ReportTotalUndergraduate>> GetRawReportTotalUndergraduateSurvey(ReportTotalUndergraduateFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyReportTotal> query = null;
            var recordsTotal = await _context.AsEduSurveyUndergraduateReportTotal.CountAsync();

            var recordsFiltered = recordsTotal;
            if (query != null)
            {
                recordsFiltered = await query.CountAsync();
            }

            var result = await _context.AsEduSurveyUndergraduateReportTotal
                .Skip(skip).Take(take)
                .Join(_context.AsEduSurveyUndergraduateSurveyRound, o => o.SurveyRoundId, o => o.Id, (report, surveyRound) => new { report, surveyRound })
                .Join(_context.AsEduSurveyUndergraduateBaiKhaoSat, o => o.report.TheSurveyId, o => o.Id, (reportSurveyRound, theSurvey) => new { reportSurveyRound, theSurvey })
                .OrderByDescending(o => o.reportSurveyRound.surveyRound.FromDate)
                .Select(o => new ReportTotalUndergraduate
                {
                    Id = o.reportSurveyRound.report.Id,
                    SurveyRoundId = o.reportSurveyRound.surveyRound.Id,
                    SurveyRoundName = o.reportSurveyRound.surveyRound.Name,
                    TheSurveyId = o.theSurvey.Id,
                    TheSurveyName = o.theSurvey.Name,
                    ChuyenNganh = o.reportSurveyRound.report.ChuyenNganh,
                    QuestionCode = o.reportSurveyRound.report.QuestionCode,
                    AnswerCode = o.reportSurveyRound.report.AnswerCode,
                    Total = o.reportSurveyRound.report.Total,
                    Content = o.reportSurveyRound.report.Content
                }).ToListAsync();

            return new PaginationModel<ReportTotalUndergraduate>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = result
            };
        }

        public async Task ReportTotalUndergraduateSurvey(Guid surveyRoundId, Guid theSurveyId)
        {
            var surveyRound = _context.AsEduSurveyUndergraduateSurveyRound.FirstOrDefault(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            //đợt khảo sát chưa kết thúc
            if (surveyRound.Status != (int)SurveyRoundStatus.Closed && surveyRound.Status != (int)SurveyRoundStatus.End)
            {
                throw new HandleException.InvalidInputDataException("Đợt khảo sát chưa đóng hoặc chưa kết thúc");
            }

            var theSurvey = await _context.AsEduSurveyUndergraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurveyId && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            _logger.LogInformation("report total undergraduate is start.");
            var students = _context.AsEduSurveyUndergraduateStudent.Where(o => o.DotKhaoSatId == surveyRoundId);
            var baiKSSinhViens = _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien.Where(o => o.BaiKhaoSatId == theSurveyId && o.Status == (int)SurveyStudentStatus.Done);
            var svBaiLam = students.Join(baiKSSinhViens, o => o.Masv, o => o.StudentCode, (sv, baikssv) => new { baikssv })
                .Select(o => o.baikssv)
                .ToList();

            var groupSvBaiLamTheoChNganh = svBaiLam
                .Select(o => new { 
                    o.Id,
                    o.BaiKhaoSatId,
                    o.StudentCode,
                    o.DeThi,
                    o.BaiLam,
                    o.Nganh,
                    ChuyenNganh = o.ChuyenNganh ?? o.Nganh,
                    o.NgayGioBatDau,
                    o.NgayGioNopBai,
                    o.LogIp,
                    o.Type,
                    o.Status
                })
                .GroupBy(o => new { o.ChuyenNganh }).ToList();

            foreach (var itemSvBaiLam in groupSvBaiLamTheoChNganh)
            {
                var dsBaiKSSV = itemSvBaiLam.ToList();
                var selectedAnswers = new List<SelectedAnswerExtend>();
                foreach (var baikssv in dsBaiKSSV)
                {
                    var baikhaosatId = baikssv.BaiKhaoSatId;

                    var json = JsonSerializer.Deserialize<List<SelectedAnswerExtend>>(baikssv.BaiLam);

                    json.ForEach(o => o.TheSurveyId = baikhaosatId);
                    selectedAnswers.AddRange(json);
                }

                var total = AnswerSelectedReportTotal(selectedAnswers);

                foreach (var item in total)
                {
                    var report = await _context.AsEduSurveyUndergraduateReportTotal
                        .FirstOrDefaultAsync(o => o.SurveyRoundId == surveyRoundId && o.TheSurveyId == o.TheSurveyId && o.ChuyenNganh == itemSvBaiLam.Key.ChuyenNganh && o.QuestionCode == item.QuestionCode && o.AnswerCode == item.AnswerCode);
                    if (report == null) //chưa có thì thêm
                    {
                        _context.AsEduSurveyUndergraduateReportTotal.Add(new AsEduSurveyUndergraduateReportTotal
                        {
                            Id = Guid.NewGuid(),
                            SurveyRoundId = surveyRoundId,
                            TheSurveyId = item.TheSurveyId,
                            ChuyenNganh = itemSvBaiLam.Key.ChuyenNganh,
                            QuestionCode = item.QuestionCode,
                            AnswerCode = item.AnswerCode,
                            Content = item.Content,
                            Total = item.Total,
                        });
                    }
                    else //có thì update
                    {
                        report.Total = item.Total;
                        report.Content = item.Content;
                    }
                }

            }
            _context.SaveChanges();
            _logger.LogInformation("report total undergraduate is done.");
        }

    }
}
