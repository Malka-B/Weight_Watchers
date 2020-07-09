using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using Subscriber.Services;

namespace SubscriberHandlerNSB
{
    public class UpdateMeasureHandler : IHandleMessages<UpdateMeasure>
    {
        //private readonly ISubscriberService _subscriberService;
        static ILog log = LogManager.GetLogger<BMIUpdatedSaga>();

        //public UpdateMeasureHandler(ISubscriberService subscriberService)
        //{
        //    _subscriberService = subscriberService;
        //}
        public async Task Handle(UpdateMeasure message, IMessageHandlerContext context)
        {

            log.Info($"In UpdateMeasureHandler: The measure {message.MeasureId} completed!");
            BMIUpdated updated ;
            bool isExist = true;// await _subscriberService.CardExistAsync(message.CardId);
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
