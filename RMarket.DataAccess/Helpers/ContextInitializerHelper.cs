using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using RMarket.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace RMarket.DataAccess.Helpers
{
    public class ContextInitializerHelper
    {
        private readonly RMarketContext context;
        public ContextInitializerHelper(RMarketContext context)
        {
            this.context = context;
        }


        public void SeedTickers()
        {
            List<Ticker> listTicker = new List<Ticker>
            {
                new Ticker {Id=1, Name="SBER", Code="SBER", QtyInLot=10},
                new Ticker {Id=2,  Name="GAZP", Code="GAZP"},
                new Ticker {Id=3, Name="AVAZ", Code="AVAZ"},
            };
            context.Tickers.AddRange(listTicker);
            context.SaveChanges();

        }

        public void SeedTimeFrames()
        {
            List<TimeFrame> listTimeFrame = new List<TimeFrame>
            {
                new TimeFrame {Id=1,Name="tick",ToMinute=0 },
                new TimeFrame {Id=2,Name="1",ToMinute=1 },
                new TimeFrame {Id=3,Name="2",ToMinute=2 },
                new TimeFrame {Id=4,Name="10",ToMinute=10 },
                new TimeFrame {Id=5,Name="15",ToMinute=15 },
                new TimeFrame {Id=6,Name="30",ToMinute=30 },
                new TimeFrame {Id=7,Name="60",ToMinute=60 },
                new TimeFrame {Id=8,Name="day",ToMinute=1440 },
            };
            context.TimeFrames.AddRange(listTimeFrame);
            context.SaveChanges();
        }

    }
}
