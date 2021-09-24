using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Core;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Implements
{
    public class ClientParameterService : IClientParameterService
    {
        private readonly NuceCoreIdentityContext _context;
        private int activeStatus = 1;
        public ClientParameterService(NuceCoreIdentityContext _context)
        {
            this._context = _context;
        }

        #region Admin

        public async Task<ClientParameters> FindByCodeAsync(string code, string role)
        {
            return await findByCodeAsync(code, null, role);
        }

        public IQueryable<ClientParameters> FindByType(string type, string role)
        {
            return _context.ClientParameters
                        .AsNoTracking()
                        .Where(cp => cp.Type == type &&
                                    cp.Role == role);
        }

        public async Task UpdateParameter(List<UpdateClientParameterModel> model, List<string> loggedUserRoles, string username)
        {
            try
            {
                var handlingParam = new ClientParameters();
                foreach (var param in model)
                {
                    handlingParam = _context.ClientParameters.Find(param.Id);
                    if (!loggedUserRoles.Contains(handlingParam.Role))
                    {
                        throw new UnauthorizedAccessException($"Không có quyền {handlingParam.Role}");
                    }
                    handlingParam.Status = param.Status;
                    handlingParam.Value = param.Value;
                    handlingParam.UpdateDatetime = DateTime.Now;
                    handlingParam.UpdateUsername = username;
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Client
        public async Task<ClientParameters> FindByCodeActiveAsync(string code)
        {
            return await _context.ClientParameters
                        .AsNoTracking()
                        .FirstOrDefaultAsync(cp => cp.Code == code && cp.Status == activeStatus);
        }

        public IQueryable<ClientParameters> FindByTypeActive(string type)
        {
            return _context.ClientParameters
                        .AsNoTracking()
                        .Where(cp => cp.Type == type && cp.Status == activeStatus);
        }
        #endregion

        #region helper
        private async Task<ClientParameters> findByCodeAsync(string code, int? status, string role)
        {
            return await _context.ClientParameters
                        .AsNoTracking()
                        .FirstOrDefaultAsync(cp => cp.Code == code && 
                                                    (status == null || cp.Status == status) &&
                                                    cp.Role == role);
        }

        private IQueryable<ClientParameters> findByType(string type, int? status, string role)
        {
            return _context.ClientParameters
                        .AsNoTracking()
                        .Where(cp => cp.Type == type && 
                                    (status == null || cp.Status == status) &&
                                    cp.Role == role);
        }

        #endregion

    }
}
