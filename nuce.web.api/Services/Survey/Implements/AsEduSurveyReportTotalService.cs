using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Normal.Statistic;
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
        private readonly EduDataContext _eduContext;
        private readonly IStatusService _statusService;

        public AsEduSurveyReportTotalService(ILogger<AsEduSurveyReportTotalService> logger, SurveyContext context, EduDataContext eduContext, IStatusService statusService)
        {
            _logger = logger;
            _context = context;
            _eduContext = eduContext;
            _statusService = statusService;
        }

        public async Task<PaginationModel<ReportTotalNormal>> GetRawReportTotalNormalSurvey(ReportTotalNormalFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyReportTotal> query = null;
            var recordsTotal = _context.AsEduSurveyReportTotal.Count();


            var recordsFiltered = recordsTotal;
            if(query != null)
            {
                recordsFiltered = query.Count();
            }

            var result = await _context.AsEduSurveyReportTotal
                .Skip(skip).Take(take)
                .Join(_context.AsEduSurveyDotKhaoSat, o => o.SurveyRoundId, o => o.Id, (report, surveyRound) => new { report, surveyRound })
                .Join(_context.AsEduSurveyBaiKhaoSat, o => o.report.TheSurveyId, o => o.Id, (reportSurveyRound, theSurvey) => new { reportSurveyRound, theSurvey })
                .Select(o => new ReportTotalNormal
                {
                    SurveyRoundId = o.reportSurveyRound.surveyRound.Id,
                    SurveyRoundName = o.reportSurveyRound.surveyRound.Name,
                    TheSurveyId = o.theSurvey.Id,
                    TheSurveyName = o.theSurvey.Name,
                    Id = o.reportSurveyRound.report.Id,
                    LecturerCode = o.reportSurveyRound.report.LecturerCode,
                    ClassRoomCode = o.reportSurveyRound.report.ClassRoomCode,
                    QuestionCode = o.reportSurveyRound.report.QuestionCode,
                    AnswerCode = o.reportSurveyRound.report.AnswerCode,
                    Total = o.reportSurveyRound.report.Total,
                    Content = o.reportSurveyRound.report.Content
                }).ToListAsync();

            return new PaginationModel<ReportTotalNormal>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = result
            };
        }

        public async Task<List<TempDataNormal>> GetTempDataNormalSurvey(Guid? surveyRoundId)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            //do chỉ có một bài ks nên lấy id của bài ks đó
            var idbaikscuadotnay = _context.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id).Select(o => o.Id).ToList();
            if (idbaikscuadotnay.Count == 0)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát của đợt khảo sát này");
            }

            var tatCaBaiLamKs = _context.AsEduSurveyBaiKhaoSatSinhVien.Where(o => idbaikscuadotnay.Contains(o.BaiKhaoSatId));
            var result = new List<TempDataNormal>();

            var facultys = await _eduContext.AsAcademyFaculty.ToListAsync();
            foreach(var f in facultys)
            {
                _logger.LogInformation($"Đang thong ke tam cho khoa có mã {f.Code}");
                var classF = await _eduContext.AsAcademyClass.Where(o => o.FacultyCode == f.Code).Select(o => o.Code).ToListAsync();
                //tất cả sv có đk
                var allStudents = await _eduContext.AsAcademyStudent
                    .Where(o => classF.Contains(o.ClassCode))
                    .Where(o => _eduContext.AsAcademyStudentClassRoom.FirstOrDefault(sc => sc.StudentCode == o.Code) != null)
                    .Select(o => o.Code)
                    .ToListAsync();

                //sinh viên có đk
                var tatCaBaiKs = _context.AsEduSurveyBaiKhaoSatSinhVien.Where(o => allStudents.Contains(o.StudentCode));

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
            return result;
        }
    }
}
