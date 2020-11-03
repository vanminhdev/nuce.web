using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IQuaTrinhHocRepository
    {
        public Task<List<AsAcademyStudentQuaTrinhHocTap>> FindByCodeAsync(string studentCode);
    }
}
