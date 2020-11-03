using nuce.web.api.Models.Ctsv;
using nuce.web.api.Services.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class ParameterService : IParameterService
    {
        private readonly CTSVNUCE_DATAContext _context;
        public ParameterService(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }
        public GsSetting GetByCode(string code)
        {
            return _context.GsSetting.FirstOrDefault(setting => setting.Code == code);
        }

        public bool isCapNhatHoSo()
        {
            var param = GetByCode("CAP_NHAT_HO_SO");
            return param != null ? (param.Enabled ?? false) : false;
        }
    }
}
