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
using static nuce.web.api.Common.Ctsv;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class DichVuService : IDichVuService
    {
        #region declare
        private readonly IXacNhanRepository _xacNhanRepository;
        private readonly IGioiThieuRepository _gioiThieuRepository;
        private readonly IUuDaiGiaoDucRepository _uuDaiRepository;
        private readonly IVayVonRepository _vayVonRepository;
        private readonly IThueNhaRepository _thueNhaRepository;
        private readonly ILoaiDichVuRepository _loaiDichVuRepository;
        private readonly IStudentRepository _studentRepository;

        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogService _logService;
        private readonly ILogger<DichVuService> _logger;

        public DichVuService(IXacNhanRepository _xacNhanRepository, IGioiThieuRepository _gioiThieuRepository,
            IUuDaiGiaoDucRepository _uuDaiRepository, IVayVonRepository _vayVonRepository,
            IThueNhaRepository _thueNhaRepository, IUserService _userService,
            IUnitOfWork _unitOfWork, IEmailService _emailService, ILogService _logService,
            ILoaiDichVuRepository _loaiDichVuRepository, IStudentRepository _studentRepository
        )
        {
            this._xacNhanRepository = _xacNhanRepository;
            this._gioiThieuRepository = _gioiThieuRepository;
            this._uuDaiRepository = _uuDaiRepository;
            this._vayVonRepository = _vayVonRepository;
            this._thueNhaRepository = _thueNhaRepository;
            this._loaiDichVuRepository = _loaiDichVuRepository;
            this._studentRepository = _studentRepository;

            this._userService = _userService;
            this._unitOfWork = _unitOfWork;
            this._emailService = _emailService;
            this._logService = _logService;
        }
        #endregion

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
                            TinNhanCode = $"{dichVu.TinNhanCode}_THEM_MOI",
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

        public IQueryable GetAllByStudent(int dichVuType)
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

        public async Task<IQueryable> GetRequestForAdmin(QuanLyDichVuDetailModel model)
        {
            var result = new List<QuanLyDichVuDetailResponse>();

            object yeuCauDichVu = null;

            string studentsIds;
            var students = new Dictionary<long, AsAcademyStudent>();

            switch (model.Type)
            {
                case (int)Common.Ctsv.DichVu.XacNhan:
                    var xacNhanList = await _xacNhanRepository.GetAllForAdmin(model);
                    if (xacNhanList.Count() > 0)
                    {
                        studentsIds = $",{string.Join(',', xacNhanList.Select(r => r.StudentId.ToString()))},";

                        yeuCauDichVu = xacNhanList;
                        students = _studentRepository.GetAll().Where(s => studentsIds.Contains(("," + s.Id.ToString() + ","))).ToDictionary(s => s.Id, s => s);
                    }

                    foreach (var yeuCau in xacNhanList)
                    {
                        var item = new QuanLyDichVuDetailResponse()
                        {
                            Student = students[yeuCau.StudentId],
                            DichVu = yeuCau
                        };
                        result.Add(item);
                    }
                    break;
                case (int)Common.Ctsv.DichVu.GioiThieu:
                    var gioiThieuList = await _gioiThieuRepository.GetAllForAdmin(model);
                    if (gioiThieuList.Count() > 0)
                    {
                        studentsIds = $",{string.Join(',', gioiThieuList.Select(r => r.StudentId.ToString()))},";

                        yeuCauDichVu = gioiThieuList;
                        students = _studentRepository.GetAll().Where(s => studentsIds.Contains(("," + s.Id.ToString() + ","))).ToDictionary(s => s.Id, s => s);
                    }

                    foreach (var yeuCau in gioiThieuList)
                    {
                        var item = new QuanLyDichVuDetailResponse()
                        {
                            Student = students[yeuCau.StudentId],
                            DichVu = yeuCau
                        };
                        result.Add(item);
                    }
                    break;
                case (int)Common.Ctsv.DichVu.UuDaiGiaoDuc:
                    var uuDaiList = await _uuDaiRepository.GetAllForAdmin(model);
                    if (uuDaiList.Count() > 0)
                    {
                        studentsIds = $",{string.Join(',', uuDaiList.Select(r => r.StudentId.ToString()))},";

                        yeuCauDichVu = uuDaiList;
                        students = _studentRepository.GetAll().Where(s => studentsIds.Contains(("," + s.Id.ToString() + ","))).ToDictionary(s => s.Id, s => s);
                    }

                    foreach (var yeuCau in uuDaiList)
                    {
                        var item = new QuanLyDichVuDetailResponse()
                        {
                            Student = students[yeuCau.StudentId],
                            DichVu = yeuCau
                        };
                        result.Add(item);
                    }
                    break;
                case (int)Common.Ctsv.DichVu.VayVonNganHang:
                    var vayVonList = await _vayVonRepository.GetAllForAdmin(model);
                    if (vayVonList.Count() > 0)
                    {
                        studentsIds = $",{string.Join(',', vayVonList.Select(r => r.StudentId.ToString()))},";

                        yeuCauDichVu = vayVonList;
                        students = _studentRepository.GetAll().Where(s => studentsIds.Contains(("," + s.Id.ToString() + ","))).ToDictionary(s => s.Id, s => s);
                    }

                    foreach (var yeuCau in vayVonList)
                    {
                        var item = new QuanLyDichVuDetailResponse()
                        {
                            Student = students[yeuCau.StudentId],
                            DichVu = yeuCau
                        };
                        result.Add(item);
                    }
                    break;
                case (int)Common.Ctsv.DichVu.ThueNha:
                    var thueNhaList = await _thueNhaRepository.GetAllForAdmin(model);
                    if (thueNhaList.Count() > 0)
                    {
                        studentsIds = $",{string.Join(',', thueNhaList.Select(r => r.StudentId.ToString()))},";

                        yeuCauDichVu = thueNhaList;
                        students = _studentRepository.GetAll().Where(s => studentsIds.Contains(("," + s.Id.ToString() + ","))).ToDictionary(s => s.Id, s => s);
                    }
                    foreach (var yeuCau in thueNhaList)
                    {
                        var item = new QuanLyDichVuDetailResponse()
                        {
                            Student = students[yeuCau.StudentId],
                            DichVu = yeuCau
                        };
                        result.Add(item);
                    }
                    break;
                default:
                    break;
            }

            return result.AsQueryable();
        }

        public List<AllTypeDichVuModel> GetAllLoaiDichVuInfo()
        {
            var allDichVu = _loaiDichVuRepository.GetAllInUse();
            var quantityDictionary = new Dictionary<int, AllTypeDichVuModel>
            {
                { (int)Common.Ctsv.DichVu.XacNhan, _xacNhanRepository.GetRequestInfo() },
                { (int)Common.Ctsv.DichVu.ThueNha, _thueNhaRepository.GetRequestInfo() },
                { (int)Common.Ctsv.DichVu.UuDaiGiaoDuc, _uuDaiRepository.GetRequestInfo() },
                { (int)Common.Ctsv.DichVu.VayVonNganHang, _vayVonRepository.GetRequestInfo() },
            };
            var result = new List<AllTypeDichVuModel>();
            foreach (var dichVu in allDichVu)
            {
                if (quantityDictionary.ContainsKey(dichVu.Id))
                {
                    var info = quantityDictionary[dichVu.Id];
                    info.TenDichVu = dichVu.Description;
                    info.LinkDichVu = dichVu.Param1;
                    result.Add(info);
                }
            }
            return result;
        }

        public async Task<ResponseBody> UpdateRequestStatus(UpdateRequestStatusModel model)
        {
            DateTime now = DateTime.Now;
            var dayOfWeek = (int)now.DayOfWeek;
            bool earlierThanFriday = now.DayOfWeek < DayOfWeek.Friday;
            bool isMorning = now.Hour <= 13;
            bool daXuLyCoLichHen = (TrangThaiYeuCau)model.Status == TrangThaiYeuCau.DaXuLyVaCoLichHen;

            DateTime? ngayTao = DateTime.Now;
            AsAcademyStudent student = null;
            #region ngay hen
            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (daXuLyCoLichHen && !model.AutoUpdateNgayHen)
            {
                if (model.NgayBatDau == null)
                {
                    return new ResponseBody { Message = "Ngày bắt đầu không được trống" };
                }
                else if (model.NgayKetThuc == null)
                {
                    return new ResponseBody { Message = "Ngày kết thúc không được trống" };
                }
                else if (model.NgayBatDau < now)
                {
                    return new ResponseBody { Message = "Ngày bắt đầu không được nhỏ hơn hiện tại" };
                }
                else if (model.NgayBatDau > model.NgayKetThuc)
                {
                    return new ResponseBody { Message = "Ngày bắt đầu không được lớn hơn ngày kết thúc" };
                }
                fromDate = model.NgayBatDau;
                toDate = model.NgayKetThuc;
            }
            else if (daXuLyCoLichHen && model.AutoUpdateNgayHen)
            {
                if (isMorning)
                {
                    if (earlierThanFriday)
                    {
                        fromDate = DateTime.Parse(string.Format("{0}/{1}/{2} 10:00:00 AM", DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, DateTime.Now.AddDays(1).Year));
                    }
                    else
                    {
                        fromDate = DateTime.Parse(string.Format("{0}/{1}/{2} 10:00:00 AM", DateTime.Now.AddDays(8 - dayOfWeek).Month, DateTime.Now.AddDays(8 - dayOfWeek).Day, DateTime.Now.AddDays(8 - dayOfWeek).Year));
                    }
                }
                else
                {
                    //Cap nhat vào buổi chiều
                    if (earlierThanFriday)
                    {
                        fromDate = DateTime.Parse(string.Format("{0}/{1}/{2} 4:00:00 PM", DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, DateTime.Now.AddDays(1).Year));
                    }
                    else
                    {
                        fromDate = DateTime.Parse(string.Format("{0}/{1}/{2} 10:00:00 AM", DateTime.Now.AddDays(8 - dayOfWeek).Month, DateTime.Now.AddDays(8 - dayOfWeek).Day, DateTime.Now.AddDays(8 - dayOfWeek).Year));
                    }
                }
                toDate = fromDate?.AddMonths(1);
            }
            #endregion
            bool run = true;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    #region dich vu
                    switch ((DichVu)model.Type)
                    {
                        case DichVu.XacNhan:
                            var xacNhan = await _xacNhanRepository.FindByIdAsync(model.RequestID);
                            xacNhan.PhanHoi = model.PhanHoi;
                            xacNhan.Status = model.Status;
                            xacNhan.NgayHenTuNgay = fromDate;
                            xacNhan.NgayHenDenNgay = toDate;
                            ngayTao = xacNhan.CreatedTime;

                            student = _studentRepository.FindByCode(xacNhan.StudentCode);
                            break;
                        case DichVu.GioiThieu:
                            var gioiThieu = await _gioiThieuRepository.FindByIdAsync(model.RequestID);
                            gioiThieu.PhanHoi = model.PhanHoi;
                            gioiThieu.Status = model.Status;
                            gioiThieu.NgayHenTuNgay = fromDate;
                            gioiThieu.NgayHenDenNgay = toDate;
                            ngayTao = gioiThieu.CreatedTime;

                            student = _studentRepository.FindByCode(gioiThieu.StudentCode);
                            break;
                        case DichVu.ThueNha:
                            var thueNha = await _thueNhaRepository.FindByIdAsync(model.RequestID);
                            thueNha.PhanHoi = model.PhanHoi;
                            thueNha.Status = model.Status;
                            thueNha.NgayHenTuNgay = fromDate;
                            thueNha.NgayHenDenNgay = toDate;
                            ngayTao = thueNha.CreatedTime;

                            student = _studentRepository.FindByCode(thueNha.StudentCode);
                            break;
                        case DichVu.UuDaiGiaoDuc:
                            var uuDai = await _uuDaiRepository.FindByIdAsync(model.RequestID);
                            uuDai.PhanHoi = model.PhanHoi;
                            uuDai.Status = model.Status;
                            uuDai.NgayHenTuNgay = fromDate;
                            uuDai.NgayHenDenNgay = toDate;
                            ngayTao = uuDai.CreatedTime;

                            student = _studentRepository.FindByCode(uuDai.StudentCode);
                            break;
                        case DichVu.VayVonNganHang:
                            var vayVon = await _vayVonRepository.FindByIdAsync(model.RequestID);
                            vayVon.PhanHoi = model.PhanHoi;
                            vayVon.Status = model.Status;
                            vayVon.NgayHenTuNgay = fromDate;
                            vayVon.NgayHenDenNgay = toDate;
                            ngayTao = vayVon.CreatedTime;

                            student = _studentRepository.FindByCode(vayVon.StudentCode);
                            break;
                        default:
                            run = false;
                            break;
                    }
                    #endregion
                    if (run)
                    {
                        #region email
                        var dichVu = DichVuDictionary[model.Type];
                        var trangThai = TrangThaiYeuCauDictionary[model.Status];
                        string tinNhanTitle = $"Thông báo về việc {trangThai} {dichVu.TenDichVu}";
                        string tinNhanCode = $"{dichVu.TinNhanCode}_CHUYENTRANGTHAI_{model.Status}";
                        TinNhanModel tinNhan = new TinNhanModel
                        {
                            StudentCode = student.Code,
                            StudentEmail = student.EmailNhaTruong,
                            StudentName = student.FulName,
                            StudentID = (int)student.Id,
                            TinNhanCode = tinNhanCode,
                            TinNhanTitle = tinNhanTitle,
                            TenDichVu = dichVu.TenDichVu,
                            YeuCauStatus = model.Status,
                            NgayHen = fromDate,
                            NgayTao = ngayTao
                        };
                        var sendEmailRs = await _emailService.SendEmailUpdateStatusRequest(tinNhan);
                        if (sendEmailRs != null)
                        {
                            return sendEmailRs;
                        }
                        #endregion
                        await _unitOfWork.SaveAsync();
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError("Cập nhật trạng thái yêu cầu dịch vụ", $"{ex.ToString()} \n", JsonConvert.SerializeObject(model));
                return new ResponseBody { Data = ex, Message = "Lỗi hệ thống" };
            }
            return null;
        }
    }
}
