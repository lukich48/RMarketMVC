using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Infrastructure;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers
{
    public static class TickHelper
    {
        public static ITickRepository tickRepository = CurrentRepository.TickRepository;

        private static ConcurrentDictionary<string, byte> usedTickerCode = new ConcurrentDictionary<string, byte>();
        private static ConcurrentBag<Tick> ticksBuffer = new ConcurrentBag<Tick>();
        private static int ticksBufferCount = 100; //!!!вынести в константы

        public static void InsertNewTick(TickEventArgs tick)
        {
            //!!! Сделать быстрое удаление и сохранение тиков

            if (!usedTickerCode.ContainsKey(tick.TickerCode))
            {
                usedTickerCode.AddOrUpdate(tick.TickerCode, 1, (k, v) => 1);
              
                //удалить все последующие тики
                IEnumerable<Tick> ticksDel = tickRepository.Ticks.Where(t => t.TickerCode==tick.TickerCode && t.Date >= tick.Date);
                tickRepository.RemoveRange(ticksDel);
                
            }
            else
            {
                //записать тик
                if (ticksBuffer.Count >= ticksBufferCount)
                {
                    //new TaskFactory().StartNew(()=>tickRepository.AddRange(ticksBuffer));
                    var newBag = new ConcurrentBag<Tick>();
                    Interlocked.Exchange<ConcurrentBag<Tick>>(ref ticksBuffer, newBag);
                }

                Tick newTick = new Tick
                {
                    Date = tick.Date,
                    Price = tick.Price,
                    Quantity = tick.Quantity,
                    TickerCode = tick.TickerCode,
                    TradePeriod = tick.TradePeriod,
                    Extended = tick.Extended.Select(e => new Tick.KeyValue { Key = e.Key, Value = e.Value })
                };

                ticksBuffer.Add(newTick);
            }
        }

    }
}
