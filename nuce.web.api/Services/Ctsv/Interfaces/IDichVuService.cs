using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Interfaces
{
    public interface IDichVuService
    {
        public IQueryable GetAll(int dichVuType);
        public Task AddDichVu(DichVuModel model);
    }
}
