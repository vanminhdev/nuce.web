using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;

namespace nuce.web.api.Controllers.Survey
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ExamQuestions : ControllerBase
    {
        private readonly ILogger<ExamQuestions> _logger;
        private readonly IAsEduSurveyDeThiService _asEduSurveyDeThiService;

        public ExamQuestions(ILogger<ExamQuestions> logger, IAsEduSurveyDeThiService asEduSurveyDeThiService)
        {
            _logger = logger;
            _asEduSurveyDeThiService = asEduSurveyDeThiService;
        }
    }
}
