﻿using nuce.web.api.Models.Ctsv;
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
        private readonly IStudentRepository _studentRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public StudentService(IStudentRepository _studentRepository,
            IUserService _userService, IUnitOfWork _unitOfWork
        )
        {
            this._studentRepository = _studentRepository;
            this._unitOfWork = _unitOfWork;
            this._userService = _userService;
        }

        public AsAcademyStudent GetStudentByCode(string studentCode)
        {
            return _studentRepository.FindByCode(studentCode);
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
                return new ResponseBody { Message = "Lỗi hệ thống" };
            }
            return null;
        }

        public async Task<ResponseBody> UpdateStudentBasic(StudentModel basicStudent)
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
            student.LaNoiTru = !basicStudent.CoNoiOCuThe;
            student.DiaChiCuThe = basicStudent.DiaChiCuThe.Trim();

            _studentRepository.Update(student);
            await _unitOfWork.SaveAsync();

            return null;
        }
    }
}
