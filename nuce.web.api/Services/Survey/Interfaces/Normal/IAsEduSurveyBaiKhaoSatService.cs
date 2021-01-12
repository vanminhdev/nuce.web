using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyBaiKhaoSatService
    {
        public Task<PaginationModel<TheSurvey>> GetTheSurvey(TheSurveyFilter filter, int skip = 0, int take = 20);
        public Task Create(TheSurveyCreate theSurvey);
        public Task Update(Guid id, TheSurveyUpdate theSurvey);
        public Task Delete(Guid id);
        public Task<AsEduSurveyBaiKhaoSat> GetTheSurveyById(Guid id);
        public Task Deactive(Guid value);
    }
}
