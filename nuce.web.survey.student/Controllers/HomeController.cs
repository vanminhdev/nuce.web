using Newtonsoft.Json;
using nuce.web.survey.student.Attributes.ActionFilter;
using nuce.web.survey.student.Models;
using nuce.web.survey.student.Models.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    [AuthorizeActionFilter("Student")]
    public class HomeController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetTheSurvey");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    ViewData["Content"] = jsonString;
                    return View();
                }
            );
        }

        [HttpGet]
        public async Task<ActionResult> TheSurvey(string theSurveyId, string classRoomCode)
        {
            var resSelectedAnswer = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetSelectedAnswerAutoSave?classRoomCode={classRoomCode}");
            await base.HandleResponseAsync(resSelectedAnswer,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    //var selectedAnswer = JsonConvert.DeserializeObject<List<SelectedAnswer>>(jsonString);
                    if(jsonString != "")
                    {
                        ViewData["SelectedAnswer"] = jsonString;
                    }
                    return null;
                }
            );

            var resTheSurvey = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetTheSurveyContent?id={theSurveyId}");
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
        public async Task<ActionResult> AutoSave(string classRoomCode, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, bool isAnswerCodesAdd = true)
        {
            var jsonStr = JsonConvert.SerializeObject(new { classRoomCode, questionCode, answerCode, answerCodeInMulSelect, isAnswerCodesAdd, answerContent });
            var stringContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/TheSurveyStudent/AutoSave", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return Json(new { type = "success", message = "lưu thành công" }, JsonRequestBehavior.AllowGet);
                },
                action400Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    return Json(new { type = "error", resMess.message, resMess.detailMessage }, JsonRequestBehavior.AllowGet);
                },
                action500Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    return Json(new { type = "error", resMess.message, resMess.detailMessage }, JsonRequestBehavior.AllowGet);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> TheSurveySubmit(string classRoomCode)
        {
            var stringContent = new StringContent($"'{classRoomCode}'", Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/TheSurveyStudent/SaveTask", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return Json(new { type = "success", message = "lưu thành công" }, JsonRequestBehavior.AllowGet);
                },
                action500Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    return Json(new { type = "error", resMess.message, resMess.detailMessage }, JsonRequestBehavior.AllowGet);
                }
            );
        }
    }
}