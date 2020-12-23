﻿using nuce.web.quanly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    public class FrontendManagerController : BaseController
    {
        // GET: FrontendManager
        private Dictionary<int, string> roleDict = new Dictionary<int, string>
        {
            { 1, "P_KhaoThi" },
            { 2, "P_CTSV" },
        };
        // GET: NewsManagement
        [HttpGet]
        public async Task<ActionResult> Index(int role)
        {
            string roleName = "";
            if (roleDict.ContainsKey(role))
            {
                roleName = roleDict[role];
            }
            TempData["role"] = role;
            string api = $"api/NewsManager/admin/news-category/{roleName}";
            var response = await base.MakeRequestAuthorizedAsync("get", api);

            return await base.HandleResponseAsync(response, action200Async: async res =>
            {
                var model = await base.DeserializeResponseAsync<List<NewsCatsModel>>(res.Content);
                return View("Index", model);
            });
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string type, bool richText = false)
        {
            ViewData["richText"] = richText;
            string api = $"api/ClientParameters/admin/{type}";
            var response = await base.MakeRequestAuthorizedAsync("get", api);

            return await base.HandleResponseAsync(response, action200Async: async res =>
            {
                var model = await base.DeserializeResponseAsync<List<ClientParameterModel>>(res.Content);
                return View("Edit", model);
            });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateClientParameter(List<UpdateClientParameterModel> model)
        {
            string api = $"api/ClientParameters/admin/update";

            var content = base.MakeContent(model);

            var response = await base.MakeRequestAuthorizedAsync("put", api, content);

            return Json(new ResponseBody
            {
                Data = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode
            }, JsonRequestBehavior.AllowGet);
        }
    }
}