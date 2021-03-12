using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Undergraduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyUndergraduateStudentService
    {
        public Task<PaginationModel<UndergraduateStudent>> GetAll(UndergraduateStudentFilter filter, int skip = 0, int take = 20);

        public Task<AsEduSurveyUndergraduateStudent> GetById(string id);

        public Task<AsEduSurveyUndergraduateStudent> GetByStudentCode(string studentCode);

        public Task Create(AsEduSurveyUndergraduateStudent student);

        public Task CreateAll(List<AsEduSurveyUndergraduateStudent> students);

        public Task TruncateTable();

        public Task Delete(string studentCode);

        public Task<byte[]> DownloadListStudent(DateTime? fromDate, DateTime? toDate);
    }
}
