using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Measure.Services.Interfaces;
using Measure.Services.Models;
using Messages.Events;
using NServiceBus;

namespace Measure.Services
{
    public class MeasureService : IMeasureService
    {
        private readonly IMeasureRepository _measureRepository;
       

        public MeasureService(IMeasureRepository measureRepository)
        {
            _measureRepository = measureRepository;           
        }

        public async Task<int> AddMeasureAsync(MeasureModel measureModel)
        {
            measureModel.Date = DateTime.Now;
            measureModel.Status = "InProcess";           
            
            return await _measureRepository.AddMeasureAsync(measureModel);
        }

        public async Task UpdateStatusAsync(SubscriberUpdated message)
        {
            await _measureRepository.UpdateStatusAsync(message);
        }
    }
}
