using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class BaseStudentServiceRepository<Entity> : IBaseStudentServiceRepository<Entity> where Entity : class
    {
        private readonly CTSVNUCE_DATAContext _context;
        public BaseStudentServiceRepository(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }
        public IQueryable<Entity> GetAll(int studentId)
        {
            return _context.Set<Entity>().AsNoTracking().ToList()
                    .Where(item => Convert.ToInt32(item.GetType().GetProperty("StudentId").GetValue(item, null)) == studentId &&
                            !Convert.ToBoolean(item.GetType().GetProperty("Deleted").GetValue(item, null)))
                    .AsQueryable();
        }
    }
}
