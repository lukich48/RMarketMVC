using LightInject;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Helpers.Extentions.IEnumerableExtension;
using RMarket.ClassLib.Services;
using RMarket.DataAccess.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.CompositionRoot.Resolvers
{
    /// <summary>
    /// Регистратор зависимостей
    /// </summary>
    public class DependencyRegister
    {
        public void RegisterDomainAssemblyDependencies(IServiceContainer container)
        {
            // Получаю всех имплементоров IDependency из всех загруженных сборок
            var listOfNativeDependencies = new ConcurrentBag<Type>();
            var listOfCustomDependencies = new ConcurrentBag<MethodInfo>();
            Parallel.ForEach(AppDomain.CurrentDomain.GetAssemblies(), ass =>
            {
                try
                {
                    ass.GetTypes()
                        .AsParallel()
                        .Where(t => typeof(IDependency).IsAssignableFrom(t))
                        .ForEach(t =>
                        {
                            var mi = t.GetMethod(nameof(DependencyRegister), BindingFlags.Public | BindingFlags.Static);
                            if (mi != null && mi.GetParameters().Length == 1)
                                listOfCustomDependencies.Add(mi);
                            else
                                listOfNativeDependencies.Add(t);
                        });
                }
                catch (Exception exception)
                {
                    // Не удалось загрузить одну из связанных сборок
                    EventLog.WriteEntry(this.GetType().Namespace, exception.ToString(),
                        EventLogEntryType.FailureAudit);
                }
            });
            // Регистрирую общие зависимости
            listOfNativeDependencies.ForEach(t => container.Register(t));
            // Регистрирую частные зависимости
            listOfCustomDependencies
                .ForEach(mi =>
                {
                    try
                    {
                        mi.Invoke(null, new object[] { container });
                    }
                    catch (Exception exception)
                    {
                            // Не удалось исполнить частную регистрацию типа
                            EventLog.WriteEntry(this.GetType().Namespace, exception.ToString(),
                            EventLogEntryType.FailureAudit);
                    }
                });

            //Регистрирую репозитории
            container.RegisterAssembly(typeof(EFInstanceRepository).Assembly, (serviceType, implementingType) =>
            typeof(IEntityRepository).IsAssignableFrom(serviceType));

            //Регистрирую сервисы
            container.RegisterAssembly(typeof(InstanceService).Assembly, (serviceType, implementingType) =>
            typeof(IEntityService).IsAssignableFrom(serviceType));

            //Регистрируем классы IDependency
            //container.RegisterAssembly(typeof(InstanceService).Assembly, (serviceType, implementingType) =>
            //typeof(IEntityService).IsAssignableFrom(serviceType));

            //сервис-локатор ))
            container.Register<IResolver, LightInjectResolver>();

            //container.Register<IOptimizationSettingRepository, EFOptimizationSettingRepository>();
            //container.Register<IOptimizationSettingService, OptimizationService>();


        }
    }
}
