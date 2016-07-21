using RMarket.WebUI.Infrastructure.HighChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Models
{
    public class ChartResult
    {
        public int Status { get; set; }
        public List<object[]> Candles { get; set; }
        public SeriesOption Orders { get; set; }
        public List<SeriesOption> Indicators { get; set; }
        public List<SeriesOption> ProfitList { get; set; }
    }

}