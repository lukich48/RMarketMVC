using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Models;
using System.Threading;
using RMarket.Examples.Indicators;
using RMarket.ClassLib.Entities;

namespace RMarket.Examples.Strategies
{
    public class StrategyMock : IStrategy
    {
        public Instrument Instr { get; set; }
        public IManager Manager { get; set; }
        public List<Order> Orders { get; set; }

        public void Initialize()
        {
            ind1 = new DonchianChannel(Instr, Period1);
            ind2 = new DonchianChannel(Instr, Period2);
            ind3 = new DonchianChannel(Instr, 15);
        }

        [DisplayChart(Name = "Индикатор1")]
        public DonchianChannel ind1;
        [DisplayChart]
        public DonchianChannel ind2;
        public DonchianChannel ind3;

        [Parameter(Name = "period1", Description = "Период наименьшего канала")]
        public int Period1 { get; set; }

        [Parameter(Description = "Период среднего канала")]
        public int Period2 { get; set; }

        [Parameter(Description = "Объем лота")]
        public int Volume { get; set; }

        [Parameter(Name = "decimal param", Description = "Какой-то параметр типа Decimal")]
        public decimal DecimalParam { get; set; }

        [Parameter(Name = "Включает задержку")]
        public bool WithSleep { get; set; }

        public StrategyMock()
        {
            Period1 = 5;
            Period2 = 15;
            Volume = 1;
            DecimalParam = 30.25m;
            WithSleep = false;

        }

        public void Begin()
        {
            if (WithSleep)
                Thread.Sleep(100);

            //покупаем и продаем через каждые 50 свечек
            if(Instr.Candles.Count % 50 == 0)
            {
                Manager.OrderSender.OrderCloseAll(OrderType.Buy);
                Manager.OrderSender.OrderCloseAll(OrderType.Sell);

                if((Instr.Candles.Count / 50) % 2 == 0)
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
