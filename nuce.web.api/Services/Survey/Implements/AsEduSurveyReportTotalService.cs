using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
using System.Net.Http;
using System.Net.Mime;
using System.Text;
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
        private readonly IConfiguration _configuration;

        public AsEduSurveyReportTotalService(ILogger<AsEduSurveyReportTotalService> logger, SurveyContext context, EduDataContext eduContext, IStatusService statusService, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _eduContext = eduContext;
            _statusService = statusService;
            _configuration = configuration;
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

        class DataUrgingEmail
        {
            public string percent { get; set; }
        }

        class UrgingEmail
        {
            public string email { get; set; }
            public DataUrgingEmail data { get; set; }
        }

        public async Task SendUrgingEmail()
        {
            var status = await _statusService.GetStatusTableTaskNotResetMessage(TableNameTask.TempDataNormalSurvey);
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Đang thống kê tạm, thao tác bị huỷ");
            }

            var lstKhoa = JsonSerializer.Deserialize<List<TempDataNormal>>(status.Message);
            var lstEmailTarget = new List<UrgingEmail>();
            foreach(var khoa in lstKhoa)
            {
                if(khoa.Total > 0 && khoa.Num/khoa.Total < 0.5)
                {
                    var thongTin = await _eduContext.AsAcademyFaculty.FirstOrDefaultAsync(o => o.Code == khoa.FacultyCode);
                    lstEmailTarget.Add(new UrgingEmail
                    {
                        email = "vanminh.dev@gmail.com",
                        data = new DataUrgingEmail
                        {
                            percent = "abc"
                        }
                    });
                }
            }
            HttpClient client = new HttpClient();
            var strContent = JsonSerializer.Serialize(new
            {
                emails = lstEmailTarget,
                template = 25,
                subject = "V/v Thông báo số lượng bài khảo sát của sinh viên về hoạt động giáo dục",
                email_identifier = "emails",
                datetime = DateTime.Now.ToString("dd-MM-yyyy hh:mm"),
                send_later_email = 0,
                timezone = 7
            });
            var content = new StringContent(strContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync(_configuration.GetValue<string>("ApiSendEmail"), content);
            if (!response.IsSuccessStatusCode)
            {
                throw new SendEmailException("Yêu cầu api gửi email không thành công");
            }
        }
    }
}
