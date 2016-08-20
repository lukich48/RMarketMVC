using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Infrastructure;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Managers
{
    public class EmulManager : IManager
    {
        private AliveStrategy aliveStrategy;

        private readonly IOrderRepository orderRepository;

        public EmulManager(IOrderRepository orderRepository,
            IStrategy strategy, Instrument instr, Portfolio portf, IDataProvider connector, AliveStrategy aliveStrategy)
        {
            this.orderRepository = orderRepository;

            this.Strategy = strategy;
            this.Instr = instr;
            this.Portf = portf;
            this.Connector = connector;
            this.AliveType = AliveType.Emul;
            this.OrderSender = new OrderSender(this);

            Strategy.Instr = Instr;
            Strategy.Manager = this;

            Strategy.Initialize();
            StrategyHelper.SubscriptionToEventAsync(Strategy); //Подписываем на событие формирования свечи асинхронно индикаторы, которые найдем в стратегии.

            this.aliveStrategy = aliveStrategy;

            //!!!проверить на наличие подписки
            Connector.TickPoked += Instr.OnTickPoked;
        }

        #region IManager

        public IStrategy Strategy { get; set; }
        public Instrument Instr { get; set; }
        public Portfolio Portf { get; set; }
        public IDataProvider Connector { get; set; }
        public bool IsStarted { get; set; }

        /// <summary>
        /// Пока не используется
        /// </summary>
        public DateTime DateFrom { get; set; }
        /// <summary>
        /// Пока не используется
        /// </summary>
        public DateTime DateTo { get; set; }
        public AliveType AliveType { get; set; }
        public OrderSender OrderSender { get; set; }

        #region Events

        public event EventHandler StrategyStopped;

        #endregion


        public void StartStrategy()
        {

            Instr.TickPoked += Strategy.OnTickPoked;
            Instr.CreatedCandleReal += (sender, e) =>
            {
                Strategy.Begin();
            };

            Connector.StartServer();
            
            IsStarted = true;

        }
        public void StopStrategy()
        {
            try
            {
                Instr.TickPoked -= Strategy.OnTickPoked;
                Instr.CreatedCandleReal -= (sender, e) =>
                {
                    Strategy.Begin();
                }; //!!!Проверить

            }
            finally
            {
                IsStarted = false;

                if (StrategyStopped != null)
                    StrategyStopped(this, null);

            }

        }
       
        public Order OrderSend(string tickerCode, OrderType orderType, int volume, decimal stoploss = 0, decimal takeprofit = 0, DateTime? expirationDate = null, string comment = "")
        {

            Order order = new Order();
            order.TickerCode = tickerCode;
            order.OrderType = orderType;
            order.Volume = volume;
            order.StopLoss = stoploss;
            order.TakeProfit = takeprofit;
            order.ExpirationDate = expirationDate;
            order.DateOpen = Instr.Candles[0].DateOpen;
            order.DateOpenUTC = DateTime.UtcNow;
            order.PriceOpen = Instr.Candles[0].ClosePrice + Portf.Slippage * ((order.OrderType == OrderType.Sell) ? (-1) : 1);

            //TODO:Потокобезопасно  
            decimal oldBalance = Portf.Balance;
            Portf.Balance = Math.Round(Portf.Balance - order.PriceOpen * order.Volume * (100 - Portf.Rent) / 100, 2);
            if (Portf.Balance < 0)
            {
                Portf.Balance = oldBalance;
                return null;
            }

            Strategy.Orders.Insert(0, order);

            order.AliveStrategyId = aliveStrategy.Id;
            orderRepository.Save(order);

            return order;
        }

        public int OrderClose(Order order)
        {
            int res = 0;

            order.DateClose = Instr.Candles[0].DateOpen;
            order.DateCloseUTC = DateTime.UtcNow;

            order.PriceClose = Instr.Candles[0].ClosePrice;
            order.Profit = order.Volume * (order.PriceClose - order.PriceOpen) * ((order.OrderType == OrderType.Sell) ? (-1) : 1);

            //TODO:Потокобезопасно
            Portf.Balance = Portf.Balance + order.PriceClose * order.Volume;

            orderRepository.Save(order);

            return res;
        }
        #endregion 

        //////////////////////////////////////////Управление состоянием



    }
}
