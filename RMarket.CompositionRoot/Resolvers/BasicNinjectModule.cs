using System;
using System.Collections.Generic;
using Ninject;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.EFRepository;
using Ninject.Modules;
using Ninject.Extensions.Conventions;
using RMarket.DataAccess.Context;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Services;
using RMarket.DataAccess.Repositories;
using RMarket.ClassLib.Abstract.IRepository;

namespace RMarket.CompositionRoot.Resolvers
{
    public class BasicNinjectModule:NinjectModule
    {
        public override void Load()
        {
            RMarketContext context = new RMarketContext();

            Bind<IInstanceService>().To<InstanceService>();
            Bind<ISelectionService>().To<SelectionService>();

            Bind<IInstanceRepository>().To<EFInstanceRepository>().WithConstructorArgument(context);
            Bind<ISelectionRepository>().To<EFSelectionRepository>().WithConstructorArgument(context);

            Bind<ITickerRepository>().To<EFTickerRepository>();
            Bind<ITimeFrameRepository>().To<EFTimeFrameRepository>();
            Bind<IStrategyInfoRepository>().To<EFStrategyInfoRepository>();
            Bind<ICandleRepository>().To<EFCandleRepository>();
            //Bind<ISelectionRepository>().To<EFSelectionRepository>();
            Bind<IConnectorInfoRepository>().To<EFConnectorInfoRepository>();
            Bind<ISettingRepository>().To<EFSettingRepository>();
            Bind<IAliveStrategyRepository>().To<EFAliveStrategyRepository>();
            Bind<IOrderRepository>().To<EFOrderRepository>();

            //this.Kernel.Bind(x => x
            //.From("RMarket.DataAccess", "RMarket.ClassLib")
            //.SelectAllClasses().InNamespaceOf<EFCandleRepository>()
            //.BindDefaultInterface()
            //.Configure(b => b.InTransientScope())); //!!! сделать один на запрос
        }
    }
}