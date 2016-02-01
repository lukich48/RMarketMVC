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
            ind1 = new DonchianChannel(Instr, period1);
            ind2 = new DonchianChannel(Instr, period2);
            ind3 = new DonchianChannel(Instr, period3);
        }

        [DisplayChart(Name = "Индикатор1")]
        public DonchianChannel ind1;
        [DisplayChart]
        public DonchianChannel ind2;
        public DonchianChannel ind3;

        [Parameter(Name = "period1", Description = "Период наименьшего канала")]
        public int period1 = 5;

        [Parameter(Description = "Период среднего канала")]
        public int period2 = 15;

        [Parameter]
        public int period3 = 30;

        [Parameter(Description = "Объем лота")]
        public int volume = 1;

        [Parameter(Name = "decimal param", Description = "Какой-то параметр типа Decimal")]
        public decimal decimalParam = 30.25m;

        [Parameter(Name = "Включает задержку")]
        public bool withSleep = false;

        public void Begin()
        {
            if (withSleep)
                Thread.Sleep(100);

            //покупаем и продаем через каждые 50 свечек
            if(Instr.Candles.Count % 50 == 0)
            {
                Manager.OrderCloseAll(OrderType.Buy);
                Manager.OrderCloseAll(OrderType.Sell);

                if((Instr.Candles.Count / 50) % 2 == 0)
                {
                    Manager.OrderBuy(1);
                }
                else
                {
                    Manager.OrderSell(1);
                }

            }
        }

        public void OnTickPoked(object sender, TickEventArgs e)
        {

        }

    }

}
