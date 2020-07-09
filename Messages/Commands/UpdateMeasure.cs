using System;
using System.Collections.Generic;
using System.Text;
using NServiceBus;

namespace Messages.Commands
{
    public class UpdateMeasure
    {
        public int MeasureId { get; set; }

        public int CardId { get; set; }

        public decimal Weight { get; set; }
    }
}
