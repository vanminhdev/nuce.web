using nuce.web.quanly.Models;
using nuce.web.quanly.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    public class ManagerRestoreController : BaseController
    {
        // GET: Backup
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> BackupHistory(DataTableRequest request)
        {
            return await GetDataTabeFromApi<HistoryBackup>(request, "/api/ManagerRestore/GetHistoryBackup");
        }

        [HttpPost]
        public async Task<ActionResult> BackupDatabaseSurvey()
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/ManagerRestore/BackupDatabaseSurvey");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> RestoreDatabaseSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/ManagerRestore/RestoreDatabaseSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
    }
}