using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyBaiKhaoSatSinhVienService
    {
        public Task SaveSelectedAnswer(string id, string ipAddress);
        public Task<Guid> GetIdByCode(string studentCode, string classroomCode);
        public Task GenerateTheSurveyStudent();
        public Task<int> GetGenerateTheSurveyStudentStatus();
        public Task<List<TheSurveysStudent>> GetTheSurvey(string studentCode);
        public Task<string> GetTheSurveyJsonStringByBaiKhaoSatId(string id);
        public Task<string> GetSelectedAnswerAutoSave(string studentCode, string classroomCode);
        public Task AutoSave(string id, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, bool isAnswerCodesAdd = true);
    }
}
