using Ninject.Modules;
using RMarket.DataAccess.Context;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Services;
using RMarket.DataAccess.Repositories;
using RMarket.ClassLib.Abstract.IRepository;
using Ninject.Web.Common;

namespace RMarket.CompositionRoot.Resolvers
{
    public class BasicNinjectModule:NinjectModule
    {
        public override void Load()
        {
            Bind<RMarketContext>().ToSelf().InRequestScope();

            Bind<IInstanceService>().To<InstanceService>();
            Bind<ISelectionService>().To<SelectionService>();
            Bind<ISettingService>().To<SettingService>();

            Bind<IInstanceRepository>().To<EFInstanceRepository>();
            Bind<ISelectionRepository>().To<EFSelectionRepository>();
            Bind<ITickerRepository>().To<EFTickerRepository>();
            Bind<ITimeFrameRepository>().To<EFTimeFrameRepository>();
            Bind<IStrategyInfoRepository>().To<EFStrategyInfoRepository>();
            Bind<ISettingRepository>().To<EFSettingRepository>();
            Bind<IConnectorInfoRepository>().To<EFConnectorInfoRepository>();
            Bind<ICandleRepository>().To<EFCandleRepository>();
            Bind<IAliveStrategyRepository>().To<EFAliveStrategyRepository>();
            Bind<IOrderRepository>().To<EFOrderRepository>();

            //this.Kernel.Bind(x => x
            //.From("RMarket.DataAccess", "RMarket.ClassLib")
            //.SelectAllClasses().InNamespaceOf<EFInstanceRepository>()
            //.BindDefaultInterface()
            //.Configure(b => b.InTransientScope())); 
        }
    }
}