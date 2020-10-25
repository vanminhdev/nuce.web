using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Services.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class DichVuService : IDichVuService
    {
        private readonly IXacNhanRepository _xacNhanRepository;
        private readonly IGioiThieuRepository _gioiThieuRepository;
        private readonly IUuDaiGiaoDucRepository _uuDaiRepository;
        private readonly IVayVonRepository _vayVonRepository;
        private readonly IThueNhaRepository _thueNhaRepository;

        public DichVuService(IXacNhanRepository _xacNhanRepository, IGioiThieuRepository _gioiThieuRepository,
            IUuDaiGiaoDucRepository _uuDaiRepository, IVayVonRepository _vayVonRepository,
            IThueNhaRepository _thueNhaRepository)
        {
            this._xacNhanRepository = _xacNhanRepository;
            this._gioiThieuRepository = _gioiThieuRepository;
            this._uuDaiRepository = _uuDaiRepository;
            this._vayVonRepository = _vayVonRepository;
            this._thueNhaRepository = _thueNhaRepository;
        }

        public IQueryable GetAll(int dichVuType, int studentId)
        {
            switch (dichVuType)
            {
                case 1:
                    return _xacNhanRepository.GetAll(studentId);
                case 2:
                    return _gioiThieuRepository.GetAll(studentId);
                case 4:
                    return _uuDaiRepository.GetAll(studentId);
                case 6:
                    return _vayVonRepository.GetAll(studentId);
                case 7:
                    return _thueNhaRepository.GetAll(studentId);
                default:
                    break;
            }
            return null;
        }
    }
}
