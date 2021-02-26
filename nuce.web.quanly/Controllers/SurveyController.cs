using Newtonsoft.Json;
using nuce.web.quanly.Attributes.ActionFilter;
using nuce.web.quanly.Attributes.ValidationAttributes;
using nuce.web.quanly.Models;
using nuce.web.quanly.Models.Base;
using nuce.web.quanly.Models.JsonData;
using nuce.web.quanly.Models.Survey.Graduate;
using nuce.web.quanly.Models.Survey.Normal;
using nuce.web.quanly.Models.Survey.Normal.Statistic;
using nuce.web.quanly.Models.Survey.Undergraduate;
using nuce.web.quanly.ViewModel.Base;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    public class SurveyController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        #region question
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public ActionResult Question()
        {
            return View("~/Views/Survey/Normal/Question.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public ActionResult CreateQuestion()
        {
            return View("~/Views/Survey/Normal/CreateQuestion.cshtml", new QuestionCreate());
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> CreateQuestionSubmit(QuestionCreate question)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Survey/Normal/CreateQuestion.cshtml", question);
            }
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/question/create", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Thêm thành công";
                    return View("~/Views/Survey/Normal/CreateQuestion.cshtml", question);
                },
                action500Async: async res =>
                {
                    ViewData["UpdateErrorMessage"] = "Thêm không thành công";
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["UpdateErrorMessageDetail"] = resMess.message;
                    return View("~/Views/Survey/Normal/CreateQuestion.cshtml", question);
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> DetailQuestion(string questionId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/Question/GetById?id={questionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<QuestionDetail>(jsonString);
                    return View("~/Views/Survey/Normal/DetailQuestion.cshtml", data);
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> UpdateQuestion(QuestionDetail question)
        {
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("PUT", $"/api/question/update?id={question.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() });
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> DeleteQuestion(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/Question/Delete?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region answer
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
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

                    return View("~/Views/Survey/Normal/Answer.cshtml", new AnswerOfQuestion() {
                        Answers = answers,
                        QuestionContent = questionContent,
                        QuestionId = questionId
                    });
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> CreateAnswer(string questionId)
        {
            Question question = new Question();
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/question/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                question = (JsonConvert.DeserializeObject<Question>(jsonString));
            }

            return View("~/Views/Survey/Normal/CreateAnswer.cshtml", new AnswerCreateOfQuestion {
                AnswerBind = new AnswerCreate() {
                    cauHoiId = questionId,
                    cauHoiCode = question.code
                },
                QuestionContent = HttpUtility.UrlEncode(question.content),
                QuestionType = question.type,
                QuestionId = questionId
            });
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> CreateAnswerSubmit(AnswerCreateOfQuestion answer)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Survey/Normal/CreateAnswer.cshtml", answer);
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> DetailAnswer(string id, string questionId)
        {
            string questionContent = "";
            string questionType = "";
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/question/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                var question = (JsonConvert.DeserializeObject<Question>(jsonString));
                questionContent = question.content;
                questionType = question.type;
            }

            var response = await base.MakeRequestAuthorizedAsync("GET", $"/api/answer/GetById?id={id}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var answer = JsonConvert.DeserializeObject<Answer>(jsonString);

                    if (answer.childQuestionId != null)
                    {
                        var resChildQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/question/GetById?id={answer.childQuestionId}");
                        if (resChildQues.IsSuccessStatusCode)
                        {
                            var str = await resChildQues.Content.ReadAsStringAsync();
                            ViewData["childQuestion"] = str;
                        }
                    }

                    return View("~/Views/Survey/Normal/DetailAnswer.cshtml", new UpdateAnswer
                    {
                        AnswerBind = answer,
                        QuestionContent = questionContent,
                        QuestionType = questionType,
                        QuestionId = questionId
                    });
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> UpdateAnswer(UpdateAnswer detail)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Survey/Normal/DetailAnswer.cshtml", detail);
            }
            var content = new StringContent(JsonConvert.SerializeObject(detail.AnswerBind), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/answer/update?id={detail.AnswerBind.id}", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Cập nhật thành công";
                    return Redirect($"/survey/answer?questionId={detail.QuestionId}");
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> DeleteAnswer(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/Answer/Delete?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region phiếu khảo sát
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public ActionResult ExamQuestions()
        {
            return View("~/Views/Survey/Normal/ExamQuestions.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> GetExamQuestions(DataTableRequest request)
        {
            return await GetDataTabeFromApi<ExamQuestions>(request, "/api/ExamQuestions/GetAll");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> DeleteExam(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/ExamQuestions/Delete?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public ActionResult ExamStructure(string examQuestionId)
        {
            ViewData["ExamQuestionId"] = examQuestionId;
            return View("~/Views/Survey/Normal/ExamStructure.cshtml");
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
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

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> CreateExamQuestions(ExamQuestionsCreate exam)
        {
            var content = new StringContent(JsonConvert.SerializeObject(exam), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/ExamQuestions/Create", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> AddQuestionExam(AddQuestionExam question)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { statusCode = HttpStatusCode.BadRequest }, JsonRequestBehavior.AllowGet);
            }
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/ExamQuestions/AddQuestion", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> GenerateExam(string examQuestionId, List<SortQuestion> sortResult)
        {
            var json = JsonConvert.SerializeObject(new { examQuestionId, sortResult });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/ExamQuestions/GenerateExam", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> ExamDetail(string examQuestionId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/ExamQuestions/GetExamDetailJsonString?examQuestionId={examQuestionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<QuestionJson>>(jsonString);
                    if (result == null)
                        return View("~/Views/Survey/Normal/ExamDetail.cshtml", new List<QuestionJson>());
                    return View("~/Views/Survey/Normal/ExamDetail.cshtml", result);
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> DeleteQuestionFromStructure(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/ExamQuestions/DeleteQuestionFromStructure?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region đợt khảo sát sv thường
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> SurveyRound()
        {
            var resTableStatus = await base.MakeRequestAuthorizedAsync("Get", $"/api/Statistic/GetStatusTempDataNormalSurveyTask");
            ViewData["TempDataNormalSurvey"] = await resTableStatus.Content.ReadAsStringAsync();

            return View("~/Views/Survey/Normal/SurveyRound.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> GetAllSurveyRound(DataTableRequest request)
        {
            return await GetDataTabeFromApi<SurveyRound>(request, "/api/SurveyRound/GetSurveyRound");
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> GetSurveyRoundById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/SurveyRound/GetSurveyRoundById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> CreateSurveyRound(SurveyRoundCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/SurveyRound/CreateSurveyRound", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> UpdateSurveyRound(SurveyRoundUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/SurveyRound/UpdateSurveyRound?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> OpenSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/SurveyRound/OpenSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> CloseSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/SurveyRound/CloseSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> DeleteSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/SurveyRound/DeleteSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> AddEndDateSurveyRound(string id, DateTime? endDate)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { endDate }), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/SurveyRound/AddEndDateSurveyRound?id={id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> GetTempDataNormalSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/Statistic/GetTempDataNormalSurvey?surveyRoundId={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> SendUrgingEmail()
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/Statistic/SendUrgingEmail");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> PreviewUrgingEmail()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/Statistic/PreviewUrgingEmail");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region bài khảo sát sv thường
        //[HttpGet]
        //public async Task<ActionResult> TheSurvey()
        //{
        //    var resTableStatus = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetGenerateTheSurveyStudentStatus");
        //    ViewData["TableTheSurveyStudentStatus"] = await resTableStatus.Content.ReadAsStringAsync();

        //    var resSurveyRound = await base.MakeRequestAuthorizedAsync("Get", $"/api/SurveyRound/GetSurveyRoundActive");
        //    ViewData["SurveyRoundActive"] = await resSurveyRound.Content.ReadAsStringAsync();

        //    var resExam = await base.MakeRequestAuthorizedAsync("Get", $"/api/ExamQuestions/GetAll");
        //    ViewData["ExamQuestions"] = await resExam.Content.ReadAsStringAsync();

        //    return View("~/Views/Survey/Normal/TheSurvey.cshtml");
        //}

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> TheSurvey(string surveyRoundId)
        {
            var resTableStatus = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetGenerateTheSurveyStudentStatus");
            ViewData["TableTheSurveyStudentStatus"] = await resTableStatus.Content.ReadAsStringAsync();

            var resExam = await base.MakeRequestAuthorizedAsync("Get", $"/api/ExamQuestions/GetAll");
            ViewData["ExamQuestions"] = await resExam.Content.ReadAsStringAsync();

            var resCount = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/CountGenerateTheSurveyStudent?surveyRoundId={surveyRoundId}");
            ViewData["CountGenerateTheSurveyStudent"] = await resCount.Content.ReadAsStringAsync();

            ViewData["surveyRoundId"] = surveyRoundId;
            return View("~/Views/Survey/Normal/TheSurvey.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> GetAllTheSurvey(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/TheSurvey/GetTheSurvey", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<TheSurvey>>(jsonString);
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> GetTheSurveyById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurvey/GetTheSurveyById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> CreateTheSurvey(TheSurveyCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/TheSurvey/CreateTheSurvey", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> UpdateTheSurvey(GraduateTheSurveyUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/TheSurvey/UpdateTheSurvey?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> DeleteTheSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/TheSurvey/DeleteTheSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> CloseTheSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/TheSurvey/CloseTheSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> GenerateTheSurveyStudent(string surveyRoundId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/TheSurveyStudent/GenerateTheSurveyStudent?surveyRoundId={surveyRoundId}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region thống kê sv thường
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> StatisticNormalSurvey()
        {
            var resTableStatus = await base.MakeRequestAuthorizedAsync("Get", $"/api/Statistic/GetStatusReportTotalNormalSurveyTask");
            ViewData["TableReportNormalStatus"] = await resTableStatus.Content.ReadAsStringAsync();

            //trạng thái export file
            var resExportStatus = await base.MakeRequestAuthorizedAsync("Get", $"/api/Statistic/GetStatusExportReportTotalNormalSurveyTask");
            ViewData["ExportReportTotalNormalStatus"] = await resExportStatus.Content.ReadAsStringAsync();

            //tồn tại file export file
            var resExistReportTotalNormalSurvey = await base.MakeRequestAuthorizedAsync("Get", $"/api/Statistic/CheckExistReportTotalNormalSurvey");
            if(resExistReportTotalNormalSurvey.IsSuccessStatusCode)
            {
                ViewData["isExistReportTotalNormalSurvey"] = true;
            } 
            else
            {
                ViewData["isExistReportTotalNormalSurvey"] = false;
            }

            var resSurveyRound = await base.MakeRequestAuthorizedAsync("Get", $"/api/SurveyRound/GetSurveyRoundClosedOrEnd");
            ViewData["SurveyRoundClosedOrEnd"] = await resSurveyRound.Content.ReadAsStringAsync();

            return View("~/Views/Survey/Normal/Statistic.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> GetRawReportTotalNormalSurvey(DataTableRequest request)
        {
            return await GetDataTabeFromApi<ReportTotalNormal>(request, "/api/Statistic/GetRawReportTotalNormalSurvey");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> ReportTotalNormalSurvey(string surveyRoundId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/Statistic/ReportTotalNormalSurvey?surveyRoundId={surveyRoundId}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> ExportReportTotalNormalSurvey(List<string> surveyRoundIds)
        {
            var content = new StringContent(JsonConvert.SerializeObject(surveyRoundIds), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/Statistic/ExportReportTotalNormalSurvey", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> DownloadReportTotalNormalSurvey()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/Statistic/DownloadReportTotalNormalSurvey");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await streamToReadFrom.CopyToAsync(memoryStream);
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Kết xuất.xlsx");
                    }
                }
            }
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Normal)]
        public async Task<ActionResult> ExportStudentDidSurvey(string surveyRoundId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/Statistic/ExportStudentDidSurvey?surveyRoundId={surveyRoundId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await streamToReadFrom.CopyToAsync(memoryStream);
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ds sinh viên tham gia.xlsx");
                    }
                }
            }
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region question cựu sv
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public ActionResult GraduateQuestion()
        {
            return View("~/Views/Survey/Graduate/Question.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GetAllGraduateQuestion(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateQuestion/GetAll", stringContent);
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public ActionResult CreateGraduateQuestion()
        {
            return View("~/Views/Survey/Graduate/CreateQuestion.cshtml", new QuestionCreate());
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> CreateGraduateQuestionSubmit(QuestionCreate question)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Survey/Graduate/CreateQuestion.cshtml", question);
            }
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/Graduatequestion/create", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Thêm thành công";
                    return View("~/Views/Survey/Graduate/CreateQuestion.cshtml", question);
                },
                action500Async: async res =>
                {
                    ViewData["UpdateErrorMessage"] = "Thêm không thành công";
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["UpdateErrorMessageDetail"] = resMess.message;
                    return View("~/Views/Survey/Graduate/CreateQuestion.cshtml", question);
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> DetailGraduateQuestion(string questionId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateQuestion/GetById?id={questionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<QuestionDetail>(jsonString);
                    return View("~/Views/Survey/Graduate/DetailQuestion.cshtml", data);
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> UpdateGraduateQuestion(QuestionDetail question)
        {
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("PUT", $"/api/Graduatequestion/update?id={question.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() });
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> DeleteGraduateQuestion(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/GraduateQuestion/Delete?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region answer cựu sv
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GraduateAnswer(string questionId)
        {
            string questionContent = "";
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/Graduatequestion/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                questionContent = (JsonConvert.DeserializeObject<Question>(jsonString)).content;
            }

            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateAnswer/GetByQuestionId?questionId={questionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var answers = JsonConvert.DeserializeObject<List<Answer>>(jsonString);

                    return View("~/Views/Survey/Graduate/Answer.cshtml", new AnswerOfQuestion()
                    {
                        Answers = answers,
                        QuestionContent = questionContent,
                        QuestionId = questionId
                    });
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> CreateGraduateAnswer(string questionId)
        {
            Question question = new Question();
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/Graduatequestion/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                question = (JsonConvert.DeserializeObject<Question>(jsonString));
            }

            return View("~/Views/Survey/Graduate/CreateAnswer.cshtml", new AnswerCreateOfQuestion
            {
                AnswerBind = new AnswerCreate()
                {
                    cauHoiId = questionId,
                    cauHoiCode = question.code
                },
                QuestionContent = HttpUtility.UrlEncode(question.content),
                QuestionType = question.type,
                QuestionId = questionId
            });
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> CreateGraduateAnswerSubmit(AnswerCreateOfQuestion answer)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Survey/Graduate/CreateAnswer.cshtml", answer);
            }
            var content = new StringContent(JsonConvert.SerializeObject(answer.AnswerBind), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/Graduateanswer/create", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    return Redirect($"/survey/graduateanswer?questionId={answer.QuestionId}");
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> DetailGraduateAnswer(string id, string questionId)
        {
            string questionContent = "";
            string questionType = "";
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/Graduatequestion/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                var question = (JsonConvert.DeserializeObject<Question>(jsonString));
                questionType = question.type;
                questionContent = question.content;
            }

            var response = await base.MakeRequestAuthorizedAsync("GET", $"/api/Graduateanswer/GetById?id={id}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var answer = JsonConvert.DeserializeObject<Answer>(jsonString);
                    if (answer.childQuestionId != null)
                    {
                        var resChildQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateQuestion/GetById?id={answer.childQuestionId}");
                        if (resChildQues.IsSuccessStatusCode)
                        {
                            var str = await resChildQues.Content.ReadAsStringAsync();
                            ViewData["childQuestion"] = str;
                        }
                    }
                    return View("~/Views/Survey/Graduate/DetailAnswer.cshtml", new UpdateAnswer
                    {
                        AnswerBind = answer,
                        QuestionContent = questionContent,
                        QuestionType = questionType,
                        QuestionId = questionId
                    });
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> UpdateGraduateAnswer(UpdateAnswer detail)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Survey/Graduate/DetailAnswer.cshtml", detail);
            }
            var content = new StringContent(JsonConvert.SerializeObject(detail.AnswerBind), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/Graduateanswer/update?id={detail.AnswerBind.id}", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Cập nhật thành công";
                    return Redirect($"/survey/Graduateanswer?questionId={detail.QuestionId}");
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> DeleteGraduateAnswer(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/GraduateAnswer/Delete?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region phiếu khảo sát cựu sv
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public ActionResult GraduateExamQuestions()
        {
            return View("~/Views/Survey/Graduate/ExamQuestions.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GetGraduateExamQuestions(DataTableRequest request)
        {
            return await GetDataTabeFromApi<ExamQuestions>(request, "/api/GraduateExamQuestions/GetAll");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> DeleteGraduateExam(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/GraduateExamQuestions/Delete?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public ActionResult GraduateExamStructure(string examQuestionId)
        {
            ViewData["ExamQuestionId"] = examQuestionId;
            return View("~/Views/Survey/Graduate/ExamStructure.cshtml");
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GetGraduateStructure(string examQuestionId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateExamQuestions/GetExamStructure?examQuestionId={examQuestionId}");
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public ActionResult CreateGraduateExamQuestions()
        {
            return View("~/Views/Survey/Graduate/ExamQuestions.cshtml", new ExamQuestionsCreate());
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> CreateGraduateExamQuestions(ExamQuestionsCreate exam)
        {
            var content = new StringContent(JsonConvert.SerializeObject(exam), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateExamQuestions/Create", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> AddGraduateQuestionExam(AddQuestionExam question)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { statusCode = HttpStatusCode.BadRequest }, JsonRequestBehavior.AllowGet);
            }
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateExamQuestions/AddQuestion", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GenerateGraduateExam(string examQuestionId, List<SortQuestion> sortResult)
        {
            var json = JsonConvert.SerializeObject(new { examQuestionId, sortResult });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateExamQuestions/GenerateExam", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GraduateExamDetail(string examQuestionId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateExamQuestions/GetExamDetailJsonString?examQuestionId={examQuestionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<QuestionJson>>(jsonString);
                    if (result == null)
                        return View("~/Views/Survey/Graduate/ExamDetail.cshtml", new List<QuestionJson>());
                    return View("~/Views/Survey/Graduate/ExamDetail.cshtml", result);
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> DeleteGraduateQuestionFromStructure(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/GraduateExamQuestions/DeleteQuestionFromStructure?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region thống kê cựu sv
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> ExportReportTotalGraduateSurvey(string surveyRoundId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/StatisticGraduate/ExportReportTotalGraduateSurvey?surveyRoundId={surveyRoundId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await streamToReadFrom.CopyToAsync(memoryStream);
                        memoryStream.ToArray();
                        var guid = Guid.NewGuid();
                        TempData[guid.ToString()] = new FileDownload()
                        {
                            FileName = "thong_ke.xlsx",
                            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            Data = memoryStream.ToArray()
                        };
                        return Json(new { statusCode = response.StatusCode, content = new { url = $"/survey/downloadexport?fileGuid={guid}" } }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region đợt khảo sát đã tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public ActionResult GraduateSurveyRound()
        {
            return View("~/Views/Survey/Graduate/GraduateSurveyRound.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GetAllGraduateSurveyRound(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateSurveyRound/GetSurveyRound", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<GraduateSurveyRound>>(jsonString);
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GetGraduateSurveyRoundById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateSurveyRound/GetSurveyRoundById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> CreateGraduateSurveyRound(GraduateSurveyRoundCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateSurveyRound/CreateSurveyRound", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> UpdateGraduateSurveyRound(GraduateSurveyRoundUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateSurveyRound/UpdateSurveyRound?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> CloseGraduateSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateSurveyRound/CloseSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> DeleteGraduateSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/GraduateSurveyRound/DeleteSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GetTempDataGraduateSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/StatisticGraduate/GetTempDataGraduateSurvey?surveyRoundId={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region sinh viên đã tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GraduateStudent()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateSurveyRound/GetSurveyRoundActive");
            ViewData["SurveyRoundActive"] =  await response.Content.ReadAsStringAsync();

            return View("~/Views/Survey/Graduate/GraduateStudent.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> TransferDataFromUndergraduate(string surveyRoundId, DateTime? fromDate, DateTime? toDate, List<string> HeTotNghieps)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { fromDate, toDate, HeTotNghieps }), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateStudent/TransferDataFromUndergraduate?surveyRoundId={surveyRoundId}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GetAllGraduateStudent(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateStudent/GetGraduateStudent", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<GraduateStudent>>(jsonString);
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> DownloadTemplateUploadFile()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateStudent/DownloadTemplateUploadFile");
            if(response.StatusCode == HttpStatusCode.OK)
            {
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await streamToReadFrom.CopyToAsync(memoryStream);
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", response.Content.Headers.ContentDisposition.FileName);
                    }
                }
            }
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> UploadFileGraudate(HttpPostedFileBase fileUpload, string surveyRoundId)
        {
            var contentLength = fileUpload.ContentLength;
            int maxSize = int.Parse(ConfigurationManager.AppSettings["MaxSizeFileUpload"]);
            if (contentLength > maxSize)
            {
                return Json(new { statusCode = HttpStatusCode.BadRequest, message = $"file lớn hơn {(int)(maxSize/1024)} KB" }, JsonRequestBehavior.AllowGet);
            }

            using (var memoryStream = new MemoryStream())
            {
                await fileUpload.InputStream.CopyToAsync(memoryStream);

                var byteArrayContent = new ByteArrayContent(memoryStream.ToArray());
                byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse(fileUpload.ContentType);

                var stringContent = new StringContent($"{surveyRoundId}", Encoding.UTF8, "application/json");

                var multipartFormData = new MultipartFormDataContent();
                multipartFormData.Add(byteArrayContent, "fileUpload", fileUpload.FileName);

                var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateStudent/UploadFile?surveyRoundId={surveyRoundId}", multipartFormData);
                return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DeleteGraduateStudent(string mssv)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/GraduateStudent/Delete?studentCode={mssv}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> DeleteAllGraduateStudent()
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/GraduateStudent/DeleteAll");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region bài khảo sát đã tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GraduateTheSurvey()
        {
            var resTableStatus = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateTheSurveyStudent/GetGenerateTheSurveyStudentStatus");
            ViewData["TableTheSurveyStudentStatus"] = await resTableStatus.Content.ReadAsStringAsync();

            var resSurveyRound = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateSurveyRound/GetSurveyRoundActive");
            ViewData["SurveyRoundActive"] = await resSurveyRound.Content.ReadAsStringAsync();

            var resExam = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateExamQuestions/GetAll");
            ViewData["ExamQuestions"] = await resExam.Content.ReadAsStringAsync();

            return View("~/Views/Survey/Graduate/GraduateTheSurvey.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GetAllGraduateTheSurvey(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateTheSurvey/GetTheSurvey", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<GraduateTheSurvey>>(jsonString);
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GetGraduateTheSurveyById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateTheSurvey/GetTheSurveyById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> CreateGraduateTheSurvey(GraduateTheSurveyCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateTheSurvey/CreateTheSurvey", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> UpdateGraduateTheSurvey(GraduateTheSurveyUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateTheSurvey/UpdateTheSurvey?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> DeleteGraduateTheSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/GraduateTheSurvey/DeleteTheSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> CloseGraduateTheSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateTheSurvey/CloseTheSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate)]
        public async Task<ActionResult> GenerateGraduateTheSurveyStudent(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateTheSurveyStudent/GenerateTheSurveyStudent?theSurveyId={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region question sv trước tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public ActionResult UndergraduateQuestion()
        {
            return View("~/Views/Survey/Undergraduate/Question.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> GetAllUndergraduateQuestion(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateQuestion/GetAll", stringContent);
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public ActionResult CreateUndergraduateQuestion()
        {
            return View("~/Views/Survey/Undergraduate/CreateQuestion.cshtml", new QuestionCreate());
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> CreateUndergraduateQuestionSubmit(QuestionCreate question)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Survey/Undergraduate/CreateQuestion.cshtml", question);
            }
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/Undergraduatequestion/create", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Thêm thành công";
                    return View("~/Views/Survey/Undergraduate/CreateQuestion.cshtml", question);
                },
                action500Async: async res =>
                {
                    ViewData["UpdateErrorMessage"] = "Thêm không thành công";
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["UpdateErrorMessageDetail"] = resMess.message;
                    return View("~/Views/Survey/Undergraduate/CreateQuestion.cshtml", question);
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DetailUndergraduateQuestion(string questionId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateQuestion/GetById?id={questionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<QuestionDetail>(jsonString);
                    return View("~/Views/Survey/Undergraduate/DetailQuestion.cshtml", data);
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> UpdateUndergraduateQuestion(QuestionDetail question)
        {
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("PUT", $"/api/Undergraduatequestion/update?id={question.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() });
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DeleteUndergraduateQuestion(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/UndergraduateQuestion/Delete?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region answer sv trước tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> UndergraduateAnswer(string questionId)
        {
            string questionContent = "";
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/Undergraduatequestion/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                questionContent = (JsonConvert.DeserializeObject<Question>(jsonString)).content;
            }

            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateAnswer/GetByQuestionId?questionId={questionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var answers = JsonConvert.DeserializeObject<List<Answer>>(jsonString);

                    return View("~/Views/Survey/Undergraduate/Answer.cshtml", new AnswerOfQuestion()
                    {
                        Answers = answers,
                        QuestionContent = questionContent,
                        QuestionId = questionId
                    });
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> CreateUndergraduateAnswer(string questionId)
        {
            Question question = new Question();
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/Undergraduatequestion/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                question = (JsonConvert.DeserializeObject<Question>(jsonString));
            }

            return View("~/Views/Survey/Undergraduate/CreateAnswer.cshtml", new AnswerCreateOfQuestion
            {
                AnswerBind = new AnswerCreate()
                {
                    cauHoiId = questionId,
                    cauHoiCode = question.code
                },
                QuestionContent = HttpUtility.UrlEncode(question.content),
                QuestionType = question.type,
                QuestionId = questionId
            });
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> CreateUndergraduateAnswerSubmit(AnswerCreateOfQuestion answer)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Survey/Undergraduate/CreateAnswer.cshtml", answer);
            }
            var content = new StringContent(JsonConvert.SerializeObject(answer.AnswerBind), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/Undergraduateanswer/create", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    return Redirect($"/survey/Undergraduateanswer?questionId={answer.QuestionId}");
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DetailUndergraduateAnswer(string id, string questionId)
        {
            string questionContent = "";
            string questionType = "";
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/Undergraduatequestion/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                var question = (JsonConvert.DeserializeObject<Question>(jsonString));
                questionType = question.type;
                questionContent = question.content;
            }

            var response = await base.MakeRequestAuthorizedAsync("GET", $"/api/Undergraduateanswer/GetById?id={id}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var answer = JsonConvert.DeserializeObject<Answer>(jsonString);
                    if (answer.childQuestionId != null)
                    {
                        var resChildQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateQuestion/GetById?id={answer.childQuestionId}");
                        if (resChildQues.IsSuccessStatusCode)
                        {
                            var str = await resChildQues.Content.ReadAsStringAsync();
                            ViewData["childQuestion"] = str;
                        }
                    }
                    return View("~/Views/Survey/Undergraduate/DetailAnswer.cshtml", new UpdateAnswer
                    {
                        AnswerBind = answer,
                        QuestionContent = questionContent,
                        QuestionType = questionType,
                        QuestionId = questionId
                    });
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> UpdateUndergraduateAnswer(UpdateAnswer detail)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Survey/Undergraduate/DetailAnswer.cshtml", detail);
            }
            var content = new StringContent(JsonConvert.SerializeObject(detail.AnswerBind), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/Undergraduateanswer/update?id={detail.AnswerBind.id}", content);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Cập nhật thành công";
                    return Redirect($"/survey/Undergraduateanswer?questionId={detail.QuestionId}");
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DeleteUndergraduateAnswer(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/UndergraduateAnswer/Delete?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region phiếu khảo sát sv trước tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public ActionResult UndergraduateExamQuestions()
        {
            return View("~/Views/Survey/Undergraduate/ExamQuestions.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> GetUndergraduateExamQuestions(DataTableRequest request)
        {
            return await GetDataTabeFromApi<ExamQuestions>(request, "/api/UndergraduateExamQuestions/GetAll");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DeleteUndergraduateExam(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/UndergraduateExamQuestions/Delete?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public ActionResult UndergraduateExamStructure(string examQuestionId)
        {
            ViewData["ExamQuestionId"] = examQuestionId;
            return View("~/Views/Survey/Undergraduate/ExamStructure.cshtml");
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> GetUndergraduateStructure(string examQuestionId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateExamQuestions/GetExamStructure?examQuestionId={examQuestionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<ExamStructure>>(jsonString);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> CreateUndergraduateExamQuestions(ExamQuestionsCreate exam)
        {
            var content = new StringContent(JsonConvert.SerializeObject(exam), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateExamQuestions/Create", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> AddUndergraduateQuestionExam(AddQuestionExam question)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { statusCode = HttpStatusCode.BadRequest }, JsonRequestBehavior.AllowGet);
            }
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateExamQuestions/AddQuestion", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> GenerateUndergraduateExam(string examQuestionId, List<SortQuestion> sortResult)
        {
            var json = JsonConvert.SerializeObject(new { examQuestionId, sortResult });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateExamQuestions/GenerateExam", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> UndergraduateExamDetail(string examQuestionId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateExamQuestions/GetExamDetailJsonString?examQuestionId={examQuestionId}");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<QuestionJson>>(jsonString);
                    if (result == null)
                        return View("~/Views/Survey/Undergraduate/ExamDetail.cshtml", new List<QuestionJson>());
                    return View("~/Views/Survey/Undergraduate/ExamDetail.cshtml", result);
                }
            );
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DeleteUndergraduateQuestionFromStructure(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/UndergraduateExamQuestions/DeleteQuestionFromStructure?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region đợt khảo sát trước tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public ActionResult UndergraduateSurveyRound()
        {
            return View("~/Views/Survey/Undergraduate/UndergraduateSurveyRound.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> GetAllUndergraduateSurveyRound(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateSurveyRound/GetSurveyRound", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<UndergraduateSurveyRound>>(jsonString);
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> GetUndergraduateSurveyRoundById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateSurveyRound/GetSurveyRoundById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> CreateUndergraduateSurveyRound(GraduateSurveyRoundCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateSurveyRound/CreateSurveyRound", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> UpdateUndergraduateSurveyRound(GraduateSurveyRoundUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateSurveyRound/UpdateSurveyRound?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> OpenUndergraduateSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateSurveyRound/OpenSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> CloseUndergraduateSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateSurveyRound/CloseSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DeleteUndergraduateSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/UndergraduateSurveyRound/DeleteSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region sinh viên trước tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> UndergraduateStudent()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateSurveyRound/GetSurveyRoundActive");
            ViewData["SurveyRoundActive"] = await response.Content.ReadAsStringAsync();

            response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateSurveyRound/GetAllSurveyRound");
            ViewData["AllSurveyRound"] = await response.Content.ReadAsStringAsync();

            return View("~/Views/Survey/Undergraduate/UndergraduateStudent.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> GetAllUndergraduateStudent(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateStudent/GetUndergraduateStudent", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<UndergraduateStudent>>(jsonString);
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DownloadTemplateUploadFileUndergraudate()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateStudent/DownloadTemplateUploadFile");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await streamToReadFrom.CopyToAsync(memoryStream);
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", response.Content.Headers.ContentDisposition.FileName);
                    }
                }
            }
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> UploadFileUndergraudate(HttpPostedFileBase fileUpload, string surveyRoundId)
        {
            var contentLength = fileUpload.ContentLength;
            int maxSize = int.Parse(ConfigurationManager.AppSettings["MaxSizeFileUpload"]);
            if (contentLength > maxSize)
            {
                return Json(new { statusCode = HttpStatusCode.BadRequest, message = $"file lớn hơn {(int)(maxSize / 1024)} KB" }, JsonRequestBehavior.AllowGet);
            }

            using (var memoryStream = new MemoryStream())
            {
                await fileUpload.InputStream.CopyToAsync(memoryStream);

                var byteArrayContent = new ByteArrayContent(memoryStream.ToArray());
                byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse(fileUpload.ContentType);

                var stringContent = new StringContent($"{surveyRoundId}", Encoding.UTF8, "application/json");

                var multipartFormData = new MultipartFormDataContent();
                multipartFormData.Add(byteArrayContent, "fileUpload", fileUpload.FileName);

                var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateStudent/UploadFile?surveyRoundId={surveyRoundId}", multipartFormData);
                return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DownloadListStudent(string surveyRoundId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateStudent/DownloadListStudent?surveyRoundId={surveyRoundId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await streamToReadFrom.CopyToAsync(memoryStream);
                        memoryStream.ToArray();
                        var guid = Guid.NewGuid();
                        TempData[guid.ToString()] = new FileDownload()
                        {
                            FileName = "danh_sach_sinh_vien.xlsx",
                            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            Data = memoryStream.ToArray()
                        };
                        return Json(new { statusCode = response.StatusCode, content = new { url = $"/survey/downloadexport?fileGuid={guid}" } }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate, RoleNames.KhaoThi_Survey_Graduate)]
        public ActionResult DownloadExport(Guid fileGuid)
        {
            return base.DownloadFileFromTempData(fileGuid);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DeleteUndergraduateStudent(string mssv)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/UndergraduateStudent/Delete?studentCode={mssv}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DeleteAllUndergraduateStudent()
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/UndergraduateStudent/DeleteAll");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region bài khảo sát trước tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> UndergraduateTheSurvey()
        {
            var resTableStatus = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateTheSurveyStudent/GetGenerateTheSurveyStudentStatus");
            ViewData["TableTheSurveyStudentStatus"] = await resTableStatus.Content.ReadAsStringAsync();

            var resExam = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateExamQuestions/GetAll");
            ViewData["ExamQuestions"] = await resExam.Content.ReadAsStringAsync();

            return View("~/Views/Survey/Undergraduate/UndergraduateTheSurvey.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> GetAllUndergraduateTheSurvey(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateTheSurvey/GetTheSurvey", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<UndergraduateTheSurvey>>(jsonString);
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
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> GetUndergraduateTheSurveyById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateTheSurvey/GetTheSurveyById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> CreateUndergraduateTheSurvey(UndergraduateTheSurveyCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateTheSurvey/CreateTheSurvey", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> UpdateUndergraduateTheSurvey(UndergraduateTheSurveyUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateTheSurvey/UpdateTheSurvey?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> DeleteUndergraduateTheSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/UndergraduateTheSurvey/DeleteTheSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> GenerateUndergraduateTheSurveyStudent(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateTheSurveyStudent/GenerateTheSurveyStudent?theSurveyId={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region thống kê sv trước tốt nghiệp
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> StatisticUndergraduateSurvey()
        {
            var resSurveyRound = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateSurveyRound/GetSurveyRoundClosedOrEnd");
            ViewData["SurveyRoundClosedOrEnd"] = await resSurveyRound.Content.ReadAsStringAsync();

            var resTheSurvey = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateTheSurvey/GetTheSurveyDoing");
            ViewData["TheSurveys"] = await resTheSurvey.Content.ReadAsStringAsync();

            return View("~/Views/Survey/Undergraduate/Statistic.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> GetRawReportTotalUndergraduateSurvey(DataTableRequest request)
        {
            return await GetDataTabeFromApi<ReportTotalUndergraduate>(request, "/api/StatisticUndergraduate/GetRawReportTotalUndergraduateSurvey");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> ReportTotalUndergraduateSurvey(string surveyRoundId, string theSurveyId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/StatisticUndergraduate/ReportTotalUndergraduateSurvey?surveyRoundId={surveyRoundId}&theSurveyId={theSurveyId}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Undergraduate)]
        public async Task<ActionResult> ExportReportTotalUndergraduateSurvey(string surveyRoundId, string theSurveyId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/StatisticUndergraduate/ExportReportTotalUndergraduateSurvey?surveyRoundId={surveyRoundId}&theSurveyId={theSurveyId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await streamToReadFrom.CopyToAsync(memoryStream);
                        memoryStream.ToArray();
                        var guid = Guid.NewGuid();
                        TempData[guid.ToString()] = new FileDownload()
                        {
                            FileName = "bao_cao.xlsx",
                            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            Data = memoryStream.ToArray()
                        };
                        return Json(new { statusCode = response.StatusCode, content = new { url = $"/survey/downloadexport?fileGuid={guid}" } }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cập nhật chú ý khảo sát
        [HttpGet]
        public ActionResult Editsurveynotice(string paracode)
        {
            TempData["paracode"] = paracode;
            return View("EditSurveyNotice");
        }
        #endregion
    }
}