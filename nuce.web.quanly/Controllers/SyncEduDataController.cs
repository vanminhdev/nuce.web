using Newtonsoft.Json;
using nuce.web.quanly.Models;
using nuce.web.quanly.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    public class SyncEduDataController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SyncFromEduData()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SyncFromEduData(string action)
        {
            switch(action)
            {
                case "SyncFaculty":
                case "SyncDepartment":
                case "SyncAcademics":
                case "SyncSubject":
                case "SyncClass":
                case "SyncLecturer":
                case "SyncStudent":
                    break;
                default:
                    return Json(new { type = "fail", message = "Hành động không hợp lệ", detailMessage = "" },JsonRequestBehavior.AllowGet);
            }

            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/SyncEduData/{action}");
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    return Json(new { type = "success", message = "Đồng bộ thành công" }, JsonRequestBehavior.AllowGet);
                },
                action500Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    return Json(new { type = "fail", message = "Đồng bộ không thành công", detailMessage = resMess.message }, JsonRequestBehavior.AllowGet);
                }
            );
        }

        private async Task<ActionResult> GetDataApi(DataTableRequest request, string apiUri)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", apiUri, stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<Question>>(jsonString);
                    return Json(new
                    {
                        draw = data.Draw,
                        recordsTotal = data.RecordsTotal,
                        recordsFiltered = data.RecordsFiltered,
                        data = data.Data
                    });
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> GetSubject(DataTableRequest request)
        {
            return await GetDataApi(request, "/api/SyncEduData/GetSubject");
        }
    }
}