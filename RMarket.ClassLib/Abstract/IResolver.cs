using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    /// <summary>
    /// интерфейс для IoC
    /// </summary>
    public interface IResolver
    {
        T Resolve<T>(Type type);
    }
}
