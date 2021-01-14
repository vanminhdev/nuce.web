using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Normal.Statistic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private readonly ILogger<AsEduSurveyReportTotalService> _logger;
        private readonly SurveyContext _context;
        private readonly EduDataContext _eduContext;
        private readonly IStatusService _statusService;
        private readonly IConfiguration _configuration;
        private readonly IPathProvider _pathProvider;

        public AsEduSurveyReportTotalService(ILogger<AsEduSurveyReportTotalService> logger, SurveyContext context, EduDataContext eduContext, IStatusService statusService, IConfiguration configuration, IPathProvider pathProvider)
        {
            _logger = logger;
            _context = context;
            _eduContext = eduContext;
            _statusService = statusService;
            _configuration = configuration;
            _pathProvider = pathProvider;
        }

        public async Task<PaginationModel<ReportTotalNormal>> GetRawReportTotalNormalSurvey(ReportTotalNormalFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyReportTotal> query = null;
            var recordsTotal = await _context.AsEduSurveyReportTotal.CountAsync();

            var recordsFiltered = recordsTotal;
            if(query != null)
            {
                recordsFiltered = await query.CountAsync();
            }

            var result = await _context.AsEduSurveyReportTotal
                .Skip(skip).Take(take)
                .Join(_context.AsEduSurveyDotKhaoSat, o => o.SurveyRoundId, o => o.Id, (report, surveyRound) => new { report, surveyRound })
                .Join(_context.AsEduSurveyBaiKhaoSat, o => o.report.TheSurveyId, o => o.Id, (reportSurveyRound, theSurvey) => new { reportSurveyRound, theSurvey })
                .OrderByDescending(o => o.reportSurveyRound.surveyRound.FromDate)
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

        public async Task SendUrgingEmail()
        {
            var status = await _statusService.GetStatusTableTaskNotResetMessage(TableNameTask.TempDataNormalSurvey);
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Đang thống kê tạm, thao tác bị huỷ");
            }

            if (status.Status == (int)TableTaskStatus.Done && !status.IsSuccess)
            {
                throw new TableBusyException("Không thông kê tạm thành công không gửi được email");
            }

            string filePath = "Templates/Survey/template_mail_doc_thuc_khoa_ban.txt";
            var dir = _pathProvider.MapPath(filePath);
            if (!File.Exists(dir))
            {
                throw new FileNotFoundException("Không tìm thấy mẫu gửi email");
            }
            string templateContent = await File.ReadAllTextAsync(dir);

            var tempData = JsonSerializer.Deserialize<TempDataNormal>(status.Message);
            var str = "";
            var sumTotalDaLam = 0;
            var sumTotalChuaLam = 0;
            var sumTotalSinhVien = 0;
            tempData.TongHopKhoa.ForEach(item => {
                str += $"<tr>";
                str += $"<td style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;'>{item.FacultyCode}</td>";
                str += $"<td style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;'>{item.FacultyName}</td>";
                str += $"<td style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;'>{ item.TotalDaLam}</td>";
                       sumTotalDaLam += +item.TotalDaLam;
                str += $"<td style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;'>{ item.TotalChuaLam}</td>";
                       sumTotalChuaLam += +item.TotalChuaLam;
                str += $"<td style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;'>{ item.TotalSinhVien}</td>";
                       sumTotalSinhVien += +item.TotalSinhVien;
                str += $"<td style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;'>{ item.Percent}%</td>";
                str += $"</tr>";
            });
            str += $"<tr>";
            str += $"<td style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;' colspan = '2'>Toàn trường</td>";
            str += $"<td style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;'>{sumTotalDaLam}</td>";
            str += $"<td style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;'>{sumTotalChuaLam}</td>";
            str += $"<td style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;'>{sumTotalSinhVien}</td>";
            str += $"<td style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;'>{tempData.ChiemTiLe}%</td>";
            str += $"</tr style='border: 1px solid #ddd; text-align: center; padding-top: 10px; padding-bottom: 10px;'>";

            string contentEmail = templateContent
                .Replace("[thoi_gian_ket_thuc]", $"{tempData.ThoiGianKetThuc.ToString("HH:mm")} ngày {tempData.ThoiGianKetThuc.ToString("dd/MM/yyyy")}")
                .Replace("[ngay_hien_tai]", tempData.NgayHienTai.ToString("dd/MM/yyyy"))
                .Replace("[so_sv_ks]", tempData.SoSVKhaoSat.ToString())
                .Replace("[ty_le]", tempData.ChiemTiLe.ToString())
                .Replace("[nd_bang_thong_ke]", str)
                .Replace("\n", "")
                .Replace("\r", "");

            foreach (var khoa in tempData.TongHopKhoa)
            {
                var thongTin = await _eduContext.AsAcademyFaculty.FirstOrDefaultAsync(o => o.Code == khoa.FacultyCode);
                if (thongTin != null)
                {
                    HttpClient client = new HttpClient();
                    var strContent = JsonSerializer.Serialize(new
                    {
                        emails = new[] {
                            new {
                                email = "vanminh.dev@gmail.com",
                                data = new
                                {
                                    contentEmail
                                }
                            }
                        },
                        template = 28,
                        subject = "V/v Thông báo số lượng bài khảo sát của sinh viên về hoạt động giảng dạy",
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
    }
}
