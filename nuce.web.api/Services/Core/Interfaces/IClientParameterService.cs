using nuce.web.api.Models.Core;
using nuce.web.api.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface IClientParameterService
    {
        public Task<ClientParameters> FindByCodeAsync(string code, string role);
        public IQueryable<ClientParameters> FindByType(string type, string role);
        public Task<ClientParameters> FindByCodeActiveAsync(string code);
        public IQueryable<ClientParameters> FindByTypeActive(string type);
        public Task UpdateParameter(List<UpdateClientParameterModel> model, List<string> loggedUserRoles, string username);
    }
}
