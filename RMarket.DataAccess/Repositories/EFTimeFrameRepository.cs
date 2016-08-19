using RMarket.ClassLib.Entities;
using System;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;

namespace RMarket.DataAccess.Repositories
{
    public class EFTickerRepository: EFRepositoryBase<Ticker>, ITickerRepository
    {
        public EFTickerRepository(RMarketContext context)
            :base(context)
        { }

    }
}
