using nuce.web.api.Models.EduData;
using nuce.web.api.Services.EduData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.EduData.Implements
{
    public class StudentEduDataService
    {
        private readonly EduDataContext _eduDataContext;
        public StudentEduDataService(EduDataContext _eduDataContext)
        {
            this._eduDataContext = _eduDataContext;
        }
        public AsAcademyStudent FindByCode(string code)
        {
            return _eduDataContext.AsAcademyStudent.FirstOrDefault(s => s.Code == code);
        }
    }
}
