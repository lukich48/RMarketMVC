using RMarket.ClassLib.Entities;
using System;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;

namespace RMarket.DataAccess.Repositories
{
    public class EFTimeFrameRepository: EFRepositoryBase<TimeFrame>, ITimeFrameRepository
    {
        public EFTimeFrameRepository(RMarketContext context)
            :base(context)
        { }

    }
}
