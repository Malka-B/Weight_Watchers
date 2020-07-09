using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messages.Events;

using NServiceBus;
using NServiceBus.Logging;

namespace SubscriberHandlerNSB
{
    public class BMIUpdatedSaga : Saga<BMIUpdatedData>,
        IAmStartedByMessages<BMIUpdated>
    {
        static ILog log = LogManager.GetLogger<BMIUpdatedSaga>();

        public async Task Handle(BMIUpdated message, IMessageHandlerContext context)
        {
            SubscriberUpdated subscriber;
            log.Info($"In BMIUpdatedSaga: The measure {message.MeasureId} completed!");
            subscriber = new SubscriberUpdated { Status = message.Status, MeasureId = message.MeasureId };
            if (message.Status == "failed")
            {
                subscriber.Comment = "Card Id is wrong";
            }
            
            await context.Publish(subscriber);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BMIUpdatedData> mapper)
        {
            mapper.ConfigureMapping<BMIUpdated>(message => message.MeasureId)
                .ToSaga(sagaData => sagaData.MeasureId);


        }
    }
}
