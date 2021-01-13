using Microsoft.Data.SqlClient;
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
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey.Normal.Statistic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.BackgroundTasks
{
    public class SurveyStatisticBackgroundTask
    {
        private readonly ILogger<SurveyStatisticBackgroundTask> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly BackgroundTaskWorkder _backgroundTaskWorker;
        private readonly IStatusService _statusService;

        public SurveyStatisticBackgroundTask(ILogger<SurveyStatisticBackgroundTask> logger,
            IServiceScopeFactory scopeFactory, BackgroundTaskWorkder backgroundTaskWorker,
             IStatusService statusService)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _backgroundTaskWorker = backgroundTaskWorker;
            _statusService = statusService;
        }

        private List<AnswerSelectedReportTotal> AnswerSelectedReportTotal(List<SelectedAnswerExtend> selectedList)
        {
            var result = new List<AnswerSelectedReportTotal>();

            //loại câu chọn 1
            var groupSingleChoiceSelectedAnswers = selectedList.Where(o => o.AnswerCode != null).GroupBy(o => new { o.TheSurveyId, o.QuestionCode, o.AnswerCode });
            foreach (var item in groupSingleChoiceSelectedAnswers)
            {
                result.Add(new AnswerSelectedReportTotal
                {
                    TheSurveyId = item.Key.TheSurveyId,
                    QuestionCode = item.Key.QuestionCode,
                    AnswerCode = item.Key.AnswerCode,
                    Total = item.Count()
                });
            }

            //loại câu chọn nhiều
            var groupMultiChoiceSelectedAnswers = selectedList
                .Where(o => o.AnswerCodes != null && o.AnswerCodes.Count > 0)
                .SelectMany(o => o.AnswerCodes, (r, AnswerCode) => new { r.TheSurveyId, r.QuestionCode, AnswerCode })
                .GroupBy(o => new { o.TheSurveyId, o.QuestionCode, o.AnswerCode });

            foreach (var item in groupMultiChoiceSelectedAnswers)
            {
                result.Add(new AnswerSelectedReportTotal
                {
                    TheSurveyId = item.Key.TheSurveyId,
                    QuestionCode = item.Key.QuestionCode,
                    AnswerCode = item.Key.AnswerCode,
                    Total = item.Count()
                });
            }

            //loại câu trả lời text
            var groupShortAnswerSelectedAnswers = selectedList.Where(o => o.AnswerContent != null).GroupBy(o => new { o.TheSurveyId, o.QuestionCode }, o => new { o.AnswerContent });
            foreach (var item in groupShortAnswerSelectedAnswers)
            {
                List<string> strAllAnswerContent = new List<string>();
                foreach (var str in item)
                {
                    strAllAnswerContent.Add(str.AnswerContent);
                }

                result.Add(new AnswerSelectedReportTotal
                {
                    TheSurveyId = item.Key.TheSurveyId,
                    QuestionCode = item.Key.QuestionCode,
                    Content = JsonSerializer.Serialize(strAllAnswerContent)
                });
            }

            //loại vote sao
            var groupVoteStarSelectedAnswers = selectedList.Where(o => o.NumStart != null).GroupBy(o => new { o.TheSurveyId, o.QuestionCode, o.NumStart }, o => new { o.NumStart });
            foreach (var item in groupVoteStarSelectedAnswers)
            {
                result.Add(new AnswerSelectedReportTotal
                {
                    TheSurveyId = item.Key.TheSurveyId,
                    QuestionCode = item.Key.QuestionCode,
                    Content = $"{item.Key.NumStart} sao",
                    Total = item.Count()
                });
            }

            //loại tỉnh thành
            var groupCitySelectedAnswers = selectedList.Where(o => o.City != null).GroupBy(o => new { o.TheSurveyId, o.QuestionCode, o.City }, o => new { o.City });
            foreach (var item in groupCitySelectedAnswers)
            {
                result.Add(new AnswerSelectedReportTotal
                {
                    TheSurveyId = item.Key.TheSurveyId,
                    QuestionCode = item.Key.QuestionCode,
                    Content = item.Key.City,
                    Total = item.Count()
                });
            }

            return result;
        }

        #region thống kê sinh viên thường
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
                var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.AsNoTracking().FirstOrDefault(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
                if (surveyRound == null)
                {
                    throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
                }

                //đợt khảo sát chưa kết thúc
                if (!(surveyRound.Status == (int)SurveyRoundStatus.Closed || surveyRound.Status == (int)SurveyRoundStatus.End || DateTime.Now >= surveyRound.EndDate))
                {
                    throw new HandleException.InvalidInputDataException("Đợt khảo sát chưa đóng hoặc chưa kết thúc");
                }

                //do chỉ có một bài ks nên lấy id của bài ks đó
                var idbaikscuadotnay = surveyContext.AsEduSurveyBaiKhaoSat.AsNoTracking().Where(o => o.DotKhaoSatId == surveyRound.Id).Select(o => o.Id).ToList();
                if (idbaikscuadotnay.Count == 0)
                {
                    throw new RecordNotFoundException("Không tìm thấy bài khảo sát của đợt khảo sát này");
                }

                _logger.LogInformation("report total normal is start.");
                //surveyContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE {TableNameTask.AsEduSurveyReportTotal}");
                var baikshoanthanh = surveyContext.AsEduSurveyBaiKhaoSatSinhVien.AsNoTracking().Where(o => idbaikscuadotnay.Contains(o.BaiKhaoSatId)).Where(o => o.Status == (int)SurveyStudentStatus.Done);

                var tongBaiKsHoanThanh = baikshoanthanh.Count();
                List<SelectedAnswerExtend> selectedAnswers;

                var groupLopGiangVien = baikshoanthanh
                .GroupBy(o => new { o.LecturerCode, o.ClassRoomCode, o.BaiKhaoSatId })
                .Select(r => new { r.Key.LecturerCode, r.Key.ClassRoomCode, r.Key.BaiKhaoSatId })
                .ToList();

                var totalLectureClassroom = groupLopGiangVien.Count();
                var count = 0;
                foreach (var lectureClassroom in groupLopGiangVien)
                {
                    var lectureCode = lectureClassroom.LecturerCode;
                    var classroomCode = lectureClassroom.ClassRoomCode;

                    //từng giảng viên lớp môn học
                    var cacBaiLam = baikshoanthanh.Where(o => o.ClassRoomCode == classroomCode && o.LecturerCode == lectureCode).ToList();
                    selectedAnswers = new List<SelectedAnswerExtend>();

                    foreach (var bailam in cacBaiLam)
                    {
                        if (string.IsNullOrEmpty(bailam.BaiLam))
                        {
                            continue;
                        }
                        var json = JsonSerializer.Deserialize<List<SelectedAnswerExtend>>(bailam.BaiLam);
                        json.ForEach(o => o.TheSurveyId = bailam.BaiKhaoSatId);
                        selectedAnswers.AddRange(json);
                    }

                    var total = AnswerSelectedReportTotal(selectedAnswers);
                    foreach (var item in total)
                    {
                        var thongkecuthe = surveyContext.AsEduSurveyReportTotal
                            .FirstOrDefault(o => o.ClassRoomCode == classroomCode && o.LecturerCode == lectureCode && o.TheSurveyId == item.TheSurveyId &&
                            o.QuestionCode == item.QuestionCode && o.AnswerCode == o.AnswerCode);
                        if (thongkecuthe == null)
                        {
                            surveyContext.AsEduSurveyReportTotal.Add(new AsEduSurveyReportTotal
                            {
                                Id = Guid.NewGuid(),
                                SurveyRoundId = surveyRound.Id,
                                TheSurveyId = item.TheSurveyId,
                                ClassRoomCode = classroomCode,
                                LecturerCode = lectureCode,
                                QuestionCode = item.QuestionCode,
                                AnswerCode = item.AnswerCode,
                                Content = item.Content,
                                Total = item.Total,
                            });
                        }
                        else //nếu có rồi thì cập nhật
                        {
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
                var result = new List<TempDataNormal>();

                _logger.LogInformation($"Bat dau thong ke tam");
                var facultys = eduContext.AsAcademyFaculty.ToList();
                foreach (var f in facultys)
                {
                    _logger.LogInformation($"Đang thong ke tam cho khoa co ma {f.Code}");
                    var classF = eduContext.AsAcademyClass.Where(o => o.FacultyCode == f.Code).Select(o => o.Code).ToList();
                    //tất cả sv có đk
                    var allStudents = eduContext.AsAcademyStudent
                        .Where(o => classF.Contains(o.ClassCode))
                        .Where(o => eduContext.AsAcademyStudentClassRoom.FirstOrDefault(sc => sc.StudentCode == o.Code) != null)
                        .Select(o => o.Code)
                        .ToList();

                    //bài ks sinh viên của khoa này
                    var tatCaBaiKs = tatCaBaiKsCuaDotNay.Where(o => allStudents.Contains(o.StudentCode));

                    //số bài ks được phát
                    var total = tatCaBaiKs.Count();

                    //số bài hoàn thành
                    var num = tatCaBaiKs.Count(o => o.Status == (int)SurveyStudentStatus.Done);

                    result.Add(new TempDataNormal
                    {
                        FacultyCode = f.Code,
                        FacultyName = f.Name,
                        Total = total,
                        Num = num
                    });
                }
                _logger.LogInformation($"Thong ke tam hoan thanh");

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

            worksheet.Column(1).Width = 44.86;
            worksheet.Column(2).Width = 33;

            worksheet.Cells["A2"].Value = "Khoa";
            worksheet.Cells["B2"].Value = "Bộ môn";
            worksheet.Cells["C2"].Value = "Số sv tham gia khảo sát";
            worksheet.Cells["D2"].Value = "Số sv được khảo sát";
            worksheet.Cells["E2"].Value = "Số phiếu thu về";
            worksheet.Cells["F2"].Value = "Số phiếu ks phát ra";
            worksheet.Cells["G2"].Value = "Số giảng viên đã được ks";
            worksheet.Cells["H2"].Value = "Số giảng viên cần lấy ý kiến ks";
        }

        private void ThongKeTungLoaiBaiKS(ExcelWorksheet wsLyThuyet, EduDataContext eduContext, SurveyContext surveyContext,
            IQueryable<AsEduSurveyBaiKhaoSatSinhVien> baiLamKhaoSatCacDotDangXet, List<AsEduSurveyBaiKhaoSat> baiKhaoSats,
            List<QuestionJson> deLyThuyet, int loaiMon)
        {
            //lấy khoa
            int row = 4;
            int rowCauHoi = 2;
            int rowDapAn = 3;
            int col = 1;
            var facultys = eduContext.AsAcademyFaculty.ToList();

            var tongToanTruong = new Dictionary<int, float>();
            foreach (var f in facultys)
            {
                col = 1;
                _logger.LogInformation($"Dang ket xuat loai {loaiMon} khoa co ma {f.Code}");
                //lấy bộ môn
                var departments = eduContext.AsAcademyDepartment.Where(o => o.FacultyCode == f.Code).ToList();

                //các biến lấy tổng
                var tongSoSVThamGiaKhaoSat = 0;
                var tongSoSVDuocKhaoSat = 0;
                var tongSoPhieuThuVe = 0;
                var tongSoPhieuPhatRa = 0;
                var tongSoGiangVienCanKs = 0;
                var tongSoGiangVienDaDuocKs = 0;

                var rowTotal = row + departments.Count();

                //lấy môn học của bộ môn
                foreach (var d in departments)
                {
                    _logger.LogInformation($"Dang ket xuat loai {loaiMon} bo mon co ma {d.Code} cua khoa {f.Code}");
                    var monHocCuaBoMon = eduContext.AsAcademySubject.Where(o => o.DepartmentCode == d.Code)
                        .Join(eduContext.AsAcademySubjectExtend, o => o.Code, o => o.Code, (monhoc, loaimon) => new { monhoc, loaimon.Type })
                        .Select(o => new
                        {
                            o.monhoc.Code,
                            o.monhoc.DepartmentCode,
                            o.monhoc.Name,
                            o.Type
                        });

                    #region môn
                    var monLyThuyetCuaBoMonCodes = monHocCuaBoMon.Where(o => o.Type == loaiMon).Select(o => o.Code).ToList();
                    //if(monLyThuyetCuaBoMonCodes.Count == 0) //bộ môn không có môn loại này
                    //{
                    //    continue;
                    //}

                    //các bài làm của môn lý thuyết của bộ môn đang xét
                    var baiLamKhaoSatLyThuyet = baiLamKhaoSatCacDotDangXet.Where(o => monLyThuyetCuaBoMonCodes.Contains(o.SubjectCode));
                    var baiLamKhaoSatLyThuyetHoanThanh = baiLamKhaoSatLyThuyet.Where(o => o.Status == (int)SurveyStudentStatus.Done).Select(o => new { o.StudentCode, o.LecturerCode });

                    var soPhieuPhatRa = baiLamKhaoSatLyThuyet.Count();
                    tongSoPhieuPhatRa += soPhieuPhatRa;
                    var soPhieuThuVe = baiLamKhaoSatLyThuyetHoanThanh.Count();
                    tongSoPhieuThuVe += soPhieuThuVe;

                    var soSVThamGiaKhaoSat = baiLamKhaoSatLyThuyet.GroupBy(o => o.StudentCode).Select(o => o.Key).Count();
                    tongSoSVThamGiaKhaoSat += soSVThamGiaKhaoSat;
                    var soSVDuocKhaoSat = baiLamKhaoSatLyThuyetHoanThanh.GroupBy(o => o.StudentCode).Select(o => o.Key).Count();
                    tongSoSVDuocKhaoSat += soSVDuocKhaoSat;

                    var soGiangVienCanKs = baiLamKhaoSatLyThuyet.GroupBy(o => o.LecturerCode).Select(o => o.Key).Count();
                    tongSoGiangVienCanKs += soGiangVienCanKs;
                    var soGiangVienDaDuocKs = baiLamKhaoSatLyThuyetHoanThanh.GroupBy(o => o.LecturerCode).Select(o => o.Key).Count();
                    tongSoGiangVienDaDuocKs += soGiangVienDaDuocKs;

                    #region tổng hợp số lượng
                    wsLyThuyet.Cells[row, col++].Value = f.Name;
                    wsLyThuyet.Cells[row, col++].Value = d.Name;
                    wsLyThuyet.Cells[row, col++].Value = soSVThamGiaKhaoSat;
                    if (tongToanTruong.ContainsKey(col - 1))
                    {
                        tongToanTruong[col - 1] += soSVThamGiaKhaoSat;
                    }
                    else
                    {
                        tongToanTruong.Add(col - 1, soSVThamGiaKhaoSat);
                    }
                    wsLyThuyet.Cells[row, col++].Value = soSVDuocKhaoSat;
                    if (tongToanTruong.ContainsKey(col - 1))
                    {
                        tongToanTruong[col - 1] += soSVDuocKhaoSat;
                    }
                    else
                    {
                        tongToanTruong.Add(col - 1, soSVDuocKhaoSat);
                    }
                    wsLyThuyet.Cells[row, col++].Value = soPhieuThuVe;
                    if (tongToanTruong.ContainsKey(col - 1))
                    {
                        tongToanTruong[col - 1] += soPhieuThuVe;
                    }
                    else
                    {
                        tongToanTruong.Add(col - 1, soPhieuThuVe);
                    }
                    wsLyThuyet.Cells[row, col++].Value = soPhieuPhatRa;
                    if (tongToanTruong.ContainsKey(col - 1))
                    {
                        tongToanTruong[col - 1] += soPhieuPhatRa;
                    }
                    else
                    {
                        tongToanTruong.Add(col - 1, soPhieuPhatRa);
                    }
                    wsLyThuyet.Cells[row, col++].Value = soGiangVienDaDuocKs;
                    if (tongToanTruong.ContainsKey(col - 1))
                    {
                        tongToanTruong[col - 1] += soGiangVienDaDuocKs;
                    }
                    else
                    {
                        tongToanTruong.Add(col - 1, soGiangVienDaDuocKs);
                    }
                    wsLyThuyet.Cells[row, col++].Value = soGiangVienCanKs;
                    if (tongToanTruong.ContainsKey(col - 1))
                    {
                        tongToanTruong[col - 1] += soGiangVienCanKs;
                    }
                    else
                    {
                        tongToanTruong.Add(col - 1, soGiangVienCanKs);
                    }
                    #endregion

                    #region tổng hợp dữ liệu thống kê thô
                    var lerturerCodes = eduContext.AsAcademyLecturer.Where(o => o.DepartmentCode == d.Code).Select(o => o.Code).ToList();

                    var theSurveyIdLyThuyets = baiKhaoSats.Where(o => o.Type == loaiMon).Select(o => o.Id).ToList();
                    var reportTotalLyThuyet = surveyContext.AsEduSurveyReportTotal.Where(o => theSurveyIdLyThuyets.Contains(o.TheSurveyId));
                    var index = 0;
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

                                //tổng hợp
                                if (wsLyThuyet.Cells[rowTotal, col].Value == null)
                                {
                                    wsLyThuyet.Cells[rowTotal, col].Value = 0;
                                }

                                //tổng toàn trường
                                if (!tongToanTruong.ContainsKey(col))
                                {
                                    tongToanTruong.Add(col, 0);
                                }

                                wsLyThuyet.Cells[rowDapAn, col].Value = dapan.Content;
                                var ketqua = reportTotalLyThuyet.FirstOrDefault(o => lerturerCodes.Contains(o.LecturerCode) && o.QuestionCode == cauhoi.Code && o.AnswerCode == dapan.Code );
                                if (ketqua != null)
                                {
                                    var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                    wsLyThuyet.Cells[row, col].Value = total;
                                    sumTotal += total;

                                    //tổng hợp
                                    wsLyThuyet.Cells[rowTotal, col].Value = (int)(wsLyThuyet.Cells[rowTotal, col].Value) + total;

                                    //tổng toàn trường
                                    tongToanTruong[col] += total;

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
                            }
                            else
                            {
                                wsLyThuyet.Cells[row, col].Value = 0;
                            }

                            //tổng hợp trung bình
                            float dTBTotal = 0;
                            float sumdTBTotal = 0;

                            //tổng hợp toàn trường
                            float dTBTotalToanTruong = 0;
                            float sumTotalToanTruong = 0;
                            int totalDiem = (colTotal) - (colTotal - cauhoi.Answers.Count);
                            for (int i = colTotal - 1; i >= colTotal - cauhoi.Answers.Count; i--)
                            {
                                try
                                {
                                    sumdTBTotal += (int)wsLyThuyet.Cells[rowTotal, i].Value;
                                    dTBTotal += (int)wsLyThuyet.Cells[rowTotal, i].Value * totalDiem;
                                }
                                catch
                                {

                                }
                                sumTotalToanTruong += tongToanTruong[i];
                                dTBTotalToanTruong += tongToanTruong[i] * totalDiem;
                                totalDiem--;
                            }

                            //tổng hợp tb khoa
                            if (sumdTBTotal > 0)
                            {
                                wsLyThuyet.Cells[rowTotal, colTotal].Value = (float)dTBTotal / sumdTBTotal;
                            }
                            else
                            {
                                wsLyThuyet.Cells[rowTotal, colTotal].Value = 0;
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
                                //tổng hợp
                                if (wsLyThuyet.Cells[rowTotal, col].Value == null)
                                {
                                    wsLyThuyet.Cells[rowTotal, col].Value = 0;
                                }

                                //tổng toàn trường
                                if (!tongToanTruong.ContainsKey(col))
                                {
                                    tongToanTruong.Add(col, 0);
                                }

                                wsLyThuyet.Cells[rowDapAn, col].Value = dapan.Content;
                                var ketqua = reportTotalLyThuyet.FirstOrDefault(o => lerturerCodes.Contains(o.LecturerCode) && o.QuestionCode == cauhoi.Code && o.AnswerCode == dapan.Code);
                                if (ketqua != null)
                                {
                                    var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                    wsLyThuyet.Cells[row, col].Value = total;

                                    //tổng hợp
                                    wsLyThuyet.Cells[rowTotal, col].Value = (int)(wsLyThuyet.Cells[rowTotal, col].Value) + total;

                                    //tổng toàn trường
                                    tongToanTruong[col] += total;
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
                                    var ketquaCon = reportTotalLyThuyet.FirstOrDefault(o => o.QuestionCode == $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}" && o.AnswerCode == dapan.Code);
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
                            var ketqua = reportTotalLyThuyet.FirstOrDefault(o => lerturerCodes.Contains(o.LecturerCode) && o.QuestionCode == cauhoi.Code);
                            if(ketqua != null && ketqua.Content != null)
                            {
                                var str = "";
                                var listStr = JsonSerializer.Deserialize<List<string>>(ketqua.Content);
                                listStr.ForEach(s =>
                                {
                                    str += $"{s};";
                                });
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
                                if (cauhoicon.Type == QuestionType.SC)
                                {
                                    var colStart = col;
                                    wsLyThuyet.Cells[rowCauHoi, col].Value = $"Câu {index}.{++indexChild}: {cauhoicon.Content}";
                                    var dTB = 0;
                                    var sumTotal = 0;
                                    var diem = 0;
                                    var colTotal = col + cauhoicon.Answers.Count();
                                    foreach (var dapan in cauhoicon.Answers)
                                    {
                                        diem++;

                                        //tổng hợp
                                        if (wsLyThuyet.Cells[rowTotal, col].Value == null)
                                        {
                                            wsLyThuyet.Cells[rowTotal, col].Value = 0;
                                        }

                                        //tổng toàn trường
                                        if (!tongToanTruong.ContainsKey(col))
                                        {
                                            tongToanTruong.Add(col, 0);
                                        }

                                        wsLyThuyet.Cells[rowDapAn, col].Value = dapan.Content;
                                        var ketqua = reportTotalLyThuyet.FirstOrDefault(o => lerturerCodes.Contains(o.LecturerCode) && o.QuestionCode == cauhoicon.Code && o.AnswerCode == dapan.Code);
                                        if (ketqua != null)
                                        {
                                            var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                            wsLyThuyet.Cells[row, col].Value = total;
                                            sumTotal += total;

                                            //tổng hợp
                                            wsLyThuyet.Cells[rowTotal, col].Value = (int)(wsLyThuyet.Cells[rowTotal, col].Value) + total;

                                            //tổng toàn trường
                                            tongToanTruong[col] += total;

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
                                    }
                                    else
                                    {
                                        wsLyThuyet.Cells[row, col].Value = 0;
                                    }

                                    //tổng hợp trung bình
                                    float dTBTotal = 0;
                                    float sumdTBTotal = 0;

                                    //tổng hợp toàn trường
                                    float dTBTotalToanTruong = 0;
                                    float sumTotalToanTruong = 0;
                                    int totalDiem = (colTotal) - (colTotal - cauhoicon.Answers.Count);
                                    for (int i = colTotal - 1; i >= colTotal - cauhoicon.Answers.Count; i--)
                                    {
                                        try
                                        {
                                            sumdTBTotal += (int)wsLyThuyet.Cells[rowTotal, i].Value;
                                            dTBTotal += (int)wsLyThuyet.Cells[rowTotal, i].Value * totalDiem;
                                        }
                                        catch
                                        {

                                        }
                                        sumTotalToanTruong += tongToanTruong[i];
                                        dTBTotalToanTruong += tongToanTruong[i] * totalDiem;
                                        totalDiem--;
                                    }

                                    //tổng hợp tb khoa
                                    if (sumdTBTotal > 0)
                                    {
                                        wsLyThuyet.Cells[rowTotal, colTotal].Value = (float)dTBTotal / sumdTBTotal;
                                    }
                                    else
                                    {
                                        wsLyThuyet.Cells[rowTotal, colTotal].Value = 0;
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
                                    wsLyThuyet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                                }
                                else if (cauhoicon.Type == QuestionType.MC)
                                {
                                    var colStart = col;
                                    wsLyThuyet.Cells[rowCauHoi, col].Value = $"Câu {++index}: {cauhoicon.Content}";
                                    wsLyThuyet.Cells[rowCauHoi, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    wsLyThuyet.Cells[rowCauHoi, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                                    foreach (var dapan in cauhoicon.Answers)
                                    {
                                        //tổng hợp
                                        if (wsLyThuyet.Cells[rowTotal, col].Value == null)
                                        {
                                            wsLyThuyet.Cells[rowTotal, col].Value = 0;
                                        }

                                        //tổng toàn trường
                                        if (!tongToanTruong.ContainsKey(col))
                                        {
                                            tongToanTruong.Add(col, 0);
                                        }

                                        wsLyThuyet.Cells[rowDapAn, col].Value = dapan.Content;
                                        var ketqua = reportTotalLyThuyet.FirstOrDefault(o => lerturerCodes.Contains(o.LecturerCode) && o.QuestionCode == cauhoicon.Code && o.AnswerCode == dapan.Code);
                                        if (ketqua != null)
                                        {
                                            var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                            wsLyThuyet.Cells[row, col].Value = total;

                                            //tổng hợp
                                            wsLyThuyet.Cells[rowTotal, col].Value = (int)(wsLyThuyet.Cells[rowTotal, col].Value) + total;

                                            //tổng toàn trường
                                            tongToanTruong[col] += total;
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
                                            var ketquaCon = reportTotalLyThuyet.FirstOrDefault(o => o.QuestionCode == $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}" && o.AnswerCode == dapan.Code);
                                            wsLyThuyet.Cells[row, col].Value = ketquaCon?.Content ?? "";
                                        }
                                        col++;
                                    }
                                    var colEnd = col - 1;
                                    wsLyThuyet.Cells[rowCauHoi, colStart, rowCauHoi, colEnd].Merge = true;
                                }
                                if (cauhoicon.Type == QuestionType.SA)
                                {
                                    wsLyThuyet.Cells[rowCauHoi, col].Value = $"Câu {index}.{++indexChild}: {cauhoicon.Content}";
                                    wsLyThuyet.Column(col).Width = 48;
                                    var ketqua = reportTotalLyThuyet.FirstOrDefault(o => lerturerCodes.Contains(o.LecturerCode) && o.QuestionCode == cauhoicon.Code);
                                    if (ketqua != null && ketqua.Content != null)
                                    {
                                        var str = "";
                                        var listStr = JsonSerializer.Deserialize<List<string>>(ketqua.Content);
                                        listStr.ForEach(s =>
                                        {
                                            str += $"{s};";
                                        });
                                        wsLyThuyet.Cells[row, col].Value = str;
                                    }
                                }
                                col++;
                            }
                        }
                    }
                    #endregion

                    #endregion
                    col = 1;
                    row++;
                }

                #region tổng số
                col = 1;
                wsLyThuyet.Row(row).Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsLyThuyet.Row(row).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                wsLyThuyet.Cells[row, col++].Value = $"{f.Name}({f.Code})";
                col = 3;
                wsLyThuyet.Cells[row, col++].Value = tongSoSVThamGiaKhaoSat;
                wsLyThuyet.Cells[row, col++].Value = tongSoSVDuocKhaoSat;
                wsLyThuyet.Cells[row, col++].Value = tongSoPhieuThuVe;
                wsLyThuyet.Cells[row, col++].Value = tongSoPhieuPhatRa;
                wsLyThuyet.Cells[row, col++].Value = tongSoGiangVienDaDuocKs;
                wsLyThuyet.Cells[row, col++].Value = tongSoGiangVienCanKs;


                row++;
                #endregion
            }
            col = 1;
            wsLyThuyet.Row(row).Style.Fill.PatternType = ExcelFillStyle.Solid;
            wsLyThuyet.Row(row).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
            wsLyThuyet.Cells[row, col].Value = "Toàn trường";
            foreach (var item in tongToanTruong)
            {
                wsLyThuyet.Cells[row, item.Key].Value = item.Value;
            }

            wsLyThuyet.Cells[1, 1, row, wsLyThuyet.Dimension.Columns - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            wsLyThuyet.Cells[1, 1, row, wsLyThuyet.Dimension.Columns - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            wsLyThuyet.Cells[1, 1, row, wsLyThuyet.Dimension.Columns - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            wsLyThuyet.Cells[1, 1, row, wsLyThuyet.Dimension.Columns - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        private void ExportReportTotalNormalSurveyBG(List<Guid> surveyRoundIds)
        {
            var scope = _scopeFactory.CreateScope();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            var eduContext = scope.ServiceProvider.GetRequiredService<EduDataContext>();
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

                var baiKhaoSatIds = baiKhaoSats.Select(o => o.Id).ToList();
                var baiLamKhaoSatCacDotDangXet = surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => baiKhaoSatIds.Contains(o.BaiKhaoSatId));

                #region định nghĩa excel
                ExcelPackage excel = new ExcelPackage();
                var wsTongHopChung = excel.Workbook.Worksheets.Add("Tổng hợp chung");

                var wsLyThuyet = excel.Workbook.Worksheets.Add("Lý thuyết");
                StyleExcelExport(wsLyThuyet);

                var wsLyThuyetThucHanh = excel.Workbook.Worksheets.Add("Lý thuyết thực hành");
                StyleExcelExport(wsLyThuyetThucHanh);

                var wsThucHanh = excel.Workbook.Worksheets.Add("Thực hành");
                StyleExcelExport(wsThucHanh);

                var wsDoAn = excel.Workbook.Worksheets.Add("Đồ án");
                StyleExcelExport(wsDoAn);
                #endregion

                #region kết xuất
                _logger.LogInformation($"Bat dau ket xuat");
                ThongKeTungLoaiBaiKS(wsLyThuyet, eduContext, surveyContext, baiLamKhaoSatCacDotDangXet, baiKhaoSats, deLyThuyet, (int)TheSurveyType.TheoreticalSubjects);
                ThongKeTungLoaiBaiKS(wsLyThuyetThucHanh, eduContext, surveyContext, baiLamKhaoSatCacDotDangXet, baiKhaoSats, deLyThuyetThucHanh, (int)TheSurveyType.TheoreticalPracticalSubjects);
                ThongKeTungLoaiBaiKS(wsThucHanh, eduContext, surveyContext, baiLamKhaoSatCacDotDangXet, baiKhaoSats, deThucHanhThiNghiem, (int)TheSurveyType.PracticalSubjects);
                ThongKeTungLoaiBaiKS(wsDoAn, eduContext, surveyContext, baiLamKhaoSatCacDotDangXet, baiKhaoSats, deDoAn, (int)TheSurveyType.AssignmentSubjects);
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

        #region thống kê sinh viên trước tốt nghiệp
        private void ReportTotalUndergraduateSurveyBG(Guid surveyRoundId)
        {
            var scope = _scopeFactory.CreateScope();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();

            var status = statusContext.AsStatusTableTask.FirstOrDefault(o => o.TableName == TableNameTask.AsEduSurveyUndergraduateReportTotal);
            if (status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng thống kê khảo sát sinh viên");
            }

            status.Status = (int)TableTaskStatus.Doing;
            statusContext.SaveChanges();

            try
            {
                _logger.LogInformation("report total undergraduate is start.");

                var student = surveyContext.AsEduSurveyUndergraduateStudent.Where(o => o.DotKhaoSatId == surveyRoundId);
                var join = student.Join(surveyContext.AsEduSurveyUndergraduateBaiKhaoSatSinhVien, o => o.Masv, o => o.StudentCode, (sv, baikssv) => new { baikssv });

                var countTheSurveyStudent = join.Count();
                var skip = 0;
                var take = 500;

                List<SelectedAnswerExtend> selectedAnswers;
                while (skip <= countTheSurveyStudent)
                {
                    var dsBaiLam = join
                    .Skip(skip).Take(take)
                    .ToList();

                    selectedAnswers = new List<SelectedAnswerExtend>();

                    foreach (var bailam in dsBaiLam)
                    {
                        var baikhaosatId = bailam.baikssv.BaiKhaoSatId;

                        var json = JsonSerializer.Deserialize<List<SelectedAnswerExtend>>(bailam.baikssv.BaiLam);

                        json.ForEach(o => o.TheSurveyId = baikhaosatId);
                        selectedAnswers.AddRange(json);
                    }

                    var total = AnswerSelectedReportTotal(selectedAnswers);

                    foreach (var item in total)
                    {
                        surveyContext.AsEduSurveyUndergraduateReportTotal.Add(new AsEduSurveyUndergraduateReportTotal
                        {
                            Id = Guid.NewGuid(),
                            SurveyRoundId = surveyRoundId,
                            TheSurveyId = item.TheSurveyId,
                            QuestionCode = item.QuestionCode,
                            AnswerCode = item.AnswerCode,
                            Content = item.Content,
                            Total = item.Total,
                        });
                    }

                    surveyContext.SaveChanges();
                    _logger.LogInformation($"report total undergraduate loading: {skip}/{countTheSurveyStudent}");
                    skip += take;
                }

                _logger.LogInformation("report total undergraduate is done.");

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

        public async Task ReportTotalUndergraduateSurvey(Guid surveyRoundId)
        {
            var status = await _statusService.GetStatusTableTask(TableNameTask.AsEduSurveyUndergraduateReportTotal);
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Bảng đang làm việc, thao tác bị huỷ");
            }

            var scope = _scopeFactory.CreateScope();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();

            var surveyRound = surveyContext.AsEduSurveyUndergraduateSurveyRound.FirstOrDefault(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            //đợt khảo sát chưa kết thúc
            if (surveyRound.Status != (int)SurveyRoundStatus.Closed && surveyRound.Status != (int)SurveyRoundStatus.End)
            {
                throw new HandleException.InvalidInputDataException("Đợt khảo sát chưa đóng hoặc chưa kết thúc");
            }

            _backgroundTaskWorker.StartAction(() =>
            {
                ReportTotalUndergraduateSurveyBG(surveyRoundId);
            });
        }
        #endregion

        #region thống kê cựu sinh viên
        #endregion
    }
}
