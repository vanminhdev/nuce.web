using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Background;
using nuce.web.api.Services.Status.Implements;
using nuce.web.api.Services.Survey.Base;

using nuce.web.api.ViewModel.Survey.Normal.Statistic;
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

namespace nuce.web.api.Services.Survey.BackgroundTasks
{
    public class SurveyStatisticBackgroundTask : ThongKeServiceBase
    {
        private readonly ILogger<SurveyStatisticBackgroundTask> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly BackgroundTaskWorkder _backgroundTaskWorker;
        private readonly StatusService _statusService;

        public SurveyStatisticBackgroundTask(ILogger<SurveyStatisticBackgroundTask> logger,
            IServiceScopeFactory scopeFactory, BackgroundTaskWorkder backgroundTaskWorker,
             StatusService statusService)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _backgroundTaskWorker = backgroundTaskWorker;
            _statusService = statusService;
        }

        #region thống kê khảo sát thường
        #region thống kê dữ liệu thô
        private void ReportTotalNormalSurveyBG(Guid surveyRoundId)
        {
            var scope = _scopeFactory.CreateScope();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();

            var status = statusContext.AsStatusTableTask.FirstOrDefault(o => o.TableName == TableNameTask.AsEduSurveyReportTotal);
            if (status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng thống kê khảo sát sinh viên");
            }
            //bảng đang làm việc
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Đang thống kê, thao tác bị huỷ");
            }
            status.Status = (int)TableTaskStatus.Doing;
            statusContext.SaveChanges();

            try
            {
                var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
                if (surveyRound == null)
                {
                    throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
                }

                //đợt khảo sát chưa kết thúc
                if (!(surveyRound.Status == (int)SurveyRoundStatus.Closed || surveyRound.Status == (int)SurveyRoundStatus.End || DateTime.Now >= surveyRound.EndDate))
                {
                    throw new InvalidInputDataException("Đợt khảo sát chưa đóng hoặc chưa kết thúc");
                }

                //do chỉ có một bài ks nên lấy id của bài ks đó
                var idbaikscuadotnays = surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id).Select(o => o.Id).ToList();
                if (idbaikscuadotnays.Count == 0)
                {
                    throw new RecordNotFoundException("Không tìm thấy bài khảo sát của đợt khảo sát này");
                }

                _logger.LogInformation("report total normal is start.");
                //surveyContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE {TableNameTask.AsEduSurveyReportTotal}");
                var baikshoanthanh = surveyContext.AsEduSurveyBaiKhaoSatSinhVien
                    .Where(o => idbaikscuadotnays.Contains(o.BaiKhaoSatId))
                    .Where(o => o.Status == (int)SurveyStudentStatus.Done);

                var testSoBaiKs = baikshoanthanh.Count();

                var tongBaiKsHoanThanh = baikshoanthanh.Count();
                List<SelectedAnswerExtend> selectedAnswers;

                var groupLopGiangVien = baikshoanthanh
                .GroupBy(o => new { o.LecturerCode, o.ClassRoomCode, o.BaiKhaoSatId, o.Nhhk })
                .Select(r => new { r.Key.LecturerCode, r.Key.ClassRoomCode, r.Key.BaiKhaoSatId, r.Key.Nhhk })
                .ToList();

                var totalLectureClassroom = groupLopGiangVien.Count();
                var count = 0;
                foreach (var lectureClassroom in groupLopGiangVien)
                {
                    //hay lỗi ở lớp không có giảng viên
                    var lectureCode = lectureClassroom.LecturerCode;
                    var classroomCode = lectureClassroom.ClassRoomCode;
                    var nhhk = lectureClassroom.Nhhk;

                    //lọc theo từng giảng viên lớp môn học của từng nhhk
                    var cacBaiLam = baikshoanthanh.Where(o => o.ClassRoomCode == classroomCode && o.LecturerCode == lectureCode && o.Nhhk == nhhk).ToList();
                    selectedAnswers = new List<SelectedAnswerExtend>(); //bài làm đẩy từ json sang

                    foreach (var bailam in cacBaiLam)
                    {
                        if (string.IsNullOrEmpty(bailam.BaiLam))
                        {
                            continue;
                        }
                        //giải chuỗi json bài làm của sinh viên
                        var json = JsonSerializer.Deserialize<List<SelectedAnswerExtend>>(bailam.BaiLam);
                        json.ForEach(o => o.TheSurveyId = bailam.BaiKhaoSatId);
                        selectedAnswers.AddRange(json);
                    }

                    //lấy mã môn học
                    string subjectCode = null;
                    string classroom = null;

                    //bài đầu tiên trong lớp môn học đấy để lấy subject Code và class room code
                    var baiLamDauTien = surveyContext.AsEduSurveyBaiKhaoSatSinhVien
                        .FirstOrDefault(bl => idbaikscuadotnays.Contains(bl.BaiKhaoSatId) && bl.ClassRoomCode == classroomCode 
                            && bl.Nhhk == nhhk && !string.IsNullOrWhiteSpace(bl.SubjectCode));

                    if (baiLamDauTien != null)
                    {
                        subjectCode = baiLamDauTien.SubjectCode;
                        classroom = baiLamDauTien.ClassRoomCode?.Replace(baiLamDauTien.SubjectCode ?? "", "");
                    }

                    //giải đề
                    var deLyThuyetThucHanh = surveyContext.AsEduSurveyBaiKhaoSat.FirstOrDefault(o => o.Id == baiLamDauTien.BaiKhaoSatId);
                    List<QuestionJson> QuestionJsonData = JsonSerializer.Deserialize<List<QuestionJson>>(deLyThuyetThucHanh.NoiDungDeThi);

                    List<AnswerSelectedReportTotal> total = new List<AnswerSelectedReportTotal>();
                    foreach (var question in QuestionJsonData.Where(q => q.Type == QuestionType.SC))
                    {
                        if (question.Answers == null)
                            continue;

                        int testSum = 0;
                        foreach (var answer in question.Answers)
                        {
                            var countAnswer = selectedAnswers.Count(a => a.QuestionCode == question.Code && a.AnswerCode == answer.Code);
                            testSum += countAnswer;

                            if (countAnswer > 0)
                            {
                                total.Add(new Models.Survey.JsonData.AnswerSelectedReportTotal
                                {
                                    TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                                    QuestionCode = question.Code,
                                    AnswerCode = answer.Code,
                                    Total = countAnswer
                                });
                            }
                        }
                    }

                    foreach (var question in QuestionJsonData.Where(q => q.Type == QuestionType.MC))
                    {
                        if (question.Answers == null)
                            continue;

                        int testSum = 0;
                        foreach (var answer in question.Answers)
                        {
                            var countAnswer = selectedAnswers.Count(a => a.QuestionCode == question.Code && a.AnswerCodes != null && a.AnswerCodes.Contains(answer.Code));
                            testSum += countAnswer;

                            total.Add(new Models.Survey.JsonData.AnswerSelectedReportTotal
                            {
                                TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                                QuestionCode = question.Code,
                                AnswerCode = answer.Code,
                                Total = countAnswer
                            });

                            if (answer.AnswerChildQuestion != null)
                            {
                                var cauTraLoiCon = selectedAnswers.Where(a => a.QuestionCode == answer.AnswerChildQuestion.Code);

                                List<string> strAllAnswerContent = new List<string>();

                                foreach (var str in cauTraLoiCon)
                                {
                                    strAllAnswerContent.Add(str.AnswerContent);
                                }

                                var options = new JsonSerializerOptions
                                {
                                    IgnoreNullValues = true,
                                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                                };

                                total.Add(new AnswerSelectedReportTotal
                                {
                                    TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                                    QuestionCode = question.Code,
                                    Content = JsonSerializer.Serialize(strAllAnswerContent, options)
                                });

                                if (cauTraLoiCon != null)
                                {
                                    total.Add(new Models.Survey.JsonData.AnswerSelectedReportTotal
                                    {
                                        TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                                        QuestionCode = answer.AnswerChildQuestion.Code,
                                        Content = string.Join(";", strAllAnswerContent)
                                    });
                                }
                            }
                        }

                        if (testSum != 21)
                        {

                        }
                    }

                    foreach (var questionGQ in QuestionJsonData.Where(q => q.Type == QuestionType.GQ))
                    {
                        foreach (var question in questionGQ.ChildQuestion.Where(q => q.Type == QuestionType.SA))
                        {
                            var cauTraText = selectedAnswers.Where(a => a.QuestionCode == question.Code);
                            List<string> strAllAnswerContent = new List<string>();

                            foreach (var str in cauTraText)
                            {
                                strAllAnswerContent.Add(str.AnswerContent);
                            }

                            var options = new JsonSerializerOptions
                            {
                                IgnoreNullValues = true,
                                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                            };

                            string content = JsonSerializer.Serialize(strAllAnswerContent, options);

                            total.Add(new AnswerSelectedReportTotal
                            {
                                TheSurveyId = baiLamDauTien.BaiKhaoSatId,
                                QuestionCode = question.Code,
                                Content = content
                            });
                        }
                    }

                    //total = AnswerSelectedReportTotal(selectedAnswers);
                    foreach (var item in total)
                    {
                        var thongkecuthe = surveyContext.AsEduSurveyReportTotal
                            .FirstOrDefault(o => o.ClassRoomCode == classroomCode && o.Nhhk == nhhk && o.LecturerCode == lectureCode && o.TheSurveyId == item.TheSurveyId &&
                            o.QuestionCode == item.QuestionCode && o.AnswerCode == item.AnswerCode);
                        if (thongkecuthe == null)
                        {
                            surveyContext.AsEduSurveyReportTotal.Add(new AsEduSurveyReportTotal
                            {
                                Id = Guid.NewGuid(),
                                SurveyRoundId = surveyRound.Id,
                                TheSurveyId = item.TheSurveyId,
                                ClassRoomCode = classroomCode,
                                SubjectCode = subjectCode,
                                ClassRoom = classroom,
                                Nhhk = nhhk,
                                LecturerCode = lectureCode,
                                QuestionCode = item.QuestionCode,
                                AnswerCode = item.AnswerCode,
                                Content = item.Content,
                                Total = item.Total,
                            });
                        }
                        else //nếu có rồi thì cập nhật
                        {
                            thongkecuthe.SubjectCode = subjectCode;
                            thongkecuthe.ClassRoom = classroom;
                            thongkecuthe.Content = item.Content;
                            thongkecuthe.Total = item.Total;
                        }
                    }

                    _logger.LogInformation($"thong ke hoan thanh {++count}/{totalLectureClassroom}, ma gv: {lectureCode}, ma lop: {classroomCode}");
                }
                var test = surveyContext.SaveChanges();
                _logger.LogInformation("report total normal is done.");

                //hoàn thành
                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = true;
                status.Message = null;
                statusContext.SaveChanges();
            }
            catch (Exception e)
            {
                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = false;
                status.Message = UtilsException.GetMainMessage(e);
                statusContext.SaveChanges();
                throw e;
            }
        }

        public async Task ReportTotalNormalSurvey(Guid surveyRoundId)
        {
            var status = await _statusService.GetStatusTableTask(TableNameTask.AsEduSurveyReportTotal);
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Bảng đang làm việc, thao tác bị huỷ");
            }

            var scope = _scopeFactory.CreateScope();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();

            var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            //đợt khảo sát chưa kết thúc
            if (!(surveyRound.Status == (int)SurveyRoundStatus.Closed || surveyRound.Status == (int)SurveyRoundStatus.End || DateTime.Now >= surveyRound.EndDate))
            {
                throw new HandleException.InvalidInputDataException("Đợt khảo sát chưa đóng hoặc chưa kết thúc");
            }

            _backgroundTaskWorker.StartAction(() =>
            {
                ReportTotalNormalSurveyBG(surveyRoundId);
            });
        }
        #endregion

        #region thống kê tạm
        private void TempDataNormalSurveyBG(Guid surveyRoundId)
        {
            var scope = _scopeFactory.CreateScope();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            var eduContext = scope.ServiceProvider.GetRequiredService<EduDataContext>();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();

            var status = statusContext.AsStatusTableTask.FirstOrDefault(o => o.TableName == TableNameTask.TempDataNormalSurvey);
            if (status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng thống kê khảo sát sinh viên");
            }
            //bảng đang làm việc
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Đang thống kê, thao tác bị huỷ");
            }
            status.Status = (int)TableTaskStatus.Doing;
            statusContext.SaveChanges();

            try
            {
                var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
                if (surveyRound == null)
                {
                    throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
                }

                var idbaikscuadotnay = surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id).Select(o => o.Id).ToList();
                if (idbaikscuadotnay.Count == 0)
                {
                    throw new RecordNotFoundException("Không tìm thấy bài khảo sát của đợt khảo sát này");
                }

                var tatCaBaiKsCuaDotNay = surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => idbaikscuadotnay.Contains(o.BaiKhaoSatId));
                var result = new TempDataNormal();
                result.TongHopKhoa = new List<TotalFaculty>();
                result.ThoiGianKetThuc = surveyRound.EndDate;
                result.NgayHienTai = DateTime.Now;

                _logger.LogInformation($"Bat dau thong ke tam");
                var facultys = eduContext.AsAcademyFaculty.ToList();
                var tongSVDathamGiaKS = 0;
                var tongSVToanTruong = 0;
                foreach (var f in facultys)
                {
                    _logger.LogInformation($"Đang thong ke tam cho khoa co ma {f.Code}");
                    //thống kê theo lớp quản lý
                    var classF = eduContext.AsAcademyClass.Where(o => o.FacultyCode == f.Code).Select(o => o.Code).ToList();

                    //tất cả sv có đk
                    var allStudents = eduContext.AsAcademyStudent
                        .Where(o => classF.Contains(o.ClassCode))
                        .Where(o => eduContext.AsAcademyStudentClassRoom.FirstOrDefault(sc => sc.StudentCode == o.Code) != null)
                        .Select(o => o.Code)
                        .ToList();

                    //bài ks sinh viên của khoa này
                    var tatCaBaiKs = tatCaBaiKsCuaDotNay.Where(o => allStudents.Contains(o.StudentCode));

                    var tongSoSinhVien = allStudents.Count();
                    if(tongSoSinhVien == 0)
                    {
                        continue;
                    }
                    tongSVToanTruong += tongSoSinhVien;
                    var soSvDaLamBai = tatCaBaiKs.Where(o => o.Status == (int)SurveyStudentStatus.Done).GroupBy(o => o.StudentCode).Select(o => o.Key).Count();
                    tongSVDathamGiaKS += soSvDaLamBai;
                    var soSvChuaLamBai = tongSoSinhVien - soSvDaLamBai;

                    //số bài ks được phát
                    //var total = tatCaBaiKs.Count();

                    //số bài hoàn thành
                    //var num = tatCaBaiKs.Count(o => o.Status == (int)SurveyStudentStatus.Done);

                    result.TongHopKhoa.Add(new TotalFaculty
                    {
                        FacultyCode = f.Code,
                        FacultyName = f.Name,
                        TotalDaLam = soSvDaLamBai,
                        TotalChuaLam = soSvChuaLamBai,
                        TotalSinhVien = tongSoSinhVien,
                        Percent = $"{(double)soSvDaLamBai/(tongSoSinhVien > 0 ? tongSoSinhVien : 1) * 100:##,0}"
                    });
                }
                _logger.LogInformation($"Thong ke tam hoan thanh");

                result.SoSVKhaoSat = tongSVDathamGiaKS;
                result.ChiemTiLe = $"{(double)tongSVDathamGiaKS /(tongSVToanTruong > 0 ? tongSVToanTruong : 1) * 100:##,0}";

                //hoàn thành
                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = true;

                status.Message = JsonSerializer.Serialize(result);
                statusContext.SaveChanges();
            }
            catch (Exception e)
            {
                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = false;
                status.Message = e.Message;
                statusContext.SaveChanges();
                throw e;
            }
        }

        public async Task TempDataNormalSurvey(Guid surveyRoundId)
        {
            var status = await _statusService.GetStatusTableTask(TableNameTask.TempDataNormalSurvey);
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Đang thống kê tạm, thao tác bị huỷ");
            }

            var scope = _scopeFactory.CreateScope();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();

            var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            _backgroundTaskWorker.StartAction(() =>
            {
                TempDataNormalSurveyBG(surveyRoundId);
            });
        }
        #endregion

        #region kết xuất làm báo cáo
        private void StyleExcelExport(ExcelWorksheet worksheet)
        {
            worksheet.DefaultRowHeight = 14.25;
            worksheet.DefaultColWidth = 8.5;

            worksheet.Cells.Style.WrapText = true;

            //worksheet.Cells.Style.Font.Name = "Arial";
            worksheet.Cells.Style.Font.Size = 11;

            worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Row(2).Height = 67.5;
            worksheet.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(2).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Row(3).Height = 30;

            worksheet.Column(1).Width = 6.26;
            worksheet.Column(1).Width = 46.86;
            worksheet.Column(2).Width = 46;
            worksheet.Column(3).Width = 30;
            worksheet.Column(4).Width = 9;
            worksheet.Column(5).Width = 30;
            worksheet.Column(6).Width = 30;

            worksheet.Cells["A2"].Value = "STT";
            worksheet.Cells["B2"].Value = "Khoa/Ban";
            worksheet.Cells["C2"].Value = "Bộ môn";
            worksheet.Cells["D2"].Value = "Mã học phần";
            worksheet.Cells["E2"].Value = "Tên học phần";

            worksheet.Cells["F2"].Value = "Giảng viên";
            worksheet.Cells["G2"].Value = "Mã lớp học phần";
            worksheet.Cells["H2"].Value = "Số phiếu phát ra";
            worksheet.Cells["I2"].Value = "Số phiếu thu về";
        }

        private void StyleShortAnswerExcelExport(ExcelWorksheet worksheet)
        {
            worksheet.DefaultRowHeight = 14.25;
            worksheet.DefaultColWidth = 40;

            worksheet.Cells.Style.WrapText = true;

            //worksheet.Cells.Style.Font.Name = "Arial";
            worksheet.Cells.Style.Font.Size = 11;

            worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Column(1).Width = 33;
            worksheet.Column(2).Width = 33;
            worksheet.Column(3).Width = 33;
            worksheet.Column(4).Width = 50;
            worksheet.Column(5).Width = 30;

            worksheet.Cells["A1"].Value = "Khoa";
            worksheet.Cells["B1"].Value = "Bộ môn";
            worksheet.Cells["C1"].Value = "Giảng viên";
            worksheet.Cells["D1"].Value = "Học phần";
            worksheet.Cells["E1"].Value = "Lớp môn học";
        }

        private void ThongKeTungLoaiBaiKS(ExcelWorksheet wsLyThuyet, SurveyContext surveyContext, List<AsEduSurveyBaiKhaoSat> baiKhaoSats,
            List<QuestionJson> deLyThuyet, int loaiMon)
        {
            surveyContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var baiKhaoSatIds = baiKhaoSats.Select(o => o.Id).ToList();
            var baiLamKhaoSatCacDotDangXet = surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => baiKhaoSatIds.Contains(o.BaiKhaoSatId));

            //track
            baiLamKhaoSatCacDotDangXet.ToArray();
            surveyContext.AsEduSurveyReportTotal.ToArray();

            //lấy khoa
            int row = 4;
            int rowCauHoi = 2;
            int rowDapAn = 3;
            int col = 1;
            var facultys = surveyContext.AsAcademyFaculty.
                            OrderBy(o => o.Order).ToList();
                            //.Where(o => o.Code == "KM");

            //row col câu trả lời ngắn
            int rowSA = 2;
            int colSA = 1;

            var baiKhaoSatLyThuyetIds = baiKhaoSats.Where(o => o.Type == loaiMon).Select(o => o.Id).ToList();
            //lọc theo bài khảo sát
            var reportTotalLoaiMonQuery = surveyContext.AsEduSurveyReportTotal.Where(o => baiKhaoSatLyThuyetIds.Contains(o.TheSurveyId));

            //int test = 0;

            var soLuaChonToanTruong = new Dictionary<int, float>();
            foreach (var faculty in facultys) //lặp khoa
            {
                _logger.LogInformation($"Dang ket xuat loai {loaiMon} khoa co ma {faculty.Code}");
                //lấy bộ môn
                var departments = surveyContext.AsAcademyDepartment.Where(o => o.FacultyCode == faculty.Code).ToList();

                //tính ra dòng tổng của khoa
                var soLuaChonKhoa = new Dictionary<int, float>();

                //lấy môn học của bộ môn
                foreach (var department in departments) //lặp bộ môn
                {
                    _logger.LogInformation($"Dang ket xuat loai {loaiMon} bo mon co ma {department.Code} cua khoa {faculty.Code}");
                    //mã giảng viên dùng cho các cột đếm số lượng sv, gv từng bộ môn
                    var lecturers = surveyContext.AsAcademyLecturer.Where(o => o.DepartmentCode == department.Code).ToList();
                    var lecturerCodes = lecturers.Select(l => l.Code).ToList();

                    var monHocs = surveyContext.AsAcademySubject.Where(s => s.DepartmentCode == department.Code)
                        .Join(surveyContext.AsAcademySubjectExtend, subject => subject.Code, extend => extend.Code,
                        (subject, extend) => new
                        {
                            subject,
                            extend
                        }).Where(o => o.extend.Type == loaiMon).ToList(); //môn học có loại đang xét

                    var maMonHocs = monHocs.Select(o => o.subject.Code).ToList(); //lấy mã môn

                    var maLopMonHocs = surveyContext.AsAcademyClassRoom
                        .Join(surveyContext.AsAcademyLecturerClassRoom, classroom => classroom.Code, lectureclassroom => lectureclassroom.ClassRoomCode,
                            (classRoom, lecturerClassRoom) => new
                            {
                                classRoom,
                                lecturerClassRoom,
                            })
                        .Where(o => maMonHocs.Contains(o.classRoom.SubjectCode))
                        .OrderBy(o => o.classRoom.SubjectCode) //sắp xếp theo mã môn học tăng dần
                        .Select(o => new { ClassRoomCode = o.classRoom.Code, o.lecturerClassRoom.LecturerCode, o.classRoom.SubjectCode })
                        .Distinct().ToList();

                    #region thống kê
                    //tính ra dòng tổng của khoa
                    var soLuaChonBoMon = new Dictionary<int, float>();

                    int stt = 1;

                    //thống kê theo lớp môn học của bộ môn
                    for (int indexMonHoc = 0; indexMonHoc < maLopMonHocs.Count; indexMonHoc++)
                    {
                        var lopMonHoc = maLopMonHocs[indexMonHoc];

                        var reportTotalLopMonQuery = reportTotalLoaiMonQuery.Where(o => o.ClassRoomCode == lopMonHoc.ClassRoomCode).ToList();
                        var firstRowReport = reportTotalLopMonQuery.FirstOrDefault(o => !string.IsNullOrEmpty(o.LecturerCode));

                        var baiLamKhaoSats = baiLamKhaoSatCacDotDangXet.Where(o => o.ClassRoomCode == lopMonHoc.ClassRoomCode);

                        if (baiLamKhaoSats.Count() == 0 || baiLamKhaoSats == null)
                        {
                            continue;
                        }

                        col = 1;
                        wsLyThuyet.Cells[row, col++].Value = stt++;

                        #region đếm số lượng
                        //các bài làm của lớp môn đang xét
                        var baiLamKhaoSatHoanThanhs = baiLamKhaoSats.Where(o => o.Status == (int)SurveyStudentStatus.Done);

                        var soPhieuPhatRa = baiLamKhaoSats.Count();
                        var soPhieuThuVe = baiLamKhaoSatHoanThanhs.Count();
                        #endregion

                        wsLyThuyet.Cells[row, col++].Value = faculty.Name; //tên khoa
                        wsLyThuyet.Cells[row, col++].Value = department.Name; //tên bộ môn
                        wsLyThuyet.Cells[row, col++].Value = lopMonHoc.SubjectCode; //mã học phần

                        string tenMonHoc = "";
                        var monHocFind = surveyContext.AsAcademySubject.FirstOrDefault(o => o.Code == lopMonHoc.SubjectCode);
                        if (monHocFind != null)
                        {
                            tenMonHoc = monHocFind.Name;
                        }

                        wsLyThuyet.Cells[row, col++].Value = tenMonHoc;

                        string maGiangVien = "";
                        string tenGiangVien = "";

                        if (firstRowReport != null)
                        {
                            maGiangVien = firstRowReport.LecturerCode;
                            var giangVien = surveyContext.AsAcademyLecturer.FirstOrDefault(o => o.Code == maGiangVien);
                            if (giangVien != null)
                            {
                                tenGiangVien = giangVien.FullName;
                            }
                        }

                        wsLyThuyet.Cells[row, col++].Value = tenGiangVien;

                        wsLyThuyet.Cells[row, col++].Value = lopMonHoc.ClassRoomCode?.Replace(lopMonHoc.SubjectCode, ""); //tên lớp học phần


                        //số phiếu phát ra bộ môn
                        if (!soLuaChonBoMon.ContainsKey(col))
                            soLuaChonBoMon.Add(col, 0);
                        else
                            soLuaChonBoMon[col] += soPhieuPhatRa;

                        //số phiếu phát ra khoa
                        if (!soLuaChonKhoa.ContainsKey(col))
                            soLuaChonKhoa.Add(col, 0);
                        else
                            soLuaChonKhoa[col] += soPhieuPhatRa;

                        //số phiếu phát ra toàn trường
                        if (!soLuaChonToanTruong.ContainsKey(col))
                            soLuaChonToanTruong.Add(col, 0);
                        else
                            soLuaChonToanTruong[col] += soPhieuPhatRa;

                        wsLyThuyet.Cells[row, col++].Value = soPhieuPhatRa;



                        //số phiếu thu về ra bộ môn
                        if (!soLuaChonBoMon.ContainsKey(col))
                            soLuaChonBoMon.Add(col, 0);
                        else
                            soLuaChonBoMon[col] += soPhieuThuVe;

                        //số phiếu thu về ra khoa
                        if (!soLuaChonKhoa.ContainsKey(col))
                            soLuaChonKhoa.Add(col, 0);
                        else
                            soLuaChonKhoa[col] += soPhieuThuVe;

                        //số phiếu thu về toàn trường
                        if (!soLuaChonToanTruong.ContainsKey(col))
                            soLuaChonToanTruong.Add(col, 0);
                        else
                            soLuaChonToanTruong[col] += soPhieuThuVe;

                        wsLyThuyet.Cells[row, col++].Value = soPhieuThuVe;



                        int index = 0;
                        //lặp câu hỏi
                        foreach (var cauhoi in deLyThuyet)
                        {
                            if (cauhoi.Type == QuestionType.SC)
                            {
                                var colStart = col;
                                wsLyThuyet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoi.Content}";

                                var dTB = 0;
                                var sumTotal = 0;
                                var diem = 0;
                                var colTotal = col + cauhoi.Answers.Count();
                                foreach (var dapan in cauhoi.Answers)
                                {
                                    diem++;

                                    //tổng bộ môn
                                    if (!soLuaChonBoMon.ContainsKey(col))
                                    {
                                        soLuaChonBoMon.Add(col, 0);
                                    }

                                    //tổng khoa
                                    if (!soLuaChonKhoa.ContainsKey(col))
                                    {
                                        soLuaChonKhoa.Add(col, 0);
                                    }

                                    //tổng toàn trường
                                    if (!soLuaChonToanTruong.ContainsKey(col))
                                    {
                                        soLuaChonToanTruong.Add(col, 0);
                                    }

                                    wsLyThuyet.Cells[rowDapAn, col].Value = dapan.Content;
                                    var ketqua = reportTotalLopMonQuery.FirstOrDefault(o => o.QuestionCode == cauhoi.Code && o.AnswerCode == dapan.Code);
                                    if (ketqua != null)
                                    {
                                        var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                        wsLyThuyet.Cells[row, col].Value = total;
                                        sumTotal += total;

                                        //tổng bộ môn
                                        soLuaChonBoMon[col] += total;

                                        //tổng khoa
                                        soLuaChonKhoa[col] += total;

                                        //tổng toàn trường
                                        soLuaChonToanTruong[col] += total;

                                        dTB += diem * total;
                                    }
                                    else
                                    {
                                        wsLyThuyet.Cells[row, col].Value = 0;
                                    }
                                    col++;
                                }
                                wsLyThuyet.Cells[rowDapAn, col].Value = "đTB";

                                if (sumTotal > 0)
                                {
                                    wsLyThuyet.Cells[row, col].Value = (double)dTB / sumTotal;
                                    //format cell number
                                    wsLyThuyet.Cells[row, col].Style.Numberformat.Format = "0.00";
                                }
                                else
                                {
                                    wsLyThuyet.Cells[row, col].Value = 0;
                                }

                                //tổng hợp toàn trường
                                float nhanDiemToanTruong = 0, nhanDiemKhoa = 0, nhanDiemBoMon = 0;
                                float sumSoLuaChonToanTruong = 0, sumSoLuaChonKhoa = 0, sumSoLuaChonBoMon = 0;
                                int totalDiem = (colTotal) - (colTotal - cauhoi.Answers.Count); //từ 5 ve 1
                                for (int i = colTotal - 1; i >= colTotal - cauhoi.Answers.Count; i--)
                                {
                                    sumSoLuaChonBoMon += soLuaChonBoMon[i];
                                    nhanDiemBoMon += soLuaChonBoMon[i] * totalDiem;

                                    sumSoLuaChonKhoa += soLuaChonKhoa[i];
                                    nhanDiemKhoa += soLuaChonKhoa[i] * totalDiem;

                                    sumSoLuaChonToanTruong += soLuaChonToanTruong[i];
                                    nhanDiemToanTruong += soLuaChonToanTruong[i] * totalDiem;
                                    totalDiem--;
                                }

                                //tổng hợp tb khoa
                                if (sumSoLuaChonBoMon > 0)
                                {
                                    if (soLuaChonBoMon.ContainsKey(colTotal))
                                    {
                                        soLuaChonBoMon[colTotal] = (float)nhanDiemBoMon / sumSoLuaChonBoMon;
                                    }
                                    else
                                    {
                                        soLuaChonBoMon.Add(colTotal, (float)nhanDiemBoMon / sumSoLuaChonBoMon);
                                    }
                                }
                                else
                                {
                                    if (soLuaChonBoMon.ContainsKey(colTotal))
                                    {
                                        soLuaChonBoMon[colTotal] = 0;
                                    }
                                    else
                                    {
                                        soLuaChonBoMon.Add(colTotal, 0);
                                    }
                                }

                                //tổng hợp tb khoa
                                if (sumSoLuaChonKhoa > 0)
                                {
                                    if (soLuaChonKhoa.ContainsKey(colTotal))
                                    {
                                        soLuaChonKhoa[colTotal] = (float)nhanDiemKhoa / sumSoLuaChonKhoa;
                                    }
                                    else
                                    {
                                        soLuaChonKhoa.Add(colTotal, (float)nhanDiemKhoa / sumSoLuaChonKhoa);
                                    }
                                }
                                else
                                {
                                    if (soLuaChonKhoa.ContainsKey(colTotal))
                                    {
                                        soLuaChonKhoa[colTotal] = 0;
                                    }
                                    else
                                    {
                                        soLuaChonKhoa.Add(colTotal, 0);
                                    }
                                }

                                //tổng tb toàn trường
                                if (sumSoLuaChonToanTruong > 0)
                                {
                                    if (soLuaChonToanTruong.ContainsKey(colTotal))
                                    {
                                        soLuaChonToanTruong[colTotal] = (float)nhanDiemToanTruong / sumSoLuaChonToanTruong;
                                    }
                                    else
                                    {
                                        soLuaChonToanTruong.Add(colTotal, (float)nhanDiemToanTruong / sumSoLuaChonToanTruong);
                                    }
                                }
                                else
                                {
                                    if (soLuaChonToanTruong.ContainsKey(colTotal))
                                    {
                                        soLuaChonToanTruong[colTotal] = 0;
                                    }
                                    else
                                    {
                                        soLuaChonToanTruong.Add(colTotal, 0);
                                    }
                                }

                                var colEnd = col++;
                                wsLyThuyet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                            }
                            else if (cauhoi.Type == QuestionType.MC)
                            {
                                var colStart = col;
                                wsLyThuyet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoi.Content}";
                                wsLyThuyet.Cells[rowCauHoi, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                wsLyThuyet.Cells[rowCauHoi, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                                foreach (var dapan in cauhoi.Answers)
                                {
                                    //tổng hợp bộ môn
                                    if (!soLuaChonBoMon.ContainsKey(col))
                                    {
                                        soLuaChonBoMon.Add(col, 0);
                                    }

                                    //tổng hợp khoa
                                    if (!soLuaChonKhoa.ContainsKey(col))
                                    {
                                        soLuaChonKhoa.Add(col, 0);
                                    }

                                    //tổng toàn trường
                                    if (!soLuaChonToanTruong.ContainsKey(col))
                                    {
                                        soLuaChonToanTruong.Add(col, 0);
                                    }

                                    wsLyThuyet.Cells[rowDapAn, col].Value = dapan.Content;
                                    var ketqua = reportTotalLopMonQuery.FirstOrDefault(o => o.QuestionCode == cauhoi.Code && o.AnswerCode == dapan.Code);
                                    if (ketqua != null)
                                    {
                                        var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                        wsLyThuyet.Cells[row, col].Value = total;

                                        //tổng hợp khoa
                                        soLuaChonBoMon[col] += total;

                                        //tổng hợp khoa
                                        soLuaChonKhoa[col] += total;

                                        //tổng toàn trường
                                        soLuaChonToanTruong[col] += total;
                                    }
                                    else
                                    {
                                        wsLyThuyet.Cells[row, col].Value = 0;
                                    }

                                    //câu hỏi con của đáp án
                                    if (dapan.AnswerChildQuestion != null)
                                    {
                                        col++;
                                        wsLyThuyet.Cells[rowDapAn, col].Value = dapan.AnswerChildQuestion.Content;
                                        var ketquaCon = reportTotalLopMonQuery.FirstOrDefault(o => o.QuestionCode == $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}" && o.AnswerCode == dapan.Code);
                                        wsLyThuyet.Cells[row, col].Value = ketquaCon?.Content ?? "";
                                    }
                                    col++;
                                }
                                var colEnd = col - 1;
                                wsLyThuyet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                            }
                            else if (cauhoi.Type == QuestionType.SA)
                            {
                                wsLyThuyet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoi.Content}";
                                wsLyThuyet.Column(col).Width = 48;
                                wsLyThuyet.Row(row).Height = 30;
                                var ketqua = reportTotalLopMonQuery.FirstOrDefault(o => o.QuestionCode == cauhoi.Code);
                                if (ketqua != null && ketqua.Content != null)
                                {
                                    var str = "";
                                    var listStr = JsonSerializer.Deserialize<List<string>>(ketqua.Content);
                                    var tenGV = "";
                                    var tempGV = surveyContext.AsAcademyLecturer.FirstOrDefault(l => l.Code == ketqua.LecturerCode);
                                    if (tempGV != null)
                                    {
                                        tenGV = tempGV.FullName;
                                    }
                                    str += $"- {ketqua.LecturerCode}-{tenGV} - lớp {ketqua.ClassRoomCode} :\n";
                                    foreach (var s in listStr)
                                    {
                                        if (string.IsNullOrWhiteSpace(s))
                                            continue;
                                        str += $"\t+ {s}\n";
                                    }
                                    wsLyThuyet.Cells[row, col].Value = str;
                                }
                                col++;
                            }
                            else if (cauhoi.Type == QuestionType.GQ)
                            {
                                index++;
                                var indexChild = 0;

                                foreach (var cauhoicon in cauhoi.ChildQuestion)
                                {
                                    #region comment
                                    //if (cauhoicon.Type == QuestionType.SC)
                                    //{
                                    //    var colStart = col;
                                    //    wsLyThuyet.Cells[rowCauHoi, col].Value = $"Câu {index}.{++indexChild}: {cauhoicon.Content}";
                                    //    var dTB = 0;
                                    //    var sumTotal = 0;
                                    //    var diem = 0;
                                    //    var colTotal = col + cauhoicon.Answers.Count();
                                    //    foreach (var dapan in cauhoicon.Answers)
                                    //    {
                                    //        diem++;

                                    //        //tổng hợp bộ môn
                                    //        if (!soLuaChonBoMon.ContainsKey(col))
                                    //        {
                                    //            soLuaChonBoMon.Add(col, 0);
                                    //        }

                                    //        //tổng hợp khoa
                                    //        if (!soLuaChonKhoa.ContainsKey(col))
                                    //        {
                                    //            soLuaChonKhoa.Add(col, 0);
                                    //        }

                                    //        //tổng toàn trường
                                    //        if (!soLuaChonToanTruong.ContainsKey(col))
                                    //        {
                                    //            soLuaChonToanTruong.Add(col, 0);
                                    //        }

                                    //        wsLyThuyet.Cells[rowDapAn, col].Value = dapan.Content;
                                    //        var ketqua = reportTotalLopMonQuery.FirstOrDefault(o => o.QuestionCode == cauhoicon.Code && o.AnswerCode == dapan.Code);
                                    //        if (ketqua != null)
                                    //        {
                                    //            var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                    //            wsLyThuyet.Cells[row, col].Value = total;
                                    //            sumTotal += total;

                                    //            //tổng hợp bộ môn
                                    //            soLuaChonBoMon[col] += total;

                                    //            //tổng hợp khoa
                                    //            soLuaChonKhoa[col] += total;

                                    //            //tổng toàn trường
                                    //            soLuaChonToanTruong[col] += total;

                                    //            dTB += diem * total;
                                    //        }
                                    //        else
                                    //        {
                                    //            wsLyThuyet.Cells[row, col].Value = 0;
                                    //        }
                                    //        col++;
                                    //    }
                                    //    wsLyThuyet.Cells[rowDapAn, col].Value = "đTB";

                                    //    if (sumTotal > 0)
                                    //    {
                                    //        wsLyThuyet.Cells[row, col].Value = (double)dTB / sumTotal;
                                    //    }
                                    //    else
                                    //    {
                                    //        wsLyThuyet.Cells[row, col].Value = 0;
                                    //    }

                                    //    //tổng hợp toàn trường
                                    //    float nhanDiemToanTruong = 0, nhanDiemKhoa = 0, nhanDiemBoMon = 0;
                                    //    float sumSoLuaChonToanTruong = 0, sumSoLuaChonKhoa = 0, sumSoLuaChonBoMon = 0;
                                    //    int totalDiem = (colTotal) - (colTotal - cauhoicon.Answers.Count);
                                    //    for (int i = colTotal - 1; i >= colTotal - cauhoicon.Answers.Count; i--)
                                    //    {
                                    //        sumSoLuaChonBoMon += soLuaChonBoMon[i];
                                    //        nhanDiemBoMon += soLuaChonBoMon[i] * totalDiem;

                                    //        sumSoLuaChonKhoa += soLuaChonKhoa[i];
                                    //        nhanDiemKhoa += soLuaChonKhoa[i] * totalDiem;

                                    //        sumSoLuaChonToanTruong += soLuaChonToanTruong[i];
                                    //        nhanDiemToanTruong += soLuaChonToanTruong[i] * totalDiem;
                                    //        totalDiem--;
                                    //    }

                                    //    //tổng hợp tb bộ môn
                                    //    if (sumSoLuaChonBoMon > 0)
                                    //    {
                                    //        if (soLuaChonBoMon.ContainsKey(colTotal))
                                    //        {
                                    //            soLuaChonBoMon[colTotal] = (float)nhanDiemBoMon / sumSoLuaChonBoMon;
                                    //        }
                                    //        else
                                    //        {
                                    //            soLuaChonBoMon.Add(colTotal, (float)nhanDiemBoMon / sumSoLuaChonBoMon);
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (soLuaChonBoMon.ContainsKey(colTotal))
                                    //        {
                                    //            soLuaChonBoMon[colTotal] = 0;
                                    //        }
                                    //        else
                                    //        {
                                    //            soLuaChonBoMon.Add(colTotal, 0);
                                    //        }
                                    //    }

                                    //    //tổng hợp tb khoa
                                    //    if (sumSoLuaChonKhoa > 0)
                                    //    {
                                    //        if (soLuaChonKhoa.ContainsKey(colTotal))
                                    //        {
                                    //            soLuaChonKhoa[colTotal] = (float)nhanDiemKhoa / sumSoLuaChonKhoa;
                                    //        }
                                    //        else
                                    //        {
                                    //            soLuaChonKhoa.Add(colTotal, (float)nhanDiemKhoa / sumSoLuaChonKhoa);
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (soLuaChonKhoa.ContainsKey(colTotal))
                                    //        {
                                    //            soLuaChonKhoa[colTotal] = 0;
                                    //        }
                                    //        else
                                    //        {
                                    //            soLuaChonKhoa.Add(colTotal, 0);
                                    //        }
                                    //    }

                                    //    //tổng tb toàn trường
                                    //    if (sumSoLuaChonToanTruong > 0)
                                    //    {
                                    //        if (soLuaChonToanTruong.ContainsKey(colTotal))
                                    //        {
                                    //            soLuaChonToanTruong[colTotal] = (float)nhanDiemToanTruong / sumSoLuaChonToanTruong;
                                    //        }
                                    //        else
                                    //        {
                                    //            soLuaChonToanTruong.Add(colTotal, (float)nhanDiemToanTruong / sumSoLuaChonToanTruong);
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (soLuaChonToanTruong.ContainsKey(colTotal))
                                    //        {
                                    //            soLuaChonToanTruong[colTotal] = 0;
                                    //        }
                                    //        else
                                    //        {
                                    //            soLuaChonToanTruong.Add(colTotal, 0);
                                    //        }
                                    //    }

                                    //    var colEnd = col++;
                                    //    wsLyThuyet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                                    //}
                                    //else if (cauhoicon.Type == QuestionType.MC)
                                    //{
                                    //    var colStart = col;
                                    //    wsLyThuyet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoicon.Content}";
                                    //    wsLyThuyet.Cells[rowCauHoi, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    //    wsLyThuyet.Cells[rowCauHoi, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                                    //    foreach (var dapan in cauhoicon.Answers)
                                    //    {
                                    //        //tổng hợp bộ môn
                                    //        if (!soLuaChonBoMon.ContainsKey(col))
                                    //        {
                                    //            soLuaChonBoMon.Add(col, 0);
                                    //        }

                                    //        //tổng hợp khoa
                                    //        if (!soLuaChonKhoa.ContainsKey(col))
                                    //        {
                                    //            soLuaChonKhoa.Add(col, 0);
                                    //        }

                                    //        //tổng toàn trường
                                    //        if (!soLuaChonToanTruong.ContainsKey(col))
                                    //        {
                                    //            soLuaChonToanTruong.Add(col, 0);
                                    //        }

                                    //        wsLyThuyet.Cells[rowDapAn, col].Value = dapan.Content;
                                    //        var ketqua = reportTotalLopMonQuery.FirstOrDefault(o => o.QuestionCode == cauhoicon.Code && o.AnswerCode == dapan.Code);
                                    //        if (ketqua != null)
                                    //        {
                                    //            var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                    //            wsLyThuyet.Cells[row, col].Value = total;

                                    //            //tổng hợp bộ môn
                                    //            soLuaChonBoMon[col] += total;

                                    //            //tổng hợp khoa
                                    //            soLuaChonKhoa[col] += total;

                                    //            //tổng toàn trường
                                    //            soLuaChonToanTruong[col] += total;
                                    //        }
                                    //        else
                                    //        {
                                    //            wsLyThuyet.Cells[row, col].Value = 0;
                                    //        }

                                    //        //câu hỏi con của đáp án
                                    //        if (dapan.AnswerChildQuestion != null)
                                    //        {
                                    //            col++;
                                    //            wsLyThuyet.Cells[rowDapAn, col].Value = dapan.AnswerChildQuestion.Content;
                                    //            var ketquaCon = reportTotalLopMonQuery.FirstOrDefault(o => o.QuestionCode == $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}" && o.AnswerCode == dapan.Code);
                                    //            wsLyThuyet.Cells[row, col].Value = ketquaCon?.Content ?? "";
                                    //        }
                                    //        col++;
                                    //    }
                                    //    var colEnd = col - 1;
                                    //    wsLyThuyet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                                    //}
                                    #endregion comment
                                    if (cauhoicon.Type == QuestionType.SA) // xuất câu hỏi trả lời ngắn
                                    {
                                        wsLyThuyet.Cells[rowCauHoi, col].Value = $"{cauhoi.Content}: {cauhoicon.Content}";
                                        wsLyThuyet.Column(col).Width = 48;
                                        wsLyThuyet.Row(row).Height = 30;

                                        var bailam = reportTotalLopMonQuery.FirstOrDefault(o => o.QuestionCode == cauhoicon.Code);
                                        if (bailam != null)
                                        {
                                            var str = "";
                                            var listStr = JsonSerializer.Deserialize<List<string>>(bailam.Content);
                                            foreach (var s in listStr)
                                            {
                                                if (string.IsNullOrWhiteSpace(s))
                                                    continue;
                                                str += $"{s};";
                                            }
                                            wsLyThuyet.Cells[row, col].Value = str;
                                        }
                                    }
                                    col++;
                                }
                            }
                        }
                        row++;
                    }

                    
                    #endregion
                    col = 1;
                    wsLyThuyet.Row(row).Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wsLyThuyet.Row(row).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                    //hiện tổng bộ môn

                    foreach (var item in soLuaChonBoMon)
                    {
                        wsLyThuyet.Cells[row, item.Key].Value = item.Value;
                        wsLyThuyet.Cells[row, item.Key].Style.Numberformat.Format = "0.00";
                    }

                    row++;
                    #region thống kê câu hỏi mở 
                    //var questionSA = deLyThuyet.Where(q => q.Type == QuestionType.GQ)
                    //    .Select(q => new {
                    //        q.Content,
                    //        ChildQuestion = q.ChildQuestion.Where(c => c.Type == QuestionType.SA) 
                    //    })
                    //    .SelectMany(o => o.ChildQuestion, (container, child) => new {
                    //        child.Code, Content = container.Content + " - " + child.Content
                    //    });
                    //var questionSACode = questionSA.Select(q => q.Code).ToList();

                    
                    ////hiện nội dung câu hỏi
                    //int colCauHoi = 6; //bắt đầu cột câu hỏi
                    //foreach(var q in questionSA)
                    //{
                    //    wsHoiMo.Cells[1, colCauHoi++].Value = q.Content;
                    //}

                    ////lăp qua từng giảng viên
                    //foreach (var lecturer in lecturers)
                    //{
                    //    //lọc ra câu hỏi mở group theo lớp môn học
                    //    var totalSA = reportTotalLoaiMonQuery.Where(t => t.LecturerCode == lecturer.Code)
                    //        .Where(t => questionSACode.Contains(t.QuestionCode))
                    //        .AsEnumerable()
                    //        .GroupBy(g => new { g.ClassRoomCode, g.Nhhk, g.SubjectCode, g.ClassRoom })
                    //        .Select(g => new { g.Key, baiLam = g.Select(q => new { q.QuestionCode, q.Content }) })
                    //        .ToList();

                    //    string subjectName = null;
                    //    string classroom = null;

                    //    if (totalSA.Count == 0)
                    //    {
                    //        continue;
                    //    }

                    //    //theo lớp môn học
                    //    foreach (var item in totalSA)
                    //    {
                    //        colSA = 1; //reset về đầu
                    //        wsHoiMo.Cells[rowSA, colSA++].Value = faculty.Name;
                    //        wsHoiMo.Cells[rowSA, colSA++].Value = department.Name;
                    //        wsHoiMo.Cells[rowSA, colSA++].Value = lecturer.FullName;

                    //        var subject = surveyContext.AsAcademySubject.FirstOrDefault(a => a.Code == item.Key.SubjectCode);
                    //        if (subject != null)
                    //        {
                    //            subjectName = subject.Name;
                    //        }
                    //        classroom = item.Key.ClassRoom;

                    //        wsHoiMo.Cells[rowSA, colSA++].Value = subjectName;
                    //        wsHoiMo.Cells[rowSA, colSA++].Value = classroom;

                    //        //lặp qua từng câu hỏi
                    //        foreach (var q in questionSACode)
                    //        {
                    //            var bailam = item.baiLam.FirstOrDefault(o => o.QuestionCode == q);
                    //            if (bailam != null)
                    //            {
                    //                var str = "";
                    //                var listStr = JsonSerializer.Deserialize<List<string>>(bailam.Content);
                    //                foreach (var s in listStr)
                    //                {
                    //                    if (string.IsNullOrWhiteSpace(s))
                    //                        continue;
                    //                    str += $"{s};";
                    //                }
                    //                wsHoiMo.Cells[rowSA, colSA].Value = str;
                    //            }
                    //            colSA++;
                    //        }
                    //    }
                        
                    //    rowSA++;
                    //}
                    #endregion
                }

                #region tổng số
                col = 1;
                wsLyThuyet.Row(row).Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsLyThuyet.Row(row).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(91, 155, 213));
                wsLyThuyet.Cells[row, col++].Value = $"{faculty.Name}({faculty.Code})";

                foreach(var item in soLuaChonKhoa)
                {
                    wsLyThuyet.Cells[row, item.Key].Value = item.Value;
                    wsLyThuyet.Cells[row, item.Key].Style.Numberformat.Format = "0.00";
                }
                row++;
                #endregion

                //test++;
                //if (test == 3)
                //{
                //    break;
                //}
            }
            col = 1;
            wsLyThuyet.Row(row).Style.Fill.PatternType = ExcelFillStyle.Solid;
            wsLyThuyet.Row(row).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(91, 155, 213));
            wsLyThuyet.Cells[row, col].Value = "Toàn trường";
            foreach (var item in soLuaChonToanTruong)
            {
                wsLyThuyet.Cells[row, item.Key].Value = item.Value;
                wsLyThuyet.Cells[row, item.Key].Style.Numberformat.Format = "0.00";
            }

            wsLyThuyet.Cells[1, 1, row, wsLyThuyet.Dimension.Columns].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            wsLyThuyet.Cells[1, 1, row, wsLyThuyet.Dimension.Columns].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            wsLyThuyet.Cells[1, 1, row, wsLyThuyet.Dimension.Columns].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            wsLyThuyet.Cells[1, 1, row, wsLyThuyet.Dimension.Columns].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        private void ExportReportTotalNormalSurveyBG(List<Guid> surveyRoundIds)
        {
            var scope = _scopeFactory.CreateScope();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();

            var status = statusContext.AsStatusTableTask.FirstOrDefault(o => o.TableName == TableNameTask.ExportReportTotalNormalSurvey);
            if (status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái");
            }
            //bảng đang làm việc
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Đang thống kê, thao tác bị huỷ");
            }
            status.Status = (int)TableTaskStatus.Doing;
            statusContext.SaveChanges();

            try
            {
                #region lấy đợt ks, đề bài ks
                var dotKhaoSats = surveyContext.AsEduSurveyDotKhaoSat.Where(o => surveyRoundIds.Contains(o.Id)).ToList();
                var baiKhaoSats = surveyContext.AsEduSurveyBaiKhaoSat.Where(o => surveyRoundIds.Contains(o.DotKhaoSatId) && o.Status != (int)TheSurveyStatus.Deleted).ToList();

                var dotKhaoSatThuNhat = dotKhaoSats.FirstOrDefault(); // vì nếu các đợt là giống nhau về đề ks thì lấy thằng đầu tiên
                var baiKhaoSatCuaDotThuNhats = surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == dotKhaoSatThuNhat.Id && o.Status != (int)TheSurveyStatus.Deleted).ToList();

                var baiKsLyThuyet = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalSubjects);
                var deLyThuyet = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsLyThuyet.NoiDungDeThi);

                var baiKsThucHanh = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalPracticalSubjects);
                var deLyThuyetThucHanh = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsThucHanh.NoiDungDeThi);

                var baiKsThucHanhThiNghiem = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.PracticalSubjects);
                var deThucHanhThiNghiem = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsThucHanhThiNghiem.NoiDungDeThi);

                var baiKsDeDoAn = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.AssignmentSubjects);
                var deDoAn = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsDeDoAn.NoiDungDeThi);
                #endregion

                #region định nghĩa excel
                ExcelPackage excel = new ExcelPackage();
                //var wsTongHopChung = excel.Workbook.Worksheets.Add("Tổng hợp chung");

                var wsLyThuyet = excel.Workbook.Worksheets.Add("Lý thuyết");
                StyleExcelExport(wsLyThuyet);

                var wsLyThuyetThucHanh = excel.Workbook.Worksheets.Add("Lý thuyết thực hành");
                StyleExcelExport(wsLyThuyetThucHanh);

                var wsThucHanh = excel.Workbook.Worksheets.Add("Thực hành");
                StyleExcelExport(wsThucHanh);

                var wsDoAn = excel.Workbook.Worksheets.Add("Đồ án");
                StyleExcelExport(wsDoAn);

                //var wsHoiMoLT = excel.Workbook.Worksheets.Add("Câu hỏi mở Lý thuyết");
                //StyleShortAnswerExcelExport(wsHoiMoLT);

                //var wsHoiMoLTTH = excel.Workbook.Worksheets.Add("Câu hỏi mở Lý thuyết thực hành");
                //StyleShortAnswerExcelExport(wsHoiMoLTTH);

                //var wsHoiMoTH = excel.Workbook.Worksheets.Add("Câu hỏi mở Thực hành");
                //StyleShortAnswerExcelExport(wsHoiMoTH);

                //var wsHoiMoDA = excel.Workbook.Worksheets.Add("Câu hỏi mở Đồ án");
                //StyleShortAnswerExcelExport(wsHoiMoDA);
                #endregion

                #region kết xuất
                _logger.LogInformation($"Bat dau ket xuat");
                //tổng hợp chung

                ThongKeTungLoaiBaiKS(wsLyThuyet, surveyContext, baiKhaoSats, deLyThuyet, (int)TheSurveyType.TheoreticalSubjects);
                ThongKeTungLoaiBaiKS(wsLyThuyetThucHanh, surveyContext, baiKhaoSats, deLyThuyetThucHanh, (int)TheSurveyType.TheoreticalPracticalSubjects);
                ThongKeTungLoaiBaiKS(wsThucHanh, surveyContext, baiKhaoSats, deThucHanhThiNghiem, (int)TheSurveyType.PracticalSubjects);
                ThongKeTungLoaiBaiKS(wsDoAn, surveyContext, baiKhaoSats, deDoAn, (int)TheSurveyType.AssignmentSubjects);
                _logger.LogInformation($"ket xuat hoan thanh");
                #endregion

                //lưu
                var fileName = Guid.NewGuid().ToString() + ".xlsx";
                var filePath = System.IO.Path.GetTempPath() + fileName;
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    excel.SaveAs(fs);
                }

                //hoàn thành
                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = true;
                status.Message = fileName;
                statusContext.SaveChanges();
            }
            catch (Exception e)
            {
                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = false;
                status.Message = e.Message;
                statusContext.SaveChanges();
                throw e;
            }
        }

        public async Task ExportReportTotalNormalSurvey(List<Guid> surveyRoundIds)
        {
            var status = await _statusService.GetStatusTableTask(TableNameTask.ExportReportTotalNormalSurvey);
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Đang kết xuất, thao tác bị huỷ");
            }

            var scope = _scopeFactory.CreateScope();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();

            string ndDeLyThuyet = null;
            string ndDeLyThuyetThucHanh = null;
            string ndDeThucHanhThiNghiem = null;
            string ndDeDoAn = null;

            #region kiem tra ngoại lệ
            foreach (var id in surveyRoundIds)
            {
                var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
                if (surveyRound == null)
                {
                    throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
                }
                if (DateTime.Now < surveyRound.EndDate)
                {
                    throw new InvalidInputDataException($"Đợt khảo sát {surveyRound.Name} chưa kết thúc");
                }

                var baiKhaoSats = surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id).ToList();
                if (baiKhaoSats.Count == 0)
                {
                    throw new InvalidInputDataException($"Đợt khảo sát {surveyRound.Name} không có bài khảo sát nào");
                }

                var baiKhaoSatLyThuyet = baiKhaoSats.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalSubjects);
                if (baiKhaoSatLyThuyet == null)
                {
                    throw new InvalidInputDataException($"Đợt khảo sát {surveyRound.Name} không có bài khảo sát cho môn lý thuyết");
                }

                if (ndDeLyThuyet == null)
                {
                    ndDeLyThuyet = baiKhaoSatLyThuyet.NoiDungDeThi;
                }
                else
                {
                    if (ndDeLyThuyet != baiKhaoSatLyThuyet.NoiDungDeThi)
                    {
                        throw new InvalidInputDataException($"Các đợt khảo sát không có phiếu khảo sát cho môn lý thuyết giống nhau");
                    }
                }

                var baiKhaoSatLyThuyetThucHanh = baiKhaoSats.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalPracticalSubjects);
                if (baiKhaoSatLyThuyetThucHanh == null)
                {
                    throw new InvalidInputDataException($"Đợt khảo sát {surveyRound.Name} không có bài khảo sát cho môn lý thuyết + thực hành");
                }

                if (ndDeLyThuyetThucHanh == null)
                {
                    ndDeLyThuyetThucHanh = baiKhaoSatLyThuyetThucHanh.NoiDungDeThi;
                }
                else
                {
                    if (ndDeLyThuyetThucHanh != baiKhaoSatLyThuyetThucHanh.NoiDungDeThi)
                    {
                        throw new InvalidInputDataException($"Các đợt khảo sát không có phiếu khảo sát cho môn lý thuyết + thực hành giống nhau");
                    }
                }

                var baiKhaoSatThucHanhThiNghiem = baiKhaoSats.FirstOrDefault(o => o.Type == (int)TheSurveyType.PracticalSubjects);
                if (baiKhaoSatThucHanhThiNghiem == null)
                {
                    throw new InvalidInputDataException($"Đợt khảo sát {surveyRound.Name} không có bài khảo sát cho môn thực hành, thí nghiệm, thực tập");
                }

                if (ndDeThucHanhThiNghiem == null)
                {
                    ndDeThucHanhThiNghiem = baiKhaoSatThucHanhThiNghiem.NoiDungDeThi;
                }
                else
                {
                    if (ndDeThucHanhThiNghiem != baiKhaoSatThucHanhThiNghiem.NoiDungDeThi)
                    {
                        throw new InvalidInputDataException($"Các đợt khảo sát không có phiếu khảo sát cho môn thực hành, thí nghiệm, thực tâp giống nhau");
                    }
                }

                var baiKhaoSatDoAn = baiKhaoSats.FirstOrDefault(o => o.Type == (int)TheSurveyType.AssignmentSubjects);
                if (baiKhaoSatDoAn == null)
                {
                    throw new InvalidInputDataException($"Đợt khảo sát {surveyRound.Name} không có bài khảo sát cho môn đồ án");
                }

                if (ndDeDoAn == null)
                {
                    ndDeDoAn = baiKhaoSatDoAn.NoiDungDeThi;
                }
                else
                {
                    if (ndDeDoAn != baiKhaoSatDoAn.NoiDungDeThi)
                    {
                        throw new InvalidInputDataException($"Các đợt khảo sát không có phiếu khảo sát cho môn đồ án giống nhau");
                    }
                }
            }
            #endregion

            _backgroundTaskWorker.StartAction(() =>
            {
                ExportReportTotalNormalSurveyBG(surveyRoundIds);
            });
        }
        #endregion
        #endregion
    }
}
