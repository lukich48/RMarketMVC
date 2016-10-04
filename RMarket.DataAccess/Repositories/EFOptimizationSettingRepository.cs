using RMarket.ClassLib.Entities;
using System;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Models;

namespace RMarket.DataAccess.Repositories
{
    public class EFOptimizationSettingRepository: EFRepositoryBase<OptimizationSetting>, IOptimizationSettingRepository
    {
        public EFOptimizationSettingRepository(RMarketContext context)
            :base(context)
        { }

    }
}
