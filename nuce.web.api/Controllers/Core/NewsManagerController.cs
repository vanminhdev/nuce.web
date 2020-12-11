using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nuce.web.api.Models.Core;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace nuce.web.api.Controllers.Core
{
    [Authorize(Roles = "P_CTSV,P_KhaoThi")]
    [Route("api/[controller]")]
    public class NewsManagerController : Controller
    {
        private readonly INewsManagerService _newsManagerService;
        public NewsManagerController(INewsManagerService _newsManagerService)
        {
            this._newsManagerService = _newsManagerService;
        }
        [HttpGet]
        [Route("news-category/{role}")]
        public IActionResult GetNewsCategory(string role)
        {
            return Ok(_newsManagerService.GetAllCategory(role));
        }

        [HttpPost]
        [Route("news-items/category/{catId}")]
        public async Task<IActionResult> GetNewsItemByCategory(int catId, [FromBody] DataTableRequest request)
        {
            try
            {
                var data = await _newsManagerService.FindItemsByCatId(catId, request.Start, request.Length);
                data.Draw = request.Draw++;
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("news-items/{id}")]
        public async Task<IActionResult> GetNewsItemById(int id)
        {
            return Ok(await _newsManagerService.FindNewsItemById(id));
        }

        [HttpPost]
        [Route("news-items/create")]
        public async Task<IActionResult> CreateNewsItem([FromBody]CreateNewsItemModel model)
        {
            try
            {
                await _newsManagerService.CreateNewsItems(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = ex.Message });
            }
        }

        [HttpPut]
        [Route("news-items/update")]
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

        [HttpPut]
        [Route("news-items/lock")]
        public IActionResult LockNewsItem()
        {
            return Ok();
        }
    }
}
