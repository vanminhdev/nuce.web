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
using System.Net;
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
            ILogger<DichVuService> _logger
        )
        {
            this._xacNhanRepository = _xacNhanRepository;
            this._gioiThieuRepository = _gioiThieuRepository;
            this._uuDaiRepository = _uuDaiRepository;
            this._vayVonRepository = _vayVonRepository;
            this._thueNhaRepository = _thueNhaRepository;
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
                default:
                    break;
            }

            var data = result.Skip(model.Page ?? 0).Take(model.Size ?? 0);

            recordTotal = result.Count();
            recordFiltered = data.Count();

            var rs = new DataTableResponse<QuanLyDichVuDetailResponse>
            {
                RecordsTotal = recordTotal,
                RecordsFiltered = recordFiltered,
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
            };
            foreach (var dichVu in allDichVu)
            {
                if (quantityDictionary.ContainsKey(dichVu.Id))
                {
                    var info = quantityDictionary[dichVu.Id];
                    info.TenDichVu = dichVu.Description;
                    info.LinkDichVu = dichVu.Param1;
                }
            }
            return quantityDictionary;
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
                _logger.LogError("Cập nhật trạng thái yêu cầu dịch vụ", $"{ex.ToString()} \n", JsonConvert.SerializeObject(model));
                return new ResponseBody { Data = ex, Message = "Lỗi hệ thống" };
            }
            return null;
        }

        #region Export Excel
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
                    string tenKhoa = yeuCau.Faculty.Name ?? "";
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
                    string tenKhoa = yeuCau.Faculty.Name ?? "";
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
                    string tenKhoa = yeuCau.Faculty.Name ?? "";
                    string mobile = yeuCau.Student.Mobile ?? "";
                    string gioiTinh = yeuCau.Student.GioiTinh ?? "";
                    gioiTinh = getGender(gioiTinh);
                    string cmtNoiCap = yeuCau.Student.CmtNoiCap ?? "";
                    string cmt = yeuCau.Student.Cmt ?? "";
                    string soDienThoai = mobile;
                    string nganhHoc = yeuCau.Academics.Name ?? "";
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
                    string tenKhoa = yeuCau.Faculty.Name ?? "";
                    string mobile = yeuCau.Student.Mobile ?? "";
                    string gioiTinh = yeuCau.Student.GioiTinh ?? "";
                    gioiTinh = getGender(gioiTinh);
                    string cmtNoiCap = yeuCau.Student.CmtNoiCap ?? "";
                    string cmt = yeuCau.Student.Cmt ?? "";
                    string soDienThoai = mobile;
                    string nganhHoc = yeuCau.Academics.Name ?? "";
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
        #endregion

        #region Export Word (docx & zip)
        public async Task<byte[]> ExportWordListAsync(DichVu dichVu, List<DichVuExport> dichVuList)
        {
            List<string> dirs = new List<string>();
            foreach (var yeuCau in dichVuList)
            {
                var exportResult = await GetExportOutput(dichVu, yeuCau.ID);
                string dir = exportResult.filePath;
                exportResult.document.Save(dir);
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
                if (exportOutput != null)
                {
                    return await DocumentToByteAsync(exportOutput.document, exportOutput.filePath);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("hihi: ", ex.Message, " |||| \n", ex.ToString());
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
            string TenKhoa = studentInfo.Faculty.Name ?? "";
            string HeDaoTao = "Chính quy";
            string NienKhoa = studentInfo.AcademyClass.SchoolYear;
            string LyDoXacNhan = xacNhan.LyDo ?? "";

            //ComponentInfo.SetLicense("DTZX-HTZ5-B7Q6-2GA6");
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            DocumentModel document = new DocumentModel();
            document.DefaultCharacterFormat.Size = 12;
            document.DefaultCharacterFormat.FontName = "Times New Roman";

            Section section;
            section = new Section(document);
            #region Cộng hòa xã hội chủ nghĩa việt nam
            Table table = new Table(document);
            table.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
            table.TableFormat.Alignment = HorizontalAlignment.Center;
            var tableBorders = table.TableFormat.Borders;
            tableBorders.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
            table.Columns.Add(new TableColumn() { PreferredWidth = 35 });
            table.Columns.Add(new TableColumn() { PreferredWidth = 65 });
            TableRow rowT1 = new TableRow(document);
            table.Rows.Add(rowT1);

            rowT1.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "BỘ GIÁO DỤC VÀ ĐÀO TẠO"))
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
            rowT1.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM"))
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
            TableRow rowT2 = new TableRow(document);
            table.Rows.Add(rowT2);

            rowT2.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "TRƯỜNG ĐẠI HỌC XÂY DỰNG"))
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
            rowT2.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "Độc lập – Tự do – Hạnh phúc"))
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
            var paragraph = new Paragraph(document);

            var horizontalLine1 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                 new HorizontalPosition(1, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                new Size(125, 0)));
            horizontalLine1.Outline.Width = 1;
            horizontalLine1.Outline.Fill.SetSolid(Color.Black);
            paragraph.Inlines.Add(horizontalLine1);

            var horizontalLine2 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                new HorizontalPosition(8.78, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
               new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
               new Size(151, 0)));
            horizontalLine2.Outline.Width = 1;
            horizontalLine2.Outline.Fill.SetSolid(Color.Black);
            paragraph.Inlines.Add(horizontalLine2);

            section.Blocks.Add(table);
            section.Blocks.Add(paragraph);
            #endregion
            #region TieuDe
            Paragraph paragraphTieuDe = new Paragraph(document,
            //new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new Run(document, string.Format("{0}", "GIẤY XÁC NHẬN"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 16
                }
            }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new Run(document, "TRƯỜNG ĐẠI HỌC XÂY DỰNG")
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 14
                }
            }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new Run(document, "XÁC NHẬN")
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
            Paragraph paragraphNoiDung = new Paragraph(document,
           // new SpecialCharacter(document, SpecialCharacterType.LineBreak),
           new Run(document, string.Format("{0}", "Anh (chị): "))
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = 13
               }
           }
           , new Run(document, string.Format("{0}", HoVaTen))
           {
               CharacterFormat = new CharacterFormat()
               {
                   Bold = true,
                   Size = 13
               }
           }
           , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
           , new Run(document, "Sinh ngày: ")
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = 13
               }
           }
            , new Run(document, string.Format("{0} ", NgaySinh.Day))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            }
             , new Run(document, "tháng ")
             {
                 CharacterFormat = new CharacterFormat()
                 {
                     Size = 13
                 }
             }
            , new Run(document, string.Format("{0} ", NgaySinh.Month))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            }
             , new Run(document, "năm ")
             {
                 CharacterFormat = new CharacterFormat()
                 {
                     Size = 13
                 }
             }
            , new Run(document, string.Format("{0}", NgaySinh.Year))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            }
           , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
           , new Run(document, "Hộ khẩu thường trú: ")
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = 13
               }
           }
           //, new Run(document, string.Format("Số {0}, Phố {1}, Phường (Xã) {2}, Quận (Huyện) {3}, Thành Phố (Tỉnh) {4} ", HKTT_SoNha, HKTT_Pho, HKTT_Phuong, HKTT_Quan, HKTT_Tinh))
           , new Run(document, string.Format("{0}, {1}, {2}, {3} ", HKTT_Pho, HKTT_Phuong, HKTT_Quan, HKTT_Tinh))
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
              , new Run(document, "Hiện đang là sinh viên của trường:   ")
              {
                  CharacterFormat = new CharacterFormat()
                  {
                      Size = 13
                  }
              }
         , new Run(document, "   MSSV:")
         {
             CharacterFormat = new CharacterFormat()
             {
                 Size = 13
             }
         }
         , new Run(document, string.Format(" {0} ", MaSV))
         {
             CharacterFormat = new CharacterFormat()
             {
                 Bold = true,
                 Size = 13
             }
         }
         , new Run(document, "   Lớp:")
         {
             CharacterFormat = new CharacterFormat()
             {
                 Size = 13
             }
         }
         , new Run(document, string.Format(" {0} ", Class))
         {
             CharacterFormat = new CharacterFormat()
             {
                 Bold = true,
                 Size = 13
             }
         }
          , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
           , new Run(document, "Khoa: ")
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = 13
               }
           }
         , new Run(document, string.Format(" {0} ", TenKhoa))
         {
             CharacterFormat = new CharacterFormat()
             {
                 Bold = true,
                 Size = 13
             }
         }
          , new Run(document, " Khóa học: ")
          {
              CharacterFormat = new CharacterFormat()
              {
                  Size = 13
              }
          }
         , new Run(document, string.Format(" {0} ", NienKhoa))
         {
             CharacterFormat = new CharacterFormat()
             {
                 Bold = true,
                 Size = 13
             }
         }
         , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
          , new Run(document, "Hệ đào tạo: ")
          {
              CharacterFormat = new CharacterFormat()
              {
                  Size = 13
              }
          }
         , new Run(document, string.Format(" {0} ", HeDaoTao))
         {
             CharacterFormat = new CharacterFormat()
             {
                 Bold = true,
                 Size = 13
             }
         }
          , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
             , new Run(document, "Lý do xác nhận: ")
             {
                 CharacterFormat = new CharacterFormat()
                 {
                     Size = 13
                 }
             }
         , new Run(document, string.Format(" {0} ", LyDoXacNhan.Replace("\n", "").Replace("\r", "")))
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
            Table tableCK = new Table(document);
            tableCK.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
            tableCK.TableFormat.Alignment = HorizontalAlignment.Center;
            tableCK.TableFormat.AutomaticallyResizeToFitContents = false;
            var tableBordersCK = tableCK.TableFormat.Borders;
            tableBordersCK.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
            tableCK.Columns.Add(new TableColumn(45));
            tableCK.Columns.Add(new TableColumn(55));
            TableRow rowT1CK = new TableRow(document);
            tableCK.Rows.Add(rowT1CK);

            rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", " ")))));

            rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document,
                new Run(document, string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true,
                        Size = 13
                    }
                }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new Run(document, desChucDanhNguoiKy)
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
            , new Run(document, desTenNguoiKy)
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
            string TenKhoa = studentInfo.Faculty.Name ?? "";
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
            Paragraph paragraphMauSo = new Paragraph(
                document,
                new Run(document, "Mẫu số 02/ƯĐGD")
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
            Paragraph paragraphTieuDe = new Paragraph(document,
             new Run(document, string.Format("{0}", "GIẤY XÁC NHẬN"))
             {
                 CharacterFormat = new CharacterFormat()
                 {
                     Bold = true,
                     Size = 13
                 }
             },
             new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new Run(document, string.Format("{0}", "(Ban hành kèm theo Thông tư số 36/2015/TT-BLĐTBXH ngày 28 tháng 9 năm 2015 của Bộ Lao động-Thương binh và Xã hội)"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Italic = true,
                    Size = 13
                }
            },
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new Run(document, string.Format("{0}", "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 13
                }
            },
             new SpecialCharacter(document, SpecialCharacterType.LineBreak),
             new Run(document, string.Format("{0}", "Độc lập - Tự do - Hạnh phúc"))
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
            horizontalLine1.Outline.Fill.SetSolid(Color.Black);
            paragraphTieuDe.Inlines.Add(horizontalLine1);
            section.Blocks.Add(paragraphTieuDe);
            #endregion
            #region Giay xac nhan
            Paragraph paragraphXacNhan = new Paragraph(document,
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, string.Format("{0}", "GIẤY XÁC NHẬN"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = 14,
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
                , new Run(document, "(Dùng cho các cơ sở giáo dục nghề nghiệp, giáo dục đại học xác nhận)")
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
            Paragraph paragraphNoiDung = new Paragraph(document,
           // new SpecialCharacter(document, SpecialCharacterType.LineBreak),
           new Run(document, string.Format("{0}", "Trường: "))
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = commonFontSize
               }
           }
           , new SpecialCharacter(document, SpecialCharacterType.Tab)
           , new Run(document, string.Format("{0}", "ĐẠI HỌC XÂY DỰNG"))
           {
               CharacterFormat = new CharacterFormat()
               {
                   Bold = true,
                   Size = commonFontSize
               }
           }
           , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
           , new Run(document, "Xác nhận anh/chị: ")
           {
               CharacterFormat = new CharacterFormat()
               {
                   Size = commonFontSize
               }
           }
            , new Run(document, string.Format("{0} ", HoVaTen))
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
            Table tableNoiDung = new Table(document);
            tableNoiDung.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
            tableNoiDung.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            tableNoiDung.TableFormat.AutomaticallyResizeToFitContents = false;
            var tableBordersNoiDung = tableNoiDung.TableFormat.Borders;
            tableBordersNoiDung.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
            tableNoiDung.Columns.Add(new TableColumn(40));
            tableNoiDung.Columns.Add(new TableColumn(60));
            TableRow rowT1NoiDung = new TableRow(document);
            tableNoiDung.Rows.Add(rowT1NoiDung);

            rowT1NoiDung.Cells.Add(new TableCell(document, new Paragraph(document,
                new Run(document, "Hiện là sinh viên năm thứ: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                }
                , new Run(document, string.Format("{0} ", getNamThu(NienKhoa)))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize,
                        Bold = true,
                    }
                },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, "Mã số sinh viên: ") { CharacterFormat = new CharacterFormat { Size = commonFontSize } },
                new Run(document, $"{MaSV}   ")
                {
                    CharacterFormat = new CharacterFormat { Size = commonFontSize, Bold = true }
                },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, "Khoa: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                }
                , new Run(document, string.Format("{0}", TenKhoa))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = commonFontSize
                    }
                },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, "Hình thức đào tạo: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                }
                , new Run(document, "Chính quy")
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

            rowT1NoiDung.Cells.Add(new TableCell(document, new Paragraph(document,
                new Run(document, "Học kỳ: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                }
                , new Run(document, string.Format("{0} ", "1"))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize,
                        Bold = true,
                    }
                }
                , new SpecialCharacter(document, SpecialCharacterType.Tab)
                 , new Run(document, "Năm học: ")
                 {
                     CharacterFormat = new CharacterFormat()
                     {
                         Size = commonFontSize
                     }
                 }
                , new Run(document, string.Format("{0} - {1}", DateTime.Now.Year, DateTime.Now.Year + 1))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize,
                        Bold = true,
                    }
                },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, "Lớp: ") { CharacterFormat = new CharacterFormat { Size = commonFontSize } },
                new Run(document, Class)
                {
                    CharacterFormat = new CharacterFormat { Size = commonFontSize, Bold = true }
                }
                , new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, "Khoá học: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                },
                new Run(document, NienKhoa)
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = commonFontSize
                    }
                },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, "Thời gian khoá học: ")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    }
                },
                new Run(document, getThoiGianKhoaHoc(NienKhoa))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Bold = true,
                        Size = commonFontSize
                    }
                },
                new Run(document, " (năm);")
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
            Paragraph paragraphDeNghi = new Paragraph(
                document,
                new Run(document, "Đề nghị Phòng Lao động-Thương binh và Xã hội xem xét, giải quyết chế độ ưu đãi trong giáo dục đào tạo cho anh/chị")
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Size = commonFontSize
                    },
                },
                new Run(document, string.Format(" {0} ", HoVaTen)) { CharacterFormat = new CharacterFormat() { Size = commonFontSize, Bold = true } },
                new Run(document, "theo quy định và chế độ hiện hành.") { CharacterFormat = new CharacterFormat() { Size = commonFontSize } },
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
            Table tableCK = new Table(document);
            tableCK.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
            tableCK.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            tableCK.TableFormat.AutomaticallyResizeToFitContents = false;
            var tableBordersCK = tableCK.TableFormat.Borders;
            tableBordersCK.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
            tableCK.Columns.Add(new TableColumn(45));
            tableCK.Columns.Add(new TableColumn(55));
            TableRow rowT1CK = new TableRow(document);
            tableCK.Rows.Add(rowT1CK);

            rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", " ")))));

            rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document,
                new Run(document, string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true,
                        Size = 13
                    }
                }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak)
            , new Run(document, desChucDanhNguoiKy)
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
            , new Run(document, desTenNguoiKy)
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
            string TenKhoa = studentInfo.Faculty.Name ?? "";
            string NienKhoa = studentInfo.AcademyClass.SchoolYear;
            string LoaiDaoTao = "Chính quy";
            string cmt = studentInfo.Student.Cmt ?? "";
            string cmtNoiCap = studentInfo.Student.CmtNoiCap ?? "";
            string cmtNgayCap = CmtNgayCap == null ? "" : CmtNgayCap?.ToString("dd/MM/yyyy");
            string nghanhHoc = studentInfo.Academics.Name ?? "";
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
            Table table = new Table(document);
            table.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
            table.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            var tableBorders = table.TableFormat.Borders;
            tableBorders.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
            table.Columns.Add(new TableColumn() { PreferredWidth = 35 });
            table.Columns.Add(new TableColumn() { PreferredWidth = 65 });
            TableRow rowT1 = new TableRow(document);
            table.Rows.Add(rowT1);

            rowT1.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "BỘ GIÁO DỤC VÀ ĐÀO TẠO"))
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
            rowT1.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM"))
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
            TableRow rowT2 = new TableRow(document);
            table.Rows.Add(rowT2);

            rowT2.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "TRƯỜNG ĐẠI HỌC XÂY DỰNG"))
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
            rowT2.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", "Độc lập – Tự do – Hạnh phúc"))
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
            var paragraph = new Paragraph(document);

            var horizontalLine1 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                 new HorizontalPosition(1.45, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                new Size(80, 0)));
            horizontalLine1.Outline.Width = 1;
            horizontalLine1.Outline.Fill.SetSolid(Color.Black);
            paragraph.Inlines.Add(horizontalLine1);

            var horizontalLine2 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                new HorizontalPosition(8.78, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
               new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
               new Size(151, 0)));
            horizontalLine2.Outline.Width = 1;
            horizontalLine2.Outline.Fill.SetSolid(Color.Black);
            paragraph.Inlines.Add(horizontalLine2);

            section.Blocks.Add(table);
            section.Blocks.Add(paragraph);
            #endregion
            #region TieuDe
            Paragraph paragraphTieuDe = new Paragraph(document,
            //new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new Run(document, string.Format("{0}", "GIẤY XÁC NHẬN"))
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
            Paragraph paragraphNoiDung = new Paragraph(document,
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Họ và tên sinh viên: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, HoVaTen) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Ngày sinh: ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                new Run(document, $"{NgaySinh.ToString("dd/MM/yyyy")}  ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new Run(document, "Giới tính:    ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                new Run(document, "Nam: ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                new InlineContentControl(document, ContentControlType.CheckBox,
                    new Run(document, GioiTinh["Nam"]) { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                new Run(document, "   Nữ: ") { CharacterFormat = new CharacterFormat { Size = 13, } },
                new InlineContentControl(document, ContentControlType.CheckBox,
                    new Run(document, GioiTinh["Nữ"]) { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                //new Field(document, FieldType.FormCheckBox) { FormData = { Name = "Nữ" } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "CMND số: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{cmt} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new Run(document, "ngày cấp: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{cmtNgayCap} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new Run(document, "Nơi cấp: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{cmtNoiCap}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Mã trường theo học (mã quy ước trong  tuyển sinh ĐH): ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, matruong) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Tên trường: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, tenTruong) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Ngành học: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, nghanhHoc) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Hệ đào tạo (Đại học, cao đẳng, dạy nghề): ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, heDaoTao) { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Khoá: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{khoa}  ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new Run(document, "Loại hình đào tạo: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{LoaiDaoTao}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Lớp: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{Class}  ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new Run(document, "Mã số SV: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{MaSV}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Khoa/Ban: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{TenKhoa}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Ngày nhập học:...../...../") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{namNhapHoc} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new Run(document, "Thời gian ra trường (tháng/năm):...../...../") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{namRaTruong}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "- Số tiền học phí hàng tháng: ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{hocPhi}") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new Run(document, " đồng") { CharacterFormat = new CharacterFormat { Size = 13 } }
            )
            { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25, SpaceAfter = 0 } };
            section.Blocks.Add(paragraphNoiDung);
            #endregion
            #region Thuoc Dien/Doi Tuong
            Table tableDienDoiTuong = new Table(document);
            tableDienDoiTuong.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
            tableDienDoiTuong.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            tableDienDoiTuong.TableFormat.AutomaticallyResizeToFitContents = false;
            var tblDienDoiTuongBorder = tableDienDoiTuong.TableFormat.Borders;
            tblDienDoiTuongBorder.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
            tableDienDoiTuong.Columns.Add(new TableColumn(30));
            tableDienDoiTuong.Columns.Add(new TableColumn(40));
            tableDienDoiTuong.Columns.Add(new TableColumn(30));
            TableRow rowDien = new TableRow(document);
            tableDienDoiTuong.Rows.Add(rowDien);
            TableRow rowDoiTuong = new TableRow(document);
            tableDienDoiTuong.Rows.Add(rowDoiTuong);
            rowDien.Cells.Add(
                new TableCell(document, new Paragraph(document,
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Thuộc diện:") { CharacterFormat = new CharacterFormat { Size = 13 } }
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            rowDoiTuong.Cells.Add(
                new TableCell(document, new Paragraph(document,
                    new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "Thuộc đối tượng:") { CharacterFormat = new CharacterFormat { Size = 13 } }
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            rowDien.Cells.Add(
                new TableCell(document, new Paragraph(document,
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "- Không miễn giảm") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "- Giảm học phí") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "- Miễn học phí") { CharacterFormat = new CharacterFormat { Size = 13 } }
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            rowDien.Cells.Add(
                new TableCell(document, new Paragraph(document,
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new InlineContentControl(document, ContentControlType.CheckBox,
                    new Run(document, $"{Dien["1"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new InlineContentControl(document, ContentControlType.CheckBox,
                    new Run(document, $"{Dien["2"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new InlineContentControl(document, ContentControlType.CheckBox,
                    new Run(document, $"{Dien["3"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } })
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            rowDoiTuong.Cells.Add(
                new TableCell(document, new Paragraph(document,
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "- Mồ côi") { CharacterFormat = new CharacterFormat { Size = 13 } },
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new Run(document, "- Không mồ côi") { CharacterFormat = new CharacterFormat { Size = 13 } }
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            rowDoiTuong.Cells.Add(
                new TableCell(document, new Paragraph(document,
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new InlineContentControl(document, ContentControlType.CheckBox,
                    new Run(document, $"{DoiTuong["1"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } }),
                    new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                    //new SpecialCharacter(document, SpecialCharacterType.Tab),
                    new InlineContentControl(document, ContentControlType.CheckBox,
                    new Run(document, $"{DoiTuong["2"]}") { CharacterFormat = new CharacterFormat { Size = 13, FontName = document.DefaultCharacterFormat.FontName } })
                )
                { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } })
            );
            section.Blocks.Add(tableDienDoiTuong);
            #endregion
            #region Ket
            Paragraph paragraphKet = new Paragraph(document,
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "- Trong thời gian theo học tại trường, anh (chị) ") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new Run(document, $"{HoVaTen} ") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new Run(document, "không bị xử phạt hành chính trở lên về các hành vi: cờ bạc, nghiện hút, trộm cắp, buôn lậu.") { CharacterFormat = new CharacterFormat { Size = 13 } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, $"- Số tài khoản của nhà trường: {stkNhaTruong}, tại ngân hàng{nganHangNhaTruong}") { CharacterFormat = new CharacterFormat { Size = 13 } }
            )
            { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Left, LineSpacing = 1.25 } };
            section.Blocks.Add(paragraphKet);
            #endregion
            #region Chu ky
            Table tableCK = new Table(document);
            tableCK.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
            tableCK.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            tableCK.TableFormat.AutomaticallyResizeToFitContents = false;
            var tableBordersCK = tableCK.TableFormat.Borders;
            tableBordersCK.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
            tableCK.Columns.Add(new TableColumn(45));
            tableCK.Columns.Add(new TableColumn(55));
            TableRow rowT1CK = new TableRow(document);
            tableCK.Rows.Add(rowT1CK);

            rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document, new Run(document, string.Format("{0}", " ")))));

            rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document,
                new Run(document, string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true,
                        Size = 13
                    }
                }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new Run(document, desChucDanhNguoiKy)
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
            , new Run(document, desTenNguoiKy) { CharacterFormat = new CharacterFormat { Bold = true } }
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
            string TenKhoa = studentInfo.Faculty.Name ?? "";
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
            var paragraph = new Paragraph(document,
                new Run(document, "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, "Độc lập - Tự do - Hạnh phúc") { CharacterFormat = new CharacterFormat { Size = 13, Bold = true } }
            )
            { ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Center } };


            var horizontalLine1 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                 new HorizontalPosition(5.2, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                new VerticalPosition(3.8, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                new Size(160, 0)));
            horizontalLine1.Outline.Width = 1;
            horizontalLine1.Outline.Fill.SetSolid(Color.Black);
            paragraph.Inlines.Add(horizontalLine1);

            section.Blocks.Add(paragraph);
            #endregion
            #region TieuDe
            Paragraph paragraphTieuDe = new Paragraph(document,
            //new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new Run(document, string.Format("{0}", "ĐƠN ĐỀ NGHỊ THUÊ NHÀ Ở SINH VIÊN"))
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
            Paragraph paragraphNoiDung = new Paragraph(document,
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Kính gửi: Ban Quản lý vận hành Khu nhà ở sinh viên Pháp Vân – Tứ Hiệp."),
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Tên tôi là: "),
                new Run(document, HoVaTen) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "(Nam/Nữ):  "),
                new Run(document, gioiTinh) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "CMTND số: "),
                new Run(document, cmt) { CharacterFormat = new CharacterFormat { Bold = true } },
                new Run(document, "  cấp ngày: "),
                new Run(document, cmtNgayCap) { CharacterFormat = new CharacterFormat { Bold = true } },
                new Run(document, "  nơi cấp: "),
                new Run(document, cmtNoiCap) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Hộ khẩu thường trú: "),
                new Run(document, $"{HKTT_Phuong}, {HKTT_Quan}, {HKTT_Tinh}") { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Sinh viên, học sinh năm thứ: "),
                new Run(document, $"{getNamThu(NienKhoa)}") { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Lớp: "),
                new Run(document, Class) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Khoá: "),
                new Run(document, NienKhoa) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Ngành (khoa): "),
                new Run(document, TenKhoa) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Trường: "),
                new Run(document, "Đại học Xây dựng") { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Số thẻ sinh viên, học viên (nếu có): "),
                new Run(document, MaSV) { CharacterFormat = new CharacterFormat { Bold = true } },
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Đối tượng ưu tiên (nếu có): "),
                new Run(document, doiTuongUuTien) { CharacterFormat = new CharacterFormat { Bold = true } }
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

            Paragraph paragraphToiDa = new Paragraph(document,
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Tôi làm đơn này đề nghị: "),
                new Run(document, "BQL vận hành Khu nhà ở sinh viên Pháp Vân – Tứ Hiệp") { CharacterFormat = new CharacterFormat { Bold = true } },
                new Run(document, " xét duyệt cho tôi được thuê nhà ở tại Khu nhà ở sinh viên Pháp Vân – Tứ Hiệp."),
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Tôi đã đọc Bản nội quy sử dụng nhà ở sinh viên và cam kết tuân thủ nội quy sử dụng nhà ở sinh viên; cam kết trả tiền thuê nhà đầy đủ, đúng thời hạn khi được thuê nhà ở."),
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new SpecialCharacter(document, SpecialCharacterType.Tab),
                new Run(document, "Tôi cam đoan những lời kê khai trong đơn là đúng sự thật, tôi xin chịu trách nhiệm trước pháp luật về các nội dung kê khai."),
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
            Table tableCK = new Table(document);
            tableCK.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
            tableCK.TableFormat.Alignment = GemBox.Document.HorizontalAlignment.Center;
            tableCK.TableFormat.AutomaticallyResizeToFitContents = false;
            var tableBordersCK = tableCK.TableFormat.Borders;
            tableBordersCK.SetBorders(MultipleBorderTypes.All, BorderStyle.None, Color.Empty, 0);
            tableCK.Columns.Add(new TableColumn(55));
            tableCK.Columns.Add(new TableColumn(45));
            TableRow rowT1CK = new TableRow(document);
            tableCK.Rows.Add(rowT1CK);

            rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document,
                new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                new Run(document, desChucDanhNguoiKy)
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
                , new Run(document, desTenNguoiKy) { CharacterFormat = new CharacterFormat { Bold = true } }
            )
            {
                ParagraphFormat = new ParagraphFormat { Alignment = HorizontalAlignment.Center }
            }));

            rowT1CK.Cells.Add(new TableCell(document, new Paragraph(document,
                new Run(document, string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
                {
                    CharacterFormat = new CharacterFormat()
                    {
                        Italic = true,
                        Size = 13
                    }
                }
            , new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new Run(document, "Người viết đơn") { CharacterFormat = new CharacterFormat { Bold = true, Size = 13 } },
            new SpecialCharacter(document, SpecialCharacterType.LineBreak),
            new Run(document, "(Ký và ghi rõ họ tên)") { CharacterFormat = new CharacterFormat { Italic = true, Size = 12 } }
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

        #endregion
    }
}
