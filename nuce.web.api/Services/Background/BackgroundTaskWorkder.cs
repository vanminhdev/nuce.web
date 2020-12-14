using log4net.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Background
{
    public class BackgroundTaskWorkder
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<BackgroundTaskWorkder> _logger;
        private readonly CancellationToken _cancellationToken;

        public BackgroundTaskWorkder(IBackgroundTaskQueue taskQueue,
            ILogger<BackgroundTaskWorkder> logger,
            IHostApplicationLifetime applicationLifetime)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
        }

        public void StartAction(Action action)
        {
            Task.Run(() => { RunAction(action); });
        }

        private void RunAction(Action action)
        {
            var guid = Guid.NewGuid().ToString();
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                action();
            });
        }
    }
}
