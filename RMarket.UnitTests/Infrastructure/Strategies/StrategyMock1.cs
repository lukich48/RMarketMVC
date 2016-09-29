using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.UnitTests.Infrastructure.Strategies
{
    public class StrategyMock1: IStrategy
    {
        public Instrument Instr { get; set; }
        public IManager Manager { get; set; }
        public List<Order> Orders { get; set; }

        public void Initialize()
        {
        }

        public void Begin()
        {
            //покупаем и продаем через каждые 20 свечек
            if (Instr.Candles.Count % 20 == 0)
            {
                Manager.OrderSender.OrderCloseAll(OrderType.Buy);
                Manager.OrderSender.OrderCloseAll(OrderType.Sell);

                if ((Instr.Candles.Count / 20) % 2 != 0)
                {
                    Manager.OrderSender.OrderBuy(1);
                }
                else
                {
                    Manager.OrderSender.OrderSell(1);
                }

            }
        }

        public void OnTickPoked(object sender, TickEventArgs e)
        {

        }

    }
}
