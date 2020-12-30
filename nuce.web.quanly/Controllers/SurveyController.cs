using Newtonsoft.Json;
using nuce.web.quanly.Attributes.ActionFilter;
using nuce.web.quanly.Attributes.ValidationAttributes;
using nuce.web.quanly.Models;
using nuce.web.quanly.Models.JsonData;
using nuce.web.quanly.Models.Survey.Graduate;
using nuce.web.quanly.Models.Survey.Normal;
using nuce.web.quanly.Models.Survey.Undergraduate;
using nuce.web.quanly.ViewModel.Base;
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
                    ViewData["UpdateSuccessMessage"] = "Thêm thành công";
                    return View("CreateQuestion", question);
                },
                action500Async: async res =>
                {
                    ViewData["UpdateErrorMessage"] = "Thêm không thành công";
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["UpdateErrorMessageDetail"] = resMess.message;
                    return View("CreateQuestion", question);
                }
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
        public async Task<ActionResult> UpdateQuestion(QuestionDetail question)
        {
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("PUT", $"/api/question/update?id={question.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() });
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
                return Json(new { statusCode = HttpStatusCode.BadRequest }, JsonRequestBehavior.AllowGet);
            }
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/ExamQuestions/AddQuestion", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GenerateExam(string examQuestionId)
        {
            var content = new StringContent($@"'{examQuestionId}'", Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/ExamQuestions/GenerateExam", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public async Task<ActionResult> DeleteQuestionFromStructure(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/ExamQuestions/DeleteQuestionFromStructure?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region thống kê
        [HttpGet]
        public async Task<ActionResult> Statistic()
        {
            var resTableStatus = await base.MakeRequestAuthorizedAsync("Get", $"/api/Statistic/GetStatusReportTotalNormalSurveyTask");
            ViewData["TableReportNormalStatus"] = await resTableStatus.Content.ReadAsStringAsync();

            var resSurveyRound = await base.MakeRequestAuthorizedAsync("Get", $"/api/SurveyRound/GetSurveyRoundEnd");
            ViewData["SurveyRoundEnd"] = await resSurveyRound.Content.ReadAsStringAsync();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ReportTotalNormalSurvey(string surveyRoundId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/Statistic/ReportTotalNormalSurvey?surveyRoundId={surveyRoundId}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region đợt khảo sát
        [HttpGet]
        public ActionResult SurveyRound()
        {
            return View("~/Views/Survey/Normal/SurveyRound.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> GetAllSurveyRound(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/SurveyRound/GetSurveyRound", stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<SurveyRound>>(jsonString);
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
        public async Task<ActionResult> GetSurveyRoundById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/SurveyRound/GetSurveyRoundById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSurveyRound(SurveyRoundCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/SurveyRound/CreateSurveyRound", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateSurveyRound(SurveyRoundUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/SurveyRound/UpdateSurveyRound?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> OpenSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/SurveyRound/OpenSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CloseSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/SurveyRound/CloseSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/SurveyRound/DeleteSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AddEndDateSurveyRound(string id, DateTime? endDate)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { endDate }), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/SurveyRound/AddEndDateSurveyRound?id={id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region bài khảo sát
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
        public async Task<ActionResult> TheSurvey(string surveyRoundId)
        {
            var resTableStatus = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurveyStudent/GetGenerateTheSurveyStudentStatus");
            ViewData["TableTheSurveyStudentStatus"] = await resTableStatus.Content.ReadAsStringAsync();

            var resSurveyRound = await base.MakeRequestAuthorizedAsync("Get", $"/api/SurveyRound/GetSurveyRoundActive");
            ViewData["SurveyRoundActive"] = await resSurveyRound.Content.ReadAsStringAsync();

            var resExam = await base.MakeRequestAuthorizedAsync("Get", $"/api/ExamQuestions/GetAll");
            ViewData["ExamQuestions"] = await resExam.Content.ReadAsStringAsync();

            ViewData["surveyRoundId"] = surveyRoundId;
            return View("~/Views/Survey/Normal/TheSurveyOfSurveyRound.cshtml");
        }

        [HttpPost]
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
        public async Task<ActionResult> GetTheSurveyById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/TheSurvey/GetTheSurveyById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTheSurvey(TheSurveyCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/TheSurvey/CreateTheSurvey", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTheSurvey(GraduateTheSurveyUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/TheSurvey/UpdateTheSurvey?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTheSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/TheSurvey/DeleteTheSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CloseTheSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/TheSurvey/CloseTheSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GenerateTheSurveyStudent(string surveyRoundId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/TheSurveyStudent/GenerateTheSurveyStudent?surveyRoundId={surveyRoundId}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region đợt khảo sát đã tốt nghiệp
        [HttpGet]
        public ActionResult GraduateSurveyRound()
        {
            return View("~/Views/Survey/Graduate/GraduateSurveyRound.cshtml");
        }

        [HttpPost]
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
        public async Task<ActionResult> GetGraduateSurveyRoundById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateSurveyRound/GetSurveyRoundById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateGraduateSurveyRound(GraduateSurveyRoundCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateSurveyRound/CreateSurveyRound", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateGraduateSurveyRound(GraduateSurveyRoundUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateSurveyRound/UpdateSurveyRound?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CloseGraduateSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateSurveyRound/CloseSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteGraduateSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/GraduateSurveyRound/DeleteSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region sinh viên đã tốt nghiệp
        [HttpGet]
        public async Task<ActionResult> GraduateStudent()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateSurveyRound/GetSurveyRoundActive");
            ViewData["SurveyRoundActive"] =  await response.Content.ReadAsStringAsync();

            return View("~/Views/Survey/Graduate/GraduateStudent.cshtml");
        }

        [HttpPost]
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
        public async Task<ActionResult> DeleteAllGraduateStudent()
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/GraduateStudent/DeleteAll");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region bài khảo sát đã tốt nghiệp
        [HttpGet]
        public async Task<ActionResult> GraduateTheSurvey()
        {
            var resTableStatus = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateTheSurveyStudent/GetGenerateTheSurveyStudentStatus");
            ViewData["TableTheSurveyStudentStatus"] = await resTableStatus.Content.ReadAsStringAsync();

            var resSurveyRound = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateSurveyRound/GetSurveyRoundActive");
            ViewData["SurveyRoundActive"] = await resSurveyRound.Content.ReadAsStringAsync();

            var resExam = await base.MakeRequestAuthorizedAsync("Get", $"/api/ExamQuestions/GetAll");
            ViewData["ExamQuestions"] = await resExam.Content.ReadAsStringAsync();

            return View("~/Views/Survey/Graduate/GraduateTheSurvey.cshtml");
        }

        [HttpPost]
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
        public async Task<ActionResult> GetGraduateTheSurveyById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateTheSurvey/GetTheSurveyById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateGraduateTheSurvey(GraduateTheSurveyCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateTheSurvey/CreateTheSurvey", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateGraduateTheSurvey(GraduateTheSurveyUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateTheSurvey/UpdateTheSurvey?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteGraduateTheSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/GraduateTheSurvey/DeleteTheSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CloseGraduateTheSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/GraduateTheSurvey/CloseTheSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GenerateGraduateTheSurveyStudent(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/GraduateTheSurveyStudent/GenerateTheSurveyStudent?theSurveyId={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region đợt khảo sát sắp tốt nghiệp
        [HttpGet]
        public ActionResult UndergraduateSurveyRound()
        {
            return View("~/Views/Survey/Undergraduate/UndergraduateSurveyRound.cshtml");
        }

        [HttpPost]
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
        public async Task<ActionResult> GetUndergraduateSurveyRoundById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateSurveyRound/GetSurveyRoundById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUndergraduateSurveyRound(GraduateSurveyRoundCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateSurveyRound/CreateSurveyRound", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUndergraduateSurveyRound(GraduateSurveyRoundUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateSurveyRound/UpdateSurveyRound?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> OpenUndergraduateSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateSurveyRound/OpenSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CloseUndergraduateSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateSurveyRound/CloseSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUndergraduateSurveyRound(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/UndergraduateSurveyRound/DeleteSurveyRound?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region sinh viên sắp tốt nghiệp
        [HttpGet]
        public async Task<ActionResult> UndergraduateStudent()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateSurveyRound/GetSurveyRoundActive");
            ViewData["SurveyRoundActive"] = await response.Content.ReadAsStringAsync();

            response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateSurveyRound/GetAllSurveyRound");
            ViewData["AllSurveyRound"] = await response.Content.ReadAsStringAsync();

            return View("~/Views/Survey/Undergraduate/UndergraduateStudent.cshtml");
        }

        [HttpPost]
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

        [HttpPost]
        public async Task<ActionResult> DeleteAllUndergraduateStudent()
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/UndergraduateStudent/DeleteAll");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region bài khảo sát sắp tốt nghiệp
        [HttpGet]
        public async Task<ActionResult> UndergraduateTheSurvey()
        {
            var resTableStatus = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateTheSurveyStudent/GetGenerateTheSurveyStudentStatus");
            ViewData["TableTheSurveyStudentStatus"] = await resTableStatus.Content.ReadAsStringAsync();

            var resExam = await base.MakeRequestAuthorizedAsync("Get", $"/api/ExamQuestions/GetAll");
            ViewData["ExamQuestions"] = await resExam.Content.ReadAsStringAsync();

            return View("~/Views/Survey/Undergraduate/UndergraduateTheSurvey.cshtml");
        }

        [HttpPost]
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
        public async Task<ActionResult> GetUndergraduateTheSurveyById(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/UndergraduateTheSurvey/GetTheSurveyById?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUndergraduateTheSurvey(UndergraduateTheSurveyCreate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateTheSurvey/CreateTheSurvey", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUndergraduateTheSurvey(UndergraduateTheSurveyUpdate data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/UndergraduateTheSurvey/UpdateTheSurvey?id={data.id}", content);
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUndergraduateTheSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/UndergraduateTheSurvey/DeleteTheSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GenerateUndergraduateTheSurveyStudent(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/UndergraduateTheSurveyStudent/GenerateTheSurveyStudent?theSurveyId={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}