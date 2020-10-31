using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class StudentService : IStudentService
    {
        #region declare
        private readonly IStudentRepository _studentRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuaTrinhHocRepository _quaTrinhHocRepository;
        private readonly IGiaDinhRepository _giaDinhRepository;
        private readonly IThiHsgRepository _thiHsgRepository;
        private readonly IParameterService _paramService;
        private readonly ILogger<StudentService> _logger;
        public StudentService(
            IStudentRepository _studentRepository,
            IUserService _userService, IUnitOfWork _unitOfWork,
            IQuaTrinhHocRepository _quaTrinhHocRepository,
            IThiHsgRepository _thiHsgRepository,
            IGiaDinhRepository _giaDinhRepository,
            IParameterService _paramService,
            ILogger<StudentService> _logger
        )
        {
            this._studentRepository = _studentRepository;
            this._unitOfWork = _unitOfWork;
            this._userService = _userService;
            this._quaTrinhHocRepository = _quaTrinhHocRepository;
            this._thiHsgRepository = _thiHsgRepository;
            this._giaDinhRepository = _giaDinhRepository;
            this._paramService = _paramService;
            this._logger = _logger;
        }
        #endregion

        public async Task<FullStudentModel> GetFullStudentByCode(string studentCode)
        {
            var quaTrinhHoc = await _quaTrinhHocRepository.FindByCodeAsync(studentCode);
            var giaDinh = await _giaDinhRepository.FindByCodeAsync(studentCode);
            var thiHsg = await _thiHsgRepository.FindByCodeAsync(studentCode);
            var student = _studentRepository.FindByCode(studentCode);
            return new FullStudentModel
            {
                Student = student,
                ThiHSG = thiHsg,
                GiaDinh = giaDinh,
                QuaTrinhHoc = quaTrinhHoc
            };
        }

        public AsAcademyStudent GetStudentByCode(string studentCode)
        {
            return _studentRepository.FindByCode(studentCode);
        }

        public StudentAllowUpdateModel GetStudentByCodeAllowUpdate(string studentCode)
        {
            return new StudentAllowUpdateModel
            {
                Student = GetStudentByCode(studentCode),
                Enabled = _paramService.isCapNhatHoSo(),
            };
        }

        public async Task<ResponseBody> UpdateStudent(AsAcademyStudent student)
        {
            var currentStudent = _userService.GetCurrentStudent();

            currentStudent.HkttPhuong = student.HkttPhuong;
            currentStudent.HkttPho = student.HkttPho;
            currentStudent.HkttQuan = student.HkttQuan;
            currentStudent.HkttTinh = student.HkttTinh;
            currentStudent.HkttSoNha = student.HkttSoNha;
            currentStudent.Cmt = student.Cmt;
            currentStudent.CmtNgayCap = student.CmtNgayCap;
            currentStudent.CmtNoiCap = student.CmtNoiCap;
            currentStudent.LaNoiTru = student.LaNoiTru;
            currentStudent.Mobile = student.Mobile;
            currentStudent.Email = student.Email;
            currentStudent.EmailNhaTruong = student.EmailNhaTruong;

            try
            {
                _studentRepository.Update(currentStudent);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Cập nhật thông tin SV", $"{ex.ToString()} \n", JsonConvert.SerializeObject(student));
                return new ResponseBody { Message = "Lỗi hệ thống" };
            }
            return null;
        }

        public async Task<ResponseBody> UpdateStudentBasic(StudentUpdateModel basicStudent)
        {
            if (!Common.Validate.IsMobile(basicStudent.Mobile.Trim()))
            {
                return new ResponseBody { Message = "Mobile sai quy cách" };
            } else if (!Common.Validate.IsMobile(basicStudent.MobileBaoTin.Trim()))
            {
                return new ResponseBody { Message = "Số điện thoại người nhận sai quy cách" };
            } else if (!Common.Validate.IsValidEmail(basicStudent.Email.Trim()))
            {
                return new ResponseBody { Message = "Email sai quy cách" };
            } else if (!Common.Validate.IsValidEmail(basicStudent.EmailBaoTin.Trim()))
            {
                return new ResponseBody { Message = "Email người nhận sai quy cách" };
            }

            var student = _userService.GetCurrentStudent();
            student.Email = basicStudent.Email.Trim();
            student.Mobile = basicStudent.Mobile.Trim();
            student.BaoTinDiaChi = basicStudent.DiaChiBaotin.Trim();
            student.BaoTinHoVaTen = basicStudent.HoTenBaoTin.Trim();
            student.BaoTinEmail = basicStudent.EmailBaoTin.Trim();
            student.BaoTinSoDienThoai = basicStudent.MobileBaoTin.Trim();
            student.BaoTinDiaChiNguoiNhan = basicStudent.DiaChiNguoiNhanBaotin.Trim();
            student.LaNoiTru = basicStudent.CoNoiOCuThe;
            student.DiaChiCuThe = basicStudent.DiaChiCuThe.Trim();

            student.HkttPhuong = basicStudent.PhuongXa.Trim();
            student.HkttQuan = basicStudent.QuanHuyen.Trim();
            student.HkttTinh = basicStudent.TinhThanhPho.Trim();

            try
            {
                _studentRepository.Update(student);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Cập nhật hồ sơ SV", $"{ex.ToString()} \n", JsonConvert.SerializeObject(student));
                return new ResponseBody { Message = "Lỗi hệ thống", Data = ex };
            }

            return null;
        }
    }
}
