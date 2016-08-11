using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.EFRepository
{
    public class EFTickRepository:ITickRepository
    {
        private RMarketContext context = CurrentRepository.Context;

        public IQueryable<Tick> Ticks
        {
            get
            {
                return context.Ticks;//.Include(m => m.Ticker).Include(m => m.TimeFrame);
            }
        }

        public void AddRange(IEnumerable<Tick> ticks)
        {
            context.Ticks.AddRange(ticks);
            context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<Tick> ticks)
        {
            context.Ticks.RemoveRange(ticks);
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}
