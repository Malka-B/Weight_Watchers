using System;
using System.Collections.Generic;
using System.Text;
using NServiceBus;

namespace Messages.Events
{
    public class BMIUpdated
    {
        public string Status { get; set; }

        public int MeasureId { get; set; }
    }
}
