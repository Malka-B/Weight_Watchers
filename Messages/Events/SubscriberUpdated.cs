using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
    public class SubscriberUpdated
    {
        public string Status { get; set; }

        public string Comment { get; set; }

        public int MeasureId { get; set; }
    }
}
