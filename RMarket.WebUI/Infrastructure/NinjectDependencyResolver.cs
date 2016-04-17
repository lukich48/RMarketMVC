using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Ninject;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.EFRepository;
using RMarket.ClassLib.Entities;

namespace RMarket.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IInstanceRepository>().To<EFInstanceRepository>();
            kernel.Bind<ITickerRepository>().To<EFTickerRepository>();
            kernel.Bind<ITimeFrameRepository>().To<EFTimeFrameRepository>();
            kernel.Bind<IStrategyInfoRepository>().To<EFStrategyInfoRepository>();
            kernel.Bind<ICandleRepository>().To<EFCandleRepository>();
            kernel.Bind<ISelectionRepository>().To<EFSelectionRepository>();
            kernel.Bind<IConnectorInfoRepository>().To<EFConnectorInfoRepository>();
            kernel.Bind<ISettingRepository>().To<EFSettingRepository>();
            kernel.Bind<IAliveStrategyRepository>().To<EFAliveStrategyRepository>();
            kernel.Bind<IOrderRepository>().To<EFOrderRepository>();

        }
    }
}