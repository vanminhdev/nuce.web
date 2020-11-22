using Microsoft.AspNetCore.Http;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Interfaces
{
    public interface IStudentService
    {
        public AsAcademyStudent GetStudentByCode(string studentCode);
        public StudentAllowUpdateModel GetStudentByCodeAllowUpdate(string studentCode);
        public Task<FullStudentModel> GetFullStudentByCode(string studentCode);
        public Task<ResponseBody> UpdateStudentBasic(StudentUpdateModel basicStudent);
        public Task<ResponseBody> UpdateStudent(AsAcademyStudent student);
        public Task<string> UpdateStudentImage(IFormFile formFile, string studentCode);
    }
}
