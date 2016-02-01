namespace RMarket.ClassLib.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Candle
    {
        public int Id { get; set; }

        public int TickerId { get; set; }

        public int TimeFrameId { get; set; }

        public DateTime DateOpen { get; set; }

        public decimal OpenPrice { get; set; }

        public decimal HighPrice { get; set; }

        public decimal LowPrice { get; set; }

        public decimal ClosePrice { get; set; }

        public int Volume { get; set; }

        public DateTime CreateDate { get; set; }


        public virtual Ticker Ticker { get; set; }

        public virtual TimeFrame TimeFrame { get; set; }

        public Candle()
        {
            CreateDate = DateTime.Now;
        }

        /// <summary>
        /// Для Инсерта
        /// </summary>
        public Candle(int TickerId, int TimeFrameId, DateTime DateOpen, decimal OpenPrice, decimal HighPrice, decimal LowPrice, decimal ClosePrice, int Volume)
        {
            this.TickerId = TickerId;
            this.DateOpen = DateOpen;
            this.TimeFrameId = TimeFrameId;
            this.OpenPrice = OpenPrice;
            this.HighPrice = HighPrice;
            this.LowPrice = LowPrice;
            this.ClosePrice = ClosePrice;
            this.Volume = Volume;

            CreateDate = DateTime.Now;
        }
    }
}
