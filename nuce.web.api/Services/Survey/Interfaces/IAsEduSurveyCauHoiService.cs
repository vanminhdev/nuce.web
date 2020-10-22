using nuce.web.api.Models.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Interfaces
{
    public interface IAsEduSurveyCauHoiService
    {
        public Task<List<AsEduSurveyCauHoi>> GetAll();
    }
}
