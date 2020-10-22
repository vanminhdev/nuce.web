using nuce.web.api.Common;
using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyCauHoiService
    {
        public Task<List<Question>> GetAllActiveStatusAsync();
        public Task<List<Question>> GetAllByStatusAsync(QuestionStatus status);
        public Task<Question> GetById(string id);
        public Task CreateQuestion(Question question);
        public Task UpdateQuestion(string id, Question question);
    }
}
