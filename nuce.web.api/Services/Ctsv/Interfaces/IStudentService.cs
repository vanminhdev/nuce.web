using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Interfaces
{
    public interface IStudentService
    {
        public AsAcademyStudent GetStudentByCode(string studentCode);
    }
}
