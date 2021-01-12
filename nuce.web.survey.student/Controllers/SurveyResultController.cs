using Newtonsoft.Json;
using nuce.web.shared;
using nuce.web.shared.Models.Survey;
using nuce.web.survey.student.Attributes.ActionFilter;
using nuce.web.survey.student.Common;
using nuce.web.survey.student.Helper;
using nuce.web.survey.student.Models;
using nuce.web.survey.student.Models.Base;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    public class SurveyResultController : BaseController
    {
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
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan)]
        [HttpGet]
        public ActionResult Faculty(string code)
        {
            var model = new SurveyResultModel
            {
                FacultyCode = code ?? UserHelper.username,
            };
            return View("Faculty", model);
        }
        /// <summary>
        /// View Kết quả bộ môn
        /// </summary>
        /// <param name="code"></param>
        /// <param name="facultyCode"></param>
        /// <returns></returns>
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan, RoleNames.KhaoThi_Survey_Department)]
        [HttpGet]
        public ActionResult Department(string code, string facultyCode)
        {
            var model = new SurveyResultModel
            {
                DepartmentCode = code ?? UserHelper.username,
                FacultyCode = facultyCode,
            };

            return View("Department", model);
        }
        /// <summary>
        /// View kết quả giảng viên
        /// </summary>
        /// <param name="code"></param>
        /// <param name="facultyCode"></param>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan, RoleNames.KhaoThi_Survey_Department, RoleNames.KhaoThi_Survey_GiangVien)]
        [HttpGet]
        public ActionResult Lecturer(string code, string facultyCode, string departmentCode)
        {
            var model = new SurveyResultModel
            {
                LecturerCode = code ?? UserHelper.username,
                FacultyCode = facultyCode,
                DepartmentCode = departmentCode,
            };
            return View("Lecturer", model);
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
            switch (login.LoginUserType)
            {
                case 2:
                    target = $"/surveyresult/faculty?code={login.Username}";
                    break;
                case 3:
                    target = $"/surveyresult/department?code={login.Username}&facultyCode=";
                    break;
                case 4:
                    target = $"/surveyresult/lecturer?code={login.Username}&facultyCode=&departmentCode=";
                    break;
                default:
                    break;
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
                    ViewData["facultyname"] = data.FacultyName;
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