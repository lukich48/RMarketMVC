using RMarket.ClassLib.Entities;
using System;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;

namespace RMarket.DataAccess.Repositories
{
    public class EFEnrityInfoRepository: EFRepositoryBase<EntityInfo>, IEntityInfoRepository
    {
        public EFEnrityInfoRepository(RMarketContext context)
            :base(context)
        { }

    }
}
