using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using System.Data.Entity;
using RMarket.ClassLib.Infrastructure;
using RMarket.ClassLib.Abstract.IRepository;

namespace RMarket.ClassLib.EFRepository
{
    public class EFAliveStrategyRepository: IAliveStrategyRepository
    {
        private RMarketContext context = CurrentRepository.Context;

        public IQueryable<AliveStrategy> AliveStrategies
        {
            get
            {
                return context.AliveStrategies;
            }
        }

        public AliveStrategy Find(int id)
        {
            return context.AliveStrategies.Find(id);
        }

        public int Save(AliveStrategy aliveStrategy)
        {
            int res = 0;

            if (aliveStrategy.Id == 0)
            {
                aliveStrategy.CreateDate = DateTime.Now;
                context.AliveStrategies.Add(aliveStrategy);
                context.SaveChanges();
            }
            else
            {
                context.Entry(aliveStrategy).State = EntityState.Modified;
                context.SaveChanges();
            }

            return res;
        }

        public void Dispose()
        {
            context.Dispose();
        }


    }
}
