using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyDeThiService
    {
        public Task<List<ExamQuestions>> GetAll();
        public Task<List<ExamStructure>> GetExamStructure(string id);
        public Task AddQuestion(string examQuestionId, string maCauHoi, int order);
        public Task GenerateExam(string examQuestionId);
        public Task<string> GetExamDetailJsonString(string examQuestionId);
        public Task CreateExamQuestions(string code, string name);
        public Task DeleteQuestionFromStructure(string id);
    }
}
