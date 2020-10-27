using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class VayVonRepository : BaseStudentServiceRepository<AsAcademyStudentSvVayVonNganHang>, IVayVonRepository
    {
        public VayVonRepository(CTSVNUCE_DATAContext _context) : base(_context)
        {

        }
    }
}
