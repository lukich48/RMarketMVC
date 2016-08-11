using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract.IRepository
{
    public interface ITickRepository:IDisposable
    {
        IQueryable<Tick> Ticks { get; }
        void AddRange(IEnumerable<Tick> ticks);
        void RemoveRange(IEnumerable<Tick> ticks);
    }
}
