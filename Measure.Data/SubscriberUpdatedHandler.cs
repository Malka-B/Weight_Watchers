using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Measure.Services.Interfaces;
using Messages.Events;
using NServiceBus;

namespace Measure.Data
{
    class SubscriberUpdatedHandler : IHandleMessages<SubscriberUpdated>
    {
        IMeasureService _measureService;

        public SubscriberUpdatedHandler(IMeasureService measureService)
        {
            _measureService = measureService;
        }
        public async Task Handle(SubscriberUpdated message, IMessageHandlerContext context)
        {
            await _measureService.UpdateStatusAsync(message);
        }
    }
}
