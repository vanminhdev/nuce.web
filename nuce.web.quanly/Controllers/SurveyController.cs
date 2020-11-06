using Newtonsoft.Json;
using nuce.web.quanly.Attributes.ActionFilter;
using nuce.web.quanly.Models;
using nuce.web.quanly.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    [AuthorizeActionFilter("Admin,P_KhaoThi")]
    public class SurveyController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        #region question
        [HttpGet]
        public ActionResult Question()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetAllQuestion(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/Question/GetAll", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<Question>>(jsonString);
                    return Json(new
                    {
                        draw = data.Draw,
                        recordsTotal = data.RecordsTotal,
                        recordsFiltered = data.RecordsFiltered,
                        data = data.Data
                    });
                }
            );
        }

        [HttpGet]
        public ActionResult CreateQuestion()
        {
            return View(new QuestionCreate());
        }

        [HttpPost]
        public async Task<ActionResult> CreateQuestionSubmit(QuestionCreate question)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateQuestion", question);
            }
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/question/create", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Cập nhật thành công";
                    //return View("DetailQuestion", detail);
                    return Redirect("/survey/question");
                }
                //action500Async: async res =>
                //{
                //    ViewData["UpdateErrorMessage"] = "Cập nhật không thành công";
                //    var jsonString = await response.Content.ReadAsStringAsync();
                //    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                //    ViewData["UpdateErrorMessageDetail"] = resMess.message;
                //    return View("DetailQuestion", detail);
                //}
            );
        }

        [HttpGet]
        public async Task<ActionResult> DetailQuestion(string questionId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/Question/GetById?id={questionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<QuestionDetail>(jsonString);
                    return View(data);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> UpdateQuestion(QuestionDetail detail)
        {
            if (!ModelState.IsValid)
            {
                return View("DetailQuestion", detail);
            }
            var content = new StringContent(JsonConvert.SerializeObject(detail), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("PUT", $"/api/question/update?id={detail.id}", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Cập nhật thành công";
                    //return View("DetailQuestion", detail);
                    return Redirect("/survey/question");
                }
                //action500Async: async res =>
                //{
                //    ViewData["UpdateErrorMessage"] = "Cập nhật không thành công";
                //    var jsonString = await response.Content.ReadAsStringAsync();
                //    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                //    ViewData["UpdateErrorMessageDetail"] = resMess.message;
                //    return View("DetailQuestion", detail);
                //}
            );
        }
        #endregion


        #region answer
        [HttpGet]
        public async Task<ActionResult> Answer(string questionId)
        {
            string questionContent = "";
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/question/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                questionContent = (JsonConvert.DeserializeObject<Question>(jsonString)).content;
            }

            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/Answer/GetByQuestionId?questionId={questionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var answers = JsonConvert.DeserializeObject<List<Answer>>(jsonString);

                    return View(new AnswerOfQuestion() {
                        Answers = answers,
                        QuestionContent = questionContent,
                        QuestionId = questionId
                    });
                }
            );
        }

        [HttpGet]
        public async Task<ActionResult> CreateAnswer(string questionId)
        {
            Question question = new Question();
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/question/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                question = (JsonConvert.DeserializeObject<Question>(jsonString));
            }

            return View(new AnswerCreateOfQuestion {
                AnswerBind = new AnswerCreate() {
                    cauHoiGId = questionId,
                    cauHoiId = int.Parse(question.ma)
                },
                QuestionContent = HttpUtility.UrlEncode(question.content),
                QuestionId = questionId
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateAnswerSubmit(AnswerCreateOfQuestion answer)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateAnswer", answer);
            }
            var content = new StringContent(JsonConvert.SerializeObject(answer.AnswerBind), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/answer/create", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    return Redirect($"/survey/answer?questionId={answer.QuestionId}");
                }
            );
        }

        [HttpGet]
        public async Task<ActionResult> DetailAnswer(string id, string questionId)
        {
            string questionContent = "";
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/question/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                var question = (JsonConvert.DeserializeObject<Question>(jsonString));
                questionContent = question.content;
            }

            var response = await base.MakeRequestAuthorizedAsync("GET", $"/api/answer/GetById?id={id}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var answer = JsonConvert.DeserializeObject<Answer>(jsonString);
                    return View(new UpdateAnswer
                    {
                        AnswerBind = answer,
                        QuestionContent = questionContent,
                        QuestionId = questionId
                    });
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAnswer(UpdateAnswer detail)
        {
            if (!ModelState.IsValid)
            {
                return View("DetailAnswer", detail);
            }
            var content = new StringContent(JsonConvert.SerializeObject(detail.AnswerBind), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/answer/update?id={detail.AnswerBind.id}", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Cập nhật thành công";
                    //return View("DetailQuestion", detail);
                    return Redirect($"/survey/answer?questionId={detail.QuestionId}");
                }
            );
        }
        #endregion
    }
}