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
using System;
using System.Collections.Generic;
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
                .SelectMany(o => o.AnswerCodes, (r, AnswerCode) => new {r.TheSurveyId, r.QuestionCode, AnswerCode })
                .GroupBy(o => new {o.TheSurveyId, o.QuestionCode, o.AnswerCode });

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
            var groupShortAnswerSelectedAnswers = selectedList.Where(o => o.AnswerContent != null).GroupBy(o => new {o.TheSurveyId, o.QuestionCode }, o => new { o.AnswerContent });
            foreach (var item in groupShortAnswerSelectedAnswers)
            {
                string strAllAnswerContent = "";
                foreach (var str in item)
                {
                    strAllAnswerContent += $";{str.AnswerContent}";
                }
                result.Add(new AnswerSelectedReportTotal
                {
                    TheSurveyId = item.Key.TheSurveyId,
                    QuestionCode = item.Key.QuestionCode,
                    Content = strAllAnswerContent
                });
            }

            //loại vote sao
            var groupVoteStarSelectedAnswers = selectedList.Where(o => o.NumStart != null).GroupBy(o => new { o.TheSurveyId, o.QuestionCode, o.NumStart }, o => new { o.NumStart });
            foreach(var item in groupVoteStarSelectedAnswers)
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
                if(!(surveyRound.Status == (int)SurveyRoundStatus.Closed || surveyRound.Status == (int)SurveyRoundStatus.End || DateTime.Now >= surveyRound.EndDate))
                {
                    throw new HandleException.InvalidInputDataException("Đợt khảo sát chưa đóng hoặc chưa kết thúc");
                }

                //do chỉ có một bài ks nên lấy id của bài ks đó
                var idbaikscuadotnay = surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id).Select(o => o.Id).ToList();
                if(idbaikscuadotnay.Count == 0)
                {
                    throw new RecordNotFoundException("Không tìm thấy bài khảo sát của đợt khảo sát này");
                }

                _logger.LogInformation("report total normal is start.");
                //surveyContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE {TableNameTask.AsEduSurveyReportTotal}");
                var baikshoanthanh = surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => idbaikscuadotnay.Contains(o.BaiKhaoSatId)).Where(o => o.Status == (int)SurveyStudentStatus.Done);
                var skip = 0;
                var take = 1000;
                
                var tongBaiKsHoanThanh = baikshoanthanh.Count();

                List<SelectedAnswerExtend> selectedAnswers;
                while (skip <= tongBaiKsHoanThanh)
                {
                    var chiaNhoBaiKsHoanThanh = baikshoanthanh
                    .Skip(skip).Take(take)
                    .ToList();

                    var groupLopGiangVien = chiaNhoBaiKsHoanThanh
                    .GroupBy(o => new { o.LecturerCode, o.ClassRoomCode, o.BaiKhaoSatId })
                    .Select(r => new { r.Key.LecturerCode, r.Key.ClassRoomCode, r.Key.BaiKhaoSatId });

                    foreach (var lectureClassroom in groupLopGiangVien)
                    {
                        var lectureCode = lectureClassroom.LecturerCode;
                        var classroomCode = lectureClassroom.ClassRoomCode;

                        //từng giảng viên lớp môn học
                        var cacBaiLam = baikshoanthanh.Where(o => o.ClassRoomCode == classroomCode && o.LecturerCode == lectureCode && !string.IsNullOrEmpty(o.BaiLam)).ToList();
                        selectedAnswers = new List<SelectedAnswerExtend>();

                        foreach (var bailam in cacBaiLam)
                        {
                            var json = JsonSerializer.Deserialize<List<SelectedAnswerExtend>>(bailam.BaiLam);
                            json.ForEach(o => o.TheSurveyId = bailam.BaiKhaoSatId);
                            selectedAnswers.AddRange(json);
                        }

                        var total = AnswerSelectedReportTotal(selectedAnswers);

                        foreach (var item in total)
                        {
                            var thongkecuthe = surveyContext.AsEduSurveyReportTotal.FirstOrDefault(o => o.ClassRoomCode == classroomCode && o.LecturerCode == lectureCode && o.TheSurveyId == item.TheSurveyId);
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
                            else
                            {
                                thongkecuthe.QuestionCode = item.QuestionCode;
                                thongkecuthe.AnswerCode = item.AnswerCode;
                                thongkecuthe.Content = item.Content;
                                thongkecuthe.Total = item.Total;
                            }
                        }
                    }
                    surveyContext.SaveChanges();
                    _logger.LogInformation($"report total normal loading: {skip}/{tongBaiKsHoanThanh}");
                    skip += take;
                }

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
                status.Message = e.Message;
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

                //do chỉ có một bài ks nên lấy id của bài ks đó
                var idbaikscuadotnay = surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id).Select(o => o.Id).ToList();
                if (idbaikscuadotnay.Count == 0)
                {
                    throw new RecordNotFoundException("Không tìm thấy bài khảo sát của đợt khảo sát này");
                }

                var tatCaBaiLamKs = surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => idbaikscuadotnay.Contains(o.BaiKhaoSatId));
                var result = new List<TempDataNormal>();

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

                    //sinh viên có đk
                    var tatCaBaiKs = surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => allStudents.Contains(o.StudentCode));

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

        /// <summary>
        /// Thống kê tạm
        /// </summary>
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
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.DefaultRowHeight = 12;

                #region kết xuất

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
            catch(Exception e)
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

            surveyRoundIds.ForEach(id =>
            {
                var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
                if (surveyRound == null)
                {
                    throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
                }
            });

            _backgroundTaskWorker.StartAction(() =>
            {
                ExportReportTotalNormalSurveyBG(surveyRoundIds);
            });
        }
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

                    foreach(var item in total)
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

        public async System.Threading.Tasks.Task ReportTotalUndergraduateSurvey(Guid surveyRoundId)
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
