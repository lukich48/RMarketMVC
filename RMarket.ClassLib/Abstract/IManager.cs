using System;
using System.Collections.Generic;
using RMarket.ClassLib.Models;
using RMarket.ClassLib.Entities;

namespace RMarket.ClassLib.Abstract
{
    public interface IManager
    {
        IStrategy Strategy { get; set; }
        Instrument Instr { get; set; }
        Portfolio Portf { get; set; }
        bool IsStarted { get; set; }
        DateTime DateFrom { get; set; }
        DateTime DateTo { get; set; }
        bool IsReal { get; set; }

        void StartStrategy();
        void StopStrategy();

        #region Отправка ордеров

        //!!!переработать. оставить один метод - остальное вынести в хелперы
        Order OrderBuy(int volume, decimal stoploss = 0, decimal takeprofit = 0, DateTime expiration = new DateTime(), string comment = "");
        Order OrderSell(int volume, decimal stoploss = 0, decimal takeprofit = 0, DateTime expiration = new DateTime(), string comment = "");
        Order OrderSend(string tickerCode, OrderType orderType, int volume, decimal stoploss = 0, decimal takeprofit = 0, DateTime expiration = new DateTime(), string comment = "");

        #endregion

        #region Закрытие ордеров

        int OrderCloseAll(OrderType orderType);
        int OrderClose(Order order);

        #endregion
    }

}
