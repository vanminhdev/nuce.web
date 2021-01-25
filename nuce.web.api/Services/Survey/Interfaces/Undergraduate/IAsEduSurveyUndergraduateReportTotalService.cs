using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Undergraduate.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyUndergraduateReportTotalService
    {
        public Task<PaginationModel<ReportTotalUndergraduate>> GetRawReportTotalUndergraduateSurvey(ReportTotalUndergraduateFilter filter, int skip = 0, int take = 20);

        public Task ReportTotalUndergraduateSurvey(Guid surveyRoundId, Guid theSurveyId);

        public Task<byte[]> ExportReportTotalUndergraduateSurvey(Guid surveyRoundId, Guid theSurveyId);
    }
}
