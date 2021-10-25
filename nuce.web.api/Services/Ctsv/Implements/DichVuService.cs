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
using System.Drawing;
using nuce.web.api.Helper;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

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
        private readonly IDangKyChoORepository _dangKyChoORepository;
        private readonly IDotDangKyChoORepository _dotDangKyChoORepository;
        private readonly IXinMienGiamHocPhiRepository _xinMienGiamHocPhiRepository;
        private readonly IDotXinMienGiamHocPhiRepository _dotXinMienGiamHocPhiRepository;
        private readonly IDeNghiHoTroChiPhiRepository _deNghiHoTroChiPhiRepository;
        private readonly IDotDeNghiHoTroChiPhiRepository _dotDeNghiHoTroChiPhiRepository;
        private readonly IHoTroHocTapRepository _hoTroHocTapRepository;
        private readonly IDotHoTroHocTapRepository _dotHoTroHocTapRepository;
        private readonly ICapLaiTheRepository _capLaiTheRepository;
        private readonly IMuonHocBaRepository _muonHocBaRepository;

        private readonly IThamSoDichVuService _thamSoDichVuService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogService _logService;
        private readonly IPathProvider _pathProvider;
        private readonly ILogger<DichVuService> _logger;
        private readonly IConfiguration _configuration;

        public DichVuService(IXacNhanRepository _xacNhanRepository, IGioiThieuRepository _gioiThieuRepository,
            IUuDaiGiaoDucRepository _uuDaiRepository, IVayVonRepository _vayVonRepository,
            IThueNhaRepository _thueNhaRepository, IUserService _userService,
            IUnitOfWork _unitOfWork, IEmailService _emailService, ILogService _logService,
            ILoaiDichVuRepository _loaiDichVuRepository, IStudentRepository _studentRepository,
            IThamSoDichVuService _thamSoDichVuService, IPathProvider _pathProvider,
            ILogger<DichVuService> _logger, IVeXeBusRepository _veXeBusRepository,
            ICapLaiTheRepository _capLaiTheRepository, IMuonHocBaRepository _muonHocBaRepository,
            IDangKyChoORepository _dangKyChoORepository, IDotDangKyChoORepository _dotDangKyChoORepository,
            IXinMienGiamHocPhiRepository _xinMienGiamHocPhiRepository, IDotXinMienGiamHocPhiRepository _dotXinMienGiamHocPhiRepository,
            IDeNghiHoTroChiPhiRepository _deNghiHoTroChiPhiRepository, IDotDeNghiHoTroChiPhiRepository _dotDeNghiHoTroChiPhiRepository,
            IHoTroHocTapRepository _hoTroHocTapRepository, IDotHoTroHocTapRepository _dotHoTroHocTapRepository,
            IConfiguration _configuration
        )
        {
            this._xacNhanRepository = _xacNhanRepository;
            this._gioiThieuRepository = _gioiThieuRepository;
            this._capLaiTheRepository = _capLaiTheRepository;
            this._uuDaiRepository = _uuDaiRepository;
            this._vayVonRepository = _vayVonRepository;
            this._thueNhaRepository = _thueNhaRepository;
            this._veXeBusRepository = _veXeBusRepository;
            this._dangKyChoORepository = _dangKyChoORepository;
            this._dotDangKyChoORepository = _dotDangKyChoORepository;
            this._xinMienGiamHocPhiRepository = _xinMienGiamHocPhiRepository;
            this._dotXinMienGiamHocPhiRepository = _dotXinMienGiamHocPhiRepository;
            this._deNghiHoTroChiPhiRepository = _deNghiHoTroChiPhiRepository;
            this._dotDeNghiHoTroChiPhiRepository = _dotDeNghiHoTroChiPhiRepository;
            this._hoTroHocTapRepository = _hoTroHocTapRepository;
            this._dotHoTroHocTapRepository = _dotHoTroHocTapRepository;

            this._loaiDichVuRepository = _loaiDichVuRepository;
            this._studentRepository = _studentRepository;
            this._muonHocBaRepository = _muonHocBaRepository;

            this._pathProvider = _pathProvider;
            this._thamSoDichVuService = _thamSoDichVuService;
            this._userService = _userService;
            this._unitOfWork = _unitOfWork;
            this._emailService = _emailService;
            this._logService = _logService;
            this._logger = _logger;
            this._configuration = _configuration;
        }
        #endregion
        /// <summary>
        /// Tạo yêu cầu dịch vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

            string templateName = null;

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
                        case DichVu.CapLaiThe:
                            if (_capLaiTheRepository.IsDuplicated(currentStudent.Id))
                            {
                                throw new DuplicateWaitObjectException();
                            }
                            AsAcademyStudentSvCapLaiTheSinhVien capLaiThe = new AsAcademyStudentSvCapLaiTheSinhVien
                            {
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
                            await _capLaiTheRepository.AddAsync(capLaiThe);
                            templateName = "template_mail_tao_yeu_cau_dich_vu_cap_lai_the.txt";
                            break;
                        case DichVu.MuonHocBaGoc:
                            if (_muonHocBaRepository.IsDuplicated(currentStudent.Id))
                            {
                                throw new DuplicateWaitObjectException();
                            }
                            AsAcademyStudentSvMuonHocBaGoc muonHocBa = new AsAcademyStudentSvMuonHocBaGoc
                            {
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
                                DeletedBy = -1,
                                LyDo = model.LyDo,
                                ThoiGianMuon = model.ThoiGianMuon
                            };
                            await _muonHocBaRepository.AddAsync(muonHocBa);
                            await _unitOfWork.SaveAsync();
                            await UpdateRequestStatus(new UpdateRequestStatusModel
                            {
                                AutoUpdateNgayHen = true,
                                Status = 4,
                                Type = (int)DichVu.MuonHocBaGoc,
                                RequestID = (int)muonHocBa.Id,
                            });
                            //templateName = "template_mail_tao_yeu_cau_dich_vu_muon_hoc_ba.txt";
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
                        case DichVu.DangKyChoO:
                            if (!NhuCauNhaO.All.Contains(model.NhuCauNhaO))
                            {
                                throw new Exception("Nhu cầu nhà ở không hợp lệ");
                            }

                            if (!DoiTuongUuTienNhaO.All.Contains(model.DoiTuongUuTienNhaO))
                            {
                                throw new Exception("Đối tượng ưu tiên nhà ở không hợp lệ");
                            }

                            var dotActive = await _dotDangKyChoORepository.GetDotActive();
                            AsAcademyStudentSvDangKyChoO dkNhaO = new AsAcademyStudentSvDangKyChoO
                            {
                                PhanHoi = model.PhanHoi,
                                CreatedTime = now,
                                DeletedTime = now,
                                LastModifiedTime = now,
                                MaXacNhan = model.MaXacNhan,
                                StudentId = studentID,
                                StudentCode = currentStudent.Code,
                                StudentName = currentStudent.FulName,
                                Status = (int)TrangThaiYeuCau.HoanThanh,
                                Deleted = false,
                                CreatedBy = studentID,
                                LastModifiedBy = studentID,
                                DeletedBy = -1,
                                DotDangKy = dotActive.Id,
                                NhuCauNhaO = model.NhuCauNhaO,
                                DoiTuongUuTienNhaO = model.DoiTuongUuTienNhaO
                            };
                            await _dangKyChoORepository.AddDangKyNhaO(dkNhaO);
                            break;
                        case DichVu.XinMienGiamHocPhi:
                            if (!DoiTuongXinMienGiamHocPhi.All.Contains(model.DoiTuongHuongMienGiam))
                            {
                                throw new Exception("Lựa chọn đối tượng không hợp lệ");
                            }

                            if (!Regex.IsMatch(model.Sdt, "^[0-9]{10}$"))
                            {
                                throw new Exception("Số điện thoại không được bỏ trống và phải là 10 số");
                            }

                            var dotXinActive = await _dotXinMienGiamHocPhiRepository.GetDotActive();
                            AsAcademyStudentSvXinMienGiamHocPhi xinMien = new AsAcademyStudentSvXinMienGiamHocPhi
                            {
                                PhanHoi = model.PhanHoi,
                                CreatedTime = now,
                                DeletedTime = now,
                                LastModifiedTime = now,
                                MaXacNhan = model.MaXacNhan,
                                StudentId = studentID,
                                StudentCode = currentStudent.Code,
                                StudentName = currentStudent.FulName,
                                Status = (int)TrangThaiYeuCau.DaGuiLenNhaTruong,
                                Deleted = false,
                                CreatedBy = studentID,
                                LastModifiedBy = studentID,
                                DeletedBy = -1,
                                DotDangKy = dotXinActive.Id,
                                DoiTuongHuong = model.DoiTuongHuongMienGiam,
                                Sdt = model.Sdt
                            };
                            await _xinMienGiamHocPhiRepository.AddDangKy(xinMien);
                            break;
                        case DichVu.DeNghiHoTroChiPhiHocTap:
                            if (!DoiTuongDeNghiHoTroChiPhi.All.Contains(model.DoiTuongDeNghiHoTro))
                            {
                                throw new Exception("Lựa chọn đối tượng không hợp lệ");
                            }

                            if (!Regex.IsMatch(model.Sdt, "^[0-9]{10}$"))
                            {
                                throw new Exception("Số điện thoại không được bỏ trống và phải là 10 số");
                            }

                            var dotDeNghiActive = await _dotXinMienGiamHocPhiRepository.GetDotActive();
                            AsAcademyStudentSvDeNghiHoTroChiPhiHocTap deNghi = new AsAcademyStudentSvDeNghiHoTroChiPhiHocTap
                            {
                                PhanHoi = model.PhanHoi,
                                CreatedTime = now,
                                DeletedTime = now,
                                LastModifiedTime = now,
                                MaXacNhan = model.MaXacNhan,
                                StudentId = studentID,
                                StudentCode = currentStudent.Code,
                                StudentName = currentStudent.FulName,
                                Status = (int)TrangThaiYeuCau.DaGuiLenNhaTruong,
                                Deleted = false,
                                CreatedBy = studentID,
                                LastModifiedBy = studentID,
                                DeletedBy = -1,
                                DotDangKy = dotDeNghiActive.Id,
                                DoiTuongHuong = model.DoiTuongDeNghiHoTro,
                                Sdt = model.Sdt
                            };
                            await _deNghiHoTroChiPhiRepository.AddDangKy(deNghi);
                            break;
                        case DichVu.HoTroHocTap:
                            var dot = await _dotHoTroHocTapRepository.GetDotActive();
                            if (string.IsNullOrEmpty(currentStudent.DanToc))
                            {
                                throw new Exception($"Sinh viên chưa cập nhật dân tộc");
                            }
                            if (!DanTocHoTroHocTap.All.Contains(StringHelper.ConvertToLatin(currentStudent.DanToc.ToLower())))
                            {
                                throw new Exception($"Dân tộc {currentStudent.DanToc} không được đăng ký dịch vụ hỗ trợ học tập");
                            }
                            AsAcademyStudentSvDangKyHoTroHocTap dky = new AsAcademyStudentSvDangKyHoTroHocTap
                            {
                                PhanHoi = model.PhanHoi,
                                CreatedTime = now,
                                DeletedTime = now,
                                LastModifiedTime = now,
                                MaXacNhan = model.MaXacNhan,
                                StudentId = studentID,
                                StudentCode = currentStudent.Code,
                                StudentName = currentStudent.FulName,
                                Status = (int)TrangThaiYeuCau.DaGuiLenNhaTruong,
                                Deleted = false,
                                CreatedBy = studentID,
                                LastModifiedBy = studentID,
                                DeletedBy = -1,
                                DotDangKy = dot.Id,
                            };
                            await _hoTroHocTapRepository.AddDangKyNhaO(dky);
                            break;
                        default:
                            run = false;
                            break;
                    }
                    if (run)
                    {
                        if (!model.NotSendEmail)
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
                                TenDichVu = dichVu.TenDichVu,
                                TemplateName = templateName,
                            };
                            var result = await _emailService.SendEmailNewServiceRequest(tinNhan);
                            if (result != null)
                            {
                                throw new Exception(result.Message);
                            }
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
                    if (!model.NotSendEmail)
                    {
                        await _logService.WriteLog(new ActivityLogModel
                        {
                            LogCode = dichVu.LogCodeSendEmail,
                            LogMessage = $"gửi mail tới {currentStudent.EmailNhaTruong}",
                        });
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogError("Tạo mới yêu cầu dịch vụ", $"{ex.ToString()} \n", JsonConvert.SerializeObject(model));
                throw ex;
            }
        }
        /// <summary>
        /// Lấy yêu cầu dịch vụ theo sinh viên đang login
        /// </summary>
        /// <param name="dichVuType"></param>
        /// <returns></returns>
        public IQueryable GetAllByStudent(int dichVuType)
        {
            long studentId = _userService.GetCurrentStudentID() ?? 0;
            switch ((DichVu)dichVuType)
            {
                case DichVu.XacNhan:
                    return _xacNhanRepository.GetAll(studentId);
                case DichVu.GioiThieu:
                    return _gioiThieuRepository.GetAll(studentId);
                case DichVu.CapLaiThe:
                    return _capLaiTheRepository.GetAll(studentId);
                case DichVu.MuonHocBaGoc:
                    return _muonHocBaRepository.GetAll(studentId);
                case DichVu.UuDaiGiaoDuc:
                    return _uuDaiRepository.GetAll(studentId);
                case DichVu.VayVonNganHang:
                    return _vayVonRepository.GetAll(studentId);
                case DichVu.ThueNha:
                    return _thueNhaRepository.GetAll(studentId);
                case DichVu.VeBus:
                    return _veXeBusRepository.GetAll(studentId);
                case DichVu.DangKyChoO:
                    return _dangKyChoORepository.GetAllDangKyChoO(studentId);
                case DichVu.XinMienGiamHocPhi:
                    return _xinMienGiamHocPhiRepository.GetAllDangKyChoO(studentId);
                case DichVu.DeNghiHoTroChiPhiHocTap:
                    return _deNghiHoTroChiPhiRepository.GetAllDangKyChoO(studentId);
                case DichVu.HoTroHocTap:
                    return _hoTroHocTapRepository.GetAllDangKyChoO(studentId);
                default:
                    break;
            }
            return null;
        }
        /// <summary>
        /// lấy thông tin chi tiết yêu cầu dịch vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                    //var xacNhanGetAll = _xacNhanRepository.GetAllForAdminCustom(model);
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
                case DichVu.CapLaiThe:
                    var capLaiTheAll = await _capLaiTheRepository.GetAllForAdmin(model);

                    var capLaiTheList = capLaiTheAll.FinalData;

                    foreach (var yeuCau in capLaiTheList)
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
                case DichVu.MuonHocBaGoc:
                    var muonHobaAll = await _muonHocBaRepository.GetAllForAdmin(model);

                    var muonHocBaList = muonHobaAll.FinalData;

                    foreach (var yeuCau in muonHocBaList)
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
                case DichVu.DangKyChoO:
                    var dangKyChoOGetAll = await _dangKyChoORepository.GetAllForAdminDangKyChoO(model);
                    var dangKyChoOList = dangKyChoOGetAll.FinalData;

                    foreach (var yeuCau in dangKyChoOList)
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
                case DichVu.XinMienGiamHocPhi:
                    var xinMienGetAll = await _xinMienGiamHocPhiRepository.GetAllForAdminDangKy(model);
                    var xinMienList = xinMienGetAll.FinalData;

                    foreach (var yeuCau in xinMienList)
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
                case DichVu.DeNghiHoTroChiPhiHocTap:
                    var deNghiGetAll = await _deNghiHoTroChiPhiRepository.GetAllForAdminDangKy(model);
                    var deNghiList = deNghiGetAll.FinalData;

                    foreach (var yeuCau in deNghiList)
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
                case DichVu.HoTroHocTap:
                    var dkyGetAll = await _hoTroHocTapRepository.GetAllForAdminDangKyChoO(model);
                    var dkyList = dkyGetAll.FinalData;

                    foreach (var yeuCau in dkyList)
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
        /// <summary>
        /// Lấy thông tin tổng quát về các yêu cầu dịch vụ
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, AllTypeDichVuModel> GetAllLoaiDichVuInfo()
        {
            var allDichVu = _loaiDichVuRepository.GetAllInUse();
            var quantityDictionary = new Dictionary<int, AllTypeDichVuModel>
            {
                { (int)DichVu.XacNhan, _xacNhanRepository.GetRequestInfo() },
                { (int)DichVu.GioiThieu, _gioiThieuRepository.GetRequestInfo() },
                { (int)DichVu.ThueNha, _thueNhaRepository.GetRequestInfo() },
                { (int)DichVu.CapLaiThe, _capLaiTheRepository.GetRequestInfo() },
                { (int)DichVu.MuonHocBaGoc, _muonHocBaRepository.GetRequestInfo() },
                { (int)DichVu.UuDaiGiaoDuc, _uuDaiRepository.GetRequestInfo() },
                { (int)DichVu.VayVonNganHang, _vayVonRepository.GetRequestInfo() },
                { (int)DichVu.VeBus, _veXeBusRepository.GetRequestInfo() },
                { (int)DichVu.DangKyChoO, _dangKyChoORepository.GetRequestInfo() },
                { (int)DichVu.XinMienGiamHocPhi, _xinMienGiamHocPhiRepository.GetRequestInfo() },
                { (int)DichVu.DeNghiHoTroChiPhiHocTap, _deNghiHoTroChiPhiRepository.GetRequestInfo() },
                { (int)DichVu.HoTroHocTap, _hoTroHocTapRepository.GetRequestInfo() }
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
                    info.TenDichVu = dichVu.Name;
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
        /// <summary>
        /// Chuyển trạng thái một yêu cầu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                        Type = model.Type,
                        MuonHocBaGoc = model.MuonHocBaGoc
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
        /// <summary>
        /// Chuyển trạng thái nhiều yêu cầu sang ĐÃ XỬ LÝ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                            MuonHocBaGoc = model.MuonHocBaGoc
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
            string templateMail = null;

            bool isUsechuyenPhatNhanh = _configuration.GetValue<string>("IsUseDiaChiChuyenPhatNhanh") == "1";
            bool chuyenPhatNhanh = false;
            #region dich vu
            switch (loaiDichVu)
            {
                case DichVu.XacNhan:
                    var xacNhan = await _xacNhanRepository.FindByIdAsync(model.RequestID);
                    xacNhan.Status = model.Status;
                    xacNhan.NgayHenTuNgay = model.NgayHenBatDau;
                    xacNhan.NgayHenDenNgay = model.NgayHenKetThuc;
                    xacNhan.PhanHoi = model.PhanHoi;
                    xacNhan.ChuyenPhatNhanh = isUsechuyenPhatNhanh;
                    ngayTao = xacNhan.CreatedTime;

                    chuyenPhatNhanh = isUsechuyenPhatNhanh;
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
                case DichVu.CapLaiThe:
                    var capLaiThe = await _capLaiTheRepository.FindByIdAsync(model.RequestID);
                    capLaiThe.Status = model.Status;
                    capLaiThe.NgayHenTuNgay = model.NgayHenBatDau;
                    capLaiThe.NgayHenDenNgay = model.NgayHenKetThuc;
                    capLaiThe.PhanHoi = model.PhanHoi;
                    ngayTao = capLaiThe.CreatedTime;

                    student = _studentRepository.FindByCode(capLaiThe.StudentCode);
                    break;
                case DichVu.MuonHocBaGoc:
                    templateMail = "template_mail_cap_nhat_trang_thai_dich_vu_muon_hoc_ba.txt";

                    var muonHocBa = await _muonHocBaRepository.FindByIdAsync(model.RequestID);
                    muonHocBa.Status = model.Status;
                    muonHocBa.NgayHenTuNgay = model.NgayHenBatDau;
                    muonHocBa.NgayHenDenNgay = model.NgayHenKetThuc;
                    muonHocBa.NgayMuon = model.NgayHenBatDau;
                    muonHocBa.NgayTraDuKien = model.NgayHenKetThuc;
                    muonHocBa.PhanHoi = model.PhanHoi;

                    if (muonHocBa.Status == (int)TrangThaiYeuCau.HoanThanh)
                    {
                        muonHocBa.NgayTra = DateTime.Now;
                        templateMail = "";
                    }


                    ngayTao = muonHocBa.CreatedTime;

                    student = _studentRepository.FindByCode(muonHocBa.StudentCode);

                    break;
                case DichVu.ThueNha:
                    var thueNha = await _thueNhaRepository.FindByIdAsync(model.RequestID);
                    thueNha.Status = model.Status;
                    thueNha.NgayHenTuNgay = model.NgayHenBatDau;
                    thueNha.NgayHenDenNgay = model.NgayHenKetThuc;
                    thueNha.PhanHoi = model.PhanHoi;
                    thueNha.ChuyenPhatNhanh = isUsechuyenPhatNhanh;
                    ngayTao = thueNha.CreatedTime;

                    chuyenPhatNhanh = isUsechuyenPhatNhanh;
                    student = _studentRepository.FindByCode(thueNha.StudentCode);
                    break;
                case DichVu.UuDaiGiaoDuc:
                    var uuDai = await _uuDaiRepository.FindByIdAsync(model.RequestID);
                    uuDai.Status = model.Status;
                    uuDai.PhanHoi = model.PhanHoi;
                    uuDai.NgayHenTuNgay = model.NgayHenBatDau;
                    uuDai.NgayHenDenNgay = model.NgayHenKetThuc;
                    uuDai.ChuyenPhatNhanh = isUsechuyenPhatNhanh;
                    ngayTao = uuDai.CreatedTime;

                    chuyenPhatNhanh = isUsechuyenPhatNhanh;
                    student = _studentRepository.FindByCode(uuDai.StudentCode);
                    break;
                case DichVu.VayVonNganHang:
                    var vayVon = await _vayVonRepository.FindByIdAsync(model.RequestID);
                    vayVon.Status = model.Status;
                    vayVon.PhanHoi = model.PhanHoi;
                    vayVon.NgayHenTuNgay = model.NgayHenBatDau;
                    vayVon.NgayHenDenNgay = model.NgayHenKetThuc;
                    vayVon.ChuyenPhatNhanh = isUsechuyenPhatNhanh;
                    ngayTao = vayVon.CreatedTime;

                    chuyenPhatNhanh = isUsechuyenPhatNhanh;
                    student = _studentRepository.FindByCode(vayVon.StudentCode);
                    break;
                case DichVu.VeBus:
                    var veBus = await _veXeBusRepository.FindByIdAsync(model.RequestID);
                    veBus.Status = model.Status;
                    veBus.PhanHoi = model.PhanHoi;
                    veBus.NgayHenTuNgay = model.NgayHenBatDau;
                    veBus.NgayHenDenNgay = model.NgayHenKetThuc;
                    veBus.ChuyenPhatNhanh = isUsechuyenPhatNhanh;
                    ngayTao = veBus.CreatedTime;

                    chuyenPhatNhanh = isUsechuyenPhatNhanh;
                    student = _studentRepository.FindByCode(veBus.StudentCode);
                    break;
                case DichVu.XinMienGiamHocPhi:
                    var xinMienGiam = await _xinMienGiamHocPhiRepository.FindByIdAsync(model.RequestID);
                    xinMienGiam.Status = model.Status;
                    xinMienGiam.PhanHoi = model.PhanHoi;
                    xinMienGiam.NgayHenTuNgay = model.NgayHenBatDau;
                    xinMienGiam.NgayHenDenNgay = model.NgayHenKetThuc;
                    ngayTao = xinMienGiam.CreatedTime;

                    student = _studentRepository.FindByCode(xinMienGiam.StudentCode);
                    break;
                case DichVu.DeNghiHoTroChiPhiHocTap:
                    var deNghi = await _deNghiHoTroChiPhiRepository.FindByIdAsync(model.RequestID);
                    deNghi.Status = model.Status;
                    deNghi.PhanHoi = model.PhanHoi;
                    deNghi.NgayHenTuNgay = model.NgayHenBatDau;
                    deNghi.NgayHenDenNgay = model.NgayHenKetThuc;
                    ngayTao = deNghi.CreatedTime;

                    student = _studentRepository.FindByCode(deNghi.StudentCode);
                    break;
                case DichVu.HoTroHocTap:
                    var dky = await _hoTroHocTapRepository.FindByIdAsync(model.RequestID);
                    dky.Status = model.Status;
                    dky.PhanHoi = model.PhanHoi;
                    dky.NgayHenTuNgay = model.NgayHenBatDau;
                    dky.NgayHenDenNgay = model.NgayHenKetThuc;
                    ngayTao = dky.CreatedTime;

                    student = _studentRepository.FindByCode(dky.StudentCode);
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
                    TemplateName = templateMail,
                    ChuyenPhatNhanh = chuyenPhatNhanh,
                };
                var sendEmailRs = new ResponseBody();
                if (model.Status == (int)TrangThaiYeuCau.HoanThanh)
                {
                    sendEmailRs = await _emailService.SendEmailDoneRequest(tinNhan);
                } else
                {
                    sendEmailRs = await _emailService.SendEmailUpdateStatusRequest(tinNhan);
                }
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

        #region Mượn học bạ (riêng)
        public async Task UpdatePartialInfoMuonHocBa(UpdateRequestStatusMuonHocBaGocModel model)
        {
            try
            {
                if (model.NgayTraDuKien == null)
                {
                    throw new ArgumentNullException("Ngày trả dự kiến không được để trống");
                }
                var muonHocBa = await _muonHocBaRepository.FindByIdAsync(model.Id);
                muonHocBa.Description = model.Description;
                muonHocBa.Notice = model.Notice;
                muonHocBa.NgayMuon = DateTime.Now;
                muonHocBa.NgayTraDuKien = model.NgayTraDuKien ?? DateTime.Now;
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<byte[]> ExportWordMuonHocBaAsync(string studentCode)
        {
            try
            {
                var muonHocBaDefault = _muonHocBaRepository.GetAll(studentCode).FirstOrDefault();
                if (muonHocBaDefault == null)
                {
                    throw new ArgumentNullException("Không có yêu cầu mượn học bạ nào!");
                }
                var exportOutput = await GetExportOutput(DichVu.MuonHocBaGoc, (int)muonHocBaDefault.Id);
                if (exportOutput == null) return null;
                if (exportOutput.document != null)
                {
                    return await DocumentToByteAsync(exportOutput.document, exportOutput.filePath);
                }
                else if (exportOutput.document == null)
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
        #endregion

        #region Đợt đăng ký chỗ ở (riêng)
        public async Task<AsAcademyStudentSvDangKyChoODot> GetDotDangKyChoOActive()
        {
            return await _dotDangKyChoORepository.GetDotActive();
        }

        public async Task<PaginationModel<AsAcademyStudentSvDangKyChoODot>> GetAllDotDangKyChoO(int skip = 0, int take = 20)
        {
            return await _dotDangKyChoORepository.GetAll(skip, take);
        }

        public async Task AddDotDangKyChoO(AddDotDangKyChoOModel model)
        {
           await _dotDangKyChoORepository.Add(model);
        }

        public async Task UpdateDotDangKyChoO(int id, AddDotDangKyChoOModel model)
        {
            await _dotDangKyChoORepository.Update(id, model);
        }

        public async Task DeleteDotDangKyChoO(int id)
        {
            await _dotDangKyChoORepository.Delete(id);
        }
        #endregion

        #region Đợt xin miễn giảm hp (riêng)
        public async Task<AsAcademyStudentSvXinMienGiamHocPhiDot> GetDotXinMienGiamHocPhiActive()
        {
            return await _dotXinMienGiamHocPhiRepository.GetDotActive();
        }

        public async Task<PaginationModel<AsAcademyStudentSvXinMienGiamHocPhiDot>> GetAllDotXinMienGiamHocPhi(int skip = 0, int take = 20)
        {
            return await _dotXinMienGiamHocPhiRepository.GetAll(skip, take);
        }

        public async Task AddDotXinMienGiamHocPhi(AddDotXinMienGiamHocPhi model)
        {
            await _dotXinMienGiamHocPhiRepository.Add(model);
        }

        public async Task UpdateDotXinMienGiamHocPhi(int id, AddDotXinMienGiamHocPhi model)
        {
            await _dotXinMienGiamHocPhiRepository.Update(id, model);
        }

        public async Task DeleteDotXinMienGiamHocPhi(int id)
        {
            await _dotXinMienGiamHocPhiRepository.Delete(id);
        }
        #endregion

        #region Đợt đề nghị giảm chi phí học tập (riêng)
        public async Task<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot> GetDotDeNghiHoTroChiPhiActive()
        {
            return await _dotDeNghiHoTroChiPhiRepository.GetDotActive();
        }

        public async Task<PaginationModel<AsAcademyStudentSvDeNghiHoTroChiPhiHocTapDot>> GetAllDotDeNghiHoTroChiPhi(int skip = 0, int take = 20)
        {
            return await _dotDeNghiHoTroChiPhiRepository.GetAll(skip, take);
        }

        public async Task AddDotDeNghiHoTroChiPhi(AddDotDeNghiHoTroChiPhi model)
        {
            await _dotDeNghiHoTroChiPhiRepository.Add(model);
        }

        public async Task UpdateDotDeNghiHoTroChiPhi(int id, AddDotDeNghiHoTroChiPhi model)
        {
            await _dotDeNghiHoTroChiPhiRepository.Update(id, model);
        }

        public async Task DeleteDotDeNghiHoTroChiPhi(int id)
        {
            await _dotDeNghiHoTroChiPhiRepository.Delete(id);
        }
        #endregion

        #region Đợt hỗ trợ học tập (riêng)
        public async Task<AsAcademyStudentSvDangKyHoTroHocTapDot> GetDotHoTroHocTapActive()
        {
            return await _dotHoTroHocTapRepository.GetDotActive();
        }

        public async Task<PaginationModel<AsAcademyStudentSvDangKyHoTroHocTapDot>> GetAllDotHoTroHocTap(int skip = 0, int take = 20)
        {
            return await _dotHoTroHocTapRepository.GetAll(skip, take);
        }

        public async Task AddDotHoTroHocTap(AddDotHoTroHocTap model)
        {
            await _dotHoTroHocTapRepository.Add(model);
        }

        public async Task UpdateDotHoTroHocTap(int id, AddDotHoTroHocTap model)
        {
            await _dotHoTroHocTapRepository.Update(id, model);
        }

        public async Task DeleteDotHoTroHocTap(int id)
        {
            await _dotHoTroHocTapRepository.Delete(id);
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

        public async Task<byte[]> ExportExcelAsync(DichVu loaiDichVu, List<DichVuExport> dichVuList, long dotDangKy = 0)
        {
            switch (loaiDichVu)
            {
                case DichVu.XacNhan:
                    return await ExportExcelXacNhan(dichVuList);
                case DichVu.GioiThieu:
                    return await ExportExcelGioiThieu(dichVuList);
                case DichVu.UuDaiGiaoDuc:
                    return await ExportExcelUuDai(dichVuList);
                case DichVu.MuonHocBaGoc:
                    return await ExportExcelMuonHocBaGoc(dichVuList);
                case DichVu.VayVonNganHang:
                    return await ExportExcelVayVon(dichVuList);
                case DichVu.ThueNha:
                    return await ExportExcelThueNha(dichVuList);
                case DichVu.VeBus:
                    return await ExportExcelVeXeBus(dichVuList);
                case DichVu.CapLaiThe:
                    return await ExportExcelCapLaiThe(dichVuList);
                case DichVu.DangKyChoO:
                    return await ExportExcelDangKyChoO(dotDangKy);
                case DichVu.XinMienGiamHocPhi:
                    return await ExportExcelXinMienGiamHocPhi(dotDangKy);
                case DichVu.DeNghiHoTroChiPhiHocTap:
                    return await ExportExcelDeNghiHoTroChiPhi(dotDangKy);
                case DichVu.HoTroHocTap:
                    return await ExportExcelHoTroHocTap(dotDangKy);
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
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Ngày sinh");
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Xã");
                initHeaderCell(ws, firstRow, ++i, "Huyện");
                initHeaderCell(ws, firstRow, ++i, "Tỉnh");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Niên khóa");
                initHeaderCell(ws, firstRow, ++i, "Khoa quản lý");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");
                initHeaderCell(ws, firstRow, ++i, "Hệ đào tạo");
                initHeaderCell(ws, firstRow, ++i, "Lý do xác nhận");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);

                initHeaderCell(ws, firstRow, ++i, "Ngày ký");
                initHeaderCell(ws, firstRow, ++i, "Tháng ký");
                initHeaderCell(ws, firstRow, ++i, "Năm ký");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ nhận chuyển phát nhanh");

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
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";
                    string diaChiChuyenPhatNhanh = yeuCau.Student.BaoTinDiaChiNhanChuyenPhatNhanh ?? "";

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
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);
                    ws.Cell(row, ++col).SetValue("Chính quy");
                    ws.Cell(row, ++col).SetValue(lyDo);
                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);
                    ws.Cell(row, ++col).SetValue(diaChiChuyenPhatNhanh);
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
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Ngày sinh");
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Xã");
                initHeaderCell(ws, firstRow, ++i, "Huyện");
                initHeaderCell(ws, firstRow, ++i, "Tỉnh");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Niên khóa");
                initHeaderCell(ws, firstRow, ++i, "Khoa quản lý");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");
                initHeaderCell(ws, firstRow, ++i, "Hệ đào tạo");
                initHeaderCell(ws, firstRow, ++i, "Năm thứ");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);
                initHeaderCell(ws, firstRow, ++i, "Học Kỳ");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);
                initHeaderCell(ws, firstRow, ++i, "Năm học");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);
                initHeaderCell(ws, firstRow, ++i, "Thời gian khóa học \n (bao nhiêu năm)");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);
                ws.Cell(firstRow, i).Style.Alignment.WrapText = true;
                initHeaderCell(ws, firstRow, ++i, "Kỷ luật");
                ws.Cell(firstRow, i).Style.Fill.SetBackgroundColor(XLColor.White);

                initHeaderCell(ws, firstRow, ++i, "Ngày ký");
                initHeaderCell(ws, firstRow, ++i, "Tháng ký");
                initHeaderCell(ws, firstRow, ++i, "Năm ký");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ nhận chuyển phát nhanh");

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
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";
                    string diaChiChuyenPhatNhanh = yeuCau.Student.BaoTinDiaChiNhanChuyenPhatNhanh ?? "";

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
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);
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
                    ws.Cell(row, ++col).SetValue(diaChiChuyenPhatNhanh);
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
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ nhận chuyển phát nhanh");


                ws.Row(firstRow).Height = 32;
                string tenTruong = "ĐẠI HỌC XÂY DỰNG HÀ NỘI";

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
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";
                    string diaChiNhanChuyenPhatNhanh = yeuCau.Student.BaoTinDiaChiNhanChuyenPhatNhanh ?? "";

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
                    ws.Cell(row, ++col).SetValue(email);
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);
                    ws.Cell(row, ++col).SetValue(diaChiNhanChuyenPhatNhanh);
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
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Ngày sinh");
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Xã");
                initHeaderCell(ws, firstRow, ++i, "Huyện");
                initHeaderCell(ws, firstRow, ++i, "Tỉnh");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Niên khóa");
                initHeaderCell(ws, firstRow, ++i, "Khoa quản lý");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");
                initHeaderCell(ws, firstRow, ++i, "Hệ đào tạo");

                initHeaderCell(ws, firstRow, ++i, "Năm thứ");
                initHeaderCell(ws, firstRow, ++i, "Giới tính");
                initHeaderCell(ws, firstRow, ++i, "Đối tượng\nưu tiên");
                ws.Cell(firstRow, i).Style.Alignment.WrapText = true;
                initHeaderCell(ws, firstRow, ++i, "Số CMTND");
                initHeaderCell(ws, firstRow, ++i, "Cấp\nngày/tháng/năm");
                ws.Cell(firstRow, i).Style.Alignment.WrapText = true;
                initHeaderCell(ws, firstRow, ++i, "Nơi cấp");

                initHeaderCell(ws, firstRow, ++i, "Ngày ký");
                initHeaderCell(ws, firstRow, ++i, "Tháng ký");
                initHeaderCell(ws, firstRow, ++i, "Năm ký");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ nhận chuyển phát nhanh");

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
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";
                    string nganhHoc = yeuCau.Academics?.Name ?? "";
                    string doiTuongUuTien = yeuCau.Student.DoiTuongUuTien ?? "";
                    string diaChiNhanChuyenPhatNhanh = yeuCau.Student.BaoTinDiaChiNhanChuyenPhatNhanh ?? "";
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
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);
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
                    ws.Cell(row, ++col).SetValue(diaChiNhanChuyenPhatNhanh);
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
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Ngày sinh");
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Xã");
                initHeaderCell(ws, firstRow, ++i, "Huyện");
                initHeaderCell(ws, firstRow, ++i, "Tỉnh");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Niên khóa");
                initHeaderCell(ws, firstRow, ++i, "Khoa quản lý");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");
                initHeaderCell(ws, firstRow, ++i, "Hệ đào tạo");

                initHeaderCell(ws, firstRow, ++i, "Liên tuyến");
                initHeaderCell(ws, firstRow, ++i, "Số tuyến");
                initHeaderCell(ws, firstRow, ++i, "Nơi nộp đơn và nhận thẻ");

                initHeaderCell(ws, firstRow, ++i, "Ngày ký");
                initHeaderCell(ws, firstRow, ++i, "Tháng ký");
                initHeaderCell(ws, firstRow, ++i, "Năm ký");

                initHeaderCell(ws, firstRow, ++i, "Địa chỉ nhận chuyển phát nhanh");

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
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";
                    string lienTuyen = (DichVuXeBusLoaiTuyen)yeuCau.YeuCauDichVu.TuyenType == DichVuXeBusLoaiTuyen.LienTuyen ? "x" : "";
                    string soTuyen = yeuCau.YeuCauDichVu.TuyenCode;
                    string noiNhanThe = yeuCau.YeuCauDichVu.NoiNhanThe;
                    string diaChiNhanChuyenPhatNhanh = yeuCau.Student.BaoTinDiaChiNhanChuyenPhatNhanh ?? "";
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
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);
                    ws.Cell(row, ++col).SetValue("Chính quy");

                    ws.Cell(row, ++col).SetValue(lienTuyen);
                    ws.Cell(row, ++col).SetValue(soTuyen);
                    ws.Cell(row, ++col).SetValue(noiNhanThe);

                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);

                    ws.Cell(row, ++col).SetValue(diaChiNhanChuyenPhatNhanh);
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

        /// <summary>
        /// Mặc định lấy đợt đăng ký mới nhất
        /// </summary>
        /// <returns></returns>
        private async Task<byte[]> ExportExcelDangKyChoO(long dotDangKy)
        {
            var yeuCauList = (await _dangKyChoORepository.GetAllYeuCauDichVuTheoDot(dotDangKy)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                int i = 0;
                int firstRow = 1;
                #region title
                initHeaderCell(ws, firstRow, ++i, "STT");
                initHeaderCell(ws, firstRow, ++i, "Dấu thời gian");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Khoa quản lý");
                initHeaderCell(ws, firstRow, ++i, "Nhu cầu nhà ở");
                initHeaderCell(ws, firstRow, ++i, "Đối tượng ưu tiên");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");


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
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";
                    string dauThoiGian = yeuCau.YeuCauDichVu.CreatedTime?.ToString("dd/MM/yyyy HH:MM:ss") ?? "";
                    string nhuCauNhaO = "";
                    switch(yeuCau.YeuCauDichVu.NhuCauNhaO)
                    {
                        case NhuCauNhaO.KTX:
                            nhuCauNhaO = "Ký túc xá ĐHXD";
                            break;
                        case NhuCauNhaO.PHAP_VAN:
                            nhuCauNhaO = "Khu nhà ở Pháp Vân";
                            break;
                    }
                    string doiTuongUuTien = "";
                    switch (yeuCau.YeuCauDichVu.DoiTuongUuTienNhaO)
                    {
                        case DoiTuongUuTienNhaO.NHOM_1:
                            doiTuongUuTien = "Nhóm 1: Sinh viên đang ở Ký túc xá ĐHXD";
                            break;
                        case DoiTuongUuTienNhaO.NHOM_2:
                            doiTuongUuTien = "Nhóm 2: Sinh viên thuộc đối tượng chính sách, hoàn cảnh khó khăn; Sinh viên nữ";
                            break;
                    }
                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(j + 1);
                    ws.Cell(row, ++col).SetValue(dauThoiGian);
                    ws.Cell(row, ++col).SetValue(studentName);
                    ws.Cell(row, ++col).SetValue(studentCode);
                    ws.Cell(row, ++col).SetValue(classCode);
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(nhuCauNhaO);
                    ws.Cell(row, ++col).SetValue(doiTuongUuTien);
                    ws.Cell(row, ++col).SetValue(mobile);
                    ws.Cell(row, ++col).SetValue(email);
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);
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

        /// <summary>
        /// Mặc định lấy đợt đăng ký mới nhất
        /// </summary>
        /// <returns></returns>
        private async Task<byte[]> ExportExcelXinMienGiamHocPhi(long dotDangKy)
        {
            var yeuCauList = (await _xinMienGiamHocPhiRepository.GetAllYeuCauDichVuTheoDot(dotDangKy)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                int i = 0;
                int firstRow = 1;
                #region title
                initHeaderCell(ws, firstRow, ++i, "STT");
                initHeaderCell(ws, firstRow, ++i, "Dấu thời gian");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Khoa quản lý");
                initHeaderCell(ws, firstRow, ++i, "Đối tượng");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");

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
                    string dauThoiGian = yeuCau.YeuCauDichVu.CreatedTime?.ToString("dd/MM/yyyy HH:MM:ss") ?? "";
                    string sdt = yeuCau.YeuCauDichVu.Sdt;
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";
                    string doiTuong = "";
                    switch (yeuCau.YeuCauDichVu.DoiTuongHuong)
                    {
                        case "CO_CONG_CACH_MANG":
                            doiTuong = "Là con của người có công với cách mạng được hưởng ưu đãi";
                            break;
                        case "SV_VAN_BANG_1":
                            doiTuong = "Là sinh viên học đại học văn bằng thứ nhất từ 16 tuổi đến 22 tuổi thuộc một trong các trường hợp quy định tại Khoản 1 Điều 5 Nghị định số 136/2013/NĐ - CP ngày 21/10/2013 của Chính phủ quy định chính sách trợ giúp xã hội";
                            break;
                        case "TAN_TAT_KHO_KHAN_KINH_TE":
                            doiTuong = "Bản thân bị khuyết tật, tàn tật có khó khăn về kinh tế";
                            break;
                        case "DAN_TOC_HO_NGHEO":
                            doiTuong = "Bản thân là người dân tộc thiểu số thuộc hộ nghèo, hộ cận nghèo";
                            break;
                        case "DAN_TOC_IT_NGUOI_VUNG_KHO_KHAN":
                            doiTuong = "Bản thân là người dân tộc thiểu số rất ít người và ở vùng có điều kiện kinh tế - xã hội khó khăn hoặc đặc biệt khó khăn";
                            break;
                        case "DAN_TOC_VUNG_KHO_KHAN":
                            doiTuong = "Bản thân là người dân tộc thiểu số và ở vùng có điều kiện kinh tế - xã hội đặc biệt khó khăn";
                            break;
                        case "CHA_ME_TAI_NAN_DUOC_TRO_CAP":
                            doiTuong = "Là con của cán bộ, công nhân, viên chức mà cha hoặc mẹ bị tai nạn lao động hoặc mắc bệnh nghề nghiệp được hưởng trợ cấp thường xuyên";
                            break;
                    }
                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(j + 1);
                    ws.Cell(row, ++col).SetValue(dauThoiGian);
                    ws.Cell(row, ++col).SetValue(studentName);
                    ws.Cell(row, ++col).SetValue(studentCode);
                    ws.Cell(row, ++col).SetValue(classCode);
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(doiTuong);
                    ws.Cell(row, ++col).SetValue(sdt);
                    ws.Cell(row, ++col).SetValue(email);
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);
                }
                for (int j = 0; j < col; j++)
                {
                    ws.Column(j + 1).AdjustToContents();
                }
                #endregion
                string file = _pathProvider.MapPath($"Templates/Ctsv/vayvon_{Guid.NewGuid()}.xlsx");
                wb.SaveAs(file);
                return await FileToByteAsync(file);
            }
        }

        /// <summary>
        /// Mặc định lấy đợt đăng ký mới nhất
        /// </summary>
        /// <returns></returns>
        private async Task<byte[]> ExportExcelDeNghiHoTroChiPhi(long dotDangKy)
        {
            var yeuCauList = (await _deNghiHoTroChiPhiRepository.GetAllYeuCauDichVuTheoDot(dotDangKy)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                int i = 0;
                int firstRow = 1;
                #region title
                initHeaderCell(ws, firstRow, ++i, "STT");
                initHeaderCell(ws, firstRow, ++i, "Dấu thời gian");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Khoa quản lý");
                initHeaderCell(ws, firstRow, ++i, "Đối tượng");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");

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
                    string dauThoiGian = yeuCau.YeuCauDichVu.CreatedTime?.ToString("dd/MM/yyyy HH:MM:ss") ?? "";
                    string sdt = yeuCau.YeuCauDichVu.Sdt;
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";
                    string doiTuong = "";
                    switch (yeuCau.YeuCauDichVu.DoiTuongHuong)
                    {
                        case "DAN_TOC_HO_NGHEO":
                            doiTuong = "Bản thân là người dân tộc thiểu số thuộc hộ nghèo";
                            break;
                        case "DAN_TOC_HO_CAN_NGHEO":
                            doiTuong = "Bản thân là người dân tộc thiểu số thuộc hộ cận nghèo";
                            break;
                    }
                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(j + 1);
                    ws.Cell(row, ++col).SetValue(dauThoiGian);
                    ws.Cell(row, ++col).SetValue(studentName);
                    ws.Cell(row, ++col).SetValue(studentCode);
                    ws.Cell(row, ++col).SetValue(classCode);
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(doiTuong);
                    ws.Cell(row, ++col).SetValue(sdt);
                    ws.Cell(row, ++col).SetValue(email);
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);
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

        /// <summary>
        /// Mặc định lấy đợt đăng ký mới nhất
        /// </summary>
        /// <returns></returns>
        private async Task<byte[]> ExportExcelHoTroHocTap(long dotDangKy)
        {
            var yeuCauList = (await _hoTroHocTapRepository.GetAllYeuCauDichVuTheoDot(dotDangKy)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                int i = 0;
                int firstRow = 1;
                #region title
                initHeaderCell(ws, firstRow, ++i, "STT");
                initHeaderCell(ws, firstRow, ++i, "Dấu thời gian");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Khoa quản lý");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");

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
                    string dauThoiGian = yeuCau.YeuCauDichVu.CreatedTime?.ToString("dd/MM/yyyy HH:MM:ss") ?? "";
                    string sdt = yeuCau.Student.Mobile;
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";
                    int row = j + 2;

                    col = 0;
                    ws.Cell(row, ++col).SetValue(j + 1);
                    ws.Cell(row, ++col).SetValue(dauThoiGian);
                    ws.Cell(row, ++col).SetValue(studentName);
                    ws.Cell(row, ++col).SetValue(studentCode);
                    ws.Cell(row, ++col).SetValue(classCode);
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(sdt);
                    ws.Cell(row, ++col).SetValue(email);
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);
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

        private async Task<byte[]> ExportExcelGioiThieu(List<DichVuExport> dichVuList)
        {
            List<long> ids = new List<long>();
            foreach (var item in dichVuList)
            {
                ids.Add(item.ID);
            }
            var yeuCauList = (await _gioiThieuRepository.GetYeuCauDichVuStudent(ids)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                int i = 0;
                int firstRow = 1;
                #region title
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Ngày sinh");
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Xã");
                initHeaderCell(ws, firstRow, ++i, "Huyện");
                initHeaderCell(ws, firstRow, ++i, "Tỉnh");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Niên khóa");
                initHeaderCell(ws, firstRow, ++i, "Khoa quản lý");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");
                initHeaderCell(ws, firstRow, ++i, "Hệ đào tạo");
                initHeaderCell(ws, firstRow, ++i, "Kính gửi");
                initHeaderCell(ws, firstRow, ++i, "Đến gặp");
                initHeaderCell(ws, firstRow, ++i, "Về việc");
                initHeaderCell(ws, firstRow, ++i, "Ngày ký");
                initHeaderCell(ws, firstRow, ++i, "Tháng ký");
                initHeaderCell(ws, firstRow, ++i, "Năm ký");

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
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";

                    string donVi = yeuCau.YeuCauDichVu.DonVi ?? "";
                    string denGap = yeuCau.YeuCauDichVu.DenGap ?? "";
                    string veViec = yeuCau.YeuCauDichVu.VeViec ?? "";
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
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);
                    ws.Cell(row, ++col).SetValue("Chính quy");
                    ws.Cell(row, ++col).SetValue(donVi);
                    ws.Cell(row, ++col).SetValue(denGap);
                    ws.Cell(row, ++col).SetValue(veViec);
                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);
                }
                for (int j = 0; j < col; j++)
                {
                    ws.Column(j + 1).AdjustToContents();
                }
                #endregion
                string file = _pathProvider.MapPath($"Templates/Ctsv/gioithieu_{Guid.NewGuid().ToString()}.xlsx");
                wb.SaveAs(file);
                return await FileToByteAsync(file);
            }
        }

        private async Task<byte[]> ExportExcelCapLaiThe(List<DichVuExport> dichVuList)
        {
            List<long> ids = new List<long>();
            foreach (var item in dichVuList)
            {
                ids.Add(item.ID);
            }
            var yeuCauList = (await _capLaiTheRepository.GetYeuCauDichVuStudent(ids)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                int i = 0;
                int firstRow = 1;
                #region title
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Ngày sinh");
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Xã");
                initHeaderCell(ws, firstRow, ++i, "Huyện");
                initHeaderCell(ws, firstRow, ++i, "Tỉnh");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Niên khóa");
                initHeaderCell(ws, firstRow, ++i, "Khoa quản lý");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");
                initHeaderCell(ws, firstRow, ++i, "Ngày ký");
                initHeaderCell(ws, firstRow, ++i, "Tháng ký");
                initHeaderCell(ws, firstRow, ++i, "Năm ký");

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
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";

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
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);
                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);
                }
                for (int j = 0; j < col; j++)
                {
                    ws.Column(j + 1).AdjustToContents();
                }
                #endregion
                string file = _pathProvider.MapPath($"Templates/Ctsv/cap_lai_the_{Guid.NewGuid().ToString()}.xlsx");
                wb.SaveAs(file);
                return await FileToByteAsync(file);
            }
        }

        private async Task<byte[]> ExportExcelMuonHocBaGoc(List<DichVuExport> dichVuList)
        {
            List<long> ids = new List<long>();
            foreach (var item in dichVuList)
            {
                ids.Add(item.ID);
            }
            var yeuCauList = (await _muonHocBaRepository.GetYeuCauDichVuStudent(ids)).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var style = XLWorkbook.DefaultStyle;
                var ws = wb.Worksheets.Add("Sheet1");
                ws.Style.Font.SetFontSize(12);
                ws.Style.Font.SetFontName("Times New Roman");

                int i = 0;
                int firstRow = 1;
                #region title
                initHeaderCell(ws, firstRow, ++i, "Mã số SV");
                initHeaderCell(ws, firstRow, ++i, "Họ và tên");
                initHeaderCell(ws, firstRow, ++i, "Ngày sinh");
                initHeaderCell(ws, firstRow, ++i, "Email");
                initHeaderCell(ws, firstRow, ++i, "Xã");
                initHeaderCell(ws, firstRow, ++i, "Huyện");
                initHeaderCell(ws, firstRow, ++i, "Tỉnh");
                initHeaderCell(ws, firstRow, ++i, "Lớp");
                initHeaderCell(ws, firstRow, ++i, "Khoa quản lý");
                initHeaderCell(ws, firstRow, ++i, "Số điện thoại");
                initHeaderCell(ws, firstRow, ++i, "Địa chỉ người nhận");

                initHeaderCell(ws, firstRow, ++i, "Thời gian mượn");
                initHeaderCell(ws, firstRow, ++i, "Mục đích");

                initHeaderCell(ws, firstRow, ++i, "Ngày ký");
                initHeaderCell(ws, firstRow, ++i, "Tháng ký");
                initHeaderCell(ws, firstRow, ++i, "Năm ký");

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
                    string diaChiBaoTin = yeuCau.Student.BaoTinDiaChiNguoiNhan ?? "";

                    string thoiGianMuon = yeuCau.YeuCauDichVu.ThoiGianMuon ?? "";
                    string mucDich = yeuCau.YeuCauDichVu.LyDo ?? "";

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
                    ws.Cell(row, ++col).SetValue(tenKhoa);
                    ws.Cell(row, ++col).SetValue(mobile);
                    ws.Cell(row, ++col).SetValue(diaChiBaoTin);

                    ws.Cell(row, ++col).SetValue(thoiGianMuon);
                    ws.Cell(row, ++col).SetValue(mucDich);

                    ws.Cell(row, ++col).SetValue(now.Day);
                    ws.Cell(row, ++col).SetValue(now.Month);
                    ws.Cell(row, ++col).SetValue(now.Year);
                }
                for (int j = 0; j < col; j++)
                {
                    ws.Column(j + 1).AdjustToContents();
                }
                #endregion
                string file = _pathProvider.MapPath($"Templates/Ctsv/muon_hoc_ba_goc_{Guid.NewGuid().ToString()}.xlsx");
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
                case DichVu.GioiThieu:
                    return await ExportWordGioiThieu(id);
                case DichVu.CapLaiThe:
                    return await ExportWordCapLaiThe(id);
                case DichVu.MuonHocBaGoc:
                    return await ExportWordMuonHocBaGoc(id);
                case DichVu.UuDaiGiaoDuc:
                    return await ExportWordUuDai(id);
                case DichVu.VayVonNganHang:
                    return await ExportWordVayVon(id);
                case DichVu.ThueNha:
                    return await ExportWordThueNha(id);
                case DichVu.VeBus:
                    return await ExportWordVeXeBus(id);
                case DichVu.XinMienGiamHocPhi:
                    return await ExportWordXinMienGiamHP(id);
                case DichVu.DeNghiHoTroChiPhiHocTap:
                    return await ExportWordDeNghiHoTroChiPhiHocTap(id);
                case DichVu.HoTroHocTap:
                    return await ExportWordHoTroHocTap(id);
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
                    Size = 11
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
                    Size = 11
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

            rowT2.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", "TRƯỜNG ĐẠI HỌC XÂY DỰNG HÀ NỘI"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 11
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
                    Size = 11
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
                 new HorizontalPosition(1.65, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                new GemBox.Document.Size(125, 0)));
            horizontalLine1.Outline.Width = 1;
            horizontalLine1.Outline.Fill.SetSolid(GemboxColor.Black);
            paragraph.Inlines.Add(horizontalLine1);

            var horizontalLine2 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                new HorizontalPosition(9.18, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
               new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
               new GemBox.Document.Size(151, 0)));
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
            , new GemboxRun(document, "TRƯỜNG ĐẠI HỌC XÂY DỰNG HÀ NỘI")
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
            string NamHocHienTai = string.Format("{0} - {1}", DateTime.Now.Year, DateTime.Now.Year + 1);
            string thoiGianKhoaHoc = getThoiGianKhoaHoc(NienKhoa);
            string HinhThuc = isLienThong(Class) ? "Liên thông" : "Chính qui";
            var NamThu = getNamThu(NienKhoa);

            string filePath = _pathProvider.MapPath($"Templates/Ctsv/giay_uu_dai.docx");
            string destination = _pathProvider.MapPath($"Templates/Ctsv/uudai_{studentInfo.Student.Code}_{DateTime.Now.ToFileTime()}.docx");

            var now = DateTime.Now;

            var ngayKy = now.ToString("dd");
            var thangKy = now.Month.ToString();
            var namKy = now.Year.ToString();

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
                        replaceTextTemplate(text, "<ho_ten>", studentInfo.Student.FulName);
                        replaceTextTemplate(text, "<nam_thu>", NamThu);
                        replaceTextTemplate(text, "<hoc_ky>", "1");
                        replaceTextTemplate(text, "<nam_hoc>", NamHocHienTai);
                        replaceTextTemplate(text, "<mssv>", MaSV);
                        replaceTextTemplate(text, "<ma_lop>", Class);
                        replaceTextTemplate(text, "<khoa>", TenKhoa);
                        replaceTextTemplate(text, "<nien_khoa>", NienKhoa);
                        replaceTextTemplate(text, "<tgkh>", thoiGianKhoaHoc);
                        replaceTextTemplate(text, "<hinh_thuc>", HinhThuc);
                        replaceTextTemplate(text, "<ngay>", ngayKy);
                        replaceTextTemplate(text, "<thang>", thangKy);
                        replaceTextTemplate(text, "<nam>", namKy);
                        replaceTextTemplate(text, "<chuc_danh_nguoi_ky>", ChucDanhNguoiKy);
                        replaceTextTemplate(text, "<ten_nguoi_ky>", TenNguoiKy);
                    }
                    #endregion
                    mainPart.Document.Save();
                }
                await File.WriteAllBytesAsync(destination, templateStream.ToArray());
            }

            return new ExportFileOutputModel { document = null, filePath = destination };
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
            string tenTruong = "TRƯỜNG ĐẠI HỌC XÂY DỰNG HÀ NỘI";
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
                    Size = 11
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
                    Size = 11
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

            rowT2.Cells.Add(new GemBox.Document.Tables.TableCell(document, new GemboxParagraph(document, new GemboxRun(document, string.Format("{0}", "TRƯỜNG ĐẠI HỌC XÂY DỰNG HÀ NỘI"))
            {
                CharacterFormat = new CharacterFormat()
                {
                    Bold = true,
                    Size = 11
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
                    Size = 11
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
                 new HorizontalPosition(2.15, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
                new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
                new GemBox.Document.Size(80, 0)));
            horizontalLine1.Outline.Width = 1;
            horizontalLine1.Outline.Fill.SetSolid(GemboxColor.Black);
            paragraph.Inlines.Add(horizontalLine1);

            var horizontalLine2 = new Shape(document, ShapeType.Line, GemBox.Document.Layout.Floating(
                new HorizontalPosition(9.2, GemBox.Document.LengthUnit.Centimeter, HorizontalPositionAnchor.Margin),
               new VerticalPosition(3.5, GemBox.Document.LengthUnit.Centimeter, VerticalPositionAnchor.InsideMargin),
               new GemBox.Document.Size(151, 0)));
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
                new GemBox.Document.Size(160, 0)));
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
                new GemboxRun(document, "Đại học Xây dựng Hà Nội") { CharacterFormat = new CharacterFormat { Bold = true } },
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
                    mainPart.Document.Save();
                }
                await File.WriteAllBytesAsync(destination, templateStream.ToArray());
            }
            return new ExportFileOutputModel { document = null, filePath = destination };
        }

        private async Task<ExportFileOutputModel> ExportWordGioiThieu(int id)
        {
            var gioiThieu = await _gioiThieuRepository.FindByIdAsync(id);
            if (gioiThieu == null)
            {
                throw new Exception("Yêu cầu không tồn tại");
            }
            var studentInfo = await _studentRepository.GetStudentDichVuInfoAsync(gioiThieu.StudentCode);
            if (studentInfo == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }

            var paramSet = _thamSoDichVuService.GetParameters(DichVu.GioiThieu)
                                .ToDictionary(x => x.Name, x => x.Value);

            string ChucDanhNguoiKy = paramSet.ContainsKey("ChucDanhNguoiKy") ? paramSet["ChucDanhNguoiKy"] : "";
            string TenNguoiKy = paramSet.ContainsKey("TenNguoiKy") ? paramSet["TenNguoiKy"] : "";

            string filePath = _pathProvider.MapPath($"Templates/Ctsv/giay_gioi_thieu.docx");
            string destination = _pathProvider.MapPath($"Templates/Ctsv/giay-gioi-thieu-{DateTime.Now.ToFileTime()}.docx");

            var now = DateTime.Now;

            var expiredDate = now.AddDays(15).ToString("dd/MM/yyyy");
            var ngayKy = now.ToString("dd");
            var thangKy = now.Month.ToString();
            var namKy = now.Year.ToString();

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
                        replaceTextTemplate(text, "<ho_ten>", studentInfo.Student.FulName);
                        replaceTextTemplate(text, "<sdt>", studentInfo.Student.Mobile);
                        replaceTextTemplate(text, "<don_vi>", gioiThieu.DonVi);
                        replaceTextTemplate(text, "<den_gap>", gioiThieu.DenGap);
                        replaceTextTemplate(text, "<ve_viec>", gioiThieu.VeViec);
                        replaceTextTemplate(text, "<lop>", studentInfo.Student?.ClassCode);
                        replaceTextTemplate(text, "<khoa>", studentInfo.AcademyClass?.SchoolYear);
                        replaceTextTemplate(text, "<han>", expiredDate);
                        replaceTextTemplate(text, "<ngay_ky>", ngayKy);
                        replaceTextTemplate(text, "<thang_ky>", thangKy);
                        replaceTextTemplate(text, "<nam_ky>", namKy);
                        replaceTextTemplate(text, "<chuc_danh_nguoi_ky>", ChucDanhNguoiKy);
                        replaceTextTemplate(text, "<ten_nguoi_ky>", TenNguoiKy);
                    }
                    #endregion
                    mainPart.Document.Save();
                }
                await File.WriteAllBytesAsync(destination, templateStream.ToArray());
            }
            return new ExportFileOutputModel { document = null, filePath = destination };
        }

        private async Task<ExportFileOutputModel> ExportWordCapLaiThe(int id)
        {
            var capLaiThe = await _capLaiTheRepository.FindByIdAsync(id);
            if (capLaiThe == null)
            {
                throw new Exception("Yêu cầu không tồn tại");
            }
            var studentInfo = await _studentRepository.GetStudentDichVuInfoAsync(capLaiThe.StudentCode);
            if (studentInfo == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }

            var paramSet = _thamSoDichVuService.GetParameters(DichVu.CapLaiThe)
                                .ToDictionary(x => x.Name, x => x.Value);

            string ChucDanhNguoiKy = paramSet.ContainsKey("ChucDanhNguoiKy") ? paramSet["ChucDanhNguoiKy"] : "";
            string TenNguoiKy = paramSet.ContainsKey("TenNguoiKy") ? paramSet["TenNguoiKy"] : "";
            string NoiNhanThe = paramSet.ContainsKey("NoiNhanThe") ? paramSet["NoiNhanThe"] : "";

            string filePath = _pathProvider.MapPath($"Templates/Ctsv/cap_lai_the_sv.docx");
            string destination = _pathProvider.MapPath($"Templates/Ctsv/cap-lai-the-sv-{DateTime.Now.ToFileTime()}.docx");

            var now = DateTime.Now;

            var ngayKy = now.ToString("dd");
            var thangKy = now.Month.ToString();
            var namKy = now.Year.ToString();
            var ngaySinh = convertStudentDateOfBirth(studentInfo.Student.DateOfBirth);
            
            Image newImg = null;
            try
            {
                string imgPath = $"ANHSV/{studentInfo.Student.Code}.jpg";
                string fullImgPath = _pathProvider.MapPathStudentImage(imgPath);
                var dataStream = await FileHelper.DownloadFileAsync(fullImgPath);
                // ảnh 4 * 6
                int width = 1200;
                int height = 1800;

                Image img = Image.FromStream(dataStream);

                Image resizedNewImg = FileHelper.ResizeImage(img, width, 2000, false);
                newImg = FileHelper.CropImage(resizedNewImg, width, height);
            }
            catch (Exception)
            {

            }

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
                        replaceTextTemplate(text, "<ho_ten>", studentInfo.Student.FulName);
                        replaceTextTemplate(text, "<mssv>", studentInfo.Student.Code);
                        replaceTextTemplate(text, "<sdt>", studentInfo.Student.Mobile);
                        replaceTextTemplate(text, "<email>", studentInfo.Student.EmailNhaTruong);
                        replaceTextTemplate(text, "<ten_lop>", studentInfo.Student?.ClassCode);
                        replaceTextTemplate(text, "<ten_khoa>", onlyTenKhoa(studentInfo.Faculty?.Name));
                        replaceTextTemplate(text, "<nien_khoa>", studentInfo.AcademyClass?.SchoolYear);
                        replaceTextTemplate(text, "<ngay_sinh>", ngaySinh.ToString("dd/MM/yyyy"));
                        replaceTextTemplate(text, "<hktt_phuong>", onlyTenPhuong(studentInfo.Student.HkttPhuong));
                        replaceTextTemplate(text, "<hktt_quan>", onlyTenQuan(studentInfo.Student.HkttQuan));
                        replaceTextTemplate(text, "<hktt_tinh>", onlyTenTinh(studentInfo.Student.HkttTinh));
                        replaceTextTemplate(text, "<ngay_ky>", ngayKy);
                        replaceTextTemplate(text, "<thang_ky>", thangKy);
                        replaceTextTemplate(text, "<nam_ky>", namKy);
                        replaceTextTemplate(text, "<chuc_danh_nguoi_ky>", ChucDanhNguoiKy);
                        replaceTextTemplate(text, "<ten_nguoi_ky>", TenNguoiKy);
                        replaceTextTemplate(text, "<noi_nhan_the>", NoiNhanThe);
                    }
                    #endregion
                    #region handle image
                    var picture = mainPart.Document.Descendants<DocumentFormat.OpenXml.Drawing.Pictures.Picture>()
                             .FirstOrDefault(p => "Picture 1" == p.NonVisualPictureProperties.NonVisualDrawingProperties.Name);
                    var blip = picture.BlipFill.Blip;
                    if (newImg != null)
                    {
                        ImagePart newImgPart = mainPart.AddImagePart(ImagePartType.Png);
                        // Put image data into the ImagePart (from a filestream)
                        var streamImg = FileHelper.ImageToStream(newImg);
                        newImgPart.FeedData(streamImg);
                        // Point blip at new image
                        blip.Embed = mainPart.GetIdOfPart(newImgPart);
                    }
                    else
                    {
                        blip.Embed = "";
                    }
                    #endregion
                    mainPart.Document.Save();
                }
                await File.WriteAllBytesAsync(destination, templateStream.ToArray());
            }
            return new ExportFileOutputModel { document = null, filePath = destination };
        }

        private async Task<ExportFileOutputModel> ExportWordMuonHocBaGoc(int id)
        {
            var muonHocBa = await _muonHocBaRepository.FindByIdAsync(id);
            if (muonHocBa == null)
            {
                throw new Exception("Yêu cầu không tồn tại");
            }
            var studentInfo = await _studentRepository.GetStudentDichVuInfoAsync(muonHocBa.StudentCode);
            if (studentInfo == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }


            string filePath = _pathProvider.MapPath($"Templates/Ctsv/muon_hoc_ba_goc.docx");
            string destination = _pathProvider.MapPath($"Templates/Ctsv/muon_hoc_ba_goc-{DateTime.Now.ToFileTime()}.docx");

            var now = DateTime.Now;

            string tenKhoa = onlyTenKhoa(studentInfo.Faculty?.Name);

            var ngayKy = now.ToString("dd");
            var thangKy = now.Month.ToString();
            var namKy = now.Year.ToString();
            string ngayMuon = muonHocBa.NgayMuon != null ? muonHocBa.NgayMuon?.ToString("dd/MM/yyyy") : "          ";
            string ngayTra = muonHocBa.NgayTraDuKien != null ? muonHocBa.NgayTraDuKien?.ToString("dd/MM/yyyy") : "          ";

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
                        replaceTextTemplate(text, "<ho_ten>", studentInfo.Student.FulName);
                        replaceTextTemplate(text, "<mssv>", studentInfo.Student.Code);
                        replaceTextTemplate(text, "<ten_lop>", studentInfo.Student.ClassCode);
                        replaceTextTemplate(text, "<ten_khoa>", tenKhoa);
                        replaceTextTemplate(text, "<ngay_muon>", ngayMuon);
                        replaceTextTemplate(text, "<ngay_tra>", ngayTra);
                        replaceTextTemplate(text, "<thoi_gian_muon>", muonHocBa.ThoiGianMuon);
                        //replaceTextTemplate(text, "<thoi_gian_muon>", muonHocBa.ThoiGianMuon);
                        replaceTextTemplate(text, "<muc_dich>", muonHocBa.LyDo);
                        replaceTextTemplate(text, "<ngay_ky>", ngayKy);
                        replaceTextTemplate(text, "<thang_ky>", thangKy);
                        replaceTextTemplate(text, "<nam_ky>", namKy);
                    }
                    #endregion
                    mainPart.Document.Save();
                }
                await File.WriteAllBytesAsync(destination, templateStream.ToArray());
            }
            return new ExportFileOutputModel { document = null, filePath = destination };
        }

        private async Task<ExportFileOutputModel> ExportWordXinMienGiamHP(int id)
        {
            var mienGiamHP = await _xinMienGiamHocPhiRepository.FindByIdAsync(id);
            if (mienGiamHP == null)
            {
                throw new Exception("Yêu cầu không tồn tại");
            }
            var studentInfo = await _studentRepository.GetStudentDichVuInfoAsync(mienGiamHP.StudentCode);
            if (studentInfo == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }

            string filePath = _pathProvider.MapPath($"Templates/Ctsv/don_xin_mien_giam_hoc_phi.docx");
            string destination = _pathProvider.MapPath($"Templates/Ctsv/xin-mien-{DateTime.Now.ToFileTime()}.docx");

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
                        string hktt = "";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttSoNha) ? "" : $"{studentInfo.Student?.HkttSoNha}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttPho) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttPho}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttPhuong) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttPhuong}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttQuan) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttQuan}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttTinh) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttTinh}";

                        replaceTextTemplate(text, "<ten_sv>", studentInfo.Student.FulName.ToUpper());
                        replaceTextTemplate(text, "<ten_nguoi_ky>", studentInfo.Student.FulName);
                        replaceTextTemplate(text, "<ma_sv>", studentInfo.Student.Code);
                        replaceTextTemplate(text, "<lop>", studentInfo.Student.ClassCode);
                        replaceTextTemplate(text, "<khoa_ban>", studentInfo.Faculty?.Name);
                        replaceTextTemplate(text, "<khoa>", studentInfo.AcademyClass.SchoolYear);
                        replaceTextTemplate(text, "<ngay_sinh>", ngaySinh.ToString("dd/MM/yyyy"));
                        replaceTextTemplate(text, "<so_dt>", mienGiamHP.Sdt);
                        replaceTextTemplate(text, "<hktt>", hktt);
                        replaceTextTemplate(text, "<dan_toc>", studentInfo.Student.DanToc);
                        replaceTextTemplate(text, "<email>", studentInfo.Student.EmailNhaTruong);

                        string option1 = "";
                        string option2 = "";
                        string option3 = "";
                        string option4 = "";
                        string option5 = "";
                        string option6 = "";
                        string option7 = "";

                        switch (mienGiamHP.DoiTuongHuong)
                        {
                            case DoiTuongXinMienGiamHocPhi.CO_CONG_CACH_MANG:
                                option1 = "x";
                                break;
                            case DoiTuongXinMienGiamHocPhi.SV_VAN_BANG_1:
                                option2 = "x";
                                break;
                            case DoiTuongXinMienGiamHocPhi.TAN_TAT_KHO_KHAN_KINH_TE:
                                option3 = "x";
                                break;
                            case DoiTuongXinMienGiamHocPhi.DAN_TOC_HO_NGHEO:
                                option4 = "x";
                                break;
                            case DoiTuongXinMienGiamHocPhi.DAN_TOC_IT_NGUOI_VUNG_KHO_KHAN:
                                option5 = "x";
                                break;
                            case DoiTuongXinMienGiamHocPhi.DAN_TOC_VUNG_KHO_KHAN:
                                option6 = "x";
                                break;
                            case DoiTuongXinMienGiamHocPhi.CHA_ME_TAI_NAN_DUOC_TRO_CAP:
                                option7 = "x";
                                break;
                        }

                        replaceTextTemplate(text, "<option1>", option1);
                        replaceTextTemplate(text, "<option2>", option2);
                        replaceTextTemplate(text, "<option3>", option3);
                        replaceTextTemplate(text, "<option4>", option4);
                        replaceTextTemplate(text, "<option5>", option5);
                        replaceTextTemplate(text, "<option6>", option6);
                        replaceTextTemplate(text, "<option7>", option7);
                    }
                    #endregion
                    mainPart.Document.Save();
                }
                await File.WriteAllBytesAsync(destination, templateStream.ToArray());
            }
            return new ExportFileOutputModel { document = null, filePath = destination };
        }

        private async Task<ExportFileOutputModel> ExportWordDeNghiHoTroChiPhiHocTap(int id)
        {
            var deNghi = await _deNghiHoTroChiPhiRepository.FindByIdAsync(id);
            if (deNghi == null)
            {
                throw new Exception("Yêu cầu không tồn tại");
            }
            var studentInfo = await _studentRepository.GetStudentDichVuInfoAsync(deNghi.StudentCode);
            if (studentInfo == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }

            var paramSet = _thamSoDichVuService.GetParameters(DichVu.VeBus)
                                .ToDictionary(x => x.Name, x => x.Value);

            string ChucDanhNguoiKy = paramSet.ContainsKey("ChucDanhNguoiKy") ? paramSet["ChucDanhNguoiKy"] : "";
            string TenNguoiKy = paramSet.ContainsKey("TenNguoiKy") ? paramSet["TenNguoiKy"] : "";

            string filePath = _pathProvider.MapPath($"Templates/Ctsv/don_de_nghi_ho_tro_chi_phi_hoc_tap.docx");
            string destination = _pathProvider.MapPath($"Templates/Ctsv/de-nghi-{DateTime.Now.ToFileTime()}.docx");
            string newImgPath = _pathProvider.MapPath($"{studentInfo.Student.File1}");

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
                        string hktt = "";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttSoNha) ? "" : $"{studentInfo.Student?.HkttSoNha}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttPho) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttPho}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttPhuong) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttPhuong}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttQuan) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttQuan}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttTinh) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttTinh}";

                        replaceTextTemplate(text, "<ten_sv>", studentInfo.Student.FulName.ToUpper());
                        replaceTextTemplate(text, "<ten_nguoi_ky>", studentInfo.Student.FulName);
                        replaceTextTemplate(text, "<ma_sv>", studentInfo.Student.Code);
                        replaceTextTemplate(text, "<lop>", studentInfo.Student.ClassCode);
                        replaceTextTemplate(text, "<khoa_ban>", studentInfo.Faculty?.Name);
                        replaceTextTemplate(text, "<khoa>", studentInfo.AcademyClass.SchoolYear);
                        replaceTextTemplate(text, "<hktt>", hktt);
                        replaceTextTemplate(text, "<dan_toc>", studentInfo.Student.DanToc);
                        replaceTextTemplate(text, "<ngay_sinh>", ngaySinh.ToString("dd/MM/yyyy"));
                        replaceTextTemplate(text, "<so_dt>", deNghi.Sdt);
                        replaceTextTemplate(text, "<email>", studentInfo.Student.EmailNhaTruong);

                        string option1 = "";
                        string option2 = "";

                        switch (deNghi.DoiTuongHuong)
                        {
                            case DoiTuongDeNghiHoTroChiPhi.DAN_TOC_HO_NGHEO:
                                option1 = "x";
                                break;
                            case DoiTuongDeNghiHoTroChiPhi.DAN_TOC_HO_CAN_NGHEO:
                                option2 = "x";
                                break;
                        }

                        replaceTextTemplate(text, "<option1>", option1);
                        replaceTextTemplate(text, "<option2>", option2);
                    }
                    #endregion
                    mainPart.Document.Save();
                }
                await File.WriteAllBytesAsync(destination, templateStream.ToArray());
            }
            return new ExportFileOutputModel { document = null, filePath = destination };
        }

        private async Task<ExportFileOutputModel> ExportWordHoTroHocTap(int id)
        {
            var deNghi = await _hoTroHocTapRepository.FindByIdAsync(id);
            if (deNghi == null)
            {
                throw new Exception("Yêu cầu không tồn tại");
            }
            var studentInfo = await _studentRepository.GetStudentDichVuInfoAsync(deNghi.StudentCode);
            if (studentInfo == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }

            var paramSet = _thamSoDichVuService.GetParameters(DichVu.VeBus)
                                .ToDictionary(x => x.Name, x => x.Value);

            string ChucDanhNguoiKy = paramSet.ContainsKey("ChucDanhNguoiKy") ? paramSet["ChucDanhNguoiKy"] : "";
            string TenNguoiKy = paramSet.ContainsKey("TenNguoiKy") ? paramSet["TenNguoiKy"] : "";

            string filePath = _pathProvider.MapPath($"Templates/Ctsv/don_de_nghi_ho_tro_hoc_tap.docx");
            string destination = _pathProvider.MapPath($"Templates/Ctsv/de-nghi-ho-tro-hoc-tap-{DateTime.Now.ToFileTime()}.docx");

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
                        string hktt = "";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttSoNha) ? "" : $"{studentInfo.Student?.HkttSoNha}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttPho) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttPho}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttPhuong) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttPhuong}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttQuan) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttQuan}";
                        hktt += string.IsNullOrEmpty(studentInfo.Student?.HkttTinh) ? "" : $"{(hktt != "" ? "," : "")} {studentInfo.Student?.HkttTinh}";


                        replaceTextTemplate(text, "<ten_sv>", studentInfo.Student.FulName.ToUpper());
                        replaceTextTemplate(text, "<ten_nguoi_ky>", studentInfo.Student.FulName);
                        replaceTextTemplate(text, "<ma_sv>", studentInfo.Student.Code);
                        replaceTextTemplate(text, "<lop>", studentInfo.Student.ClassCode);
                        replaceTextTemplate(text, "<khoa_ban>", studentInfo.Faculty?.Name);
                        replaceTextTemplate(text, "<khoa>", studentInfo.AcademyClass.SchoolYear);
                        replaceTextTemplate(text, "<hktt>", hktt);
                        replaceTextTemplate(text, "<dan_toc>", studentInfo.Student.DanToc);
                        replaceTextTemplate(text, "<ngay_sinh>", ngaySinh.ToString("dd/MM/yyyy"));
                        replaceTextTemplate(text, "<so_dt>", studentInfo.Student.Mobile);
                        replaceTextTemplate(text, "<email>", studentInfo.Student.EmailNhaTruong);
                    }
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

        private string onlyTenKhoa(string tenKhoa)
        {
            return StringHelper.WithoutPrefix(tenKhoa, "khoa");
        }

        private string onlyTenPhuong(string phuong)
        {
            return StringHelper.WithoutPrefix(phuong, "phường,xã");
        }

        private string onlyTenQuan(string quan)
        {
            return StringHelper.WithoutPrefix(quan, "huyện,quận");
        }

        private string onlyTenTinh(string tenTinh)
        {
            return StringHelper.WithoutPrefix(tenTinh, "tỉnh,thành phố");
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
            if (oldValue == null)
            {
                oldValue = "";
            }
            if (newValue == null)
            {
                newValue = "";
            }
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

        private bool isLienThong(string Class)
        {
            return Class.Trim().StartsWith("LT");
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
