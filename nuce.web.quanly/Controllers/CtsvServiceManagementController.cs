﻿using nuce.web.quanly.Models;
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
using nuce.web.quanly.Attributes.ActionFilter;
using nuce.web.shared;
using Newtonsoft.Json;

namespace nuce.web.quanly.Controllers
{
    [AuthorizeActionFilter(RoleNames.CTSV)]
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

        [HttpGet]
        public ActionResult Parameters()
        {
            return View("Parameters");
        }

        [HttpPost]
        public async Task<ActionResult> UpdateStatus(UpdateStatusModel model)
        {
            string api = "api/dichVu/admin/update-status";
            var stringContent = base.MakeContent(model);
            var response = await base.MakeRequestAuthorizedAsync("put", api, stringContent);

            return await HandleApiResponseUpdate(response);
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePartialMuonHocBa(UpdateRequestStatusMuonHocBaGocModel model)
        {
            string api = "api/dichVu/admin/muon-hoc-ba/update";
            var stringContent = base.MakeContent(model);
            var response = await base.MakeRequestAuthorizedAsync("put", api, stringContent);

            return await HandleApiResponseUpdate(response);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateMultiStatus(UpdateStatusModel model)
        {
            string api = "api/dichVu/admin/update-status/multi/four";
            var stringContent = base.MakeContent(model);
            var response = await base.MakeRequestAuthorizedAsync("put", api, stringContent);

            return await HandleApiResponseUpdate(response);
        }

        private async Task<ActionResult> HandleApiResponseUpdate(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return Json(new ResponseBody
                {
                    StatusCode = HttpStatusCode.OK,
                });
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                try
                {
                    var apiContent = await base.DeserializeResponseAsync<ResponseBody>(response.Content);
                    return Json(apiContent);
                }
                catch (Exception)
                {
                    return Json(new ResponseBody
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Lỗi hệ thống"
                    });
                }
            }
            else
            {
                return View(await base.HandleResponseAsync(response));
            }
        }

        private static Dictionary<string, string> ExportApiSet = new Dictionary<string, string>
        {
            { "word", "api/dichVu/admin/export-word" },
            { "word-list", "api/dichVu/admin/export-word-list" },
            { "excel", "api/dichVu/admin/export-excel" },
            { "excel-ov", "api/dichVu/admin/export-excel-overview" },
        };

        private static Dictionary<string, string> MimeTypeSet = new Dictionary<string, string>
        {
            { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { "zip", "application/zip" },
            { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        };

        [HttpPost]
        public async Task<ActionResult> ExportFile(ExportModel model)
        {
            if (!ExportApiSet.ContainsKey(model.ExportType))
            {
                return null;
            }
            string api = ExportApiSet[model.ExportType];
            var body = new { model.DichVuList, model.DichVuType };
            var stringContent = base.MakeContent(body);
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

            return await HandleResponseAsync(response, action200Async: async raw =>
            {
                var res = await base.DeserializeResponseAsync<DataTableResponse<QuanLyDichVuDetailModel>>(raw.Content);
                return Json(new
                {
                    draw = res.Draw,
                    recordsTotal = res.RecordsTotal,
                    recordsFiltered = res.RecordsFiltered,
                    data = res.Data
                });
            },
            actionDefault: raw => {
                return Json(new
                {
                    draw = ++request.Draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<object>()
                });
            });
        }

        [HttpPost]
        public async Task<ActionResult> SearchThamSoDichVu(DataTableRequest request)
        {
            string api = "api/dichVu/admin/search-parameters";
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
                var res = await base.DeserializeResponseAsync<DataTableResponse<ThamSoModel>>(response.Content);
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

        [HttpGet]
        public async Task<ActionResult> GetTypeDichVu()
        {
            string api = "api/dichVu/admin/get-types";
            var response = await base.MakeRequestAuthorizedAsync("get", api);

            return await base.HandleResponseAsync(response, action200Async: async raw =>
            {
                var res = await base.DeserializeResponseAsync<List<Dictionary<string, string>>>(raw.Content);
                return Json(res, JsonRequestBehavior.AllowGet);
            }, actionDefault: raw => {
                return Json("", JsonRequestBehavior.AllowGet);
            });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateParameters(DictionaryModel data)
        {
            string api = "api/dichVu/admin/update-parameters";

            var stringContent = base.MakeContent(data.dict);
            var response = await base.MakeRequestAuthorizedAsync("put", api, stringContent);

            return await base.HandleResponseAsync(response, actionDefault: raw => {
                return Json(raw);
            });
        }

        #region đợt đăng ký chỗ ở
        [HttpGet]
        public ActionResult DotDangKyChoO()
        {
            return View("DotDangKyChoO");
        }

        [HttpPost]
        public async Task<ActionResult> GetAllDotDangKyNhaO(DataTableRequest request)
        {
            string api = "api/DichVu/admin/dang-ky-cho-o/get-all";
            var stringContent = base.MakeContent(request);
            var response = await base.MakeRequestAuthorizedAsync("post", api, stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<DotDangKyChoOModel>>(jsonString);
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
        public async Task<ActionResult> AddDotDangKyNhaO(AddDotDangKyChoOModel model)
        {
            string api = "api/dichVu/admin/dang-ky-cho-o/add";
            var stringContent = base.MakeContent(model);
            var response = await base.MakeRequestAuthorizedAsync("post", api, stringContent);

            return await HandleApiResponseUpdate(response);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateDotDangKyNhaO(int id, AddDotDangKyChoOModel model)
        {
            string api = $"api/dichVu/admin/dang-ky-cho-o/update?id={id}";
            var stringContent = base.MakeContent(model);
            var response = await base.MakeRequestAuthorizedAsync("put", api, stringContent);
            return await HandleApiResponseUpdate(response);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDotDangKyNhaO(int id)
        {
            string api = $"api/dichVu/admin/dang-ky-cho-o/delete?id={id}";
            var response = await base.MakeRequestAuthorizedAsync("delete", api);
            return await HandleApiResponseUpdate(response);
        }
        #endregion

        #region đợt xin miễn giảm học phí
        [HttpGet]
        public ActionResult DotXinMienGiamHocPhi()
        {
            return View("DotXinMienGiamHocPhi");
        }

        [HttpPost]
        public async Task<ActionResult> GetAllDotXinMienGiamHocPhi(DataTableRequest request)
        {
            string api = "api/DichVu/admin/xin-mien-giam-hoc-phi/get-all";
            var stringContent = base.MakeContent(request);
            var response = await base.MakeRequestAuthorizedAsync("post", api, stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<DotXinMienGiamHocPhiModel>>(jsonString);
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
        public async Task<ActionResult> AddDotXinMienGiamHocPhi(AddDotXinMienGiamHocPhiModel model)
        {
            string api = "api/dichVu/admin/xin-mien-giam-hoc-phi/add";
            var stringContent = base.MakeContent(model);
            var response = await base.MakeRequestAuthorizedAsync("post", api, stringContent);

            return await HandleApiResponseUpdate(response);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateDotXinMienGiamHocPhi(int id, AddDotXinMienGiamHocPhiModel model)
        {
            string api = $"api/dichVu/admin/xin-mien-giam-hoc-phi/update?id={id}";
            var stringContent = base.MakeContent(model);
            var response = await base.MakeRequestAuthorizedAsync("put", api, stringContent);
            return await HandleApiResponseUpdate(response);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDotXinMienGiamHocPhi(int id)
        {
            string api = $"api/dichVu/admin/xin-mien-giam-hoc-phi/delete?id={id}";
            var response = await base.MakeRequestAuthorizedAsync("delete", api);
            return await HandleApiResponseUpdate(response);
        }
        #endregion

        #region đợt đề nghị hỗ trợ chi phí
        [HttpGet]
        public ActionResult DotDeNghiHoTroChiPhi()
        {
            return View("DotDeNghiHoTroChiPhi");
        }

        [HttpPost]
        public async Task<ActionResult> GetAllDotDeNghiHoTroChiPhi(DataTableRequest request)
        {
            string api = "api/DichVu/admin/de-nghi-ho-tro-chi-phi/get-all";
            var stringContent = base.MakeContent(request);
            var response = await base.MakeRequestAuthorizedAsync("post", api, stringContent);
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataTableResponse<DotDeNghiHoTroChiPhiModel>>(jsonString);
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
        public async Task<ActionResult> AddDotDeNghiHoTroChiPhi(AddDotDeNghiHoTroChiPhiModel model)
        {
            string api = "api/dichVu/admin/de-nghi-ho-tro-chi-phi/add";
            var stringContent = base.MakeContent(model);
            var response = await base.MakeRequestAuthorizedAsync("post", api, stringContent);

            return await HandleApiResponseUpdate(response);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateDotDeNghiHoTroChiPhi(int id, AddDotDeNghiHoTroChiPhiModel model)
        {
            string api = $"api/dichVu/admin/de-nghi-ho-tro-chi-phi/update?id={id}";
            var stringContent = base.MakeContent(model);
            var response = await base.MakeRequestAuthorizedAsync("put", api, stringContent);
            return await HandleApiResponseUpdate(response);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDotDeNghiHoTroChiPhi(int id)
        {
            string api = $"api/dichVu/admin/de-nghi-ho-tro-chi-phi/delete?id={id}";
            var response = await base.MakeRequestAuthorizedAsync("delete", api);
            return await HandleApiResponseUpdate(response);
        }
        #endregion
    }
}