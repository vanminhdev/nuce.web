using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Graduate;
using nuce.web.api.ViewModel.Survey.Undergraduate.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyGraduateReportTotalService
    {
        public Task<byte[]> ExportReportTotalGraduateSurvey(Guid surveyRoundId);

        public Task<List<TempDataModel>> TempDataGraduateSurvey(Guid surveyRoundId);
    }
}
