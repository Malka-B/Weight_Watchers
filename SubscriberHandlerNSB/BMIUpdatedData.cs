using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubscriberHandlerNSB
{
    public class BMIUpdatedData : ContainSagaData
    {
        public int MeasureId { get; set; }

        public bool IsBMIUpdated { get; set; }
    }
}
