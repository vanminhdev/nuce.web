using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyBaiKhaoSatSinhVienService
    {
        public Task SaveSelectedAnswer(string studentCode, string classroomCode, string ipAddress);
        public Task<int> GetGenerateTheSurveyStudentStatus();
        public Task<List<TheSurveyStudent>> GetTheSurvey(string studentCode);
        public Task<string> GetTheSurveyContent(string studentCode, string classroomCode, Guid theSurveyId);
        public Task<string> GetSelectedAnswerAutoSave(string studentCode, string classroomCode);
        public Task AutoSave(string studentCode, string classroomCode, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, int? numStar, string city, bool isAnswerCodesAdd = true);
    }
}
