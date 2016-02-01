using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using System.Data.Entity;

namespace RMarket.ClassLib.EFRepository
{
    public class EFTickerRepository:ITickerRepository
    {
        private RMarketContext context = RMarketContext.Current;

        public IQueryable<Ticker> Tickers
        {
            get
            {
                return context.Tickers;
            }
        }

        public Ticker Find(int id)
        {
            return context.Tickers.Find(id);
        }

        public int Save(Ticker ticker)
        {
            int res = 0;

            if (ticker.Id == 0)
            {
                context.Tickers.Add(ticker);
                context.SaveChanges();
            }
            else
            {
                context.Entry(ticker).State = EntityState.Modified;
                context.SaveChanges();
            }

            return res;
        }

        public int Remove(int id)
        {
            int res = 0;

            Ticker ticker = context.Tickers.Find(id);
            context.Tickers.Remove(ticker);
            context.SaveChanges();

            return res;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
