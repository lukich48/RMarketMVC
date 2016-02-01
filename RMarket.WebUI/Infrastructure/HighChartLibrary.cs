using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure.HighChart
{

    public class SeriesOption
    {
        public string name;
        public List<SeriesData> data;
        public int? lineWidth;
        public string color;
        /// <summary>
        /// http://api.highcharts.com/highstock#series<line>.animation
        /// </summary>
        public object animation = false;


        public MarkerOption marker;

    }

    public class SeriesData
    {
        public object x;
        public object y;
        public DataLabel dataLabels;
    }

    public class MarkerOption
    {
        public bool? enabled = true;
        public int? radius;
    }

    public class DataLabel
    {
        public bool enabled = true;
        public string format;
    }
}