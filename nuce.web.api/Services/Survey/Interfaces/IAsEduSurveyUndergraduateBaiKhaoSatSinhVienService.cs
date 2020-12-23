using nuce.web.api.ViewModel.Survey;
using nuce.web.api.ViewModel.Survey.Undergraduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyUndergraduateBaiKhaoSatSinhVienService
    {
        public Task SaveSelectedAnswer(Guid theSurveyId, string studentCode, string ipAddress);
        public Task GenerateTheSurveyStudent(Guid theSurveyId);
        public Task<int> GetGenerateTheSurveyStudentStatus();
        public Task<List<UndergraduateTheSurveyStudent>> GetTheSurvey(string studentCode);
        public Task<string> GetTheSurveyContent(string studentCode, Guid theSurveyId);
        public Task<string> GetSelectedAnswerAutoSave(Guid theSurveyId, string studentCode);
        public Task AutoSave(Guid theSurveyId, string studentCode, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, bool isAnswerCodesAdd = true);

        public Task<string> Verification(string studentCode, VerificationStudent verification);
        public Task<bool> VerifyByToken(string studentCode, string token);
        public Task SendEmailVerify(string email, string url);
    }
}
