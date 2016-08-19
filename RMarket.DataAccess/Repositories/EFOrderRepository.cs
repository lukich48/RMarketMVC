using RMarket.ClassLib.Entities;
using System;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;

namespace RMarket.DataAccess.Repositories
{
    public class EFOrderRepository : EFRepositoryBase<Order>, IOrderRepository
    {
        public EFOrderRepository(RMarketContext context)
            :base(context)
        { }

    }
}
