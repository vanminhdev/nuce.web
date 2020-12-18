using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    public class NewsController : BaseController
    {
        // GET: News
        public ActionResult Index(int catId)
        {
            TempData["catId"] = catId;
            return View("Index");
        }

        public ActionResult Detail(int itemId)
        {
            TempData["itemId"] = itemId;
            return View("Detail");
        }
    }
}