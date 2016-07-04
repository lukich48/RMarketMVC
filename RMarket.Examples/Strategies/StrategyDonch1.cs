using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Models;
using RMarket.Examples.Indicators;
using RMarket.ClassLib.Entities;

namespace RMarket.Examples.Strategies
{
    public class StrategyDonch1 : IStrategy
    {
        public Instrument Instr { get; set; }
        public IManager Manager { get; set; }
        public List<Order> Orders { get; set; }

        [DisplayChart(Name = "Donch1")]
        public DonchianChannel donch1;

        [DisplayChart(Name = "Donch2")]
        public DonchianChannel donch2;

        [DisplayChart(Name = "Donch3")]
        public DonchianChannel donch3;

        //[Parameter(Name = "period1", Description = "Период наименьшего канала")]
        public int period1 = 5;

        //[Parameter(Description = "Период среднего канала")]
        public int period2 = 15;

        //[Parameter(Description = "Период медленного канала")]
        public int period3 = 30;

        //[Parameter(Description = "Объем лота")]
        public int volume = 1;

        public void Initialize()
        {

            donch1 = new DonchianChannel(Instr, period1);
            donch2 = new DonchianChannel(Instr, period2);
            donch3 = new DonchianChannel(Instr, period3);

        }
        public void OnTickPoked(object sender, TickEventArgs e)
        { }

        public void Begin()
        {
            if (Instr.Candles.Count < period3 + 1)
                return;

            //Незакрытые ордера
            List<Order> ordersBuy = Orders.FindAll(ord => ord.OrderType == OrderType.Buy && ord.DateClose == DateTime.MinValue);
            List<Order> ordersSell = Orders.FindAll(ord => ord.OrderType == OrderType.Sell && ord.DateClose == DateTime.MinValue);

            //1. Три границы равны (сильный тренд). Покупаем на пробое.
            decimal donch1up = donch1.Results["up"].Values[1].Value; //пред бар
            decimal donch1down = donch1.Results["down"].Values[1].Value; //пред бар

            if (donch1up == donch2.Results["up"].Values[1].Value && donch1up == donch3.Results["up"].Values[1].Value && Instr.Candles[0].ClosePrice >= donch1up && ordersBuy.Count == 0)
            {
                //Берем buy
                Manager.OrderSender.OrderBuy(volume);
            }
            else if (donch1down == donch2.Results["down"].Values[1].Value && donch1down == donch3.Results["down"].Values[1].Value && Instr.Candles[0].ClosePrice <= donch1down && ordersSell.Count == 0)
            {
                //Берем sell 
                Manager.OrderSender.OrderSell(volume);
            }

            // Выход при пересечении границы быстрого канала
            if (Instr.Candles[0].ClosePrice < donch1down && ordersBuy.Count > 0)
            {
                Manager.OrderSender.OrderCloseAll(OrderType.Buy);
            }

            if (Instr.Candles[0].ClosePrice > donch1up && ordersSell.Count > 0)
            {
                Manager.OrderSender.OrderCloseAll(OrderType.Sell);
            }

        }

    }
}