using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using nuce.web.api.Services.Survey.Interfaces;

namespace nuce.web.api.Controllers.Survey
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly IAsEduSurveyCauHoiService _asEduSurveyCauHoiService;

        public QuestionController(ILogger<QuestionController> logger, IAsEduSurveyCauHoiService asEduSurveyCauHoiService)
        {
            _logger = logger;
            _asEduSurveyCauHoiService = asEduSurveyCauHoiService;
        }

        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            var questions = _asEduSurveyCauHoiService.GetAll();

            return Ok();
        }
    }
}
