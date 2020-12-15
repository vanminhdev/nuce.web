using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.api.ViewModel.Ctsv;
using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Services.Core.Interfaces;
using System.Transactions;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static nuce.web.api.Common.Ctsv;
using nuce.web.api.ViewModel.Base;
using GemBox.Document;
using GemBox.Document.Tables;
using GemBox.Document.Drawing;
using System.Globalization;
using GemBox.Document.CustomMarkups;
using System.IO.Compression;
using System.IO;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Wordprocessing;
using GemboxRun = GemBox.Document.Run;
using GemboxParagraph = GemBox.Document.Paragraph;
using GemboxTable = GemBox.Document.Tables.Table;
using GemboxColor = GemBox.Document.Color;
using GemboxTableWidth = GemBox.Document.Tables.TableWidth;
using GemboxTableRow = GemBox.Document.Tables.TableRow;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using OXRun = DocumentFormat.OpenXml.Wordprocessing.Run;
using OXInline = DocumentFormat.OpenXml.Drawing.Wordprocessing.Inline;

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
        private readonly IVeXeBusRepository _veXeBusRepository;

        private readonly IThamSoDichVuService _thamSoDichVuService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogService _logService;
        private readonly IPathProvider _pathProvider;
        private readonly ILogger<DichVuService> _logger;

        public DichVuService(IXacNhanRepository _xacNhanRepository, IGioiThieuRepository _gioiThieuRepository,
            IUuDaiGiaoDucRepository _uuDaiRepository, IVayVonRepository _vayVonRepository,
            IThueNhaRepository _thueNhaRepository, IUserService _userService,
            IUnitOfWork _unitOfWork, IEmailService _emailService, ILogService _logService,
            ILoaiDichVuRepository _loaiDichVuRepository, IStudentRepository _studentRepository,
            IThamSoDichVuService _thamSoDichVuService, IPathProvider _pathProvider,
            ILogger<DichVuService> _logger, IVeXeBusRepository _veXeBusRepository
        )
        {
            this._xacNhanRepository = _xacNhanRepository;
            this._gioiThieuRepository = _gioiThieuRepository;
            this._uuDaiRepository = _uuDaiRepository;
            this._vayVonRepository = _vayVonRepository;
            this._thueNhaRepository = _thueNhaRepository;
            this._veXeBusRepository = _veXeBusRepository;
            this._loaiDichVuRepository = _loaiDichVuRepository;
            this._studentRepository = _studentRepository;

            this._pathProvider = _pathProvider;
            this._thamSoDichVuService = _thamSoDichVuService;
            this._userService = _userService;
            this._unitOfWork = _unitOfWork;
            this._emailService = _emailService;
            this._logService = _logService;
            this._logger = _logger;
        }
        #endregion

        public async Task AddDichVu(DichVuModel model)
        {
            var currentStudent = _userService.GetCurrentStudent();
            if (currentStudent == null)
            {
                throw new Exception("Sinh viên không tồn tại trong hệ thống");
            }

            if (!(currentStudent.DaXacThucEmailNhaTruong ?? false))
            {
                throw new Exception("Chưa xác thực email nhà trường");
            }

            int studentID = Convert.ToInt32(currentStudent.Id);
            var now = DateTime.Now;

            int requestStatus = (int)TrangThaiYeuCau.DaGuiLenNhaTruong;

            bool run = true;

            try
            {
                #region thêm yêu cầu và gửi mail
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    switch ((DichVu)model.Type)
                    {
                        case DichVu.XacNhan:
                            if (_xacNhanRepository.IsDuplicated(currentStudent.Id, model.LyDo))
                            {
                                throw new DuplicateWaitObjectException();
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
                        case DichVu.GioiThieu:
                            if (_gioiThieuRepository.IsDuplicated(currentStudent.Id))
                            {
                                throw new DuplicateWaitObjectException();
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
                        case DichVu.UuDaiGiaoDuc:
                            if (_uuDaiRepository.IsDuplicated(currentStudent.Id))
                            {
                                throw new DuplicateWaitObjectException();
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
                        case DichVu.VayVonNganHang:
                            if (_vayVonRepository.IsDuplicated(currentStudent.Id))
                            {
                                throw new DuplicateWaitObjectException();
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
                        case DichVu.ThueNha:
                            if (_thueNhaRepository.IsDuplicated(currentStudent.Id))
                            {
                                throw new DuplicateWaitObjectException();
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
                        case DichVu.VeBus:
                            if (!Enum.IsDefined(typeof(DichVuXeBusLoaiTuyen), model.VeBusTuyenType))
                            {
                                throw new Exception("Loại tuyến không hợp lệ");
                            }
                            bool isMotTuyen = (DichVuXeBusLoaiTuyen)model.VeBusTuyenType == DichVuXeBusLoaiTuyen.MotTuyen;
                            if (isMotTuyen && (string.IsNullOrEmpty(model.VeBusTuyenCode) || string.IsNullOrEmpty(model.VeBusTuyenName)))
                            {
                                throw new Exception("Tuyến xe không được để trống");
                            }
                            if (string.IsNullOrEmpty(model.VeBusNoiNhanThe))
                            {
                                throw new Exception("Nơi nhận thẻ không được để trống");
                            }
                            if (_veXeBusRepository.IsDuplicated(currentStudent.Id))
                            {
                                throw new DuplicateWaitObjectException();
                            }
                            AsAcademyStudentSvVeXeBus veXeBus = new AsAcademyStudentSvVeXeBus
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
                                DeletedBy = -1,
                                TuyenType = model.VeBusTuyenType,
                                TuyenCode = model.VeBusTuyenCode,
                                TuyenName = model.VeBusTuyenName,
                                NoiNhanThe = model.VeBusNoiNhanThe
                            };
                            await _veXeBusRepository.AddAsync(veXeBus);
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
                            throw new Exception(result.Message);
                        }
                        await _unitOfWork.SaveAsync();
                    }
                    scope.Complete();
                }
                #endregion
                #region log
                if (run)
                {
                    var dichVu = DichVuDictionary[model.Type];
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
                throw ex;
            }
        }

        public IQueryable GetAllByStudent(int dichVuType)
        {
            long studentId = _userService.GetCurrentStudentID() ?? 0;
            switch ((DichVu)dichVuType)
            {
                case DichVu.XacNhan:
                    return _xacNhanRepository.GetAll(studentId);
                case DichVu.GioiThieu:
                    return _gioiThieuRepository.GetAll(studentId);
                case DichVu.UuDaiGiaoDuc:
                    return _uuDaiRepository.GetAll(studentId);
                case DichVu.VayVonNganHang:
                    return _vayVonRepository.GetAll(studentId);
                case DichVu.ThueNha:
                    return _thueNhaRepository.GetAll(studentId);
                case DichVu.VeBus:
                    return _veXeBusRepository.GetAll(studentId);
                default:
                    break;
            }
            return null;
        }

        public async Task<DataTableResponse<QuanLyDichVuDetailResponse>> GetRequestForAdmin(QuanLyDichVuDetailModel model)
        {
            var result = new List<QuanLyDichVuDetailResponse>();
            List<AsAcademyStudent> studentList = await _studentRepository.GetAll().AsNoTracking().ToListAsync();

            int recordTotal = 0;
            int recordFiltered = 0;

            switch ((DichVu)model.Type)
            {
                case DichVu.XacNhan:
                    var xacNhanGetAll = await _xacNhanRepository.GetAllForAdmin(model);
                    var xacNhanList = xacNhanGetAll.FinalData;

                    foreach (var yeuCau in xacNhanList)
                    {
                        var student = studentList.FirstOrDefault(s => s.Code == yeuCau.StudentCode);
                        var item = new QuanLyDichVuDetailResponse()
                        {
                            Student = student,
                            DichVu = yeuCau
                        };
                        result.Add(item);
                    }
                    break;
                case DichVu.GioiThieu:
                    var gioiThieuGetAll = await _gioiThieuRepository.GetAllForAdmin(model);

                    var gioiThieuList = gioiThieuGetAll.FinalData;

                    foreach (var yeuCau in gioiThieuList)
                    {
                        var student = studentList.FirstOrDefault(s => s.Code == yeuCau.StudentCode);
                        var item = new QuanLyDichVuDetailResponse()
                        {
                            Student = student,
                            DichVu = yeuCau
                        };
                        result.Add(item);
                    }
                    break;
                case DichVu.UuDaiGiaoDuc:
                    var uuDaiGetAll = await _uuDaiRepository.GetAllForAdmin(model);

                    var uuDaiList = uuDaiGetAll.FinalData;

                    foreach (var yeuCau in uuDaiList)
                    {
                        var student = studentList.FirstOrDefault(s => s.Code == yeuCau.StudentCode);
                        var item = new QuanLyDichVuDetailResponse()
                        {
                            Student = student,
                            DichVu = yeuCau
                        };
                        result.Add(item);
                    }
                    break;
                case DichVu.VayVonNganHang:
                    var vayVonGetAll = await _vayVonRepository.GetAllForAdmin(model);

                    var vayVonList = vayVonGetAll.FinalData;

                    foreach (var yeuCau in vayVonList)
                    {
                        var student = studentList.FirstOrDefault(s => s.Code == yeuCau.StudentCode);
                        var item = new QuanLyDichVuDetailResponse()
                        {
                            Student = student,
                            DichVu = yeuCau
                        };
                        result.Add(item);
                    }
                    break;
                case DichVu.ThueNha:
                    var thueNhaGetAll = await _thueNhaRepository.GetAllForAdmin(model);

                    var thueNhaList = thueNhaGetAll.FinalData;

                    foreach (var yeuCau in thueNhaList)
                    {
                        var student = studentList.FirstOrDefault(s => s.Code == yeuCau.StudentCode);
                        var item = new QuanLyDichVuDetailResponse()
                        {
                            Student = student,
                            DichVu = yeuCau
                        };
                        result.Add(item);
                    }
                    break;
                case DichVu.VeBus:
                    var veXeBusGetAll = await _veXeBusRepository.GetAllForAdmin(model);
                    var veXeBusList = veXeBusGetAll.FinalData;

                    foreach (var yeuCau in veXeBusList)
                    {
                        var student = studentList.FirstOrDefault(s => s.Code == yeuCau.StudentCode);
                        var item = new QuanLyDichVuDetailResponse()
                        {
                            Student = student,
                            DichVu = yeuCau
                        };
                        result.Add(item);
                    }
                    break;
                default:
                    break;
            }

            var data = result.Skip(model.Page ?? 0).Take(model.Size ?? 0);

            recordTotal = result.Count();
            recordFiltered = data.Count();

            var rs = new DataTableResponse<QuanLyDichVuDetailResponse>
            {
                RecordsTotal = recordTotal,
                RecordsFiltered = recordTotal,
                Data = data.ToList()
            };
            return rs;
        }

        public Dictionary<int, AllTypeDichVuModel> GetAllLoaiDichVuInfo()
        {
            var allDichVu = _loaiDichVuRepository.GetAllInUse();
            var quantityDictionary = new Dictionary<int, AllTypeDichVuModel>
            {
                { (int)DichVu.XacNhan, _xacNhanRepository.GetRequestInfo() },
                { (int)DichVu.ThueNha, _thueNhaRepository.GetRequestInfo() },
                { (int)DichVu.UuDaiGiaoDuc, _uuDaiRepository.GetRequestInfo() },
                { (int)DichVu.VayVonNganHang, _vayVonRepository.GetRequestInfo() },
                { (int)DichVu.VeBus, _veXeBusRepository.GetRequestInfo() }
            };

            var SumDichVu = new AllTypeDichVuModel
            {
                TenDichVu = "Tổng cộng",
                DangXuLy = 0,
                DaXuLy = 0,
                MoiGui = 0,
                TongSo = 0,
            };

            foreach (var dichVu in allDichVu)
            {
                if (quantityDictionary.ContainsKey(dichVu.Id))
                {
                    var info = quantityDictionary[dichVu.Id];
                    info.TenDichVu = dichVu.Description;
                    info.LinkDichVu = dichVu.Param1;

                    SumDichVu.DaXuLy += info.DaXuLy;
                    SumDichVu.DangXuLy += info.DangXuLy;
                    SumDichVu.MoiGui += info.MoiGui;
                    SumDichVu.TongSo += info.TongSo;
                }
            }

            quantityDictionary.Add(-1, SumDichVu);

            return quantityDictionary;
        }
        #region tham số dịch vụ
        public async Task<DataTableResponse<AsAcademyStudentSvThietLapThamSoDichVu>> GetThamSoByDichVu(int loaiDichVu)
        {
            var result = await _thamSoDichVuService.GetParameters((DichVu)loaiDichVu).ToListAsync();
            return new DataTableResponse<AsAcademyStudentSvThietLapThamSoDichVu>
            {
                Data = result,
                RecordsTotal = result.Count,
                RecordsFiltered = result.Count,
            };
        }
        public async Task UpdateThamSoDichVu(Dictionary<long, string> thamSoDictionary)
        {
            var idList = thamSoDictionary.Keys.ToList();
            var thamSos = _thamSoDichVuService.GetParameters(idList);
            foreach (var thamSo in thamSos)
            {
                thamSo.Value = thamSoDictionary[thamSo.Id];
            }
            await _unitOfWork.SaveAsync();
        }
        #endregion
        #region Update Status
        public async Task UpdateRequestStatus(UpdateRequestStatusModel model)
        {
            var ngayHen = getUpdateStatusNgayHen(model);
            var fromDate = ngayHen.NgayHenBatDau;
            var toDate = ngayHen.NgayHenKetThuc;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var run = await updateStatusAsync((DichVu)model.Type, new UpdateRequestStatusModel
                    {
                        NgayHenBatDau = fromDate,
                        NgayHenKetThuc = toDate,
                        PhanHoi = model.PhanHoi,
                        Status = model.Status,
                        RequestID = model.RequestID,
                        Type = model.Type
                    });
                    if (run)
                    {
                        await _unitOfWork.SaveAsync();
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cập nhật trạng thái yêu cầu dịch vụ \n {ex.Message}");
                throw ex;
            }
        }

        public async Task UpdateMultiRequestToFourStatus(UpdateRequestStatusModel model)
        {
            DichVu loaiDichVu;
            try
            {
                loaiDichVu = (DichVu)model.Type;
            }
            catch (Exception)
            {
                throw new Exception("Mã loại dịch vụ không đúng");
            }

            int status = (int)TrangThaiYeuCau.DaXuLyVaCoLichHen;
            model.Status = status;

            var ngayHen = getUpdateStatusNgayHen(model);
            var fromDate = ngayHen.NgayHenBatDau;
            var toDate = ngayHen.NgayHenKetThuc;

            int count = 0;
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var item in model.DichVuList)
                    {
                        bool run = await updateStatusAsync(loaiDichVu, new UpdateRequestStatusModel
                        {
                            RequestID = item.ID,
                            NgayHenBatDau = fromDate,
                            NgayHenKetThuc = toDate,
                            PhanHoi = null,
                            Status = status,
                            Type = (int)loaiDichVu,
                        });
                        if (run)
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        await _unitOfWork.SaveAsync();
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cập nhật các yêu cầu lên trạng thái 4 {ex.ToString()}");
                throw ex;
            }
        }

        private GetUpdateStatusNgayHenModel getUpdateStatusNgayHen(UpdateRequestStatusModel model)
        {
            DateTime now = DateTime.Now;
            var dayOfWeek = (int)now.DayOfWeek;
            bool earlierThanFriday = now.DayOfWeek < DayOfWeek.Friday;
            bool isMorning = now.Hour <= 13;
            bool daXuLyCoLichHen = (TrangThaiYeuCau)model.Status == TrangThaiYeuCau.DaXuLyVaCoLichHen;

            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (daXuLyCoLichHen && !model.AutoUpdateNgayHen)
            {
                if (model.NgayHenBatDau == null)
                {
                    throw new Exception("Ngày bắt đầu không được trống");

                }
                else if (model.NgayHenKetThuc == null)
                {
                    throw new Exception("Ngày kết thúc không được trống");
                }
                else if (model.NgayHenBatDau < now)
                {
                    throw new Exception("Ngày bắt đầu không được nhỏ hơn hiện tại");
                }
                else if (model.NgayHenBatDau > model.NgayHenKetThuc)
                {
                    throw new Exception("Ngày bắt đầu không được lớn hơn ngày kết thúc");
                }
                fromDate = model.NgayHenBatDau;
                toDate = model.NgayHenKetThuc;
            }
            else if (daXuLyCoLichHen && model.AutoUpdateNgayHen)
            {
                if (isMorning)
                {
                    if (earlierThanFriday)
                    {
                        fromDate = DateTime.Parse(string.Format("{0}/{1}/{2} 10:00:00 AM", DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day));
                    }
                    else
                    {
                        fromDate = DateTime.Parse(string.Format("{0}/{1}/{2} 10:00:00 AM", DateTime.Now.AddDays(8 - dayOfWeek).Year, DateTime.Now.AddDays(8 - dayOfWeek).Month, DateTime.Now.AddDays(8 - dayOfWeek).Day));
                    }
                }
                else
                {
                    //Cap nhat vào buổi chiều
                    if (earlierThanFriday)
                    {
                        fromDate = DateTime.Parse(string.Format("{0}/{1}/{2} 4:00:00 PM", DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day));
                    }
                    else
                    {
                        fromDate = DateTime.Parse(string.Format("{0}/{1}/{2} 10:00:00 AM", DateTime.Now.AddDays(8 - dayOfWeek).Year, DateTime.Now.AddDays(8 - dayOfWeek).Month, DateTime.Now.AddDays(8 - dayOfWeek).Day));
                    }
                }
                toDate = fromDate?.AddMonths(1);
            }
            return new GetUpdateStatusNgayHenModel { NgayHenBatDau = fromDate, NgayHenKetThuc = toDate };
        }

        private async Task<bool> updateStatusAsync(DichVu loaiDichVu, UpdateRequestStatusModel model)
        {
            bool anyAction = false;

            AsAcademyStudent student = null;
            DateTime? ngayTao = DateTime.Now;
            #region dich vu
            switch (loaiDichVu)
            {
                case DichVu.XacNhan:
                    var xacNhan = await _xacNhanRepository.FindByIdAsync(model.RequestID);
                    xacNhan.Status = model.Status;
                    xacNhan.NgayHenTuNgay = model.NgayHenBatDau;
                    xacNhan.NgayHenDenNgay = model.NgayHenKetThuc;
                    xacNhan.PhanHoi = model.PhanHoi;
                    ngayTao = xacNhan.CreatedTime;

                    student = _studentRepository.FindByCode(xacNhan.StudentCode);
                    break;
                case DichVu.GioiThieu:
                    var gioiThieu = await _gioiThieuRepository.FindByIdAsync(model.RequestID);
                    gioiThieu.Status = model.Status;
                    gioiThieu.NgayHenTuNgay = model.NgayHenBatDau;
                    gioiThieu.NgayHenDenNgay = model.NgayHenKetThuc;
                    gioiThieu.PhanHoi = model.PhanHoi;
                    ngayTao = gioiThieu.CreatedTime;

                    student = _studentRepository.FindByCode(gioiThieu.StudentCode);
                    break;
                case DichVu.ThueNha:
                    var thueNha = await _thueNhaRepository.FindByIdAsync(model.RequestID);
                    thueNha.Status = model.Status;
                    thueNha.NgayHenTuNgay = model.NgayHenBatDau;
                    thueNha.NgayHenDenNgay = model.NgayHenKetThuc;
                    thueNha.PhanHoi = model.PhanHoi;
                    ngayTao = thueNha.CreatedTime;

                    student = _studentRepository.FindByCode(thueNha.StudentCode);
                    break;
                case DichVu.UuDaiGiaoDuc:
                    var uuDai = await _uuDaiRepository.FindByIdAsync(model.RequestID);
                    uuDai.Status = model.Status;
                    uuDai.PhanHoi = model.PhanHoi;
                    uuDai.NgayHenTuNgay = model.NgayHenBatDau;
                    uuDai.NgayHenDenNgay = model.NgayHenKetThuc;
                    ngayTao = uuDai.CreatedTime;

                    student = _studentRepository.FindByCode(uuDai.StudentCode);
                    break;
                case DichVu.VayVonNganHang:
                    var vayVon = await _vayVonRepository.FindByIdAsync(model.RequestID);
                    vayVon.Status = model.Status;
                    vayVon.PhanHoi = model.PhanHoi;
                    vayVon.NgayHenTuNgay = model.NgayHenBatDau;
                    vayVon.NgayHenDenNgay = model.NgayHenKetThuc;
                    ngayTao = vayVon.CreatedTime;

                    student = _studentRepository.FindByCode(vayVon.StudentCode);
                    break;
                case DichVu.VeBus:
                    var veBus = await _veXeBusRepository.FindByIdAsync(model.RequestID);
                    veBus.Status = model.Status;
                    veBus.PhanHoi = model.PhanHoi;
                    veBus.NgayHenTuNgay = model.NgayHenBatDau;
                    veBus.NgayHenDenNgay = model.NgayHenKetThuc;
                    ngayTao = veBus.CreatedTime;

                    student = _studentRepository.FindByCode(veBus.StudentCode);
                    break;
                default:
                    break;
            }
            #endregion
            if (student != null)
            {
                #region email
                var dichVu = DichVuDictionary[(int)loaiDichVu];
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
                    NgayHen = model.NgayHenBatDau,
                    NgayTao = ngayTao,
                };
                var sendEmailRs = await _emailService.SendEmailUpdateStatusRequest(tinNhan);
                if (sendEmailRs != null)
                {
                    throw new Exception(sendEmailRs.Message);
                }
                anyAction = true;
                #endregion
            }
            return anyAction;
        }
        #endregion

        #region Export Excel
        public async Task<byte[]> ExportExcelOverviewAsync()
        {
            var data = GetAllLoaiDichVuInfo();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                int i = 0;
                int firstRow = 1;
                #region title
                setStyle(ws, firstRow, ++i, "STT");
                setStyle(ws, firstRow, ++i, "Dịch vụ");
                setStyle(ws, firstRow, ++i, "Tổng số");
                setStyle(ws, firstRow, ++i, "Mới gửi");
                setStyle(ws, firstRow, ++i, "Đang xử lý");
                setStyle(ws, firstRow, ++i, "Đã xử lý xong");

                ws.Row(firstRow).Height = 32;

                int colNum = i;
                #endregion
                #region value
                int recordLen = data.Count;
                int col = 0;
                for (int j = 0; j < recordLen; j++)
                {
                    var key = data.Keys.ElementAt(j);
                    var dichVu = data[key];

                    int stt = j + 1;
                    string dichVuName = dichVu.TenDichVu;
                    int tongSo = dichVu.TongSo;
                    int moiGui = dichVu.MoiGui;
                    int dangXuLy = dichVu.DangXuLy;
                    int daXuLy = dichVu.DaXuLy;

                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(stt);
                    ws.Cell(row, ++col).SetValue(dichVuName);
                    ws.Cell(row, ++col).SetValue(tongSo);
                    ws.Cell(row, ++col).SetValue(moiGui);
                    ws.Cell(row, ++col).SetValue(dangXuLy);
                    ws.Cell(row, ++col).SetValue(daXuLy);
                }
                for (int j = 0; j < col; j++)
                {
                    ws.Column(j + 1).AdjustToContents();
                }
                #endregion
                string file = _pathProvider.MapPath($"Templates/Ctsv/overview_{Guid.NewGuid().ToString()}.xlsx");
                wb.SaveAs(file);
                return await FileToByteAsync(file);
            }
        }
        public async Task<byte[]> ExportExcelAsync(DichVu loaiDichVu, List<DichVuExport> dichVuList)
        {
            switch (loaiDichVu)
            {
                case DichVu.XacNhan:
                    return await ExportExcelXacNhan(dichVuList);
                case DichVu.UuDaiGiaoDuc:
                    return await ExportExcelUuDai(dichVuList);
                case DichVu.VayVonNganHang:
                    return await ExportExcelVayVon(dichVuList);
                case DichVu.ThueNha:
                    return await ExportExcelThueNha(dichVuList);
                case DichVu.VeBus:
                    return await ExportExcelVeXeBus(dichVuList);
                default:
                    break;
            }
            return null;
        }

        private async Task<byte[]> ExportExcelXacNhan(List<DichVuExport> dichVuList)
        {
            List<long> ids = new List<long>();
            foreach (var item in dichVuList)
            {
                ids.Add(item.ID);
            }
            var yeuCauList = (await _xacNhanRepository.GetYeuCauDichVuStudent(ids)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                int i = 0;
                int firstRow = 1;
                #region title
                setStyle(ws, firstRow, ++i, "Mã số SV");
                setStyle(ws, firstRow, ++i, "Họ và tên");
                setStyle(ws, firstRow, ++i, "Ngày sinh");
                setStyle(ws, firstRow, ++i, "Email");
                setStyle(ws, firstRow, ++i, "Xã");
                setStyle(ws, firstRow, ++i, "Huyện");
                setStyle(ws, firstRow, ++i, "Tỉnh");
                setStyle(ws, firstRow, ++i, "Lớp");
                setStyle(ws, firstRow, ++i, "Niên khóa");
                setStyle(ws, firstRow, ++i, "Khoa quản lý");
                setStyle(ws, firstRow, ++i, "Số điện thoại");
                setStyle(ws, firstRow, ++i, "Hệ đào tạo");
                setStyle(ws, firstRow, ++i, "Lý do xác nhận");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);

                setStyle(ws, firstRow, ++i, "Ngày ký");
                setStyle(ws, firstRow, ++i, "Tháng ký");
                setStyle(ws, firstRow, ++i, "Năm ký");

                ws.Row(firstRow).Height = 32;

                int colNum = i;
                #endregion
                #region value
                int recordLen = yeuCauList.Count;
                int col = 0;
                for (int j = 0; j < recordLen; j++)
                {
                    var yeuCau = yeuCauList[j];
                    DateTime ngaySinh = convertStudentDateOfBirth(yeuCau.Student.DateOfBirth);
                    string studentCode = yeuCau.YeuCauDichVu.StudentCode ?? "";
                    string studentName = yeuCau.YeuCauDichVu.StudentName ?? "";
                    string email = yeuCau.Student.EmailNhaTruong ?? "";
                    string phuong = yeuCau.Student.HkttPhuong ?? "";
                    string quan = yeuCau.Student.HkttQuan ?? "";
                    string tinh = yeuCau.Student.HkttTinh ?? "";
                    string classCode = yeuCau.Student.ClassCode ?? "";
                    string nienKhoa = yeuCau.AcademyClass.SchoolYear ?? "";
                    string tenKhoa = yeuCau.Faculty?.Name ?? "";
                    string mobile = yeuCau.Student.Mobile ?? "";

                    string lyDo = yeuCau.YeuCauDichVu.LyDo ?? "";
                    DateTime now = DateTime.Now;
                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(studentCode);
                    ws.Cell(row, ++col).SetValue(studentName);
                    ws.Cell(row, ++col).SetValue(ngaySinh.ToString("dd/MM/yyyy"));
                    ws.Cell(row, ++col).SetValue(email);
                    ws.Cell(row, ++col).SetValue(phuong);
                    ws.Cell(row, ++col).SetValue(quan);
                    ws.Cell(row, ++col).SetValue(tinh);
                    ws.Cell(row, ++col).SetValue(classCode);
                    ws.Cell(row, ++col).SetValue(nienKhoa);
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(mobile);
                    ws.Cell(row, ++col).SetValue("Chính quy");
                    ws.Cell(row, ++col).SetValue(lyDo);
                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);
                }
                for (int j = 0; j < col; j++)
                {
                    ws.Column(j + 1).AdjustToContents();
                }
                #endregion
                string file = _pathProvider.MapPath($"Templates/Ctsv/xacnhan_{Guid.NewGuid().ToString()}.xlsx");
                wb.SaveAs(file);
                return await FileToByteAsync(file);
            }
        }

        private async Task<byte[]> ExportExcelUuDai(List<DichVuExport> dichVuList)
        {
            List<long> ids = new List<long>();
            foreach (var item in dichVuList)
            {
                ids.Add(item.ID);
            }
            var yeuCauList = (await _uuDaiRepository.GetYeuCauDichVuStudent(ids)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                string namHoc = "";
                if (yeuCauList.Any())
                {
                    var schoolYearFrom = yeuCauList[0].Year.FromYear;
                    var schoolYearTo = yeuCauList[0].Year.ToYear;

                    namHoc = $"{schoolYearFrom?.Year.ToString()}-{schoolYearTo?.Year.ToString()}";
                }                
                int i = 0;
                int firstRow = 1;
                #region title
                setStyle(ws, firstRow, ++i, "Mã số SV");
                setStyle(ws, firstRow, ++i, "Họ và tên");
                setStyle(ws, firstRow, ++i, "Ngày sinh");
                setStyle(ws, firstRow, ++i, "Email");
                setStyle(ws, firstRow, ++i, "Xã");
                setStyle(ws, firstRow, ++i, "Huyện");
                setStyle(ws, firstRow, ++i, "Tỉnh");
                setStyle(ws, firstRow, ++i, "Lớp");
                setStyle(ws, firstRow, ++i, "Niên khóa");
                setStyle(ws, firstRow, ++i, "Khoa quản lý");
                setStyle(ws, firstRow, ++i, "Số điện thoại");
                setStyle(ws, firstRow, ++i, "Hệ đào tạo");
                setStyle(ws, firstRow, ++i, "Năm thứ");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);
                setStyle(ws, firstRow, ++i, "Học Kỳ");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);
                setStyle(ws, firstRow, ++i, "Năm học");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);
                setStyle(ws, firstRow, ++i, "Thời gian khóa học \n (bao nhiêu năm)");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);
                ws.Cell(firstRow, i).Style.Alignment.WrapText = true;
                setStyle(ws, firstRow, ++i, "Kỷ luật");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);

                setStyle(ws, firstRow, ++i, "Ngày ký");
                setStyle(ws, firstRow, ++i, "Tháng ký");
                setStyle(ws, firstRow, ++i, "Năm ký");

                ws.Row(firstRow).Height = 32;

                int colNum = i;
                #endregion
                #region value
                int recordLen = yeuCauList.Count;
                int col = 0;
                for (int j = 0; j < recordLen; j++)
                {
                    var yeuCau = yeuCauList[j];
                    DateTime ngaySinh = convertStudentDateOfBirth(yeuCau.Student.DateOfBirth);
                    string studentCode = yeuCau.YeuCauDichVu.StudentCode ?? "";
                    string studentName = yeuCau.YeuCauDichVu.StudentName ?? "";
                    string email = yeuCau.Student.EmailNhaTruong ?? "";
                    string phuong = yeuCau.Student.HkttPhuong ?? "";
                    string quan = yeuCau.Student.HkttQuan ?? "";
                    string tinh = yeuCau.Student.HkttTinh ?? "";
                    string classCode = yeuCau.Student.ClassCode ?? "";
                    string nienKhoa = yeuCau.AcademyClass.SchoolYear ?? "";
                    string tenKhoa = yeuCau.Faculty?.Name ?? "";
                    string mobile = yeuCau.Student.Mobile ?? "";

                    string kyLuat = yeuCau.YeuCauDichVu.KyLuat ?? "Không";
                    string namThu = getNamThu(nienKhoa);
                    string hocKy = "1";
                    string thoiGianKhoaHoc = getThoiGianKhoaHoc(nienKhoa);
                    DateTime now = DateTime.Now;
                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(studentCode);
                    ws.Cell(row, ++col).SetValue(studentName);
                    ws.Cell(row, ++col).SetValue(ngaySinh.ToString("dd/MM/yyyy"));
                    ws.Cell(row, ++col).SetValue(email);
                    ws.Cell(row, ++col).SetValue(phuong);
                    ws.Cell(row, ++col).SetValue(quan);
                    ws.Cell(row, ++col).SetValue(tinh);
                    ws.Cell(row, ++col).SetValue(classCode);
                    ws.Cell(row, ++col).SetValue(nienKhoa);
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(mobile);
                    ws.Cell(row, ++col).SetValue("Chính quy");
                    ws.Cell(row, ++col).SetValue(namThu);
                    ws.Cell(row, col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Cell(row, ++col).SetValue(hocKy);
                    ws.Cell(row, col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Cell(row, ++col).SetValue(namHoc);
                    ws.Cell(row, col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Cell(row, ++col).SetValue(thoiGianKhoaHoc);
                    ws.Cell(row, col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Cell(row, ++col).SetValue(kyLuat);
                    ws.Cell(row, col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);
                }
                for (int j = 0; j < col; j++)
                {
                    ws.Column(j + 1).AdjustToContents();
                }
                string file = _pathProvider.MapPath($"Templates/Ctsv/uudai_{Guid.NewGuid().ToString()}.xlsx");
                wb.SaveAs(file);
                return await FileToByteAsync(file);
                #endregion
            }
        }

        private async Task<byte[]> ExportExcelVayVon(List<DichVuExport> dichVuList)
        {
            List<long> ids = new List<long>();
            foreach (var item in dichVuList)
            {
                ids.Add(item.ID);
            }
            var yeuCauList = (await _vayVonRepository.GetYeuCauDichVuStudent(ids)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                
                var hocPhiThangRow = _thamSoDichVuService.GetParameters(DichVu.VayVonNganHang).FirstOrDefault(p => p.Name == "HocPhiThang");
                string hocPhiThang = hocPhiThangRow != null ? hocPhiThangRow.Value : "";

                int i = 0;
                int firstRow = 1;
                #region title
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Ngày sinh");
                initHeaderCell(ws, firstRow, ++i, "Giới tính");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Khoa/Ban quản lý");
                initHeaderCell(ws, firstRow, ++i, "Số CMTND");
                initHeaderCell(ws, firstRow, ++i, "Ngày cấp");
                initHeaderCell(ws, firstRow, ++i, "Nơi cấp");
                initHeaderCell(ws, firstRow, ++i, "Mã trường");
                initHeaderCell(ws, firstRow, ++i, "Tên trường");
                initHeaderCell(ws, firstRow, ++i, "Ngành học");
                initHeaderCell(ws, firstRow, ++i, "Hệ đào tạo");
                initHeaderCell(ws, firstRow, ++i, "Khoá");
                initHeaderCell(ws, firstRow, ++i, "Loại hình đào tạo");
                initHeaderCell(ws, firstRow, ++i, "Năm nhập học");
                initHeaderCell(ws, firstRow, ++i, "Thời gian ra trường");
                initHeaderCell(ws, firstRow, ++i, "Số tiền học phí hàng tháng");
                initHeaderCell(ws, firstRow, ++i, "Ngày ký");
                initHeaderCell(ws, firstRow, ++i, "Tháng ký");
                initHeaderCell(ws, firstRow, ++i, "Năm ký");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");

                ws.Row(firstRow).Height = 32;
                string tenTruong = "ĐẠI HỌC XÂY DỰNG";

                int colNum = i;
                #endregion
                #region value
                int recordLen = yeuCauList.Count;
                int col = 0;
                for (int j = 0; j < recordLen; j++)
                {
                    var yeuCau = yeuCauList[j];

                    DateTime? cmtNgayCap = yeuCau.Student.CmtNgayCap;
                    DateTime ngaySinh = convertStudentDateOfBirth(yeuCau.Student.DateOfBirth);
                    string studentCode = yeuCau.YeuCauDichVu.StudentCode ?? "";
                    string studentName = yeuCau.YeuCauDichVu.StudentName ?? "";
                    string email = yeuCau.Student.EmailNhaTruong ?? "";
                    string phuong = yeuCau.Student.HkttPhuong ?? "";
                    string quan = yeuCau.Student.HkttQuan ?? "";
                    string tinh = yeuCau.Student.HkttTinh ?? "";
                    string classCode = yeuCau.Student.ClassCode ?? "";
                    string nienKhoa = yeuCau.AcademyClass.SchoolYear ?? "";
                    string tenKhoa = yeuCau.Faculty?.Name ?? "";
                    string mobile = yeuCau.Student.Mobile ?? "";
                    string gioiTinh = yeuCau.Student.GioiTinh ?? "";
                    gioiTinh = getGender(gioiTinh);
                    string cmtNoiCap = yeuCau.Student.CmtNoiCap ?? "";
                    string cmt = yeuCau.Student.Cmt ?? "";
                    string soDienThoai = mobile;
                    string nganhHoc = yeuCau.Academics?.Name ?? "";
                    DateTime now = DateTime.Now;
                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(studentCode);
                    ws.Cell(row, ++col).SetValue(studentName);
                    ws.Cell(row, ++col).SetValue(ngaySinh.ToString("dd/MM/yyyy"));
                    ws.Cell(row, ++col).SetValue(gioiTinh);
                    ws.Cell(row, ++col).SetValue(classCode);
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(cmt);
                    ws.Cell(row, ++col).SetValue(cmtNgayCap?.ToString("dd/MM/yyyy"));
                    ws.Cell(row, ++col).SetValue(cmtNoiCap);
                    ws.Cell(row, ++col).SetValue("XD1");
                    ws.Cell(row, ++col).SetValue(tenTruong);
                    ws.Cell(row, ++col).SetValue(nganhHoc);
                    ws.Cell(row, ++col).SetValue("ĐẠI HỌC");
                    ws.Cell(row, ++col).SetValue(getKhoa(classCode));
                    ws.Cell(row, ++col).SetValue("Chính quy");
                    ws.Cell(row, ++col).SetValue(getNamNhapHoc(nienKhoa));
                    ws.Cell(row, ++col).SetValue(getNamRaTruong(nienKhoa));
                    ws.Cell(row, ++col).SetValue(hocPhiThang);
                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);
                    ws.Cell(row, ++col).SetValue(soDienThoai);
                }
                for (int j = 0; j < col; j++)
                {
                    ws.Column(j + 1).AdjustToContents();
                }
                #endregion
                string file = _pathProvider.MapPath($"Templates/Ctsv/vayvon_{Guid.NewGuid().ToString()}.xlsx");
                wb.SaveAs(file);
                return await FileToByteAsync(file);
            }
        }

        private async Task<byte[]> ExportExcelThueNha(List<DichVuExport> dichVuList)
        {
            List<long> ids = new List<long>();
            foreach (var item in dichVuList)
            {
                ids.Add(item.ID);
            }
            var yeuCauList = (await _thueNhaRepository.GetYeuCauDichVuStudent(ids)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                int i = 0;
                int firstRow = 1;
                #region title
                setStyle(ws, firstRow, ++i, "Mã số SV");
                setStyle(ws, firstRow, ++i, "Họ và tên");
                setStyle(ws, firstRow, ++i, "Ngày sinh");
                setStyle(ws, firstRow, ++i, "Email");
                setStyle(ws, firstRow, ++i, "Xã");
                setStyle(ws, firstRow, ++i, "Huyện");
                setStyle(ws, firstRow, ++i, "Tỉnh");
                setStyle(ws, firstRow, ++i, "Lớp");
                setStyle(ws, firstRow, ++i, "Niên khóa");
                setStyle(ws, firstRow, ++i, "Khoa quản lý");
                setStyle(ws, firstRow, ++i, "Số điện thoại");
                setStyle(ws, firstRow, ++i, "Hệ đào tạo");

                setStyleUniqueCol(ws, firstRow, ++i, "Năm thứ");
                setStyleUniqueCol(ws, firstRow, ++i, "Giới tính");
                setStyleUniqueCol(ws, firstRow, ++i, "Đối tượng\nưu tiên");
                ws.Cell(firstRow, i).Style.Alignment.WrapText = true;
                setStyleUniqueCol(ws, firstRow, ++i, "Số CMTND");
                setStyleUniqueCol(ws, firstRow, ++i, "Cấp\nngày/tháng/năm");
                ws.Cell(firstRow, i).Style.Alignment.WrapText = true;
                setStyleUniqueCol(ws, firstRow, ++i, "Nơi cấp");

                setStyle(ws, firstRow, ++i, "Ngày ký");
                setStyle(ws, firstRow, ++i, "Tháng ký");
                setStyle(ws, firstRow, ++i, "Năm ký");

                ws.Row(firstRow).Height = 32;

                int colNum = i;
                #endregion
                #region value
                int recordLen = yeuCauList.Count;
                int col = 0;
                for (int j = 0; j < recordLen; j++)
                {
                    var yeuCau = yeuCauList[j];
                    DateTime? cmtNgayCap = yeuCau.Student.CmtNgayCap;
                    DateTime ngaySinh = convertStudentDateOfBirth(yeuCau.Student.DateOfBirth);
                    string studentCode = yeuCau.YeuCauDichVu.StudentCode ?? "";
                    string studentName = yeuCau.YeuCauDichVu.StudentName ?? "";
                    string email = yeuCau.Student.EmailNhaTruong ?? "";
                    string phuong = yeuCau.Student.HkttPhuong ?? "";
                    string quan = yeuCau.Student.HkttQuan ?? "";
                    string tinh = yeuCau.Student.HkttTinh ?? "";
                    string classCode = yeuCau.Student.ClassCode ?? "";
                    string nienKhoa = yeuCau.AcademyClass.SchoolYear ?? "";
                    string tenKhoa = yeuCau.Faculty?.Name ?? "";
                    string mobile = yeuCau.Student.Mobile ?? "";
                    string gioiTinh = yeuCau.Student.GioiTinh ?? "";
                    gioiTinh = getGender(gioiTinh);
                    string cmtNoiCap = yeuCau.Student.CmtNoiCap ?? "";
                    string cmt = yeuCau.Student.Cmt ?? "";
                    string soDienThoai = mobile;
                    string nganhHoc = yeuCau.Academics?.Name ?? "";
                    string doiTuongUuTien = yeuCau.Student.DoiTuongUuTien ?? "";
                    DateTime now = DateTime.Now;
                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(studentCode);
                    ws.Cell(row, ++col).SetValue(studentName);
                    ws.Cell(row, ++col).SetValue(ngaySinh.ToString("dd/MM/yyyy"));
                    ws.Cell(row, ++col).SetValue(email);
                    ws.Cell(row, ++col).SetValue(phuong);
                    ws.Cell(row, ++col).SetValue(quan);
                    ws.Cell(row, ++col).SetValue(tinh);
                    ws.Cell(row, ++col).SetValue(classCode);
                    ws.Cell(row, ++col).SetValue(nienKhoa);
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(mobile);
                    ws.Cell(row, ++col).SetValue("Chính quy");
                    ws.Cell(row, ++col).SetValue(getNamThu(nienKhoa));
                    ws.Cell(row, ++col).SetValue(gioiTinh);
                    ws.Cell(row, ++col).SetValue(doiTuongUuTien);
                    ws.Cell(row, ++col).SetValue(cmt);
                    ws.Cell(row, ++col).SetValue(cmtNgayCap?.ToString("dd/MM/yyyy"));
                    ws.Cell(row, ++col).SetValue(cmtNoiCap);
                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);
                }
                for (int j = 0; j < col; j++)
                {
                    ws.Column(j + 1).AdjustToContents();
                }
                #endregion
                string file = _pathProvider.MapPath($"Templates/Ctsv/vayvon_{Guid.NewGuid().ToString()}.xlsx");
                wb.SaveAs(file);
                return await FileToByteAsync(file);
            }
        }

        private async Task<byte[]> ExportExcelVeXeBus(List<DichVuExport> dichVuList)
        {
            List<long> ids = new List<long>();
            foreach (var item in dichVuList)
            {
                ids.Add(item.ID);
            }
            var yeuCauList = (await _veXeBusRepository.GetYeuCauDichVuStudent(ids)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                int i = 0;
                int firstRow = 1;
                #region title
                setStyle(ws, firstRow, ++i, "Mã số SV");
                setStyle(ws, firstRow, ++i, "Họ và tên");
                setStyle(ws, firstRow, ++i, "Ngày sinh");
                setStyle(ws, firstRow, ++i, "Email");
                setStyle(ws, firstRow, ++i, "Xã");
                setStyle(ws, firstRow, ++i, "Huyện");
                setStyle(ws, firstRow, ++i, "Tỉnh");
                setStyle(ws, firstRow, ++i, "Lớp");
                setStyle(ws, firstRow, ++i, "Niên khóa");
                setStyle(ws, firstRow, ++i, "Khoa quản lý");
                setStyle(ws, firstRow, ++i, "Số điện thoại");
                setStyle(ws, firstRow, ++i, "Hệ đào tạo");

                setStyleUniqueCol(ws, firstRow, ++i, "Liên tuyến");
                setStyleUniqueCol(ws, firstRow, ++i, "Số tuyến");
                setStyleUniqueCol(ws, firstRow, ++i, "Nơi nộp đơn và nhận thẻ");

                setStyle(ws, firstRow, ++i, "Ngày ký");
                setStyle(ws, firstRow, ++i, "Tháng ký");
                setStyle(ws, firstRow, ++i, "Năm ký");

                ws.Row(firstRow).Height = 32;

                int colNum = i;
                #endregion
                #region value
                int recordLen = yeuCauList.Count;
                int col = 0;
                for (int j = 0; j < recordLen; j++)
                {
                    var yeuCau = yeuCauList[j];
                    DateTime ngaySinh = convertStudentDateOfBirth(yeuCau.Student.DateOfBirth);
                    string studentCode = yeuCau.YeuCauDichVu.StudentCode ?? "";
                    string studentName = yeuCau.YeuCauDichVu.StudentName ?? "";
                    string email = yeuCau.Student.EmailNhaTruong ?? "";
                    string phuong = yeuCau.Student.HkttPhuong ?? "";
                    string quan = yeuCau.Student.HkttQuan ?? "";
                    string tinh = yeuCau.Student.HkttTinh ?? "";
                    string classCode = yeuCau.Student.ClassCode ?? "";
                    string nienKhoa = yeuCau.AcademyClass.SchoolYear ?? "";
                    string tenKhoa = yeuCau.Faculty?.Name ?? "";
                    string mobile = yeuCau.Student.Mobile ?? "";
                    string lienTuyen = (DichVuXeBusLoaiTuyen)yeuCau.YeuCauDichVu.TuyenType == DichVuXeBusLoaiTuyen.LienTuyen ? "x" : "";
                    string soTuyen = yeuCau.YeuCauDichVu.TuyenCode;
                    string noiNhanThe = yeuCau.YeuCauDichVu.NoiNhanThe;
                    DateTime now = DateTime.Now;
                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(studentCode);
                    ws.Cell(row, ++col).SetValue(studentName);
                    ws.Cell(row, ++col).SetValue(ngaySinh.ToString("dd/MM/yyyy"));
                    ws.Cell(row, ++col).SetValue(email);
                    ws.Cell(row, ++col).SetValue(phuong);
                    ws.Cell(row, ++col).SetValue(quan);
                    ws.Cell(row, ++col).SetValue(tinh);
                    ws.Cell(row, ++col).SetValue(classCode);
                    ws.Cell(row, ++col).SetValue(nienKhoa);
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(mobile);
                    ws.Cell(row, ++col).SetValue("Chính quy");

                    ws.Cell(row, ++col).SetValue(lienTuyen);
                    ws.Cell(row, ++col).SetValue(soTuyen);
                    ws.Cell(row, ++col).SetValue(noiNhanThe);

                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);
                }
                for (int j = 0; j < col; j++)
                {
                    ws.Column(j + 1).AdjustToContents();
                }
                #endregion
                string file = _pathProvider.MapPath($"Templates/Ctsv/vayvon_{Guid.NewGuid().ToString()}.xlsx");
                wb.SaveAs(file);
                return await FileToByteAsync(file);
            }
        }
        #endregion

        #region Export Word (docx & zip)
        public async Task<byte[]> ExportWordListAsync(DichVu dichVu, List<DichVuExport> dichVuList)
        {
            List<string> dirs = new List<string>();
            foreach (var yeuCau in dichVuList)
            {
                var exportResult = await GetExportOutput(dichVu, yeuCau.ID);
                string dir = exportResult.filePath;
                if (exportResult.document != null)
                {
                    exportResult.document.Save(dir);
                }
                dirs.Add(dir);
            }

            string zipname = _pathProvider.MapPath($"Templates/Ctsv/{dichVu.ToString()}_{Guid.NewGuid().ToString()}.zip");
            var zip = ZipFile.Open(zipname, ZipArchiveMode.Create);
            foreach (var dir in dirs)
            {
                zip.CreateEntryFromFile(dir, Path.GetFileName(dir), CompressionLevel.Optimal);
                File.Delete(dir);
            }
            zip.Dispose();
            byte[] byteArr = await File.ReadAllBytesAsync(zipname);
            File.Delete(zipname);
            return byteArr;
        }

        public async Task<byte[]> ExportWordAsync(DichVu dichVu, int id)
        {
            try
            {
                var exportOutput = await GetExportOutput(dichVu, id);
                if (exportOutput == null) return null;
                if (exportOutput.document != null)
                {
                    return await DocumentToByteAsync(exportOutput.document, exportOutput.filePath);
                } else if (exportOutput.document == null)
                {
                    return await FileToByteAsync(exportOutput.filePath);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Có lỗi: ", ex.Message, " |||| \n", ex.ToString());
                throw ex;
            }
        }

        private async Task<ExportFileOutputModel> GetExportOutput(DichVu dichVu, int id)
        {
            switch (dichVu)
            {
                case DichVu.XacNhan:
                    return await ExportWordXacNhan(id);
                case DichVu.UuDaiGiaoDuc:
                    return await ExportWordUuDai(id);
                case DichVu.VayVonNganHang:
                    return await ExportWordVayVon(id);
                case DichVu.ThueNha:
                    return await ExportWordThueNha(id);
                case DichVu.VeBus:
                    return await ExportWordVeXeBus(id);
                default:
                    break;
            }
            return null;
        }

        private async Task<ExportFileOutputModel> ExportWordXacNhan(int id)
        {
            var xacNhan = await _xacNhanRepository.FindByIdAsync(id);
            if (xacNhan == null)
            {
                throw new Exception("Yêu cầu không tồn tại");
            }
            var studentInfo = await _studentRepository.GetStudentDichVuInfoAsync(xacNhan.StudentCode);
            if (studentInfo == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }

            var paramSet = _thamSoDichVuService.GetParameters(DichVu.XacNhan)
                                .ToDictionary(x => x.Name, x => x.Value);

            string desChucDanhNguoiKy = paramSet["ChucDanhNguoiKy"].Replace("\r", "_").Replace("\n", "_");
            string desTenNguoiKy = paramSet["TenNguoiKy"].Replace("\r", "_").Replace("\n", "_");

            DateTime NgaySinh;
            string tmpNgaySinh = studentInfo.Student.DateOfBirth ?? "1/1/1980";
            try
            {
                NgaySinh = DateTime.ParseExact(tmpNgaySinh, "dd/MM/yy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                NgaySinh = DateTime.Parse(tmpNgaySinh);
            }

            string HKTT_Pho = studentInfo.Student.HkttPho ?? "";
            string HKTT_Phuong = studentInfo.Student.HkttPhuong ?? "";
            string HKTT_Quan = studentInfo.Student.HkttQuan ?? "";
            string HKTT_Tinh = studentInfo.Student.HkttTinh ?? "";
            string MaSV = studentInfo.Student.Code.ToString();
            string HoVaTen = studentInfo.Student.FulName;
            string Class = studentInfo.Student.ClassCode ?? "";
            string TenKhoa = studentInfo.Faculty?.Name ?? "";
            string HeDaoTao = "Chính quy";
            string NienKhoa = studentInfo.AcademyClass.SchoolYear;
            string LyDoXacNhan = xacNhan.LyDo ?? "";
            string specialReason = "Xin tạm hoãn nghĩa vụ quân sự (đối với SV hết hạn chính khóa)";
            if (LyDoXacNhan == specialReason)
            {
                LyDoXacNhan = "Xin tạm hoãn nghĩa vụ quân sự (do SV còn nợ môn học nên chưa thể tốt nghiệp theo đúng thời hạn chính khóa, SV được gia hạn thời gian học tập tại trường theo Khoản 3 Điều 6 Quyết định số 43/2007/QĐBGDĐT ngày 15/8/2007 của Bộ trưởng Bộ Giáo dục & Đào tạo)";
            }
            else
            {
                LyDoXacNhan = LyDoXacNhan.Replace("\n", "").Replace("\r", "");
            }

            //ComponentInfo.SetLicense("DTZX-HTZ5-B7Q6-2GA6");
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            DocumentModel document = new DocumentModel();
            document.DefaultCharacterFormat.Size = 12;
            document.DefaultCharacterFormat.FontName = "Times New Roman";

            Section section;
            section = new Section(document);
            #region Cộng hòa xã hội chủ nghĩa việt nam
            GemboxTable table = new GemboxTable(document);
            table.TableFormat.PreferredWidth = new GemboxTableWidth(100, TableWidthUnit.Percentage);
            table.TableFormat.Alignment = HorizontalAlignment.Center;
            var tableBorders = table.TableFormat.Borders;
            tableBorders.SetBorders(MultipleBorderTypes.All, BorderStyle.None, GemboxColor.Empty, 0);
            table.Columns.Add(new TableColumn() { PreferredWidth = 35 });
            table.Columns.Add(new TableColumn() { PreferredWidth = 65 });
            GemboxTableRow rowT1 = new GemboxTableRow(document);
            table.Rows.Add(rowT1);

            rowT1.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", "BỘ GIÁO DỤC VÀ ĐÀO TẠO"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Size = 12
                }
            }
            )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = HorizontalAlignment.Center
                }
            })
            {
                CellFormat = new TableCellFormat()
                {
                    VerticalAlignment = VerticalAlignment.Center
                }
            }
            );
            rowT1.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 12
                }
            }
           )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = HorizontalAlignment.Center
                }
            })
            {
                CellFormat = new TableCellFormat()
                {
                    VerticalAlignment = VerticalAlignment.Center
                }
            }
           );
            GemboxTableRow rowT2 = new GemboxTableRow(document);
            table.Rows.Add(rowT2);

            rowT2.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", "TRƯỜNG ĐẠI HỌC XÂY DỰNG"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 12
                }
            }
            )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = HorizontalAlignment.Center
                }
            })
            {
                CellFormat = new TableCellFormat()
                {
                    VerticalAlignment = VerticalAlignment.Center
                }
            }
            );
            rowT2.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", "Độc lập – Tự do – Hạnh phúc"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 12
                }
            }
           )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = HorizontalAlignment.Center
                }
            })
            {
                CellFormat = new TableCellFormat()
                {
                    VerticalAlignment = VerticalAlignment.Center
                }
            }
           );
            var paragraph = new GemboxParagraph(document);

            var horizontalLine1 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                 new HorizontalPosition(1, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                new Size(125, 0)));
            horizontalLine1.Outline.Width = 1;
            horizontalLine1.Outline.Fill.SetSolid(GemboxColor.Black);
            paragraph.Inlines.Add(horizontalLine1);

            var horizontalLine2 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                new HorizontalPosition(8.78, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
               new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
               new Size(151, 0)));
            horizontalLine2.Outline.Width = 1;
            horizontalLine2.Outline.Fill.SetSolid(GemboxColor.Black);
            paragraph.Inlines.Add(horizontalLine2);

            section.Blocks.Add(table);
            section.Blocks.Add(paragraph);
            #endregion
            #region TieuDe
            GemboxParagraph paragraphTieuDe = new GemboxParagraph(document,
            //new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new GemboxRun(document, string.Format("{0}", "GIẤY XÁC NHẬN"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 16
                }
            }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new GemboxRun(document, "TRƯỜNG ĐẠI HỌC XÂY DỰNG")
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 14
                }
            }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new GemboxRun(document, "XÁC NHẬN")
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 14
                }
            }
          )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = HorizontalAlignment.Center,
                    LineSpacing = 1.15
                }
            };
            section.Blocks.Add(paragraphTieuDe);
            #endregion
            #region NoiDung
            GemboxParagraph paragraphNoiDung = new GemboxParagraph(document,
           // new SpecialCharacter(document, SpecialCharacterType.LineBreak),
           new GemboxRun(document, string.Format("{0}", "Anh (chị): "))
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = 13
               }
           }
           , new GemboxRun(document, string.Format("{0}", HoVaTen))
           {
               CharacterFormat = new CharacterFormat()
               {
                   Bold = true,
                   Size = 13
               }
           }
           , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
           , new GemboxRun(document, "Sinh ngày: ")
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = 13
               }
           }
            , new GemboxRun(document, string.Format("{0} ", NgaySinh.Day))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            }
             , new GemboxRun(document, "tháng ")
             {
                 CharacterFormat = new CharacterFormat()
                 {
                     Size = 13
                 }
             }
            , new GemboxRun(document, string.Format("{0} ", NgaySinh.Month))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            }
             , new GemboxRun(document, "năm ")
             {
                 CharacterFormat = new CharacterFormat()
                 {
                     Size = 13
                 }
             }
            , new GemboxRun(document, string.Format("{0}", NgaySinh.Year))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            }
           , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
           , new GemboxRun(document, "Hộ khẩu thường trú: ")
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = 13
               }
           }
           //, new Run(document, string.Format("Số {0}, Phố {1}, Phường (Xã) {2}, Quận (Huyện) {3}, Thành Phố (Tỉnh) {4} ", HKTT_SoNha, HKTT_Pho, HKTT_Phuong, HKTT_Quan, HKTT_Tinh))
           , new GemboxRun(document, string.Format("{0}, {1}, {2}, {3} ", HKTT_Pho, HKTT_Phuong, HKTT_Quan, HKTT_Tinh))
           {
               CharacterFormat = new CharacterFormat()
               {
                   Bold = true,
                   Size = 13
               }
           }
             //  , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
             //, new Run(document, "Địa chỉ tạm trú: ")
             //{
             //    CharacterFormat = new CharacterFormat()
             //    {
             //        Size = 13
             //    }
             //}
             //, new Run(document, string.Format("{0}", LaNoiTru ? DiaChiCuThe + ", Ký túc xá Đại học Xây dựng" : DiaChiCuThe))
             //{
             //    CharacterFormat = new CharacterFormat()
             //    {
             //        Bold = true,
             //        Size = 13
             //    }
             //}
             , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
              , new GemboxRun(document, "Hiện đang là sinh viên của trường:   ")
              {
                  CharacterFormat = new CharacterFormat()
                  {
                      Size = 13
                  }
              }
         , new GemboxRun(document, "   MSSV:")
         {
             CharacterFormat = new CharacterFormat()
             {
                 Size = 13
             }
         }
         , new GemboxRun(document, string.Format(" {0} ", MaSV))
         {
             CharacterFormat = new CharacterFormat()
             {
                 Bold = true,
                 Size = 13
             }
         }
         , new GemboxRun(document, "   Lớp:")
         {
             CharacterFormat = new CharacterFormat()
             {
                 Size = 13
             }
         }
         , new GemboxRun(document, string.Format(" {0} ", Class))
         {
             CharacterFormat = new CharacterFormat()
             {
                 Bold = true,
                 Size = 13
             }
         }
          , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
           , new GemboxRun(document, "Khoa: ")
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = 13
               }
           }
         , new GemboxRun(document, string.Format(" {0} ", TenKhoa))
         {
             CharacterFormat = new CharacterFormat()
             {
                 Bold = true,
                 Size = 13
             }
         }
          , new GemboxRun(document, " Khóa học: ")
          {
              CharacterFormat = new CharacterFormat()
              {
                  Size = 13
              }
          }
         , new GemboxRun(document, string.Format(" {0} ", NienKhoa))
         {
             CharacterFormat = new CharacterFormat()
             {
                 Bold = true,
                 Size = 13
             }
         }
         , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
          , new GemboxRun(document, "Hệ đào tạo: ")
          {
              CharacterFormat = new CharacterFormat()
              {
                  Size = 13
              }
          }
         , new GemboxRun(document, string.Format(" {0} ", HeDaoTao))
         {
             CharacterFormat = new CharacterFormat()
             {
                 Bold = true,
                 Size = 13
             }
         }
          , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
             , new GemboxRun(document, "Lý do xác nhận: ")
             {
                 CharacterFormat = new CharacterFormat()
                 {
                     Size = 13
                 }
             }
         , new GemboxRun(document, string.Format(" {0} ", LyDoXacNhan.Replace("\n", "").Replace("\r", "")))
         {
             CharacterFormat = new CharacterFormat()
             {
                 Bold = true,
                 Size = 13
             }
         }
           , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
             , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
         )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Left,
                    LineSpacing = 1.5
                }
            };
            section.Blocks.Add(paragraphNoiDung);
            #endregion
            #region Chu ky
            GemboxTable tableCK = new GemboxTable(document);
            tableCK.TableFormat.PreferredWidth = new GemboxTableWidth(100, TableWidthUnit.Percentage);
            tableCK.TableFormat.Alignment = HorizontalAlignment.Center;
            tableCK.TableFormat.AutomaticallyResizeToFitContents = false;
            var tableBordersCK = tableCK.TableFormat.Borders;
            tableBordersCK.SetBorders(MultipleBorderTypes.All, BorderStyle.None, GemboxColor.Empty, 0);
            tableCK.Columns.Add(new TableColumn(45));
            tableCK.Columns.Add(new TableColumn(55));
            GemboxTableRow rowT1CK = new GemboxTableRow(document);
            tableCK.Rows.Add(rowT1CK);

            rowT1CK.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", " ")))));

            rowT1CK.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                new GemboxRun(document, string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true,
                        Size = 13
                    }
                }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new GemboxRun(document, desChucDanhNguoiKy)
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new GemboxRun(document, desTenNguoiKy)
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            }
           )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = HorizontalAlignment.Center
                }
            })
            {
                CellFormat = new TableCellFormat()
                {
                    VerticalAlignment = VerticalAlignment.Center
                }
            }
           );
            section.Blocks.Add(tableCK);
            #endregion
            document.Sections.Add(section);
            document.Content.Replace(desChucDanhNguoiKy, paramSet["ChucDanhNguoiKy"]);
            document.Content.Replace(desTenNguoiKy, paramSet["TenNguoiKy"]);

            string filePath = filePath = _pathProvider.MapPath($"Templates/Ctsv/xacnhan_{studentInfo.Student.Code}_{DateTime.Now.ToFileTime()}.docx");
            return new ExportFileOutputModel { document = document, filePath = filePath };
        }

        private async Task<ExportFileOutputModel> ExportWordUuDai(int id)
        {
            var uuDai = await _uuDaiRepository.FindByIdAsync(id);
            if (uuDai == null)
            {
                throw new Exception("Yêu cầu không tồn tại");
            }
            var studentInfo = await _studentRepository.GetStudentDichVuInfoAsync(uuDai.StudentCode);
            if (studentInfo == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }

            var paramSet = _thamSoDichVuService.GetParameters(DichVu.UuDaiGiaoDuc)
                                .ToDictionary(x => x.Name, x => x.Value);

            string ChucDanhNguoiKy = paramSet["ChucDanhNguoiKy"];
            string TenNguoiKy = paramSet["TenNguoiKy"];

            string desChucDanhNguoiKy = ChucDanhNguoiKy.Replace("\r", "_").Replace("\n", "_");
            string desTenNguoiKy = TenNguoiKy.Replace("\r", "_").Replace("\n", "_");
            DateTime NgaySinh;
            string tmpNgaySinh = studentInfo.Student.DateOfBirth ?? "1/1/1980";
            try
            {
                NgaySinh = DateTime.ParseExact(tmpNgaySinh, "dd/MM/yy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                NgaySinh = DateTime.Parse(tmpNgaySinh);
            }

            string HKTT_SoNha = studentInfo.Student.HkttSoNha ?? "";
            string HKTT_Pho = studentInfo.Student.HkttPho ?? "";
            string HKTT_Phuong = studentInfo.Student.HkttPhuong ?? "";
            string HKTT_Quan = studentInfo.Student.HkttQuan ?? "";
            string HKTT_Tinh = studentInfo.Student.HkttTinh ?? "";
            string MaSV = studentInfo.Student.Code.ToString();
            string HoVaTen = studentInfo.Student.FulName;
            string Class = studentInfo.Student.ClassCode ?? "";
            string TenKhoa = studentInfo.Faculty?.Name ?? "";
            string NienKhoa = studentInfo.AcademyClass.SchoolYear;
            string DiaChiCuThe = studentInfo.Student.DiaChiCuThe ?? "";

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            DocumentModel document = new DocumentModel();
            document.DefaultCharacterFormat.Size = 12;
            document.DefaultCharacterFormat.FontName = "Times New Roman";
            Section section;
            section = new Section(document);

            int commonFontSize = 14;
            #region Mẫu số
            GemboxParagraph paragraphMauSo = new GemboxParagraph(
                document,
                new GemboxRun(document, "Mẫu số 02/ƯĐGD")
                {
                    CharacterFormat = new CharacterFormat { Bold = true, Size = 13 }
                })
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = HorizontalAlignment.Right,
                    LineSpacing = 1.25
                }
            };
            section.Blocks.Add(paragraphMauSo);
            #endregion

            #region TieuDe
            GemboxParagraph paragraphTieuDe = new GemboxParagraph(document,
             new GemboxRun(document, string.Format("{0}", "GIẤY XÁC NHẬN"))
             {
                 CharacterFormat = new CharacterFormat()
                 {
                     Bold = true,
                     Size = 13
                 }
             },
             new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new GemboxRun(document, string.Format("{0}", "(Ban hành kèm theo Thông tư số 36/2015/TT-BLĐTBXH ngày 28 tháng 9 năm 2015 của Bộ Lao động-Thương binh và Xã hội)"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Italic = true,
                    Size = 13
                }
            },
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new GemboxRun(document, string.Format("{0}", "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            },
             new SpecialCharacter(document, SpecialCharacterType.LineBreak),
             new GemboxRun(document, string.Format("{0}", "Độc lập - Tự do - Hạnh phúc"))
             {
                 CharacterFormat = new CharacterFormat()
                 {
                     Bold = true,
                     Size = 13
                 }
             })
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Center,
                    LineSpacing = 1.25
                }
            };
            var horizontalLine1 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                 new HorizontalPosition(5.2, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                new VerticalPosition(7.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                new Size(155, 0)));
            horizontalLine1.Outline.Width = 1;
            horizontalLine1.Outline.Fill.SetSolid(GemboxColor.Black);
            paragraphTieuDe.Inlines.Add(horizontalLine1);
            section.Blocks.Add(paragraphTieuDe);
            #endregion
            #region Giay xac nhan
            GemboxParagraph paragraphXacNhan = new GemboxParagraph(document,
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, string.Format("{0}", "GIẤY XÁC NHẬN"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 14,
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new GemboxRun(document, "(Dùng cho các cơ sở giáo dục nghề nghiệp, giáo dục đại học xác nhận)")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 14
                    }
                }
                 , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
              )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Center,
                    LineSpacing = 1.25
                }
            };
            section.Blocks.Add(paragraphXacNhan);
            #endregion
            #region NoiDung
            GemboxParagraph paragraphNoiDung = new GemboxParagraph(document,
           // new SpecialCharacter(document, SpecialCharacterType.LineBreak),
           new GemboxRun(document, string.Format("{0}", "Trường: "))
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = commonFontSize
               }
           }
           , new SpecialCharacter(document, SpecialCharacterType.Tab)
           , new GemboxRun(document, string.Format("{0}", "ĐẠI HỌC XÂY DỰNG"))
           {
               CharacterFormat = new CharacterFormat()
               {
                   Bold = true,
                   Size = commonFontSize
               }
           }
           , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
           , new GemboxRun(document, "Xác nhận anh/chị: ")
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = commonFontSize
               }
           }
            , new GemboxRun(document, string.Format("{0} ", HoVaTen))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Size = commonFontSize,
                    Bold = true,
                }
            }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new GemboxRun(document, "Hiện là sinh viên năm thứ: ")
            {
                CharacterFormat = new CharacterFormat()
                {
                    Size = commonFontSize
                }
            }
                , new GemboxRun(document, string.Format("{0} ", getNamThu(NienKhoa)))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize,
                        Bold = true,
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.Tab)
                , new GemboxRun(document, "Học kỳ: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                }
                , new GemboxRun(document, string.Format("{0} ", "1"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize,
                        Bold = true,
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.Tab)
                 , new GemboxRun(document, "Năm học: ")
                 {
                     CharacterFormat = new CharacterFormat()
                     {
                         Size = commonFontSize
                     }
                 }
                , new GemboxRun(document, string.Format("{0} - {1}", DateTime.Now.Year, DateTime.Now.Year + 1))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize,
                        Bold = true,
                    }
                }
            )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Left,
                    LineSpacing = 1.25,
                    SpaceAfter = 0
                }
            };
            section.Blocks.Add(paragraphNoiDung);
            #endregion
            #region Hien la sinh vien
            GemboxTable tableNoiDung = new GemboxTable(document);
            tableNoiDung.TableFormat.PreferredWidth = new GemboxTableWidth(100, TableWidthUnit.Percentage);
            tableNoiDung.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            tableNoiDung.TableFormat.AutomaticallyResizeToFitContents = false;
            var tableBordersNoiDung = tableNoiDung.TableFormat.Borders;
            tableBordersNoiDung.SetBorders(MultipleBorderTypes.All, BorderStyle.None, GemboxColor.Empty, 0);
            tableNoiDung.Columns.Add(new TableColumn(55));
            tableNoiDung.Columns.Add(new TableColumn(45));
            GemboxTableRow rowT1NoiDung = new GemboxTableRow(document);
            tableNoiDung.Rows.Add(rowT1NoiDung);

            rowT1NoiDung.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                new GemboxRun(document, "Mã số sinh viên: ") { CharacterFormat = new CharacterFormat { Size = commonFontSize } },
                new GemboxRun(document, $"{MaSV}   ")
                {
                    CharacterFormat = new CharacterFormat { Size = commonFontSize, Bold = true }
                },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "Khoa: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                }
                , new GemboxRun(document, string.Format("{0}", TenKhoa))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = commonFontSize
                    }
                },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "Hình thức đào tạo: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                }
                , new GemboxRun(document, "Chính quy")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = commonFontSize
                    }
                }
            )
            {
                ParagraphFormat = new ParagraphFormat
                {
                    LineSpacing = 1.25,
                }
            }));

            rowT1NoiDung.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                new GemboxRun(document, "Lớp: ") { CharacterFormat = new CharacterFormat { Size = commonFontSize } },
                new GemboxRun(document, Class)
                {
                    CharacterFormat = new CharacterFormat { Size = commonFontSize, Bold = true }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "Khoá học: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                },
                new GemboxRun(document, NienKhoa)
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = commonFontSize
                    }
                },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "Thời gian khoá học: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                },
                new GemboxRun(document, getThoiGianKhoaHoc(NienKhoa))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = commonFontSize
                    }
                },
                new GemboxRun(document, " (năm);")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                }
            )
            {
                ParagraphFormat = new ParagraphFormat
                {
                    LineSpacing = 1.25,
                }
            }));

            section.Blocks.Add(tableNoiDung);
            #endregion
            #region Đề nghị
            GemboxParagraph paragraphDeNghi = new GemboxParagraph(
                document,
                new GemboxRun(document, "Đề nghị Phòng Lao động-Thương binh và Xã hội xem xét, giải quyết chế độ ưu đãi trong giáo dục đào tạo cho anh/chị")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    },
                },
                new GemboxRun(document, string.Format(" {0} ", HoVaTen)) { CharacterFormat = new CharacterFormat() { Size = commonFontSize, Bold = true } },
                new GemboxRun(document, "theo quy định và chế độ hiện hành.") { CharacterFormat = new CharacterFormat() { Size = commonFontSize } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak))
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = HorizontalAlignment.Left,
                    LineSpacing = 1.25
                }
            };
            section.Blocks.Add(paragraphDeNghi);
            #endregion
            #region Chu ky
            GemboxTable tableCK = new GemboxTable(document);
            tableCK.TableFormat.PreferredWidth = new GemboxTableWidth(100, TableWidthUnit.Percentage);
            tableCK.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            tableCK.TableFormat.AutomaticallyResizeToFitContents = false;
            var tableBordersCK = tableCK.TableFormat.Borders;
            tableBordersCK.SetBorders(MultipleBorderTypes.All, BorderStyle.None, GemboxColor.Empty, 0);
            tableCK.Columns.Add(new TableColumn(45));
            tableCK.Columns.Add(new TableColumn(55));
            GemboxTableRow rowT1CK = new GemboxTableRow(document);
            tableCK.Rows.Add(rowT1CK);

            rowT1CK.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", " ")))));

            rowT1CK.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                new GemboxRun(document, string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true,
                        Size = 13
                    }
                }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new GemboxRun(document, desChucDanhNguoiKy)
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 12
                }
            }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new GemboxRun(document, desTenNguoiKy)
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            }
           )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Center
                }
            })
            {
                CellFormat = new TableCellFormat()
                {
                    VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                }
            }
           );
            section.Blocks.Add(tableCK);
            #endregion
            document.Sections.Add(section);
            document.Content.Replace(desChucDanhNguoiKy, ChucDanhNguoiKy);
            document.Content.Replace(desTenNguoiKy, TenNguoiKy);

            string filePath = _pathProvider.MapPath($"Templates/Ctsv/uudai_{studentInfo.Student.Code}_{DateTime.Now.ToFileTime()}.docx");
            return new ExportFileOutputModel { document = document, filePath = filePath };
        }

        private async Task<ExportFileOutputModel> ExportWordVayVon(int id)
        {
            var vayVon = await _vayVonRepository.FindByIdAsync(id);
            if (vayVon == null)
            {
                throw new Exception("Yêu cầu không tồn tại");
            }
            var studentInfo = await _studentRepository.GetStudentDichVuInfoAsync(vayVon.StudentCode);
            if (studentInfo == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }

            var paramSet = _thamSoDichVuService.GetParameters(DichVu.VayVonNganHang)
                                .ToDictionary(x => x.Name, x => x.Value);

            string ChucDanhNguoiKy = paramSet["ChucDanhNguoiKy"];
            string TenNguoiKy = paramSet["TenNguoiKy"];
            string desChucDanhNguoiKy = ChucDanhNguoiKy?.Replace("\r", "_").Replace("\n", "_");
            string desTenNguoiKy = TenNguoiKy?.Replace("\r", "_").Replace("\n", "_");

            string hocPhi = paramSet["HocPhiThang"]?.Replace("\r", "_").Replace("\n", "_");
            string stkNhaTruong = paramSet["TaiKhoanTruong"]?.Replace("\r", "_").Replace("\n", "_");
            string nganHangNhaTruong = paramSet["NganHangTruong"]?.Replace("\r", "_").Replace("\n", "_");

            DateTime NgaySinh;
            string tmpNgaySinh = studentInfo.Student.DateOfBirth ?? "1/1/1980";
            try
            {
                NgaySinh = DateTime.ParseExact(tmpNgaySinh, "dd/MM/yy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                NgaySinh = DateTime.Parse(tmpNgaySinh);
            }
            DateTime? CmtNgayCap = studentInfo.Student.CmtNgayCap;

            string MaSV = studentInfo.Student.Code.ToString();
            string HoVaTen = studentInfo.Student.FulName;
            string Class = studentInfo.Student.ClassCode ?? "";
            string TenKhoa = studentInfo.Faculty?.Name ?? "";
            string NienKhoa = studentInfo.AcademyClass.SchoolYear;
            string LoaiDaoTao = "Chính quy";
            string cmt = studentInfo.Student.Cmt ?? "";
            string cmtNoiCap = studentInfo.Student.CmtNoiCap ?? "";
            string cmtNgayCap = CmtNgayCap == null ? "" : CmtNgayCap?.ToString("dd/MM/yyyy");
            string nghanhHoc = studentInfo.Academics?.Name ?? "";
            string thuocDien = vayVon.ThuocDien ?? "";
            string thuocDoiTuong = vayVon.ThuocDoiTuong ?? "";
            string gioiTinh = studentInfo.Student.GioiTinh;
            gioiTinh = getGender(gioiTinh);
            string matruong = "XD1";
            string tenTruong = "TRƯỜNG ĐẠI HỌC XÂY DỰNG";
            string heDaoTao = "ĐẠI HỌC";
            string khoa = getKhoa(Class);
            string namNhapHoc = getNamNhapHoc(NienKhoa);
            string namRaTruong = getNamRaTruong(NienKhoa);

            int soThangHoc = 60;
            string lopLienThong = "LT";
            if (Class.Trim().StartsWith(lopLienThong))
            {
                soThangHoc = 36;
            }

            var Dien = new Dictionary<string, string>
                {
                    {"1", "☐" },
                    {"2", "☐" },
                    {"3", "☐" },
                };
            Dien[thuocDien] = "☒";

            var DoiTuong = new Dictionary<string, string>
                {
                    {"1", "☐" },
                    {"2", "☐" },
                    {"3", "☐" },
                };
            DoiTuong[thuocDoiTuong] = "☒";

            var GioiTinh = new Dictionary<string, string>
                {
                    {"Nam", "☐" },
                    {"Nữ", "☐" },
                };
            GioiTinh[gioiTinh] = "☒";

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            DocumentModel document = new DocumentModel();
            document.DefaultCharacterFormat.Size = 12;
            document.DefaultCharacterFormat.FontName = "Times New Roman";

            Section section;
            section = new Section(document);
            var pageSetup = section.PageSetup;
            #region Cộng hòa xã hội chủ nghĩa việt nam
            GemboxTable table = new GemboxTable(document);
            table.TableFormat.PreferredWidth = new GemboxTableWidth(100, TableWidthUnit.Percentage);
            table.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            var tableBorders = table.TableFormat.Borders;
            tableBorders.SetBorders(MultipleBorderTypes.All, BorderStyle.None, GemboxColor.Empty, 0);
            table.Columns.Add(new TableColumn() { PreferredWidth = 35 });
            table.Columns.Add(new TableColumn() { PreferredWidth = 65 });
            GemboxTableRow rowT1 = new GemboxTableRow(document);
            table.Rows.Add(rowT1);

            rowT1.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", "BỘ GIÁO DỤC VÀ ĐÀO TẠO"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Size = 12
                }
            }
            )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Center
                }
            })
            {
                CellFormat = new TableCellFormat()
                {
                    VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                }
            }
            );
            rowT1.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 12
                }
            }
           )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Center
                }
            })
            {
                CellFormat = new TableCellFormat()
                {
                    VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                }
            }
           );
            GemboxTableRow rowT2 = new GemboxTableRow(document);
            table.Rows.Add(rowT2);

            rowT2.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", "TRƯỜNG ĐẠI HỌC XÂY DỰNG"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 12
                }
            }
            )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Center
                }
            })
            {
                CellFormat = new TableCellFormat()
                {
                    VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                }
            }
            );
            rowT2.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", "Độc lập – Tự do – Hạnh phúc"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 12
                }
            }
           )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Center
                }
            })
            {
                CellFormat = new TableCellFormat()
                {
                    VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                }
            }
           );
            var paragraph = new GemboxParagraph(document);

            var horizontalLine1 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                 new HorizontalPosition(1.45, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                new Size(80, 0)));
            horizontalLine1.Outline.Width = 1;
            horizontalLine1.Outline.Fill.SetSolid(GemboxColor.Black);
            paragraph.Inlines.Add(horizontalLine1);

            var horizontalLine2 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                new HorizontalPosition(8.78, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
               new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
               new Size(151, 0)));
            horizontalLine2.Outline.Width = 1;
            horizontalLine2.Outline.Fill.SetSolid(GemboxColor.Black);
            paragraph.Inlines.Add(horizontalLine2);

            section.Blocks.Add(table);
            section.Blocks.Add(paragraph);
            #endregion
            #region TieuDe
            GemboxParagraph paragraphTieuDe = new GemboxParagraph(document,
            //new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new GemboxRun(document, string.Format("{0}", "GIẤY XÁC NHẬN"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 18
                }
            }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
          )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Center,
                    LineSpacing = 1.15
                }
            };
            section.Blocks.Add(paragraphTieuDe);
            #endregion
            #region NoiDung
            GemboxParagraph paragraphNoiDung = new GemboxParagraph(document,
                new GemboxRun(document, "    "),
                new GemboxRun(document, "Họ và tên sinh viên: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, HoVaTen) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, "Ngày sinh: ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                new GemboxRun(document, $"{NgaySinh.ToString("dd/MM/yyyy")}  ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new GemboxRun(document, "Giới tính:    ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                new GemboxRun(document, "Nam: ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                new InlineContentControl(document, ContentControlType.CheckBox,
                    new GemboxRun(document, GioiTinh["Nam"]) { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                new GemboxRun(document, "   Nữ: ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                new InlineContentControl(document, ContentControlType.CheckBox,
                    new GemboxRun(document, GioiTinh["Nữ"]) { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                //new Field(document, FieldType.FormCheckBox) { FormData = { Name = "Nữ" } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, "CMND số: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{cmt} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new GemboxRun(document, "ngày cấp: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{cmtNgayCap} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new GemboxRun(document, "Nơi cấp: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{cmtNoiCap}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, "Mã trường theo học (mã quy ước trong  tuyển sinh ĐH): ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, matruong) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, "Tên trường: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, tenTruong) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, "Ngành học: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, nghanhHoc) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, "Hệ đào tạo (Đại học, cao đẳng, dạy nghề): ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, heDaoTao) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, "Khoá: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{khoa}  ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new GemboxRun(document, "Loại hình đào tạo: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{LoaiDaoTao}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, "Lớp: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{Class}  ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new GemboxRun(document, "Mã số SV: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{MaSV}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, "Khoa/Ban: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{TenKhoa}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, "Ngày nhập học:...../...../") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{namNhapHoc} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new GemboxRun(document, "Thời gian ra trường (tháng/năm):...../...../") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{namRaTruong}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, $"(Thời gian học tại trường: {soThangHoc} tháng)") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, "- Số tiền học phí hàng tháng: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{hocPhi}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new GemboxRun(document, " đồng") { CharacterFormat = new CharacterFormat { Size = 13 } }
            )
            { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25, SpaceAfter = 0 } };
            section.Blocks.Add(paragraphNoiDung);
            #endregion
            #region Thuoc Dien/Doi Tuong
            GemboxTable tableDienDoiTuong = new GemboxTable(document);
            tableDienDoiTuong.TableFormat.PreferredWidth = new GemboxTableWidth(100, TableWidthUnit.Percentage);
            tableDienDoiTuong.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            tableDienDoiTuong.TableFormat.AutomaticallyResizeToFitContents = false;
            var tblDienDoiTuongBorder = tableDienDoiTuong.TableFormat.Borders;
            tblDienDoiTuongBorder.SetBorders(MultipleBorderTypes.All, BorderStyle.None, GemboxColor.Empty, 0);
            tableDienDoiTuong.Columns.Add(new TableColumn(30));
            tableDienDoiTuong.Columns.Add(new TableColumn(40));
            tableDienDoiTuong.Columns.Add(new TableColumn(30));
            GemboxTableRow rowDien = new GemboxTableRow(document);
            tableDienDoiTuong.Rows.Add(rowDien);
            GemboxTableRow rowDoiTuong = new GemboxTableRow(document);
            tableDienDoiTuong.Rows.Add(rowDoiTuong);
            rowDien.Cells.Add(
                new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                    new GemboxRun(document, "    "),
                    new GemboxRun(document, "Thuộc diện:") { CharacterFormat = new CharacterFormat { Size = 13 } }
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            rowDoiTuong.Cells.Add(
                new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                    new GemboxRun(document, "    "),
                    new GemboxRun(document, "Thuộc đối tượng:") { CharacterFormat = new CharacterFormat { Size = 13 } }
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            rowDien.Cells.Add(
                new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new GemboxRun(document, "- Không miễn giảm") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new GemboxRun(document, "- Giảm học phí") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new GemboxRun(document, "- Miễn học phí") { CharacterFormat = new CharacterFormat { Size = 13 } }
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            rowDien.Cells.Add(
                new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new InlineContentControl(document, ContentControlType.CheckBox,
                    new GemboxRun(document, $"{Dien["1"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new InlineContentControl(document, ContentControlType.CheckBox,
                    new GemboxRun(document, $"{Dien["2"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new InlineContentControl(document, ContentControlType.CheckBox,
                    new GemboxRun(document, $"{Dien["3"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } })
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            rowDoiTuong.Cells.Add(
                new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new GemboxRun(document, "- Mồ côi") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new GemboxRun(document, "- Không mồ côi") { CharacterFormat = new CharacterFormat { Size = 13 } }
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            rowDoiTuong.Cells.Add(
                new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new InlineContentControl(document, ContentControlType.CheckBox,
                    new GemboxRun(document, $"{DoiTuong["1"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new InlineContentControl(document, ContentControlType.CheckBox,
                    new GemboxRun(document, $"{DoiTuong["2"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } })
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            section.Blocks.Add(tableDienDoiTuong);
            #endregion
            #region Ket
            GemboxParagraph paragraphKet = new GemboxParagraph(document,
                new GemboxRun(document, "    "),
                new GemboxRun(document, "- Trong thời gian theo học tại trường, anh (chị) ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new GemboxRun(document, $"{HoVaTen} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new GemboxRun(document, "không bị xử phạt hành chính trở lên về các hành vi: cờ bạc, nghiện hút, trộm cắp, buôn lậu.") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "    "),
                new GemboxRun(document, $"- Số tài khoản của nhà trường: {stkNhaTruong}, tại ngân hàng{nganHangNhaTruong}") { CharacterFormat = new CharacterFormat { Size = 13 } }
            )
            { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } };
            section.Blocks.Add(paragraphKet);
            #endregion
            #region Chu ky
            GemboxTable tableCK = new GemboxTable(document);
            tableCK.TableFormat.PreferredWidth = new GemboxTableWidth(100, TableWidthUnit.Percentage);
            tableCK.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            tableCK.TableFormat.AutomaticallyResizeToFitContents = false;
            var tableBordersCK = tableCK.TableFormat.Borders;
            tableBordersCK.SetBorders(MultipleBorderTypes.All, BorderStyle.None, GemboxColor.Empty, 0);
            tableCK.Columns.Add(new TableColumn(45));
            tableCK.Columns.Add(new TableColumn(55));
            GemboxTableRow rowT1CK = new GemboxTableRow(document);
            tableCK.Rows.Add(rowT1CK);

            rowT1CK.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", " ")))));

            rowT1CK.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                new GemboxRun(document, string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true,
                        Size = 13
                    }
                }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new GemboxRun(document, desChucDanhNguoiKy)
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new GemboxRun(document, desTenNguoiKy) { CharacterFormat = new CharacterFormat { Bold = true } }
           )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Center
                }
            })
            {
                CellFormat = new TableCellFormat()
                {
                    VerticalAlignment = GemBox.Document.VerticalAlignment.Center
                }
            }
           );
            section.Blocks.Add(tableCK);
            #endregion
            document.Sections.Add(section);
            document.Content.Replace(desChucDanhNguoiKy, ChucDanhNguoiKy);
            document.Content.Replace(desTenNguoiKy, TenNguoiKy);

            string filePath = _pathProvider.MapPath($"Templates/Ctsv/vayvon_{studentInfo.Student.Code}_{DateTime.Now.ToFileTime()}.docx");
            return new ExportFileOutputModel { document = document, filePath = filePath };
        }

        private async Task<ExportFileOutputModel> ExportWordThueNha(int id)
        {
            var thueNha = await _thueNhaRepository.FindByIdAsync(id);
            if (thueNha == null)
            {
                throw new Exception("Yêu cầu không tồn tại");
            }
            var studentInfo = await _studentRepository.GetStudentDichVuInfoAsync(thueNha.StudentCode);
            if (studentInfo == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }
            var paramSet = _thamSoDichVuService.GetParameters(DichVu.ThueNha)
                                .ToDictionary(x => x.Name, x => x.Value);

            string ChucDanhNguoiKy = paramSet["ChucDanhNguoiKy"];
            string TenNguoiKy = paramSet["TenNguoiKy"];

            string desChucDanhNguoiKy = ChucDanhNguoiKy.Replace("\r", "_").Replace("\n", "_");
            string desTenNguoiKy = TenNguoiKy.Replace("\r", "_").Replace("\n", "_");

            DateTime? CmtNgayCap = studentInfo.Student.CmtNgayCap;

            string MaSV = studentInfo.Student.Code.ToString();
            string HoVaTen = studentInfo.Student.FulName;
            string Class = studentInfo.Student.ClassCode ?? "";
            string TenKhoa = studentInfo.Faculty?.Name ?? "";
            string NienKhoa = studentInfo.AcademyClass.SchoolYear;
            string doiTuongUuTien = studentInfo.Student.DoiTuongUuTien ?? "";
            string cmt = studentInfo.Student.Cmt ?? "";
            string cmtNoiCap = studentInfo.Student.CmtNoiCap ?? "";
            string cmtNgayCap = CmtNgayCap == null ? "" : CmtNgayCap?.ToString("dd/MM/yyyy");
            string gioiTinh = studentInfo.Student.GioiTinh;
            gioiTinh = getGender(gioiTinh);
            string HKTT_Phuong = studentInfo.Student.HkttPhuong ?? "";
            string HKTT_Quan = studentInfo.Student.HkttQuan ?? "";
            string HKTT_Tinh = studentInfo.Student.HkttTinh ?? "";


            //ComponentInfo.SetLicense("DTZX-HTZ5-B7Q6-2GA6");
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            DocumentModel document = new DocumentModel();
            document.DefaultCharacterFormat.Size = 13;
            document.DefaultCharacterFormat.FontName = "Times New Roman";

            Section section;
            section = new Section(document);
            var pageSetup = section.PageSetup;
            #region Cộng hòa xã hội chủ nghĩa việt nam
            var paragraph = new GemboxParagraph(document,
                new GemboxRun(document, "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "Độc lập - Tự do - Hạnh phúc") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } }
            )
            { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Center } };


            var horizontalLine1 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                 new HorizontalPosition(5.2, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                new VerticalPosition(3.8, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                new Size(160, 0)));
            horizontalLine1.Outline.Width = 1;
            horizontalLine1.Outline.Fill.SetSolid(GemboxColor.Black);
            paragraph.Inlines.Add(horizontalLine1);

            section.Blocks.Add(paragraph);
            #endregion
            #region TieuDe
            GemboxParagraph paragraphTieuDe = new GemboxParagraph(document,
            //new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new GemboxRun(document, string.Format("{0}", "ĐƠN ĐỀ NGHỊ THUÊ NHÀ Ở SINH VIÊN"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 14
                }
            }
          )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Center,
                    LineSpacing = 1.15
                }
            };
            section.Blocks.Add(paragraphTieuDe);
            #endregion
            #region NoiDung
            GemboxParagraph paragraphNoiDung = new GemboxParagraph(document,
                new GemboxRun(document, "     "),
                new GemboxRun(document, "Kính gửi: Ban Quản lý vận hành Khu nhà ở sinh viên Pháp Vân – Tứ Hiệp."),
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "     "),
                new GemboxRun(document, "Tên tôi là: "),
                new GemboxRun(document, HoVaTen) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new GemboxRun(document, "(Nam/Nữ):  "),
                new GemboxRun(document, gioiTinh) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "     "),
                new GemboxRun(document, "CMTND số: "),
                new GemboxRun(document, cmt) { CharacterFormat = new CharacterFormat { Bold = true } },
                new GemboxRun(document, "  cấp ngày: "),
                new GemboxRun(document, cmtNgayCap) { CharacterFormat = new CharacterFormat { Bold = true } },
                new GemboxRun(document, "  nơi cấp: "),
                new GemboxRun(document, cmtNoiCap) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "     "),
                new GemboxRun(document, "Hộ khẩu thường trú: "),
                new GemboxRun(document, $"{HKTT_Phuong}, {HKTT_Quan}, {HKTT_Tinh}") { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "     "),
                new GemboxRun(document, "Sinh viên, học sinh năm thứ: "),
                new GemboxRun(document, $"{getNamThu(NienKhoa)}") { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new GemboxRun(document, "Lớp: "),
                new GemboxRun(document, Class) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new GemboxRun(document, "Khoá: "),
                new GemboxRun(document, NienKhoa) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "     "),
                new GemboxRun(document, "Ngành (khoa): "),
                new GemboxRun(document, TenKhoa) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new GemboxRun(document, "Trường: "),
                new GemboxRun(document, "Đại học Xây dựng") { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "     "),
                new GemboxRun(document, "Số thẻ sinh viên, học viên (nếu có): "),
                new GemboxRun(document, MaSV) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "     "),
                new GemboxRun(document, "Đối tượng ưu tiên (nếu có): "),
                new GemboxRun(document, doiTuongUuTien) { CharacterFormat = new CharacterFormat { Bold = true } }
            )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Left,
                    LineSpacing = 1.5,
                    SpaceAfter = 0
                }
            };
            section.Blocks.Add(paragraphNoiDung);
            #endregion
            #region Tôi đã

            GemboxParagraph paragraphToiDa = new GemboxParagraph(document,
                new GemboxRun(document, "     "),
                new GemboxRun(document, "Tôi làm đơn này đề nghị: "),
                new GemboxRun(document, "BQL vận hành Khu nhà ở sinh viên Pháp Vân – Tứ Hiệp") { CharacterFormat = new CharacterFormat { Bold = true } },
                new GemboxRun(document, " xét duyệt cho tôi được thuê nhà ở tại Khu nhà ở sinh viên Pháp Vân – Tứ Hiệp."),
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "     "),
                new GemboxRun(document, "Tôi đã đọc Bản nội quy sử dụng nhà ở sinh viên và cam kết tuân thủ nội quy sử dụng nhà ở sinh viên; cam kết trả tiền thuê nhà đầy đủ, đúng thời hạn khi được thuê nhà ở."),
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, "     "),
                new GemboxRun(document, "Tôi cam đoan những lời kê khai trong đơn là đúng sự thật, tôi xin chịu trách nhiệm trước pháp luật về các nội dung kê khai."),
                new SpecialCharacter(document, SpecialCharacterType.LineBreak)
             )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Left,
                    LineSpacing = 1.25,
                    SpaceAfter = 0
                }
            };
            section.Blocks.Add(paragraphToiDa);
            #endregion
            #region Chu ky
            GemboxTable tableCK = new GemboxTable(document);
            tableCK.TableFormat.PreferredWidth = new GemboxTableWidth(100, TableWidthUnit.Percentage);
            tableCK.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            tableCK.TableFormat.AutomaticallyResizeToFitContents = false;
            var tableBordersCK = tableCK.TableFormat.Borders;
            tableBordersCK.SetBorders(MultipleBorderTypes.All, BorderStyle.None, GemboxColor.Empty, 0);
            tableCK.Columns.Add(new TableColumn(55));
            tableCK.Columns.Add(new TableColumn(45));
            GemboxTableRow rowT1CK = new GemboxTableRow(document);
            tableCK.Rows.Add(rowT1CK);

            rowT1CK.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new GemboxRun(document, desChucDanhNguoiKy)
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 13
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new GemboxRun(document, desTenNguoiKy) { CharacterFormat = new CharacterFormat { Bold = true } }
            )
            {
                ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Center }
            }));

            rowT1CK.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document,
                new GemboxRun(document, string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true,
                        Size = 13
                    }
                }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new GemboxRun(document, "Người viết đơn") { CharacterFormat = new CharacterFormat { Bold = true, Size = 13 } },
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new GemboxRun(document, "(Ký và ghi rõ họ tên)") { CharacterFormat = new CharacterFormat { Italic = true, Size = 12 } }
           )
            {
                ParagraphFormat = new ParagraphFormat()
                {
                    Alignment = GemBox.Document.HorizontalAlignment.Center
                }
            })
           //{
           //    CellFormat = new TableCellFormat()
           //    {
           //        VerticalAlignment = GemBox.Document.VerticalAlignment.Center
           //    }
           //}
           );
            section.Blocks.Add(tableCK);
            #endregion
            document.Sections.Add(section);
            document.Content.Replace(desChucDanhNguoiKy, ChucDanhNguoiKy);
            document.Content.Replace(desTenNguoiKy, TenNguoiKy);
            string filePath = _pathProvider.MapPath($"Templates/Ctsv/thuektx_{studentInfo.Student.Code}_{DateTime.Now.ToFileTime()}.docx");
            return new ExportFileOutputModel { document = document, filePath = filePath };
        }

        private async Task<ExportFileOutputModel> ExportWordVeXeBus(int id)
        {
            var veXeBus = await _veXeBusRepository.FindByIdAsync(id);
            if (veXeBus == null)
            {
                throw new Exception("Yêu cầu không tồn tại");
            }
            var studentInfo = await _studentRepository.GetStudentDichVuInfoAsync(veXeBus.StudentCode);
            if (studentInfo == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }

            var paramSet = _thamSoDichVuService.GetParameters(DichVu.VeBus)
                                .ToDictionary(x => x.Name, x => x.Value);

            string ChucDanhNguoiKy = paramSet.ContainsKey("ChucDanhNguoiKy") ? paramSet["ChucDanhNguoiKy"] : "";
            string TenNguoiKy = paramSet.ContainsKey("TenNguoiKy") ? paramSet["TenNguoiKy"] : "";

            string filePath = _pathProvider.MapPath($"Templates/Ctsv/ve_xe_bus.docx");
            string destination = _pathProvider.MapPath($"Templates/Ctsv/bus-{DateTime.Now.ToFileTime()}.docx");
            string newImgPath = _pathProvider.MapPath($"{studentInfo.Student.File1}");

            var loaiTuyen = (DichVuXeBusLoaiTuyen)veXeBus.TuyenType;
            string motTuyen = "";
            string lienTuyen = "";
            if (loaiTuyen == DichVuXeBusLoaiTuyen.MotTuyen)
            {
                motTuyen = veXeBus.TuyenCode ?? "";
            } else if (loaiTuyen == DichVuXeBusLoaiTuyen.LienTuyen)
            {
                lienTuyen = "x";
            }

            var ngaySinh = convertStudentDateOfBirth(studentInfo.Student.DateOfBirth);

            byte[] templateBytes = await File.ReadAllBytesAsync(filePath);
            using (MemoryStream templateStream = new MemoryStream())
            {
                templateStream.Write(templateBytes, 0, templateBytes.Length);
                using (WordprocessingDocument doc = WordprocessingDocument.Open(templateStream, true))
                {
                    doc.ChangeDocumentType(WordprocessingDocumentType.Document);
                    var mainPart = doc.MainDocumentPart;
                    #region handle text
                    var textList = mainPart.Document.Descendants<Text>().ToList();
                    foreach (var text in textList)
                    {
                        replaceTextTemplate(text, "<ho_ten>", studentInfo.Student.FulName.ToUpper());
                        replaceTextTemplate(text, "<ho_ten_ky>", studentInfo.Student.FulName);
                        replaceTextTemplate(text, "<sdt>", studentInfo.Student.Mobile);
                        replaceTextTemplate(text, "<nam_sinh>", ngaySinh.ToString("dd/MM/yyyy"));
                        replaceTextTemplate(text, "<ma_lop>", studentInfo.Student.ClassCode);
                        replaceTextTemplate(text, "<ten_khoa>", studentInfo.Faculty?.Name);
                        replaceTextTemplate(text, "<mot_tuyen>", motTuyen);
                        replaceTextTemplate(text, "<lien_tuyen>", lienTuyen);
                        replaceTextTemplate(text, "<noi_nhan>", veXeBus.NoiNhanThe);
                        replaceTextTemplate(text, "<chuc_danh_nguoi_ky>", ChucDanhNguoiKy);
                        replaceTextTemplate(text, "<ten_nguoi_ky>", TenNguoiKy);
                    }
                    #endregion
                    #region handle filled image in shape
                    //if (!string.IsNullOrEmpty(newImgPath))
                    //{
                    //    var docList = mainPart.Document.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.DocProperties>().ToList();
                    //    var tmp = docList.Where(p => p.Name.Value.Contains("SHAPE_FILL") && p.Parent is DocumentFormat.OpenXml.Drawing.Wordprocessing.Anchor);

                    //    foreach (var el in tmp)
                    //    {
                    //        replaceShapeFilledImageTemplate(el, mainPart, newImgPath);
                    //    }
                    //}
                    #endregion

                    mainPart.Document.Save();
                }
                await File.WriteAllBytesAsync(destination, templateStream.ToArray());
            }
            return new ExportFileOutputModel { document = null, filePath = destination };
        }
        #endregion

        #region Helper
        private async Task<byte[]> FileToByteAsync(string filePath)
        {
            byte[] byteArr = await File.ReadAllBytesAsync(filePath);
            File.Delete(filePath);
            return byteArr;
        }

        private async Task<byte[]> DocumentToByteAsync(DocumentModel document, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                string fileName = Guid.NewGuid().ToString();
                filePath = _pathProvider.MapPath($"Templates/Ctsv/{fileName}.docx");
            }
            document.Save(filePath);
            return await FileToByteAsync(filePath);
        }

        private string getGender(string gender)
        {
            if (string.IsNullOrEmpty(gender?.Trim()))
            {
                return "Nam";
            }
            if (gender.Trim() == "N")
            {
                return "Nữ";
            }
            return "";
        }

        private string getKhoa(string classCode)
        {
            return $"K{classCode.Substring(0, 2)}";
        }

        private string getNamNhapHoc(string NienKhoa)
        {
            string[] strNamhocs = NienKhoa.Split(new char[] { '-' });
            return strNamhocs[0].Trim();
        }

        private string getNamRaTruong(string NienKhoa)
        {
            string[] strNamhocs = NienKhoa.Split(new char[] { '-' });
            return strNamhocs[1].Trim();
        }

        private string getNamThu(string NienKhoa)
        {
            string[] strNamhocs = NienKhoa.Split(new char[] { '-' });
            string nambatdau = strNamhocs[0].Trim();
            int iNamBatDau = int.Parse(nambatdau);
            int iNamHienTai = DateTime.Now.Year;
            if (DateTime.Now.Month > 8)
                iNamHienTai++;
            int iReturn = iNamHienTai - iNamBatDau;
            iReturn = iReturn > 5 ? 5 : iReturn;
            return iReturn.ToString();
        }

        private string getThoiGianKhoaHoc(string NienKhoa)
        {
            string[] strNamhocs = NienKhoa.Split(new char[] { '-' });
            string nambatdau = strNamhocs[0].Trim();
            string namKetThuc = strNamhocs[1].Trim();
            int iNamBatDau = int.Parse(nambatdau);
            int iNamKetThuc = int.Parse(namKetThuc);
            return (iNamKetThuc - iNamBatDau).ToString();
        }

        private void setStyle(IXLWorksheet ws, int row, int col, string value)
        {
            initHeaderCell(ws, row, col, value);
            ws.Cell(row, col).Style.Fill.SetBackgroundColor(XLColor.Yellow);
        }

        private void setStyleUniqueCol(IXLWorksheet ws, int row, int col, string value)
        {
            initHeaderCell(ws, row, col, value);
            ws.Cell(row, col).Style.Fill.SetBackgroundColor(XLColor.White);
        }

        private void initHeaderCell(IXLWorksheet ws, int row, int col, string value)
        {
            ws.Cell(row, col).SetValue(value);
            ws.Cell(row, col).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell(row, col).Style.Font.Bold = true;
        }

        private DateTime convertStudentDateOfBirth(string dateOfBirth)
        {
            DateTime NgaySinh;
            string tmpNgaySinh = dateOfBirth ?? "1/1/1980";
            try
            {
                NgaySinh = DateTime.ParseExact(tmpNgaySinh, "dd/MM/yy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                NgaySinh = DateTime.Parse(tmpNgaySinh);
            }
            return NgaySinh;
        }

        private void replaceTextTemplate(Text model, string oldValue, string newValue)
        {
            if (!model.Text.Contains(oldValue)) return;
            if (!newValue.Contains('\r'))
            {
                model.Text = model.Text.Replace(oldValue, newValue);
            } else 
            {
                var arr = newValue.Split('\r');
                for (int i = 0; i < arr.Count(); i++)
                {
                    string replaceText = arr[i];
                    if (i == 0)
                    {
                        model.Text = model.Text.Replace(oldValue, replaceText);
                        continue;
                    }
                    model.Parent.Append(new Break());
                    model.Parent.Append(new Text(replaceText));
                }
            }
        }

        private void replaceShapeFilledImageTemplate(DocumentFormat.OpenXml.Drawing.Wordprocessing.DocProperties docPropertyElement, MainDocumentPart mainPart, string newImagePath)
        {
            var anchor = (DocumentFormat.OpenXml.Drawing.Wordprocessing.Anchor)docPropertyElement.Parent;
            var fill = anchor.Descendants<DocumentFormat.OpenXml.Drawing.BlipFill>().FirstOrDefault();
            if (fill != null)
            {
                ImagePart newImg = mainPart.AddImagePart(ImagePartType.Png);
                newImg.FeedData(File.Open(_pathProvider.MapPath(newImagePath), FileMode.Open, FileAccess.Read));
                fill.Blip.Embed = mainPart.GetIdOfPart(newImg);
            }
        }
        #endregion
    }
}
