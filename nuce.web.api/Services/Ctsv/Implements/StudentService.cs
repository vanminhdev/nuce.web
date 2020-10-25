using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Services.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository _studentRepository)
        {
            this._studentRepository = _studentRepository;
        }

        public AsAcademyStudent GetStudentByCode(string studentCode)
        {
            return _studentRepository.FindByCode(studentCode);
        }
    }
}
