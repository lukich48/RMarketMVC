using Ninject.Modules;
using RMarket.DataAccess.Context;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Services;
using RMarket.DataAccess.Repositories;
using RMarket.ClassLib.Abstract.IRepository;
using Ninject.Web.Common;
using Ninject;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using RMarket.ClassLib.Abstract;

namespace RMarket.CompositionRoot.Resolvers
{
    public class BasicNinjectModule
    {
        public void Load(IKernel kernel)
        {
            kernel.Bind<RMarketContext>().ToSelf().InRequestScope();

            kernel.Bind<IInstanceService>().To<InstanceService>();
            kernel.Bind<ISelectionService>().To<SelectionService>();
            kernel.Bind<IDataProviderSettingService>().To<DataProviderService>();
            kernel.Bind<IHistoricalProviderSettingService>().To<HistoricalProviderService>();
            kernel.Bind<IOptimizationSettingService>().To<OptimizationService>();

            kernel.Bind<IInstanceRepository>().To<EFInstanceRepository>();
            kernel.Bind<ISelectionRepository>().To<EFSelectionRepository>();
            kernel.Bind<ITickerRepository>().To<EFTickerRepository>();
            kernel.Bind<ITimeFrameRepository>().To<EFTimeFrameRepository>();
            kernel.Bind<IEntityInfoRepository>().To<EFEnrityInfoRepository>();
            kernel.Bind<IDataProviderSettingRepository>().To<EFDataProviderSettingRepository>();
            kernel.Bind<IHistoricalProviderSettingRepository>().To<EFHistoricalProviderSettingRepository>();
            kernel.Bind<IOptimizationSettingRepository>().To<EFOptimizationSettingRepository>();
            kernel.Bind<ICandleRepository>().To<EFCandleRepository>();
            kernel.Bind<IAliveStrategyRepository>().To<EFAliveStrategyRepository>();
            kernel.Bind<IOrderRepository>().To<EFOrderRepository>();

            //Наш сервис-локатор ))
            kernel.Bind<IResolver>().To<NinjectResolver>().InSingletonScope();
            //Resolver.Current = new NinjectResolver(kernel);

        }
    }
}