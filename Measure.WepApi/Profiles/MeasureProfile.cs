using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Measure.WepApi.DTO;
using Measure.Services.Models;

using Measure.Data.Entities;

namespace Measure.WepApi.Profiles
{
    public class MeasureProfile : Profile
    {
        public MeasureProfile()
        {
            CreateMap<MeasureModel, MeasureDTO>();
            CreateMap<MeasureDTO, MeasureModel>();
            CreateMap<MeasureModel, MeasureEntity>();
            CreateMap<MeasureEntity, MeasureModel>();
        }
    }
}
