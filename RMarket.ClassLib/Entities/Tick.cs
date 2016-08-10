using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Entities
{
    public class Tick : IEntityData
    {
        public class KeyValue
        {
            public int Id { get; set; }
            public int TickId { get; set; }
            public string Key { get; set; }
            public string Value { get; set; }

            public virtual Tick Tick { get; set; }
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string TickerCode { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public TradePeriod TradePeriod { get; set; } 

        public virtual IEnumerable<KeyValue> Extended { get; set; }

    }


}
