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
        public IQueryable<Entity> GetAll(string studentCode);
        public Task<GetAllForAdminResponseRepo<Entity>> GetAllForAdmin(QuanLyDichVuDetailModel model);
        public Task AddAsync(Entity model);
        public bool IsDuplicated(long studentId, string reason = null);
        public AllTypeDichVuModel GetRequestInfo(DateTime? start, DateTime? end);
        public Task<Entity> FindByIdAsync(long id);
        public Task<IEnumerable<YeuCauDichVuStudentModel<Entity>>> GetYeuCauDichVuStudent(List<long> ids);
    }
}
