using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    [Route("")]
    public class KhaoSatController : Controller
    {
        [HttpGet]
        [Route("khaosatnhaphoc")]
        public ActionResult KhaoSatNhapHoc()
        {
            return View("~/Views/KhaoSat/KhaoSatNhapHoc.cshtml");
        }
    }
}