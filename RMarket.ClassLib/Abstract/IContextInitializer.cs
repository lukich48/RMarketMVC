using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    public interface IContextInitializer<TEntity>
    {
        IEnumerable<TEntity> Get();
    }
}
