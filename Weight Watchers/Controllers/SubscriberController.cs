using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Subscriber.Services;
using Subscriber.Services.Models;
using Weight_Watchers.DTO;
using System.Net;


namespace Weight_Watchers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriberController : ControllerBase
    {

        private readonly ISubscriberService _subscriberService;
        private readonly IMapper _mapper;

        public SubscriberController(ISubscriberService subscriberService, IMapper mapper)
        {
            _mapper = mapper;
            _subscriberService = subscriberService;
        }

        [HttpPost("subscriber")]
        public async Task<ActionResult<bool>> RegisterAsync([FromBody] SubscriberDTO subscriber)
        {

            SubscriberModel subscriberModel = _mapper.Map<SubscriberModel>(subscriber);
            var response = await _subscriberService.RegisterAsync(subscriberModel);
            if (response)
            {
                return Ok(response);
            }
            return BadRequest(false);

        }

        [HttpPost("login")]
        public async Task<ActionResult<int>> LoginAsync([FromBody] LoginDTO login)
        {
            try
            {
                int cardId = await _subscriberService.LoginAsync(login.Email, login.Password);
                return Ok(cardId);
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpGet("card")]
        public async Task<ActionResult<CardDTO>> GetCardAsync([FromQuery] int id)
        {
            try
            {
                CardModel cardModel = await _subscriberService.GetCardAsync(id);
                CardDTO cardDTO = _mapper.Map<CardDTO>(cardModel);
                return Ok(cardDTO);
            }

            catch
            {
                return BadRequest();
            }

        }

    }
}
