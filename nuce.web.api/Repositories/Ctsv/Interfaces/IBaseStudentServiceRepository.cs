using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IBaseStudentServiceRepository<Entity> where Entity: class
    {
        public IQueryable<Entity> GetAll(long studentId);
        public Task AddAsync(Entity model);
    }
}
