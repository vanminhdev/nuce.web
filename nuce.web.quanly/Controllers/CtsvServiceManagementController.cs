using nuce.web.quanly.Areas.Admin.Models;
using nuce.web.quanly.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Areas.Admin.Controllers
{
    public class CtsvServiceManagementController : BaseController
    {
        // GET: Admin/CtsvServiceManagement
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string api = "api/DichVu/all-type-info";
            var response = await base.MakeRequestAuthorizedAsync("get", api);

            return await base.HandleResponseAsync(response, action200Async: async res =>
            {
                var serviceInfo = await base.DeserializeResponseAsync<Dictionary<int, ServiceManagementModel>>(res.Content);
                var viewModel = new { ServiceInfo = serviceInfo };
                return View("Index", serviceInfo);
            });
        }
    }
}