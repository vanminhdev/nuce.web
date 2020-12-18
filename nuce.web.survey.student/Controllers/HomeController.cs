using Newtonsoft.Json;
using nuce.web.survey.student.Attributes.ActionFilter;
using nuce.web.survey.student.Common;
using nuce.web.survey.student.Models;
using nuce.web.survey.student.Models.JsonData;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    public class HomeController : BaseController
    {
        #region sinh viên thường
        [HttpGet]
        [AuthorizeActionFilter("Student")]
        public async Task<ActionResult> Index()
        {
            var accessToken = Request.Cookies[UserParameters.JwtAccessToken].Value;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(accessToken);
            if(jwtSecurityToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).FirstOrDefault(o => o == "UndergraduateStudent") != null)
            {
                var resUndergraduate = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateTheSurveyStudent/GetTheSurvey");
                await base.HandleResponseAsync(resUndergraduate,
                    action200Async: async res =>
                    {
                        var jsonString = await res.Content.ReadAsStringAsync();
                        ViewData["UndergraduateTheSurvey"] = jsonString;
                        return null;
                    }
                );
            }

            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetTheSurvey");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    ViewData["TheSurvey"] = jsonString;
                    return View();
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter("UndergraduateStudent")]
        public async Task<ActionResult> IndexUndergraduate()
        {
            var accessToken = Request.Cookies[UserParameters.JwtAccessToken].Value;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(accessToken);
            if (jwtSecurityToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).FirstOrDefault(o => o == "UndergraduateStudent") != null)
            {
                var resUndergraduate = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateTheSurveyStudent/GetTheSurvey");
                await base.HandleResponseAsync(resUndergraduate,
                    action200Async: async res =>
                    {
                        var jsonString = await res.Content.ReadAsStringAsync();
                        ViewData["UndergraduateTheSurvey"] = jsonString;
                        return null;
                    }
                );
            }

            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetTheSurvey");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    ViewData["TheSurvey"] = jsonString;
                    return View("Index");
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter("Student")]
        public async Task<ActionResult> TheSurvey(string theSurveyId, string classRoomCode)
        {
            var resSelectedAnswer = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetSelectedAnswerAutoSave?classRoomCode={classRoomCode}");
            await base.HandleResponseAsync(resSelectedAnswer,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    if(jsonString != "")
                    {
                        ViewData["SelectedAnswer"] = jsonString;
                    }
                    return null;
                }
            );

            var resTheSurvey = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetTheSurveyContent?id={theSurveyId}&classroomCode={classRoomCode}");
            return await base.HandleResponseAsync(resTheSurvey,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    var questions = JsonConvert.DeserializeObject<List<QuestionJson>>(jsonString);
                    ViewData["classRoomCode"] = classRoomCode;
                    return View(questions);
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter("Student")]
        public async Task<ActionResult> AutoSave(string classRoomCode, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, bool isAnswerCodesAdd = true)
        {
            var jsonStr = JsonConvert.SerializeObject(new { classRoomCode, questionCode, answerCode, answerCodeInMulSelect, isAnswerCodesAdd, answerContent });
            var stringContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/TheSurveyStudent/AutoSave", stringContent);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter("Student")]
        public async Task<ActionResult> TheSurveySubmit(string classRoomCode)
        {
            var stringContent = new StringContent($"'{classRoomCode}'", Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/TheSurveyStudent/SaveSelectedAnswer", stringContent);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region sinh viên sắp tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter("UndergraduateStudent")]
        public async Task<ActionResult> UndergraduateTheSurvey(string theSurveyId)
        {
            var resSelectedAnswer = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateTheSurveyStudent/GetSelectedAnswerAutoSave?theSurveyId={theSurveyId}");
            await base.HandleResponseAsync(resSelectedAnswer,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    if (jsonString != "")
                    {
                        ViewData["SelectedAnswer"] = jsonString;
                    }
                    return null;
                }
            );

            var resTheSurvey = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateTheSurveyStudent/GetTheSurveyContent?id={theSurveyId}");
            ViewData["TheSurveyId"] = theSurveyId;
            return await base.HandleResponseAsync(resTheSurvey,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    var questions = JsonConvert.DeserializeObject<List<QuestionJson>>(jsonString);
                    return View("UndergraduateTheSurvey", questions);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> UndergraduateAutoSave(string theSurveyId, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, bool isAnswerCodesAdd = true)
        {
            var jsonStr = JsonConvert.SerializeObject(new { theSurveyId, questionCode, answerCode, answerCodeInMulSelect, isAnswerCodesAdd, answerContent });
            var stringContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateTheSurveyStudent/AutoSave", stringContent);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UndergraduateTheSurveySubmit(string theSurveyId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateTheSurveyStudent/SaveSelectedAnswer?theSurveyId={theSurveyId}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}