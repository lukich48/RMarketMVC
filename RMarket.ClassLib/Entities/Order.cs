using RMarket.ClassLib.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.ClassLib.Entities
{
    public enum OrderType
    {
        Buy=1,
        Sell=2
    }

    public class Order : IEntityData
    {
        public int Id { get; set; }
        public string TickerCode { get; set; }
        public OrderType OrderType { get; set; }
        
        /// <summary>
        /// 0 - без ограничений
        /// </summary>
        public decimal TakeProfit { get; set; }
        /// <summary>
        /// 0 - без ограничений
        /// </summary>
        public decimal StopLoss { get; set; }

        public DateTime DateOpen { get; set; }
        public DateTime DateOpenUTC { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public DateTime? DateClose { get; set; }
        public DateTime? DateCloseUTC { get; set; }

        public DateTime CreateDate { get; set; }

        public int Volume { get; set; }
        public decimal PriceOpen { get; set; }
        public decimal PriceClose { get; set; }
        public decimal Profit { get; set; }

        public string Comment { get; set; }

        public int AliveStrategyId { get; set; }

        public AliveStrategy AliveStrategy { get; set; }


    }
}