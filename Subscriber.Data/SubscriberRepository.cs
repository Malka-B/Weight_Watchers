using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Subscriber.Data.Entities;
using Subscriber.Services;
using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Data
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly WeightWatchersContext _weightWatchersContext;
        private readonly IMapper _mapper;

        public SubscriberRepository(WeightWatchersContext weightWatchersContext, IMapper mapper)
        {
            _weightWatchersContext = weightWatchersContext;
            _mapper = mapper;
        }
        public async Task<CardModel> GetCardAsync(int id)
        {
            CardEntity cardEntity = await _weightWatchersContext.Card
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cardEntity != null)
            {
                SubscriberEntity subscriberEntity = await _weightWatchersContext.Subscriber
                                                .Include(s => s.CardEntity)
                                                .FirstOrDefaultAsync(s => s.Id == cardEntity.SubscriberId);

                CardModel cardModel = _mapper.Map<CardModel>(subscriberEntity);
                return cardModel;
            }
            throw new Exception();

        }



        public async Task<int> LoginAsync(string email, string password)
        {
            SubscriberEntity subscriberEntity = await _weightWatchersContext.Subscriber
                                                .Include(s => s.CardEntity)
                                                .FirstOrDefaultAsync(s => s.Email == email
                                                && s.Password == password);
            if (subscriberEntity != null)
            {
                return subscriberEntity.CardEntity.Id;
            }
            throw new Exception();
        }

        public async Task<bool> RegisterAsync(SubscriberModel subscriberModel)
        {
            SubscriberEntity subscriberEntity = _mapper.Map<SubscriberEntity>(subscriberModel);

            var s = await _weightWatchersContext.Subscriber
                                                .FirstOrDefaultAsync(s =>
                                                s.Email == subscriberEntity.Email);
            if (s == null)
            {
                subscriberEntity.Id = Guid.NewGuid();
                CardEntity cardEntity = new CardEntity
                {
                    OpenDate = DateTime.Now,
                    BMI = 0,
                    Height = subscriberModel.Height,
                    UpdateDate = DateTime.Now
                };
                subscriberEntity.CardEntity = cardEntity;

                await _weightWatchersContext.Subscriber.AddAsync(subscriberEntity);
                await _weightWatchersContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
