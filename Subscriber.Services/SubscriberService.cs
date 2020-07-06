using AutoMapper;
using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IMapper _mapper;

        public SubscriberService(ISubscriberRepository subscriberRepository, IMapper mapper)
        {
            _subscriberRepository = subscriberRepository;
            _mapper = mapper;
        }

        public async Task<CardModel> GetCardAsync(int id)
        {
                return await _subscriberRepository.GetCardAsync(id);
                    }

        public async Task<int> LoginAsync(string email, string password)
        {
            
            return await _subscriberRepository.LoginAsync(email, password);
        }

        public async Task<bool> RegisterAsync(SubscriberModel subscriberModel)
        {
            //subscriberModel.Id = Guid.NewGuid();

            return await _subscriberRepository.RegisterAsync(subscriberModel);
        }
    }
}
