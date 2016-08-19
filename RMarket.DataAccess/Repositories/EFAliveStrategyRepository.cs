using System;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;
using System.Data.Entity;

namespace RMarket.DataAccess.Repositories
{
    public class EFAliveStrategyRepository : EFRepositoryBase<AliveStrategy>, IAliveStrategyRepository
    {
        public EFAliveStrategyRepository(RMarketContext context)
            :base(context)
        { }

        public override void Save(AliveStrategy data)
        {
            if (data.Id == 0)
            {
                data.CreateDate = DateTime.Now;
                context.AliveStrategies.Add(data);
                
            }
            else
            {
                context.Entry(data).State = EntityState.Modified;
            }

            context.SaveChanges();

        }

    }
}
