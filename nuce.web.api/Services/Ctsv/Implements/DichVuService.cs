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
using System.Transactions;
using nuce.web.api.ViewModel;
using System.Net;
using nuce.web.api.ViewModel.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        private readonly IEmailService _emailService;
        private readonly ILogService _logService;
        private readonly ILogger<DichVuService> _logger;

        public DichVuService(IXacNhanRepository _xacNhanRepository, IGioiThieuRepository _gioiThieuRepository,
            IUuDaiGiaoDucRepository _uuDaiRepository, IVayVonRepository _vayVonRepository,
            IThueNhaRepository _thueNhaRepository, IUserService _userService,
            IUnitOfWork _unitOfWork, IEmailService _emailService, ILogService _logService
        )
        {
            this._xacNhanRepository = _xacNhanRepository;
            this._gioiThieuRepository = _gioiThieuRepository;
            this._uuDaiRepository = _uuDaiRepository;
            this._vayVonRepository = _vayVonRepository;
            this._thueNhaRepository = _thueNhaRepository;
            this._userService = _userService;
            this._unitOfWork = _unitOfWork;
            this._emailService = _emailService;
            this._logService = _logService;
        }

        public async Task<ResponseBody> AddDichVu(DichVuModel model)
        {
            var currentStudent = _userService.GetCurrentStudent();
            if (currentStudent == null)
            {
                return new ResponseBody { Message = "Sinh viên không tồn tại trong hệ thống" };
            }

            if (!(currentStudent.DaXacThucEmailNhaTruong ?? false))
            {
                return new ResponseBody { Message = "Chưa xác thực email nhà trường" };
            }

            int studentID = Convert.ToInt32(currentStudent.Id);
            var now = DateTime.Now;

            int requestStatus = (int)Common.Ctsv.TrangThaiYeuCau.DaGuiLenNhaTruong;
            string duplicateMsg = "Trùng yêu cầu dịch vụ";
            var duplicateCode = HttpStatusCode.Conflict;

            bool run = true;

            try
            {
                #region thêm yêu cầu và gửi mail
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    switch (model.Type)
                    {
                        case (int)Common.Ctsv.DichVu.XacNhan:
                            if (_xacNhanRepository.IsDuplicated(currentStudent.Id, model.LyDo))
                            {
                                return new ResponseBody { Message = duplicateMsg, StatusCode = duplicateCode };
                            }
                            AsAcademyStudentSvXacNhan xacNhan = new AsAcademyStudentSvXacNhan
                            {
                                LyDo = model.LyDo,
                                PhanHoi = model.PhanHoi,
                                CreatedTime = now,
                                DeletedTime = now,
                                LastModifiedTime = now,
                                MaXacNhan = model.MaXacNhan,
                                Status = requestStatus,
                                StudentId = studentID,
                                StudentCode = currentStudent.Code,
                                StudentName = currentStudent.FulName,
                                Deleted = false,
                                CreatedBy = studentID,
                                LastModifiedBy = studentID,
                                DeletedBy = -1
                            };
                            await _xacNhanRepository.AddAsync(xacNhan);
                            break;
                        case (int)Common.Ctsv.DichVu.GioiThieu:
                            if (_gioiThieuRepository.IsDuplicated(currentStudent.Id))
                            {
                                return new ResponseBody { Message = duplicateMsg, StatusCode = duplicateCode };
                            }
                            AsAcademyStudentSvGioiThieu gioiThieu = new AsAcademyStudentSvGioiThieu
                            {
                                VeViec = model.VeViec,
                                DenGap = model.DenGap,
                                DonVi = model.DonVi,
                                PhanHoi = model.PhanHoi,
                                CreatedTime = now,
                                DeletedTime = now,
                                LastModifiedTime = now,
                                Status = requestStatus,
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
                            break;
                        case (int)Common.Ctsv.DichVu.UuDaiGiaoDuc:
                            if (_uuDaiRepository.IsDuplicated(currentStudent.Id))
                            {
                                return new ResponseBody { Message = duplicateMsg, StatusCode = duplicateCode };
                            }
                            AsAcademyStudentSvXacNhanUuDaiTrongGiaoDuc uuDai = new AsAcademyStudentSvXacNhanUuDaiTrongGiaoDuc
                            {
                                KyLuat = model.KyLuat,
                                PhanHoi = model.PhanHoi,
                                CreatedTime = now,
                                DeletedTime = now,
                                Status = requestStatus,
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
                            break;
                        case (int)Common.Ctsv.DichVu.VayVonNganHang:
                            if (_vayVonRepository.IsDuplicated(currentStudent.Id))
                            {
                                return new ResponseBody { Message = duplicateMsg, StatusCode = duplicateCode };
                            }
                            AsAcademyStudentSvVayVonNganHang vayVon = new AsAcademyStudentSvVayVonNganHang
                            {
                                ThuocDien = model.ThuocDien,
                                ThuocDoiTuong = model.ThuocDoiTuong,
                                PhanHoi = model.PhanHoi,
                                CreatedTime = now,
                                DeletedTime = now,
                                LastModifiedTime = now,
                                MaXacNhan = model.MaXacNhan,
                                Status = requestStatus,
                                StudentId = studentID,
                                StudentCode = currentStudent.Code,
                                StudentName = currentStudent.FulName,
                                Deleted = false,
                                CreatedBy = studentID,
                                LastModifiedBy = studentID,
                                DeletedBy = -1
                            };
                            await _vayVonRepository.AddAsync(vayVon);
                            break;
                        case (int)Common.Ctsv.DichVu.ThueNha:
                            if (_thueNhaRepository.IsDuplicated(currentStudent.Id))
                            {
                                return new ResponseBody { Message = duplicateMsg, StatusCode = duplicateCode };
                            }
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
                                Status = requestStatus,
                                Deleted = false,
                                CreatedBy = studentID,
                                LastModifiedBy = studentID,
                                DeletedBy = -1
                            };
                            await _thueNhaRepository.AddAsync(thueNha);
                            break;
                        default:
                            run = false;
                            break;
                    }
                    if (run)
                    {
                        var dichVu = Common.Ctsv.DichVuDictionary[model.Type];
                        TinNhanModel tinNhan = new TinNhanModel
                        {
                            StudentCode = currentStudent.Code,
                            StudentEmail = currentStudent.EmailNhaTruong,
                            StudentName = currentStudent.FulName,
                            StudentID = studentID,
                            TinNhanCode = dichVu.TinNhanCode,
                            TinNhanTitle = dichVu.TieuDeTinNhan,
                            TenDichVu = dichVu.TenDichVu
                        };
                        var result = await _emailService.SendEmailNewServiceRequest(tinNhan);
                        if (result != null)
                        {
                            return result;
                        }
                        await _unitOfWork.SaveAsync();
                    }
                    scope.Complete();
                }
                #endregion
                #region log
                if (run)
                {
                    var dichVu = Common.Ctsv.DichVuDictionary[model.Type];
                    await _logService.WriteLog(new ActivityLogModel
                    {
                        LogCode = dichVu.LogCodeSendEmail,
                        LogMessage = $"gửi mail tới {currentStudent.EmailNhaTruong}",
                    });
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogError("Tạo mới yêu cầu dịch vụ", $"{ex.ToString()} \n", JsonConvert.SerializeObject(model));
                return new ResponseBody { Data = ex, Message = "Lỗi hệ thống" };
            }
            
            return null;
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
