using System.Collections.Generic;
using RMarket.ClassLib.Entities;
using System;
using System.Linq;

namespace RMarket.ClassLib.Abstract
{
    public interface IStrategyInfoRepository: IDisposable
    {
        IQueryable<StrategyInfo> StrategyInfoes { get; }
        StrategyInfo Find(int id);
        int Save(StrategyInfo strategyInfo);
        int Remove(int id);
    }
}
