using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyDapAnService
    {
        public Task<List<Answer>> GetByQuestionIdActiveStatus(string questionsId);
        public Task<Answer> GetById(string id);
        public Task Create(AnswerCreate answer);
        public Task Update(string id, AnswerUpdate answer);
        public Task Delete(string id);
    }
}
