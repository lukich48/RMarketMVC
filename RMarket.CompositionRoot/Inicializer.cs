using AutoMapper;
using AutoMapper.Configuration;
using LightInject;
using RMarker.Concrete.DataProviders.Infrastructure;
using RMarker.Concrete.Optimization.Infrastructure;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers.Extentions.IEnumerableExtension;
using RMarket.CompositionRoot.Mapper;
using RMarket.CompositionRoot.Resolvers;
using RMarket.Concrete.HistoricalProviders.Infrastructure;
using RMarket.Concrete.Strategies.Infrastructure;
using RMarket.DataAccess.Context;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        public void SetMapperConfiguration()
        {
            //Обойдем сборку и зарегистрируем все профили
            var profiles = new ConcurrentBag<Profile>();
            Parallel.ForEach(AppDomain.CurrentDomain.GetAssemblies(), ass =>
            {
                try
                {
                    ass.GetTypes()
                        .AsParallel()
                        .Where(t => typeof(Profile).IsAssignableFrom(t))
                        .ForEach(t =>
                        {
                            var prof = (Profile)Activator.CreateInstance(t);
                            profiles.Add(prof);
                        });
                }
                catch (Exception)
                {
                    // Не удалось загрузить одну из связанных сборок
                    //EventLog.WriteEntry(this.GetType().Namespace, exception.ToString(),
                    //    EventLogEntryType.FailureAudit);
                }
            });

            var customMapper = new CustomMapper(profiles);
            RMarket.ClassLib.Infrastructure.AmbientContext.MyMapper.Current = customMapper;
        }

        public void InitIoC(Action<IServiceContainer> doConfig)
        {
            // Инициализация IoC
            var container = new ServiceContainer
            {
                ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider() // Все зависимости в Scope-режиме
            };

            // Регистрирую все возможные IDependency в домене
            new DependencyRegister().RegisterDomainAssemblyDependencies(container);

            // Применяю частные настройки соединений к БД и режим работы IoC
            doConfig(container);

        }

    }
}
