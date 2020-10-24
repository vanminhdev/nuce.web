using nuce.web.api.Models.Ctsv;
using nuce.web.api.Services.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class CtsvLogService : ICtsvLogService
    {
        private readonly CTSVNUCE_DATAContext _context;
        public CtsvLogService()
        {
            this._context = new CTSVNUCE_DATAContext();
        }
        public async Task WriteLog(AsLogs model)
        {
            AsLogs log = new AsLogs
            {
                UserId = model.UserId,
                UserCode = model.UserCode,
                Status = model.Status,
                Code = model.Code,
                Message = model.Message,
                CreatedTime = DateTime.Now
            };
            await _context.AsLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
