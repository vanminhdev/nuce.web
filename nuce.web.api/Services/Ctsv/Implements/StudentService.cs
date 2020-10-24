using nuce.web.api.Models.Ctsv;
using nuce.web.api.Services.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class StudentService : IStudentService
    {
        private readonly CTSVNUCE_DATAContext _context;
        public StudentService()
        {
            this._context = new CTSVNUCE_DATAContext();
        }

        public AsAcademyStudent GetStudentByCode(string studentCode)
        {
            return _context.AsAcademyStudent.FirstOrDefault(s => s.Code == studentCode);
        }
    }
}
