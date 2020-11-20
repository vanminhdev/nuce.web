using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyBaiKhaoSatService : IAsEduSurveyBaiKhaoSatService
    {
        private readonly SurveyContext _surveyContext;

        public AsEduSurveyBaiKhaoSatService(SurveyContext surveyContext)
        {
            _surveyContext = surveyContext;
        }
    }
}
