using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyReportTotalService : IAsEduSurveyReportTotalService
    {
        ILogger<AsEduSurveyReportTotalService> _logger;
        private readonly SurveyContext _context;
        private readonly IStatusService _statusService;

        public AsEduSurveyReportTotalService(ILogger<AsEduSurveyReportTotalService> logger, SurveyContext context, IStatusService statusService)
        {
            _logger = logger;
            _context = context;
            _statusService = statusService;
        }
    }
}
