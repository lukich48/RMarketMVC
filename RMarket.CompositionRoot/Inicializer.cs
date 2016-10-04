using AutoMapper;
using AutoMapper.Configuration;
using RMarker.Concrete.DataProviders.Infrastructure;
using RMarker.Concrete.Optimization.Infrastructure;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.CompositionRoot.Mapper;
using RMarket.Concrete.HistoricalProviders.Infrastructure;
using RMarket.Concrete.Strategies.Infrastructure;
using RMarket.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.CompositionRoot
{
    public class Inicializer
    {
        /// <summary>
        /// Начальное заполнение БД (если не создана)
        /// </summary>
        public void InitializeDbContext()
        {
            //Заполнение БД
            RMarketInitializer.DataProviderSettingInitializer = new DataProvidersContextInicializer(); ;
            RMarketInitializer.HistoricalProviderSettingInitializer = new HistoricalProvidersContextInitializer(); ;
            RMarketInitializer.OptimizationSettingInitializer = new OptimizationsContextInicializer();
            RMarketInitializer.EntityInfoInitializer = new StrategyInfoesContextInitializer(); 

        }

        /// <summary>
        /// Инициализация автомаппера
        /// </summary>
        /// <param name="profiles"></param>
        public void SetMapperConfiguration(ICollection<Profile> profiles = null)
        {

            if (profiles == null)
                profiles = new List<Profile>();

            //Собираем все профили для маппера
            profiles.Add(new ClassLib.MapperProfiles.AutoMapperDomainProfile());

            var customMapper = new CustomMapper(profiles);
            RMarket.ClassLib.Infrastructure.AmbientContext.MyMapper.Current = customMapper;
        }

        public void SetIoC(Ninject.IKernel kernel)
        {
            new Resolvers.BasicNinjectModule().Load(kernel);
        }

    }
}
