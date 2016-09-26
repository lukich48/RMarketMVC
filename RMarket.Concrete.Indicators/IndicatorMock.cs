using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.Concrete.Indicators
{
    public class IndicatorMock : IndicatorBase
    {
        public IndicatorMock(Instrument instr, int period)
            : base(instr)
        {
            Results.Add("value1", new IndicatorResult(period));
            Results.Add("value2", new IndicatorResult(period));
        }

        protected override Dictionary<string, decimal> Begin()
        {
            var dict = new Dictionary<string, decimal>();
            dict.Add("value1", 0);
            dict.Add("value2", 0);

            return dict;
        }
    }
}
