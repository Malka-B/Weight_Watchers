using Messages.Commands;
using Messages.Events;
using NServiceBus;
using Subscriber.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberHandler
{
    public class UpdateMeasureHandler : IHandleMessages<UpdateMeasure>
    {
        //private readonly ISubscriberService _subscriberService;

        //public UpdateMeasureHandler(ISubscriberService subscriberService)
        //{
        //    _subscriberService = subscriberService;
        //}
        public async Task Handle(UpdateMeasure message, IMessageHandlerContext context)
        {
            BMIUpdated updated;
            bool isExist = true;//await _subscriberService.CardExistAsync(message.CardId);
            if (isExist)
            {
                //await _subscriberService.UpdateBMIAsync(message);
                updated = new BMIUpdated { Status = "succeeded", MeasureId = message.MeasureId };
                await context.Publish(updated);
            }
            else
            {
                updated = new BMIUpdated { Status = "failed", MeasureId = message.MeasureId };
                await context.Publish(updated);
            }
        }
    }
}
