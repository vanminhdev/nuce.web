﻿using nuce.web.api.Services.Survey.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Survey;
using nuce.web.api.Repositories.EduData.Interfaces;
using nuce.web.shared.Models.Survey;

namespace nuce.web.api.Services.Survey.Implements
{
    public class SurveyResultService : ISurveyResultService
    {
        private readonly SurveyContext _surveyContext;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        public SurveyResultService(SurveyContext _surveyContext, IFacultyRepository _facultyRepository, IDepartmentRepository _departmentRepository, ILecturerRepository _lecturerRepository)
        {
            this._surveyContext = _surveyContext;
            this._facultyRepository = _facultyRepository;
            this._departmentRepository = _departmentRepository;
            this._lecturerRepository = _lecturerRepository;
        }
        /// <summary>
        /// Kết quả khoa ban
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public FacultyResultModel FacultyResult(string code)
        {
            var departments = _facultyRepository.GetDepartment(code);
            var faculty = departments.FirstOrDefault(d => d.Code == code);

            var result = new FacultyResultModel
            {
                FacultyCode = faculty.Code,
                FacultyName = faculty.Name,
                Result = new List<SurveyResultResponseModel>(),
            };

            var total = new SurveyResultResponseModel
            {
                IsTotal = true,
            };

            foreach (var department in departments.Where(d => d.Code !=  faculty.Code))
            {
                var tmp = new SurveyResultResponseModel
                {
                    DeparmentName = department.Name,
                    DepartmentCode = department.Code,
                    SoGiangVienCanLayYKien = 5,
                    SoGiangVienDaKhaoSat = 23,
                    SoPhieuPhatRa = 50,
                    SoPhieuThuVe = 20,
                    SoSvDuocKhaoSat = 100,
                    SoSvThamGiaKhaoSat = 10,
                };
                total.SoGiangVienCanLayYKien += tmp.SoGiangVienCanLayYKien;
                total.SoGiangVienDaKhaoSat += tmp.SoGiangVienDaKhaoSat;
                total.SoPhieuPhatRa += tmp.SoGiangVienCanLayYKien;
                total.SoPhieuThuVe += tmp.SoPhieuThuVe;
                total.SoSvDuocKhaoSat += tmp.SoSvDuocKhaoSat;
                total.SoSvThamGiaKhaoSat += tmp.SoSvThamGiaKhaoSat;
                result.Result.Add(tmp);
            }

            result.Result.Add(total);

            return result;
        }
        /// <summary>
        /// Kết quả bộ môn
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<DepartmentResultModel> DepartmentResultAsync(string code)
        {
            var department = await _departmentRepository.FindByCode(code);
            var faculty = await _facultyRepository.FindByCode(department.FacultyCode);
            var lecturers = _departmentRepository.GetLecturer(code);
            var result = new DepartmentResultModel
            {
                FacultyCode = faculty.Code,
                FacultyName = faculty.Name,
                DepartmentCode = department.Code,
                DepartmentName = department.Name,
                Result = new List<SurveyResultResponseModel>(),
            };

            var total = new SurveyResultResponseModel
            {
                IsTotal = true,
            };

            foreach (var lecturer in lecturers.Where(d => d.Code != faculty.Code))
            {
                var tmp = new SurveyResultResponseModel
                {
                    DeparmentName = department.Name,
                    DepartmentCode = department.Code,
                    LecturerCode = lecturer.Code,
                    LecturerName = lecturer.FullName,
                    SoGiangVienCanLayYKien = 5,
                    SoGiangVienDaKhaoSat = 23,
                    SoPhieuPhatRa = 50,
                    SoPhieuThuVe = 20,
                    SoSvDuocKhaoSat = 100,
                    SoSvThamGiaKhaoSat = 10,
                };
                total.SoGiangVienCanLayYKien += tmp.SoGiangVienCanLayYKien;
                total.SoGiangVienDaKhaoSat += tmp.SoGiangVienDaKhaoSat;
                total.SoPhieuPhatRa += tmp.SoGiangVienCanLayYKien;
                total.SoPhieuThuVe += tmp.SoPhieuThuVe;
                total.SoSvDuocKhaoSat += tmp.SoSvDuocKhaoSat;
                total.SoSvThamGiaKhaoSat += tmp.SoSvThamGiaKhaoSat;

                result.Result.Add(tmp);
            }

            result.Result.Add(total);

            return result;
        }
        public async Task<SurveyResultResponseModel> LecturerResultAsync(string code)
        {
            var lecturer = await _lecturerRepository.FindByCode(code);
            var department = await _departmentRepository.FindByCode(lecturer.DepartmentCode);
            var faculty = await _facultyRepository.FindByCode(department.FacultyCode);


            var result = new SurveyResultResponseModel
            {
                FacultyCode = faculty.Code,
                FacultyName = faculty.Name,
                DepartmentCode = department.Code,
                DeparmentName = department.Name,
                LecturerCode = lecturer.Code,
                LecturerName = lecturer.FullName,
                SoGiangVienCanLayYKien = 5,
                SoGiangVienDaKhaoSat = 23,
                SoPhieuPhatRa = 50,
                SoPhieuThuVe = 20,
                SoSvDuocKhaoSat = 100,
                SoSvThamGiaKhaoSat = 10,
                DeMuc = new List<DeMuc>
                {
                    new DeMuc {
                        Title = "NỘI DUNG VÀ LỊCH TRÌNH GIẢNG DẠY",
                        CauHoi = new List<ChiTietCauHoi>
                        {
                            new ChiTietCauHoi
                            {
                                Content = "Câu 1: Nội dung giảng dạy, mục tiêu của học phần, vị trí học phần trong chương trình đào tạo, đề cương chi tiết của học phần được giảng viên giới thiệu đầy đủ rõ ràng khi bắt đầu học phần.",
                            },
                            new ChiTietCauHoi
                            {
                                Content = "Câu 2: Các quy định và kế hoạch kiểm tra, thực hiện đánh giá điểm quá trình được giảng viên phổ biến rõ ràng, đầy đủ khi bắt đầu học phần.",
                            },
                            new ChiTietCauHoi
                            {
                                Content = "Câu 3: Giáo trình chính thức và tài liệu tham khảo được giảng viên giời thiệu chi tiết, đầy đủ; giảng viên hỗ trợ sinh viên trong việc tìm tài liệu học tập một cách hiệu quả",
                            },
                            new ChiTietCauHoi
                            {
                                Content = "Câu 1: Nội dung giảng dạy, mục tiêu của học phần, vị trí học phần trong chương trình đào tạo, đề cương chi tiết của học phần được giảng viên giới thiệu đầy đủ rõ ràng khi bắt đầu học phần.",
                            },
                            new ChiTietCauHoi
                            {
                                Content = "Câu 1: Nội dung giảng dạy, mục tiêu của học phần, vị trí học phần trong chương trình đào tạo, đề cương chi tiết của học phần được giảng viên giới thiệu đầy đủ rõ ràng khi bắt đầu học phần.",
                            },
                            new ChiTietCauHoi
                            {
                                Content = "Câu 1: Nội dung giảng dạy, mục tiêu của học phần, vị trí học phần trong chương trình đào tạo, đề cương chi tiết của học phần được giảng viên giới thiệu đầy đủ rõ ràng khi bắt đầu học phần.",
                            },
                                }
                            },
                    new DeMuc {
                        Title = "PHƯƠNG PHÁP GIẢNG DẠY CỦA GIẢNG VIÊN",
                        CauHoi = new List<ChiTietCauHoi>
                        {
                            new ChiTietCauHoi
                            {
                                Content = "Câu 1: Nội dung giảng dạy, mục tiêu của học phần, vị trí học phần trong chương trình đào tạo, đề cương chi tiết của học phần được giảng viên giới thiệu đầy đủ rõ ràng khi bắt đầu học phần.",
                            },
                            new ChiTietCauHoi
                            {
                                Content = "Câu 2: Các quy định và kế hoạch kiểm tra, thực hiện đánh giá điểm quá trình được giảng viên phổ biến rõ ràng, đầy đủ khi bắt đầu học phần.",
                            },
                            new ChiTietCauHoi
                            {
                                Content = "Câu 3: Giáo trình chính thức và tài liệu tham khảo được giảng viên giời thiệu chi tiết, đầy đủ; giảng viên hỗ trợ sinh viên trong việc tìm tài liệu học tập một cách hiệu quả",
                            },
                            new ChiTietCauHoi
                            {
                                Content = "Câu 1: Nội dung giảng dạy, mục tiêu của học phần, vị trí học phần trong chương trình đào tạo, đề cương chi tiết của học phần được giảng viên giới thiệu đầy đủ rõ ràng khi bắt đầu học phần.",
                            },
                            new ChiTietCauHoi
                            {
                                Content = "Câu 1: Nội dung giảng dạy, mục tiêu của học phần, vị trí học phần trong chương trình đào tạo, đề cương chi tiết của học phần được giảng viên giới thiệu đầy đủ rõ ràng khi bắt đầu học phần.",
                            },
                            new ChiTietCauHoi
                            {
                                Content = "Câu 1: Nội dung giảng dạy, mục tiêu của học phần, vị trí học phần trong chương trình đào tạo, đề cương chi tiết của học phần được giảng viên giới thiệu đầy đủ rõ ràng khi bắt đầu học phần.",
                            },
                                }
                            },

                },
                
            };

            return result;
        }
    }
}
