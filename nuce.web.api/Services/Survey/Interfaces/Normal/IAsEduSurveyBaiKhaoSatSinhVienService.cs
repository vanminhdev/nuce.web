using nuce.web.api.ViewModel.Survey;
using nuce.web.api.ViewModel.Survey.Normal.TheSurvey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyBaiKhaoSatSinhVienService
    {
        public Task SaveSelectedAnswer(string studentCode, string classroomCode, string nhhk, string ipAddress);
        public Task<int> GetGenerateTheSurveyStudentStatus();
        public Task<List<TheSurveyStudent>> GetTheSurvey(string studentCode);
        public Task<TheSurveyContent> GetTheSurveyContent(string studentCode, string classroomCode, string nhhk, Guid theSurveyId);
        public Task<string> GetSelectedAnswerAutoSave(string studentCode, string classroomCode, string nhhk);
        public Task AutoSave(string studentCode, string classroomCode, string nhhk, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, int? numStar, string city, bool isAnswerCodesAdd = true);
        
        /// <summary>
        /// Item1 : số bài ks sinh viên
        /// Item2 : số sinh viên lớp môn học
        /// </summary>
        /// <param name="surveyRoundId"></param>
        /// <returns></returns>
        public Task<Tuple<int,int>> CountGenerateTheSurveyStudent(Guid surveyRoundId);
    }
}
