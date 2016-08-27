using RMarket.ClassLib.Entities;
using System;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;

namespace RMarket.DataAccess.Repositories
{
    public class EFConnectorInfoRepository : EFRepositoryBase<EntityInfo>, IConnectorInfoRepository
    {
        public EFConnectorInfoRepository(RMarketContext context)
            :base(context)
        { }

    }
}
