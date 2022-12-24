using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Survey.Base;

using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Undergraduate.Statistic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements.Undergraduate
{
    public class AsEduSurveyUndergraduateReportTotalService : ThongKeServiceBase
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

        public async Task<byte[]> ExportReportTotalUndergraduateSurvey(Guid surveyRoundId, Guid theSurveyId, DateTime fromDate, DateTime toDate)
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
            var students = _context.AsEduSurveyUndergraduateStudent.Where(o => o.DotKhaoSatId == surveyRoundId)
                .Select(stu => new { 
                    stu.ExMasv,
                    //trường kết hợp ngành /chuyên ngành, chuyên ngành mà không bỏ trống thì lấy nếu bỏ trống thì lấy ngành
                    ChuyenNganh = !string.IsNullOrWhiteSpace(stu.Tenchnga) ? stu.Tenchnga.Trim() : stu.Tennganh.Trim() 
                });
            var groupChuyenNganh = students.Select(o => o.ChuyenNganh).Distinct().ToList();

            //join bài làm được xét, => loại sinh viên không có bài được xét
            var joinSinhVienBaiLam = students
                    .Join(_context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien.Where(o => o.BaiKhaoSatId == theSurveyId && fromDate <= o.NgayGioNopBai && toDate >= o.NgayGioNopBai),
                    o => o.ExMasv, o => o.StudentCode, (sv, bailam) => new { sv, bailam });
#if DEBUG
            //var soStudent = students.Where(o => o.Tennganh == "Kiến trúc" || o.Tenchnga == "Kiến trúc").Count();
            //var svBaiLam = sinhVienBaiLam.Where(o => o.bailam.Nganh == "Kiến trúc" || o.bailam.ChuyenNganh == "Kiến trúc" && o.bailam.Status == 5).Count();
            //var svBaiLam2 = _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien
            //    .Where(o => (o.Nganh == "Kiến trúc" || o.ChuyenNganh == "Kiến trúc") && o.Status == 5 && fromDate <= o.NgayGioNopBai && toDate >= o.NgayGioNopBai).Count();

#endif
            //thống kê theo đợt và bài ks
            var reportTotal = _context.AsEduSurveyUndergraduateReportTotal.Where(o => o.TheSurveyId == theSurvey.Id && o.SurveyRoundId == surveyRoundId);
            foreach (var chuyenNganh in groupChuyenNganh)
            {
                _logger.LogInformation($"export nganh: {chuyenNganh}");
                col = 1;
                var bailamsv = joinSinhVienBaiLam.Where(o => o.sv.ChuyenNganh == chuyenNganh || o.sv.ChuyenNganh == chuyenNganh);
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
#if DEBUG
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                excel.SaveAs(fs);
            }
#endif
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


        public void ReportTotalUnderGraduateSurveyCustom(Guid surveyRoundId)
        {
            try
            {
                string baiKhaoSatId = "84204608-2C88-4663-8A9C-F25CCE5E046D".ToLower();

                // giải bài làm
                List<AnswerSelectedReportTotal> total = new List<AnswerSelectedReportTotal>();

                var cacBaiLam = _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien
                    .Where(o => o.BaiKhaoSatId.ToString().ToLower() == baiKhaoSatId)
                    //.Where(o => o.Status == (int)SurveyStudentStatus.Done);
                    ;
                var selectedAnswers = new List<SelectedAnswerExtend>();
                foreach (var bailam in cacBaiLam)
                {
                    if (string.IsNullOrEmpty(bailam.BaiLam))
                    {
                        continue;
                    }
                    //giải chuỗi json bài làm của sinh viên
                    var json = JsonSerializer.Deserialize<List<SelectedAnswerExtend>>(bailam.BaiLam);
                    json.ForEach(o => {
                        o.TheSurveyId = bailam.BaiKhaoSatId;
                        o.StudentCode = bailam.StudentCode;
                    });
                    selectedAnswers.AddRange(json);
                }

                //giải đề
                var deLyThuyetThucHanh = _context.AsEduSurveyUndergraduateBaiKhaoSat.FirstOrDefault(o => o.Id.ToString().ToLower() == baiKhaoSatId);
                List<QuestionJson> QuestionJsonData = JsonSerializer.Deserialize<List<QuestionJson>>(deLyThuyetThucHanh.NoiDungDeThi);

                foreach (var question in QuestionJsonData.Where(q => q.Type == QuestionType.SC))
                {
                    if (question.Answers == null)
                        continue;

                    var listAnswer = selectedAnswers.Where(a => a.QuestionCode == question.Code);

                    foreach (var svTraLoi in listAnswer)
                    {
                        total.Add(new Models.Survey.JsonData.AnswerSelectedReportTotal
                        {
                            TheSurveyId = svTraLoi.TheSurveyId,
                            QuestionCode = question.Code,
                            QuestionContent = question.Content,
                            AnswerCode = svTraLoi.AnswerCode,
                            Content = question.Answers.FirstOrDefault(x => x.Code == svTraLoi.AnswerCode)?.Content,
                            StudentCode = svTraLoi.StudentCode
                        });
                    }
                }

                foreach (var question in QuestionJsonData.Where(q => q.Type == QuestionType.SA))
                {
                    var cauTraText = selectedAnswers.Where(a => a.QuestionCode == question.Code);

                    foreach (var traLoiText in cauTraText)
                    {
                        total.Add(new AnswerSelectedReportTotal
                        {
                            TheSurveyId = traLoiText.TheSurveyId,
                            QuestionCode = question.Code,
                            QuestionContent = question.Content,
                            Content = traLoiText.AnswerContent,
                            StudentCode = traLoiText.StudentCode 
                        });
                    }
                }

                foreach (var question in QuestionJsonData.Where(q => q.Type == QuestionType.MC))
                {
                    //if (question.Answers == null)
                    //    continue;

                    //int testSum = 0;
                    //foreach (var answer in question.Answers)
                    //{
                    //    var listAnswer = selectedAnswers.Where(a => a.QuestionCode == question.Code && a.AnswerCode != null && a.AnswerCodes.Contains(answer.Code));

                    //    foreach (var svTraLoi in listAnswer)
                    //    {
                    //        total.Add(new Models.Survey.JsonData.AnswerSelectedReportTotal
                    //        {
                    //            TheSurveyId = svTraLoi.TheSurveyId,
                    //            QuestionCode = question.Code,
                    //            QuestionContent = question.Content,
                    //            AnswerCode = answer.Code,
                    //            StudentCode = svTraLoi.StudentCode
                    //            //Total = countAnswer
                    //        });
                    //    }


                    //    if (answer.AnswerChildQuestion != null)
                    //    {
                    //        var cauTraLoiCon = selectedAnswers.Where(a => a.QuestionCode == answer.AnswerChildQuestion.Code);

                    //        foreach (var svTraLoiCon in cauTraLoiCon)
                    //        {
                    //            total.Add(new AnswerSelectedReportTotal
                    //            {
                    //                TheSurveyId = svTraLoiCon.TheSurveyId,
                    //                QuestionCode = question.Code,
                    //                QuestionContent = question.Content,
                    //                Content = svTraLoiCon.AnswerContent,
                    //                StudentCode = svTraLoiCon.StudentCode,
                    //            });
                    //        }


                    //        //if (cauTraLoiCon != null)
                    //        //{
                    //        //    total.Add(new Models.Survey.JsonData.AnswerSelectedReportTotal
                    //        //    {
                    //        //        TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                    //        //        QuestionCode = answer.AnswerChildQuestion.Code,
                    //        //        QuestionContent = question.Content,
                    //        //        Content = string.Join(";", strAllAnswerContent)
                    //        //    });
                    //        //}
                    //    }
                    //}                    //if (question.Answers == null)
                    //    continue;

                    //int testSum = 0;
                    //foreach (var answer in question.Answers)
                    //{
                    //    var listAnswer = selectedAnswers.Where(a => a.QuestionCode == question.Code && a.AnswerCode != null && a.AnswerCodes.Contains(answer.Code));

                    //    foreach (var svTraLoi in listAnswer)
                    //    {
                    //        total.Add(new Models.Survey.JsonData.AnswerSelectedReportTotal
                    //        {
                    //            TheSurveyId = svTraLoi.TheSurveyId,
                    //            QuestionCode = question.Code,
                    //            QuestionContent = question.Content,
                    //            AnswerCode = answer.Code,
                    //            StudentCode = svTraLoi.StudentCode
                    //            //Total = countAnswer
                    //        });
                    //    }


                    //    if (answer.AnswerChildQuestion != null)
                    //    {
                    //        var cauTraLoiCon = selectedAnswers.Where(a => a.QuestionCode == answer.AnswerChildQuestion.Code);

                    //        foreach (var svTraLoiCon in cauTraLoiCon)
                    //        {
                    //            total.Add(new AnswerSelectedReportTotal
                    //            {
                    //                TheSurveyId = svTraLoiCon.TheSurveyId,
                    //                QuestionCode = question.Code,
                    //                QuestionContent = question.Content,
                    //                Content = svTraLoiCon.AnswerContent,
                    //                StudentCode = svTraLoiCon.StudentCode,
                    //            });
                    //        }


                    //        //if (cauTraLoiCon != null)
                    //        //{
                    //        //    total.Add(new Models.Survey.JsonData.AnswerSelectedReportTotal
                    //        //    {
                    //        //        TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                    //        //        QuestionCode = answer.AnswerChildQuestion.Code,
                    //        //        QuestionContent = question.Content,
                    //        //        Content = string.Join(";", strAllAnswerContent)
                    //        //    });
                    //        //}
                    //    }
                    //}
                }

                foreach (var questionGQ in QuestionJsonData.Where(q => q.Type == QuestionType.GQ))
                {
                    foreach (var question in questionGQ.ChildQuestion.Where(q => q.Type == QuestionType.SA))
                    {
                        var cauTraText = selectedAnswers.Where(a => a.QuestionCode == question.Code);

                        foreach (var traLoiText in cauTraText)
                        {
                            total.Add(new AnswerSelectedReportTotal
                            {
                                TheSurveyId = traLoiText.TheSurveyId,
                                QuestionCode = question.Code,
                                QuestionContent = question.Content,
                                Content = traLoiText.AnswerContent,
                                StudentCode = traLoiText.StudentCode
                            });
                        }

                        //List<string> strAllAnswerContent = new List<string>();

                        ////foreach (var str in cauTraText)
                        ////{
                        ////    strAllAnswerContent.Add(str.AnswerContent);
                        ////}

                        //var options = new JsonSerializerOptions
                        //{
                        //    IgnoreNullValues = true,
                        //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                        //};

                        //string content = JsonSerializer.Serialize(strAllAnswerContent, options);

                        //total.Add(new AnswerSelectedReportTotal
                        //{
                        //    //TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                        //    QuestionCode = question.Code,
                        //    QuestionContent = question.Content,
                        //    Content = content
                        //});
                    }
                }

                //var surveyRound = _context.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
                //if (surveyRound == null)
                //{
                //    throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
                //}

                ////đợt khảo sát chưa kết thúc
                //if (!(surveyRound.Status == (int)SurveyRoundStatus.Closed || surveyRound.Status == (int)SurveyRoundStatus.End || DateTime.Now >= surveyRound.EndDate))
                //{
                //    throw new InvalidInputDataException("Đợt khảo sát chưa đóng hoặc chưa kết thúc");
                //}

                ////do chỉ có một bài ks nên lấy id của bài ks đó
                //var idbaikscuadotnays = _context.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id).Select(o => o.Id).ToList();
                //if (idbaikscuadotnays.Count == 0)
                //{
                //    throw new RecordNotFoundException("Không tìm thấy bài khảo sát của đợt khảo sát này");
                //}

                //_logger.LogInformation("report total normal is start.");
                ////surveyContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE {TableNameTask.AsEduSurveyReportTotal}");
                //var baikshoanthanh = _context.AsEduSurveyBaiKhaoSatSinhVien
                //    .Where(o => idbaikscuadotnays.Contains(o.BaiKhaoSatId))
                //    .Where(o => o.Status == (int)SurveyStudentStatus.Done);

                //var testSoBaiKs = baikshoanthanh.Count();

                //var tongBaiKsHoanThanh = baikshoanthanh.Count();
                //List<SelectedAnswerExtend> selectedAnswers;

                //var groupLopGiangVien = baikshoanthanh
                //.GroupBy(o => new { o.LecturerCode, o.ClassRoomCode, o.BaiKhaoSatId, o.Nhhk })
                //.Select(r => new { r.Key.LecturerCode, r.Key.ClassRoomCode, r.Key.BaiKhaoSatId, r.Key.Nhhk })
                //.ToList();

                //var totalLectureClassroom = groupLopGiangVien.Count();
                //var count = 0;
                //foreach (var lectureClassroom in groupLopGiangVien)
                //{
                //    //hay lỗi ở lớp không có giảng viên
                //    var lectureCode = lectureClassroom.LecturerCode;
                //    var classroomCode = lectureClassroom.ClassRoomCode;
                //    var nhhk = lectureClassroom.Nhhk;

                //    //lọc theo từng giảng viên lớp môn học của từng nhhk
                //    var cacBaiLam = baikshoanthanh.Where(o => o.ClassRoomCode == classroomCode && o.LecturerCode == lectureCode && o.Nhhk == nhhk).ToList();
                //    selectedAnswers = new List<SelectedAnswerExtend>(); //bài làm đẩy từ json sang

                //    foreach (var bailam in cacBaiLam)
                //    {
                //        if (string.IsNullOrEmpty(bailam.BaiLam))
                //        {
                //            continue;
                //        }
                //        //giải chuỗi json bài làm của sinh viên
                //        var json = JsonSerializer.Deserialize<List<SelectedAnswerExtend>>(bailam.BaiLam);
                //        json.ForEach(o => o.TheSurveyId = bailam.BaiKhaoSatId);
                //        selectedAnswers.AddRange(json);
                //    }

                //    //lấy mã môn học
                //    string subjectCode = null;
                //    string classroom = null;

                //    //bài đầu tiên trong lớp môn học đấy để lấy subject Code và class room code
                //    var baiLamDauTien = _context.AsEduSurveyBaiKhaoSatSinhVien
                //        .FirstOrDefault(bl => idbaikscuadotnays.Contains(bl.BaiKhaoSatId) && bl.ClassRoomCode == classroomCode
                //            && bl.Nhhk == nhhk && !string.IsNullOrWhiteSpace(bl.SubjectCode));

                //    if (baiLamDauTien != null)
                //    {
                //        subjectCode = baiLamDauTien.SubjectCode;
                //        classroom = baiLamDauTien.ClassRoomCode?.Replace(baiLamDauTien.SubjectCode ?? "", "");
                //    }

                //    //giải đề
                //    var deLyThuyetThucHanh = _context.AsEduSurveyBaiKhaoSat.FirstOrDefault(o => o.Id == baiLamDauTien.BaiKhaoSatId);
                //    List<QuestionJson> QuestionJsonData = JsonSerializer.Deserialize<List<QuestionJson>>(deLyThuyetThucHanh.NoiDungDeThi);

                //    List<AnswerSelectedReportTotal> total = new List<AnswerSelectedReportTotal>();
                //    foreach (var question in QuestionJsonData.Where(q => q.Type == QuestionType.SC))
                //    {
                //        if (question.Answers == null)
                //            continue;

                //        int testSum = 0;
                //        foreach (var answer in question.Answers)
                //        {
                //            var countAnswer = selectedAnswers.Count(a => a.QuestionCode == question.Code && a.AnswerCode == answer.Code);
                //            testSum += countAnswer;

                //            if (countAnswer > 0)
                //            {
                //                total.Add(new Models.Survey.JsonData.AnswerSelectedReportTotal
                //                {
                //                    TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                //                    QuestionCode = question.Code,
                //                    AnswerCode = answer.Code,
                //                    Total = countAnswer
                //                });
                //            }
                //        }
                //    }

                //    foreach (var question in QuestionJsonData.Where(q => q.Type == QuestionType.SA))
                //    {
                //        var cauTraText = selectedAnswers.Where(a => a.QuestionCode == question.Code);
                //        List<string> strAllAnswerContent = new List<string>();

                //        foreach (var str in cauTraText)
                //        {
                //            strAllAnswerContent.Add(str.AnswerContent);
                //        }

                //        var options = new JsonSerializerOptions
                //        {
                //            IgnoreNullValues = true,
                //            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                //        };

                //        string content = JsonSerializer.Serialize(strAllAnswerContent, options);

                //        total.Add(new AnswerSelectedReportTotal
                //        {
                //            TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                //            QuestionCode = question.Code,
                //            Content = content
                //        });
                //    }

                //    foreach (var question in QuestionJsonData.Where(q => q.Type == QuestionType.MC))
                //    {
                //        if (question.Answers == null)
                //            continue;

                //        int testSum = 0;
                //        foreach (var answer in question.Answers)
                //        {
                //            var countAnswer = selectedAnswers.Count(a => a.QuestionCode == question.Code && a.AnswerCodes != null && a.AnswerCodes.Contains(answer.Code));
                //            testSum += countAnswer;

                //            total.Add(new Models.Survey.JsonData.AnswerSelectedReportTotal
                //            {
                //                TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                //                QuestionCode = question.Code,
                //                AnswerCode = answer.Code,
                //                Total = countAnswer
                //            });

                //            if (answer.AnswerChildQuestion != null)
                //            {
                //                var cauTraLoiCon = selectedAnswers.Where(a => a.QuestionCode == answer.AnswerChildQuestion.Code);

                //                List<string> strAllAnswerContent = new List<string>();

                //                foreach (var str in cauTraLoiCon)
                //                {
                //                    strAllAnswerContent.Add(str.AnswerContent);
                //                }

                //                var options = new JsonSerializerOptions
                //                {
                //                    IgnoreNullValues = true,
                //                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                //                };

                //                total.Add(new AnswerSelectedReportTotal
                //                {
                //                    TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                //                    QuestionCode = question.Code,
                //                    Content = JsonSerializer.Serialize(strAllAnswerContent, options)
                //                });

                //                if (cauTraLoiCon != null)
                //                {
                //                    total.Add(new Models.Survey.JsonData.AnswerSelectedReportTotal
                //                    {
                //                        TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                //                        QuestionCode = answer.AnswerChildQuestion.Code,
                //                        Content = string.Join(";", strAllAnswerContent)
                //                    });
                //                }
                //            }
                //        }

                //        if (testSum != 21)
                //        {

                //        }
                //    }

                //    foreach (var questionGQ in QuestionJsonData.Where(q => q.Type == QuestionType.GQ))
                //    {
                //        foreach (var question in questionGQ.ChildQuestion.Where(q => q.Type == QuestionType.SA))
                //        {
                //            var cauTraText = selectedAnswers.Where(a => a.QuestionCode == question.Code);
                //            List<string> strAllAnswerContent = new List<string>();

                //            foreach (var str in cauTraText)
                //            {
                //                strAllAnswerContent.Add(str.AnswerContent);
                //            }

                //            var options = new JsonSerializerOptions
                //            {
                //                IgnoreNullValues = true,
                //                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                //            };

                //            string content = JsonSerializer.Serialize(strAllAnswerContent, options);

                //            total.Add(new AnswerSelectedReportTotal
                //            {
                //                TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                //                QuestionCode = question.Code,
                //                Content = content
                //            });
                //        }
                //    }

                //total = AnswerSelectedReportTotal(selectedAnswers);
                //foreach (var item in total)
                //{
                //    var thongkecuthe = _context.AsEduSurveyReportTotal
                //        .FirstOrDefault(o => o.ClassRoomCode == classroomCode && o.Nhhk == nhhk && o.LecturerCode == lectureCode && o.TheSurveyId == item.TheSurveyId &&
                //        o.QuestionCode == item.QuestionCode && o.AnswerCode == item.AnswerCode);
                //    if (thongkecuthe == null)
                //    {
                //        _context.AsEduSurveyReportTotal.Add(new AsEduSurveyReportTotal
                //        {
                //            Id = Guid.NewGuid(),
                //            SurveyRoundId = surveyRound.Id,
                //            TheSurveyId = item.TheSurveyId,
                //            ClassRoomCode = classroomCode,
                //            SubjectCode = subjectCode,
                //            ClassRoom = classroom,
                //            Nhhk = nhhk,
                //            LecturerCode = lectureCode,
                //            QuestionCode = item.QuestionCode,
                //            AnswerCode = item.AnswerCode,
                //            Content = item.Content,
                //            Total = item.Total,
                //        });
                //    }
                //    else //nếu có rồi thì cập nhật
                //    {
                //        thongkecuthe.SubjectCode = subjectCode;
                //        thongkecuthe.ClassRoom = classroom;
                //        thongkecuthe.Content = item.Content;
                //        thongkecuthe.Total = item.Total;
                //    }
                //}

                //_logger.LogInformation($"thong ke hoan thanh {++count}/{totalLectureClassroom}, ma gv: {lectureCode}, ma lop: {classroomCode}");
            //}
                var test = total;  
                _logger.LogInformation("report total under graduate is done.");
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ReportTotalUndergraduateSurvey(Guid surveyRoundId, Guid theSurveyId, DateTime fromDate, DateTime toDate)
        {
            _context.Database.ExecuteSqlRaw("TRUNCATE TABLE As_Edu_Survey_Undergraduate_ReportTotal");
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

            var theSurvey = _context.AsEduSurveyUndergraduateBaiKhaoSat.FirstOrDefault(o => o.Id == theSurveyId && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            _logger.LogInformation("report total undergraduate is start.");
            var students = _context.AsEduSurveyUndergraduateStudent.Where(o => o.DotKhaoSatId == surveyRoundId);
            var baiKSSinhViens = _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien
                .Where(o => o.BaiKhaoSatId == theSurveyId && o.Status == (int)SurveyStudentStatus.Done && fromDate <= o.NgayGioNopBai && toDate >= o.NgayGioNopBai);
#if DEBUG
            //var test = baiKSSinhViens.Count();
#endif
            var svBaiLam = students.Join(baiKSSinhViens, o => o.ExMasv, o => o.StudentCode, (sv, baikssv) => new { baikssv })
                .Select(o => o.baikssv)
                .ToList();

            var groupSvBaiLamTheoChNganh = svBaiLam
                .Select(o => new
                {
                    o.Id,
                    o.BaiKhaoSatId,
                    o.StudentCode,
                    o.DeThi,
                    o.BaiLam,
                    o.Nganh,
                    ChuyenNganh = !string.IsNullOrWhiteSpace(o.ChuyenNganh) ? o.ChuyenNganh.Trim() : o.Nganh.Trim(), //trường kết hợp ngành /chuyên ngành
                    o.NgayGioBatDau,
                    o.NgayGioNopBai,
                    o.LogIp,
                    o.Type,
                    o.Status
                })
                .OrderBy(o => o.ChuyenNganh)
                .GroupBy(o => o.ChuyenNganh).ToList();

            foreach (var itemSvBaiLam in groupSvBaiLamTheoChNganh)
            {
                var dsBaiKSSV = itemSvBaiLam.ToList();
#if DEBUG
                if (itemSvBaiLam.Key == "Kỹ thuật xây dựng Công trình Giao thông")
                {
                    var count = dsBaiKSSV.Count();
                }
#endif
                var selectedAnswers = new List<SelectedAnswerExtend>();
                foreach (var baikssv in dsBaiKSSV)
                {
                    var baikhaosatId = baikssv.BaiKhaoSatId;

                    try
                    {
                        var json = JsonSerializer.Deserialize<List<SelectedAnswerExtend>>(baikssv.BaiLam);

                        json.ForEach(o => o.TheSurveyId = baikhaosatId);
                        selectedAnswers.AddRange(json);
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning(e, $"khong Deserialize bai khao sat sinh vien id = {baikssv.Id}, student code = {baikssv.StudentCode}, bai lam = {baikssv.BaiLam}");
                    }
                }

                var total = AnswerSelectedReportTotal(selectedAnswers);

                foreach (var item in total)
                {
                    var report = _context.AsEduSurveyUndergraduateReportTotal
                        .FirstOrDefault(o => o.SurveyRoundId == surveyRoundId && o.TheSurveyId == o.TheSurveyId &&
                        o.ChuyenNganh == itemSvBaiLam.Key && o.QuestionCode == item.QuestionCode && o.AnswerCode == item.AnswerCode);
                    if (report == null) //chưa có thì thêm
                    {
                        _context.AsEduSurveyUndergraduateReportTotal.Add(new AsEduSurveyUndergraduateReportTotal
                        {
                            Id = Guid.NewGuid(),
                            SurveyRoundId = surveyRoundId,
                            TheSurveyId = item.TheSurveyId,
                            ChuyenNganh = itemSvBaiLam.Key,
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
