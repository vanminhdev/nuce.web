using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Graduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyGraduateStudentService
    {
        public Task<bool> Login(string masv, string pwd);

        public Task<PaginationModel<GraduateStudent>> GetAll(GraduateStudentFilter filter, int skip = 0, int take = 20);

        public Task<AsEduSurveyGraduateStudent> GetById(string id);

        public Task Create(AsEduSurveyGraduateStudent student);

        public Task CreateAll(List<AsEduSurveyGraduateStudent> students);

        public Task TruncateTable();

        public Task TransferDataFromUndergraduate(Guid surveyRoundId, TransferDataUndergraduateModel filter);

        public Task<byte[]> DownloadListStudent(Guid surveyRoundId);

        public Task Delete(string studentCode);
    }
}
