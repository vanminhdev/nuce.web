using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IBaseStudentServiceRepository<Entity> where Entity: class
    {
        public IQueryable<Entity> GetAll(long studentId);
        public Task<IQueryable<Entity>> GetAllForAdmin(QuanLyDichVuDetailModel model);
        public Task AddAsync(Entity model);
        public bool IsDuplicated(long studentId, string reason = null);
        public AllTypeDichVuModel GetRequestInfo();
        public Task<Entity> FindByIdAsync(long id);
    }
}
