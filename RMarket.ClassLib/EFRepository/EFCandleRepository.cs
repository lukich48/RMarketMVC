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
    public class EFCandleRepository : ICandleRepository
    {
        private Entities_old.RMarketContext context = CurrentRepository.Context;

        public IQueryable<Candle> Candles
        {
            get
            {
                return context.Candles;
            }
        }

        public Candle Find(int id)
        {
            return context.Candles.Find(id);
        }

        public int Save(Candle candle)
        {
            int res = 0;

            if (candle.Id == 0)
            {
                context.Candles.Add(candle);
                context.SaveChanges();
            }
            else
            {
                context.Entry(candle).State = EntityState.Modified;
                context.SaveChanges();
            }

            return res;
        }

        public int Remove(int id)
        {
            int res = 0;

            Candle candle = context.Candles.Find(id);
            context.Candles.Remove(candle);
            context.SaveChanges();

            return res;
        }

        public void AddRange(IEnumerable<Candle> candles)
        {
            context.Candles.AddRange(candles);
            context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<Candle> candles)
        {
            context.Candles.RemoveRange(candles);
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
