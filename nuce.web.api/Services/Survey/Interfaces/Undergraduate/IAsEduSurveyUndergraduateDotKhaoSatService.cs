using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Undergraduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyUndergraduateDotKhaoSatService
    {
        public Task<PaginationModel<AsEduSurveyUndergraduateSurveyRound>> GetSurveyRound(UndergraduateSurveyRoundFilter filter, int skip = 0, int take = 20);

        public Task<AsEduSurveyUndergraduateSurveyRound> GetSurveyRoundById(string id);

        public Task Create(UndergraduateSurveyRoundCreate surveyRound);

        public Task Update(Guid id, UndergraduateSurveyRoundUpdate surveyRound);

        public Task Delete(Guid id);

        public Task Open(Guid id);

        public Task Close(Guid id);

        public Task<List<AsEduSurveyUndergraduateSurveyRound>> GetSurveyRoundActive();
        public Task<List<AsEduSurveyUndergraduateSurveyRound>> GetAllSurveyRound();
    }
}
