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
        private readonly IUnitOfWork _unitOfWork;

        public DichVuService(IXacNhanRepository _xacNhanRepository, IGioiThieuRepository _gioiThieuRepository,
            IUuDaiGiaoDucRepository _uuDaiRepository, IVayVonRepository _vayVonRepository,
            IThueNhaRepository _thueNhaRepository, IUserService _userService,
            IUnitOfWork _unitOfWork
        )
        {
            this._xacNhanRepository = _xacNhanRepository;
            this._gioiThieuRepository = _gioiThieuRepository;
            this._uuDaiRepository = _uuDaiRepository;
            this._vayVonRepository = _vayVonRepository;
            this._thueNhaRepository = _thueNhaRepository;
            this._userService = _userService;
            this._unitOfWork = _unitOfWork;
        }

        public async Task AddDichVu(DichVuModel model)
        {
            var currentStudent = _userService.GetCurrentStudent();
            int studentID = Convert.ToInt32(currentStudent.Id);
            var now = DateTime.Now;
            switch (model.Type)
            {
                case (int)Common.Ctsv.DichVu.XacNhan:
                    AsAcademyStudentSvXacNhan xacNhan = new AsAcademyStudentSvXacNhan
                    {
                        LyDo = model.LyDo,
                        PhanHoi = model.PhanHoi,
                        CreatedTime = now,
                        DeletedTime = now,
                        LastModifiedTime = now,
                        MaXacNhan = model.MaXacNhan,
                        StudentId = studentID,
                        StudentCode = currentStudent.Code,
                        StudentName = currentStudent.FulName,
                        Deleted = false,
                        CreatedBy = studentID,
                        LastModifiedBy = studentID,
                        DeletedBy = -1
                    };
                    await _xacNhanRepository.AddAsync(xacNhan);
                    await _unitOfWork.SaveAsync();
                    break;
                case (int)Common.Ctsv.DichVu.GioiThieu:
                    AsAcademyStudentSvGioiThieu gioiThieu = new AsAcademyStudentSvGioiThieu
                    {
                        VeViec = model.VeViec,
                        DenGap = model.DenGap,
                        DonVi = model.DonVi,
                        PhanHoi = model.PhanHoi,
                        CreatedTime = now,
                        DeletedTime = now,
                        LastModifiedTime = now,
                        MaXacNhan = model.MaXacNhan,
                        StudentId = studentID,
                        StudentCode = currentStudent.Code,
                        StudentName = currentStudent.FulName,
                        Deleted = false,
                        CreatedBy = studentID,
                        LastModifiedBy = studentID,
                        DeletedBy = -1
                    };
                    await _gioiThieuRepository.AddAsync(gioiThieu);
                    await _unitOfWork.SaveAsync();
                    break;
                case (int)Common.Ctsv.DichVu.UuDaiGiaoDuc:
                    AsAcademyStudentSvXacNhanUuDaiTrongGiaoDuc uuDai = new AsAcademyStudentSvXacNhanUuDaiTrongGiaoDuc
                    {
                        KyLuat = model.KyLuat,
                        PhanHoi = model.PhanHoi,
                        CreatedTime = now,
                        DeletedTime = now,
                        LastModifiedTime = now,
                        MaXacNhan = model.MaXacNhan,
                        StudentId = studentID,
                        StudentCode = currentStudent.Code,
                        StudentName = currentStudent.FulName,
                        Deleted = false,
                        CreatedBy = studentID,
                        LastModifiedBy = studentID,
                        DeletedBy = -1
                    };
                    await _uuDaiRepository.AddAsync(uuDai);
                    await _unitOfWork.SaveAsync();
                    break;
                case (int)Common.Ctsv.DichVu.VayVonNganHang:
                    AsAcademyStudentSvVayVonNganHang vayVon = new AsAcademyStudentSvVayVonNganHang
                    {
                        ThuocDien = model.ThuocDien,
                        ThuocDoiTuong = model.ThuocDoiTuong,
                        PhanHoi = model.PhanHoi,
                        CreatedTime = now,
                        DeletedTime = now,
                        LastModifiedTime = now,
                        MaXacNhan = model.MaXacNhan,
                        StudentId = studentID,
                        StudentCode = currentStudent.Code,
                        StudentName = currentStudent.FulName,
                        Deleted = false,
                        CreatedBy = studentID,
                        LastModifiedBy = studentID,
                        DeletedBy = -1
                    };
                    await _vayVonRepository.AddAsync(vayVon);
                    await _unitOfWork.SaveAsync();
                    break;
                case (int)Common.Ctsv.DichVu.ThueNha:
                    AsAcademyStudentSvThueNha thueNha = new AsAcademyStudentSvThueNha
                    {
                        PhanHoi = model.PhanHoi,
                        CreatedTime = now,
                        DeletedTime = now,
                        LastModifiedTime = now,
                        MaXacNhan = model.MaXacNhan,
                        StudentId = studentID,
                        StudentCode = currentStudent.Code,
                        StudentName = currentStudent.FulName,
                        Deleted = false,
                        CreatedBy = studentID,
                        LastModifiedBy = studentID,
                        DeletedBy = -1
                    };
                    await _thueNhaRepository.AddAsync(thueNha);
                    await _unitOfWork.SaveAsync();
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
                case (int)Common.Ctsv.DichVu.XacNhan:
                    return _xacNhanRepository.GetAll(studentId);
                case (int)Common.Ctsv.DichVu.GioiThieu:
                    return _gioiThieuRepository.GetAll(studentId);
                case (int)Common.Ctsv.DichVu.UuDaiGiaoDuc:
                    return _uuDaiRepository.GetAll(studentId);
                case (int)Common.Ctsv.DichVu.VayVonNganHang:
                    return _vayVonRepository.GetAll(studentId);
                case (int)Common.Ctsv.DichVu.ThueNha:
                    return _thueNhaRepository.GetAll(studentId);
                default:
                    break;
            }
            return null;
        }
    }
}
