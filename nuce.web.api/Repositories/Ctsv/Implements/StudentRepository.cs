﻿using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CTSVNUCE_DATAContext _context;
        public StudentRepository(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }

        public AsAcademyStudent FindByCode(string studentCode)
        {
            return _context.AsAcademyStudent.FirstOrDefault(student => student.Code == studentCode);
        }

        public void Update(AsAcademyStudent student)
        {
             _context.AsAcademyStudent.Update(student);
        }
    }
}