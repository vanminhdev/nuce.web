using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using CKSource.CKFinder.Connector.Core;
using CKSource.CKFinder.Connector.Core.Events;
using CKSource.CKFinder.Connector.Core.Events.Messages;
using CKSource.CKFinder.Connector.Core.Logs;
using CKSource.CKFinder.Connector.Core.Plugins;
using nuce.web.quanly.Controllers;

namespace nuce.web.quanly.CKFinder
{
    [Export(typeof(IPlugin))]
    public class CustomizeCommand : BaseController, IPlugin, IDisposable
    {
        private readonly List<object> _subscriptions = new List<object>();

        private IEventAggregator _eventAggregator;

        public void Initialize(IComponentResolver componentResolver, IReadOnlyDictionary<string, IReadOnlyCollection<string>> options)
        {
            _eventAggregator = componentResolver.Resolve<IEventAggregator>();

            _subscriptions.AddRange(new[]
            {
                //_eventAggregator.Subscribe<BeforeCommandEvent>(next => async messageContext => await CustomizeBeforeCommand(messageContext, next)),
                _eventAggregator.Subscribe<FileUploadEvent>(next => async messageContext => await OnFileUpload(messageContext, next))
            });
        }

        public void Dispose()
        {
            if (_eventAggregator == null)
            {
                return;
            }

            foreach (var subscription in _subscriptions)
            {
                _eventAggregator.Unsubscribe(subscription);
            }

            _subscriptions.Clear();
        }

        //private static async Task CustomizeBeforeCommand(MessageContext<BeforeCommandEvent> messageContext, EventHandlerFunc<BeforeCommandEvent> next)
        //{
        //}

        private static async Task OnFileUpload(MessageContext<FileUploadEvent> messageContext, EventHandlerFunc<FileUploadEvent> next)
        {
            var msg = messageContext.Message;
            await next(messageContext);
        }
    }
}