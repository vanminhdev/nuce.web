using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Services.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class NewsService : INewsService
    {
        private readonly INewsItemsRepository _newsItemsRepository;
        public NewsService(INewsItemsRepository _newsItemsRepository)
        {
            this._newsItemsRepository = _newsItemsRepository;
        }
        public IQueryable<AsNewsItems> GetNewsItems()
        {
            return _newsItemsRepository.Get().OrderByDescending(item => item.UpdateDatetime);
        }
    }
}
