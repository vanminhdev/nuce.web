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
using Microsoft.Extensions.Logging;

namespace nuce.web.api.Services.Survey.Implements
{
    public class SurveyResultService
    {
        private readonly ILogger<SurveyResultService> _logger;
        private readonly SurveyContext _surveyContext;
        private readonly EduDataContext _eduContext;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        public SurveyResultService(ILogger<SurveyResultService> _logger, SurveyContext _surveyContext, IFacultyRepository _facultyRepository, 
                IDepartmentRepository _departmentRepository, ILecturerRepository _lecturerRepository,
                EduDataContext _eduContext)
        {
            this._logger = _logger;
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
            var lastDotKhaoSat = _surveyContext.AsEduSurveyDotKhaoSat
                .OrderByDescending(dks => dks.EndDate)
                .FirstOrDefault(dks => dks.Status != (int)SurveyRoundStatus.Deleted);
            if (lastDotKhaoSat == null)
            {
                return null;
            }
            var baiKhaoSats = _surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == lastDotKhaoSat.Id && o.Status != (int)TheSurveyStatus.Deleted).ToList();
            
            var baiKhaoSatIds = baiKhaoSats.Select(o => o.Id).ToList();
            var baiLamKhaoSatCacDotDangXet = _surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => baiKhaoSatIds.Contains(o.BaiKhaoSatId)).ToList();
            #endregion

            var departments = await _facultyRepository.GetDepartment(code).ToListAsync();
            var faculty = await _facultyRepository.FindByCode(code);

            if (faculty == null)
            {
                _logger.LogError($"Khong tim thay khoa tai 'FacultyResultAsync{code}' 'await _facultyRepository.GetDepartment({code})'");
            }

            var result = new FacultyResultModel
            {
                FacultyCode = faculty?.Code ?? "",
                FacultyName = faculty?.Name ?? "",
                Result = new List<SurveyResultResponseModel>(),
            };

            var total = new SurveyResultResponseModel
            {
                IsTotal = true,
            };

            var actualDeparments = departments.Where(d => d.Code != (faculty?.Code ?? "")).ToList();

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

                CalculateDeparatmentResult(tmp, baiLamKhaoSatCacDotDangXet, department);

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
            var lastDotKhaoSat = _surveyContext.AsEduSurveyDotKhaoSat
                .OrderByDescending(dks => dks.EndDate)
                .FirstOrDefault(dks => dks.Status != (int)SurveyRoundStatus.Deleted);
            if (lastDotKhaoSat == null)
            {
                return null;
            }
            var baiKhaoSats = await _surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == lastDotKhaoSat.Id && o.Status != (int)TheSurveyStatus.Deleted).ToListAsync();

            var baiKhaoSatIds = baiKhaoSats.Select(o => o.Id).ToList();
            var baiLamKhaoSatCacDotDangXet = await _surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Where(o => baiKhaoSatIds.Contains(o.BaiKhaoSatId)).ToListAsync();
            #endregion

            var department = await _departmentRepository.FindByCode(code);

            if (department == null)
            {
                _logger.LogError($"Khong tim thay bo mon tai 'DepartmentResultAsync{code}' 'await _departmentRepository.FindByCode({code})'");
            }

            var faculty = await _facultyRepository.FindByCode(department?.FacultyCode ?? code);
            var lecturers = await _departmentRepository.GetLecturer(code).ToListAsync();

            if (faculty == null)
            {
                _logger.LogError($"Khong tim thay khoa tai 'DepartmentResultAsync{code}' 'await _facultyRepository.FindByCode(department.FacultyCode)'");
            }

            var result = new DepartmentResultModel
            {
                FacultyCode = faculty?.Code ?? "",
                FacultyName = faculty?.Name ?? "",
                DepartmentCode = department?.Code ?? "",
                DepartmentName = department?.Name ?? "",
                Result = new List<SurveyResultResponseModel>()
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
                await CalculateLecturerResult(tmp, lecturer, baiLamKhaoSatCacDotDangXet);

                if (tmp.SoPhieuPhatRa == 0)
                {
                    continue;
                }

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
        /// Kết quả giảng viên
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<SurveyResultResponseModel> LecturerResultAsync(string code)
        {
            #region setup
            var lastDotKhaoSat = _surveyContext.AsEduSurveyDotKhaoSat
                .OrderByDescending(dks => dks.EndDate)
                .FirstOrDefault(dks => dks.Status != (int)SurveyRoundStatus.Deleted);
            if (lastDotKhaoSat == null)
            {
                return null;
            }
            var baiKhaoSats = _surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == lastDotKhaoSat.Id && o.Status != (int)TheSurveyStatus.Deleted);
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
                ChiTietTungLopMonHoc = new List<ChiTietLopMonHoc>(),
            };

            var classroom = _surveyContext.AsEduSurveyReportTotal
                .Where(o => o.SurveyRoundId == lastDotKhaoSat.Id && o.LecturerCode == code)
                .GroupBy(o => new { o.ClassRoomCode, o.TheSurveyId }).Select(o => o.Key).ToList();

            foreach (var c in classroom)
            {
                var tenLop = "";
                var tenMon = "";
                var lopMonHoc = _eduContext.AsAcademyClassRoom.FirstOrDefault(o => o.Code == c.ClassRoomCode);
                if (lopMonHoc != null)
                {
                    tenLop = lopMonHoc.ClassCode;

                    var monHoc = _eduContext.AsAcademySubject.FirstOrDefault(o => o.Code == lopMonHoc.SubjectCode);
                    if (monHoc != null)
                    {
                        tenMon = monHoc.Name;
                    }
                }

                var baiks = baiKhaoSats.FirstOrDefault(o => o.Id == c.TheSurveyId);
                if (baiks != null)
                {
                    List<QuestionJson> deThi = JsonSerializer.Deserialize<List<QuestionJson>>(baiks.NoiDungDeThi);
                    CalculateDetailResult(result, lecturer, deThi, c.TheSurveyId, tenLop, tenMon, c.ClassRoomCode);
                }
            }
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
        private void CalculateDeparatmentResult(SurveyResultResponseModel Result, List<AsEduSurveyBaiKhaoSatSinhVien> baiLamKhaoSatCacDotDangXet, AsAcademyDepartment department)
        {
            var tmpLoaiMon = Enum.GetValues(typeof(TheSurveyType)).Cast<int>().ToList();
            var danhSachLoaiMonHoc = tmpLoaiMon.Cast<int?>().ToList();
            Result.DeparmentName = department.Name;
            Result.DepartmentCode = department.Code;

            //giảng viên của bộ môn
            var lerturerCodes = _eduContext.AsAcademyLecturer.Where(o => o.DepartmentCode == department.Code).Select(o => o.Code).ToList();

            #region môn
            //các bài làm của môn lý thuyết của bộ môn đang xét
            var baiLamKhaoSatLyThuyet = baiLamKhaoSatCacDotDangXet.ToList().Where(o => lerturerCodes.Contains(o.LecturerCode));
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
        /// <param name="deThiJson"></param>
        /// <param name="baiKhaoSats"></param>
        /// <param name="loaiMon"></param>
        private void CalculateDetailResult(SurveyResultResponseModel Result, AsAcademyLecturer lecturer, List<QuestionJson> deThiJson, 
            Guid baiKhaoSatId, string tenLop, string tenMon, string maLop)
        {
            #region tổng hợp dữ liệu thống kê thô

            string saCode = QuestionType.SA;

            var reportTotalCuThe = _surveyContext.AsEduSurveyReportTotal.Where(o => o.TheSurveyId == baiKhaoSatId);
            var index = 0;

            var test = reportTotalCuThe.Count();

            List<ChiTietCauHoi> chiTietCauHoiList = new List<ChiTietCauHoi>();
            foreach (var cauhoi in deThiJson)
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
                        string diemKey = dapan.Content;
                        diemList.Add(diemKey, "0");

                        var ketqua = reportTotalCuThe.FirstOrDefault(o => o.LecturerCode == lecturer.Code &&
                                                                                o.ClassRoomCode == maLop &&
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
                        var ketqua = reportTotalCuThe.FirstOrDefault(o => o.LecturerCode == lecturer.Code &&
                                                                                o.ClassRoomCode == maLop &&
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
                            var ketquaCon = reportTotalCuThe.FirstOrDefault(o => o.LecturerCode == lecturer.Code &&
                                                                                o.ClassRoomCode == maLop &&
                                                                                o.QuestionCode == $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}" &&
                                                                                o.AnswerCode == dapan.Code);
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

                    var ketqua = reportTotalCuThe.FirstOrDefault(o => o.LecturerCode == lecturer.Code &&
                                                                        o.ClassRoomCode == maLop &&
                                                                        o.QuestionCode == cauhoi.Code);
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
                    chiTietCauHoiList.Add(new ChiTietCauHoi
                    {
                        Content = $"Câu {index}: {cauhoi.Content}"
                    });
                    foreach (var cauhoicon in cauhoi.ChildQuestion)
                    {
                        var chiTietCauHoiCon = new ChiTietCauHoi();
                        Dictionary<string, string> diemList = new Dictionary<string, string>();
                        if (cauhoicon.Type == QuestionType.SC)
                        {
                            chiTietCauHoiCon.Content = $"   Câu {index}.{++indexChild}: {cauhoicon.Content}";

                            var dTB = 0;
                            var sumTotal = 0;
                            var diem = 0;
                            //var colTotal = col + cauhoicon.Answers.Count();
                            foreach (var dapan in cauhoicon.Answers)
                            {
                                diem++;
                                string diemKey = diem.ToString();
                                diemList.Add(diemKey, "0");

                                var ketqua = reportTotalCuThe.FirstOrDefault(o => o.LecturerCode == lecturer.Code &&
                                                                                        o.ClassRoomCode == maLop &&
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
                            chiTietCauHoiCon.DanhSachDiem = diemList;
                            chiTietCauHoiList.Add(chiTietCauHoiCon);
                        }
                        else if (cauhoicon.Type == QuestionType.MC)
                        {
                            chiTietCauHoiCon.Content = $"   Câu {++index}: {cauhoicon.Content}";

                            foreach (var dapan in cauhoicon.Answers)
                            {
                                string dapanKey = dapan.Content;
                                string dapAnValue = "";
                                var ketqua = reportTotalCuThe.FirstOrDefault(o => o.LecturerCode == lecturer.Code &&
                                                                                o.ClassRoomCode == maLop &&
                                                                                o.QuestionCode == cauhoicon.Code &&
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
                                    var ketquaCon = reportTotalCuThe.FirstOrDefault(o => o.LecturerCode == lecturer.Code &&
                                                                                        o.ClassRoomCode == maLop &&
                                                                                        o.QuestionCode == $"{dapan.Code}_{dapan.AnswerChildQuestion.Code}" &&
                                                                                        o.AnswerCode == dapan.Code);
                                    dapAnValue = ketquaCon?.Content ?? "";
                                }
                            }
                            chiTietCauHoiCon.DanhSachDiem = diemList;
                            chiTietCauHoiList.Add(chiTietCauHoiCon);
                        }
                        if (cauhoicon.Type == QuestionType.SA)
                        {
                            chiTietCauHoiCon.Content = $"   Câu {index}.{++indexChild}: {cauhoicon.Content}";

                            var ketqua = reportTotalCuThe.FirstOrDefault(o => o.LecturerCode == lecturer.Code &&
                                                                              o.ClassRoomCode == maLop &&
                                                                              o.QuestionCode == cauhoicon.Code);
                            if (ketqua != null && ketqua.Content != null)
                            {
                                var str = "";
                                var listStr = JsonSerializer.Deserialize<List<string>>(ketqua.Content);
                                listStr.ForEach(s =>
                                {
                                    str += $"{s}; ";
                                });
                                diemList.Add(saCode, str);
                            }

                            chiTietCauHoiCon.DanhSachDiem = diemList;
                            chiTietCauHoiList.Add(chiTietCauHoiCon);
                        }
                    }
                    //chiTietCauHoi.Content = "group choice";
                    //chiTietCauHoiList.Add(chiTietCauHoi);
                }
                else if (cauhoi.Type == QuestionType.T)
                {
                    chiTietCauHoiList.Add(new ChiTietCauHoi
                    {
                        OnlyTitle = cauhoi.Content
                    });
                }
            }

            Result.ChiTietTungLopMonHoc.Add(new ChiTietLopMonHoc
            {
                TenLop = tenLop,
                TenMon = tenMon,
                CauHoi = chiTietCauHoiList
            });
            #endregion
        }
        // subject <= classroom <= lecturer classroom <= lecturer
        private async Task CalculateLecturerResult(SurveyResultResponseModel Result, AsAcademyLecturer lecturer, List<AsEduSurveyBaiKhaoSatSinhVien> baiLamKhaoSatCacDotDangXet)
        {
            Result.LecturerCode = lecturer.Code;
            Result.LecturerName = lecturer.FullName;

            //các bài làm của môn lý thuyết của bộ môn đang xét
            var baiLamKhaoSatLyThuyet = baiLamKhaoSatCacDotDangXet.Where(o => o.LecturerCode == lecturer.Code);
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
