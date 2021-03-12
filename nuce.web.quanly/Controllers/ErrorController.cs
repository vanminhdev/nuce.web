using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult Index(string message, int code)
        {
            ViewData["Title"] = code;
            ViewData["Message"] = message;
            ViewData["Code"] = code;
            return View();
        }

        [HttpGet]
        public ActionResult UnderConstruction()
        {
            return View();
        }
    }
}