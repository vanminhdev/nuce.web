using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using nuce.web.api.Models.Core;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core;
using nuce.web.shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace nuce.web.api.Controllers.Core
{
    [AppAuthorize(RoleNames.Admin, RoleNames.KhaoThi)]
    [Route("api/[controller]")]
    public class NewsManagerController : Controller
    {
        private readonly INewsManagerService _newsManagerService;
        private readonly IUserService _userService;
        private readonly ILogger<NewsManagerController> _logger;
        public NewsManagerController(INewsManagerService _newsManagerService, IUserService _userService, ILogger<NewsManagerController> _logger)
        {
            this._newsManagerService = _newsManagerService;
            this._userService = _userService;
            this._logger = _logger;
        }
        #region client
        /// <summary>
        /// List danh mục theo site
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("news-category/{role}")]
        public IActionResult GetNewsCategory(string role)
        {
            return Ok(_newsManagerService.GetAllCategoryByRole(role, 1));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("news-category/menu/{role}")]
        public IActionResult GetNewsCategoryOnMenu(string role)
        {
            return Ok(_newsManagerService.GetAllCategoryByRole(role, 1, true));
        }

        /// <summary>
        /// Avatar bài tin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("news-items/avatar/{id}")]
        public async Task<IActionResult> GetAvatar(int id, [FromQuery] int? width, [FromQuery] int? height)
        {
            try
            {
                var result = await _newsManagerService.GetNewsItemAvatar(id, width, height);
                if (result != null)
                {
                    return File(result.Data, $"image/{result.Extension}");
                }
                return null;
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }

        /// <summary>
        /// Chi tiết bài tin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("news-items/{id}")]
        public async Task<IActionResult> GetNewsItemById(int id)
        {
            return Ok(await _newsManagerService.FindNewsItemById(id, (int)NewsItemStatus.Approved));
        }

        /// <summary>
        /// List bài tin theo danh mục
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("news-items/category/{catId}")]
        public async Task<IActionResult> GetNewsItemByCategory(int catId, [FromBody] DataTableRequest request)
        {
            try
            {
                var data = await _newsManagerService.FindItemsByCatId(catId, request.Start, request.Length, 1, (int)NewsItemStatus.Approved);
                data.Draw = request.Draw++;
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }

        /// <summary>
        /// Danh sách bài tin xung quanh 1 bài tin (Theo dòng sự kiện)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("news-items/{id}/surround")]
        public async Task<IActionResult> GetNewsItemByCategory(int id)
        {
            return Ok(await _newsManagerService.GetCousinNewsItemsById(id, NewsItemStatus.Approved));
        }
        #endregion

        #region admin
        /// <summary>
        /// List danh mục theo quyền
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize(RoleNames.KhaoThi_Add_Cat, RoleNames.KhaoThi_Edit_Cat, RoleNames.KhaoThi_Upload_WebImage, RoleNames.KhaoThi_Edit_Contact,
            RoleNames.KhaoThi_Add_NewsItem, RoleNames.KhaoThi_Approve_NewsItem, RoleNames.KhaoThi_Edit_NewsItem)]
        [Route("admin/news-category")]
        public IActionResult GetNewsCategoryAdmin()
        {
            var loggedUserRoles = _userService.GetClaimListByKey(ClaimTypes.Role);
            List<NewsCats> result = new List<NewsCats>();
            foreach (var role in loggedUserRoles)
            {
                var tmp = _newsManagerService.GetAllCategoryByRoleAdmin(role);
                result.AddRange(tmp);
            }
            return Ok(result);
        }
        /// <summary>
        /// Tạo danh mục mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AppAuthorize(RoleNames.KhaoThi_Add_Cat)]
        [HttpPost]
        [Route("admin/news-category/create")]
        public async Task<IActionResult> CreateNewsCategoryAdmin([FromBody] CreateNewsCategoryModel model)
        {
            var loggedUserRoles = _userService.GetClaimListByKey(ClaimTypes.Role);
            try
            {
                var rslt = await _newsManagerService.CreateNewsCatsAdmin(loggedUserRoles, model);
                return Ok(new ResponseBody
                {
                    Data = rslt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }
        /// <summary>
        /// Cập nhật danh mục
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AppAuthorize(RoleNames.KhaoThi_Edit_Cat)]
        [HttpPut]
        [Route("admin/news-category/update")]
        public async Task<IActionResult> UpdateNewsCategoryAdmin([FromBody] NewsCats model)
        {
            var loggedUserRoles = _userService.GetClaimListByKey(ClaimTypes.Role);
            try
            {
                await _newsManagerService.UpdateNewsCatsAdmin(loggedUserRoles, model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }

        /// <summary>
        /// Đăng bài
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AppAuthorize(RoleNames.KhaoThi_Add_NewsItem)]
        [HttpPost]
        [Route("admin/news-items/create")]
        public async Task<IActionResult> CreateNewsItem([FromBody]CreateNewsItemModel model)
        {
            try
            {
                var id = await _newsManagerService.CreateNewsItems(model);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật ảnh bài tin
        /// </summary>
        /// <param name="file"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [AppAuthorize(RoleNames.KhaoThi_Add_NewsItem, RoleNames.KhaoThi_Edit_NewsItem)]
        [Route("admin/news-items/avatar/{id}")]
        public async Task<IActionResult> UploadAvatar(IFormFile file, int id)
        {
            try
            {
                var result = await _newsManagerService.UploadNewsItemAvatar(file, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }

        /// <summary>
        /// Chỉnh sửa nội dung bài tin
        /// </summary>
        /// <param name="newsItem"></param>
        /// <returns></returns>
        [AppAuthorize(RoleNames.KhaoThi_Edit_NewsItem)]
        [HttpPut]
        [Route("admin/news-items/update")]
        public async Task<IActionResult> UpdateNewsItem([FromBody]NewsItems newsItem)
        {
            try
            {
                await _newsManagerService.UpdateNewsItems(newsItem);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }
        /// <summary>
        /// Xoá bài tin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AppAuthorize(RoleNames.KhaoThi_Edit_NewsItem)]
        [HttpDelete]
        [Route("admin/news-items/delete/{id}")]
        public IActionResult LockNewsItem(int id)
        {
            return Ok();
        }

        /// <summary>
        /// Duyệt bài
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AppAuthorize(RoleNames.KhaoThi_Approve_NewsItem)]
        [HttpPut]
        [Route("admin/news-items/{id}/status/{status}")]
        public async Task<IActionResult> ApproveNewsItemAsync(int id, int status)
        {
            try
            {
                await _newsManagerService.UpdateNewsItemStatus(id, (NewsItemStatus)status);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }

        [AppAuthorize(RoleNames.KhaoThi_Edit_NewsItem)]
        [HttpDelete]
        [Route("admin/news-items/{id}")]
        public async Task<IActionResult> DeleteNewsItemAsync(int id)
        {
            try
            {
                await _newsManagerService.DeleteNewsItem(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }
        #endregion

        #region common
        /// <summary>
        /// Chi tiết bài tin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("admin/news-items/{id}")]
        public async Task<IActionResult> GetNewsItemByIdAdmin(int id)
        {
            return Ok(await _newsManagerService.FindNewsItemById(id, (int)NewsItemStatus.IgnoreStatus));
        }

        /// <summary>
        /// List bài tin theo danh mục
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("admin/news-items/category/{catId}")]
        public async Task<IActionResult> GetNewsItemByCategoryAdmin(int catId, [FromBody] DataTableRequest request)
        {
            try
            {
                var data = await _newsManagerService.FindItemsByCatId(catId, request.Start, request.Length, 1, (int)NewsItemStatus.IgnoreStatus);
                data.Draw = request.Draw++;
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }
        #endregion
    }
}
