using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nuce.web.api.Services.Ctsv.Interfaces;

namespace nuce.web.api.Controllers.Ctsv
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService _newsService)
        {
            this._newsService = _newsService;
        }

        [Route("get-news-items")]
        [HttpGet]
        public IActionResult GetNewsItems()
        {
            return Ok(_newsService.GetNewsItems());
        }
    }
}