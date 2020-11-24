using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyBaiKhaoSatSinhVienService
    {
        public Task SaveTask(string id, string task, string ipAddress);
        public Task<Guid> GetIdByCode(string studentCode, string classroomCode);
        public Task GenerateTheSurveyStudent();
    }
}
