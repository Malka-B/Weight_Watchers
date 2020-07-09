using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Measure.Services.Interfaces;
using Measure.Services.Models;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using AutoMapper;
using Measure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Messages.Events;

namespace Measure.Data
{
    public class MeasureRepository : IMeasureRepository
    {
        private readonly MeasureContext _measureContext;
        private readonly IMapper _mapper;

        public MeasureRepository(MeasureContext measureContext, IMapper mapper)
        {
            _measureContext = measureContext;
            _mapper = mapper;
        }

        public async Task<int> AddMeasureAsync(MeasureModel measureModel)
        {
            MeasureEntity measureEntity = _mapper.Map<MeasureEntity>(measureModel);
            await _measureContext.Measures.AddAsync(measureEntity);
            await _measureContext.SaveChangesAsync();
            MeasureEntity measure = await _measureContext.Measures
                .FirstOrDefaultAsync(m => m.CardId == measureEntity.CardId
                                       && m.Date == measureEntity.Date
                                       && m.Status == measureEntity.Status
                                       && m.Weight == measureEntity.Weight);
            return measure.Id;
        }

        public async Task UpdateStatusAsync(SubscriberUpdated message)
        {
            MeasureEntity measure = await _measureContext.Measures
                .FirstOrDefaultAsync(m => m.Id == message.MeasureId);

            measure.Status = message.Status;
            measure.Comments = message.Comment;

            await _measureContext.SaveChangesAsync();
        }
    }
}
