﻿using nuce.web.quanly.Attributes.ActionFilter;
using nuce.web.quanly.Models;
using nuce.web.quanly.ViewModel.Base;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    public class ManagerRestoreController : BaseController
    {
        // GET: Backup
        public ActionResult Index(int type)
        {
            ViewData["type"] = type;
            return View();
        }

        [AuthorizeActionFilter(RoleNames.Admin, RoleNames.KhaoThi_Survey_Normal, RoleNames.KhaoThi_Survey_Undergraduate, RoleNames.KhaoThi_Survey_Graduate)]
        [HttpPost]
        public async Task<ActionResult> BackupHistory(int type, DataTableRequest request)
        {
            return await GetDataTabeFromApi<HistoryBackup>(request, $"/api/ManagerRestore/GetHistoryBackup/{type}");
        }

        [AuthorizeActionFilter(RoleNames.Admin, RoleNames.KhaoThi_Survey_Normal, RoleNames.KhaoThi_Survey_Undergraduate, RoleNames.KhaoThi_Survey_Graduate)]
        [HttpPost]
        public async Task<ActionResult> BackupDatabaseSurvey(int type)
        {
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/ManagerRestore/BackupDatabaseSurvey/{type}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> RestoreDatabaseSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/ManagerRestore/RestoreDatabaseSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadFileBackupDatabaseSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/ManagerRestore/DownloadFileBackupDatabaseSurvey?id={id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await streamToReadFrom.CopyToAsync(memoryStream);
                        return File(memoryStream.ToArray(), "application/octet-stream", $"Backup_{DateTime.Now:dd_MM_yyyy_HH_mm}.bak");
                    }
                }
            }
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteHistoryBackupDatabaseSurvey(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/ManagerRestore/DeleteHistoryBackupDatabaseSurvey?id={id}");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }
    }
}