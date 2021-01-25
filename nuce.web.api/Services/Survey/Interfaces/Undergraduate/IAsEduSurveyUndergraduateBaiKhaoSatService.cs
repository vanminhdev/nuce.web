using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using nuce.web.api.ViewModel.Survey.Undergraduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyUndergraduateBaiKhaoSatService
    {
        public Task<PaginationModel<UndergraduateTheSurvey>> GetTheSurvey(UndergraduateTheSurveyFilter filter, int skip = 0, int take = 20);
        public Task Create(UndergraduateTheSurveyCreate theSurvey);
        public Task Update(Guid id, UndergraduateTheSurveyUpdate theSurvey);
        public Task Delete(Guid id);
        public Task<AsEduSurveyUndergraduateBaiKhaoSat> GetTheSurveyById(Guid id);
        public Task Deactive(Guid value);
        public Task<List<AsEduSurveyUndergraduateBaiKhaoSat>> GetTheSurveyDoing();
    }
}
