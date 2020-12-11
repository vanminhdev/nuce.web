using nuce.web.api.Models.Core;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface INewsManagerService
    {
        public IQueryable GetAllCategory(string role);
        public Task<DataTableResponse<NewsItems>> FindItemsByCatId(int catId, int seen, int size);
        public Task<NewsItems> FindNewsItemById(int id);
        public Task CreateNewsItems(CreateNewsItemModel model);
        public Task UpdateNewsItems(NewsItems model);
    }
}
