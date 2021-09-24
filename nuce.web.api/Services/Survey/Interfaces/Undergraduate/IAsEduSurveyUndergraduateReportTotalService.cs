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

        /// <summary>
        /// Thống kê nạp vào bảng report total
        /// </summary>
        /// <param name="surveyRoundId"></param>
        /// <param name="theSurveyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        public void ReportTotalUndergraduateSurvey(Guid surveyRoundId, Guid theSurveyId, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Kết xuất ra file excel
        /// </summary>
        /// <param name="surveyRoundId"></param>
        /// <param name="theSurveyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public Task<byte[]> ExportReportTotalUndergraduateSurvey(Guid surveyRoundId, Guid theSurveyId, DateTime fromDate, DateTime toDate);
    }
}
