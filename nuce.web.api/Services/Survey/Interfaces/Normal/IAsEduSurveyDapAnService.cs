using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyDapAnService
    {
        public Task<List<AnswerModel>> GetByQuestionIdActiveStatus(string questionsId);
        public Task<AnswerModel> GetById(string id);
        public Task Create(AnswerCreateModel answer);
        public Task Update(string id, AnswerUpdateModel answer);
        public Task Delete(string id);
    }
}
