using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyDotKhaoSatService
    {
        public Task<PaginationModel<AsEduSurveyDotKhaoSat>> GetSurveyRound(SurveyRoundFilter filter, int skip = 0, int take = 20);

        public Task<AsEduSurveyDotKhaoSat> GetSurveyRoundById(Guid id);

        public Task Create(SurveyRoundCreate surveyRound);

        public Task Update(Guid id, SurveyRoundUpdate surveyRound);

        public Task Delete(Guid id);

        public Task Close(Guid id);

        public Task<List<AsEduSurveyDotKhaoSat>> GetSurveyRoundActive();
        public Task<List<AsEduSurveyDotKhaoSat>> GetSurveyRoundEnd();
    }
}
