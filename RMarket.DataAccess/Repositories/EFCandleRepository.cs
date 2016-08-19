using RMarket.ClassLib.Entities;
using System;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;

namespace RMarket.DataAccess.Repositories
{
    public class EFCandleRepository: EFRepositoryBase<Candle>, ICandleRepository
    {
        public EFCandleRepository(RMarketContext context)
            :base(context)
        { }
    }
}
