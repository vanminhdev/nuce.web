﻿using nuce.web.api.ViewModel.Survey;
using nuce.web.api.ViewModel.Survey.Graduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyGraduateBaiKhaoSatSinhVienService
    {
        public Task SaveSelectedAnswer(Guid theSurveyId, string studentCode, string ipAddress, string loaiHinh);
        public Task GenerateTheSurveyStudent(Guid theSurveyId);
        public Task<int> GetGenerateTheSurveyStudentStatus();
        public Task<List<GraduateTheSurveyStudent>> GetTheSurvey(string studentCode);
        public Task<List<GraduateTheSurveyStudent>> GetTheSurvey(string facultyCode, string studentCode);
        public Task<string> GetTheSurveyContent(string studentCode, Guid theSurveyId);
        public Task<string> GetSelectedAnswerAutoSave(Guid theSurveyId, string studentCode);
        public Task AutoSave(Guid theSurveyId, string studentCode, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, int? numStar, string city, bool isAnswerCodesAdd = true);
    }
}
