using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IGiaDinhRepository
    {
        public Task<List<AsAcademyStudentGiaDinh>> FindByCodeAsync(string studentCode);
    }
}
