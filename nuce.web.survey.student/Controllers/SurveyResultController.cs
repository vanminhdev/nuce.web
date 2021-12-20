using Newtonsoft.Json;
using nuce.web.shared;
using nuce.web.shared.Common;
using nuce.web.shared.Models.Survey;
using nuce.web.survey.student.Attributes.ActionFilter;
using nuce.web.survey.student.Common;
using nuce.web.survey.student.Entities;
using nuce.web.survey.student.Models;
using nuce.web.survey.student.Models.Base;
using nuce.web.survey.student.Models.SurveyResult;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    public class SurveyResultController : BaseController
    {
        private readonly SurveyContext _surveyContext;
        public SurveyResultController()
        {
            _surveyContext = new SurveyContext();
        }

        #region view
        // GET: SurveyResult
        /// <summary>
        /// View Login cho cả 3 loại user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login(int loginType)
        {
            return View("Login", new LoginModel
            {
               LoginUserType = loginType
            });
        }

        /// <summary>
        /// View Kết quả khoa ban
        /// Chú ý thứ tự Role Authorize: Role đầu dùng để điều hướng sang đúng loại login nếu chưa có token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan)]
        [HttpGet]
        public ActionResult Faculty(string code)
        {
            var model = new SurveyResultModel
            {
                FacultyCode = code ?? GetCurrentUserCode(),
            };
            return View(model);
        }

        public ActionResult DsDotKhaoSat()
        {
            var dotks = _surveyContext.AS_Edu_Survey_DotKhaoSat.Where(d => d.Status == (int)SurveyRoundStatus.End).ToList();
            return View(dotks);
        }

        public ActionResult DsBoMon(Guid surveyRoundId)
        {
            ViewBag.SurveyRoundId = surveyRoundId;
            var maKhoa = GetCurrentUserCode();
            var dsBoMon = _surveyContext.AS_Academy_Department.Where(b => b.FacultyCode == maKhoa).ToList();
            return View(dsBoMon);
        }

        /// <summary>
        /// View Kết quả bộ môn
        /// Chú ý thứ tự Role Authorize: Role đầu dùng để điều hướng sang đúng loại login nếu chưa có token
        /// </summary>
        /// <param name="code"></param>
        /// <param name="facultyCode"></param>
        /// <returns></returns>
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Department, RoleNames.KhaoThi_Survey_KhoaBan)]
        [HttpGet]
        public ActionResult Department(string code, Guid surveyRoundId)
        {
            var dsMonHoc = _surveyContext.AS_Academy_Subject.Where(s => s.DepartmentCode == code).ToList();

            var dsCodeMonHoc = dsMonHoc.Select(m => m.Code).ToList();

            var baiKhaoSat = _surveyContext.AS_Edu_Survey_BaiKhaoSat.AsNoTracking()
                .FirstOrDefault(o => o.DotKhaoSatID == surveyRoundId && o.Status != (int)TheSurveyStatus.Deleted);

            var baiLamKhaoSatQuery = _surveyContext.AS_Edu_Survey_BaiKhaoSat_SinhVien.AsNoTracking()
                .Where(o => o.BaiKhaoSatID == baiKhaoSat.ID && !string.IsNullOrEmpty(o.LecturerCode));

            var result = new List<KetQuaMonHocCuaBoMon>();

            ViewBag.SurveyRoundId = surveyRoundId;

            foreach (var monHoc in dsMonHoc)
            {
                result.Add(new KetQuaMonHocCuaBoMon
                {
                    Code = monHoc.Code,
                    Name = monHoc.Name,
                    SoSVHoc = baiLamKhaoSatQuery.Where(o => o.SubjectCode == monHoc.Code).Count(),
                    SoSVLamKhaoSat = baiLamKhaoSatQuery.Where(o => o.SubjectCode == monHoc.Code && o.Status == (int)shared.Common.SurveyStudentStatus.Done).Count(),
                });
            }
            return View(result);
        }

        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Department, RoleNames.KhaoThi_Survey_KhoaBan)]
        [HttpGet]
        public ActionResult Subject(string code, Guid surveyRoundId)
        {
            var baiKhaoSat = _surveyContext.AS_Edu_Survey_BaiKhaoSat.AsNoTracking()
                .FirstOrDefault(o => o.DotKhaoSatID == surveyRoundId && o.Status != (int)TheSurveyStatus.Deleted);

            var baiLamKhaoSatQuery = _surveyContext.AS_Edu_Survey_BaiKhaoSat_SinhVien.AsNoTracking().Where(o => o.BaiKhaoSatID == baiKhaoSat.ID);

            var result = new List<KetQuaGiangVienCuaMonHoc>();

            var ketQuaKsQuery = _surveyContext.AS_Edu_Survey_ReportTotal.AsNoTracking().Where(d => d.SurveyRoundId == surveyRoundId);

            var LecturerCodes = baiLamKhaoSatQuery.Where(o => !string.IsNullOrEmpty(o.LecturerCode) && o.SubjectCode == code)
                .Select(o => new { o.LecturerCode, o.ClassRoomCode }).Distinct().ToList();

            var subject = _surveyContext.AS_Academy_Subject.FirstOrDefault(o => o.Code == code);

            ViewBag.SubjectCode = code;
            ViewBag.SubjectName = subject?.Name ?? "";

            ViewBag.SurveyRoundId = surveyRoundId;

            foreach (var gvLop in LecturerCodes)
            {
                var gv = _surveyContext.AS_Academy_Lecturer.FirstOrDefault(o => o.Code == gvLop.LecturerCode);

                result.Add(new KetQuaGiangVienCuaMonHoc
                {
                    LecturerCode = gvLop.LecturerCode,
                    LecturerName = gv?.FullName ?? "",
                    MaLop = gvLop.ClassRoomCode,
                    SoSVHoc = baiLamKhaoSatQuery.Where(o => o.LecturerCode == gvLop.LecturerCode && o.ClassRoomCode == gvLop.ClassRoomCode && o.SubjectCode == code).Count(),
                    SoSVLamKhaoSat = baiLamKhaoSatQuery.Where(o => o.LecturerCode == gvLop.LecturerCode && o.ClassRoomCode == gvLop.ClassRoomCode && o.SubjectCode == code && o.Status == (int)shared.Common.SurveyStudentStatus.Done).Count(),
                });
            }
            return View(result);
        }

        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_GiangVien, RoleNames.KhaoThi_Survey_Department, RoleNames.KhaoThi_Survey_KhoaBan)]
        [HttpGet]
        public ActionResult SubjectLecturer(string lecturerCode, string subjectCode, Guid surveyRoundId)
        {
            return View();
        }

        /// <summary>
        /// View kết quả giảng viên
        /// Chú ý thứ tự Role Authorize: Role đầu dùng để điều hướng sang đúng loại login nếu chưa có token
        /// </summary>
        /// <param name="code"></param>
        /// <param name="facultyCode"></param>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_GiangVien, RoleNames.KhaoThi_Survey_KhoaBan, RoleNames.KhaoThi_Survey_Department)]
        [HttpGet]
        public async Task<ActionResult> Lecturer(string code, string facultyCode, string departmentCode)
        {
            code = code ?? base.GetCurrentUsername();
            var stringContent = new StringContent("", Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/SurveyResult/lecturer/{code}", stringContent);
            var data = await base.DeserializeResponseAsync<SurveyResultResponseModel>(response.Content);

            var model = new SurveyResultModel
            {
                LecturerCode = code ?? base.GetCurrentUsername(),
                FacultyCode = facultyCode,
                DepartmentCode = departmentCode,
                Data = data
            };
            return View("Lecturer", model);
        }
        #endregion


        #region Thống kê kiểu mới
        public ActionResult DanhSachMonHocTheoBoMon(string code, Guid surveyRoundId)
        {
            var dsMonHoc = _surveyContext.AS_Academy_Subject.Where(s => s.DepartmentCode == code).ToList();

            var dsCodeMonHoc = dsMonHoc.Select(m => m.Code).ToList();

            #region setup
            var baiKhaoSat = _surveyContext.AS_Edu_Survey_BaiKhaoSat.AsNoTracking()
                .FirstOrDefault(o => o.DotKhaoSatID == surveyRoundId && o.Status != (int)TheSurveyStatus.Deleted);

            var baiLamKhaoSatCacDotDangXet = _surveyContext.AS_Edu_Survey_BaiKhaoSat_SinhVien.AsNoTracking().Where(o => o.BaiKhaoSatID == baiKhaoSat.ID);
            #endregion

            return View();
        }

        public void DanhSachGiangVienTheoMonHoc(string maMon, Guid dotKhaoSatId)
        {
            var reportTotalQuery = _surveyContext.AS_Edu_Survey_ReportTotal.Where(r => r.SurveyRoundId == dotKhaoSatId);

            var dsGiangVien = reportTotalQuery.Where(r => r.SubjectCode == maMon && !string.IsNullOrWhiteSpace(r.LecturerCode)).ToList();

        }

        public void DanhSachLopMonHoc(string maMon, string maGiangVien, Guid dotKhaoSatId)
        {
            var reportTotalQuery = _surveyContext.AS_Edu_Survey_ReportTotal.Where(r => r.SurveyRoundId == dotKhaoSatId);

            var dsLopMonHoc = reportTotalQuery.Where(r => r.SubjectCode == maMon && r.LecturerCode == maGiangVien)
                .Select(r => new {
                    r.NHHK,
                    r.ClassRoomCode,
                    r.SubjectCode,
                    r.ClassRoom,
                    r.LecturerCode
                })
                .ToList();
        }

        public void KetQua(string nhhk, string classroomCode, string subjectCode, string classroom, string lecturerCode)
        {

        }
        #endregion


        #region action (call api)
        /// <summary>
        /// Call api login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostLogin(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return View("login", new LoginModel());
            }

            var userNamePasswordJsonString = JsonConvert.SerializeObject(new
            {
                username = login.Username,
                password = login.Password,
                loginUserType = login.LoginUserType
            });

            var content = new StringContent(userNamePasswordJsonString, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/user/login", content);

            IEnumerable<Cookie> responseCookies = base.GetAllCookies();

            var accessToken = responseCookies.FirstOrDefault(c => c.Name == UserParameters.JwtAccessToken);
            var refreshToken = responseCookies.FirstOrDefault(c => c.Name == UserParameters.JwtRefreshToken);

            if (accessToken != null)
            {
                Response.Cookies[UserParameters.JwtAccessToken].Value = accessToken.Value;
                Response.Cookies[UserParameters.JwtAccessToken].HttpOnly = true;
                Response.Cookies[UserParameters.JwtAccessToken].Expires = accessToken.Expires;
            }

            if (refreshToken != null)
            {
                Response.Cookies[UserParameters.JwtRefreshToken].Value = refreshToken.Value;
                Response.Cookies[UserParameters.JwtRefreshToken].HttpOnly = true;
                Response.Cookies[UserParameters.JwtRefreshToken].Expires = refreshToken.Expires;
            }

            var target = "/surveyresult/lecturer";

            if (login.LoginUserType == (int)LoginType.Faculty)
            {
                target = $"/surveyresult/faculty?code={login.Username}";
            }
            else if (login.LoginUserType == (int)LoginType.Department)
            {
                target = $"/surveyresult/department?code={login.Username}&facultyCode=";
            }
            else if (login.LoginUserType == (int)LoginType.Lecturer)
            {
                target = $"/surveyresult/lecturer?code={login.Username}&facultyCode=&departmentCode=";
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var handler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = handler.ReadJwtToken(accessToken.Value);
                    var roles = jwtSecurityToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();
                    return Redirect(target);
                case HttpStatusCode.NotFound:
                    ViewData["LoginMessage"] = "Tài khoản không tồn tại";
                    break;
                case HttpStatusCode.BadGateway:
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["LoginMessage"] = "Hiện tại không thể kết nối đến Đào tạo";
                    ViewData["LoginFailed"] = jsonString;
                    break;
                case HttpStatusCode.InternalServerError:
                    jsonString = await response.Content.ReadAsStringAsync();
                    message = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["LoginMessage"] = message.message;
                    ViewData["LoginFailed"] = jsonString;
                    break;
                case HttpStatusCode.Unauthorized:
                    ViewData["LoginMessage"] = "Tài khoản hoặc mật khẩu không chính xác";
                    break;
                default:
                    jsonString = await response.Content.ReadAsStringAsync();
                    ViewData["LoginMessage"] = "Đăng nhập không thành công";
                    ViewData["LoginFailed"] = jsonString;
                    break;
            }
            return View("login", new LoginModel());
        }

        /// <summary>
        /// Call api lấy kết quả khoa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan)]
        [HttpPost]
        public async Task<ActionResult> GetFacultyResult(string code, DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request.Search), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/SurveyResult/faculty/{code}", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var data = await base.DeserializeResponseAsync<FacultyResultModel>(response.Content);
                    ViewData["facultyname"] = data.FacultyName;
                    return Json(new
                    {
                        draw = request.Draw += 1,
                        data = data.Result,
                        recordsTotal = data.Result.Count,
                        recordsFiltered = data.Result.Count,
                    });
                }
            );
        }
        
        /// <summary>
        /// Call api lấy kết quả bộ môn
        /// </summary>
        /// <param name="code"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan, RoleNames.KhaoThi_Survey_Department)]
        [HttpPost]
        public async Task<ActionResult> GetDepartmentResult(string code, DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request.Search), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/SurveyResult/department/{code}", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var data = await base.DeserializeResponseAsync<DepartmentResultModel>(response.Content);
                    ViewData["departmentName"] = data.DepartmentName;
                    return Json(new
                    {
                        draw = request.Draw += 1,
                        data = data.Result,
                        recordsTotal = data.Result.Count,
                        recordsFiltered = data.Result.Count,
                    });
                }
            );
        }
        #endregion
    }
}