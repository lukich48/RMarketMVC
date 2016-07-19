using System;
using System.Collections.Generic;
using RMarket.ClassLib.Models;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;

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
        AliveType AliveType { get; set; }
        OrderSender OrderSender { get; set; }

        #region Events

        event EventHandler StrategyStopped;

        #endregion


        void StartStrategy();
        void StopStrategy();

        Order OrderSend(string tickerCode, OrderType orderType, int volume, decimal stoploss = 0, decimal takeprofit = 0, DateTime? expirationdate = null, string comment = "");
        int OrderClose(Order order);
    }

}
