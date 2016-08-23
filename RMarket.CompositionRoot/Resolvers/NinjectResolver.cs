using Ninject;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.CompositionRoot.Resolvers
{
    public class NinjectResolver: Resolver
    {
        private IKernel kernel;

        public NinjectResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override object Resolve(Type type)
        {
            return kernel.TryGet(type);
        }
    }
}
