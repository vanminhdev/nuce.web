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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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

        private void ReportTotalNormalSurveyBG(Guid surveyRoundId)
        {
            var scope = _scopeFactory.CreateScope();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();

            var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            //đợt khảo sát chưa kết thúc
            if(surveyRound.Status != (int)SurveyRoundStatus.Closed)
            {
                throw new InvalidDataException("Đợt khảo sát chưa kết thúc chưa thể thống kê");
            }

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
                _logger.LogInformation("report total normal is start.");
                surveyContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE {TableNameTask.AsEduSurveyReportTotal}");
                var query = surveyContext.AsEduSurveyBaiKhaoSatSinhVien;
                var countTheSurveyStudent = query.Count();
                var skip = 0;
                var take = 500;
                var semesterId = -1;

                var lectureClassroomCode = surveyContext.AsEduSurveyBaiKhaoSatSinhVien
                    .GroupBy(o => new { o.LecturerCode, o.ClassRoomCode, o.BaiKhaoSatId })
                    .Select(r => new { r.Key.LecturerCode, r.Key.ClassRoomCode, r.Key.BaiKhaoSatId });

                List<SelectedAnswer> selectedAnswers;
                while (skip <= countTheSurveyStudent)
                {
                    var list = lectureClassroomCode
                    .Skip(skip).Take(take)
                    .ToList();

                    foreach (var lectureClassroom in list)
                    {
                        var lectureCode = lectureClassroom.LecturerCode;
                        var classroomCode = lectureClassroom.ClassRoomCode;
                        var baikhaosatId = lectureClassroom.BaiKhaoSatId;

                        //từng giảng viên lớp môn học
                        var answers = surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => o.ClassRoomCode == classroomCode && o.LecturerCode == lectureCode && !string.IsNullOrEmpty(o.BaiLam)).ToList();
                        selectedAnswers = new List<SelectedAnswer>();

                        foreach (var answer in answers)
                        {
                            var json = JsonSerializer.Deserialize<List<SelectedAnswer>>(answer.BaiLam);
                            selectedAnswers.AddRange(json);
                        }

                        //loại câu chọn 1
                        var groupSingleChoiceSelectedAnswers = selectedAnswers.Where(o => o.AnswerCode != null).GroupBy(o => new { o.QuestionCode, o.AnswerCode });
                        foreach (var item in groupSingleChoiceSelectedAnswers)
                        {
                            surveyContext.AsEduSurveyReportTotal.Add(new AsEduSurveyReportTotal
                            {
                                Id = Guid.NewGuid(),
                                SemesterId = semesterId,
                                CampaignId = baikhaosatId,
                                ClassRoomCode = classroomCode,
                                LecturerCode = lectureCode,
                                QuestionCode = item.Key.QuestionCode,
                                QuestionType = QuestionType.SC,
                                AnswerCode = item.Key.AnswerCode,
                                Total = item.Count()
                            });
                        }

                        //loại câu chọn nhiều
                        var groupMultiChoiceSelectedAnswers = selectedAnswers
                            .Where(o => o.AnswerCodes != null && o.AnswerCodes.Count > 0)
                            .SelectMany(o => o.AnswerCodes, (r, AnswerCode) => new { r.QuestionCode, AnswerCode })
                            .GroupBy(o => new { o.QuestionCode, o.AnswerCode });

                        foreach (var item in groupMultiChoiceSelectedAnswers)
                        {
                            surveyContext.AsEduSurveyReportTotal.Add(new AsEduSurveyReportTotal
                            {
                                Id = Guid.NewGuid(),
                                SemesterId = semesterId,
                                CampaignId = baikhaosatId,
                                ClassRoomCode = classroomCode,
                                LecturerCode = lectureCode,
                                QuestionCode = item.Key.QuestionCode,
                                QuestionType = QuestionType.MC,
                                AnswerCode = item.Key.AnswerCode,
                                Total = item.Count()
                            });
                        }

                        //loại câu trả lời text
                        var groupShortAnswerSelectedAnswers = selectedAnswers.Where(o => o.AnswerContent != null).GroupBy(o => new { o.QuestionCode }, o => new { o.AnswerContent });
                        foreach (var item in groupShortAnswerSelectedAnswers)
                        {
                            string strAllAnswerContent = "";
                            foreach (var str in item)
                            {
                                strAllAnswerContent += $",{str.AnswerContent}";
                            }
                            surveyContext.AsEduSurveyReportTotal.Add(new AsEduSurveyReportTotal
                            {
                                Id = Guid.NewGuid(),
                                SemesterId = semesterId,
                                CampaignId = baikhaosatId,
                                ClassRoomCode = classroomCode,
                                LecturerCode = lectureCode,
                                QuestionCode = item.Key.QuestionCode,
                                QuestionType = QuestionType.SA,
                                Content = strAllAnswerContent
                            });
                        }
                    }
                    surveyContext.SaveChanges();
                    _logger.LogInformation($"report total normal loading: {skip}/{countTheSurveyStudent}");
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

        public async System.Threading.Tasks.Task ReportTotalNormalSurvey(Guid surveyRoundId)
        {
            var status = await _statusService.GetStatusTableTask(TableNameTask.AsEduSurveyReportTotal);
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Bảng đang làm việc, thao tác bị huỷ");
            }

            _backgroundTaskWorker.StartAction(() =>
            {
                ReportTotalNormalSurveyBG(surveyRoundId);
            });
        }

    }
}
