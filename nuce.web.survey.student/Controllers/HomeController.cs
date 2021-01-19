using Newtonsoft.Json;
using nuce.web.survey.student.Attributes.ActionFilter;
using nuce.web.shared;
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
using nuce.web.survey.student.Models.Survey.Normal;

namespace nuce.web.survey.student.Controllers
{
    public class HomeController : BaseController
    {
        #region sinh viên thường
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.Student)]
        public async Task<ActionResult> Index()
        {
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
        [AuthorizeActionFilter(RoleNames.UndergraduateStudent)]
        public async Task<ActionResult> IndexUndergraduate()
        {
            var resUndergraduate = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateTheSurveyStudent/GetTheSurvey");
            return await base.HandleResponseAsync(resUndergraduate,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    ViewData["UndergraduateTheSurvey"] = jsonString;
                    return View();
                },
                action404Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    ViewData["message"] = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    return View();
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.Student)]
        public async Task<ActionResult> TheSurvey(string theSurveyId, string classRoomCode, string nhhk)
        {
            var resSelectedAnswer = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetSelectedAnswerAutoSave?classRoomCode={classRoomCode}&nhhk={nhhk}");
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

            var resTheSurvey = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetTheSurveyContent?id={theSurveyId}&classroomCode={classRoomCode}&nhhk={nhhk}");
            return await base.HandleResponseAsync(resTheSurvey,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    var theSurveyContent = JsonConvert.DeserializeObject<TheSurveyContent>(jsonString);
                    var questions = JsonConvert.DeserializeObject<List<QuestionJson>>(theSurveyContent.NoiDungDeKhaoSat);
                    ViewData["classRoomCode"] = classRoomCode;
                    ViewData["nhhk"] = nhhk;

                    ViewData["ClassroomName"] = theSurveyContent.ClassroomName;
                    ViewData["NHHK"] = theSurveyContent.NHHK;
                    ViewData["LeturerName"] = theSurveyContent.LeturerName;
                    ViewData["SurveyRoundName"] = theSurveyContent.SurveyRoundName;
                    return View(questions);
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.Student)]
        public async Task<ActionResult> AutoSave(string classRoomCode, string nhhk, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, bool isAnswerCodesAdd = true)
        {
            var jsonStr = JsonConvert.SerializeObject(new { classRoomCode, nhhk, questionCode, answerCode, answerCodeInMulSelect, isAnswerCodesAdd, answerContent });
            var stringContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/TheSurveyStudent/AutoSave", stringContent);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.Student)]
        public async Task<ActionResult> TheSurveySubmit(string classRoomCode, string nhhk)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/TheSurveyStudent/SaveSelectedAnswer?classRoomCode={classRoomCode}&nhhk={nhhk}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region sinh viên sắp tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.UndergraduateStudent)]
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
        [AuthorizeActionFilter(RoleNames.UndergraduateStudent)]
        public async Task<ActionResult> UndergraduateAutoSave(string theSurveyId, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, bool isAnswerCodesAdd = true)
        {
            var jsonStr = JsonConvert.SerializeObject(new { theSurveyId, questionCode, answerCode, answerCodeInMulSelect, isAnswerCodesAdd, answerContent });
            var stringContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateTheSurveyStudent/AutoSave", stringContent);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.UndergraduateStudent)]
        public async Task<ActionResult> UndergraduateTheSurveySubmit(string theSurveyId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateTheSurveyStudent/SaveSelectedAnswer?theSurveyId={theSurveyId}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.UndergraduateStudent)]
        public ActionResult Verification()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.UndergraduateStudent)]
        public async Task<ActionResult> Verification(string email, string phone)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { email, phone }), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateTheSurveyStudent/Verification", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() });
        }

        [HttpGet]
        public async Task<ActionResult> VerifyByToken(string studentCode, string token)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateTheSurveyStudent/VerifyByToken?studentCode={studentCode}&token={token}");
            ViewData["statusCode"] = response.StatusCode;
            ViewData["content"] = JsonConvert.DeserializeObject<ResponseMessage>(await response.Content.ReadAsStringAsync());
            return View();
        }
        #endregion
    }
}