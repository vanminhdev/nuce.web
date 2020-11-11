using nuce.web.quanly.Models;
using nuce.web.quanly.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using nuce.web.quanly.ViewModel.Base;
using System.Net.Http;
using System.Net;

namespace nuce.web.quanly.Controllers
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

        [HttpGet]
        public ActionResult Detail(int id)
        {
            ViewData["id"] = id;
            return View("Detail");
        }

        [HttpPost]
        public async Task<ActionResult> UpdateStatus(UpdateStatusModel model)
        {
            string api = "api/dichVu/admin/update-status";
            var stringContent = base.MakeContent(model);
            var response = await base.MakeRequestAuthorizedAsync("put", api, stringContent);

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                return Redirect($"/notfound?message={HttpUtility.UrlEncode("Không có quyền truy cập")}");
            } else if (response.IsSuccessStatusCode)
            {
                return Json(response);
            }
            return Json(response);
        }

        private static Dictionary<string, string> ExportApiSet = new Dictionary<string, string>
        {
            { "word", "api/dichVu/admin/export-word" },
            { "word-list", "api/dichVu/admin/export-word-list" }
        };

        private static Dictionary<string, string> MimeTypeSet = new Dictionary<string, string>
        {
            { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { "zip", "application/zip" }
        };

        [HttpPost]
        public async Task<ActionResult> ExportFile(ExportModel model)
        {
            if (!ExportApiSet.ContainsKey(model.ExportType))
            {
                return null;
            }
            string api = ExportApiSet[model.ExportType];
            var stringContent = base.MakeContent(new { model.DichVuList, model.DichVuType });
            var response = await base.MakeRequestAuthorizedAsync("post", api, stringContent);

            if (response.IsSuccessStatusCode)
            {
                var byteFile = await response.Content.ReadAsByteArrayAsync();
                string id = Guid.NewGuid().ToString();
                TempData[id] = byteFile;
                return Json(new
                {
                    fileId = id
                });
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return Redirect($"/notfound?message={HttpUtility.UrlEncode("Không có quyền truy cập")}");
            }
            return null;
        }

        [HttpGet]
        public ActionResult DownloadFile(string id, string format)
        {
            if (TempData[id] != null)
            {
                byte[] data = (byte[])TempData[id];
                return File(data, MimeTypeSet[format] ?? "", $"{DateTime.Now.ToFileTime()}.{format}");
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult> SearchServiceRequest(DataTableRequest request)
        {
            string api = "api/dichVu/admin/search-request";
            var stringContent = base.MakeContent(request);
            var response = await base.MakeRequestAuthorizedAsync("post", api, stringContent);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return Redirect($"/notfound?message={HttpUtility.UrlEncode("Không có quyền truy cập")}");
            }
            else if (response.IsSuccessStatusCode)
            {
                var res = await base.DeserializeResponseAsync<DataTableResponse<QuanLyDichVuDetailModel>>(response.Content);
                return Json(new
                {
                    draw = res.Draw,
                    recordsTotal = res.RecordsTotal,
                    recordsFiltered = res.RecordsFiltered,
                    data = res.Data
                });                
            }
            return Json(new
            {
                draw = ++request.Draw,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new List<object>()
            });
        }
    }
}