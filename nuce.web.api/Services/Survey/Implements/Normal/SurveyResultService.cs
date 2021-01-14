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
using Microsoft.EntityFrameworkCore;

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
        public async Task<FacultyResultModel> FacultyResultAsync(string code)
        {
            #region setup
            var lastDotKhaoSat = _surveyContext.AsEduSurveyDotKhaoSat.OrderByDescending(dks => dks.EndDate).FirstOrDefault();
            if (lastDotKhaoSat == null)
            {
                return null;
            }
            var baiKhaoSats = _surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == lastDotKhaoSat.Id && o.Status != (int)TheSurveyStatus.Deleted).ToList();
            
            var baiKhaoSatIds = baiKhaoSats.Select(o => o.Id).ToList();
            var baiLamKhaoSatCacDotDangXet = _surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => baiKhaoSatIds.Contains(o.BaiKhaoSatId)).ToList();
            #endregion

            var departments = await _facultyRepository.GetDepartment(code).ToListAsync();
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

            foreach (var department in departments)
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

                CalculateDeparatmentResult(tmp, baiLamKhaoSatCacDotDangXet, department, 1);
                CalculateDeparatmentResult(tmp, baiLamKhaoSatCacDotDangXet, department, 2);
                CalculateDeparatmentResult(tmp, baiLamKhaoSatCacDotDangXet, department, 3);
                CalculateDeparatmentResult(tmp, baiLamKhaoSatCacDotDangXet, department, 4);

                total.SoGiangVienCanLayYKien += tmp.SoGiangVienCanLayYKien;
                total.SoGiangVienDaKhaoSat += tmp.SoGiangVienDaKhaoSat;
                total.SoPhieuPhatRa += tmp.SoPhieuPhatRa;
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
            #region setup
            var lastDotKhaoSat = await _surveyContext.AsEduSurveyDotKhaoSat.OrderByDescending(dks => dks.EndDate).FirstOrDefaultAsync();
            if (lastDotKhaoSat == null)
            {
                return null;
            }
            var baiKhaoSats = await _surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == lastDotKhaoSat.Id && o.Status != (int)TheSurveyStatus.Deleted).ToListAsync();

            var baiKhaoSatIds = baiKhaoSats.Select(o => o.Id).ToList();
            var baiLamKhaoSatCacDotDangXet = await _surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => baiKhaoSatIds.Contains(o.BaiKhaoSatId)).ToListAsync();
            #endregion

            var department = await _departmentRepository.FindByCode(code);
            var faculty = await _facultyRepository.FindByCode(department.FacultyCode);
            var lecturers = await _departmentRepository.GetLecturer(code).ToListAsync();
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

            //var lecturers = lecturers.Where(d => d.Code != faculty.Code);

            foreach (var lecturer in lecturers)
            {
                var tmp = new SurveyResultResponseModel
                {
                    DeparmentName = department.Name,
                    DepartmentCode = department.Code,
                    LecturerCode = lecturer.Code,
                    LecturerName = lecturer.FullName,
                    SoGiangVienCanLayYKien = 0,
                    SoGiangVienDaKhaoSat = 0,
                    SoPhieuPhatRa = 0,
                    SoPhieuThuVe = 0,
                    SoSvDuocKhaoSat = 0,
                    SoSvThamGiaKhaoSat = 0,
                };
                await CalculateLecturerResult(tmp, lecturer, baiLamKhaoSatCacDotDangXet, 1);
                await CalculateLecturerResult(tmp, lecturer, baiLamKhaoSatCacDotDangXet, 2);
                await CalculateLecturerResult(tmp, lecturer, baiLamKhaoSatCacDotDangXet, 3);
                await CalculateLecturerResult(tmp, lecturer, baiLamKhaoSatCacDotDangXet, 4);

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
        /// Kết quả giảng viên
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<SurveyResultResponseModel> LecturerResultAsync(string code)
        {
            #region setup
            var lastDotKhaoSat = _surveyContext.AsEduSurveyDotKhaoSat.OrderByDescending(dks => dks.EndDate).FirstOrDefault();
            var dotKhaoSats = _surveyContext.AsEduSurveyDotKhaoSat.Where(o => o.EndDate == lastDotKhaoSat.EndDate);
            if (lastDotKhaoSat == null)
            {
                return null;
            }
            var baiKhaoSats = _surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == lastDotKhaoSat.Id && o.Status != (int)TheSurveyStatus.Deleted);

            var dotKhaoSatThuNhat = dotKhaoSats.FirstOrDefault(); // vì nếu các đợt là giống nhau về đề ks thì lấy thằng đầu tiên
            var baiKhaoSatCuaDotThuNhats = _surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == dotKhaoSatThuNhat.Id && o.Status != (int)TheSurveyStatus.Deleted);

            var baiKsLyThuyet = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalSubjects);
            var deLyThuyet = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsLyThuyet.NoiDungDeThi);

            var baiKsThucHanh = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalPracticalSubjects);
            var deLyThuyetThucHanh = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsThucHanh.NoiDungDeThi);

            var baiKsThucHanhThiNghiem = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.PracticalSubjects);
            var deThucHanhThiNghiem = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsThucHanhThiNghiem.NoiDungDeThi);

            var baiKsDeDoAn = baiKhaoSatCuaDotThuNhats.FirstOrDefault(o => o.Type == (int)TheSurveyType.AssignmentSubjects);
            var deDoAn = JsonSerializer.Deserialize<List<QuestionJson>>(baiKsDeDoAn.NoiDungDeThi);
            #endregion

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
                LoaiMonHoc = new List<ChiTietLoaiMonHoc>(),
            };

            CalculateDetailResult(result, lecturer, deLyThuyet, baiKhaoSats, (int)TheSurveyType.TheoreticalSubjects);
            CalculateDetailResult(result, lecturer, deLyThuyetThucHanh, baiKhaoSats, (int)TheSurveyType.TheoreticalPracticalSubjects);
            CalculateDetailResult(result, lecturer, deThucHanhThiNghiem, baiKhaoSats, (int)TheSurveyType.PracticalSubjects);
            CalculateDetailResult(result, lecturer, deDoAn, baiKhaoSats, (int)TheSurveyType.AssignmentSubjects);

            return result;
        }

        #region private
        /// <summary>
        /// Tính kết quả các loại số phiếu của bộ môn vào Result
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="baiLamKhaoSatCacDotDangXet"></param>
        /// <param name="department"></param>
        /// <param name="loaiMon"></param>
        private void CalculateDeparatmentResult(SurveyResultResponseModel Result, List<AsEduSurveyBaiKhaoSatSinhVien> baiLamKhaoSatCacDotDangXet, AsAcademyDepartment department, int loaiMon = 1)
        {
            var tmpLoaiMon = Enum.GetValues(typeof(TheSurveyType)).Cast<int>().ToList();
            var danhSachLoaiMonHoc = tmpLoaiMon.Cast<int?>().ToList();
            List<int?> ss = new List<int?> { 1, 2, 3, 4 };
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
        /// <summary>
        /// Tính kết quả câu hỏi của giảng viên vào Result
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="lecturer"></param>
        /// <param name="deLyThuyet"></param>
        /// <param name="baiKhaoSats"></param>
        /// <param name="loaiMon"></param>
        private void CalculateDetailResult(SurveyResultResponseModel Result, AsAcademyLecturer lecturer, List<QuestionJson> deLyThuyet, IQueryable<AsEduSurveyBaiKhaoSat> baiKhaoSats, int loaiMon)
        {
            #region tổng hợp dữ liệu thống kê thô

            string saCode = QuestionType.SA;

            var theSurveyIdLyThuyets = baiKhaoSats.Where(o => o.Type == loaiMon).Select(o => o.Id).ToList();
            var reportTotalLyThuyet = _surveyContext.AsEduSurveyReportTotal.Where(o => theSurveyIdLyThuyets.Contains(o.TheSurveyId));
            var index = 0;

            List<ChiTietCauHoi> chiTietCauHoiList = new List<ChiTietCauHoi>();
            foreach (var cauhoi in deLyThuyet)
            {
                var chiTietCauHoi = new ChiTietCauHoi();
                if (cauhoi.Type == QuestionType.SC)
                { 
                    chiTietCauHoi.Content = $"Câu {++index}: {cauhoi.Content}";
                    Dictionary<string, string> diemList = new Dictionary<string, string>();

                    var dTB = 0;
                    var sumTotal = 0;
                    var diem = 0;
                    foreach (var dapan in cauhoi.Answers)
                    {
                        diem++;
                        string diemKey = diem.ToString();
                        diemList.Add(diemKey, "0");

                        var ketqua = reportTotalLyThuyet.FirstOrDefault(o => o.LecturerCode == lecturer.Code && 
                                                                                o.QuestionCode == cauhoi.Code && 
                                                                                o.AnswerCode == dapan.Code);
                        if (ketqua != null)
                        {
                            var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                            sumTotal += total;

                            dTB += diem * total;
                            diemList[diemKey] = total.ToString();
                        }
                        else
                        {
                            diemList[diemKey] = "0";
                        }
                    }
                    string dtbKey = "đTB";
                    diemList.Add(dtbKey, "0");

                    if (sumTotal > 0)
                    {
                        string dtbValue = ((double)dTB / sumTotal).ToString();
                        diemList[dtbKey] = dtbValue;
                    }
                    chiTietCauHoi.DanhSachDiem = diemList;
                    chiTietCauHoiList.Add(chiTietCauHoi);
                }
                else if (cauhoi.Type == QuestionType.MC)
                {
                    chiTietCauHoi.Content = $"Câu {++index}: {cauhoi.Content}";
                    Dictionary<string, string> diemList = new Dictionary<string, string>();

                    foreach (var dapan in cauhoi.Answers)
                    {
                        //wsLyThuyet.Cells[rowDapAn, col].Value = dapan.Content;
                        string dapAnKey = dapan.Content;
                        string dapAnValue = "";
                        var ketqua = reportTotalLyThuyet.FirstOrDefault(o => o.LecturerCode == lecturer.Code && 
                                                                                o.QuestionCode == cauhoi.Code && 
                                                                                o.AnswerCode == dapan.Code);
                        if (ketqua != null)
                        {
                            var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                            dapAnValue = total.ToString();
                        }
                        else
                        {
                            dapAnValue = "0";
                        }

                        //câu hỏi con của đáp án
                        if (dapan.AnswerChildQuestion != null)
                        {
                            var ketquaCon = reportTotalLyThuyet.FirstOrDefault(o => o.QuestionCode == $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}" && o.AnswerCode == dapan.Code);
                            dapAnValue = ketquaCon?.Content ?? "";
                        }

                        if (!string.IsNullOrEmpty(dapAnKey))
                        {
                            diemList.Add(dapAnKey, dapAnValue);
                        }
                    }
                    chiTietCauHoi.DanhSachDiem = diemList;
                    chiTietCauHoiList.Add(chiTietCauHoi);
                }
                else if (cauhoi.Type == QuestionType.SA)
                {
                    chiTietCauHoi.Content = $"Câu {++index}: {cauhoi.Content}";
                    Dictionary<string, string> diemList = new Dictionary<string, string>();

                    var ketqua = reportTotalLyThuyet.FirstOrDefault(o => o.LecturerCode == lecturer.Code && o.QuestionCode == cauhoi.Code);
                    if (ketqua != null && ketqua.Content != null)
                    {
                        var str = "";
                        var listStr = JsonSerializer.Deserialize<List<string>>(ketqua.Content);
                        listStr.ForEach(s =>
                        {
                            str += $"{s};";
                        });
                        diemList.Add(saCode, str);
                    }
                    chiTietCauHoi.DanhSachDiem = diemList;
                    chiTietCauHoiList.Add(chiTietCauHoi);
                }
                else if (cauhoi.Type == QuestionType.GQ)
                {
                    index++;
                    var indexChild = 0;
                    foreach (var cauhoicon in cauhoi.ChildQuestion)
                    {
                        if (cauhoicon.Type == QuestionType.SC)
                        {
                            chiTietCauHoi.Content = $"Câu {index}.{++indexChild}: {cauhoicon.Content}";
                            Dictionary<string, string> diemList = new Dictionary<string, string>();

                            var dTB = 0;
                            var sumTotal = 0;
                            var diem = 0;
                            //var colTotal = col + cauhoicon.Answers.Count();
                            foreach (var dapan in cauhoicon.Answers)
                            {
                                diem++;
                                string diemKey = diem.ToString();
                                diemList.Add(diemKey, "0");

                                var ketqua = reportTotalLyThuyet.FirstOrDefault(o => o.LecturerCode == lecturer.Code && 
                                                                                        o.QuestionCode == cauhoicon.Code && 
                                                                                        o.AnswerCode == dapan.Code);
                                if (ketqua != null)
                                {
                                    var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                    diemList[diemKey] = total.ToString();

                                    sumTotal += total;
                                    dTB += diem * total;
                                }
                                else
                                {
                                    diemList[diemKey] = "0";
                                }
                            }
                            string dtbKey = "đTB";
                            diemList.Add(dtbKey, "0");

                            if (sumTotal > 0)
                            {
                                string dtbValue = ((double)dTB / sumTotal).ToString();
                                diemList[dtbKey] = dtbValue;
                            }
                            chiTietCauHoi.DanhSachDiem = diemList;
                            chiTietCauHoiList.Add(chiTietCauHoi);
                        }
                        else if (cauhoicon.Type == QuestionType.MC)
                        {
                            chiTietCauHoi.Content = $"Câu {++index}: {cauhoicon.Content}";
                            Dictionary<string, string> diemList = new Dictionary<string, string>();

                            foreach (var dapan in cauhoicon.Answers)
                            {
                                string dapanKey = dapan.Content;
                                string dapAnValue = "";
                                var ketqua = reportTotalLyThuyet.FirstOrDefault(o => o.LecturerCode == lecturer.Code && o.QuestionCode == cauhoicon.Code && o.AnswerCode == dapan.Code);
                                if (ketqua != null)
                                {
                                    var total = ketqua.Total != null ? ketqua.Total.Value : 0;
                                    dapAnValue = total.ToString();
                                }
                                else
                                {
                                    dapAnValue = "0";
                                }

                                //câu hỏi con của đáp án
                                if (dapan.AnswerChildQuestion != null)
                                {
                                    var ketquaCon = reportTotalLyThuyet.FirstOrDefault(o => o.QuestionCode == $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}" && o.AnswerCode == dapan.Code);
                                    dapAnValue = ketquaCon?.Content ?? "";
                                }
                            }
                            chiTietCauHoi.DanhSachDiem = diemList;
                            chiTietCauHoiList.Add(chiTietCauHoi);
                        }
                        if (cauhoicon.Type == QuestionType.SA)
                        {
                            chiTietCauHoi.Content = $"Câu {index}.{++indexChild}: {cauhoicon.Content}";
                            Dictionary<string, string> diemList = new Dictionary<string, string>();

                            var ketqua = reportTotalLyThuyet.FirstOrDefault(o => o.LecturerCode == lecturer.Code && o.QuestionCode == cauhoicon.Code);
                            if (ketqua != null && ketqua.Content != null)
                            {
                                var str = "";
                                var listStr = JsonSerializer.Deserialize<List<string>>(ketqua.Content);
                                listStr.ForEach(s =>
                                {
                                    str += $"{s};";
                                });
                                diemList.Add(saCode, str);
                            }

                            chiTietCauHoi.DanhSachDiem = diemList;
                            chiTietCauHoiList.Add(chiTietCauHoi);
                        }
                    }
                    //chiTietCauHoi.Content = "group choice";
                    //chiTietCauHoiList.Add(chiTietCauHoi);
                }
            }

            string loaiMonHoc = "";
            switch ((TheSurveyType)loaiMon)
            {
                case TheSurveyType.TheoreticalSubjects:
                    loaiMonHoc = "Lý thuyết";
                    break;
                case TheSurveyType.TheoreticalPracticalSubjects:
                    loaiMonHoc = "Lý thuyết thực hành";
                    break;
                case TheSurveyType.PracticalSubjects:
                    loaiMonHoc = "Thực hành";
                    break;
                case TheSurveyType.AssignmentSubjects:
                    loaiMonHoc = "Đồ án";
                    break;
                default:
                    break;
            }

            Result.LoaiMonHoc.Add(new ChiTietLoaiMonHoc
            {
                Title = loaiMonHoc,
                CauHoi = chiTietCauHoiList
            });
            #endregion
        }
        // subject <= classroom <= lecturer classroom <= lecturer
        private async Task CalculateLecturerResult(SurveyResultResponseModel Result, AsAcademyLecturer lecturer, List<AsEduSurveyBaiKhaoSatSinhVien> baiLamKhaoSatCacDotDangXet, int loaiMon = 1)
        {
            Result.LecturerCode = lecturer.Code;
            Result.LecturerName = lecturer.FullName;
            var tmpLoaiMon = Enum.GetValues(typeof(TheSurveyType)).Cast<int>().ToList();
            var danhSachLoaiMonHoc = tmpLoaiMon.Cast<int?>().ToList();

            var monHocCuaGiangVien = await _eduContext.AsAcademyLecturerClassRoom
                                                .Where(lcr => lcr.LecturerCode == lecturer.Code)
                                                .Join(_eduContext.AsAcademyClassRoom,
                                                        lcr => lcr.ClassRoomCode,
                                                        cr => cr.Code,
                                                        (lcr, cr) => cr)
                                                .Distinct()
                                                .Join(_eduContext.AsAcademySubject,
                                                    cr => cr.SubjectCode,
                                                    s => s.Code,
                                                    (cr, s) => s)
                                                .Join(_eduContext.AsAcademySubjectExtend,
                                                    s => s.Code,
                                                    se => se.Code,
                                                    (s, se) => new
                                                    {
                                                        s,
                                                        se.Type
                                                    })
                                                .Select(subject => new
                                                {
                                                    subject.s.Code,
                                                    subject.s.DepartmentCode,
                                                    subject.s.Name,
                                                    subject.Type
                                                }).ToListAsync();


            var monLyThuyetCuaBoMonCodes = monHocCuaGiangVien.Where(o => loaiMon == o.Type)
                                                        .Select(o => o.Code);

            //các bài làm của môn lý thuyết của bộ môn đang xét
            var baiLamKhaoSatLyThuyet = baiLamKhaoSatCacDotDangXet.Where(o => o.LecturerCode == lecturer.Code && monLyThuyetCuaBoMonCodes.Contains(o.SubjectCode));
            var baiLamKhaoSatLyThuyetHoanThanh = baiLamKhaoSatLyThuyet.Where(o => o.Status == (int)SurveyStudentStatus.Done)
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
        }
        #endregion
    }
}
