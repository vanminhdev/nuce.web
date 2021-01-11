using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class MuonHocBaRepository : BaseStudentServiceRepository<AsAcademyStudentSvMuonHocBaGoc>, IMuonHocBaRepository
    {
        public MuonHocBaRepository(CTSVNUCE_DATAContext _context) : base(_context)
        {

        }
    }
}
