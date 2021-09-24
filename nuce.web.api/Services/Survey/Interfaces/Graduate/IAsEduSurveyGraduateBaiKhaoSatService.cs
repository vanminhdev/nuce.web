using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using nuce.web.api.ViewModel.Survey.Graduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyGraduateBaiKhaoSatService
    {
        public Task<PaginationModel<GraduateTheSurvey>> GetTheSurvey(GraduateTheSurveyFilter filter, int skip = 0, int take = 20);
        public Task Create(GraduateTheSurveyCreate theSurvey);
        public Task Update(Guid id, GraduateTheSurveyUpdate theSurvey);
        public Task Delete(Guid id);
        public Task<AsEduSurveyGraduateBaiKhaoSat> GetTheSurveyById(Guid id);
        public Task Deactive(Guid value);
    }
}
