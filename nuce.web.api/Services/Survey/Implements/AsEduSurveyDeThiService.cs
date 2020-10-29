using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    public class AsEduSurveyDeThiService : IAsEduSurveyDeThiService

    {
        private readonly SurveyContext _surveyContext;

        public AsEduSurveyDeThiService(SurveyContext surveyContext)
        {
            _surveyContext = surveyContext;
        }
    }
}
