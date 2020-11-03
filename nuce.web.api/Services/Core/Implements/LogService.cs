using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel.Core;
using nuce.web.api.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Implements
{
    public class LogService : ILogService
    {
        private readonly NuceCoreIdentityContext _context;
        private readonly IUserService _userService;
        public LogService(NuceCoreIdentityContext _context, IUserService _userService)
        {
            this._context = _context;
            this._userService = _userService;
        }
        public async Task WriteLog(ActivityLogModel log)
        {
            try
            {
                if (string.IsNullOrEmpty(log.Username))
                {
                    log.Username = _userService.GetUserName();
                }

                var newLog = new ActiviyLogs
                {
                    CreatedTime = DateTime.Now,
                    Code = log.LogCode,
                    Message = log.LogMessage,
                    UserCode = log.Username,
                    UserId = -1,
                    Status = log.Status ?? 1,
                };
                await _context.AddAsync(newLog);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
