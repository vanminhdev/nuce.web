﻿using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Survey;
using nuce.web.shared.Models.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface ISurveyResultService
    {
        public Task<FacultyResultModel> FacultyResultAsync(string code);
        public Task<DepartmentResultModel> DepartmentResultAsync(string code);
        public Task<SurveyResultResponseModel> LecturerResultAsync(string code);
    }
}
