using Ninject;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.CompositionRoot.Resolvers
{
    public class NinjectResolver: IResolver
    {
        private IKernel kernel;

        public NinjectResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T Resolve<T>(Type type)
        {
            return (T)kernel.TryGet(type);
        }
    }
}
