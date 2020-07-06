using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Subscriber.Data.Entities;
using Subscriber.Services.Models;
using Weight_Watchers.DTO;

namespace Subscriber.Data.Profiles
{
    public class SubscriberProfile : Profile
    {
        public SubscriberProfile()
        {
            CreateMap<SubscriberModel, SubscriberDTO>();
            CreateMap<SubscriberDTO, SubscriberModel>();
            CreateMap<SubscriberEntity, SubscriberModel>();
            CreateMap<SubscriberModel, SubscriberEntity>();
            CreateMap<CardModel, CardDTO>();
            CreateMap<CardDTO, CardModel>();
            CreateMap<CardEntity, CardModel>();
            CreateMap<CardModel, SubscriberEntity>();
            CreateMap<SubscriberEntity, CardModel>()
                .ForMember(destination => destination.Id, option => option.MapFrom(src =>
                src.CardEntity.Id))
                .ForMember(destination => destination.BMI, option => option.MapFrom(src =>
                src.CardEntity.BMI))
                .ForMember(destination => destination.Height, option => option.MapFrom(src =>
                src.CardEntity.Height))
                .ForMember(destination => destination.Weight, option => option.MapFrom(src =>
                src.CardEntity.Weight));
        }
    }
}
