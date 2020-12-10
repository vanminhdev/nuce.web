using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Graduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyGraduateDotKhaoSatService
    {
        public Task<PaginationModel<AsEduSurveyGraduateSurveyRound>> GetSurveyRound(GraduateSurveyRoundFilter filter, int skip = 0, int take = 20);

        public Task<AsEduSurveyGraduateSurveyRound> GetSurveyRoundById(string id);

        public Task Create(GraduateSurveyRoundCreate surveyRound);

        public Task Update(Guid id, GraduateSurveyRoundUpdate surveyRound);

        public Task Delete(Guid id);

        public Task Close(Guid id);

        public Task<List<AsEduSurveyGraduateSurveyRound>> GetSurveyRoundActive();
    }
}
