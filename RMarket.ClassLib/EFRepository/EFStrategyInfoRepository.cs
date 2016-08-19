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
    public class EFStrategyInfoRepository: IStrategyInfoRepository
    {
        private Entities_old.RMarketContext context = CurrentRepository.Context;

        public IQueryable<StrategyInfo> StrategyInfoes
        {
            get
            {
                return context.StrategyInfoes;
            }
        }

        public StrategyInfo Find(int id)
        {
            return context.StrategyInfoes.Find(id);
        }

        public int Save(StrategyInfo strategyInfo)
        {
            int res = 0;

            if(strategyInfo.Id==0)
            {
                context.StrategyInfoes.Add(strategyInfo);
            }
            else
            {
                context.Entry(strategyInfo).State = EntityState.Modified;
            }

            context.SaveChanges();

            return res;
        }

        public int Remove(int id)
        {
            int res = 0;

            StrategyInfo strategyInfo = context.StrategyInfoes.Find(id);
            context.StrategyInfoes.Remove(strategyInfo);
            context.SaveChanges();

            return res;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
