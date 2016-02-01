using System.Collections.Generic;
using RMarket.ClassLib.Entities;
using System;
using System.Linq;

namespace RMarket.ClassLib.Abstract
{
    public interface ITickerRepository:IDisposable
    {
        IQueryable<Ticker> Tickers { get; }
        Ticker Find(int id);
        int Save(Ticker ticker);
        int Remove(int id);
    }
}
