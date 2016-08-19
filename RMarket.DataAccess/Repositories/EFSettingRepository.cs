using RMarket.ClassLib.Entities;
using System;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;

namespace RMarket.DataAccess.Repositories
{
    public class EFSettingRepository: EFRepositoryBase<Setting>, ISettingRepository
    {
        public EFSettingRepository(RMarketContext context)
            :base(context)
        { }
    }
}
