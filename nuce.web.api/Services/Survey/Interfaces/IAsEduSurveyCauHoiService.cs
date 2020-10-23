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
        public Task<List<Question>> GetAllActiveStatus();
        public Task<List<Question>> GetAllByStatus(QuestionStatus status);
        public Task<Question> GetById(string id);
        public Task Create(QuestionCreate question);
        public Task Update(string id, QuestionUpdate question);
        public Task Delete(string id);
    }
}
