using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Infrastructure.AmbientContext
{
    public abstract class MyMapper
    {
        public static MyMapper Current { get; set; }

        public abstract TDestination Map<TSource, TDestination>(TSource source);
    }
}
