using nuce.web.api.ViewModel;
using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.ViewModel.Ctsv;

namespace nuce.web.api.Services.Ctsv.Interfaces
{
    public interface IEmailService
    {
        public Task<ResponseBody> SendEmailNewServiceRequest(TinNhanModel tinNhan);
        public Task<ResponseBody> SendEmailUpdateStatusRequest(TinNhanModel tinNhan);
    }
}
