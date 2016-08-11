using System;
using System.Collections.Generic;
using RMarket.ClassLib.Entities;
using System.Linq;

namespace RMarket.ClassLib.Abstract.IRepository
{
    public interface ICandleRepository : IDisposable
    {
        IQueryable<Candle> Candles { get; }
        Candle Find(int id);
        int Save(Candle candle);
        int Remove(int id);
        void AddRange(IEnumerable<Candle> candles);
        void RemoveRange(IEnumerable<Candle> candles);
    }
}