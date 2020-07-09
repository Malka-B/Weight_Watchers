using Measure.Services.Models;
using Messages.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Measure.Services.Interfaces
{
    public interface IMeasureService
    {
        Task<int> AddMeasureAsync(MeasureModel measureModel);

        Task UpdateStatusAsync(SubscriberUpdated message);
    }
}
