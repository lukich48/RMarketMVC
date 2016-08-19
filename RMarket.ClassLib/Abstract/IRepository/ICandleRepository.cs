using System;
using System.Collections.Generic;
using RMarket.ClassLib.Entities;
using System.Linq;

namespace RMarket.ClassLib.Abstract.IRepository
{
    public interface ICandleRepository : IEntityRepository<Candle>
    {
        //void AddRange(IEnumerable<Candle> candles);
        //void RemoveRange(IEnumerable<Candle> candles);
    }
}