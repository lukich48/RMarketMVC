using AutoMapper;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Infrastructure
{
    public class AutoMapperConfiguration
    {
        public MapperConfiguration CreateDefaultConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Instance, InstanceModel>()
                .ForMember(m => m.StrategyParams, opt => opt.MapFrom(d =>
                       StrategyHelper.GetStrategyParams(d)
                 ));


            });
        }
    }
}
