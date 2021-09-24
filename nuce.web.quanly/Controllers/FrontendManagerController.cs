using nuce.web.quanly.Attributes.ActionFilter;
using nuce.web.quanly.Models;
using nuce.web.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    [AuthorizeActionFilter(RoleNames.KhaoThi_Edit_NewsItem, RoleNames.KhaoThi_Add_NewsItem, RoleNames.KhaoThi_Approve_NewsItem,
            RoleNames.KhaoThi_Add_Cat, RoleNames.KhaoThi_Edit_Cat, RoleNames.KhaoThi_Upload_WebImage, RoleNames.KhaoThi_Edit_Contact)]
    public class FrontendManagerController : BaseController
    {
        #region view
        /// <summary>
        /// Hiển thị tree quản trị
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string api = $"api/NewsManager/admin/news-category";
            var response = await base.MakeRequestAuthorizedAsync("get", api);

            return await base.HandleResponseAsync(response, action200Async: async res =>
            {
                var model = await base.DeserializeResponseAsync<List<NewsCatsModel>>(res.Content);
                return View("Index", model);
            });
        }
        /// <summary>
        /// Trang sửa những tham số là text
        /// </summary>
        /// <param name="type"></param>
        /// <param name="richText"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Survey_Graduate, RoleNames.KhaoThi_Survey_Undergraduate, RoleNames.KhaoThi_Survey_Normal, RoleNames.KhaoThi_Edit_Contact)]
        public async Task<ActionResult> Edit(string type, string code = "", bool richText = false)
        {
            ViewData["richText"] = richText;
            ViewData["code"] = code;
            string api = $"api/ClientParameters/admin/{type}";
            var response = await base.MakeRequestAuthorizedAsync("get", api);

            return await base.HandleResponseAsync(response, action200Async: async res =>
            {
                var model = await base.DeserializeResponseAsync<List<ClientParameterModel>>(res.Content);
                return View("Edit", model);
            });
        }
        /// <summary>
        /// Trang sửa những tham số là hình ảnh
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Upload_WebImage)]
        public ActionResult Image(string type)
        {
            TempData["type"] = type;
            return View("Image");
        }
        #endregion
        /// <summary>
        /// Hàm call api update tham số là text
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Edit_Contact, RoleNames.KhaoThi_Survey_Graduate, RoleNames.KhaoThi_Survey_Undergraduate, RoleNames.KhaoThi_Survey_Normal)]
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
        /// <summary>
        /// Hàm cal api tạo danh mục
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Add_Cat)]
        public async Task<ActionResult> CreateCategory(CreateCategoryModel model)
        {
            string api = $"api/NewsManager/admin/news-category/create";

            var content = base.MakeContent(model);

            var response = await base.MakeRequestAuthorizedAsync("post", api, content);

            return Json(new ResponseBody
            {
                Data = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Hàm cập nhật danh mục
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Edit_Cat)]
        public async Task<ActionResult> UpdateCategory(NewsCatsModel model)
        {
            string api = $"/api/NewsManager/admin/news-category/update";

            var content = base.MakeContent(model);

            var response = await base.MakeRequestAuthorizedAsync("put", api, content);

            return Json(new ResponseBody
            {
                Data = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Hàm cập nhật ảnh cho website
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeActionFilter(RoleNames.KhaoThi_Upload_WebImage)]
        public async Task<ActionResult> UploadImage(string imgCode)
        {
            if (Request.Files.Count == 0)
            {
                return null;
            }
            HttpPostedFileBase image = Request.Files[0];
            byte[] content = new byte[image.ContentLength];
            await image.InputStream.ReadAsync(content, 0, content.Length);

            var multipartFormData = new MultipartFormDataContent();
            multipartFormData.Add(new ByteArrayContent(content), "file", image.FileName);

            string api = $"api/UserFile/upload/image/{imgCode}";

            var response = await base.MakeRequestAuthorizedAsync("post", api, multipartFormData);

            return Json(new ResponseBody
            {
                StatusCode = response.StatusCode,
                Data = await response.Content.ReadAsStringAsync(),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}