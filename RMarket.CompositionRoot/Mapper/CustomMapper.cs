using AutoMapper;
using AutoMapper.Configuration;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.CompositionRoot.Mapper
{
    public class MapperAutoMapper: RMarket.ClassLib.Infrastructure.AmbientContext.MyMapper
    {
        private readonly IMapper mapper;

        public MapperAutoMapper(IEnumerable<AutoMapper.Profile> profiles)
        {
            if (profiles == null || profiles.Count() == 0)
                throw new CustomException("Ошибка инициализации маппера! Не определены профили");

            MapperConfigurationExpression cfg = new MapperConfigurationExpression();

            foreach (AutoMapper.Profile profile in profiles)
            {
                cfg.AddProfile(profile);
            }

            mapper = new MapperConfiguration(cfg).CreateMapper();
        }

        public override TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TSource, TDestination>(source);
        }

        public override TDestination Map<TDestination>(object source)
        {
            return mapper.Map<TDestination>(source);
        }

    }
}
