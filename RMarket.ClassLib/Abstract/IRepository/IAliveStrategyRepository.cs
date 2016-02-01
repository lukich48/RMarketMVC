using System.Collections.Generic;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Linq;

namespace RMarket.ClassLib.Abstract
{
    public interface IAliveStrategyRepository:IDisposable
    {
        IQueryable<AliveStrategy> AliveStrategies { get; }
        AliveStrategy Find(int id);
        int Save(AliveStrategy aliveStrategy);
    }
}
