using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyReportTotalService : IAsEduSurveyReportTotalService
    {
        ILogger<AsEduSurveyReportTotalService> _logger;
        private readonly SurveyContext _context;
        private readonly IStatusService _statusService;

        public AsEduSurveyReportTotalService(ILogger<AsEduSurveyReportTotalService> logger, SurveyContext context, IStatusService statusService)
        {
            _logger = logger;
            _context = context;
            _statusService = statusService;
        }

        public async Task ReportTotalGoingToGraduateSurvey()
        {
            throw new NotImplementedException();
        }

        public async Task ReportTotalGraduateSurvey()
        {
            throw new NotImplementedException();
        }

        public async Task ReportTotalNormalSurvey()
        {
            _logger.LogInformation("Start report total normal survey");
            _context.Database.ExecuteSqlRaw($"TRUNCATE TABLE {TableNameTask.AsEduSurveyReportTotal}");
            var query = _context.AsEduSurveyBaiKhaoSatSinhVien;
            var countTheSurveyStudent = query.Count();

            var skip = 0;
            var take = 500;

            var semesterId = await _statusService.GetLastSemesterId();

            var lectureClassroomCode = _context.AsEduSurveyBaiKhaoSatSinhVien
                .GroupBy(o => new { o.LecturerCode, o.ClassRoomCode, o.BaiKhaoSatId })
                .Select(r => new { r.Key.LecturerCode, r.Key.ClassRoomCode, r.Key.BaiKhaoSatId });

            var timer = new Stopwatch();
            timer.Restart();
            List<SelectedAnswer> selectedAnswers;
            while (skip <= countTheSurveyStudent)
            {
                _logger.LogInformation($"report total normal: skip = {skip} take = {take}");
                
                var list = await lectureClassroomCode
                .Skip(skip).Take(take)
                .ToListAsync();
                
                foreach (var lectureClassroom in list)
                {
                    var lectureCode = lectureClassroom.LecturerCode;
                    var classroomCode = lectureClassroom.ClassRoomCode;
                    var baikhaosatId = lectureClassroom.BaiKhaoSatId;

                    //từng giảng viên lớp môn học
                    var answers = await _context.AsEduSurveyBaiKhaoSatSinhVien.Where(o => o.ClassRoomCode == classroomCode && o.LecturerCode == lectureCode && !string.IsNullOrEmpty(o.BaiLam)).ToListAsync();
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
                        _context.AsEduSurveyReportTotal.Add(new AsEduSurveyReportTotal
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
                        .SelectMany(o => o.AnswerCodes, (r, AnswerCode) => new {r.QuestionCode, AnswerCode })
                        .GroupBy(o => new { o.QuestionCode, o.AnswerCode });

                    foreach (var item in groupMultiChoiceSelectedAnswers)
                    {
                        _context.AsEduSurveyReportTotal.Add(new AsEduSurveyReportTotal
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
                        _context.AsEduSurveyReportTotal.Add(new AsEduSurveyReportTotal
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
                skip += take;
                await _context.SaveChangesAsync();
            }
            timer.Stop();
            _logger.LogInformation($"total time repost: {timer.Elapsed.TotalMilliseconds}");
            _logger.LogInformation($"report total normal done");
        }
    }
}
