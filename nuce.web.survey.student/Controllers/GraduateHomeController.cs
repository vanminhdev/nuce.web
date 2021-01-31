using Newtonsoft.Json;
using nuce.web.shared;
using nuce.web.survey.student.Attributes.ActionFilter;
using nuce.web.survey.student.Models;
using nuce.web.survey.student.Models.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    [AuthorizeActionFilter(RoleNames.GraduateStudent, RoleNames.KhaoThi_Survey_KhoaBan)]
    public class GraduateHomeController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateTheSurveyStudent/GetTheSurvey");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    ViewData["TheSurveys"] = jsonString;
                    return View();
                },
                action404Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    ViewData["message"] = JsonConvert.DeserializeObject<ResponseMessage>(jsonString)?.message;
                    return View();
                }
            );
        }

        [HttpGet]
        public async Task<ActionResult> TheSurvey(string theSurveyId, string studentCode)
        {
            var resSelectedAnswer = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateTheSurveyStudent/GetSelectedAnswerAutoSave?theSurveyId={theSurveyId}&studentCode={studentCode}");
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

            var resTheSurvey = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateTheSurveyStudent/GetTheSurveyContent?id={theSurveyId}&studentCode={studentCode}");
            ViewData["TheSurveyId"] = theSurveyId;
            ViewData["studentCode"] = studentCode;
            return await base.HandleResponseAsync(resTheSurvey,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    var questions = JsonConvert.DeserializeObject<List<QuestionJson>>(jsonString);
                    return View(questions);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> AutoSave(string studentCode, string theSurveyId, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, int? numStar, string city, bool isAnswerCodesAdd = true)
        {
            var jsonStr = JsonConvert.SerializeObject(new { theSurveyId, questionCode, answerCode, answerCodeInMulSelect, isAnswerCodesAdd, answerContent, numStar, city });
            var stringContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateTheSurveyStudent/AutoSave?studentCode={studentCode}", stringContent);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> TheSurveySubmit(string theSurveyId, string studentCode, string loaiHinh)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateTheSurveyStudent/SaveSelectedAnswer?theSurveyId={theSurveyId}&studentCode={studentCode}&loaiHinh={loaiHinh}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
    }
}