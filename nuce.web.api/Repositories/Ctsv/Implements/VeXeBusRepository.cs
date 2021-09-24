using nuce.web.api.Repositories.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.Ctsv;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class VeXeBusRepository : BaseStudentServiceRepository<AsAcademyStudentSvVeXeBus>, IVeXeBusRepository
    {
        public VeXeBusRepository(CTSVNUCE_DATAContext _context) : base(_context)
        {

        }
    }
}
