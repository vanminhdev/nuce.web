using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyUndergraduateDeThiService
    {
        public Task<PaginationModel<ExamQuestions>> GetAll(ExamQuestionsFilter filter, int skip = 0, int take = 20);
        public Task<List<ExamStructure>> GetExamStructure(string id);
        public Task AddQuestion(Guid examQuestionId, string maCauHoi, int order);
        public Task GenerateExam(GenerateExam generateExam);
        public Task<string> GetExamDetailJsonString(string examQuestionId);
        public Task Create(ExamQuestionsCreate exam);
        public Task DeleteQuestionFromStructure(string id);
        public Task Delete(Guid value);
    }
}
