using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class NewsItemsRepository : INewsItemsRepository
    {
        private readonly CTSVNUCE_DATAContext _context;
        public NewsItemsRepository(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }
        public IQueryable<AsNewsItems> Get()
        {
            return _context.AsNewsItems.AsNoTracking().Where(item => item.CatId > 9 && item.CatId < 14);
        }
    }
}
