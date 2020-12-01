using Newtonsoft.Json;
using nuce.web.quanly.Attributes.ActionFilter;
using nuce.web.quanly.Attributes.ValidationAttributes;
using nuce.web.quanly.Models;
using nuce.web.quanly.Models.JsonData;
using nuce.web.quanly.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    [AuthorizeActionFilter("P_KhaoThi")]
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
                    cauHoiId = questionId,
                    cauHoiCode = question.code
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

        #region đề khảo sát
        [HttpGet]
        public async Task<ActionResult> ExamQuestions()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/ExamQuestions/GetAll");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<ExamQuestions>>(jsonString);
                    return View(result);
                }
            );
        }

        [HttpGet]
        public ActionResult ExamStructure(string examQuestionId)
        {
            ViewData["ExamQuestionId"] = examQuestionId;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetStructure(string examQuestionId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/ExamQuestions/GetExamStructure?examQuestionId={examQuestionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<ExamStructure>>(jsonString);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            );
        }

        [HttpGet]
        public ActionResult CreateExamQuestions()
        {
            return View(new ExamQuestionsCreate());
        }

        [HttpPost]
        public async Task<ActionResult> CreateExamQuestions(ExamQuestionsCreate exam)
        {
            if (!ModelState.IsValid)
            {
                return View(exam);
            }
            var content = new StringContent(JsonConvert.SerializeObject(exam), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/ExamQuestions/Create", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["CreateSuccessMessage"] = "Thêm thành công";
                    return View(exam);
                },
                action500Async: async res =>
                {
                    ViewData["CreateErrorMessage"] = "Thêm không thành công";
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["CreateErrorMessageDetail"] = resMess.message;
                    return View(exam);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> AddQuestionExam(AddQuestionExam question)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { type = "fail", message = "Thêm không thành công", detailMessage = "Dữ liệu đã nhập không hợp lệ" });
            }
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/ExamQuestions/AddQuestion", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    return Json(new { type = "success", message = "Thêm thành công" });
                },
                action500Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    return Json(new { type = "fail", message = "Thêm không thành công", detailMessage = resMess.message });
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> GenerateExam(string examQuestionId)
        {
            var content = new StringContent($@"'{examQuestionId}'", Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/ExamQuestions/GenerateExam", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    return Json(new { type = "success", message = "Tạo thành công" });
                },
                action500Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    return Json(new { type = "fail", message = "Tạo không thành công", detailMessage = resMess.message });
                }
            );
        }

        [HttpGet]
        public async Task<ActionResult> ExamDetail(string examQuestionId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/ExamQuestions/GetExamDetailJsonString?examQuestionId={examQuestionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<QuestionJson>>(jsonString);
                    if (result == null)
                        return View(new List<QuestionJson>());
                    return View(result);
                }
            );
        }


        #endregion

        #region thống kê
        [HttpGet]
        public ActionResult Statistic()
        {
            return View();
        }

        public async Task<ActionResult> GetStatusReportTotalNormalSurvey()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/Statistic/GetStatusReportTotalNormalSurveyTask");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    return Json(jsonString, JsonRequestBehavior.AllowGet);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> ReportTotalNormalSurvey()
        {
            await base.MakeRequestAuthorizedAsync("Post", $"/api/Statistic/ReportTotalNormalSurvey");
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}