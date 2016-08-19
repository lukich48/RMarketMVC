using RMarket.ClassLib.Entities;
using System;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;

namespace RMarket.DataAccess.Repositories
{
    public class EFStrategyInfoRepository: EFRepositoryBase<StrategyInfo>, IStrategyInfoRepository
    {
        public EFStrategyInfoRepository(RMarketContext context)
            :base(context)
        { }

    }
}
