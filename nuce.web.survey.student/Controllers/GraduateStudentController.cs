using nuce.web.survey.student.Attributes.ActionFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    [GraduateActionFilter]
    public class GraduateStudentController : Controller
    {
        // GET: GraduateStudent
        public ActionResult Index()
        {
            return View();
        }
    }
}