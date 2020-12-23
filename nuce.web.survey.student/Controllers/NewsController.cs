using System.Web.Mvc;

namespace nuce.web.survey.student.Controllers
{
    public class NewsController : BaseController
    {
        // GET: News
        [HttpGet]
        public ActionResult Index(int catId)
        {
            TempData["catId"] = catId;
            return View("Index");
        }

        [HttpGet]
        public ActionResult Detail(int catId, int itemId)
        {
            TempData["catId"] = catId;
            TempData["itemId"] = itemId;
            return View("Detail");
        }

    }
}