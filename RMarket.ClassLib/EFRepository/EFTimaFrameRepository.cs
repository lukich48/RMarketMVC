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
    public class EFTimeFrameRepository : ITimeFrameRepository
    {
        private RMarketContext context = CurrentRepository.Context;

        public IQueryable<TimeFrame> TimeFrames
        {
            get
            {
                return context.TimeFrames;
            }
        }

        public TimeFrame Find(int id)
        {
            return context.TimeFrames.Find(id);
        }

        public int Save(TimeFrame timeFrame)
        {
            int res = 0;

            if (timeFrame.Id == 0)
            {
                context.TimeFrames.Add(timeFrame);
                context.SaveChanges();
            }
            else
            {
                context.Entry(timeFrame).State = EntityState.Modified;
                context.SaveChanges();
            }

            return res;
        }

        public int Remove(int id)
        {
            int res = 0;

            TimeFrame timeFrame = context.TimeFrames.Find(id);
            context.TimeFrames.Remove(timeFrame);
            context.SaveChanges();

            return res;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
