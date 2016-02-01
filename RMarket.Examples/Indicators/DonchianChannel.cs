using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.Examples.Indicators
{
    public class DonchianChannel : IndicatorBase
    {
        private List<decimal> up;
        private List<decimal> down;

        public DonchianChannel(Instrument instr, int period)
            : base(instr)
        {
            Results.Add("up", new IndicatorResult(period));
            Results.Add("down", new IndicatorResult(period));

            up = new List<decimal>(period);
            down = new List<decimal>(period);

        }

        protected override Dictionary<string, decimal> Begin()
        {

            if (up.Count < Results["up"].Period)
            {
                //Первоначальное заполнение
                up.Insert(0, Instr.Candles[0].HighPrice);
            }
            else
            {
                up.RemoveAt(Results["up"].Period - 1);
                up.Insert(0, Instr.Candles[0].HighPrice);
            }

            if (down.Count < Results["up"].Period)
            {
                //Первоначальное заполнение
                down.Insert(0, Instr.Candles[0].LowPrice);
            }
            else
            {
                down.RemoveAt(Results["down"].Period - 1);
                down.Insert(0, Instr.Candles[0].LowPrice);
            }

            var dict = new Dictionary<string, decimal>();
            dict.Add("up", up.Max());
            dict.Add("down", down.Min());

            return dict;
        }



    }
}
