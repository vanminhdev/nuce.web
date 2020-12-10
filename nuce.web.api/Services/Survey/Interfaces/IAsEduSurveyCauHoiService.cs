using nuce.web.api.Common;
using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyCauHoiService
    {
        public Task<PaginationModel<QuestionModel>> GetAllActiveStatus(QuestionFilter filter, int skip = 0, int pageSize = 20);
        public Task<List<QuestionModel>> GetAllByStatus(QuestionStatus status);
        public Task<QuestionModel> GetById(string id);
        public Task Create(QuestionCreateModel question);
        public Task Update(string id, QuestionUpdateModel question);
        public Task Delete(string id);
    }
}
