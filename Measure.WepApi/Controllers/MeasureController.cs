using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Measure.WepApi.DTO;
using Measure.Services.Interfaces;
using Measure.Services.Models;
using AutoMapper;
using NServiceBus;
using Messages.Events;
using Messages.Commands;

namespace Measure.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasureController : ControllerBase
    {
        private readonly IMeasureService _measureService;
        private readonly IMapper _mapper;
        private readonly IMessageSession _messageSession;

        public MeasureController(IMeasureService measureService, IMapper mapper, IMessageSession messageSession)
        {//026368732
            _measureService = measureService;
            _mapper = mapper;
            _messageSession = messageSession;
        }

        [HttpPost("measure")]
        public async Task<ActionResult> AddMeasureAsync([FromBody] MeasureDTO measureDTO)
        {
            MeasureModel measureModel = _mapper.Map<MeasureModel>(measureDTO);
            int measureId = await _measureService.AddMeasureAsync(measureModel);
            UpdateMeasure updateMeasure = new UpdateMeasure
            {
                MeasureId = measureId,
                CardId = measureModel.CardId,
                Weight = measureModel.Weight
            };
           await _messageSession.Send(updateMeasure)
                .ConfigureAwait(false);

            return Ok("הפעולה נקלטה בהצלחה!");            

        }
    }
}
