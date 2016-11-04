using AutoMapper;
using AutoMapper.Configuration;
using LightInject;
using RMarker.Concrete.DataProviders.Infrastructure;
using RMarker.Concrete.Optimization.Infrastructure;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.CompositionRoot.Mapper;
using RMarket.CompositionRoot.Resolvers;
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

        public void InitIoC(Action<IServiceContainer> doConfig)
        {
            // Инициализация IoC
            var container = new ServiceContainer
            {
                ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider() // Все зависимости в Scope-режиме
            };
            // Регистрирую текущий контейнер сам в себе
            container.Register<IServiceContainer>(factory => container, new PerContainerLifetime());
            // Все остальные зависимости в Scope-режиме
            container.SetDefaultLifetime<PerScopeLifetime>();

            // Регистрирую все возможные IDependency в домене
            new DependencyRegister().RegisterDomainAssemblyDependencies(container);

            // Применяю частные настройки соединений к БД и режим работы IoC
            doConfig(container);

        }

    }
}
