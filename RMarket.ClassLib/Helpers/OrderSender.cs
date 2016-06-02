using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers
{
    public class OrderSender
    {
        IManager manager;

        public OrderSender(IManager manager)
        {
            this.manager = manager;
        }


        #region Отправка ордеров

        /// <summary>
        /// Рыночный ордер по текущей цене и текущему инструменту
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="loss"></param>
        /// <param name="profit"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        public Order OrderBuy(int volume, decimal stoploss = 0, decimal takeprofit = 0, DateTime expiration = new DateTime(), string comment = "")
        {
            Order res;

            res = manager.OrderSend(manager.Instr.Ticker.Code, OrderType.Buy, volume, stoploss, takeprofit, expiration, comment);

            return res;
        }

        public Order OrderSell(int volume, decimal stoploss = 0, decimal takeprofit = 0, DateTime expiration = new DateTime(), string comment = "")
        {

            Order res;

            res = manager.OrderSend(manager.Instr.Ticker.Code, OrderType.Sell, volume, stoploss, takeprofit, expiration, comment);

            return res;
        }
        #endregion


        #region Закрытие ордеров  

        public int OrderCloseAll(OrderType orderType)
        {
            List<Order> foundOrders = manager.Strategy.Orders.FindAll(ord => ord.OrderType == orderType && ord.DateClose == DateTime.MinValue);

            foreach (Order order in foundOrders)
            {
                manager.OrderClose(order);
            }

            return foundOrders.Count;
        }

        #endregion
    }
}
