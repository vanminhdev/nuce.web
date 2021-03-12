using Newtonsoft.Json;
using nuce.web.shared;
using nuce.web.survey.student.Attributes.ActionFilter;
using nuce.web.survey.student.Models.Base;
using nuce.web.survey.student.Models.JsonData;
using nuce.web.survey.student.Models.Survey.Graduate;
using nuce.web.survey.student.Models.Survey.Undergaduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    public class SurveyController : BaseController
    {
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan)]
        public ActionResult Faculty()
        {
            return View("~/Views/Survey/Faculty/Index.cshtml");
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan)]
        public ActionResult GraduateTheSurveyStudent()
        {
            return View("~/Views/Survey/Faculty/GraduateTheSurveyStudent.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan)]
        public async Task<ActionResult> GetGraduateTheSurveyStudent(DataTableRequest request)
        {
            return await GetDataTabeFromApi<GraduateStudent>(request, "/api/GraduateStudent/GetGraduateStudent");
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan)]
        public async Task<ActionResult> GetGraduateTheSurveyStudent(string studentCode)
        {
            var resSelectedAnswer = await base.MakeRequestAuthorizedAsync("Get", $"/api/GraduateTheSurveyStudent/GetTheSurveyStudent?studentCode={studentCode}");
            return await base.HandleResponseAsync(resSelectedAnswer,
                action200Async: async res =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    if (jsonString != "")
                    {
                        var theSurveyStudent = JsonConvert.DeserializeObject<TheSurveyStudent>(jsonString);
                        return Redirect($"/graduatehome/thesurvey?theSurveyId={theSurveyStudent.baiKhaoSatId}&studentCode={studentCode}");
                    }
                    return Redirect($"/error?message={HttpUtility.UrlEncode("Sinh viên không có bài khảo sát")}&code={(int)HttpStatusCode.NotFound}");
                }
            );
        }

        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan)]
        public ActionResult UndergraduateStudent()
        {
            return View("~/Views/Survey/Faculty/UndergraduateStudent.cshtml");
        }

        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_KhoaBan)]
        public async Task<ActionResult> GetAllUndergraduateStudent(DataTableRequest request)
        {
            return await GetDataTabeFromApi<UndergraduateStudent>(request, "/api/UndergraduateStudent/GetUndergraduateStudent");
        }
    }
}