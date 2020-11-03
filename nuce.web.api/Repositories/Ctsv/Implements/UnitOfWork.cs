using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CTSVNUCE_DATAContext _context;
        public UnitOfWork(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
