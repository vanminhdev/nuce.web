using log4net.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Hosted
{
    public class TableTaskHostedService : IHostedService
    {
        private readonly ILogger<TableTaskHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public TableTaskHostedService(ILogger<TableTaskHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting TableTaskHostedService in Startup");

            var scope = _scopeFactory.CreateScope();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();
            var tableTasks = statusContext.AsStatusTableTask.Where(o => o.Status == (int)TableTaskStatus.Doing).ToList();
            tableTasks.ForEach(t => t.Status = (int)TableTaskStatus.DoNot);
            statusContext.SaveChanges();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping TableTaskHostedService in Startup");

            var scope = _scopeFactory.CreateScope();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();
            var tableTasks = statusContext.AsStatusTableTask.Where(o => o.Status == (int)TableTaskStatus.Doing).ToList();
            tableTasks.ForEach(t => t.Status = (int)TableTaskStatus.DoNot);
            statusContext.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
