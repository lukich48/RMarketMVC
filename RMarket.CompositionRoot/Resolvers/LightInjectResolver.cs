using LightInject;
using RMarket.ClassLib.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.CompositionRoot.Resolvers
{
    class LightInjectResolver: IResolver
    {
        private IServiceContainer container;

        public LightInjectResolver(IServiceContainer container)
        {
            this.container = container;
        }

        public T Resolve<T>(Type type)
        {
            return (T)container.GetInstance(type);
        }
    }
}
