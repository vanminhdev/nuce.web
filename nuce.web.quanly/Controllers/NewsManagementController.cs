﻿using nuce.web.quanly.Models;
using nuce.web.quanly.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    public class NewsManagementController : BaseController
    {
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
            string api = $"api/NewsManager/news-category/{roleName}";
            var response = await base.MakeRequestAuthorizedAsync("get", api);

            return await base.HandleResponseAsync(response, action200Async: async res =>
            {
                var model = await base.DeserializeResponseAsync<List<NewsCatsModel>>(res.Content);
                return View("Index", model);
            });
        }

        [HttpGet]
        public ActionResult ItemsList(int catId)
        {
            TempData["catId"] = catId;
            return View("ItemsList");
        }

        [HttpGet]
        public ActionResult Create(int catId)
        {
            ViewData["catId"] = catId;
            return View("Create");
        }

        [HttpGet]
        public async Task<ActionResult> ItemsDetail(int id)
        {
            return await GetNewsItem(id, "ItemsDetail");
        }

        [HttpGet]
        public async Task<ActionResult> ItemsUpdate(int id)
        {
            return await GetNewsItem(id, "ItemsUpdate");
        }

        [HttpPost]
        public async Task<ActionResult> GetItemsListByCatId(GetNewsItemByCatIdModel request)
        {
            string api = $"api/NewsManager/news-items/category/{request.catId}";
            var stringContent = base.MakeContent(request.body);
            var response = await base.MakeRequestAuthorizedAsync("post", api, stringContent);

            return await HandleResponseAsync(response, action200Async: async raw =>
            {
                var res = await base.DeserializeResponseAsync<DataTableResponse<NewsItemModel>>(raw.Content);
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
                    draw = ++request.body.Draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<object>()
                });
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewsItem(CreateNewsItemModel request)
        {
            string api = "api/NewsManager/news-items/create";

            var stringContent = base.MakeContent(request);
            var response = await base.MakeRequestAuthorizedAsync("post", api, stringContent);

            return await HandleApiResponseUpdate(response);
        }

        [HttpPost]
        public async Task<ActionResult> UploadAvatar(int id)
        {
            if (Request.Files.Count == 0)
            {
                return null;
            }
            HttpPostedFileBase imgAvatar = Request.Files[0];
            byte[] content = new byte[imgAvatar.ContentLength];
            await imgAvatar.InputStream.ReadAsync(content, 0, content.Length);
            var extension = Path.GetExtension(imgAvatar.FileName).Substring(1).ToLower();
            UploadFileModel model = new UploadFileModel
            {
                Content = content,
                FileName = imgAvatar.FileName,
                Key = "file",
                ContentType = $"image/{extension}",
            };

            string api = $"api/NewsManager/news-items/avatar/{id}";

            var stringContent = base.MakeContent(model);
            var response = await base.MakeRequestAuthorizedAsync("put", api, stringContent);

            return await HandleApiResponseUpdate(response);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateNewsItem(NewsItemModel request)
        {
            string api = "api/NewsManager/news-items/update";

            var stringContent = base.MakeContent(request);
            var response = await base.MakeRequestAuthorizedAsync("put", api, stringContent);

            return await HandleApiResponseUpdate(response);
        }

        private async Task<ActionResult> GetNewsItem(int id, string viewName)
        {
            string api = $"api/NewsManager/news-items/{id}";
            var response = await base.MakeRequestAuthorizedAsync("get", api);

            return await base.HandleResponseAsync(response, action200Async: async res =>
            {
                var model = await base.DeserializeResponseAsync<NewsItemModel>(res.Content);
                return View(viewName, model);
            });
        }

        private async Task<ActionResult> HandleApiResponseUpdate(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return Json(new ResponseBody
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = response
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
    }
}