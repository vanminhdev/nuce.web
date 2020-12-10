using Newtonsoft.Json;
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
    [AuthorizeActionFilter("GraduateStudent")]
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
                }
            );
        }

        [HttpGet]
        public async Task<ActionResult> TheSurvey(string theSurveyId)
        {
            var resSelectedAnswer = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateTheSurveyStudent/GetSelectedAnswerAutoSave?theSurveyId={theSurveyId}");
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

            var resTheSurvey = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateTheSurveyStudent/GetTheSurveyContent?id={theSurveyId}");
            ViewData["TheSurveyId"] = theSurveyId;
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
        public async Task<ActionResult> AutoSave(string theSurveyId, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, bool isAnswerCodesAdd = true)
        {
            var jsonStr = JsonConvert.SerializeObject(new { theSurveyId, questionCode, answerCode, answerCodeInMulSelect, isAnswerCodesAdd, answerContent });
            var stringContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateTheSurveyStudent/AutoSave", stringContent);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> TheSurveySubmit(string theSurveyId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateTheSurveyStudent/SaveSelectedAnswer?theSurveyId={theSurveyId}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
    }
}