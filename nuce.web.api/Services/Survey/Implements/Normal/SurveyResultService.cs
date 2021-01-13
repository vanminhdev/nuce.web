using nuce.web.api.Services.Survey.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.Survey;
using nuce.web.api.ViewModel.Survey;
using nuce.web.api.Repositories.EduData.Interfaces;
using nuce.web.shared.Models.Survey;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Common;
using System.Text.Json;

namespace nuce.web.api.Services.Survey.Implements
{
    public class SurveyResultService : ISurveyResultService
    {
        private readonly SurveyContext _surveyContext;
        private readonly EduDataContext _eduContext;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        public SurveyResultService(SurveyContext _surveyContext, IFacultyRepository _facultyRepository, 
                IDepartmentRepository _departmentRepository, ILecturerRepository _lecturerRepository,
                EduDataContext _eduContext)
        {
            this._surveyContext = _surveyContext;
            this._facultyRepository = _facultyRepository;
            this._departmentRepository = _departmentRepository;
            this._lecturerRepository = _lecturerRepository;
            this._eduContext = _eduContext;
        }
        /// <summary>
        /// Kết quả khoa ban
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public FacultyResultModel FacultyResult(string code)
        {
            var lastDotKhaoSat = _surveyContext.AsEduSurveyDotKhaoSat.OrderByDescending(dks => dks.EndDate).FirstOrDefault();
            if (lastDotKhaoSat == null)
            {
                return null;
            }
            var dotKhaoSats = _surveyContext.AsEduSurveyDotKhaoSat.Where(o => o.EndDate == lastDotKhaoSat.EndDate).ToList();
            var baiKhaoSats = _surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == lastDotKhaoSat.Id && o.Status != (int)TheSurveyStatus.Deleted).ToList();

            var dotKhaoSatThuNhat = dotKhaoSats.FirstOrDefault(); // vì nếu các đợt là giống nhau về đề ks thì lấy thằng đầu tiên
            var baiKhaoSatCuaDotThuNhats = _surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == dotKhaoSatThuNhat.Id && o.Status != (int)TheSurveyStatus.Deleted).ToList();

            var baiKsLyThuyet = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalSubjects);
            var deLyThuyet = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsLyThuyet.NoiDungDeThi);

            var baiKsThucHanh = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalPracticalSubjects);
            var deLyThuyetThucHanh = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsThucHanh.NoiDungDeThi);

            var baiKsThucHanhThiNghiem = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.PracticalSubjects);
            var deThucHanhThiNghiem = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsThucHanhThiNghiem.NoiDungDeThi);

            var baiKsDeDoAn = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.AssignmentSubjects);
            var deDoAn = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsDeDoAn.NoiDungDeThi);

            var baiKhaoSatIds = baiKhaoSats.Select(o => o.Id).ToList();
            var baiLamKhaoSatCacDotDangXet = _surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => baiKhaoSatIds.Contains(o.BaiKhaoSatId)).ToList();


            ///////
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

            var actualDeparments = departments.Where(d => d.Code != faculty.Code).ToList();

            foreach (var department in actualDeparments)
            {
                var tmp = new SurveyResultResponseModel
                {
                    DeparmentName = department.Name,
                    DepartmentCode = department.Code,
                    SoGiangVienCanLayYKien = 0,
                    SoGiangVienDaKhaoSat = 0,
                    SoPhieuPhatRa = 0,
                    SoPhieuThuVe = 0,
                    SoSvDuocKhaoSat = 0,
                    SoSvThamGiaKhaoSat = 0,
                };

                CaculateDeparatmentResult(tmp, baiLamKhaoSatCacDotDangXet, department, (int)TheSurveyType.TheoreticalSubjects);
                CaculateDeparatmentResult(tmp, baiLamKhaoSatCacDotDangXet, department, (int)TheSurveyType.TheoreticalPracticalSubjects);
                CaculateDeparatmentResult(tmp, baiLamKhaoSatCacDotDangXet, department, (int)TheSurveyType.PracticalSubjects);
                CaculateDeparatmentResult(tmp, baiLamKhaoSatCacDotDangXet, department, (int)TheSurveyType.AssignmentSubjects);
                
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

        public void CaculateDeparatmentResult(SurveyResultResponseModel Result, List<AsEduSurveyBaiKhaoSatSinhVien> baiLamKhaoSatCacDotDangXet, AsAcademyDepartment department, int loaiMon)
        {
            Result.DeparmentName = department.Name;
            Result.DepartmentCode = department.Code;
            //lấy môn học của bộ môn
            var monHocCuaBoMon = _eduContext.AsAcademySubject.Where(o => o.DepartmentCode == department.Code)
                .Join(_eduContext.AsAcademySubjectExtend, o => o.Code, o => o.Code, (monhoc, loaimon) => new { monhoc, loaimon.Type })
                .Select(o => new
                {
                    o.monhoc.Code,
                    o.monhoc.DepartmentCode,
                    o.monhoc.Name,
                    o.Type
                }).ToList();

            #region môn
            var monLyThuyetCuaBoMonCodes = monHocCuaBoMon.ToList().Where(o => o.Type == loaiMon).Select(o => o.Code).ToList();

            //các bài làm của môn lý thuyết của bộ môn đang xét
            var baiLamKhaoSatLyThuyet = baiLamKhaoSatCacDotDangXet.ToList().Where(o => monLyThuyetCuaBoMonCodes.Contains(o.SubjectCode));
            var baiLamKhaoSatLyThuyetHoanThanh = baiLamKhaoSatLyThuyet.ToList().Where(o => o.Status == (int)SurveyStudentStatus.Done)
                                                        .Select(o => new { o.StudentCode, o.LecturerCode });

            var soPhieuPhatRa = baiLamKhaoSatLyThuyet.Count();
            var soPhieuThuVe = baiLamKhaoSatLyThuyetHoanThanh.Count();

            var soSVThamGiaKhaoSat = baiLamKhaoSatLyThuyet.GroupBy(o => o.StudentCode).Select(o => o.Key).Count();
            var soSVDuocKhaoSat = baiLamKhaoSatLyThuyetHoanThanh.GroupBy(o => o.StudentCode).Select(o => o.Key).Count();

            var soGiangVienCanKs = baiLamKhaoSatLyThuyet.GroupBy(o => o.LecturerCode).Select(o => o.Key).Count();
            var soGiangVienDaDuocKs = baiLamKhaoSatLyThuyetHoanThanh.GroupBy(o => o.LecturerCode).Select(o => o.Key).Count();

            Result.SoPhieuPhatRa += soPhieuPhatRa;
            Result.SoPhieuThuVe += soPhieuThuVe;
            Result.SoSvThamGiaKhaoSat += soSVThamGiaKhaoSat;
            Result.SoSvDuocKhaoSat += soSVDuocKhaoSat;
            Result.SoGiangVienCanLayYKien += soGiangVienCanKs;
            Result.SoGiangVienDaKhaoSat += soGiangVienDaDuocKs;
            #endregion
        }
    }
}
