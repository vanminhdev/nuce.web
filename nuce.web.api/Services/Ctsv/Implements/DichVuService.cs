using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.api.ViewModel.Ctsv;
using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Services.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class DichVuService : IDichVuService
    {
        private readonly IXacNhanRepository _xacNhanRepository;
        private readonly IGioiThieuRepository _gioiThieuRepository;
        private readonly IUuDaiGiaoDucRepository _uuDaiRepository;
        private readonly IVayVonRepository _vayVonRepository;
        private readonly IThueNhaRepository _thueNhaRepository;
        private readonly IUserService _userService;

        public DichVuService(IXacNhanRepository _xacNhanRepository, IGioiThieuRepository _gioiThieuRepository,
            IUuDaiGiaoDucRepository _uuDaiRepository, IVayVonRepository _vayVonRepository,
            IThueNhaRepository _thueNhaRepository, IUserService _userService)
        {
            this._xacNhanRepository = _xacNhanRepository;
            this._gioiThieuRepository = _gioiThieuRepository;
            this._uuDaiRepository = _uuDaiRepository;
            this._vayVonRepository = _vayVonRepository;
            this._thueNhaRepository = _thueNhaRepository;
            this._userService = _userService;
        }

        public async Task AddDichVu(DichVuModel model)
        {
            var now = DateTime.Now;
            switch (model.Type)
            {
                case 1:
                    AsAcademyStudentSvXacNhan xacNhan = new AsAcademyStudentSvXacNhan
                    {
                        LyDo = model.LyDo,
                        CreatedTime = now,
                        DeletedTime = now,
                        LastModifiedTime = now,
                        MaXacNhan = model.MaXacNhan
                    };
                    await _xacNhanRepository.AddAsync(xacNhan);
                    break;
                case 2:
                    break;
                case 4:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                default:
                    break;
            }
        }

        public IQueryable GetAll(int dichVuType)
        {
            long studentId = _userService.GetCurrentStudentID() ?? 0;
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
